#!/bin/bash

# Use the mongo shell commands and the Azure CLI (az) load documents
# into the Azure Cosmos/Mongo DB, then reduce collection throughput.
# Finally open a mongo CLI pointing at your Azure CosmosDB.
# Chris Joakim, Microsoft, 2019/04/24

# set the appropriate environment variables
source common_env.sh

./mongo_import.sh azure_airports

echo 'reducing throughput on '$AZURE_MONGODB_COLL
az cosmosdb collection update \
    -n $AZURE_MONGODB_ACCT    \
    -d $AZURE_MONGODB_DB      \
    -c $AZURE_MONGODB_COLL    \
    --throughput 1000         \
    --key $AZURE_MONGODB_PASS

./mongo_query.sh azure_airports

./mongo_cli.sh azure 
