# azure-cosmosdb-hackathon

## Challenge 5 with Node.js - Server-Side programming with Stored Procedures

The solution code is in directory **solutions/node/** in this repository.

### Azure Portal

In Azure Portal, create a CosmosDB with SQL API Account.

Then create a **hackathon** database within it.

In your **hackathon** database create a collection named **music_bands** with a partition key named **pk**

### Node.js

```
npm install
npm list

node main.js create_stored_proc hackathon music_bands lookupDoc create

node main.js create_stored_proc hackathon music_bands bulkImport create

node main.js execute_stored_proc hackathon music_bands bulkImport
```

In Azure Portal, execute this query:
```
{
    "pk": "u2",
    "name": "bono",
    "bulk_import_date": "2019-04-24T13:56:41.455Z",
    "id": "7f2a5051-8a30-129f-0a0e-7db2f2b124ad",
    "_rid": "VPMYAK5SYPIBAAAAAAAAAA==",
    "_self": "dbs/VPMYAA==/colls/VPMYAK5SYPI=/docs/VPMYAK5SYPIBAAAAAAAAAA==/",
    "_etag": "\"0300fc48-0000-0100-0000-5cc06b190000\"",
    "_attachments": "attachments/",
    "_ts": 1556114201
}
```

Then use that **id** and **pk** as parameters to the **lookupDoc** stored-procedure:
```
node main.js execute_stored_proc hackathon music_bands lookupDoc 7f2a5051-8a30-129f-0a0e-7db2f2b124ad u2
```

Delete the stored-procedures:
```
node main.js delete_stored_proc hackathon music_bands lookupDoc
node main.js delete_stored_proc hackathon music_bands bulkImport
```

### Notes

Notice how the stored-procedures are written in file **sproc.js** as simple
JavaScript functions, then referenced in main.js like this:
```
sproc_def = sproc.bulkImport;
```
