package com.microsoft.csu.cdbhack.azure.cosmosdb;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.microsoft.csu.cdbhack.EnvVarNames;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.microsoft.csu.cdbhack.Config;
import com.microsoft.csu.cdbhack.EnvVarNames;
import com.microsoft.csu.cdbhack.io.FileUtil;
import com.microsoft.csu.cdbhack.utils.CommandLineArgs;

import com.microsoft.azure.cosmosdb.Document;
import com.microsoft.azure.cosmosdb.FeedOptions;
import com.microsoft.azure.cosmosdb.FeedResponse;

import rx.Observable;

/**
 * Read a collection with the new RxJava-based CosmosDB SDK.
 * The SDK used RxJava version 1.3.3.
 * 
 * @see https://github.com/ReactiveX/RxJava
 * @see https://azure.microsoft.com/en-us/blog/announcing-new-async-java-sdk-for-azure-cosmosdb/
 * @see https://github.com/Azure/azure-cosmosdb-java
 *
 * @author Chris Joakim, Microsoft
 * @date   2019/04/20
 */

public class CosmosRxReader extends CosmosRxBase implements EnvVarNames {

    // Class variables
    private static final Logger logger = LoggerFactory.getLogger(CosmosRxReader.class);
    
    // Instance variables
	private HashMap<String, Object> iataCodes = new HashMap<String, Object>();
	
	/**
	 * Default constructor; not for loading the DB.
	 */
	public CosmosRxReader() {
		
		this(false, null, null);
	}
	
	/**
	 * Default constructor; use for loading the DB.
	 */
	public CosmosRxReader(boolean useDb, String dbName, String collName) {
		
		super(useDb, dbName, collName);
	}

	public static void main(String[] args) throws Exception {
		
		CommandLineArgs clArgs = new CommandLineArgs(args);
        logger.warn("Main.main; args count: " + clArgs.count());
        String function = clArgs.stringArg("--function", "none");
        logger.warn("Main.main; function: " + function);
        
        String infile   = clArgs.stringArg("--infile", "data/world_airports_flat.json");
        boolean useDb   = clArgs.booleanArg("--use-db", false);
        String dbName   = clArgs.stringArg("--dbname", Config.getCosmosSqlDbName());
        String collName = clArgs.stringArg("--collname", Config.getCosmosSqlDbDefaultCollName());
        
        FileUtil fileUtil = new FileUtil();
        CosmosRxReader reader = new CosmosRxReader(useDb, dbName, collName);
        List<String> lines = fileUtil.readFileLines(infile);

        for (int i = 0; i < lines.size(); i++) {
        	reader.processLine(lines.get(i));
        }
        reader.close();
	}
	
	private void processLine(String line) {
		
		try {
			logger.warn("processLine: " + line);
			Map<String, Object> airport = parseJsonLineToMap(line);
			String iata = ((String) airport.getOrDefault("iata_code", "")).trim();
			if (iata.length() > 2) {
		        FeedOptions options = new FeedOptions();
		        options.setMaxItemCount(3);
		        String sql = String.format("SELECT c.country, c.city, c.pk FROM c where c.pk = '%s'", iata);
		        logger.warn("sql: " + sql);

		        // See SDK file DocumentQueryAsyncAPITest
		        Observable<FeedResponse<Document>> documentQueryObservable =
		        		this.client.queryDocuments(this.collLink, sql, options);
		        
		        documentQueryObservable.forEach(page -> {
		            for (Document doc : page.getResults()) {
		        		logger.warn("read_doc: " + doc.toJson());
		        		this.lastResponseTime = getEpochSecond();
		            }
		        });
			}
		}
		catch (Exception e) {
			e.printStackTrace();
		}
	}

}

//{
//    "country": "United States",
//    "altitude": "748",
//    "iata_code": "CLT",
//    "city": "Charlotte",
//    "timezone_num": "-5",
//    "latitude": "35.214",
//    "epoch": "1527362284528",
//    "timezone_code": "America/New_York",
//    "name": "Charlotte Douglas Intl",
//    "location": {
//        "coordinates": [
//            -80.943139,
//            35.214
//        ],
//        "type": "Point"
//    },
//    "pk": "CLT",
//    "id": "c6b2231d-3b6f-42bd-8279-3f0013fe92bd",
//    "longitude": "-80.943139",
//    "_rid": "wboEAPCYAgBBAgAAAAAAAA==",
//    "_self": "dbs/wboEAA==/colls/wboEAPCYAgA=/docs/wboEAPCYAgBBAgAAAAAAAA==/",
//    "_etag": "\"010045e3-0000-0000-0000-5b09b2ec0000\"",
//    "_attachments": "attachments/",
//    "_ts": 1527362284
//},
