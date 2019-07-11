# azure-cosmosdb-hackathon - Server-Side Programming

## Topics

- Stored Procedures
- Triggers; pre and post
- User-Defined Functions (UDFs)
- Authored in JavaScript
- Invoked via SDKs

[Server-Side Programming Documentation](https://docs.microsoft.com/en-us/azure/cosmos-db/stored-procedures-triggers-udfs)


## Code

See the **solutions/node/** directory in this repo

---

## Stored Procedures

[Stored Procedures Example](solutions/node/STORED_PROCEDURES.md)

---

## User Defined Functions

#### What is a UDF?

"A user-defined function (UDF) is a side effect free piece of application logic written in JavaScript. It allows developers to construct a query operator, thus extending the core of the Cosmos DB query language."

[Documentation](https://docs.microsoft.com/en-us/rest/api/cosmos-db/user-defined-functions)

- No side effects (i.e. - Database changes)
- Query only
- Extends the SQL language for your application

UDF Code:
```

var southEastUsa = {
    id: "southEastUsa",
    serverScript: function southEastUsa(pk) {
        return ["ATL", "CLT", "MIA"].includes(pk);
    }
}

var southEastUsa = {
    id: "southEastUsa",
    serverScript: function southEastUsa(pk) {
        if (pk == 'ATL') {
            return true;
        } 
        if (pk == 'CLT') {
            return true;
        }
        return false;
    }
}
```

Deploy the UDF:
```
node main.js create_udf dev airports southEastUsa create
```

Use the UDF in a SQL Query:
```
SELECT * FROM c WHERE udf.southEastUsa(c.pk)
SELECT c.pk, c.city, c.name FROM c WHERE udf.southEastUsa(c.pk)
```

--- 

## Triggers

- Allows you to execute logic before and/or after a database operation
- But you have to request them when you execute the operation
- They aren't invoked automatically like in a Relational Database
- You specify the Triggers in the RequestOptions

[Documentation](https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-use-stored-procedures-triggers-udfs#pre-triggers)

```
// This trigger ensures that the Document contains a create_time epoch timestamp attrubute.

var preCreate = {
    id: "preCreate",
    triggerType: TriggerType.Pre,
    triggerOperation: TriggerOperation.All,

    serverScript: function validate() {
        var context = getContext();
        var request = context.getRequest();
        var doc     = request.getBody();
        var date    = new Date();
        doc["create_time"] = date.getTime();
        request.setBody(doc);
    }
}

// This trigger creates a "history" Document for the given Document

var postHistory = {
    id: "postHistory",
    triggerType: TriggerType.Post,
    triggerOperation: TriggerOperation.All,

    serverScript: function () {
        var context = getContext();
        var request = context.getRequest();
        var collection = context.getCollection();
        var selfLink = collection.getSelfLink();
        var doc  = request.getBody();
        var date = new Date();
        var id = doc['id']
        var pk = doc['pk']

        // create a "deep copy" of the given doc, for modification
        let historyDoc = JSON.parse(JSON.stringify(doc));  
        delete historyDoc['id'];
        delete historyDoc['_attachments'];
        delete historyDoc['_etag'];
        delete historyDoc['_lsn'];
        delete historyDoc['_rid'];
        delete historyDoc['_self'];
        delete historyDoc['_ts']; 
        historyDoc['doctype'] = doc['doctype'] + '_history';
        historyDoc['history_id_pk'] = '' + id + '|' + pk;
        historyDoc['history_date'] = date;
        historyDoc['history_epoc'] = date.getTime(); 
        historyDoc['history_method'] = 'postHistoryTrigger';

        var created = collection.createDocument(selfLink, historyDoc,  
            function (err, newDoc) { 
                if (err) {
                    doc['last_err_msg'] = err.message; 
                    doc['last_err_date'] = date; 
                }
            }); 
    }
}
```

Sample C# code to execute the two above Triggers with an insert:

```
RequestOptions requestOptions =
    new RequestOptions {
        PreTriggerInclude = new List<string> { "preCreate" },
        PostTriggerInclude = new List<string> { "postHistory" }
    };

await client.CreateDocumentAsync(containerUri, newItem, requestOptions);
```
