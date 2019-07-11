#!/bin/bash

# This script executes a test of the 'upsertAirportDoc' stored proc
# Chris Joakim, Microsoft, 2019/07/11
# notes:
# optionally, in another terminal: azure-cosmosdb-samples/python]$ python main.py truncate_collection dev airports
# in another terminal: cat tmp/test.txt | grep evt_obj_results

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

echo 'loading and updating the airports collection with the stored proc ...'
iterations=200
sleepms=50
randomness=0.60
node airport_sproc_test.js test dev airports $iterations $sleepms $randomness > tmp/test.txt

echo 'grepping for ATL, CLT, and error results ...'
cat tmp/test.txt | grep cache_refresh_result | grep ATL
cat tmp/test.txt | grep cache_refresh_result | grep CLT
cat tmp/test.txt | grep cache_refresh_result | grep ERR

echo 'querying all airports ...'
node admin.js query_db dev airports sql/query_all_docs.json xpart tmp/all_docs_final.json

echo 'querying ATL ...'
node admin.js query_db dev airports sql/query_atl.json xpart tmp/atl.json

echo 'querying CLT ...'
node admin.js query_db dev airports sql/query_clt.json xpart tmp/clt.json

#cat tmp/clt.json

echo ''
echo 'test completed'
