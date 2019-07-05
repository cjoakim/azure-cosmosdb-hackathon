# azure-cosmosdb-hackathon - Data Ingestion Pipeline

- Event Hubs -> Azure Functions -> CosmosDB
- Introduce Event Hubs
- Introduce Functions - serverless, event-driven
- Show and deploy an EventHub triggered function that writes to a CosmosDB collection
- Write some world airport data to the EventHub
- Query the collection in Azure Portal
- Query GeoSpatial data in the collection in Azure Portal (i.e. - query airports close to Singapore and Bulgaria)
- Also show another Function that reads the CosmosDB change-feed
- Have CosmosDB automatically replicate the data to regions near Singapore and Bulgaria
- [IoT with PaaS](https://github.com/cjoakim/azure-cosmosdb-iot)
