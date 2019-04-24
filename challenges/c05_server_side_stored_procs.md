# azure-cosmosdb-hackathon

## Challenge 5 - Server-Side programming with Stored-Procedures

Use your favorite workstation OS and programming language for this challenge.
Node.js is recommended since CosmosDB server-side programming is done in **JavaScript**.

## Processing

- In your CosmosDB/SQL **hackathon** database create a collection named **music_bands** with a partition key named **pk**
- In this collection either manually or programatically create a stored-procedure named **lookupDoc**
  - It should look up a document given both its' id and partition key attributes
- Programatically create a stored-procedure named **bulkImport**
  - It should receive an Array of JSON documents and insert them into the collection
  - It should add an attribute named 'bulk_import_date' to each document with the server-side date
- Invoke the **bulkImport** stored-procedure with this simple array of four documents:
  - {pk: "u2", name: "bono"}
  - {pk: "u2", name: "edge"}
  - {pk: "u2", name: "adam clayton"}
  - {pk: "u2", name: "larry mullen"} 
- Invoke the **lookupDoc** stored-procedure to lookup 'bono' 
- Programatically delete the stored-procedure named 'lookupDoc'
- Programatically delete the stored-procedure named 'bulkImport'

## Questions

- Invoke the 'lookupDoc' stored-procedure for name 'bono'
  What is the 'bulk_import_date' of that document?
  What is the RU charge for that execution of the stored-procedure?

- Programatically delete the stored-procedure named 'lookupDoc'.
  View Azure Portal - is the stored-procedure still there?

- Programatically delete the stored-procedure named 'bulkImport'.
  View Azure Portal - is the stored-procedure still there?

## Links

- https://docs.microsoft.com/en-us/azure/cosmos-db/stored-procedures-triggers-udfs
- https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-write-stored-procedures-triggers-udfs
- https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-use-stored-procedures-triggers-udfs
