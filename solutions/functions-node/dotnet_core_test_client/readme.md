## Test Client Program for functions-node/hackathon Azure Functions

## Links

- https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-send
- https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-dotnetcore-get-started

## Create the App
```
$ dotnet new console -o dotnet_core_test_client
$ cd dotnet_core_test_client/
$ dotnet build
$ dotnet run

$ dotnet add package Microsoft.Azure.EventHubs
$ dotnet add package Microsoft.Azure.DocumentDB.Core
```


## Run the App - Send Messages to EventHubs

```
$ dotnet build
$ dotnet run send_event_hub_messsages 1
Sending message: {"name":"Sydney Intl","city":"Sydney","country":"Australia","iata_code":"SYD","latitude":"-33.946111","longitude":"151.177222","altitude":"21","timezone_num":"10","timezone_code":"Australia/Sydney","location":{"type":"Point","coordinates":[151.177222,-33.946111]},"pk":"SYD","epoch":1562440407,"airline":"DL","flightNumber":"2413","eventName":"Depart"}
```

Sample Event Document in CosmosDB:
```
{
    "name": "Sydney Intl",
    "city": "Sydney",
    "country": "Australia",
    "iata_code": "SYD",
    "latitude": "-33.946111",
    "longitude": "151.177222",
    "altitude": "21",
    "timezone_num": "10",
    "timezone_code": "Australia/Sydney",
    "location": {
        "type": "Point",
        "coordinates": [
            151.177222,
            -33.946111
        ]
    },
    "pk": "SYD",
    "epoch": 1562440407,
    "airline": "DL",
    "flightNumber": "2413",
    "eventName": "Depart",
    "id": "047f35b4-7a09-4312-afe1-c44d171606ca",
    "_rid": "M2ZjAMK7mfMEAAAAAAAAAA==",
    "_self": "dbs/M2ZjAA==/colls/M2ZjAMK7mfM=/docs/M2ZjAMK7mfMEAAAAAAAAAA==/",
    "_etag": "\"5501e8c3-0000-0100-0000-5d20f2d90000\"",
    "_attachments": "attachments/",
    "_ts": 1562440409
}
```

## Run the App - Query the Event Documents in CosmosDB

```
$ dotnet run query_cosmosdb airport_events SYD
QueryCosmosDB: airport_events, dev, events
QueryAirportEvents: SYD
SQL: SELECT * FROM functions WHERE functions.pk = 'SYD'
{"name":"Sydney Intl","city":"Sydney","country":"Australia","iata_code":"SYD","latitude":"-33.946111","longitude":"151.177222","altitude":"21","timezone_num":"10","timezone_code":"Australia/Sydney","location":{"type":"Point","coordinates":[151.177222,-33.946111]},"pk":"SYD","epoch":1562440407,"airline":"DL","flightNumber":"2413","eventName":"Depart","id":"047f35b4-7a09-4312-afe1-c44d171606ca","_rid":"M2ZjAMK7mfMEAAAAAAAAAA==","_self":"dbs/M2ZjAA==/colls/M2ZjAMK7mfM=/docs/M2ZjAMK7mfMEAAAAAAAAAA==/","_etag":"\"5501e8c3-0000-0100-0000-5d20f2d90000\"","_attachments":"attachments/","_ts":1562440409}
```
