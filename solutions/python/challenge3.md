# azure-cosmosdb-hackathon

## Challenge 3 with Python - Port a MongoDB database CosmosDB/SQL

This solution uses a dataset containing 1459 US Airports.

---

## Part 1 - localhost/on-prem MongoDB

- Use **mongo** program to drop/recreate the **airports** collection 
- Use **mongo** program to load the airports collection
  - db.airports.insert(...)
- Use **mongo** program to query the airports collection
- Use **mongoexport** program to export the airports collection to a JSON file
- Use **mongo** program to open a DB shell

See [**demo_local.sh**](demo_local.sh)

MongoDB Shell Commands:
```
> show dbs
> use demo
> show collections
airports
> db.airports.count()
1459
> db.airports.find({iata_code:"ATL"}).pretty()
> exit
```

Inspect the output from **mongoexport**:
```
cat tmp/airports.json
```

---

## Part 2 - Port Data to Azure CosmosDB w/MongoDB API

The focus here is to use the tooling MongoDB tooling that you're
probably already familiar with - the MongoDB CLI programs, and Python.

- View Azure Portal - see no collections in the demo database
- Use the **Azure CLI** (az) to create two collections in CosmosDB
  - one is **fixed** in size, the other is **partitioned**
  - purpose is to demonstrate two ways of migrating your data
    - mongo cli tools
    - application code (python in this case)
  - see script [create_azure_collections.sh](create_azure_collections.sh)
- Use the **mongoimport** program import to the CosmosDB fixed collection
  - input is the JSON file produced by the above localhost **mongoexport**
  - See [mongo_import.sh](mongo_import.sh)
  - ./mongo_import.sh azure_airports
