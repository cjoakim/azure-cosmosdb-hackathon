# azure-cosmosdb-hackathon

## Challenge 9 Solution with Python - Cassandra

The solution code is in directory **solutions/python/** in this repository.

This solution uses a dataset containing 30 World Airports.

### Azure Portal

In Azure Portal, create a CosmosDB with Cassandra API Account.

Then create table **hackathon.major_airports** with this CQL:
```
(iata_code text, city text, country text, name text, rank int, passengers int, tz_num float, tz_code text, latitude float, longitude float, altitude float, PRIMARY KEY (iata_code)) 
```

### Python

Create and activate the python virtual library with the necessary libraries:
See files **requirements.in** and **venv.sh**
```
./venv.sh
source bin/activate
python --version
```

Then load the table with Python:
```
python challenge9.py load_major_airports hackathon major_airports
```

Then query all rows in the table with Python:
```
python challenge9.py query_all_major_airports hackathon major_airports
```

### cqlsh

If you have cassandra installed on your workstation, you can use **cqlsh**.
Otherwise, execute CQL queries in Azure Portal.

```
./cqlsh_cosmosdb.sh

cjoakimcosmosdbcass@cqlsh> select * from hackathon.major_airports where iata_code = 'PHX';

 iata_code | city    | country       | name                    | rank | passengers | tz_num | tz_code         | latitude | longitude  | altitude
-----------+---------+---------------+-------------------------+------+------------+--------+-----------------+----------+------------+----------
       PHX | Phoenix | United States | Phoenix Sky Harbor Intl |   41 |   43921670 |     -7 | America/Phoenix | 33.43428 | -112.01158 |     1135

(1 rows)

cjoakimcosmosdbcass@cqlsh> exit
```

## Links

- https://docs.microsoft.com/en-us/azure/cosmos-db/create-cassandra-python
- https://github.com/datastax/python-driver
