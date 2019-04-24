# azure-cosmosdb-hackathon

## Challenge 3 Solution with Python - Port a MongoDB database CosmosDB/SQL

The solution code is in directory **solutions/python/** in this repository.

This solution uses a dataset containing 1459 US Airports.  Each line of this
file contains a valid JSON document exported from a MongoDB database with the 
**mogoexport** program.

See file **solutions/python/data/mongoexport_airports.json**


### Azure Portal

In Azure Portal, create a CosmosDB with SQL API Account.

Then create a **hackathon** database within it.

Then create collection **airports3** within the hackathon database.

### Python

Create and activate the python virtual library with the necessary libraries:
See files **requirements.in** and **venv.sh**
```
./venv.sh
source bin/activate
python --version
```

Then load the collection with Python:
```
python challenge3.py load_azure_sql_collection hackathon airports3 data/mongoexport_airports.json
```

Then query the number of documents in the collection:
```
python challenge3.py count_docs_in_collection hackathon airports3
```

Then query for the airport with the iata code 'ATL':
```
python challenge3.py query_by_iata_code hackathon airports3 ATL
```
