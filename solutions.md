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

- [.Net Core](solutions/challenge1/dotnetcore/notes.md) &nbsp;&nbsp; **Implemented!**
- [Java](solutions/challenge1/java/notes.md)
- [Node.js](solutions/challenge1/node/notes.md)
- [Python](solutions/challenge1/python/notes.md)

## Challenge 2 - Port a MongoDB database CosmosDB/Mongo with CLI tools

- [CLI](solutions/challenge2/cli/notes.md) &nbsp;&nbsp; **Implemented!**

## Challenge 3 - Port a MongoDB database CosmosDB/SQL

- [.Net Core](solutions/challenge3/dotnetcore/notes.md)
- [Java](solutions/challenge3/java/notes.md)
- [Node.js](solutions/challenge3/node/notes.md)
- [Python](solutions/challenge3/python/notes.md) &nbsp;&nbsp; **Implemented!**

## Challenge 4 - Use the Change Feed with Azure Functions

- [.Net Core](solutions/challenge4/dotnetcore/notes.md)
- [Java](solutions/challenge4/java/notes.md)
- [Node.js](solutions/challenge4/node/notes.md)
- [Python](solutions/challenge4/python/notes.md)

## Challenge 5 - Server-Side programming with Stored Procedures

- [.Net Core](solutions/challenge5/dotnetcore/notes.md)
- [Java](solutions/challenge5/java/notes.md)
- [Node.js](solutions/challenge5/node/notes.md) &nbsp;&nbsp; **Implemented!**
- [Python](solutions/challenge5/python/notes.md)

## Challenge 6 - Server-Side programming with UDFs

- [.Net Core](solutions/challenge6/dotnetcore/notes.md)
- [Java](solutions/challenge6/java/notes.md)
- [Node.js](solutions/challenge6/node/notes.md)
- [Python](solutions/challenge6/python/notes.md)

## Challenge 7 - Server-Side programming with Triggers

- [.Net Core](solutions/challenge7/dotnetcore/notes.md)
- [Java](solutions/challenge7/java/notes.md)
- [Node.js](solutions/challenge7/node/notes.md)
- [Python](solutions/challenge7/python/notes.md)

## Challenge 8 - Gremlin Graph Database

- [.Net Core](solutions/challenge8/dotnetcore/notes.md)
- [Java](solutions/challenge8/java/notes.md)
- [Node.js](solutions/challenge8/node/notes.md)
- [Python](solutions/challenge8/python/notes.md)

## Challenge 9 - Cassandra

- [.Net Core](solutions/challenge9/dotnetcore/notes.md)
- [Java](solutions/challenge9/java/notes.md)
- [Node.js](solutions/challenge9/node/notes.md)
- [Python](solutions/challenge9/python/notes.md) &nbsp;&nbsp; **Implemented!**

## Challenge 10 - Use the CosmosDB REST API

- [.Net Core](solutions/challenge10/dotnetcore/notes.md)
- [Java](solutions/challenge10/java/notes.md)
- [Node.js](solutions/challenge10/node/notes.md) &nbsp;&nbsp; **Implemented!**
- [Python](solutions/challenge10/python/notes.md)

## Challenge 11 - Integrate Azure Search with CosmosDB

- [.Net Core](solutions/challenge11/dotnetcore/notes.md)
- [Java](solutions/challenge11/java/notes.md)
- [Node.js](solutions/challenge11/node/notes.md)
- [Python](solutions/challenge11/python/notes.md)

---

[Hackathon Challenges](challenges.md)
