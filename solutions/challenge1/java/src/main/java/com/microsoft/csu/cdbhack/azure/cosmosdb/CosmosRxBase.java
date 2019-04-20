package com.microsoft.csu.cdbhack.azure.cosmosdb;

import java.time.Instant;
import java.util.HashMap;
import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.microsoft.csu.cdbhack.Config;
import com.microsoft.csu.cdbhack.EnvVarNames;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.microsoft.azure.cosmosdb.ConnectionPolicy;
import com.microsoft.azure.cosmosdb.ConsistencyLevel;
import com.microsoft.azure.cosmosdb.rx.AsyncDocumentClient;

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

public abstract class CosmosRxBase implements EnvVarNames {

    // Class variables
    protected static final Logger logger = LoggerFactory.getLogger(CosmosRxBase.class);
    
    // Instance variables
	protected AsyncDocumentClient client = null;
	protected boolean useDb = false;
	protected String  dbName = null;
	protected String  collName = null;
	protected String  collLink = null;
	protected long    lastResponseTime = getEpochSecond();

	/**
	 * Default constructor; not for loading the DB.
	 */
	public CosmosRxBase() {
		
		this(false, null, null);
	}
	
	/**
	 * Default constructor; use for loading the DB.
	 */
	public CosmosRxBase(boolean useDb, String dbName, String collName) {
		
		super();
		this.useDb = useDb;
		this.dbName = dbName;
		this.collName = collName;
		this.collLink = getCollLink();

        if (this.useDb) {
        	createClient();
        }
	}

	protected void createClient() {

		String uri = Config.getCosmosSqlDbUri();
		String key = Config.getCosmosSqlDbKey();
		
		logger.warn(String.format("createClient; useDb:    %s", this.useDb));
		logger.warn(String.format("createClient; dbName:   %s", this.dbName));
		logger.warn(String.format("createClient; collName: %s", this.collName));
		logger.warn(String.format("createClient; collLink: %s", this.collLink));
		logger.warn(String.format("createClient; uri:      %s", uri));
		logger.warn(String.format("createClient; key:      %s", key));

		logger.warn("creating client...");
		this.client = new AsyncDocumentClient.Builder()
				.withServiceEndpoint(uri)
				.withMasterKey(key)
				.withConnectionPolicy(ConnectionPolicy.GetDefault()).withConsistencyLevel(ConsistencyLevel.Session)
				.build();

		logger.warn("client created");
		logger.warn(String.format("service endpoint: %s", this.client.getServiceEndpoint()));
	}
	
    protected String getCollLink() {

        return "dbs/" + this.dbName + "/colls/" + this.collName;
    }
    
	protected Map<String, Object> parseJsonLineToMap(String line) throws Exception {
		
		ObjectMapper mapper = new ObjectMapper();
		TypeReference<HashMap<String, Object>> typeRef 
			= new TypeReference<HashMap<String, Object>>() {};
		return mapper.readValue(line, typeRef);
	}
	
	protected String mapToJson(Map<String, Object> map) throws Exception {
		
		ObjectMapper mapper = new ObjectMapper();
		return mapper.writeValueAsString(map);
	}
	
	protected void close() {
		
        if (this.useDb) {
    		if (this.client != null) {
    			long now = getEpochSecond();
    			if (now > (lastResponseTime + 10)) {
        			logger.warn("close - closing...");
        			this.client.close();
        			logger.warn("close - closed");
    			}
    			else {
    				logger.warn(String.format("close - still active; now: %s last: %s", now, lastResponseTime));
        			pause(3000);
        			this.close();
    			}
    		}
        }
	}
	
	protected long getEpochSecond() {
		
		return Instant.now().getEpochSecond();
	}
	
	protected void pause(long ms) {

		try {
			Thread.sleep(ms);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}
