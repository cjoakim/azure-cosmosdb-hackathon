'use strict';

// This file is executed from the command line to administer and test
// the server-side CosmosDB code (stored procs, triggers, udfs).
// It also implements some general CosmosDB account querying functionality.
// Chris Joakim, Microsoft, 2019/04/14
//
// Links:
// https://docs.microsoft.com/en-us/azure/cosmos-db/programming
// https://www.npmjs.com/package/azure
// https://www.npmjs.com/package/documentdb
// https://github.com/Azure/azure-cosmosdb-node
// https://azure.github.io/azure-cosmosdb-js-server/  (SDK)
// https://github.com/Azure/azure-cosmosdb-node/blob/master/source/lib/documentclient.js

// Command-line use:
//
// node admin.js get_database_account
// node admin.js get_read_endpoint
// node admin.js get_write_endpoint
// node admin.js list_databases
// node admin.js list_collections dev
//
// node admin.js create_stored_proc dev airports lookupDoc create
// node admin.js create_stored_proc dev airports createHistoryDoc create
// node admin.js create_stored_proc dev airports bulkImport create
// node admin.js create_stored_proc dev airports bulkDelete create
// node admin.js create_stored_proc dev airports upsertAirportDoc create
//
// node admin.js delete_stored_proc dev airports lookupDoc
//
// node admin.js execute_stored_proc dev airports lookupDoc bos bos
// node admin.js execute_stored_proc dev airports createHistoryDoc bos bos
// node admin.js execute_stored_proc dev airports bulkImport
// node admin.js execute_stored_proc dev airports bulkDelete
// node admin.js execute_stored_proc dev airports upsertAirportDoc
//
// node admin.js create_udf dev airports docOverlayUdf create
//
// node admin.js create_trigger dev airports preCreate create
// node admin.js create_trigger dev airports postHistory create
//
// node admin.js query_db dev airports sql/query_all_docs.json xpart tmp/all_docs.json
//
// node admin.js read_by_id dev airports DEN
//
// node admin.js delete_docs dev airports tmp/all_docs.json 

const events = require('events');
const fs     = require('fs');
const util   = require('util');

const sproc  = require('./sproc')
const trig   = require('./trig')
const udf    = require('./udf')

const DocumentBase = require('documentdb').DocumentBase;
const TriggerType = DocumentBase.TriggerType;
const TriggerOperation = DocumentBase.TriggerOperation;
const CosmosDocDbUtil = require('./cosmos_docdb_util').CosmosDocDbUtil;


class Main {

    constructor() {
        this.client    = null;
        this.db_link   = null;
        this.coll_link = null;
    }

    execute() {

        if (process.argv.length < 3) {
            console.log('error: too few command-line args provided.');
            process.exit();
        }
        else {
            var funct = process.argv[2];

            switch (funct) {

                case 'adhoc':
                    this.adhoc();
                    break;

                case 'get_database_account':
                    this.get_database_account();
                    break;

                case 'list_databases':
                    this.list_databases();
                    break;

                case 'get_read_endpoint':
                    this.get_read_endpoint();
                    break;

                case 'get_write_endpoint':
                    this.get_write_endpoint();
                    break;

                case 'list_collections':
                    var dbname = process.argv[3];
                    this.list_collections(dbname);
                    break;

                case 'create_stored_proc':
                    var dbname = process.argv[3];
                    var cname  = process.argv[4];
                    var sprocname = process.argv[5];
                    var create_ind = process.argv[6];
                    this.create_stored_proc(dbname, cname, sprocname, create_ind);
                    break;

                case 'execute_stored_proc':
                    var dbname = process.argv[3];
                    var cname  = process.argv[4];
                    var sprocname = process.argv[5];
                    this.execute_stored_proc(dbname, cname, sprocname);
                    break;

                case 'delete_stored_proc':
                    var dbname = process.argv[3];
                    var cname  = process.argv[4];
                    var sprocname = process.argv[5];
                    this.delete_stored_proc(dbname, cname, sprocname);
                    break;

                case 'create_udf':
                    var dbname = process.argv[3];
                    var cname  = process.argv[4];
                    var udfname = process.argv[5];
                    var create_ind = process.argv[6];
                    this.create_udf(dbname, cname, udfname, create_ind);
                    break;

                case 'create_trigger':
                    var dbname = process.argv[3];
                    var cname  = process.argv[4];
                    var triggername = process.argv[5];
                    var create_ind = process.argv[6];
                    this.create_trigger(dbname, cname, triggername, create_ind);
                    break;

                case 'query_db':
                    var dbname   = process.argv[3];
                    var collname = process.argv[4];
                    var specfile = process.argv[5];
                    var xpartition = process.argv[6] === 'xpart' ? true : false;
                    var outfile  = process.argv[7];
                    this.query_db(dbname, collname, specfile, xpartition, outfile);
                    break;

                case 'read_by_id':
                    var dbname   = process.argv[3];
                    var collname = process.argv[4];
                    var doc_id   = process.argv[5];
                    this.read_by_id(dbname, collname, doc_id);
                    break;

                case 'delete_docs':
                    var dbname   = process.argv[3];
                    var collname = process.argv[4];
                    var query_results_file = process.argv[5];
                    this.delete_docs(dbname, collname, query_results_file);
                    break;

                default:
                    console.log('error: unknown function - ' + funct);
            }
        }
    }

