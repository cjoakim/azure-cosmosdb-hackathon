# azure-cosmosdb-hackathon

## Challenge 9 with Python

### Azure Portal

In Azure Portal, create table hackathon.major_airports with this CQL:
```
(iata_code text, city text, country text, name text, rank int, passengers int, tz_num float, tz_code text, latitude float, longitude float, altitude float, PRIMARY KEY (iata_code)) 
```

### Python

Then load the table with Python:
```
python main.py load_major_airports hackathon major_airports
```

Then query all rows in the table with Python:
```
python main.py query_all_major_airports hackathon major_airports
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
```

## Links

- https://docs.microsoft.com/en-us/azure/cosmos-db/create-cassandra-python
- https://github.com/datastax/python-driver
