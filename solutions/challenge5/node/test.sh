#!/bin/bash

# This script executes a test of the 'upsertAirportDoc' stored proc
#Chris Joakim, Microsoft, 2019/04/14
# notes:
# optionally, in another terminal: azure-cosmosdb-samples/python]$ python main.py truncate_collection hackathon airports5
# in another terminal: cat tmp/test.txt | grep evt_obj_results

rm tmp/*.*

echo 'querying all current docs in the airports5 collection ...'
node admin.js query_db hackathon airports5 sql/query_all_docs.json xpart tmp/all_docs_original.json

echo 'deleting all current docs in the airports5 collection ...'
node admin.js delete_docs hackathon airports5 tmp/all_docs_original.json

echo 'deleting the stored proc ...'
node admin.js delete_stored_proc hackathon airports5 upsertAirportDoc
sleep 3

echo 'deploying the stored proc ...'
node admin.js create_stored_proc hackathon airports5 upsertAirportDoc create
sleep 3

echo 'loading and updating the airports5 collection with the stored proc ...'
iterations=300
sleepms=200
randomness=0.33
node airport_sproc_test.js test hackathon airports5 $iterations $sleepms $randomness > tmp/test.txt

echo 'grepping for CLT and error results ...'
cat tmp/test.txt | grep evt_obj_results | grep CLT
cat tmp/test.txt | grep evt_obj_results | grep ERR

echo 'querying all airports5 ...'
node admin.js query_db hackathon airports5 sql/query_all_docs.json xpart tmp/all_docs_final.json

echo 'querying CLT ...'
node admin.js query_db hackathon airports5 sql/query_clt.json xpart tmp/clt.json

cat tmp/clt.json

echo ''
echo 'test completed'
