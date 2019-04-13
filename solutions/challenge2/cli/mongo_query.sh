#!/bin/bash

# Bash script to query a Mongo database - either local or azure
# Use:
# $ ./mongo_query.sh local_airports
# $ ./mongo_query.sh azure_airports
# Chris Joakim, Microsoft, 2019/04/13

source common_env.sh

if [ "$1" == 'local_airports' ]
then 
    echo 'connecting to '$LOCAL_MONGODB_URL
    mongo $LOCAL_MONGODB_URL < mongo/query_local_airports.ddl
fi

if [ "$1" == 'azure_airports' ]
then 
    echo 'connecting to azure at, airports_f: '$AZURE_COSMOSDB_MONGODB_CONN_STRING
    mongo $AZURE_MONGODB_CONN_STR --ssl < mongo/query_azure_airports_f.ddl

    echo 'connecting to azure at, airports_p: '$AZURE_COSMOSDB_MONGODB_CONN_STRING
    mongo $AZURE_MONGODB_CONN_STR --ssl < mongo/query_azure_airports_p.ddl
fi
