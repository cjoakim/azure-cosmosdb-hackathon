package com.microsoft.csu.cdbhack;

import java.util.Map;

/**
 * This class returns configuration values specified as environment variables.
 *
 * See https://12factor.net
 * See https://12factor.net/config
 *
 * @author Chris Joakim, Microsoft
 * @date   2019/04/20
 */

public class Config implements EnvVarNames {

    public static synchronized Map<String, String> envVars() {

        return System.getenv();
    }

    public static synchronized String envVar(String name) {

        return envVars().get(name);
    }

    public static String getCosmosSqlDbAcct() {

        return System.getenv(AZURE_COSMOSDB_SQLDB_ACCT);
    }

    public static String getCosmosSqlDbKey() {

        return System.getenv(AZURE_COSMOSDB_SQLDB_KEY);
    }

    public static String getCosmosSqlDbUri() {

        return System.getenv(AZURE_COSMOSDB_SQLDB_URI);
    }
    
    public static String getCosmosSqlDbName() {

        return System.getenv(AZURE_COSMOSDB_SQLDB_DBNAME);
    }

    public static String getCosmosSqlDbDefaultCollName() {

        return System.getenv(AZURE_COSMOSDB_SQLDB_COLLNAME);
    }
    
    public static String getOpenWeatherMapKey() {

        return System.getenv(AZURE_OPENWEATHERMAP_KEY);
    }

    public static synchronized String storageConnectionString() {

        String acctName = System.getenv(AZURE_STORAGE_ACCOUNT);
        String acctKey  = System.getenv(AZURE_STORAGE_KEY);
        
        String s = System.getenv(AZURE_STORAGE_CONNECTION_STRING);
        // return String.format("DefaultEndpointsProtocol=https;AccountName=%s;AccountKey=%s", acctName, acctKey);
        return s;
    }

}
