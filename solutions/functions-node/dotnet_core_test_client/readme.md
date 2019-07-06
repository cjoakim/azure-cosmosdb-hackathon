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


## Run the App

```
$ dotnet build
$ dotnet run send_event_hub_messsages 1
Sending message: {"name":"Ninoy Aquino Intl","city":"Manila","country":"Philippines","iata_code":"MNL","latitude":"14.508647","longitude":"121.019581","altitude":"75","timezone_num":"8","timezone_code":"Asia/Manila","location":{"type":"Point","coordinates":[121.019581,14.508647]},"pk":"1562439498","epoch":1562439498,"airline":"UA","flightNumber":"4792","eventName":"Arrive"}
```

Sample Event Document in CosmosDB:
```
{
    "name": "Ninoy Aquino Intl",
    "city": "Manila",
    "country": "Philippines",
    "iata_code": "MNL",
    "latitude": "14.508647",
    "longitude": "121.019581",
    "altitude": "75",
    "timezone_num": "8",
    "timezone_code": "Asia/Manila",
    "location": {
        "type": "Point",
        "coordinates": [
            121.019581,
            14.508647
        ]
    },
    "pk": "1562439498",
    "epoch": 1562439498,
    "airline": "UA",
    "flightNumber": "4792",
    "eventName": "Arrive",
    "id": "3e350c39-f185-446d-ae2a-1cfc5474d2a7",
    "_rid": "M2ZjAMK7mfMBAAAAAAAAAA==",
    "_self": "dbs/M2ZjAA==/colls/M2ZjAMK7mfM=/docs/M2ZjAMK7mfMBAAAAAAAAAA==/",
    "_etag": "\"1301da2c-0000-0100-0000-5d20ef4d0000\"",
    "_attachments": "attachments/",
    "_ts": 1562439502
}
```