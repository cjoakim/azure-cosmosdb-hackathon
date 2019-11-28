#!/bin/bash

# bash script to execute challenge 3 in python
# Chris Joakim, Microsoft, 2019/11/28

python challenge3.py list_databases

python challenge3.py list_collections dev

python challenge3.py count_docs_in_collection dev airports

python challenge3.py load_azure_sql_collection dev airports data/mongoexport_airports.json --to-numerics

python challenge3.py count_docs_in_collection dev airports

python challenge3.py query_by_iata_code dev airports3 ATL

echo 'done'
