#!/bin/bash

# Bash script using the Azure CLI to get your CosmosDB account credentials.
# See https://pypi.org/project/azure-cosmos/
# Chris Joakim, Microsoft, 2019/11/28

RES_GROUP=cjoakim-cosmos
ACCT_NAME=cjoakimcosmosdbsql

# az login   # run this first

az cosmosdb show --resource-group $RES_GROUP --name $ACCT_NAME --query documentEndpoint
az cosmosdb keys list --resource-group $RES_GROUP --name $ACCT_NAME --query primaryMasterKey

# Alternatively, export these values as environment variables:
# export ACCOUNT_URI=$(az cosmosdb show --resource-group $RES_GROUP --name $ACCT_NAME --query documentEndpoint --output tsv)
# export ACCOUNT_KEY=$(az cosmosdb keys list --resource-group $RES_GROUP --name $ACCT_NAME --query primaryMasterKey --output tsv)
