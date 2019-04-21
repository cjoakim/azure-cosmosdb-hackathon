package com.microsoft.csu.cdbhack.azure.cosmosdb;

import java.time.Instant;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.UUID;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.microsoft.csu.cdbhack.Config;
import com.microsoft.csu.cdbhack.EnvVarNames;
import com.microsoft.csu.cdbhack.io.FileUtil;
import com.microsoft.csu.cdbhack.utils.CommandLineArgs;

import com.microsoft.azure.cosmosdb.Document;
import com.microsoft.azure.cosmosdb.RequestOptions;
import com.microsoft.azure.cosmosdb.ResourceResponse;

import rx.Observable;

/**
 * Load a collection with the new RxJava-based CosmosDB SDK.
 * The SDK used RxJava version 1.3.3.
 * 
 * @see https://github.com/ReactiveX/RxJava
 * @see https://azure.microsoft.com/en-us/blog/announcing-new-async-java-sdk-for-azure-cosmosdb/
 * @see https://github.com/Azure/azure-cosmosdb-java
 *
 * @author Chris Joakim, Microsoft
 * @date   2019/04/20
 */

public class CosmosRxLoader extends CosmosRxBase implements EnvVarNames {

    // Class variables
    private static final Logger logger = LoggerFactory.getLogger(CosmosRxLoader.class);
    
    // Instance variables
	private HashMap<String, Object> iataCodes = new HashMap<String, Object>();
	private long pauseMs;
	
	/**
	 * Default constructor; not for loading the DB.
	 */
	public CosmosRxLoader() {
		
		this(false, null, null, 10);
	}
	
	/**
	 * Default constructor; use for loading the DB.
	 */
	public CosmosRxLoader(boolean useDb, String dbName, String collName, long pauseMs) {
		
		super(useDb, dbName, collName);
		this.pauseMs = pauseMs;
	}

	public static void main(String[] args) throws Exception {
		
		CommandLineArgs clArgs = new CommandLineArgs(args);
        logger.warn("Main.main; args count: " + clArgs.count());
        String function = clArgs.stringArg("--function", "none");
        logger.warn("Main.main; function: " + function);
        
        String  infile   = clArgs.stringArg("--infile",  "data/public/world_airports_flat.json");
        boolean useDb    = clArgs.booleanArg("--use-db",  false);
        String  dbName   = clArgs.stringArg("--dbname",   Config.getCosmosSqlDbName());
        String  collName = clArgs.stringArg("--collname", Config.getCosmosSqlDbDefaultCollName());
        long    pauseMs  = clArgs.longArg("--pause", 50);
        		
        FileUtil fileUtil = new FileUtil();
        CosmosRxLoader loader = new CosmosRxLoader(useDb, dbName, collName, pauseMs);
        List<String> lines = fileUtil.readFileLines(infile);

        for (int i = 0; i < lines.size(); i++) {
        	loader.processLine(lines.get(i));
        }
        loader.close();
	}
	
	private void processLine(String line) {
		
		try {
			logger.warn("processLine: " + line);
			Map<String, Object> airport = parseJsonLineToMap(line);
			String iata = ((String) airport.getOrDefault("iata_code", "")).trim();
			if (iata.length() > 2) {
				airport.put("pk", iata);
				airport.put("id", UUID.randomUUID().toString());
				airport.put("epoch", "" + Instant.now().toEpochMilli());
				
				if (iataCodes.containsKey(iata)) {
					logger.warn("duplicate iata code: " + iata);
				}
				Document doc = new Document(mapToJson(airport));
				logger.warn("loading_doc: " + doc.toString());
				
		        if (this.useDb) {
		        	RequestOptions opts = new RequestOptions();
					Observable<ResourceResponse<Document>> createObservable = 
						this.client.createDocument(this.collLink, doc, opts, true);
					
					createObservable.single().subscribe(
						documentResourceResponse -> {
							// documentResourceResponse is instance of com.microsoft.azure.cosmosdb.ResourceResponse
							this.lastResponseTime = getEpochSecond();
							logger.warn("ResourceResponse: " + documentResourceResponse.getResource()); 
						},
						error -> {
							this.lastResponseTime = getEpochSecond();
							logger.warn("ERROR: " + error.getMessage());
						});
		        }
		        pause(pauseMs);
			}
		}
		catch (Exception e) {
			e.printStackTrace();
		}
	}

}
