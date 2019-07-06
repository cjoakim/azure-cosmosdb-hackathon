using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.EventHubs;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


// DotNet Core Text Client App for the Azure Functions
// Chris Joakim, Microsoft, 2019/07/06

namespace dotnet_core_test_client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(args);

            if (args.Length < 1) {
                Console.WriteLine("Invalid program args.");
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
                        QueryCosmosDB(queryName);
                        break;
                    default:
                        Console.WriteLine("Unknown CLI function: " + func);
                        DisplayCommandLineExamples();
                        break;
                }
            }
        }

        private static void SendEventHubMessages(int messageCount)
        {
            Console.WriteLine("SendEventHubMessages: " + messageCount);
            ReadAirportData();


            // TODO

        }

        private static void QueryCosmosDB(string queryName)
        {
            Console.WriteLine("QueryCosmosDB: " + queryName);
            // TODO
        }

        private static void DisplayCommandLineExamples()
        {
            Console.WriteLine("Command-Line Examples:");
            Console.WriteLine("  dotnet run send_event_hub_messsagess 10");
            Console.WriteLine("  dotnet run query_cosmosdb tbd");
            Console.WriteLine("");
        }

        private static void ReadAirportData() {
            try {
                string infile = @"data/world_airports_50.json";
                string jsonString = System.IO.File.ReadAllText(infile);
                Console.WriteLine(jsonString);
                JArray airports = JArray.Parse(jsonString);
                Console.WriteLine(airports);

                foreach (JObject airport in airports.Children<JObject>())
                {
                    // Console.WriteLine(airport.GetType());
                    // airport is an instance of Newtonsoft.Json.Linq.JObject
                    Console.WriteLine("---");
                    string[] flight = RandomFlight();
                    airport.Add("epoch", CurrentEpochTime());
                    airport.Add("airline", flight[0]);
                    airport.Add("flightNumber", flight[1]);
                    airport.Add("eventName", flight[2]);
                    Console.WriteLine(airport);
                }

            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        private static long CurrentEpochTime() {
            DateTime now = DateTime.Now;
            DateTimeOffset dto = new DateTimeOffset(now);
            return dto.ToUnixTimeSeconds();
        }

        private static string[] RandomFlight() {
            Random random = new Random();
            string airline = "AA";
            string evt     = "Depart";
            string number  = "" + random.Next(100, 5000);
            int n1 = random.Next(0,5);
            int n2 = random.Next(0,2);
            Console.WriteLine("" + n1 + "," + n2);

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
                    evt = "Depart";
                    break;
                default:
                    evt = "Arrive";
                    break;
            }

            //return airline + " " + number + " " + evt;
            return new [] { airline, number, evt };
        }

        private static void RandomFlightLoop() {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(RandomFlight());
            }
        }
    }
}
