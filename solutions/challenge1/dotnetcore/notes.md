# azure-cosmosdb-hackathon

## Challenge 1 with .Net Core

### Build the Solution

```
cd <this directory>

dotnet build
```

### Execute the Solution

```
dotnet run read_airports_csv_file

dotnet run load_cosmosdb_airports_collection <dbName> <collName> <sleepMs> <maxRows>
dotnet run load_cosmosdb_airports_collection hackathon airports 50 9999

dotnet run query_airport_by_iata_code <dbName> <collName> <iataCode>
dotnet run query_airport_by_iata_code hackathon airports CLT

dotnet run query_airports_by_location <dbName> <collName> <lat> <lng> <km>
dotnet run query_airports_by_location hackathon airports 35.499235 -80.848469 40
```

### Sample Results

```
$ dotnet run query_airport_by_iata_code hackathon airports CLT

ReadAirportsCsv: /Users/cjoakim/github/azure-cosmosdb-hackathon/data/openflights_airports.csv
CosmosDB EndpointUrl: https://cjoakimcosmosdbsql.documents.azure.com:443/
CosmosDB PrimaryKey:  <filtered>
CosmosDB DbName:      hackathon
CosmosDB CollName:    airports
Airports CSV count:   1631
PWD and PathSepChar   /Users/cjoakim/github/azure-cosmosdb-hackathon/solutions/challenge1/dotnetcore/cosmosdb1  /
InitializeClient: Microsoft.Azure.Documents.Client.DocumentClient
sql: SELECT * FROM c WHERE c.pk = 'CLT'
Document {
  "pk": "CLT",
  "AirportId": 3876,
  "Name": "Charlotte Douglas Intl",
  "City": "Charlotte",
  "Country": "United States",
  "IataCode": "CLT",
  "IcaoCode": "KCLT",
  "Latitude": 35.214,
  "Longitude": -80.943139,
  "location": {
    "type": "Point",
    "coordinates": [
      -80.943139,
      35.214
    ]
  },
  "Altitude": 748,
  "TimezoneNum": -5,
  "Dst": "A",
  "TimezoneCode": "America/New_York",
  "id": "5121a924-cfe6-4510-84cb-16811c5df4e3",
  "_rid": "VPMYAK8a-tg5AQAAAAAAAA==",
  "_self": "dbs/VPMYAA==/colls/VPMYAK8a-tg=/docs/VPMYAK8a-tg5AQAAAAAAAA==/",
  "_etag": "\"0000f40d-0000-0100-0000-5cb1fe0c0000\"",
  "_attachments": "attachments/",
  "_ts": 1555168780
}
```
