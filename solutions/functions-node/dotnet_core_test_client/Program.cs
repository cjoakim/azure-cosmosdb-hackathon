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
// Chris Joakim, Microsoft, 2019/07/12

namespace dotnet_core_test_client {

    class EventDoc {
        public string id { get; set; }
        public string pk { get; set; }
        public string doctype { get; set; }
        public double epoch { get; set; }
    }

    class Program {
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
        private static double queryCharge = 0;
 
        static void Main(string[] args) {
            
            Console.WriteLine(eventHubName);
            Console.WriteLine(eventHubConnStr);
            
            for (int i = 0; i < args.Length; i++) {
                Console.WriteLine("arg: " + i + " -> "+ args[i]);
            }
            if (args.Length < 1) {
                Log("Invalid program args.");
                DisplayCommandLineExamples();
            }
            else {
                var func = args[0];
                int messageCount = 0;
                switch (func) {
                    case "time_now":
                        var now = DateTime.Now;
                        var epoch = new DateTimeOffset(now).ToUnixTimeSeconds();
                        Console.WriteLine($"date: {now}  epoch: {epoch}");
                        break;
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
                    default:
                        Log("Unknown CLI function: " + func);
                        DisplayCommandLineExamples();
                        break;
                }
            }
        }

        private static void DisplayCommandLineExamples() {
            Log("Command-Line Format:");
            Log("  dotnet run time_now");
            Log("  dotnet run send_event_hub_messsages 10");
            Log("  dotnet run query_cosmos doc_by_pk_id <pk> <id>");
            Log("  dotnet run query_cosmos all_events <optional-after-epoch>");
            Log("  dotnet run query_cosmos events_for_airport <pk> <optional-after-epoch>");
            Log("  dotnet run query_cosmos events_for_city <city> <optional-after-epoch>");
            Log("  dotnet run query_cosmos delete_documents <max-count> <optional-after-epoch>");
            Log("  dotnet run query_cosmos count_documents");
            Log("  dotnet run query_cosmos events_for_location -80.842842 35.499586 1 <optional-after-epoch>");
            Log("  dotnet run insert_cosmos_documents 10");
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

            for (int seq = 1; seq <= messageCount; seq++) {
                var index = random.Next(0, airportData.airports.Count);
                JObject airport = (JObject) airportData.airports[index];
                airportData.AddRandomEvent(airport, seq);
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
            double fromEpoch = 0;
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
                // cosmosClient.ReadDocumentAsync
                case "doc_by_pk_id":
                    pk = args[2];
                    id = args[3];
                    ResourceResponse<Document> result = QueryByPkId(pk, id);
                    Console.WriteLine(result.Resource);
                    Console.WriteLine("Charge: " + result.RequestCharge);
                    break;

                // cosmosClient.CreateDocumentQuery....AsDocumentQuery() below
                case "all_events":
                    fromEpoch = EpochArg(args, 2);
                    DisplayDocuments(QueryAllEvents(fromEpoch));
                    DisplayQueryMetadata();  
                    break;
                case "events_for_airport":
                    iataCode = args[2];
                    fromEpoch = EpochArg(args, 3);
                    DisplayDocuments(QueryEventsForAirport(iataCode, fromEpoch));
                    DisplayQueryMetadata();  
                    break;
                case "events_for_city":
                    city = args[2];
                    DisplayDocuments(QueryEventsForCity(city, fromEpoch));
                    DisplayQueryMetadata();  
                    break;
                case "events_for_location":
                    lng = Double.Parse(args[2]);
                    lat = Double.Parse(args[3]);
                    km  = Double.Parse(args[4]);
                    fromEpoch = EpochArg(args, 5);
                    DisplayDocuments(QueryEventsForLocation(lng, lat, km, fromEpoch));
                    DisplayQueryMetadata();  
                    break;
                case "delete_documents":
                    int maxCount = Int32.Parse(args[2]);
                    fromEpoch = EpochArg(args, 3);
                    DeleteDocuments(maxCount, fromEpoch);
                    break; 
                case "count_documents":
                    DisplayDocuments(CountDocuments());
                    DisplayQueryMetadata();  
                    break; 
                default:
                    Log("Unknown queryName: " + queryName);
                    DisplayCommandLineExamples();
                    break;
            }
        }

        private static double EpochArg(string[] args, int idx) {
            if (idx == (args.Length - 1)) {
                return Double.Parse(args[idx]);
            }
            else {
                return 0;
            }
        }

