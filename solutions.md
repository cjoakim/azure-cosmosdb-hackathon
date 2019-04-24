# azure-cosmosdb-hackathon

Hackathon with CosmosDB

Alpha version 1

---

# Solution Implementation Notes

In general, the use of **environment variables** for configuration and secret values
is strongly suggested, rather than using configuration files.

This codebase uses the following environment variables in all programming languages.
**These values are samples**, see Azure Portal for **your values**.

```
COSMOSDB_HACKATHON_BASE_DIR
AZURE_SUBSCRIPTION_ID

AZURE_COSMOSDB_CASSDB_ACCT=cjoakimcosmosdbcass
AZURE_COSMOSDB_CASSDB_PASS=...secret...
AZURE_COSMOSDB_CASSDB_PORT=10350
AZURE_COSMOSDB_CASSDB_URI=cjoakimcosmosdbcass.cassandra.cosmos.azure.com
AZURE_COSMOSDB_CASSDB_USER=cjoakimcosmosdbcass

AZURE_COSMOSDB_MONGODB_CONN_STRING=mongodb://cjoakimcosmosdbmongo:...secret...@cjoakimcosmosdbmongo.documents.azure.com:10255/?ssl=true&replicaSet=globaldb
AZURE_COSMOSDB_MONGODB_DBNAME=hackathon
AZURE_COSMOSDB_MONGODB_HOST=cjoakimcosmosdbmongo.documents.azure.com
AZURE_COSMOSDB_MONGODB_PASS=...secret...
AZURE_COSMOSDB_MONGODB_PORT=10255
AZURE_COSMOSDB_MONGODB_USER=cjoakimcosmosdbmongo

AZURE_COSMOSDB_SQLDB_DBNAME=hackathon
AZURE_COSMOSDB_SQLDB_KEY=...secret...
AZURE_COSMOSDB_SQLDB_URI=https://cjoakimcosmosdbsql.documents.azure.com:443/
```

---

# Solutions

The following are implementations of one of many possible solutions.
Not every challenge is currently implemented in each language.
It's entirely fine if your working implementation varies from the solutions
in this GitHub repository.

## Challenge 1 - Port Relational and GPS Data to CosmosDB/SQL

- [.Net Core](solutions/challenge1/dotnetcore/notes.md)

## Challenge 2 - Port a MongoDB database CosmosDB/Mongo with CLI tools

- [CLI](solutions/challenge2/cli/notes.md)

## Challenge 3 - Port a MongoDB database CosmosDB/SQL

- [Python](solutions/python/challenge3.md)

## Challenge 4 - Use the Change Feed with Azure Functions

&nbsp;

## Challenge 5 - Server-Side programming with Stored Procedures

- [Node.js](solutions/challenge5/node/notes.md)

## Challenge 6 - Server-Side programming with UDFs

&nbsp;

## Challenge 7 - Server-Side programming with Triggers

&nbsp;

## Challenge 8 - Gremlin Graph Database

&nbsp;

## Challenge 9 - Cassandra

- [Python](solutions/python/challenge9.md)

## Challenge 10 - Use the CosmosDB REST API

- [Node.js](solutions/challenge10/node/notes.md)

## Challenge 11 - Integrate Azure Search with CosmosDB

&nbsp;

## Challenge 12 - Provision and modify CosmosDB with the Azure CLI (az)

- [bash](solutions/challenge12/bash/provision_sql_db.sh)

---

[Hackathon Challenges](challenges.md)
