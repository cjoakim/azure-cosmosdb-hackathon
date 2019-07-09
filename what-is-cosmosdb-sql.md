# What is CosmosDB SQL?

It's the Microsoft Document-oriented database with a **SQL syntax**.

## Concepts

- Core Concepts
  - Database
  - Collection
  - [Scale by Request Units](https://docs.microsoft.com/en-us/azure/cosmos-db/request-units)
  - Partition Key
  - Document - JSON, id
  - Indexing
  - Transactions
  - SDKs

- Distributed Concepts
  - [Azure Regions](https://azure.microsoft.com/en-us/global-infrastructure/regions/)
  - Replication
  - Single vs Multi-Master

- [Server-Side Programming](https://docs.microsoft.com/en-us/azure/cosmos-db/stored-procedures-triggers-udfs)
  - Stored Procedure
  - Triggers; pre and post
  - UDF

- Interesting Features
  - [Change Feed](https://docs.microsoft.com/en-us/azure/cosmos-db/change-feed)
  - TTL
  - [Spatial Queries and GeoJSON](https://docs.microsoft.com/en-us/azure/cosmos-db/geospatial)
  - [Pooled RUs](https://docs.microsoft.com/en-us/azure/cosmos-db/set-throughput)

## Links

- Common Use-Cases: https://docs.microsoft.com/en-us/azure/cosmos-db/use-cases

## pydocumentdb SDK in-a-nutshell

See https://pypi.org/project/pydocumentdb/

```
import pydocumentdb.document_client as document_client

client = document_client.DocumentClient(host, {'masterKey': key})
client.default_headers['x-ms-documentdb-query-enablecrosspartition'] = True

coll_link = ''dbs/dev/colls/map_points'

client.UpsertDocument(coll_link, some_json_doc)
```

## CosmosDB, Azure Maps, and Spatial Queries demonstration

See https://github.com/cjoakim/azure-web-services/blob/master/maps/readme_v2.md
