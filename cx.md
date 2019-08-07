# azure-cosmosdb-hackathon

A CosmosDB Overview and Hackathon

![azure-cosmos-db](img/azure-cosmosdb-2019.png)

---

## Overview and Topics

## Specific Items

- How to partition containers
  - [Design](design.md)
  - Know your Data

- How to improve the quality of queries
  - Execute sample queries, see response.RequestCharge values
  - dotnet run send_event_hub_messsages 10
  - dotnet run query_cosmos doc_by_pk_id MIA 6d61f28f-a796-45ea-8025-355e4a35bd39
  - dotnet run query_cosmos all_events 
  - dotnet run query_cosmos events_for_airport MIA
  - dotnet run query_cosmos events_for_city Miami
  - dotnet run query_cosmos delete_documents 1
  - dotnet run query_cosmos count_documents
  - dotnet run query_cosmos events_for_location -80.842842 35.499586 50
  - dotnet run query_cosmos events_for_location -80.290556 25.79325 1

- Available features – single vs multi region
  - https://docs.microsoft.com/en-us/azure/cosmos-db/distribute-data-globally
  
- Cosmos DB from an Ops perspective
  - [Performance tips](perf.md)

### Overview

- [What is NoSQL?](what-is-nosql.md)
- [What is CosmosDB?](what-is-cosmosdb.md)
- [What is CosmosDB SQL?](what-is-cosmosdb-sql.md)

### CosmosDB Design - Patterns and Anti-Patterns

- [Design](design.md)

### Scale CosmosDB with the Azure CLI

- [Scale with az CLI](scale_with_cli.md)

### Streaming Data Ingestion Pipeline

- [Streaming Data Ingestion Pipeline](data_ingest_pipeline.md)

### DotNet Core & SDKs 

- [DotNet Core](dotnet_core.md)

### CosmosDB Server-Side Programming - Stored Procedures, Triggers, UDFs

- [Server Side](server_side.md)

### Demonstration of the CosmosDB Graph DB

- [What does a Graph Look Like?](img/sample-graph.png)
- [Bill-of-Materials at https://github.com/Azure-Samples/](https://github.com/Azure-Samples/azure-cosmos-db-graph-npm-bom-sample)
- [Six Degrees of Kevin Bacon](https://github.com/cjoakim/azure-cosmosdb-graph-node)

---

## Hackathon

[Hackathon Setup](hackathon_setup.md)

[Hackathon Challenges](challenges/challenges_list.md)
