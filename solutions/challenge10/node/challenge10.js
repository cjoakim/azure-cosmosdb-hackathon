'use strict';

// This code was created based on the following link:
// https://docs.microsoft.com/en-us/rest/api/cosmos-db/access-control-on-cosmosdb-resources
//
// Example command-line use:
// $ node main.js get_document_by_pk_and_id hackathon airports3 LAX 4edc3477-fd20-4dff-975d-123a11adf192
//
// Chris Joakim, Microsoft, 2019/04/13

const crypto  = require("crypto"); 
const request = require('request');

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

                case 'get_document_by_pk_and_id':
                    var db   = process.argv[3];
                    var coll = process.argv[4];
                    var pkey = process.argv[5];
                    var id   = process.argv[6];
                    var link = 'dbs/' + db + '/colls/' + coll + '/docs/' + id;
                    var creds = this.gen_request_credentials('get', 'docs', link);
                    console.log(JSON.stringify(creds, null, 2));

                    var base_uri = process.env.AZURE_COSMOSDB_SQLDB_URI;
                    var full_uri = base_uri + link;

                    var options = {};
                    var headers = {};
                    headers['Authorization'] = creds['hmac'];
                    headers['x-ms-date']     = creds['date'];
                    headers['x-ms-version']  = creds['vers'];
                    headers['x-ms-documentdb-partitionkey'] = JSON.stringify([pkey]);
                    //headers['x-ms-query-enable-crosspartition']  = true;
                    console.log("headers:\n" + JSON.stringify(headers, null, 2));

                    options['method'] = 'GET';
                    options['url'] = full_uri;
                    options['headers'] = headers;
                    console.log("request options:\n" + JSON.stringify(options, null, 2));
                    
                    request(options, function(err, res, body) { 
                        console.log('response data:');
                        console.log(JSON.stringify(res, null, 2));
                        console.log('response body:');
                        console.log(JSON.parse(body));
                    });
                    break;

                default:
                    console.log('error: unknown function - ' + funct);
            }
        }
    }

    gen_request_credentials(http_verb, resource_type, resource_link) {
        // Return a creds object containing the 'date' and 'hmac' keys.
        var creds = {};
        var now  = new Date();
        var date = now.toUTCString(); // Format: 'Sat, 15 Dec 2018 15:46:55 GMT'
        console.log("date: " + date + " -> " + now.getTime());

        creds['date'] = date;  // passed as 'x-ms-date' header in the HTTP request.

        // The primary or secondary key for the CosmosDB account; see your Azure Portal
        var master_key  = process.env.AZURE_COSMOSDB_SQLDB_KEY;
        var key  = new Buffer(master_key, "base64");

        var text = (http_verb || "").toLowerCase() + "\n" +   
                   (resource_type || "").toLowerCase() + "\n" +   
                   (resource_link || "") + "\n" +   
                   date.toLowerCase() + "\n\n"; 
        console.log('message below:');
        console.log(text);
        console.log('message above:');

        var body = new Buffer(text, "utf8");  
        var signature = crypto.createHmac("sha256", key).update(body).digest("base64");  
        var master_token = "master";  
        var token_version = "1.0";
        creds['signature'] = signature;

        // Generate the hash message authentication code (HMAC)
        var hmac = encodeURIComponent("type=" + master_token + "&ver=" + token_version + "&sig=" + signature);
        creds['hmac'] = hmac;
        creds['vers'] = '2017-02-22';
        return creds
    }
}

new Main().execute();
