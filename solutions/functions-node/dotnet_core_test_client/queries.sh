#!/bin/bash

# Bash shell script to execute dotnet to query CosmosDB in several ways.
# Chris Joakim, Microsoft, 2019/07/08

fromEpoch=1562600941

rm tmp/*.*

dotnet build

echo 'time_now ...'
dotnet run time_now

echo 'query_cosmos_doc_by_pk_id ...'
dotnet run query_cosmos doc_by_pk_id SYD 047f35b4-7a09-4312-afe1-c44d171606ca > tmp/query_cosmos_doc_by_pk_id.txt

echo 'query_cosmos_all_events ...'
dotnet run query_cosmos all_events $fromEpoch > tmp/query_cosmos_all_events.txt

echo 'query_cosmos_events_for_airport ...'
dotnet run query_cosmos events_for_airport SYD $fromEpoch > tmp/query_cosmos_events_for_airport.txt

echo 'query_cosmos_events_for_city ...'
dotnet run query_cosmos events_for_city Sydney $fromEpoch > tmp/query_cosmos_events_for_city.txt

echo 'query_cosmos_events_for_location ...'
dotnet run query_cosmos events_for_location -80.842842 35.499586 1 $fromEpoch > tmp/query_cosmos_events_for_location.txt

echo 'done'
