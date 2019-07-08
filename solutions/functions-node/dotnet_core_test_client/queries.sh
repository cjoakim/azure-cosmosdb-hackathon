#!/bin/bash

# Bash shell script to execute dotnet to query CosmosDB in several ways.
# Chris Joakim, Microsoft, 2019/07/08

fromEpoch=0

rm tmp/query*.*

dotnet build

echo 'time_now ...'
dotnet run time_now

echo 'query_cosmos_doc_by_pk_id ...'
dotnet run query_cosmos doc_by_pk_id SYD e2c0316e-547c-472c-bcfd-60294dd68a0c > tmp/query_cosmos_doc_by_pk_id.txt

echo 'query_cosmos_all_events ...'
dotnet run query_cosmos all_events $fromEpoch > tmp/query_cosmos_all_events.txt

echo 'query_cosmos_events_for_airport SYD and CLT ...'
dotnet run query_cosmos events_for_airport SYD $fromEpoch > tmp/query_cosmos_events_for_airport_SYD.txt
dotnet run query_cosmos events_for_airport CLT $fromEpoch > tmp/query_cosmos_events_for_airport_CLT.txt

echo 'query_cosmos_events_for_city Sydney and Charlotte ...'
dotnet run query_cosmos events_for_city Sydney  $fromEpoch > tmp/query_cosmos_events_for_city_Sydney.txt
dotnet run query_cosmos events_for_city Charlotte $fromEpoch > tmp/query_cosmos_events_for_city_Charlotte.txt

echo 'query_cosmos_events_for_location ...'
dotnet run query_cosmos events_for_location -80.842842 35.499586 1 $fromEpoch > tmp/query_cosmos_events_for_location.txt

echo 'done'
