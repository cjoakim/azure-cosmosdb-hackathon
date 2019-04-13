# azure-cosmosdb-hackathon

## Port a MongoDB database CosmosDB/SQL

Use your favorite workstation OS and programming language for this challenge

## Processing

- Use database export file **data/mongoexport_airports.json** as the input
  - This file was create by the **mongoexport** program vs an existing database
- Manually create a CosmosDB with SQL API database account in Azure Portal
- Use the Azure CLI to create a **hackathon** database with **airports** collection in the DB
- Use a Partition Key, and set its' value to the IATA Code of each airport
- Write a program to read the mongoexport_airports.json file and load each airport into your CosmosDB/SQL collection
  - Each line in this file is valid JSON
  - Remove any Mongo-specific attributes from each document before loading (i.e. - _id)
- Prefer to use environment variables vs hard-coded values in your scripts

## Questions

- Query the number of documents in the collection.
  How many documents are there?

- Query the database for IATA Code 'ATL' and display the resulting document.

## Links


