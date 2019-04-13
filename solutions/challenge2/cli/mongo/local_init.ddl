// MongoDB "DDL" initialize the hackathon database with airports collection.
// Chris Joakim, Microsoft, 2019/04/13

use hackathon

db.airports.drop()
db.createCollection("airports")
db.airports.ensureIndex({"iata_code" : 1}, {"unique" : false})
db.airports.ensureIndex({"name" : 1}, {"unique" : false})
db.airports.ensureIndex({"timezone_num" : 1}, {"unique" : false})
db.airports.getIndexKeys()

show collections
db.airports.count()
 