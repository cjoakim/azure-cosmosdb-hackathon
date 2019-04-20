package com.microsoft.csu.cdbhack;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.microsoft.azure.documentdb.Document;
import com.microsoft.azure.documentdb.DocumentClient;
import com.microsoft.azure.documentdb.PartitionKey;
import com.microsoft.azure.documentdb.RequestOptions;

import java.io.File;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * Example program to access Azure CosmosDB.
 *
 * @author Chris Joakim, Microsoft
 * @date   2019/04/20
 *
 * THIS CODE IS FOR PROOF-OF-CONCEPT ONLY; NOT FOR PRODUCTION USE!
 */
public class Program {

    // Class variables:
    private static String[] clArgs = null;
    private static String   runFunction = null;
    private static DocumentClient client = null;

    // execute_bulk_import dev airports data/top_50_airports_list.json

    public static void main( String[] args ) throws Exception {

        if (args.length > 1) {
            clArgs = args;
            runFunction  = clArgs[0];
            System.out.println("runFunction:  " + runFunction);
        }
        else {
            System.err.println("Invalid command-line args, expected runFunction");
            System.exit(1);
        }

        switch (runFunction) {
            case "execute_bulk_import":
                executeBulkImport();
                break;

            default:
                throw new IllegalArgumentException("Invalid runFunction: " + runFunction);
        }
    }

    private static void executeBulkImport() throws Exception {

        // Note, it assumed that the 'bulkImport' stored procedure is present in your database.
        // Execute the following command in this repo to create it.
        // server_side]$ node main.js create_stored_proc dev airports bulkImport create

        int bundleSize = 8;
        String dbName = clArgs[1];
        String collName = clArgs[2];
        String jsonInfile = clArgs[3];
        System.out.println(String.format("executeBulkImport %s %s %s", dbName, collName, jsonInfile));

        ObjectMapper objectMapper = new ObjectMapper();
        ArrayList<Map> documents = objectMapper.readValue(
                new File(jsonInfile), new TypeReference<ArrayList<Map>>() {});

        System.out.println(documents);

        ArrayList<Map> bundle = new ArrayList<Map>();
        int bundleNum = 1;

        for (int i = 0; i < documents.size(); i++) {
            Map doc = documents.get(i);
            String iata = (String) doc.get("iata");
            doc.put("pk", "bundle_" + bundleNum);
            bundle.add(doc);

            if (bundle.size() == bundleSize) {
                invokeBulkImportStoredProc(dbName, collName, bundle);
                bundle = new ArrayList<Map>();
                bundleNum++;
            }
        }

        // handle any docs in the bundle at the end-of-file
        if (bundle.size() > 0) {
            invokeBulkImportStoredProc(dbName, collName, bundle);
        }
    }

    private static void invokeBulkImportStoredProc(String dbName, String collName, ArrayList<Map> docs) throws Exception {

        // See https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-java-samples

        System.out.println("invokeStoredProc; bundle size: " + docs.size());

        if (docs.size() > 0) {
            String storedProcLink = String.format("/dbs/%s/colls/%s/sprocs/%s", dbName, collName, "bulkImport");
            System.out.println("storedProcLink: " + storedProcLink);

            ObjectMapper mapper = new ObjectMapper();
            List<String> jsonStrings = new ArrayList<>();

            for (int i = 0; i < docs.size(); i++) {
                Map doc = docs.get(i);
                String json = mapper.writeValueAsString(doc);
                Document d = new Document(json);
                System.out.println(d.toString());
                jsonStrings.add(d.toString());
            }
            Object[] storedProcedureArgs = new Object[] { jsonStrings };

            String pk = (String) docs.get(0).get("pk");
            RequestOptions requestOptions = new RequestOptions();
            requestOptions.setPartitionKey(new PartitionKey(pk));
            requestOptions.setScriptLoggingEnabled(true);

            String result = getDocumentDbClient().executeStoredProcedure(
                    storedProcLink, requestOptions, storedProcedureArgs).getResponseAsString();
            System.out.println("result: " + result);
        }
    }

    private static DocumentClient getDocumentDbClient() {

        if (client == null) {  // lazy-initialize the client
            String host = System.getenv("AZURE_COSMOSDB_SQLDB_URI");
            String key  = System.getenv("AZURE_COSMOSDB_SQLDB_KEY");
            System.out.println("getDocumentDbClient; host: " + host);
            System.out.println("getDocumentDbClient; key:  " + key);
            client = new DocumentClient(host, key, null, null);
        }
        return client;
    }
}
