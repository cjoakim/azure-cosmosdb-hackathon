'use strict';

// const DocumentBase = require('documentdb').DocumentBase;
// const TriggerType  = DocumentBase.TriggerType;
// const TriggerOperation = DocumentBase.TriggerOperation;

// This file contains CosmosDB User-Defined Functions (UDF).
// See https://azure.github.io/azure-cosmosdb-js-server/
// Chris Joakim, Microsoft, 2019/04/14


var docAge = {
    id: "docAge",
    serverScript: function docAge(epoch) {
        if (epoch == undefined) {
            return Number.MAX_VALUE;
        } 
        else {
            return (new Date().getTime()) - Number(epoch);
        }
    }
}

var docOverlay = {
    id: "docOverlay",
    serverScript: function docOverlay(givendoc, dbdoc) {
        dbdoc['diff'] = 0;
        var keys = Object.keys(givendoc);
        var len = keys.length;
        for (var i = 0; i < len; i++) {
            var key = keys[i];
            var newval = givendoc[key];
            if (dbdoc.hasOwnProperty(key)) {
                var oldval = dbdoc[key];
                if (newval === oldval) {

                }
                else {
                    dbdoc[key] = newval;
                    dbdoc['diff'] = 1;  
                }
            }
            else {
                dbdoc[key] = newval;
                dbdoc['diff'] = 1;  
            }
        }
        return dbdoc;
    }
}

module.exports.docAge     = docAge;
module.exports.docOverlay = docOverlay;
