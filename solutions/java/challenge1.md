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
```

## Links

