# azure-cosmosdb-hackathon

## Challenge 1 - Port Relational and GPS Data to CosmosDB/SQL

## Processing

- Use database export file **data/openflights_airports.csv** as the input
- This file contains thousands of Airport rows, keep only valid fully-populated records
- Filter the rows to include only the United States, the United Kingdom, and Japan
- Load the filtered Airport documents into a CosmosDB/SQL API collection
- Ensure that the GPS information for each Airport document is in GeoJSON format
- Ensure that queries by IATA Code are fast and efficient
- Use your favorite workstation OS and programming language

## Questions

- Query the number of documents in the collection.
  How many documents are there?

- What is the approximate RU cost (RequestCharge) to insert each document into the DB?

- Query the collection efficiently by the IATA Code **CLT** (Charlotte, NC)
  and display the resulting document(s).  What is the altitude of CLT airport?

- Using native CosmosDB database functionality, query the airports that are within
  40 kilometers of GPS location 35.499235 (latitude) and -80.848469 (longitude).
  How many airports are there and what are their names?

## Links

- http://geojson.org
- https://docs.microsoft.com/en-us/azure/cosmos-db/geospatial
- https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-sql-query
