#!/bin/bash

# Use:
# $ ./mongo_import.sh local_airports
# $ ./mongo_import.sh azure_airports
# Chris Joakim, Microsoft, 2019/04/13

source common_env.sh

if [ "$1" == 'local_airports' ]
then 
    echo 'connecting to '$LOCAL_MONGODB_URL
    mongoimport --host $LOCAL_MONGODB_HOST --port $LOCAL_MONGODB_PORT -d $LOCAL_MONGODB_DB -c $LOCAL_MONGODB_COLL data/mongoexport_airports.json
fi

if [ "$1" == 'azure_airports' ]
then 
    echo 'connecting to '$AZURE_MONGODB_HOST
    mongoimport --host $AZURE_MONGODB_HOST --port $AZURE_MONGODB_PORT -d $AZURE_MONGODB_DB -c $AZURE_MONGODB_COLL -u $AZURE_MONGODB_USER -p $AZURE_MONGODB_PASS --ssl data/mongoexport_airports.json
fi
