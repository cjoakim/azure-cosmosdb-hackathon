# azure-cosmosdb-hackathon

## Challenge 2 with CLI Tools

### Execute the Solution

Use the Azure CLI to create the database and collections
```
./create_azure_collections.sh
```

Then load the Azure database, reduce DB throughput, and open a
mongo CLI session pointing at your Azure Mongo database.
```
./load_azure.sh
```

### Sample Results

```
globaldb:PRIMARY> show dbs
hackathon  0.000GB
globaldb:PRIMARY> use hackathon
switched to db hackathon
globaldb:PRIMARY> show collections
airports
globaldb:PRIMARY> db.airports.count()
NumberLong(1459)
globaldb:PRIMARY> db.airports.find({iata_code:"ATL"}).pretty()
{
	"_id" : ObjectId("5cb21be890d09ce938a7b4e5"),
	"name" : "Hartsfield Jackson Atlanta Intl",
	"city" : "Atlanta",
	"country" : "United States",
	"iata_code" : "ATL",
	"latitude" : "33.636719",
	"longitude" : "-84.428067",
	"altitude" : "1026",
	"timezone_num" : "-5",
	"timezone_code" : "America/New_York",
	"location" : {
		"type" : "Point",
		"coordinates" : [
			-84.428067,
			33.636719
		]
	}
}
globaldb:PRIMARY>
```
