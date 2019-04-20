package com.microsoft.csu.cdbhack.azure.cosmosdb;

import com.microsoft.csu.cdbhack.utils.JsonUtil;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.microsoft.azure.cosmosdb.*;
import com.microsoft.azure.cosmosdb.rx.AsyncDocumentClient;

import com.fasterxml.jackson.databind.ObjectMapper;

import com.microsoft.csu.cdbhack.io.FileUtil;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.microsoft.csu.cdbhack.Config;
import rx.Observable;

import java.util.HashMap;
import java.util.List;
import java.util.Map;


/**
 * This class performs CRUD operations vs CosmosDB with SQL API
 * with the new Azync SDK.
 *
 * See https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-java-application
 * See test class com.microsoft.azure.cosmosdb.rx.TestSuiteBase in the SDK codebase
 * See https://azure.github.io/azure-cosmosdb-java/2.2.0/overview-summary.html (JavaDocs)
 * See https://azure.github.io/azure-cosmosdb-java/2.2.0/com/microsoft/azure/cosmosdb/rx/AsyncDocumentClient.html
 *
 * @author Chris Joakim, Microsoft
 * @date   2019/04/20
 */

public class CosmosDbUtil {

    // Class variables
    private static final Logger logger = LoggerFactory.getLogger(CosmosDbUtil.class);

    // Instance variables:
    private AsyncDocumentClient client = null;

    public CosmosDbUtil(boolean connect) {

        if (connect) {
            String endpoint = getServiceEndpoint();
            String key = Config.getCosmosSqlDbKey();
            logger.warn("constructor; endpoint: " + endpoint);
            logger.warn("constructor; key: " + key);

            client = new AsyncDocumentClient.Builder()
                    .withServiceEndpoint(endpoint)
                    .withMasterKey(key)
                    .withConnectionPolicy(ConnectionPolicy.GetDefault())
                    .withConsistencyLevel(ConsistencyLevel.Eventual)
                    .build();
        }
        else {
            logger.warn("constructor; connect bypassed for testing");
        }
    }

    // Client connection methods

    /**
     * Return the underlying AsyncDocumentClient object to allow the user of this class
     * to do their own (Async) operations.
     * @return AsyncDocumentClient
     */
    public AsyncDocumentClient getClient() {

        return client;
    }

    public boolean isConnected() {

        return (client == null) ? false : true;
    }

    public void close() {

        if (isConnected()) {
            client.close();
            logger.warn("close() completed");
        }
    }

    // Account methods

    public DatabaseAccount getDatabaseAccount() {

        return client.getDatabaseAccount().toBlocking().first();
    }

    // Configuration methods

    public String getServiceEndpoint() {

        return "https://" + Config.getCosmosSqlDbAcct() + ".documents.azure.com:443/";
    }

    protected String getKey() {

        return Config.getCosmosSqlDbKey();
    }

    // Link methods

    public String dbLink() {

        return client.getDatabaseAccount().toBlocking().first().getDatabasesLink();
    }

    public String dbLink(String dbName) {

        String d = dbName;
        if (d == null) {
            d = Config.getCosmosSqlDbName();
        }
        return "dbs/" + d;
    }

    public String collLink(String dbName, String collName) {

        String dbLink = dbLink(dbName);
        String c = collName;
        if (c == null) {
            c = Config.getCosmosSqlDbDefaultCollName();
        }
        return dbLink + "/colls/" + c;
    }

    public String documentLink(String dbName, String collName, String docId) {

        String collLink = collLink(dbName, collName);
        return collLink + "/docs/" + docId;
    }

    public String storedProcLink(String dbName, String collName, String procName) {

        String collLink = collLink(dbName, collName);
        return collLink + "/sprocs/" + procName;
    }

    // Document Operations

    public ResourceResponse<Document> createDocumentSync(String dbName, String collName, String json) {

        Document doc = docFromJsonString(json);
        return this.createDocumentSync(dbName, collName, doc);
    }

    public ResourceResponse<Document> createDocumentSync(String dbName, String collName, Map<String, Object> map) {

        Document doc = docFromMap(map);
        return this.createDocumentSync(dbName, collName, doc);
    }

