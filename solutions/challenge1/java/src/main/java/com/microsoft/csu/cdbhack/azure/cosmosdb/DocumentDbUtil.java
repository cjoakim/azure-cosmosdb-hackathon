package com.microsoft.csu.cdbhack.azure.cosmosdb;

import com.microsoft.csu.cdbhack.Config;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.microsoft.azure.documentdb.*;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;

/**
 * This class performs all CRUD operations vs CosmosDB for the application.
 * TODO - revisit this class
 *
 * See https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-java-application
 * See https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-sql-query
 * See https://github.com/Azure/azure-documentdb-java/blob/master/documentdb-examples/src/test/java/com/microsoft/azure/documentdb/examples/DocumentQuerySamples.java
 *
 * @author Chris Joakim, Microsoft
 * @date   2019/04/20
 */

public class DocumentDbUtil {

    // Class variables
    private static final Logger logger = LoggerFactory.getLogger(DocumentDbUtil.class);

    // Instance variables:
    private DocumentClient client = null;


    public DocumentDbUtil() {

        try {
            this.client = new DocumentClient(
                    serviceEndpoint(),
                    Config.getCosmosSqlDbKey(),
                    new ConnectionPolicy(),
                    ConsistencyLevel.Session);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public Document insertDocument(String dbName, String collName, Object obj) throws DocumentClientException {

        String collLink = this.collLink(dbName, collName);
        return this.client.createDocument(collLink, obj, new RequestOptions(), false).getResource();
    }

    public Document upsertDocument(String dbName, String collName, Object obj) throws DocumentClientException {

        String collLink = this.collLink(dbName, collName);
        return this.client.upsertDocument(collLink, obj, new RequestOptions(), false).getResource();
    }

    public FeedResponse<Document> queryAsDocuments(String dbName, String collName, String sql) {

        String collLink = this.collLink(dbName, collName);
        FeedOptions options = new FeedOptions();
        options.setEnableCrossPartitionQuery(true);
        return this.client.queryDocuments(collLink, sql, options);
    }

    public ArrayList<String> queryAsJsonList(String dbName, String collName, String sql) {

        FeedResponse<Document> queryResults = queryAsDocuments(dbName, collName, sql);
        ArrayList<String> jsonStrings = new ArrayList<String>();

        for (Document doc : queryResults.getQueryIterable()) {
            if (doc != null) {
                jsonStrings.add(doc.toJson());
            }
            // doc is an instance of com.microsoft.azure.documentdb.Document
        }
        return jsonStrings;
    }

    protected String collLink(String dbName, String collName) {

        String d = dbName;
        String c = collName;

        if (d == null) {
            d = Config.getCosmosSqlDbName();
        }
        if (c == null) {
            c = Config.getCosmosSqlDbDefaultCollName();
        }
        return "dbs/" + d + "/colls/" + c;
    }

    protected String serviceEndpoint() {

        return "https://" + Config.getCosmosSqlDbAcct() + ".documents.azure.com";
    }

    public static void main(String[] args) {

        try {
            //main_queryAsDocuments();
            main_queryAsJsonList();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        finally {
            System.exit(0);
        }
    }

    private static void main_queryAsDocuments() {

        try {
            String dbName = Config.getCosmosSqlDbName();
            String collName = Config.getCosmosSqlDbDefaultCollName();
            logger.warn(String.format("main_queryAsDocuments: %s  db: %s  coll: %s", Config.getCosmosSqlDbAcct(), dbName, collName));

            DocumentDbUtil dao = new DocumentDbUtil();

            String sql = "SELECT * FROM c WHERE c.id = '11787:current'";
            logger.warn(String.format("executing sql: %s", sql));
            FeedResponse<Document> queryResults = dao.queryAsDocuments(dbName, collName, sql);

            for (Document doc : queryResults.getQueryIterable()) {
                // doc is an instance of com.microsoft.azure.documentdb.Document
                String json = doc.toJson();
                logger.warn("---");
                logger.warn(String.format("%s", json));
            }
            logger.warn("run complete");
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        finally {
            System.exit(0);
        }
    }

    private static void main_queryAsJsonList() {

        try {
            String dbName = Config.getCosmosSqlDbName();
            String collName = Config.getCosmosSqlDbDefaultCollName();
            logger.warn(String.format("main_queryAsDocuments: %s  db: %s  coll: %s", Config.getCosmosSqlDbAcct(), dbName, collName));

            DocumentDbUtil dao = new DocumentDbUtil();

            String sql = "SELECT * FROM c WHERE c.id = '11787:current'";
            logger.warn(String.format("executing sql: %s", sql));
            ArrayList<String> queryResults = dao.queryAsJsonList(dbName, collName, sql);

            for (int i = 0; i < queryResults.size(); i++) {
                // doc is an instance of com.microsoft.azure.documentdb.Document
                String json = queryResults.get(i);
                logger.warn("---");
                logger.warn(String.format("%s", json));

                ObjectMapper mapper = new ObjectMapper();
                mapper.disable(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES);
                JsonNode jsonNodeRoot = mapper.readTree(json);
            }
            logger.warn("run complete");
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        finally {
            System.exit(0);
        }
    }

}