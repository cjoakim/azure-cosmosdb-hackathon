# azure-cosmosdb-hackathon

## Challenge 5 - Server-Side programming with Stored Procedures

Use your favorite workstation OS and programming language for this challenge

## Processing

- Use a CosmosDB/SQL database collection previously created in this Hackathon, such as in Challenge 1
- Either manually or programatically create a stored procedure named 'lookupDoc'
  - It should look up a document given both its' id and partition key attributes
- Programatically create a stored procedure named 'bulkImport'
  - It should receive an Array of JSON documents and insert them into the collection
  - It should add an attribute named 'bulk_import_date' to each document with the server-side date
- Invoke the 'bulkImport' stored procedure with the contents of file **data/world_airports_50.json**
- In Azure Portal, query the bulk-imported airport with IATA Code 'SYD' and capture its' value
- Programatically delete the stored procedure named 'lookupDoc'
- Programatically delete the stored procedure named 'bulkImport'

## Questions

- Query the airport with IATA Code 'SYD', using  stored procedure 'lookupDoc', and display it.
  What is the value of its' 'bulk_import_date' attribute?


## Links

- https://docs.microsoft.com/en-us/azure/cosmos-db/stored-procedures-triggers-udfs
- https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-write-stored-procedures-triggers-udfs
- https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-use-stored-procedures-triggers-udfs
