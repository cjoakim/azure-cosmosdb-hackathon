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
dotnet run load_cosmosdb_airports_collection hackathon airports 200 3

dotnet run query_airport_by_iata_code <dbName> <collName> <iataCode>
dotnet run query_airport_by_iata_code hackathon airports CLT

dotnet run query_airports_by_location <dbName> <collName> <lat> <lng> <km>
dotnet run query_airports_by_location hackathon airports 35.499235 -80.848469 40
```
