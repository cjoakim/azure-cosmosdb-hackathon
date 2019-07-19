using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace solution3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello from Solution3!");
            // reletive path - feel free to move it...
            ReadAndLoadFile(@"..\..\..\..\..\data\mongoexport_airports.json");
            CountDocuments();
            AirportsByIATACode();
            Console.WriteLine("Bye bye from Solution3!");
        }

        public static void ReadAndLoadFile(string filename)
        {
            List<Airport> list = new List<Airport>();
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            string line = string.Empty;
            Uri uri = getUri();
            DocumentClient client = getClient();

            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
                try
                {
                    dynamic obj = JsonConvert.DeserializeObject(line);
                    Airport airport = new Airport
                    {
                        name = obj.name,
                        iata_code = obj.iata_code,
                        city = obj.city,
                        country = obj.country,
                        latitude = obj.latitude,
                        altitude = obj.altitude,
                        timezone_num = obj.timezone_num,
                        timezone_code = obj.timezone_code,
                        location = new Location((double)obj.latitude, (double)obj.longitude)
                    };
                    
                    Task<ResourceResponse<Document>> doc = client.CreateDocumentAsync(uri, airport);
                    doc.Wait();
                    Console.WriteLine("RequestCharge: {0}", doc.Result.RequestCharge); // // x-ms-request-charge
                }
                catch (Exception e)
                {
                    Exception baseException = e.GetBaseException();
                    Console.WriteLine("Error in CreateAirportDocument: {0}, Message: {1}", e.Message, baseException.Message);
                }

            }
        }

        public static void CountDocuments()
        {
            Uri uri = getUri();
            DocumentClient client = getClient();

            string sql = "SELECT VALUE COUNT(1) FROM c";
            Console.WriteLine("sql: {0}", sql);

            // This approach casts the result into a generic Object:
            FeedOptions queryOptions =
                new FeedOptions
                {
                    MaxItemCount = -1,
                    EnableCrossPartitionQuery = true,
                    PopulateQueryMetrics = true
                };
            IQueryable<Object> results =
                client.CreateDocumentQuery<Object>(uri, sql, queryOptions);
            foreach (Object result in results)
            {
                Console.WriteLine("Count {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
            }

        }

        public static void AirportsByIATACode()
        {
            Uri uri = getUri();
            DocumentClient client = getClient();

            string sql = $"SELECT * FROM c WHERE c.iata_code = 'ATL'";
            Console.WriteLine("sql: {0}", sql);

            // This approach casts the result into a generic Object:
            FeedOptions queryOptions =
                new FeedOptions
                {
                    MaxItemCount = -1,
                    EnableCrossPartitionQuery = false,
                    PopulateQueryMetrics = true
                };
            IQueryable<Object> airports =
                client.CreateDocumentQuery<Object>(uri, sql, queryOptions);
            foreach (Object airport in airports)
            {
                Console.WriteLine("Document {0}", JsonConvert.SerializeObject(airport, Formatting.Indented));
            }

        }

        private static Uri getUri()
        {
            return UriFactory.CreateDocumentCollectionUri("hackathon", "airports");
        }

        private static DocumentClient getClient()
        {
            string url = Environment.GetEnvironmentVariable("AZURE_COSMOSDB_SQLDB_URI");
            string key = Environment.GetEnvironmentVariable("AZURE_COSMOSDB_SQLDB_KEY");

            DocumentClient client = CosmosDBClient.GetInstance(url, key).GetClient();
            return client;

        }
    }
}
