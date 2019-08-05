#!/bin/bash

# Bash shell script to execute dotnet to query CosmosDB in several ways.
# Chris Joakim, Microsoft, 2019/08/05

fromEpoch=0
MIA_ID="6d61f28f-a796-45ea-8025-355e4a35bd39"

rm tmp/query*.*

dotnet build

echo 'time_now ...'
dotnet run time_now

echo 'query_cosmos_doc_by_pk_id ...'
dotnet run query_cosmos doc_by_pk_id MIA 6d61f28f-a796-45ea-8025-355e4a35bd39 > tmp/query_cosmos_doc_by_pk_id.txt

echo 'query_cosmos_all_events ...'
dotnet run query_cosmos all_events $fromEpoch > tmp/query_cosmos_all_events.txt

echo 'query_cosmos_events_for_airport MIA and MAD ...'
dotnet run query_cosmos events_for_airport MIA $fromEpoch > tmp/query_cosmos_events_for_airport_MIA.txt
dotnet run query_cosmos events_for_airport MAD $fromEpoch > tmp/query_cosmos_events_for_airport_MAD.txt

echo 'query_cosmos_events_for_city Miami and Madrid ...'
dotnet run query_cosmos events_for_city Miami  $fromEpoch > tmp/query_cosmos_events_for_city_Miami.txt
dotnet run query_cosmos events_for_city Madrid $fromEpoch > tmp/query_cosmos_events_for_city_Madrid.txt

echo 'query_cosmos_events_for_location ...'
dotnet run query_cosmos events_for_location -80.290556 25.79325  1 $fromEpoch > tmp/query_cosmos_events_for_location.txt

echo 'done'
