# azure-cosmosdb-hackathon

## Challenge 5 with Node.js

```
npm install
npm list

node main.js create_stored_proc hackathon music_bands lookupDoc create
node main.js create_stored_proc hackathon music_bands bulkImport create

node main.js execute_stored_proc hackathon music_bands bulkImport

select * from c where c.pk = 'u2' and c.name = 'bono'

{
    "pk": "u2",
    "name": "bono",
    "bulk_import_date": "2019-04-14T11:21:58.365Z",
    "id": "c1c0b11c-d385-0575-455f-1b6041dda27b",
    "_rid": "VPMYAPnONMQBAAAAAAAAAA==",
    "_self": "dbs/VPMYAA==/colls/VPMYAPnONMQ=/docs/VPMYAPnONMQBAAAAAAAAAA==/",
    "_etag": "\"03002e38-0000-0100-0000-5cb317d60000\"",
    "_attachments": "attachments/",
    "_ts": 1555240918
}

node main.js execute_stored_proc hackathon music_bands lookupDoc c1c0b11c-d385-0575-455f-1b6041dda27b u2

{
  "type": "CosmosDocDbUtil:execute_stored_proc",
  "dbname": "hackathon",
  "cname": "music_bands",
  "sproc": "lookupDoc",
  "sproclink": "dbs/hackathon/colls/music_bands/sprocs/lookupDoc",
  "params": [
    "c1c0b11c-d385-0575-455f-1b6041dda27b",
    "u2"
  ],
  "options": {
    "partitionKey": "u2"
  },
  "results": "{\"id\":\"c1c0b11c-d385-0575-455f-1b6041dda27b\",\"pk\":\"u2\",\"doc\":{\"pk\":\"u2\",\"name\":\"bono\",\"bulk_import_date\":\"2019-04-14T11:21:58.365Z\",\"id\":\"c1c0b11c-d385-0575-455f-1b6041dda27b\",\"_rid\":\"VPMYAPnONMQBAAAAAAAAAA==\",\"_self\":\"dbs/VPMYAA==/colls/VPMYAPnONMQ=/docs/VPMYAPnONMQBAAAAAAAAAA==/\",\"_etag\":\"\\\"03002e38-0000-0100-0000-5cb317d60000\\\"\",\"_attachments\":\"attachments/\",\"_ts\":1555240918}}",
  "responseHeaders": {
    "cache-control": "no-store, no-cache",
    "pragma": "no-cache",
    "transfer-encoding": "chunked",
    "content-type": "application/json",
    "server": "Microsoft-HTTPAPI/2.0",
    "strict-transport-security": "max-age=31536000",
    "x-ms-last-state-change-utc": "Sat, 13 Apr 2019 12:50:14.944 GMT",
    "lsn": "4",
    "x-ms-schemaversion": "1.7",
    "x-ms-alt-content-path": "dbs/hackathon/colls/music_bands/sprocs/lookupDoc",
    "x-ms-content-path": "VPMYAPnONMQ=",
    "x-ms-quorum-acked-lsn": "4",
    "x-ms-current-write-quorum": "3",
    "x-ms-current-replica-set-size": "4",
    "x-ms-xp-role": "1",
    "x-ms-global-committed-lsn": "4",
    "x-ms-number-of-read-regions": "0",
    "x-ms-transport-request-id": "1",
    "x-ms-cosmos-llsn": "4",
    "x-ms-cosmos-quorum-acked-llsn": "4",
    "x-ms-session-token": "0:4",
    "x-ms-request-charge": "5.42",
    "x-ms-serviceversion": "version=2.2.0.0",
    "x-ms-activity-id": "a96cd97b-51cf-49e2-9334-c9723cfe252d",
    "x-ms-gatewayversion": "version=2.2.0.0",
    "date": "Sun, 14 Apr 2019 11:26:57 GMT",
    "x-ms-throttle-retry-count": 0,
    "x-ms-throttle-retry-wait-time-ms": 0
  },
  "start_epoch": 1555241217081,
  "finish_epoch": 1555241217592,
  "elapsed_time": 511
}

node main.js delete_stored_proc hackathon music_bands lookupDoc
node main.js delete_stored_proc hackathon music_bands bulkImport
```

