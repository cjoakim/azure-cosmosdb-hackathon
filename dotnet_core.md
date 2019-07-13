# azure-cosmosdb-hackathon - DotNet Core

## The Future of .Net

- .Net 5 (https://devblogs.microsoft.com/dotnet/introducing-net-5/)

"**There will be just one .NET going forward, and you will be able to use it to target Windows, Linux, macOS, iOS, Android, tvOS, watchOS and WebAssembly and more.**"

#### Highlights

- Open source and community-oriented on GitHub
- Cross-platform implementation
- Capable command-line interface (CLI)
- Visual Studio, Visual Studio for Mac, and Visual Studio Code integration

## DotNet Core

See example project at **solutions/functions-node/dotnet_core_test_client/** in this repo.

### Demonstrate

#### SetUp

- dotnet new --help
- dotnet new console --output cosmos_console_app
- Build/Compile
- Entries in dotnet_core_test_client.csproj

#### Code

- Create Client
  - cosmosClient = new DocumentClient(new Uri(cosmosUri), cosmosKey);
- Task and Wait() or .Result, Promises
- SQL-style code
- Linq-style code
- [URIs](https://docs.microsoft.com/en-us/rest/api/cosmos-db/cosmosdb-resource-uri-syntax-for-rest), line 360
- FeedOptions / RequestOptions, line 339+
- Operations
  - InsertCosmosDocuments, line 280
  - QueryByPkId, line 276
  - QueryEventsForAirport, SQL-style, line 224
  - QueryEventsForLocation, ST_DISTANCE, line 234
  - DeleteDocuments, Linq-style, line 309
- RU Request Charge display
  - DocumentsQuery, line 245
  - .AsDocumentQuery() enables the collection of the RequestCharge

```
$ dotnet build

$ dotnet run
dotnet run time_now
dotnet run send_event_hub_messsages 10
dotnet run query_cosmos doc_by_pk_id <pk> <id>
dotnet run query_cosmos all_events <optional-after-epoch>
dotnet run query_cosmos events_for_airport <pk> <optional-after-epoch>
dotnet run query_cosmos events_for_city <city> <optional-after-epoch>
dotnet run query_cosmos delete_documents <max-count> <optional-after-epoch>
dotnet run query_cosmos count_documents
dotnet run query_cosmos events_for_location -80.842842 35.499586 1 <optional-after-epoch>
dotnet run insert_cosmos_documents 10
```

## Other Programming Language SDKs for CosmosDB/SQL

- Java
- Node.js
- Python
- REST

## ASP.DotNet Core

- Similar to Ruby-on-Rails, and Node.js/Express
- Entity Framework Core Cosmos DB (preview)
  - https://devblogs.microsoft.com/dotnet/announcing-entity-framework-core-2-2-preview-2/
  - https://msdn.microsoft.com/en-us/magazine/mt848702.aspx

