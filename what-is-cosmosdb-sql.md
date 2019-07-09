# What is CosmosDB SQL?

It's the Microsoft Document-oriented database with a **SQL syntax**.  It's **not** a relational database.

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

- SQL-like **Query** Language
  - SELECT * FROM c WHERE c.epoch > 0
  - SELECT * FROM c WHERE c.pk = 'SYD' AND c.id = '1efad365-b53c-4084-a688-22b9c5ec3c2f'
  - SELECT VALUE COUNT(1) FROM c

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

---

## Deeper Dive on Partitions

- Partition Key Values != Physical Partitions
- CosmosDB Algorithm calculates the physical partition for a given pk
- Partition Splits, like VSAM

![resource-partition](img/resource-partition.png)

### An even distribution of your PK values and data is desired

![skew](img/cosmosdbpartitions.jpg)

---

## Links

- Common Use-Cases: https://docs.microsoft.com/en-us/azure/cosmos-db/use-cases
- https://docs.microsoft.com/en-us/azure/cosmos-db/sql-query-getting-started

---

## pydocumentdb SDK in-a-nutshell

See https://pypi.org/project/pydocumentdb/

```
import pydocumentdb.document_client as document_client

client = document_client.DocumentClient(host, {'masterKey': key})
client.default_headers['x-ms-documentdb-query-enablecrosspartition'] = True

coll_link = ''dbs/dev/colls/map_points'

client.UpsertDocument(coll_link, some_json_doc)
```
