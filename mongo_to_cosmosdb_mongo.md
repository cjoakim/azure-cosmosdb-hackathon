# azure-cosmosdb-hackathon

## Port a MongoDB database CosmosDB/Mongo with CLI tools

No programming is involved in this solution.  The only tools you should
use are native MongoDB CLI tools (mongo, mongoimport, etc), the Azure CLI,
and shell scripting native to your OS (PowerShell, bash, etc).

## Processing

- Use database export file **data/mongoexport_airports.json** as the input
  - This file was create by the **mongoexport** program vs an existing database
- Manually create a CosmosDB with Mongo API database account in Azure Portal
- Use the Azure CLI to create a **hackathon** database with **airports** collection in the DB
- Use the **mongoimport** program to load the data into Azure
- The airports collection should be indexed on three non-quique attributes: iata_code, name, and timezone_num
- Use the Azure CLI to reduce the Throughput of the collection after it is loaded
- Use the **mongo** program to connect to Azure and query your new loaded database
- Prefer to use environment variables vs hard-coded values in your scripts

## Questions

- Query the number of documents in the collection.
  How many documents are there?

- Query the database for IATA Code 'ATL' and display the resulting document.

## Links

- https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest
- https://docs.microsoft.com/en-us/azure/cosmos-db/scripts/create-mongodb-database-account-cli
- https://docs.mongodb.com/manual/reference/program/mongoimport/
- https://docs.mongodb.com/manual/mongo/