    public ResourceResponse<Document> createDocumentSync(String dbName, String collName, Document doc) {

        String collLink = this.collLink(dbName, collName);
        return client.createDocument(collLink, doc, null, false).toBlocking().single();
    }

    public boolean deleteDocumentSync(String dbName, String collName, String pk, String docId) {

        try {
            String link = documentLink(dbName, collName, docId);
            RequestOptions opts = new RequestOptions();
            opts.setPartitionKey(new PartitionKey(pk));
            ResourceResponse<Document> resp = client.deleteDocument(link, opts).toBlocking().single();
            if (resp.getStatusCode() == 204) {
                return true;
            }
        }
        catch (Throwable t) {
            //t.printStackTrace();
        }
        return false;
    }

    public ResourceResponse<Document> upsertDocumentSync(String dbName, String collName, String pk, Document doc) {

        String collLink = this.collLink(dbName, collName);
        RequestOptions opts = new RequestOptions();
        opts.setPartitionKey(new PartitionKey(pk));
        boolean disableAutomaticIdGeneration = false;
        return client.upsertDocument(collLink, doc, opts, disableAutomaticIdGeneration).toBlocking().single();
    }

    public Observable<FeedResponse<Document>> queryDocumentsAsync(
            String dbName, String collName, String sql, boolean enableCrossPartition) {

        String collLink = this.collLink(dbName, collName);
        FeedOptions options = new FeedOptions();
        if (enableCrossPartition) {
            options.setEnableCrossPartitionQuery(enableCrossPartition);
        }
        options.setMaxItemCount(10000);
        return client.queryDocuments(collLink, sql, options);
    }


    public Observable<StoredProcedureResponse> executeStoredProcedureAsync(
            String dbName, String collName, String pk, String procName, Object[] objects) {

        String procLink = this.storedProcLink(dbName, collName, procName);
        logger.warn("executeStoredProcedureAsync; procLink: " + procLink);

        RequestOptions requestOptions = new RequestOptions();
        requestOptions.setScriptLoggingEnabled(true);
        if (pk != null) {
            requestOptions.setPartitionKey(new PartitionKey(pk));
        }
        return client.executeStoredProcedure(procLink, requestOptions, objects);
    }

    // Document ResourceResponse methods

    public String documentResourceResponseJson(ResourceResponse<Document> rr) throws JsonProcessingException  {
        // See file data/drr.json for an example of this pretty-printed JSON.
        Map<String, Object> drrMap = this.documentResourceResponseMap(rr);
        ObjectMapper mapper = new ObjectMapper();
        return mapper.writerWithDefaultPrettyPrinter().writeValueAsString(drrMap);
    }

    public Map<String, Object> documentResourceResponseMap(ResourceResponse<Document> rr) {

        Map<String, Object> map = new HashMap();
        map.put("ActivityId", rr.getActivityId());
        map.put("CollectionQuota", rr.getCollectionQuota());
        map.put("CollectionUsage", rr.getCollectionUsage());
        map.put("CollectionSizeQuota", rr.getCollectionSizeQuota());
        map.put("CollectionSizeUsage", rr.getCollectionSizeUsage());
        map.put("CurrentResourceQuotaUsage", rr.getCurrentResourceQuotaUsage());
        map.put("DatabaseQuota", rr.getDatabaseQuota());
        map.put("DatabaseUsage", rr.getDatabaseUsage());
        map.put("DocumentCountQuota", rr.getDocumentCountQuota());
        map.put("DocumentCountUsage", rr.getDocumentCountUsage());
        map.put("IndexTransformationProgress", rr.getIndexTransformationProgress());
        map.put("LazyIndexingProgress", rr.getLazyIndexingProgress());
        map.put("MaxResourceQuota", rr.getMaxResourceQuota());
        map.put("PermissionQuota", rr.getPermissionQuota());
        map.put("PermissionUsage", rr.getPermissionUsage());
        map.put("RequestCharge", rr.getRequestCharge());
        map.put("Resource", rr.getResource());
        map.put("ResponseHeaders", rr.getResponseHeaders());
        map.put("SessionToken", rr.getSessionToken());
        map.put("StatusCode", rr.getStatusCode());
        map.put("StoredProceduresQuota", rr.getStoredProceduresQuota());
        map.put("StoredProceduresUsage", rr.getStoredProceduresUsage());
        map.put("TriggersQuota", rr.getTriggersQuota());
        map.put("TriggersUsage", rr.getTriggersUsage());
        map.put("UserDefinedFunctionsQuota", rr.getUserDefinedFunctionsQuota());
        map.put("UserDefinedFunctionsUsage", rr.getUserDefinedFunctionsUsage());
        map.put("UserQuota", rr.getUserQuota());
        map.put("UserUsage", rr.getUserUsage());
        return map;
    }

