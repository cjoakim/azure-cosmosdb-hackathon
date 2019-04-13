#!/bin/bash

# Bash script to open a mongo CLI pointing to either localhost or Azure.
# Chris Joakim, Microsoft, 2019/04/13

source common_env.sh

if [ "$1" == 'local' ]
then 
    echo 'connecting to '$LOCAL_MONGODB_URL
    mongo $LOCAL_MONGODB_URL
fi

if [ "$1" == 'azure' ]
then 
    echo 'connecting to: '$AZURE_MONGODB_CONN_STR
    mongo $AZURE_MONGODB_CONN_STR --ssl
fi