    adhoc() {
        var dbdoc = require('./clt');
        dbdoc['diff'] = 0;
        console.log(dbdoc);

        var givendoc = {};
        givendoc['temperature'] = 40;
        givendoc['humidity']    = 90;
        givendoc['new_thing']   = 777;

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
        console.log(dbdoc);
    }

    get_database_account() {
        var util = new CosmosDocDbUtil();
        util.on('done', (evt_obj) => {
            console.log(JSON.stringify(evt_obj, null, 2));
        });
        util.get_database_account();
    }

    list_databases() {
        var util = new CosmosDocDbUtil();
        util.on('done', (evt_obj) => {
            console.log(JSON.stringify(evt_obj, null, 2));
        });
        util.list_databases();
    }

    get_read_endpoint() {
        var util = new CosmosDocDbUtil();
        util.on('done', (evt_obj) => {
            console.log(JSON.stringify(evt_obj, null, 2));
        });
        util.get_read_endpoint();
    }

    get_write_endpoint() {
        var util = new CosmosDocDbUtil();
        util.on('done', (evt_obj) => {
            console.log(JSON.stringify(evt_obj, null, 2));
        });
        util.get_write_endpoint();
    }

    list_collections(dbname) {
        var util = new CosmosDocDbUtil();
        util.on('done', (evt_obj) => {
            console.log(JSON.stringify(evt_obj, null, 2));
        });
        util.list_collections(dbname);
    }

    create_stored_proc(dbname, cname, sprocname, create_ind) {
        var util = new CosmosDocDbUtil();
        var sproc_def = null;

        switch (sprocname) {
            case 'helloWorld':
                sproc_def = sproc.helloWorld;
                break;
            case 'lookupDoc':
                sproc_def = sproc.lookupDoc;
                break;
            case 'createHistoryDoc':
                sproc_def = sproc.createHistoryDoc;
                break;
            case 'bulkImport':
                sproc_def = sproc.bulkImport;
                break;
            case 'bulkDelete':
                sproc_def = sproc.bulkDelete;
                break;
            case 'upsertAirportDoc':
                sproc_def = sproc.upsertAirportDoc;
                break;

            default:
                console.log('error: unknown sprocname - ' + sprocname);
        }
        console.log(sproc_def);

        if (create_ind == 'create') {
            if (sproc_def != null) {
                util.on('done', (evt_obj) => {
                    console.log(JSON.stringify(evt_obj, null, 2));
                });
                util.create_stored_proc(dbname, cname, sproc_def);
            }
            else {
                console.log('error: null sproc_def');
            } 
        }
    }

    delete_stored_proc(dbname, cname, sproc_name) {
        var util = new CosmosDocDbUtil();
        util.on('done', (evt_obj) => {
            console.log(JSON.stringify(evt_obj, null, 2));
        });
        util.delete_stored_proc(dbname, cname, sproc_name);
    }

    create_udf(dbname, cname, udfname, create_ind) {
        var util = new CosmosDocDbUtil();
        var udf_def = null;

        switch (udfname) {
            case 'docAge':
                udf_def = udf.docAge;
                break;
            case 'docOverlay':
                udf_def = udf.docOverlay;
                break;
            default:
                console.log('error: unknown triggername - ' + udfname);
        }
        console.log(udf_def);

        if (create_ind == 'create') {
            if (udf_def != null) {
                util.on('done', (evt_obj) => {
                    console.log(JSON.stringify(evt_obj, null, 2));
                });
                util.create_udf(dbname, cname, udf_def);
            }
            else {
                console.log('error: null udf_def');
            } 
        }
    }

