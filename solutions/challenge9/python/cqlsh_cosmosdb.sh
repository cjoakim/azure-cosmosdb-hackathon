#!/bin/bash

# Bash script to open a Cassandra cqlsh shell connected to CosmosDB.
# Chris Joakim, Microsoft, 2019/04/13

CQLSH_HOST=$AZURE_COSMOSDB_CASSDB_URI
CQLSH_PORT=$AZURE_COSMOSDB_CASSDB_PORT
export CQLSH_HOST
export CQLSH_PORT

export SSL_VERSION=TLSv1_2
export SSL_VALIDATE=false

echo 'connecting to '$CQLSH_HOST
# echo $CQLSH_PORT
# echo $AZURE_COSMOSDB_CASSDB_ACCT
# echo $AZURE_COSMOSDB_CASSDB_PASS

export SSL_VERSION=TLSv1_2
export SSL_VALIDATE=false

# Execute cqlsh.py in your exact local installation directory:
/usr/local/Cellar/cassandra/3.11.4/bin/cqlsh.py $AZURE_COSMOSDB_CASSDB_URI 10350 -u $AZURE_COSMOSDB_CASSDB_ACCT -p $AZURE_COSMOSDB_CASSDB_PASS --ssl --no-color

# Previous installation directories:
#/usr/local/Cellar/cassandra/3.11.3_1/bin/cqlsh.py $AZURE_COSMOSDB_CASSDB_URI 10350 -u $AZURE_COSMOSDB_CASSDB_ACCT -p $AZURE_COSMOSDB_CASSDB_PASS --ssl
#/usr/local/Cellar/cassandra/3.11.2_1/bin/cqlsh.py $AZURE_COSMOSDB_CASSDB_URI 10350 -u $AZURE_COSMOSDB_CASSDB_ACCT -p $AZURE_COSMOSDB_CASSDB_PASS --ssl
