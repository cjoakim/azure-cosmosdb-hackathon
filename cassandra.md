# azure-cosmosdb-hackathon

## Challenge 9 - Cassandra


Use your favorite workstation OS and programming language for this challenge

## Processing

- Use file **data/major_airports.json** as the input
- Load all rows and non-nested fields (i.e. - exclude location) for each airport.
- You can use the following CQL to create the hackathon.major_airports table

```
(iata_code text, city text, country text, name text, rank int, passengers int, tz_num float, tz_code text, latitude float, longitude float, altitude float, PRIMARY KEY (iata_code)) 
```

## Questions

- Query the number of documents in the table.
  How many documents are there?

- Query the table where iata_code is 'PHX' and display the row.
  What is the altitude of PHX?

- If you have cassandra installed on your workstation, use the cqlsh program
  to connect to your CosmosDB/Cassandra database and query it.


## Links


