﻿using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.EventHubs;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// DotNet Core Text Client Console App for the Azure Functions
// It is used to send messages to Azure Event Hubs, and query
// the resulting documents in Azure CosmosDB.
// Chris Joakim, Microsoft, 2019/07/06

namespace dotnet_core_test_client
{
    class Program
    {
        private static EventHubClient eventHubClient = null;
        private static DocumentClient cosmosDbClient = null;
        private static string eventHubName     = Environment.GetEnvironmentVariable("AZURE_EVENTHUB_HUBNAME");
        private static string eventHubConnStr  = Environment.GetEnvironmentVariable("AZURE_EVENTHUB_CONN_STRING");
        private static string cosmosUri        = Environment.GetEnvironmentVariable("AZURE_COSMOSDB_SQLDB_URI");
        private static string cosmosKey        = Environment.GetEnvironmentVariable("AZURE_COSMOSDB_SQLDB_KEY");
        private static string cosmosDbName     = "dev";    // Environment.GetEnvironmentVariable("AZURE_COSMOSDB_SQLDB_DBNAME");
        private static string cosmosCollName   = "events"; // Environment.GetEnvironmentVariable("AZURE_COSMOSDB_SQLDB_COLLNAME");

        static void Main(string[] args)
        {
            Console.WriteLine(args);

            if (args.Length < 1) {
                Log("Invalid program args.");
                DisplayCommandLineExamples();
            }
            else {
                var func = args[0];
                switch (func)
                {
                    case "send_event_hub_messsages":
                        int messageCount = Int32.Parse(args[1]);
                        SendEventHubMessages(messageCount);
                        break;
                    case "query_cosmosdb":
                        string queryName = args[1];
                        QueryCosmosDB(queryName, args);
                        break;
                    default:
                        Log("Unknown CLI function: " + func);
                        DisplayCommandLineExamples();
                        break;
                }
            }
        }

        private static void DisplayCommandLineExamples()
        {
            Log("Command-Line Examples:");
            Log("  dotnet run send_event_hub_messsages 10");
            Log("  dotnet run query_cosmosdb airport_events ATL");
            Log("");
        }

        private static void SendEventHubMessages(int messageCount)
        {
            Log("SendEventHubMessages: " + messageCount);
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(eventHubConnStr)
            {
                EntityPath = eventHubName
            };
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            JArray airports = ReadAirportData();
            Random random = new Random();

            for (int i = 0; i < messageCount; i++) {
                var index = random.Next(0, airports.Count);
                JObject airport = (JObject) airports[index];
                RandomFlight(airport);
                Console.WriteLine(airport);
                Task task = SendMessageAsync(airport);
                task.Wait();
            }
        }

        private static async Task SendMessageAsync(JObject airport)
        {
            try
            {
                // Log("airport Type: " + airport.GetType());  // Newtonsoft.Json.Linq.JObject
                var message = airport.ToString(Formatting.None);
                Console.WriteLine($"Sending message: {message}");
                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }
        }

        private static JArray ReadAirportData()
        {
            JArray array = new JArray();
            try {
                string infile = @"data/world_airports_50.json";
                string jsonString = System.IO.File.ReadAllText(infile);
                //Log(jsonString);
                JArray airports = JArray.Parse(jsonString);

                foreach (JObject airport in airports.Children<JObject>())
                {
                    array.Add(airport);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);  // System.Exception
            }
            Log("" + array.Count + " airports read");
            return array;
        }

        private static void RandomFlight(JObject airport) 
        {  
            // airport is a Newtonsoft.Json.Linq.JObject
            Random random = new Random();
            string airline = "AA";
            string eventName = "Depart";
            string flightNumber = "" + random.Next(100, 5000);
            int n1 = random.Next(0,5);
            int n2 = random.Next(0,2);
            //Log("" + n1 + "," + n2);

            switch (n1)
            {
                case 0:
                    airline = "AA";
                    break;
                case 1:
                    airline = "DL";
                    break;
                case 2:
                    airline = "UA";
                    break;
                case 3:
                    airline = "LH";
                    break;
                case 4:
                    airline = "AF";
                    break;
                default:
                    airline = "AA";
                    break;
            }

            switch (n2)
            {
                case 0:
                    eventName = "Depart";
                    break;
                default:
                    eventName = "Arrive";
                    break;
            }

            if (airport["airline"] != null)
            {
                Log("already present");
            }
            else {
                airport.Add("pk", "" + airport["iata_code"]);
                airport.Add("epoch", CurrentEpochTime());
                airport.Add("airline", airline);
                airport.Add("flightNumber", flightNumber);
                airport.Add("eventName", eventName);
            }
        }

        private static void QueryCosmosDB(string queryName, string[] args)
        {
            Log("QueryCosmosDB: " + queryName + "" + cosmosDbName + "" + cosmosCollName);
            cosmosDbClient = new DocumentClient(new Uri(cosmosUri), cosmosKey);

            switch (queryName)
            {
                case "airport_events":
                    string iataCode = args[2];
                    QueryAirportEvents(iataCode);
                    break;
                default:
                    Log("Unknown queryName: " + queryName);
                    DisplayCommandLineExamples();
                    break;
            }
        }

        private static void QueryAirportEvents(string iataCode)
        {
            Log("QueryAirportEvents: " + iataCode);

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true};
            string sql = $"SELECT * FROM functions WHERE functions.pk = '{iataCode}'";
            Log(sql);
            IQueryable<Object> query = cosmosDbClient.CreateDocumentQuery<Object>(
                UriFactory.CreateDocumentCollectionUri(cosmosDbName, cosmosCollName),
                sql, queryOptions);

            Console.WriteLine("Running direct SQL query...");
            foreach (Object obj in query)
            {
                Console.WriteLine("{0}", obj);
            }
        }

        private static long CurrentEpochTime()
        {
            DateTime now = DateTime.Now;
            DateTimeOffset dto = new DateTimeOffset(now);
            return dto.ToUnixTimeSeconds();
        }

        private static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
