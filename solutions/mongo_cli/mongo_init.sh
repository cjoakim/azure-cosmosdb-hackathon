#!/bin/bash

# Bash script to initialize a Mongo database - either local or azure;
#
# Use:
# $ ./mongo_init.sh local
# $ ./mongo_init.sh azure
#
# Chris Joakim, Microsoft, 2019/04/13

source common_env.sh

if [ "$1" == 'local' ]
then 
    echo 'connecting to '$LOCAL_MONGODB_URL
    mongo $LOCAL_MONGODB_URL < mongo/local_init.ddl
fi

if [ "$1" == 'azure' ]
then 
    echo 'connecting to azure at '$AZURE_COSMOSDB_MONGODB_CONN_STRING
    mongo $AZURE_MONGODB_CONN_STR --ssl < mongo/azure_init.ddl
fi
