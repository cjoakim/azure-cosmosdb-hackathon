# azure-cosmosdb-hackathon - Agenda

### Overview

- [What is NoSQL?](what-is-nosql.md)
- [What is CosmosDB?](what-is-cosmosdb.md)

### Data Ingestion (90 min)

- Event Hubs -> Azure Functions -> CosmosDB
- Introduce Event Hubs
- Introduce Functions - serverless, event-driven
- Show and deploy an EventHub triggered function that writes to a CosmosDB collection
- Write some world airport data to the EventHub
- Query the collection in Azure Portal
- Query GeoSpatial data in the collection in Azure Portal (i.e. - query airports close to Singapore and Bulgaria)
- Also show another Function that reads the CosmosDB change-feed
- Have CosmosDB automatically replicate the data to regions near Singapore and Bulgaria
 
### DotNet Core & SDKs  (50 min)

- Walk through and execute code that does CosmosDB CRUD and query operations
- Deeper dive into RUs and how to see how many RUs your code is using 
- Mention the other programming language SDKs - Java, Node, Python
- Mention .Net 5 and future roadmap (https://devblogs.microsoft.com/dotnet/introducing-net-5/)
 
### CosmosDB Server-Side Programming - Stored Procedures, Triggers, UDFs (50 min)
 
### CosmosDB Design - Patterns and Anti-Patterns (50 min)
 
### Scale CosmosDB with the Azure CLI (20 min)
 
### Demonstration of the CosmosDB Graph DB (30 min)

### Azure Search Integration (30 min)

### Hackathon (2 hours)

[Hackathon Challenges](challenges/challenges_list.md)

### Examples

- [SQL API](sql-api-demo.md)
- [Gremlin API](gremlin-graph-demo.md)
- [IoT with PaaS](https://github.com/cjoakim/azure-cosmosdb-iot)

---

### More Examples

- [Port a MongoDB database to CosmosDB](mongo-to-cosmosdb-demo.md)
- [Cassandra API](cassandra-api-demo.md)
