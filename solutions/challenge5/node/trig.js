'use strict';

const DocumentBase = require('documentdb').DocumentBase;
const TriggerType  = DocumentBase.TriggerType;
const TriggerOperation = DocumentBase.TriggerOperation;

// This file contains CosmosDB Triggers.
// See https://azure.github.io/azure-cosmosdb-js-server/
// Chris Joakim, Microsoft, 2019/04/14

// triggerOperation: TriggerOperation.Create
// triggerOperation: TriggerOperation.All

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
        // see https://scotch.io/bar-talk/copying-objects-in-javascript
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

        //request.setBody(doc);
    }
}

module.exports.preCreate   = preCreate;
module.exports.postHistory = postHistory;