    create_trigger(dbname, cname, triggername, create_ind) {
        var util = new CosmosDocDbUtil();
        var trigger_def = null;

        switch (triggername) {
            case 'preCreate':
                trigger_def = trig.preCreate;
                break;
            case 'postHistory':
                trigger_def = trig.postHistory;
                break;
            default:
                console.log('error: unknown triggername - ' + triggername);
        }
        console.log(trigger_def);

        if (create_ind == 'create') {
            if (trigger_def != null) {
                util.on('done', (evt_obj) => {
                    console.log(JSON.stringify(evt_obj, null, 2));
                });
                util.create_trigger(dbname, cname, trigger_def);
            }
            else {
                console.log('error: null trigger_def');
            } 
        }
    }

    execute_stored_proc(dbname, cname, sprocname) {
        var util = new CosmosDocDbUtil();
        var params = [];
        var options = {};

        console.log('execute_stored_proc: ' + dbname + '/' + cname + '/' + sprocname);

        if (sprocname == 'lookupDoc') {
            params.push(process.argv[6]);
            params.push(process.argv[7]);
            options['partitionKey'] = process.argv[7];
        }
        if (sprocname == 'createHistoryDoc') {
            params.push(process.argv[6]);
            params.push(process.argv[7]);
            options['partitionKey'] = process.argv[7];
        }
        if (sprocname == 'bulkImport') {
            var docs = [];
            docs.push({pk: "u2", name: "bono"});
            docs.push({pk: "u2", name: "edge"});
            docs.push({pk: "u2", name: "adam clayton"});
            docs.push({pk: "u2", name: "larry mullen"});         
            params.push(docs);
            options['partitionKey'] = "u2";
        }
        if (sprocname == 'bulkDelete') {
            var docs = [];
            docs.push({id: "NRT"});
            docs.push({id: "SVO"});
            docs.push({id: "PEK"});      
            params.push(docs);
            options['partitionKey'] = "";
        }
        if (sprocname == 'upsertAirportDoc') {
            var givendoc = {};
            givendoc['pk'] = 'CLT';
            givendoc['name'] = 'Charlotte Douglas Intl'; 
            givendoc['temperature'] = 39;
            givendoc['humidity']    = 88;
            givendoc['new_thing']   = 888;

            params.push(givendoc);
            options['partitionKey'] = "CLT";
        }

        util.on('done', (evt_obj) => {
            console.log(JSON.stringify(evt_obj, null, 2));
        });
        util.execute_stored_proc(dbname, cname, sprocname, params, options);
    }

    query_db(dbname, collname, specfile, xpartition, outfile) {
        var db_util = new CosmosDocDbUtil();
        var coll_link  = 'dbs/' + dbname + '/colls/' + collname;
        var query_json = fs.readFileSync(specfile).toString();
        var query_spec = JSON.parse(query_json);

        db_util.on('done', (evt_obj) => {
            var jstr = JSON.stringify(evt_obj, null, 2);
            fs.writeFileSync(outfile, jstr, 'utf8');
            console.log('file written: ' + outfile);
        });
        db_util.query_documents(coll_link, query_spec, xpartition);
    }

    read_by_id(dbname, collname, doc_id) {
        var db_util = new CosmosDocDbUtil();
        db_util.on('done', (evt_obj) => {
            var jstr = JSON.stringify(evt_obj, null, 2);
            console.log(jstr);
        });
        db_util.read_by_id(dbname, collname, doc_id);
    }

    delete_docs(dbname, collname, query_results_file) {
        this.dbname = dbname;
        this.collname = collname;
        this.db_util = new CosmosDocDbUtil();
        var jstr = fs.readFileSync(query_results_file).toString();
        var obj = JSON.parse(jstr);
        this.docs_to_delete = obj['results'];
        this.delete_index = 0;

        this.db_util.on('done', (evt_obj) => {
            console.log('evt_obj: ' + JSON.stringify(evt_obj, null, 2));
            this.delete_index++;
            this.delete_next_doc();
        });
        this.delete_next_doc();  

    }

