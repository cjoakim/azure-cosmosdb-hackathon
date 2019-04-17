#!/bin/bash

# Bash shell script to invoke the Azure CLI program (az) to 
# provision and modify CosmosDB.

# See the Azure CLI installation instructions here:
# https://docs.microsoft.com/en-us/cli/azure/install-azure-cli
# The syntax for the az commands is identical on Windows, Linux, and macOS.
#
# See the Azure CLI documentation for CosmosDB here:
# https://docs.microsoft.com/en-us/cli/azure/cosmosdb?view=azure-cli-latest
#
# Chris Joakim, Microsoft, 2019/04/17

# Define environment variables for this script:
rg_name=cjoakim-cosmos
acct_name=cjoakim-azcli
db_name=dev
coll_name=collection1

dbkind=GlobalDocumentDB
conlevel=Eventual
locations='eastus=0'
dbtags=' purpose[demonstration] created_on[20190417]'
throughput1=10000
throughput2=5000

# Use; typically used in this sequence:
# $ ./provision_sql_db.sh display_help
# $ ./provision_sql_db.sh create_acct
# $ ./provision_sql_db.sh create_db
# $ ./provision_sql_db.sh create_collection
# $ ./provision_sql_db.sh update_collection

if [ "$1" == "display_help" ]
then
    echo 'display_help ...'
    az cosmosdb create --help > help.txt
    echo '==========' >> help.txt
    az cosmosdb database create --help >> help.txt
    echo '==========' >> help.txt
    az cosmosdb collection create --help >> help.txt
    echo '==========' >> help.txt
    az cosmosdb collection update --help >> help.txt
    cat help.txt
fi

if [ "$1" == "create_acct" ]
then
    echo 'create_acct ...'
    az cosmosdb create \
        --name $acct_name \
        --resource-group $rg_name \
        --kind $dbkind \
        --locations $locations \
        --subscription $AZURE_SUBSCRIPTION_ID \
        --tags $dbtags
fi

if [ "$1" == "create_db" ]
then
    echo 'create_db ...'
    az cosmosdb database create \
        --db-name $db_name \
        --name $acct_name \
        --resource-group $rg_name \
        --subscription $AZURE_SUBSCRIPTION_ID 
fi

if [ "$1" == "create_collection" ]
then
    echo 'create_collection ...'
    az cosmosdb collection create \
        --collection-name $coll_name \
        --db-name $db_name \
        --name $acct_name \
        --partition-key-path "/pk" \
        --resource-group-name $rg_name \
        --subscription $AZURE_SUBSCRIPTION_ID \
        --throughput $throughput1
fi

if [ "$1" == "update_collection" ]
then
    echo 'update_collection ...'
    az cosmosdb collection update \
        --collection-name $coll_name \
        --db-name $db_name \
        --name $acct_name \
        --resource-group $rg_name \
        --subscription $AZURE_SUBSCRIPTION_ID \
        --throughput $throughput2
fi

echo 'done'
