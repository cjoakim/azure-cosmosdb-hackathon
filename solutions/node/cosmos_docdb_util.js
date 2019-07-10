'use strict';

const events = require('events');
const util   = require('util');

const DocumentDBClient = require('documentdb').DocumentClient;
const DocumentBase = require('documentdb').DocumentBase;

// This utility class contains functions for invoking Azure CosmosDB/DocumentDB.
// See https://github.com/Azure/azure-cosmosdb-node/blob/master/source/lib/documentclient.js
// Chris Joakim, Microsoft, 2019/07/10

class CosmosDocDbUtil extends events.EventEmitter {

    constructor() {
        super();
        this.dbname = process.env.AZURE_COSMOSDB_SQLDB_DBNAME;
        var uri     = process.env.AZURE_COSMOSDB_SQLDB_URI;
        var key     = process.env.AZURE_COSMOSDB_SQLDB_KEY;
        this.client = new DocumentDBClient(uri, { masterKey: key });
    }

    // Account operations

    first_region() {
        if (this.locations.length > 0) {
            return this.locations[0];
        }
        else {
            return undefined;
        }
    }

    get_database_account() {
        var start_epoch = (new Date).getTime();
        this.client.getDatabaseAccount((err, db_acct, headers) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']    = 'CosmosDocDbUtil:get_database_account';
            evt_obj['err']     = err;
            evt_obj['db_acct'] = db_acct;
            evt_obj['headers'] = headers;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    get_read_endpoint(endpoint_url) {
        var start_epoch = (new Date).getTime();
        this.client.getReadEndpoint((result) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']   = 'CosmosDocDbUtil:get_read_endpoint';
            evt_obj['result'] = result;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    get_write_endpoint(endpoint_url) {
        var start_epoch = (new Date).getTime();
        this.client.getWriteEndpoint((result) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']   = 'CosmosDocDbUtil:get_write_endpoint';
            evt_obj['result'] = result;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    list_databases() {
        var start_epoch = (new Date).getTime();
        this.client.readDatabases().toArray((err, dbs) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type'] = 'CosmosDocDbUtil:list_databases';
            evt_obj['err']  = err;
            evt_obj['dbs']  = dbs;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    create_collection(dbname, cname) {
        var dblink = 'dbs/' + dbname;
        var collspec = { id: cname };
        var start_epoch = (new Date).getTime();
        this.client.createCollection(dblink, collspec, (err, created) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']    = 'CosmosDocDbUtil:create_collection';
            evt_obj['dbname']  = dbname;
            evt_obj['cname']   = cname;
            evt_obj['created'] = created;
            evt_obj['error']   = err;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    delete_collection(dbname, cname) {
        var colllink = 'dbs/' + dbname + '/colls/' + cname;
        var start_epoch = (new Date).getTime();
        this.client.deleteCollection(colllink, (err) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']    = 'CosmosDocDbUtil:delete_collection';
            evt_obj['dbname']  = dbname;
            evt_obj['cname']   = cname;
            evt_obj['error']   = err;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    list_collections(dbname) {
        var dblink = 'dbs/' + dbname;
        var start_epoch = (new Date).getTime();
        this.client.readCollections(dblink).toArray((err, collections) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type'] = 'CosmosDocDbUtil:list_collections';
            evt_obj['err']  = err;
            evt_obj['dbname'] = dbname;
            evt_obj['collections']  = collections;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    preferred_locations(comma_delim_locations) {
        if (typeof comma_delim_locations !== 'undefined' && comma_delim_locations) {
            return comma_delim_locations.split(',');
        }
        else {
            return [];
        }
    }

    // Document Operations

    create_document(dbname, cname, doc) {
        var colllink = 'dbs/' + dbname + '/colls/' + cname;
        var start_epoch = (new Date).getTime();
        this.client.createDocument(colllink, doc, (err, new_doc) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']   = 'CosmosDocDbUtil:create_document';
            evt_obj['dbname'] = dbname;
            evt_obj['cname']  = cname;
            evt_obj['doc']    = new_doc;
            evt_obj['error']  = err;
            evt_obj['start_epoch'] = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    query_documents(coll_link, query_spec, xpartition) {
        var bool = xpartition === true ? true : false;
        var opts = {};
        opts['enableCrossPartitionQuery'] = bool;

        //console.log('query_documents; ' + JSON.stringify(opts));
        var start_epoch = (new Date).getTime();
        this.client.queryDocuments(coll_link, query_spec, opts).toArray((err, results) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type'] = 'CosmosDocDbUtil:query_documents';
            evt_obj['coll_link'] = coll_link;
            evt_obj['query_spec'] = query_spec;
            evt_obj['err']     = err;
            evt_obj['results'] = results;
            evt_obj['start_epoch'] = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    read_by_id(dbname, cname, doc_id) {
        var opts = {};
        opts['enableCrossPartitionQuery'] = true;
        opts['partitionKey'] = '';
        var doclink = 'dbs/' + dbname + '/colls/' + cname + '/docs/' + doc_id;
        console.log(doclink);
        var start_epoch = (new Date).getTime();
        this.client.readDocument(doclink, opts, (err, doc) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']    = 'CosmosDocDbUtil:read_by_id';
            evt_obj['dbname']  = dbname;
            evt_obj['cname']   = cname;
            evt_obj['doc_id']  = doc_id;
            evt_obj['doclink'] = doclink;
            evt_obj['error']   = err;
            evt_obj['doc']     = doc;
            evt_obj['start_epoch'] = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    update_document(dbname, cname, doc_id, doc) {
        var doclink = 'dbs/' + dbname + '/colls/' + cname + '/docs/' + doc_id;
        var start_epoch = (new Date).getTime();
        this.client.replaceDocument(doclink, doc, (err, updated) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']    = 'CosmosDocDbUtil:update_document';
            evt_obj['dbname']  = dbname;
            evt_obj['cname']   = cname;
            evt_obj['doc_id']  = doc_id;
            evt_obj['doclink'] = doclink;
            evt_obj['error']   = err;
            evt_obj['updated'] = updated;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('update_document', evt_obj);
        });
    }

    delete_document(dbname, cname, doc_id, options) {
        var doclink = 'dbs/' + dbname + '/colls/' + cname + '/docs/' + doc_id;
        var start_epoch = (new Date).getTime();
        this.client.deleteDocument(doclink, options, (err) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']    = 'CosmosDocDbUtil:delete_document';
            evt_obj['dbname']  = dbname;
            evt_obj['cname']   = cname;
            evt_obj['doc_id']  = doc_id;
            evt_obj['doclink'] = doclink;
            evt_obj['error']   = err;
            evt_obj['start_epoch'] = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    // Stored Procedure Operations

    create_stored_proc(dbname, cname, sproc_def) {
        var colllink = 'dbs/' + dbname + '/colls/' + cname;
        var start_epoch = (new Date).getTime();
        this.client.createStoredProcedure(colllink, sproc_def, null, (err, sproc) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']   = 'CosmosDocDbUtil:create_stored_proc';
            evt_obj['dbname'] = dbname;
            evt_obj['cname']  = cname;
            evt_obj['sproc']  = sproc;
            evt_obj['error']  = err;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    delete_stored_proc(dbname, cname, sproc_name) {
        var sproc_link = 'dbs/' + dbname + '/colls/' + cname + '/sprocs/' + sproc_name;
        var start_epoch = (new Date).getTime();
        this.client.deleteStoredProcedure(sproc_link, null, (err, sproc) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']   = 'CosmosDocDbUtil:delete_stored_proc';
            evt_obj['dbname'] = dbname;
            evt_obj['cname']  = cname;
            evt_obj['sproc_name'] = sproc_name;
            evt_obj['sproc_link'] = sproc_link;
            evt_obj['error']  = err;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    execute_stored_proc(dbname, cname, sprocname, params, options) {
        var sproclink = 'dbs/' + dbname + '/colls/' + cname + '/sprocs/' + sprocname;
        var start_epoch = (new Date).getTime();
        this.client.executeStoredProcedure(sproclink, params, options, (err, results, responseHeaders) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']   = 'CosmosDocDbUtil:execute_stored_proc';
            evt_obj['dbname'] = dbname;
            evt_obj['cname']  = cname;
            evt_obj['sproc']  = sprocname;
            evt_obj['sproclink'] = sproclink;
            evt_obj['error']  = err;
            evt_obj['params'] = params;
            evt_obj['options'] = options;
            evt_obj['results'] = results;
            evt_obj['responseHeaders'] = responseHeaders;
            evt_obj['start_epoch'] = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    dummy(dbname, cname, sprocname, params, options) {
        var sproclink = 'dbs/' + dbname + '/colls/' + cname + '/sprocs/' + sprocname;
        console.log('dummy: ' + sproclink)
        var evt_obj = {};
        evt_obj['type']   = 'CosmosDocDbUtil:dummy';
        evt_obj['dbname'] = dbname;
        evt_obj['cname']  = cname;
        evt_obj['sproc']  = sprocname;
        evt_obj['sproclink'] = sproclink;
        this.emit('done', evt_obj);
    }

    // UDF Operations

    create_udf(dbname, cname, udf_def) {
        var colllink = 'dbs/' + dbname + '/colls/' + cname;
        var start_epoch = (new Date).getTime();
        this.client.createUserDefinedFunction(colllink, udf_def, null, (err, udf) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']    = 'CosmosDocDbUtil:create_udf';
            evt_obj['dbname']  = dbname;
            evt_obj['cname']   = cname;
            evt_obj['udf'] = udf;
            evt_obj['error']   = err;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    // Trigger Operations

    create_trigger(dbname, cname, trigger_def) {
        var colllink = 'dbs/' + dbname + '/colls/' + cname;
        var start_epoch = (new Date).getTime();
        this.client.createTrigger(colllink, trigger_def, null, (err, trigger) => {
            var finish_epoch = (new Date).getTime();
            var evt_obj = {};
            evt_obj['type']    = 'CosmosDocDbUtil:create_trigger';
            evt_obj['dbname']  = dbname;
            evt_obj['cname']   = cname;
            evt_obj['trigger'] = trigger;
            evt_obj['error']   = err;
            evt_obj['start_epoch']  = start_epoch;
            evt_obj['finish_epoch'] = finish_epoch;
            evt_obj['elapsed_time'] = finish_epoch - start_epoch;
            this.emit('done', evt_obj);
        });
    }

    // Miscellaneous methods

    db_link(dbname) {
        return 'dbs/' + dbname;
    }

    coll_link(dbname, cname) {
        return 'dbs/' + dbname + '/colls/' + cname;
    }
}

module.exports.CosmosDocDbUtil = CosmosDocDbUtil;