    // Document factory methods

    public Document docFromJsonString(String json) {

        return new Document(String.format(json));
    }

    public Document docFromMap(Map<String, Object> map) {

        Document doc = new Document();

        if (map != null) {
            map.forEach((key, value) -> {
                doc.set(key, value);
            });
        }
        return doc;
    }

    // ========== static methods below ==========

    /**
     * mvn exec:java -Dexec.mainClass="com.chrisjoakim.azure.cosmosdb.CosmosDbUtil" -Dexec.args="deleteDoc dev airports ATL c190a1cf-a4e8-4449-a142-1220f9cd316d"
     * mvn exec:java -Dexec.mainClass="com.chrisjoakim.azure.cosmosdb.CosmosDbUtil" -Dexec.args="queryDocs dev airports 100"
     * mvn exec:java -Dexec.mainClass="com.chrisjoakim.azure.cosmosdb.CosmosDbUtil" -Dexec.args="loadOpenFlightsAirports dev airports"
     */
    public static void main(String[] args) throws Exception {

        String function = "none";
        String dbName   = null;
        String collName = null;
        String procName = null;
        String pk       = null;
        String querySql = null;
        String docId    = null;
        String sqlFile  = null;

        if (args.length > 0) {
            function = args[0];
        }

        switch(function) {

            case "loadOpenFlightsAirports":
                dbName   = args[1];
                collName = args[2];
                loadOpenFlightsAirports(dbName, collName);
                break;
            case "loadOpenFlightsAirportsWithSproc":
                dbName   = args[1];
                collName = args[2];
                procName = args[3];
                loadOpenFlightsAirportsWithSproc(dbName, collName, procName);
                break;
            case "deleteDoc":
                dbName   = args[1];
                collName = args[2];
                pk       = args[3];
                docId    = args[4];
                deleteDoc(dbName, collName, pk, docId);
                break;
            case "queryDocs":
                dbName   = args[1];
                collName = args[2];
                sqlFile  = args[3];
                queryDocs(dbName, collName, sqlFile);
                break;
            default:
                System.out.println("Invalid args; function not defined: " + function);
        }
    }

    private static void deleteDoc(String dbName, String collName, String pk, String docId) throws Exception {

        CosmosDbUtil cosmos = new CosmosDbUtil(true);
        try {
            boolean deleted = cosmos.deleteDocumentSync(dbName, collName, pk, docId);
            System.out.println("deleted: " + deleted);
        }
        catch (Throwable t) {
            t.printStackTrace();
        }
        finally {
            if (cosmos != null) {
                cosmos.close();
                System.out.println("CosmosDB connection closed");
                System.exit(0);
            }
            else {
                System.exit(-1);
            }
        }
    }

