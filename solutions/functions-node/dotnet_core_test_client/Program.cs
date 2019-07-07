using System;
using System.Collections.Generic;
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
        private static AirportData airportData = new AirportData();

        static void Main(string[] args)
        {
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
                        QueryCosmos(args);
                        break;
                    case "insert_cosmosdb_documents":
                        InsertCosmosDocuments(args);
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

            Random random = new Random();

            for (int i = 0; i < messageCount; i++) {
                var index = random.Next(0, airportData.airports.Count);
                JObject airport = (JObject) airportData.airports[index];
                airportData.AddRandomFlight(airport);
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

        // ===

        private static void QueryCosmos(string[] args)
        {
            string queryName = args[1];
            Log("QueryCosmos: " + queryName + ", " + cosmosDbName + ", " + cosmosCollName);
            cosmosDbClient = new DocumentClient(new Uri(cosmosUri), cosmosKey);

            switch (queryName)
            {
                case "airport_events":
                    string iataCode = args[2];
                    List<Object> events = QueryAirportEvents(iataCode);
                    foreach (Object obj in events)
                    {
                        Console.WriteLine("{0}", obj);
                    }
                    break;
                default:
                    Log("Unknown queryName: " + queryName);
                    DisplayCommandLineExamples();
                    break;
            }
        }

        private static List<Object> QueryAirportEvents(string iataCode)
        {
            Log("QueryAirportEvents: " + iataCode);
            List<Object> objects = new List<Object>();
            string sql = $"SELECT * FROM functions WHERE functions.pk = '{iataCode}'";
            Log("SQL: " + sql);
            IQueryable<Object> query = cosmosDbClient.CreateDocumentQuery<Object>(
                CollectionUri(), sql, DefaultFeedOptions());

            foreach (Object obj in query)
            {
                objects.Add(obj);
                //Console.WriteLine("{0}", obj);
            }
            return objects;            
        }

        private static void InsertCosmosDocuments(string[] args)
        {
            Random random = new Random();

            // for (int i = 0; i < messageCount; i++) {
            //     var index = random.Next(0, airports.Count);
            //     JObject airport = (JObject) airports[index];
            //     RandomFlight(airport);
            //     Console.WriteLine(airport);
            // }
        }


        private static FeedOptions DefaultFeedOptions()
        {
            return new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true};
        }

        private static Uri CollectionUri()
        {
            return UriFactory.CreateDocumentCollectionUri(cosmosDbName, cosmosCollName);
        }

        private static long CurrentEpochTime()
        {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        }

        private static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
