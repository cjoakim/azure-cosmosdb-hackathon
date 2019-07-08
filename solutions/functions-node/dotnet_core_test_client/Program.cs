using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

using Microsoft.Azure.EventHubs;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

// DotNet Core Text Client Console App for the Azure Functions
// It is used to send messages to Azure Event Hubs, and query
// the resulting documents in Azure CosmosDB.
// Chris Joakim, Microsoft, 2019/07/08

namespace dotnet_core_test_client
{
    class Program
    {
        private static EventHubClient eventHubClient = null;
        private static DocumentClient cosmosClient   = null;
        private static string eventHubName    = Environment.GetEnvironmentVariable("AZURE_EVENTHUB_HUBNAME");
        private static string eventHubConnStr = Environment.GetEnvironmentVariable("AZURE_EVENTHUB_CONN_STRING");
        private static string cosmosUri       = Environment.GetEnvironmentVariable("AZURE_COSMOSDB_SQLDB_URI");
        private static string cosmosKey       = Environment.GetEnvironmentVariable("AZURE_COSMOSDB_SQLDB_KEY");
        private static string cosmosDatabaseName   = "dev";    // Environment.GetEnvironmentVariable("AZURE_COSMOSDB_SQLDB_DBNAME");
        private static string cosmosCollectionName = "events"; // Environment.GetEnvironmentVariable("AZURE_COSMOSDB_SQLDB_COLLNAME");
        private static AirportData airportData = new AirportData();
        private static string sql = null;
        private static double ruCharge = 0;

        static void Main(string[] args) {
            if (args.Length < 1) {
                Log("Invalid program args.");
                DisplayCommandLineExamples();
            }
            else {
                var func = args[0];
                int messageCount = 0;
                switch (func) {
                    case "send_event_hub_messsages":
                        messageCount = Int32.Parse(args[1]);
                        SendEventHubMessages(messageCount);
                        break;
                    case "query_cosmos":
                        QueryCosmos(args);
                        break;
                    case "insert_cosmos_documents":
                        messageCount = Int32.Parse(args[1]);
                        InsertCosmosDocuments(messageCount);
                        break;
                    case "time_now":
                        var now = DateTime.Now;
                        var epoch = new DateTimeOffset(now).ToUnixTimeSeconds();
                        Console.WriteLine($"date: {now}  epoch: {epoch}");
                        break;
                    default:
                        Log("Unknown CLI function: " + func);
                        DisplayCommandLineExamples();
                        break;
                }
            }
        }

        private static void DisplayCommandLineExamples() {
            Log("Command-Line Examples:");
            Log("  dotnet run send_event_hub_messsages 10");
            Log("  dotnet run query_cosmos doc_by_pk_id SYD 047f35b4-7a09-4312-afe1-c44d171606ca");
            Log("  dotnet run query_cosmos all_events");
            Log("  dotnet run query_cosmos events_for_airport SYD");
            Log("  dotnet run query_cosmos events_for_city Sydney");
            Log("  dotnet run query_cosmos events_for_location -80.842842 35.499586 1"); // 35.499586, -80.842842
            Log("  dotnet run insert_cosmos_documents 10");
            Log("  dotnet run time_now");
            Log("");
        }

        // === EventHub methods below

