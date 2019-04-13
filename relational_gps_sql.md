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

- Query the collection efficiently by the IATA Code **CLT** (Charlotte, NC)
  and display the resulting document(s).  What is the altitude of CLT airport?

- What is the RU cost to query the DB by an IATA Code?

- Using native CosmosDB database functionality, query the airports that are within
  40 kilometers of GPS location 35.499235 (latitude) and -80.848469 (longitude).
  How many airports are there and what are their names?

## Links

- See http://geojson.org
- See https://docs.microsoft.com/en-us/azure/cosmos-db/geospatial
