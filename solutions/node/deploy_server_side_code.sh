#!/bin/bash

# Delete all documents in the dev/airports collection, 
# then deploy server-side code JavaScript code to CosmosDB.
#
# Chris Joakim, Microsoft, 2019/04/14

rm tmp/*.*

echo 'querying all current docs in the airports collection ...'
node admin.js query_db dev airports sql/query_all_docs.json xpart tmp/all_docs_original.json

echo 'deleting all current docs in the airports collection ...'
node admin.js delete_docs dev airports tmp/all_docs_original.json

echo 'deleting the stored proc ...'
node admin.js delete_stored_proc dev airports upsertAirportDoc
sleep 3

echo 'deploying the stored proc ...'
node admin.js create_stored_proc dev airports upsertAirportDoc create
sleep 3

echo 'done'
