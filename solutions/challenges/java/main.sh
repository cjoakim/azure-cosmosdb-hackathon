#!/bin/bash

# Ad-hoc Java program execution with mvn.
# Chris Joakim, Microsoft, 2018/11/07

mvn clean compile

class="com.chrisjoakim.azure.cosmosdb.CosmosDbUtil"

# mvn exec:java -Dexec.mainClass="com.chrisjoakim.azure.cosmosdb.CosmosDbUtil" -Dexec.args="deleteDoc dev airports ATL c190a1cf-a4e8-4449-a142-1220f9cd316d"
# mvn exec:java -Dexec.mainClass="com.chrisjoakim.azure.cosmosdb.CosmosDbUtil" -Dexec.args="queryDocs dev airports 100"
# mvn exec:java -Dexec.mainClass="com.chrisjoakim.azure.cosmosdb.CosmosDbUtil" -Dexec.args="loadOpenFlightsAirports dev airports"

mvn exec:java -Dexec.mainClass="com.chrisjoakim.azure.cosmosdb.CosmosDbUtil" -Dexec.args="queryDocs dev airports sql/charlotte_nearby.sql"

echo 'done'
