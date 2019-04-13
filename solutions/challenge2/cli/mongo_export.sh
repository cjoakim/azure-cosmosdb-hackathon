#!/bin/bash

# Bash script to export a Mongo database
# MongoDB paired utilities:
# - mongodump & mongorestore
# - mongoexport & mongoimport 
# Use:
# $ ./mongo_dump.sh local_airports
# $ ./mongo_dump.sh azure_airports
# Chris Joakim, Microsoft, 2019/04/13

# delete the output dump directory and export file
rm -rf dump/
rm tmp/airports.json

source common_env.sh

if [ "$1" == 'local_airports' ]
then 
    echo 'connecting to '$LOCAL_MONGODB_URL
    mongoexport -d $LOCAL_MONGODB_DB -c $LOCAL_MONGODB_COLL --out tmp/airports.json
fi

if [ "$1" == 'azure_airports' ]
then 
    echo 'TODO: dumping azure database...'
fi
