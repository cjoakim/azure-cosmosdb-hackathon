'use strict';

// This file executes a test of the 'upsertAirportDoc' stored-procedure
// using the world_airports_50.json data file.
// Chris Joakim, Microsoft, 2019/04/14

// Command-line use:
// node airport_sproc_test.js test <db> <coll> <upsert-count> <sleep-ms> <change-randomness>
// node airport_sproc_test.js test dev airports 200 300 0.20 > tmp/test.txt
// node airport_sproc_test.js query_db dev airports sql/query_all_docs.json xpart tmp/all_docs.json

const events = require('events');
const fs     = require('fs');
const util   = require('util');
const uuidv4 = require('uuid/v4');
const sleep  = require('sleep');

const airports_data = require('./data/world_airports_50');

const CosmosDocDbUtil = require('./cosmos_docdb_util').CosmosDocDbUtil;


class Main {

    constructor() {
        this.db_util   = null;
        this.client    = null;
        this.db_link   = null;
        this.airports  = [];
        for (var i = 0; i < airports_data.length; i++) {
            this.airports.push(airports_data[i]);
        }
    }

    execute() {

        if (process.argv.length < 3) {
            console.log('error: too few command-line args provided.');
            process.exit();
        }
        else {
            var funct = process.argv[2];

            switch (funct) {

                case 'test':
                    var dbname = process.argv[3];
                    var collname = process.argv[4];
                    var upsert_count = Number(process.argv[5]);
                    var sleep_ms     = Number(process.argv[6]);
                    var randomness   = Number(process.argv[7]);
                    this.test(dbname, collname, upsert_count, sleep_ms, randomness);
                    break;

                case 'query_db':
                    var dbname   = process.argv[3];
                    var collname = process.argv[4];
                    var specfile = process.argv[5];
                    var xpartition = process.argv[6] === 'xpart' ? true : false;
                    var outfile  = process.argv[7];
                    this.query_db(dbname, collname, specfile, xpartition, outfile);
                    break;

                default:
                    console.log('error: unknown function - ' + funct);
            }
        }
    }

    test(dbname, collname, upsert_count, sleep_ms, randomness) {
        console.log(util.format('test; db: %s coll: %s uc: %s ss: %s rp: %s',
            dbname, collname, upsert_count, sleep_ms, randomness));

        this.dbname = dbname;
        this.collname = collname;
        this.max_upserts = upsert_count;
        this.actual_upserts = 0;
        this.sleep_ms = sleep_ms;
        this.randomness = randomness;
        this.db_util = new CosmosDocDbUtil();
        this.airports_count = this.airports.length;
        this.airports_index = -1;
        this.load_count = 0;
        this.random_yes = 0;
        this.random_no  = 0;
        this.sp_diff_expected = 0;

        // see http://metaduck.com/01-asynchronous-iteration-patterns.html

        this.db_util.on('done', (evt_obj) => {
            var msg = 'OK ';
            var iata          = evt_obj['results']['iata_code']; 
            var sp_diff       = evt_obj['results']['__sp_diff'];
            var sp_diffs      = evt_obj['results']['__sp_diffs'];
            var sp_updated_at = evt_obj['results']['__sp_updated_at'];

            if (sp_diff != this.sp_diff_expected) {
                msg = 'ERR';
            }
            console.log('evt_obj: ' + JSON.stringify(evt_obj, null, 2));
            console.log(util.format('evt_obj_results; %s %s iata: %s xdiff: %s sp_diff: %s sp_updated_at %s sp_diffs %s ',
                msg, this.actual_upserts, iata, this.sp_diff_expected, sp_diff, sp_updated_at, JSON.stringify(sp_diffs)));

            sleep.msleep(this.sleep_ms);
            this.upsert_next_doc();
        });
        this.upsert_next_doc();
    }

    upsert_next_doc() {
        this.actual_upserts++;
        console.log('==========');

        if (this.actual_upserts < this.max_upserts) {
            this.airports_index++;
            if (this.airports_index >= this.airports_count) {
                this.airports_index = 0;
            }
            var airport = this.airports[this.airports_index];
            airport['id'] = airport['iata_code'];
            airport['pk'] = airport['iata_code'];
            this.sp_diff_expected = 0;
            //console.log("executing stored proc for: " + airport['pk']);

            if (this.actual_upserts <= this.airports_count) {
                // first, unconditionally add ALL of the airports to the DB
                console.log('UPSERT_LOADING: ' + airport['iata_code'] + ' ' + this.actual_upserts);
                this.add_random_values(airport, true);
            }
            else {
                // Randomly change the values in the airport, or leave it unchanged.
                // The stored-proc should detect if the doc has changed or not.
                var r = Math.random(); 
                if (r < this.randomness) {
                    this.add_random_values(airport, false);
                    this.random_yes++;
                    this.sp_diff_expected = 1;
                    console.log('UPSERT_UPDATING: ' + airport['iata_code'] + ' ' + this.actual_upserts);
                } 
                else {
                    this.random_no++;
                    console.log('UPSERT_UNCHANGED: ' + airport['iata_code'] + ' ' + this.actual_upserts);
                }
            }
            // put the updated airport back into the array for the following iterations
            this.airports[this.airports_index] = airport; 

            // create params and options to be passed to the stored-proc
            var params = [];
            var options = {};
            var sprocname = 'upsertAirportDoc';
            params.push(airport);
            options['partitionKey'] = airport['pk'];
            this.db_util.execute_stored_proc(this.dbname, this.collname, sprocname, params, options);
        }
        else {
            console.log('---');
            console.log('processing completed');
            var outfile = 'tmp/airports_array.json';
            var jstr = JSON.stringify(this.airports, null, 2);
            fs.writeFileSync(outfile, jstr, 'utf8');
            console.log('file written:   ' + outfile);
            console.log('dbname:         ' + this.dbname);
            console.log('collname:       ' + this.collname);
            console.log('randomness:     ' + this.randomness);
            console.log('sleep_ms:       ' + this.sleep_ms);
            console.log('max_upserts:    ' + this.max_upserts);
            console.log('actual_upserts: ' + this.actual_upserts);
            console.log('random_yes:     ' + this.random_yes);
            console.log('random_no:      ' + this.random_no);
        }
    }

    add_random_values(airport, is_initial) {
        // Generate these random values so the Stored-Proc can detect changes
        var pk    = null;
        var temp  = null;
        var humid = null;
        var rain  = null;

        if (!is_initial) {
            pk    = airport['pk'];
            temp  = airport['temperature'];
            humid = airport['humidity'];
            rain  = airport['rain'];
            console.log(util.format('add_random_values, before: %s %s %s %s', pk, temp, humid, rain));
        }

        temp  = 32.0 + Math.floor(Math.random() * 80) + 1;
        humid = 60.0 + Math.floor(Math.random() * 20) + 1;
        rain  = Math.floor(Math.random() * 2) + 1;
        airport['temperature'] = temp;
        airport['humidity']    = humid;
        airport['rain']        = rain;
        console.log(util.format('add_random_values, after: %s %s %s %s', pk, temp, humid, rain));

        // airport['uuid'] = uuidv4();  // added to ensure differences in the doc instances
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
}

new Main().execute();