        private static void SendEventHubMessages(int messageCount) {
            Log("SendEventHubMessages: " + messageCount);
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(eventHubConnStr) {
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

        private static async Task SendMessageAsync(JObject airport) {
            try {
                var message = airport.ToString(Formatting.None);
                Console.WriteLine($"Sending message: {message}");
                await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
            }
            catch (Exception exception) {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }
        }

        // === CosmosDB methods below

        private static void QueryCosmos(string[] args) {
            string queryName = args[1];
            Log("QueryCosmos: " + queryName + ", " + cosmosDatabaseName + ", " + cosmosCollectionName);
            cosmosClient = new DocumentClient(new Uri(cosmosUri), cosmosKey);
            string iataCode = null;
            string city = null;
            string pk  = null;
            string id  = null;
            double lat = 0.0;
            double lng = 0.0;
            double km  = 0.0;
  
            switch (queryName) {
                case "all_events":
                    DisplayDocuments(QueryAllEvents());
                    DisplayChargeAndSql();  
                    break;
                case "doc_by_pk_id":
                    pk = args[2];
                    id = args[3];
                    ResourceResponse<Document> result = QueryByPkId(pk, id);
                    Console.WriteLine(result.Resource);
                    Console.WriteLine("Charge: " + result.RequestCharge);
                    break;
                case "events_for_airport":
                    iataCode = args[2];
                    DisplayDocuments(QueryEventsForAirport(iataCode));
                    DisplayChargeAndSql();  
                    break;
                case "events_for_city":
                    city = args[2];
                    DisplayDocuments(QueryEventsForCity(city));
                    DisplayChargeAndSql();  
                    break;
                case "events_for_location":
                    lng = Double.Parse(args[2]);
                    lat = Double.Parse(args[3]);
                    km  = Double.Parse(args[4]);
                    DisplayDocuments(QueryEventsForLocation(lng, lat, km));
                    DisplayChargeAndSql();  
                    break;
                default:
                    Log("Unknown queryName: " + queryName);
                    DisplayCommandLineExamples();
                    break;
            }
        }

        private static void DisplayDocuments(List<Object> documents) {
            foreach (Object doc in documents) {
                Console.WriteLine("{0}", doc);
            }
        }

        private static void DisplayChargeAndSql() {
            Log("RU Charge: " + ruCharge + ",  SQL: " + sql);
        }

        private static List<Object> QueryAllEvents() {
            List<object> objects = new List<object>();
            sql = $"SELECT * FROM c";
            return DocumentsQuery(sql, true).Result;        
        }

        private static List<Object> QueryEventsForAirport(string iataCode) {
            Log("QueryEventsForAirport: " + iataCode);
            List<Object> objects = new List<Object>();
            sql = $"SELECT * FROM c WHERE c.pk = '{iataCode}'";
            return DocumentsQuery(sql, false).Result;                 
        }

        private static List<Object> QueryEventsForCity(string city) {
            Log("QueryEventsForCity: " + city);
            List<Object> objects = new List<Object>();
            sql = $"SELECT * FROM c WHERE c.city = '{city}'";
            return DocumentsQuery(sql, true).Result;                 
        }

        private static List<Object> QueryEventsForLocation(double lng, double lat, double km) {
            Log("QueryEventsForLocation: " + lat + ", " + lng + ", " + km);
            List<Object> objects = new List<Object>();
            double meters = km * 1000;
            sql = $"SELECT * from c WHERE ST_DISTANCE(c.location, {{'type': 'Point', 'coordinates':[ {lng}, {lat} ]}}) < {meters}";
            return DocumentsQuery(sql, true).Result;  
        }

        private static async Task<List<object>> DocumentsQuery(string sql, bool enableCrossPartition) {
            List<object> objects = new List<object>();
            double queryCharge = 0.0;
            try {
                Log("DocumentsQuery SQL: " + sql);
                // .AsDocumentQuery() enables the collection of the RequestCharge
                var query = cosmosClient.CreateDocumentQuery<Object>(
                    CollectionUri(), sql, StandardFeedOptions(enableCrossPartition)).AsDocumentQuery();

                while (query.HasMoreResults) {
                    var response = await query.ExecuteNextAsync<Object>();
	                objects.AddRange(response);  // Add the next few Documents from the while-iteration response
                    queryCharge = queryCharge + response.RequestCharge;  // accumulate the query RequestCharge
                }
            }
            catch (Exception exception) {
                Console.WriteLine($"DocumentsQuery {DateTime.Now} > Exception: {exception.Message}");
            }
            finally {
                Log("total queryCharge: " + queryCharge);
                ruCharge = queryCharge;
            }
            return objects;
        }

        // This is the most efficient and least-cost query - by Partition-Key and Document Id

        private static ResourceResponse<Document> QueryByPkId(string pk, string id) {
            return cosmosClient.ReadDocumentAsync(DocumentUri(id), RequestOptionsWithPk(pk)).Result; 
        }

        private static void InsertCosmosDocuments(int messageCount) {
            cosmosClient = new DocumentClient(new Uri(cosmosUri), cosmosKey);
            Random random = new Random();

            for (int i = 0; i < messageCount; i++) {
                var index = random.Next(0, airportData.airports.Count);
                JObject airport = (JObject) airportData.airports[index];
                airportData.AddRandomFlight(airport);
                Console.WriteLine(airport);
                Task task = UpsertDoc(airport);
                task.Wait();
            }
        }

        private static async Task UpsertDoc(JObject airport) {
            try {
                ResourceResponse<Document> response = 
                    await cosmosClient.UpsertDocumentAsync(CollectionUri(), airport);
                var doc = response.Resource;
                Log("Document: " + doc.ToString());
                Log("ActivityId:    " + response.ActivityId);
                Log("StatusCode:    " + response.StatusCode);
                Log("RequestCharge: " + response.RequestCharge);
            }
            catch (Exception exception) {
                Console.WriteLine($"UpsertDoc {DateTime.Now} > Exception: {exception.Message}");
            }
        }

        private static FeedOptions StandardFeedOptions(bool enableCrossPartition) {
            return new FeedOptions {
                MaxItemCount = -1, 
                EnableCrossPartitionQuery = enableCrossPartition
            };
        }

        private static FeedOptions FeedOptionsWithPk(string pk) {
            return new FeedOptions {
                MaxItemCount = -1, 
                EnableCrossPartitionQuery = false,
                PartitionKey = new PartitionKey(pk)
            };
        }

        private static RequestOptions RequestOptionsWithPk(string pk) {
            return new RequestOptions {
                PartitionKey = new PartitionKey(pk)
            };
        }

        private static Uri CollectionUri() {
            return UriFactory.CreateDocumentCollectionUri(cosmosDatabaseName, cosmosCollectionName);
        }

        private static Uri DocumentUri(string documentId) {
            return UriFactory.CreateDocumentUri(cosmosDatabaseName, cosmosCollectionName, documentId);
        }

        private static long CurrentEpochTime() {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        }

        private static void Log(string msg) {
            Console.WriteLine(msg);
        }
    }
}
