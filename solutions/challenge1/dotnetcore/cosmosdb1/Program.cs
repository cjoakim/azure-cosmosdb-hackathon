namespace Hackathon
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;

    using CsvHelper;
    using Newtonsoft.Json;

    // Main .Net Core program for the Hackathon
    // Chris Joakim, Microsoft, 2019/04/12

    // https://github.com/Azure/azure-cosmos-dotnet-v2/blob/master/samples/partition-stats/Program.cs

    //[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "For illustration purposes.")]
    public class Program
    {
        // Instance variables:
        private string EndpointUrl = null;
        private string PrimaryKey = null;

        private string DbName = null;

        private string CollName = null;

        private string[] CountriesList = new string[3] {"United States", "United Kingdom", "Japan"};
        private List<Airport> Airports = new List<Airport>();
        private DocumentClient Client = null;

        public Program()
        {
            this.EndpointUrl = ReadEnvVar("AZURE_COSMOSDB_SQLDB_URI");
            this.PrimaryKey  = ReadEnvVar("AZURE_COSMOSDB_SQLDB_KEY");
        }

        private void DisplayValues()
        {
            Console.WriteLine("CosmosDB EndpointUrl: {0}", EndpointUrl);
            Console.WriteLine("CosmosDB PrimaryKey:  {0}", PrimaryKey);
            Console.WriteLine("CosmosDB DbName:      {0}", DbName);
            Console.WriteLine("CosmosDB CollName:    {0}", CollName);
            Console.WriteLine("Airports CSV count:   {0}", Airports.Count());
            Console.WriteLine("PWD and PathSepChar   {0}  {1}",
                Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar);
        }

        private void ReadAirportsCsv()
        {
            this.Airports = new FileUtil().ReadAirportsCsv(CountriesList); 

        }

        private void InitializeClient()
        {
            this.Client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey); 
            Console.WriteLine("InitializeClient: {0}", this.Client);
        }





        private Document CreateAirportDocumentSync(Airport airport)
        {
            Uri uri = UriFactory.CreateDocumentCollectionUri(this.DbName, this.CollName);
            //Console.WriteLine("uri: {0}", uri);
            try
            {
                Task<ResourceResponse<Document>> doc = this.Client.CreateDocumentAsync(uri, airport);
                doc.Wait();
                return doc.Result;
            }
            catch (Exception e) {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error in CreateAirportDocument: {0}, Message: {1}", e.Message, baseException.Message);
                return null;
            }
        }

        private async Task CreateAirportDocumentAsync(Airport airport)
        {
            Uri uri = getUri();
            Console.WriteLine("uri: {0}", uri);
            try
            {
                await this.Client.CreateDocumentAsync(uri, airport);
            }
            catch (Exception e) {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error in CreateAirportDocument: {0}, Message: {1}", e.Message, baseException.Message);
            }
        }

        private Uri getUri()
        {

            return UriFactory.CreateDocumentCollectionUri(this.DbName, this.CollName);
        }


        private void QueryAirportByIataCode(string iataCode) {

            Uri uri = getUri();
            string sql = $"SELECT * FROM c WHERE c.pk = '{iataCode}'";
            Console.WriteLine("sql: {0}", sql);

            // This approach casts the result into a generic Object:
            FeedOptions queryOptions = 
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            IQueryable<Object> airports =
                this.Client.CreateDocumentQuery<Object>(uri, sql, queryOptions);
            foreach (Object airport in airports)
            {
                Console.WriteLine("Document {0}", JsonConvert.SerializeObject(airport, Formatting.Indented));
            }

            // This approach casts the result into an Airport object:
            // FeedOptions queryOptions = 
            //     new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            // IQueryable<Airport> airports =
            //     this.Client.CreateDocumentQuery<Airport>(uri, sql, queryOptions);
            // foreach (Airport airport in airports)
            // {
            //     Console.WriteLine("Document {0}", airport.ToJson());
            // }
        }

        private  void QueryAirportsByLocation(double lat, double lng, double km) {

            try {
                int count = 0;
                double meters = km * 1000;
                Uri uri = getUri();
                string sql = $"SELECT * from c WHERE ST_DISTANCE(c.location, {{'type': 'Point', 'coordinates':[{lng}, {lat}]}}) < {meters}";
                Console.WriteLine("sql: {0}", sql);

                FeedOptions queryOptions = 
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
                IQueryable<Object> airports =
                    this.Client.CreateDocumentQuery<Object>(uri, sql, queryOptions);
                foreach (Object airport in airports)
                {   
                    count++;
                    Console.WriteLine("Document {0}", JsonConvert.SerializeObject(airport, Formatting.Indented));
                }
                Console.WriteLine("{0} airports found", count);
            }
            catch (Exception e) {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error in QueryAirportsByLocation: {0}, Message: {1}", e.Message, baseException.Message);
            }
        }

        // static methods invoked by Main() follow:
        private static void ReadAirportsCsvFile() 
        {
            Program p = new Program();
            p.ReadAirportsCsv();
            p.DisplayValues();

            foreach (var airport in p.Airports) {
                airport.postParse();
                Console.WriteLine(airport.ToString());
                Console.WriteLine(airport.ToJson());
            } 
        }

        private static void LoadCosmosDbAirportsCollection(
            string dbName, string collName, int sleepMs, int maxRows) 
        {
            Program p = new Program();
            p.DbName = dbName;
            p.CollName = collName;
            p.ReadAirportsCsv();
            p.DisplayValues();
            p.InitializeClient();

            int count = 0;
            foreach (var airport in p.Airports) {
                count++;
                if (count <= maxRows)
                {
                    airport.postParse();
                    string jstr = airport.ToJson();
                    Console.WriteLine("loading:  {0}", jstr);
                    Document doc = p.CreateAirportDocumentSync(airport);
                    Console.WriteLine("document: {0}", doc);
                    Thread.Sleep(sleepMs);
                }
            }
            Console.WriteLine("Terminating...");
            Thread.Sleep(3000);
        }
        private static void QueryAirportByIataCode(string dbName, string collName, string iataCode) {

            Program p = new Program();
            p.DbName = dbName;
            p.CollName = collName;
            p.ReadAirportsCsv();
            p.DisplayValues();
            p.InitializeClient();
            p.QueryAirportByIataCode(iataCode);
        }
        private static void QueryAirportsByLocation(
            string dbName, string collName, double lat, double lng, double km) {

            Console.WriteLine("QueryAirportsByLocation - db: {0} coll: {1} lat: {2} lng: {3} km: {4}", dbName, collName, lat, lng, km);

            Program p = new Program();
            p.DbName = dbName;
            p.CollName = collName;
            p.ReadAirportsCsv();
            p.DisplayValues();
            p.InitializeClient();
            p.QueryAirportsByLocation(lat, lng, km);
        }

        private static string ReadEnvVar(string name) {

            return Environment.GetEnvironmentVariable(name);
        } 

        // Example command-lines:
        // dotnet run read_airports_csv_file
        //
        // dotnet run load_cosmosdb_airports_collection <dbName> <collName> <sleepMs> <maxRows>
        // dotnet run load_cosmosdb_airports_collection hackathon airports 200 3
        //
        // dotnet run query_airport_by_iata_code <dbName> <collName> <iataCode>
        // dotnet run query_airport_by_iata_code hackathon airports CLT
        //
        // dotnet run query_airports_by_location <dbName> <collName> <lat> <lng> <km>
        // dotnet run query_airports_by_location hackathon airports 35.499235 -80.848469 40

        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string programFunction = args[0].ToLower();
                string dbName   = null;
                string collName = null;
                string iataCode = null;
                int sleepMs = 0;
                int maxRows = 0;

                switch (programFunction) {
                    case "read_airports_csv_file":
                        ReadAirportsCsvFile();
                        break;
                    case "load_cosmosdb_airports_collection":
                        dbName   = args[1];
                        collName = args[2];
                        sleepMs = Int32.Parse(args[3]);
                        maxRows = Int32.Parse(args[4]);
                        LoadCosmosDbAirportsCollection(dbName, collName, sleepMs, maxRows);
                        break;
                    case "query_airport_by_iata_code":
                        dbName   = args[1];
                        collName = args[2];
                        iataCode = args[3];
                        QueryAirportByIataCode(dbName, collName, iataCode);
                        break;
                    case "query_airports_by_location":
                        dbName   = args[1];
                        collName = args[2];
                        double lat = Double.Parse(args[3]);
                        double lng = Double.Parse(args[4]);
                        double km  = Double.Parse(args[5]);
                        QueryAirportsByLocation(dbName, collName, lat, lng, km);
                        break;
                    default:
                        Console.WriteLine("Unknown programFunction: {0}", programFunction);
                        break;
                }
            }
        }
    }
}
