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

# az cosmosdb create --name
#                    --resource-group
#                    [--capabilities]
#                    [--default-consistency-level {BoundedStaleness, ConsistentPrefix, Eventual, Session, Strong}]
#                    [--enable-automatic-failover {false, true}]
#                    [--enable-multiple-write-locations {false, true}]
#                    [--enable-virtual-network {false, true}]
#                    [--ip-range-filter]
#                    [--kind {GlobalDocumentDB, MongoDB, Parse}]
#                    [--locations]
#                    [--max-interval]
#                    [--max-staleness-prefix]
#                    [--subscription]
#                    [--tags]
#                    [--virtual-network-rules]


rg=cjoakim-cosmos
kind=GlobalDocumentDB
clevel1=Eventual



echo 'done'