    private static void queryDocs(String dbName, String collName, String sqlFile) {

        CosmosDbUtil cosmos = new CosmosDbUtil(true);
        try {
            String sql = new FileUtil().readText(sqlFile);
            logger.warn("sql: " + sql);
            Observable<FeedResponse<Document>> queryObservable =
                    cosmos.queryDocumentsAsync(dbName, collName, sql, true);

            queryObservable.forEach(page -> {
                for (Document doc : page.getResults()) {
                    logger.warn("read_doc: " + doc.toJson());
                }
            });
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        finally {
            if (cosmos != null) {
                try {
                    Thread.sleep(5000);
                }
                catch (InterruptedException e) {
                    e.printStackTrace();
                }
                cosmos.close();
                System.out.println("CosmosDB connection closed");
                System.exit(0);
            }
            else {
                System.exit(-1);
            }
        }
    }

    private static void loadOpenFlightsAirports(String dbName, String collName) throws Exception {

        CosmosDbUtil cosmos = new CosmosDbUtil(true);
        String infile = "data/public/openflights_airports.csv";
        boolean doInserts = true;
        int maxInserts = 10000;

        try {
            FileUtil fu = new FileUtil();
            System.out.println("reading file " + infile);
            List<Map> rows = new FileUtil().readCsvFile(infile, true, ',');
            System.out.println("rows read: " + rows.size());

            for (int i = 0; i < rows.size(); i++) {
                if (doInserts && (i < maxInserts)) {
                    System.out.println("--- " + i);
                    Map row = rows.get(i);

                    // Add the partition key attribute; same value as iata_code:
                    row.put("pk", row.get("iata_code"));

                    // Add the GPS location in GeoJSON format:
                    double longitude = Double.parseDouble((String) row.get("longitude"));
                    double latitude  = Double.parseDouble((String) row.get("latitude"));
                    double[] coordinates = { longitude, latitude };
                    HashMap<String, Object> location = new HashMap();
                    location.put("type", "Point");
                    location.put("coordinates", coordinates);
                    row.put("location", location);
                    System.out.println("row: " + row);

                    // Insert the document and display the resulting response:
                    ResourceResponse<Document> rr = cosmos.createDocumentSync(dbName, collName, row);
                    System.out.println("status: " + rr.getStatusCode());
                    System.out.println("charge: " + rr.getRequestCharge());
                    System.out.println("document:\n" + rr.getResource().toJson());
                    System.out.println("rr:\n" + cosmos.documentResourceResponseJson(rr));
                }
            }
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        finally {
            if (cosmos != null) {
                cosmos.close();
                System.out.println("CosmosDB connection closed");
                System.exit(0);
            }
            else {
                System.exit(-1);
            }
        }
    }

    private static void loadOpenFlightsAirportsWithSproc(String dbName, String collName, String procName) throws Exception {

        CosmosDbUtil cosmos = new CosmosDbUtil(true);
        String infile = "data/public/openflights_airports.csv";
        boolean doInserts = true;
        int maxInserts = 10000;

        try {
            FileUtil fu = new FileUtil();
            System.out.println("reading file " + infile);
            List<Map> rows = new FileUtil().readCsvFile(infile, true, ',');
            System.out.println("rows read: " + rows.size());

            for (int i = 0; i < rows.size(); i++) {
                if (doInserts && (i < maxInserts)) {
                    System.out.println("--- " + i);
                    Map row = rows.get(i);
                    String pk = (String) row.get("iata_code");

                    // Add the partition key attribute; same value as iata_code:
                    row.put("pk", pk);

                    // Add the GPS location in GeoJSON format:
                    double longitude = Double.parseDouble((String) row.get("longitude"));
                    double latitude  = Double.parseDouble((String) row.get("latitude"));
                    double[] coordinates = { longitude, latitude };
                    HashMap<String, Object> location = new HashMap();
                    location.put("type", "Point");
                    location.put("coordinates", coordinates);
                    row.put("location", location);
                    System.out.println("row: " + row);

                    Object[] sprocObjects = { row };

                    // Insert the document and display the resulting response:
                    StoredProcedureResponse spr = cosmos.executeStoredProcedureAsync(
                            dbName, collName, pk, procName, sprocObjects).toBlocking().single();

                    String json = spr.getResponseAsString();
                    System.out.println("status: " + spr.getStatusCode());
                    System.out.println("charge: " + spr.getRequestCharge());
                    System.out.println("string:\n" + json);

                    JsonUtil ju = new JsonUtil();
                    TypeReference ref = new TypeReference<Map<String, Object>>(){};
                    Map<String, Object> map = (Map<String, Object>) ju.parse(json, ref);
                    System.out.println("pretty:\n" + ju.pretty(map));
                }
            }
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        finally {
            if (cosmos != null) {
                cosmos.close();
                System.out.println("CosmosDB connection closed");
                System.exit(0);
            }
            else {
                System.exit(-1);
            }
        }
    }

}
