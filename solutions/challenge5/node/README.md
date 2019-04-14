# azure-cosmosdb-samples - server-side stored procedures

## The Test

- See file **test.sh** which executes the following
- Queries and Deletes all Documents in the CosmosDB **dev** database **airports** collection
- Deletes and Redeploys the  **upsertAirportDoc** Stored Procedure
- Executes the **airport_sproc_test.js** Node.js client program:
  - Loads the list of top 50 world airports
  - Perform 300 iterations through this list and invoke the Stored Procedure for each iteration
  - The first 50 are inserts
  - The remaining 250 are either **no-change upserts** or **actual upserts**
  - Randomly determine if the iteration is a no-change or not
  - Randomly generate temperature, humidity, and rainfall values for changes
  - Test client program checks the expected vs actual results of the Stored Proc and writes to a log file
- Grep the log file for CLT (Charlotte, NC) results
- Query CosmosDB for the final state of CLT

## upsertAirportDoc Stored Proc Logic

- Receives a full or partial JavaScript Object; a full or partial Document (partial updates are supported)
- Queries CosmosDB (on pk and iata_code) to find the Document corresponding to the given JavaScript Object
- Inserts a new Document if the query returns no results
- If not a new Document:
  - Detects individual attribute changes between existing Document and given Object
  - Overlays the Document with the changed or additional values
  - **upserts the Document only if there are changes**
- Several **__sp_xxx** attributes are added as necessary:
  - __sp_created_at - an epoch time
  - __sp_updated_at - an epoch time
  - __sp_diff - indicates if the Document was changed; 1 = true, 0 = false
  - __sp_diffs - an array of the specific changes
  - These last two fields are intented for the intelligent use by the client program for ML purposes

See **sproc.js** for the implementation of **upsertAirportDoc** Stored Procedure.

## Files of Interest

### Server-Side code and its Deployment

- **sproc.js** contains CosmosDB Stored Procedures
- **trig.js** contains CosmosDB Trigger functions
- **udf.js** contains CosmosDB UDFs
- **admin.js** executed from command line, deploys the server-side code, etc

### Client test files

- **test.sh** bash shell script to execute the test
- **airport_sproc_test.js** Node.js program invoked by test.sh, tests the **upsertAirportDoc** sproc
- **cosmos_docdb_util.js** DAO object

### Environment Variables

This code uses the following environment variables.  Set these on your system as necessary with your values.
```
AZURE_COSMOSDB_SQLDB_DBNAME
AZURE_COSMOSDB_SQLDB_URI
AZURE_COSMOSDB_SQLDB_KEY
AZURE_COSMOSDB_SQLDB_PREF_LOC
```

## Sample Data

CLT raw input data looks like this:
```
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
    }
}
```

CLT looks like this as a Document in CosmosDB - attributes are added:
```
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
    "id": "CLT",
    "pk": "CLT",
    "temperature": 107,
    "humidity": 80,
    "rain": 2,
    "__sp_created_at": 1537045945514,
    "__sp_updated_at": 1537046032284,
    "__sp_diff": 1,
    "__sp_diffs": [
        "chg; temperature: 49 -> 107",
        "chg; humidity: 75 -> 80",
        "chg; rain: 1 -> 2"
    ],
    "_rid": "VAtpAM6VPlI9AwAAAAAAAA==",
    "_self": "dbs/VAtpAA==/colls/VAtpAM6VPlI=/docs/VAtpAM6VPlI9AwAAAAAAAA==/",
    "_etag": "\"b900b3c4-0000-0000-0000-5b9d76100000\"",
    "_attachments": "attachments/",
    "_ts": 1537046032
}
```

- Note the **__sp_created_at** and **__sp_updated_at** tombstone attributes added by the Stored Proc
- Note the other **__sp_** attributes added by the Stored Procedure for testing purposes

## Sample Test Results

```
loading and updating the airports with the stored proc ...
grepping for CLT results ...
evt_obj_results; OK  47 iata: CLT xdiff: 0 sp_diff: 0 sp_updated_at 1537097761965 sp_diffs []
evt_obj_results; OK  97 iata: CLT xdiff: 0 sp_diff: 0 sp_updated_at 1537097761965 sp_diffs []
evt_obj_results; OK  147 iata: CLT xdiff: 0 sp_diff: 0 sp_updated_at 1537097761965 sp_diffs []
evt_obj_results; OK  197 iata: CLT xdiff: 1 sp_diff: 1 sp_updated_at 1537097802495 sp_diffs ["chg; temperature: 45 -> 109","chg; humidity: 63 -> 74"]
evt_obj_results; OK  247 iata: CLT xdiff: 1 sp_diff: 1 sp_updated_at 1537097815683 sp_diffs ["chg; humidity: 74 -> 75"]
evt_obj_results; OK  297 iata: CLT xdiff: 1 sp_diff: 1 sp_updated_at 1537097828954 sp_diffs ["chg; temperature: 109 -> 93","chg; humidity: 75 -> 62"]
```

Final state of CLT; note the correctness of the temperature, humidity, and __sp_xxx attributes:
```
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
    "id": "CLT",
    "pk": "CLT",
    "temperature": 93,
    "humidity": 62,
    "rain": 1,
    "__sp_created_at": 1537097761965,
    "__sp_updated_at": 1537097828954,
    "__sp_diff": 1,
    "__sp_diffs": [
        "chg; temperature: 109 -> 93",
        "chg; humidity: 75 -> 62"
    ],
    "_rid": "VAtpAM6VPlJJAwAAAAAAAA==",
    "_self": "dbs/VAtpAA==/colls/VAtpAM6VPlI=/docs/VAtpAM6VPlJJAwAAAAAAAA==/",
    "_etag": "\"ca00b3ae-0000-0000-0000-5b9e40640000\"",
    "_attachments": "attachments/",
    "_ts": 1537097828
}
```