        private static void DisplayDocuments(List<Object> documents) {
            foreach (Object doc in documents) {
                Console.WriteLine("{0}", doc);
            }
            Console.WriteLine("Document count: " + documents.Count);
        }

        private static void DisplayQueryMetadata() {
            Log("Query Charge: " + queryCharge + ",  SQL: " + sql);
        }

        private static List<Object> QueryAllEvents(double fromEpoch) {
            sql = $"SELECT * FROM c WHERE c.epoch > {fromEpoch}";
            return DocumentsQuery(sql, true).Result;        
        }

        private static List<Object> QueryEventsForAirport(string iataCode, double fromEpoch) {
            sql = $"SELECT * FROM c WHERE c.pk = '{iataCode}' AND c.epoch > {fromEpoch}";
            return DocumentsQuery(sql, false).Result;                 
        }

        private static List<Object> QueryEventsForCity(string city, double fromEpoch) {
            sql = $"SELECT * FROM c WHERE c.city = '{city}' AND c.epoch > {fromEpoch}";
            return DocumentsQuery(sql, true).Result;                 
        }

        private static List<Object> QueryEventsForLocation(double lng, double lat, double km, double fromEpoch) {
            double meters = km * 1000;
            sql = $"SELECT * from c WHERE ST_DISTANCE(c.location, {{'type': 'Point', 'coordinates':[ {lng}, {lat} ]}}) < {meters} AND c.epoch > {fromEpoch}";
            return DocumentsQuery(sql, true).Result;  
        }

        private static List<Object> CountDocuments() {
            sql = "SELECT VALUE COUNT(1) FROM c";
            return DocumentsQuery(sql, true).Result;  
        }

        private static async Task<List<object>> DocumentsQuery(string sql, bool enableCrossPartition) {
            List<object> objects = new List<object>();
            double charge = 0.0;
            try {
                Log("DocumentsQuery SQL: " + sql);
                // .AsDocumentQuery() enables the collection of the RequestCharge
                var query = cosmosClient.CreateDocumentQuery<object>(
                    CollectionUri(), sql, StandardFeedOptions(enableCrossPartition)).AsDocumentQuery();

                while (query.HasMoreResults) {
                    var response = await query.ExecuteNextAsync<object>();
	                objects.AddRange(response);  // Add the next few Documents from the while-iteration response
                    charge = charge + response.RequestCharge;  // accumulate the query RequestCharge
                }
            }
            catch (DocumentClientException dce) {
                Exception baseException = dce.GetBaseException();
                Console.WriteLine("DocumentsQuery Exception; {0}, {1}, {2}", 
                    dce.StatusCode, dce.Message, baseException.Message);
            }
            catch (Exception excp) {
                Console.WriteLine($"DocumentsQuery Exception: {excp.Message}");
            }
            finally {
                queryCharge = charge;
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

            for (int seq = 0; seq < messageCount; seq++) {
                var index = random.Next(0, airportData.airports.Count);
                JObject airport = (JObject) airportData.airports[index];
                airportData.AddRandomEvent(airport, seq);
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

        private static void DeleteDocuments(int maxDeleteCount, double fromEpoch) {
            Console.WriteLine("DeleteDocuments; max: " + maxDeleteCount + ", fromEpoch: " + fromEpoch);

            // this is an example of Linq queries, select the oldest docs with an epoch > fromEpoch
            IQueryable<EventDoc> eventQuery = 
                cosmosClient.CreateDocumentQuery<EventDoc>(
                    CollectionUri(), StandardFeedOptions(true))
                    .Where(ed => ed.epoch >= fromEpoch)
                    .OrderBy(ed => ed.epoch);

            int count = 0;
            Console.WriteLine("Running LINQ query...");
            foreach (EventDoc evtDoc in eventQuery) {
                if (count < maxDeleteCount) {
                    count++;
                    Console.WriteLine("Deleting EventDoc, pk: {0} id: {1} epoch: {2}", evtDoc.pk, evtDoc.id, evtDoc.epoch);
                    Task task = DeleteDoc(evtDoc);
                    task.Wait();
                }
            }
        }

        private static async Task DeleteDoc(EventDoc evtDoc) {
            ResourceResponse<Document> response =
                await cosmosClient.DeleteDocumentAsync(DocumentUri(evtDoc.id),
                    new RequestOptions { PartitionKey = new PartitionKey(evtDoc.pk) });

            Console.WriteLine("Delete RU Charge: {0}", response.RequestCharge);
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
