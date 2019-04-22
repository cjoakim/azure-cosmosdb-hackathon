package com.microsoft.csu.cdbhack;

import java.util.List;
import java.util.Map;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

import com.microsoft.azure.documentdb.Document;
import com.microsoft.azure.documentdb.DocumentClient;
import com.microsoft.azure.documentdb.PartitionKey;
import com.microsoft.azure.documentdb.RequestOptions;

import com.microsoft.csu.cdbhack.io.FileUtil;
import com.microsoft.csu.cdbhack.utils.CommandLineArgs;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 * Abstract superclass of the several Challenge classes.
 *
 * @author Chris Joakim, Microsoft
 * @date   2019/04/20
 */
public abstract class Challenge {

    // Constants:
    public static final String OPENFLIGHTS_AIRPORTS_CSV = "data/openflights_airports.csv";

    // Class variables:
    private static final Logger logger = LoggerFactory.getLogger(Challenge.class);

    // Instance variables:
    protected String[] args = null;
    protected CommandLineArgs clArgs = new CommandLineArgs(args);
    protected DocumentClient  client = null;

    public Challenge(String[] args) {

        super();
        this.args = args;
    }

    protected DocumentClient getDocumentDbClient() {

        if (client == null) {  // lazy-initialize the client
            String host = System.getenv("AZURE_COSMOSDB_SQLDB_URI");
            String key  = System.getenv("AZURE_COSMOSDB_SQLDB_KEY");
            System.out.println("getDocumentDbClient; host: " + host);
            System.out.println("getDocumentDbClient; key:  " + key);
            client = new DocumentClient(host, key, null, null);
        }
        return client;
    }

    protected List<Map> readCsvFile(String filename, boolean hasHeader, char delim) throws Exception {

        FileUtil util = new FileUtil();
        return util.readCsvFile(filename, hasHeader, delim);
    }
}
