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
    class AirportData
    {

        public JArray airports;

        public AirportData()
        {
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


        public void AddRandomFlight(JObject airport) 
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

        private static long CurrentEpochTime()
        {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        }

        public void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
