#!/bin/bash

# Use the Azure CLI (az) to create a database and a collection
# in your Azure CosmosDB with Mongo API.
# Chris Joakim, Microsoft, 2019/04/13

source common_env.sh

pk0='/iata_code'
pk="/'\$v'/iata_code/'\$v'" # https://blog.olandese.nl/2017/12/13/create-a-sharded-mongodb-in-azure-cosmos-db/
tp=10000

# You'll need to run these two commands before creating the collections:
# az login
# az account set -s $AZURE_SUBSCRIPTION_ID

echo $AZURE_MONGODB_ACCT
echo $AZURE_MONGODB_DB
echo $AZURE_MONGODB_COLL1

az cosmosdb database create \
    -n $AZURE_MONGODB_ACCT    \
    -d $AZURE_MONGODB_DB      \
    --key $AZURE_MONGODB_PASS

az cosmosdb collection create \
    -n $AZURE_MONGODB_ACCT    \
    -d $AZURE_MONGODB_DB      \
    -c $AZURE_MONGODB_COLL    \
    --throughput $tp          \
    --key $AZURE_MONGODB_PASS
                        