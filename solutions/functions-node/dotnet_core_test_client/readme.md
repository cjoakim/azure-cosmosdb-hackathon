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