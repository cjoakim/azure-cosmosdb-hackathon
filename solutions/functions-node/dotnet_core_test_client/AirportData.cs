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

namespace dotnet_core_test_client {

    class AirportData {

        public JArray airports;

        public AirportData() {
            this.airports = new JArray();
            try {
                string infile = @"data/world_airports_50.json";
                string jsonString = System.IO.File.ReadAllText(infile);
                //Log(jsonString);
                JArray objects = JArray.Parse(jsonString);
                foreach (JObject obj in objects.Children<JObject>())
                {
                    this.airports.Add(obj);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);  // System.Exception
            }
            Log("AirportData " + this.airports.Count + " airports read");
        }

        public void AddRandomEvent(JObject airport, int seq) { 
            removeAddedAttributes(airport);
            Random random = new Random();
            int n = random.Next(0,100);
            if (n < 80) {
                AddRandomFlight(airport, seq);
            }
            else {
                AddRandomWeather(airport, seq);
            }
        }

        public void AddRandomFlight(JObject airport, int seq) {  
            // airport is a Newtonsoft.Json.Linq.JObject
            Random random = new Random();
            string airline = "AA";
            string flightState = "Depart";
            string flightNumber = "" + random.Next(100, 5000);
            int n1 = random.Next(0,5);
            int n2 = random.Next(0,2);

            switch (n1) {
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

            switch (n2) {
                case 0:
                    flightState = "Depart";
                    break;
                default:
                    flightState = "Arrive";
                    break;
            }

            airport.Add("pk", "" + airport["iata_code"]);
            airport.Add("doctype", "flight");
            airport.Add("epoch", CurrentEpochTime());
            airport.Add("seq", seq);
            airport.Add("airline", airline);
            airport.Add("flightNumber", flightNumber);
            airport.Add("flightState", flightState);
        }

        public void AddRandomWeather(JObject airport, int seq) {  
            // airport is a Newtonsoft.Json.Linq.JObject
            Random random = new Random();
            int n1 = random.Next(0,3);
            int n2 = random.Next(-10, 40);
            double n3 = random.Next(29000, 31000) / 1000.0;
            string skies = "sunny";

            switch (n1) {
                case 0:
                    skies = "sunny";
                    break;
                case 1:
                    skies = "fair";
                    break;
                case 2:
                    skies = "cloudy";
                    break;
                default:
                    skies = "fair";
                    break;
            }

            airport.Add("pk", "" + airport["iata_code"]);
            airport.Add("doctype", "weather");
            airport.Add("epoch", CurrentEpochTime());
            airport.Add("seq", seq);
            airport.Add("skies", skies);
            airport.Add("temperature", n2);
            airport.Add("barometric_pressure", n3);
        }

        private static void removeAddedAttributes(JObject airport) {
            airport.Remove("pk");
            airport.Remove("doctype");
            airport.Remove("epoch");
            airport.Remove("seq");

            airport.Remove("airline");
            airport.Remove("flightNumber");
            airport.Remove("flightState");

            airport.Remove("skies");
            airport.Remove("temperature");
            airport.Remove("barometric_pressure");
        }

        private static long CurrentEpochTime() {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        }

        public void Log(string msg) {
            Console.WriteLine(msg);
        }
    }
}
