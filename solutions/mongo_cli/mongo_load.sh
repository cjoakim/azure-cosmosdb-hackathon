#!/bin/bash

# Bash script to load a Mongo database - either local or azure
# Use:
# $ ./mongo_load.sh local_airports
# $ ./mongo_load.sh azure_airports
# Chris Joakim, Microsoft, 2019/04/13

source common_env.sh

if [ "$1" == 'local_airports' ]
then 
    echo 'connecting to '$LOCAL_MONGODB_URL
    mongo $LOCAL_MONGODB_URL < mongo/load_airports.ddl
fi

if [ "$1" == 'azure_airports' ]
then 
    echo 'connecting to azure at '$AZURE_COSMOSDB_MONGODB_CONN_STRING
    mongo $AZURE_MONGODB_CONN_STR --ssl < mongo/load_airports.ddl
fi