    delete_next_doc() {
        if (this.delete_index < this.docs_to_delete.length) {
            var doc = this.docs_to_delete[this.delete_index];
            var id = doc['id'];
            var opts = {};
            opts['enableCrossPartitionQuery'] = true;
            opts['partitionKey'] = doc['pk'];
            console.log('deleting doc: ' + id);
            this.db_util.delete_document(this.dbname, this.collname, id, opts);   
        }
        else {
            console.log('deletion loop complete');
        }
    }

    query_postalcode_by_pkey_and_id(dbname, collname, pkey, code, count) {
        var util = new CosmosDocDbUtil();
        var db_link = 'dbs/' + dbname;
        var coll_link = db_link + '/colls/' + collname;
        var query_spec = {};
        var query = "SELECT * FROM c WHERE c.pkey = @pkey AND c.id = @id";
        query_spec['query'] = query;
        query_spec['parameters'] = [{name: '@pkey', value: pkey}, {name: '@id', value: code}];
        console.log('query_postalcode_by_pkey_and_id:')
        console.log('coll_link:  ' + coll_link);
        console.log('query_spec: ' + JSON.stringify(query_spec, null, 2));
        var n = 0;
        util.on('done', (evt_obj) => {
            console.log(JSON.stringify(evt_obj, null, 2));
            n = n + 1;
            if (n < count) {
                util.query_documents(coll_link, query_spec);
            }
        });
        util.query_documents(coll_link, query_spec);
    }

    query_postalcodes_list(count) {
        var dbname   = 'dev';
        var collname = 'postalcodes';
        this.db_link = 'dbs/' + dbname;
        this.coll_link = this.db_link + '/colls/' + collname;
        console.log('query_postalcodes_list: ' + count);
        console.log('coll_link: ' + this.coll_link);

        var csvfile = 'data/postal_codes_nc.csv'
        this.csvlines = fs.readFileSync(csvfile).toString().split("\n");
        console.log('csv lines read: ' + this.csvlines.length);

        this.curridx = 0;
        this.maxidx = this.csvlines.length;
        if (count < this.maxidx) {
            this.maxidx = count;
        }

        this.docDbUtil = new CosmosDocDbUtil();
        this.docDbUtil.on('done', (evt_obj) => {
            console.log(JSON.stringify(evt_obj, null, 2));
            this.curridx = this.curridx + 1;
            if (this.curridx < this.maxidx) {
                this.read_next_postalcode();
            }
        });
        this.read_next_postalcode();
    }

    read_next_postalcode() {
        var tokens  = this.csvlines[this.curridx].trim().split('|');
        var zipcode = tokens[1];
        var city    = tokens[3];

        var query_spec = {};
        var query = "SELECT * FROM c WHERE c.pkey = @pkey AND c.id = @id";
        query_spec['query'] = query;
        query_spec['parameters'] = [{name: '@pkey', value: city}, {name: '@id', value: zipcode}];
        //console.log('query_spec: ' + JSON.stringify(query_spec, null, 2));

        this.docDbUtil.query_documents(this.coll_link, query_spec);
    }

    query_zipcodes_list(count) {
        var dbname   = 'dev';
        var collname = 'zipcodes';
        this.db_link = 'dbs/' + dbname;
        this.coll_link = this.db_link + '/colls/' + collname;
        console.log('query_zipcodes_list: ' + count);
        console.log('coll_link: ' + this.coll_link);

        var csvfile = 'data/postal_codes_nc.csv'
        this.csvlines = fs.readFileSync(csvfile).toString().split("\n");
        console.log('csv lines read: ' + this.csvlines.length);

        this.curridx = 0;
        this.maxidx = this.csvlines.length;
        if (count < this.maxidx) {
            this.maxidx = count;
        }

        this.docDbUtil = new CosmosDocDbUtil();
        this.docDbUtil.on('done', (evt_obj) => {
            console.log(JSON.stringify(evt_obj, null, 2));
            this.curridx = this.curridx + 1;
            if (this.curridx < this.maxidx) {
                this.read_next_postalcode();
            }
        });
        this.read_next_zipcode();
    }

    read_next_zipcode() {
        var tokens  = this.csvlines[this.curridx].trim().split('|');
        var zipcode = tokens[1];

        var query_spec = {};
        var query = "SELECT * FROM c WHERE c.id = @id";
        query_spec['query'] = query;
        query_spec['parameters'] = [{name: '@id', value: zipcode}];
        //console.log('query_spec: ' + JSON.stringify(query_spec, null, 2));

        this.docDbUtil.query_documents(this.coll_link, query_spec);
    }
}

new Main().execute();

