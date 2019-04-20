package com.microsoft.csu.cdbhack.azure.cosmosdb;

import java.io.File;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.mongodb.MongoClient;
import com.mongodb.MongoClientURI;
import com.mongodb.client.FindIterable;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoDatabase;
import org.bson.Document;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.microsoft.csu.cdbhack.Config;
import com.microsoft.csu.cdbhack.EnvVarNames;
import com.microsoft.csu.cdbhack.io.FileUtil;

/**
 * This class implements Azure CosmosDB/MongoDB operations.
 *
 * @author Chris Joakim, Microsoft
 * @date   2019/04/20
 *
 * @see    https://github.com/Azure-Samples/azure-cosmos-db-mongodb-java-getting-started
 * @see    https://github.com/mongodb/mongo-java-driver
 */

public class MongoDbUtil implements EnvVarNames {

    // Class variables
    private static final Logger logger = LoggerFactory.getLogger(MongoDbUtil.class);

    // Instance variables:
	private MongoClientURI uri    = null;
	private MongoClient    client = null;
	private MongoDatabase  db     = null;
	private MongoCollection<Document> airportsColl = null;
	private MongoCollection<Document> zipcodesColl = null;

	public MongoDbUtil() {

		super();

		try {
			String connString = Config.envVar(AZURE_COSMOSDB_MONGODB_CONN_STRING);
			String dbName     = Config.envVar(AZURE_COSMOSDB_MONGODB_DBNAME);
            logger.warn("constructor; connString: " + connString);

			uri = new MongoClientURI(connString);
            logger.warn("constructor; uri: " + uri);

			client = new MongoClient(uri);
            logger.warn("constructor; client: " + client);

			db  = client.getDatabase(dbName);
            logger.warn("constructor; db: " + db + " name: " + db.getName());
		}
		catch (Exception e) {
			e.printStackTrace();
		}
	}

	public MongoCollection<Document> getCollection(String name) {

	    return db.getCollection(name);
    }

	public void addDocument(String collName, Map<String,Object> map) {

		Document doc = new Document(map);
		getCollection(collName).insertOne(doc);
	}

	public FindIterable<Document> queryCollection(String collName, Document query) {

		return getCollection(collName).find(query);
	}

	public void close() {

        if (client != null) {
            client.close();
        }
    }

	public static void main(String[] args) {

        String function = "none";
        String dbName   = null;
        String collName = null;

		try {
            if (args.length > 0) {
                function = args[0];
            }

            switch(function) {

                case "loadOpenFlightsAirports":  // loadOpenFlightsAirports dev airports
                    dbName   = args[1];
                    collName = args[2];
                    loadOpenFlightsAirports(dbName, collName);
                    break;
                case "loadProducts":  // loadProducts dev products
                    dbName   = args[1];
                    collName = args[2];
                    loadProducts(dbName, collName);
                    break;
                default:
                    logger.error("main - invalid args; function not defined: " + function);
            }
		}
		catch (Exception e) {
			e.printStackTrace();
		}
		finally {
			System.exit(0);
		}
	}

    private static void loadOpenFlightsAirports(String dbName, String collName) throws Exception {

        logger.warn("loadOpenFlightsAirports - db: " + dbName + " coll: " + collName);

        MongoDbUtil mongo = new MongoDbUtil();
        String infile = "data/public/openflights_airports.csv";
        boolean doInserts = true;
        int maxInserts = 10;

        try {
            FileUtil fu = new FileUtil();
            logger.warn("loadOpenFlightsAirports - reading file " + infile);
            List<Map> rows = new FileUtil().readCsvFile(infile, true, ',');
            logger.warn("rows read: " + rows.size());

            for (int i = 0; i < rows.size(); i++) {
                if (doInserts && (i < maxInserts)) {
                    logger.warn("--- " + i);
                    Map row = rows.get(i);

                    // Add the partition key attribute; same value as iata_code:
                    row.put("pk", row.get("iata_code"));

                    logger.warn(row.toString());
                    mongo.addDocument(collName, row);
                }
            }
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        finally {
            if (mongo != null) {
                mongo.close();
                logger.warn("loadOpenFlightsAirports - connection closed");
                System.exit(0);
            }
            else {
                System.exit(-1);
            }
        }
    }

    private static void loadProducts(String dbName, String collName) throws Exception {

        logger.warn("loadProducts - db: " + dbName + " coll: " + collName);

        MongoDbUtil mongo = new MongoDbUtil();
        String infile = "data/public/products2.json";
        boolean doInserts = true;
        int maxInserts = 10;

        try {
            FileUtil fu = new FileUtil();
            logger.warn("loadProducts - reading file " + infile);
            String json = new FileUtil().readText(infile);

            ObjectMapper objectMapper = new ObjectMapper();
            ArrayList<Map> documents = objectMapper.readValue(
                    new File(infile), new TypeReference<ArrayList<Map>>() {});
            logger.warn("documents read: " + documents.size());

            for (int i = 0; i < documents.size(); i++) {
                if (doInserts && (i < maxInserts)) {
                    logger.warn("--- " + i);
                    Map row = documents.get(i);

                    // Add the partition key attribute; same value as iata_code:
                    //row.put("pk", "" + i);

                    logger.warn(row.toString());
                    mongo.addDocument(collName, row);
                }
            }
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        finally {
            if (mongo != null) {
                mongo.close();
                logger.warn("loadProducts - connection closed");
                System.exit(0);
            }
            else {
                System.exit(-1);
            }
        }
    }
}