- Use **python & pymongo** to load the CosmosDB partitioned collection
  - See [main.py](main.py) and the open-source [pymongo](https://pypi.org/project/pymongo/) library on PyPI
  - ./venv.sh
  - source bin/activate
  - source common_env.sh
  - env | grep AZURE_MONGODB_   (verify that environment variables are set)
  - python main.py load_azure_mongo_collection airports_p tmp/airports.json
- Use the Azure CLI to reduce the **RU** throughput and cost of the collections
- Use **mongo** program to query the collection
- Use **mongo** program to open a DB shell and connect to Azure CosmosDB

See [**demo_azure.sh**](demo_azure.sh)

Note: Most, but not all, MongoDB functionality is supported by CosmosDB.
See https://docs.microsoft.com/en-us/azure/cosmos-db/mongodb-feature-support

---

## Part 3 - Port Data to Azure CosmosDB w/SQL API

- Python code is similar to #2 above, but uses the open-source
  [**pydocumentdb**](https://pypi.org/project/pydocumentdb/) library on PyPI
  - See [main.py](main.py)
- python main.py load_azure_sql_collection airports tmp/airports.json
- SDKs in Java, .Net, Node.js, Python, Go
  - See https://docs.microsoft.com/en-us/azure/cosmos-db/

---

## Part 4 - Microsoft Tooling

- The Azure CLI (az)
  - Cross-platform with the **same syntax** - Windows, Linux, macOS
  - az cosmosdb list -g cjoakim-cosmos
  - az cosmosdb collection update ... --throughput 5000
  - https://pypi.org/project/azure-cli-cosmosdb/0.2.8/

- PowerShell
  - Similar functionality to the above CLI

- Azure Storage Explorer UI
  - CosmosDB SQL API is in preview.  Also Azure Storage and Data Lake.

- Azure Cosmos DB Data Migration Tool
  - Windows, CosmosDB SQL and Table APIs
  - See https://docs.microsoft.com/en-us/azure/cosmos-db/import-data

- Azure Database Migration Service
  - See https://docs.microsoft.com/en-us/azure/dms/tutorial-mongodb-cosmos-db?toc=/azure/cosmos-db/toc.json

---

## Part 5 - Queries 

### MongoDB API

**JavaScript** syntax

mongo shell connected to Azure CosmosDB with MongoDB API:
```
globaldb:PRIMARY> use demo
switched to db demo
globaldb:PRIMARY> db.airports_p.count()
NumberLong(1459)
globaldb:PRIMARY> db.airports_p.find({iata_code:"ATL"}).pretty()
{
    "_id" : ObjectId("5c7077cb8f57094c31f57cc5"),
    "name" : "Hartsfield Jackson Atlanta Intl",
    "city" : "Atlanta",
    "country" : "United States",
    "iata_code" : "ATL",
    "latitude" : "33.636719",
    "longitude" : "-84.428067",
    "altitude" : "1026",
    "timezone_num" : "-5",
    "timezone_code" : "America/New_York",
    "location" : {
        "type" : "Point",
        "coordinates" : [
            -84.428067,
            33.636719
        ]
    }
}
```

### CosmosDB API

**SQL** syntax

```
SELECT * FROM c where c.pk = 'CLT'

[
    {
        "name": "Charlotte Douglas Intl",
        "city": "Charlotte",
        "country": "United States",
        "iata_code": "CLT",
        "latitude": "35.214",
        "longitude": "-80.943139",
        "altitude": "748",
        "timezone_num": "-5",
        "timezone_code": "America/New_York",
        "location": {
            "type": "Point",
            "coordinates": [
                -80.943139,
                35.214
            ]
        },
        "pk": "CLT",
        "id": "a50d4492-26a0-4147-91e3-81e0375b3ea6",
        "_rid": "d3FGAPbzln78AAAAAAAAAA==",
        "_self": "dbs/d3FGAA==/colls/d3FGAPbzln4=/docs/d3FGAPbzln78AAAAAAAAAA==/",
        "_etag": "\"07006404-0000-0100-0000-5c713ef00000\"",
        "_attachments": "attachments/",
        "_ts": 1550925552
    }
]
```

Queries with deeply-nested attributes; longitude of Kauai, Hawaii.
```
SELECT c.name, c.iata_code, c.timezone_code, c.location FROM c 
where (c.location.coordinates[0] < -159.0) 
and   (c.location.coordinates[0] > -160.0)
```


**Geo-Spatial** query of airports within 60km (37 miles) from Duluth, GA.
See full results [duluth_airports](data/duluth_airports.json)
```
SELECT c.name, c.city, c.iata_code,c.location.coordinates from c 
WHERE ST_DISTANCE(c.location, 
    {'type': 'Point', 'coordinates':[-84.080786, 34.002227]}) < 60000

[
    {
        "name": "Fulton County Airport Brown Field",
        "city": "Atlanta",
        "iata_code": "FTY",
        "coordinates": [
            -84.5214,
            33.7791
        ]
    },
    {
        "name": "Gwinnett County Airport-Briscoe Field",
        "city": "Lawrenceville",
        "iata_code": "LZU",
        "coordinates": [
            -83.9623772,
            33.9780761
        ]
    },
...
    {
        "name": "Hartsfield Jackson Atlanta Intl",
        "city": "Atlanta",
        "iata_code": "ATL",
        "coordinates": [
            -84.428067,
            33.636719
        ]
    },
...
]
```

---

## Links

- https://docs.microsoft.com/en-us/azure/cosmos-db/tutorial-query-mongodb
- https://blog.olandese.nl/2017/12/13/create-a-sharded-mongodb-in-azure-cosmos-db/
- https://docs.microsoft.com/en-us/cli/azure/cosmosdb?view=azure-cli-latest
- https://pypi.org/project/pymongo/
- https://pypi.org/project/pydocumentdb/
- https://docs.microsoft.com/en-us/azure/cosmos-db/geospatial

---

## Set Up for this Demo

- Requires the MongoDB, Azure CLI, and Python 3 to be installed.
- Create a Python virtual environment; run **venv.sh**
- Provision an Azure CosmosDB databases with the MongoDB and SQL APIs
- Set configuration environment variables; see [common_env.sh](common_env.sh)

### Install MongoDB 3.2 Locally on macOS

```
brew install mongodb@3.2

rm -rf /data/db
mkdir -p /data/db
```
