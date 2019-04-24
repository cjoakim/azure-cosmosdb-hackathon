# azure-cosmosdb-hackathon

## Challenge 1 - Port Relational and GPS Data to CosmosDB/SQL

The solution code is in directory **solutions/java/** in this repository.

See classes:
- com.microsoft.csu.cdbhack.Program
- com.microsoft.csu.cdbhack.Challenge1

This solution uses a dataset containing 8100+ World Airports;
see data/openflights_airports.csv

### Azure Portal

In Azure Portal, create a CosmosDB with SQL API Account.

Then create a **hackathon** database within it.

Then create collection **airports** within the hackathon database
with a partition key named **pk**

### Java

Compile and package the code with Maven:
```
./build.sh
...
[INFO] ------------------------------------------------------------------------
[INFO] BUILD SUCCESS
[INFO] ------------------------------------------------------------------------
...
```

Execute the program:
```
./challenge1.sh

...

WARN  {
  "TimezoneCode" : "America/Los_Angeles",
  "Dst" : "A",
  "IataCode" : "OLT",
  "Latitude" : "32.7552",
  "City" : "San Diego",
  "Longitude" : "-117.1995",
  "TimezoneNum" : "-8",
  "Name" : "San Diego Old Town Transit Center",
  "AirportId" : "9541",
  "IcaoCode" : "\\N",
  "Country" : "United States",
  "location" : {
    "coordinates" : [ -117.1995, 32.7552 ],
    "type" : "Point"
  },
  "pk" : "OLT",
  "Altitude" : "0"
}


WARN  countDocuments, sql: SELECT VALUE COUNT(1) FROM c
WARN  {"_aggregate":1668}
WARN  results count: 1


WARN  queryAirport, sql: SELECT * from c where c.pk = 'CLT'
WARN  {"TimezoneCode":"America/New_York","_rid":"VPMYAJ9IwRCpAAAAAAAAAA==","Dst":"A","IataCode":"CLT","Latitude":"35.214","City":"Charlotte","Longitude":"-80.943139","TimezoneNum":"-5","Name":"Charlotte Douglas Intl","_attachments":"attachments/","AirportId":"3876","IcaoCode":"KCLT","Country":"United States","location":{"coordinates":[-80.943139,35.214],"type":"Point"},"pk":"CLT","id":"ec360693-aa99-4e47-b606-93aa99ce475f","_self":"dbs/VPMYAA==/colls/VPMYAJ9IwRA=/docs/VPMYAJ9IwRCpAAAAAAAAAA==/","_etag":"\"0e005315-0000-0100-0000-5cc0add80000\"","Altitude":"748","_ts":1556131288}
WARN  results count: 1


WARN  geoQuery, sql: SELECT * from c WHERE ST_DISTANCE(c.location, {'type': 'Point', 'coordinates':[-80.84847,35.499233]}) < 40000
WARN  {"TimezoneCode":"America/New_York","_rid":"VPMYAJ9IwRCpAAAAAAAAAA==","Dst":"A","IataCode":"CLT","Latitude":"35.214","City":"Charlotte","Longitude":"-80.943139","TimezoneNum":"-5","Name":"Charlotte Douglas Intl","_attachments":"attachments/","AirportId":"3876","IcaoCode":"KCLT","Country":"United States","location":{"coordinates":[-80.943139,35.214],"type":"Point"},"pk":"CLT","id":"ec360693-aa99-4e47-b606-93aa99ce475f","_self":"dbs/VPMYAA==/colls/VPMYAJ9IwRA=/docs/VPMYAJ9IwRCpAAAAAAAAAA==/","_etag":"\"0e005315-0000-0100-0000-5cc0add80000\"","Altitude":"748","_ts":1556131288}
WARN  {"TimezoneCode":"America/New_York","_rid":"VPMYAJ9IwRBYAQAAAAAACA==","Dst":"A","IataCode":"SVH","Latitude":"35.7649958","City":"Statesville","Longitude":"-80.9538958","TimezoneNum":"-5","Name":"Regional Airport","_attachments":"attachments/","AirportId":"8526","IcaoCode":"KSVH","Country":"United States","location":{"coordinates":[-80.9538958,35.7649958],"type":"Point"},"pk":"SVH","id":"8ad9c0da-8a71-4e1b-99c0-da8a718e1b96","_self":"dbs/VPMYAA==/colls/VPMYAJ9IwRA=/docs/VPMYAJ9IwRBYAQAAAAAACA==/","_etag":"\"060035cd-0000-0100-0000-5cc0ae040000\"","Altitude":"968","_ts":1556131332}
WARN  results count: 2
```

## Links

- http://geojson.org
- https://docs.microsoft.com/en-us/azure/cosmos-db/geospatial
- https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-sql-query
