package com.microsoft.csu.cdbhack;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.microsoft.azure.documentdb.DocumentClient;
import com.microsoft.csu.cdbhack.utils.CommandLineArgs;

import com.microsoft.csu.cdbhack.Config;
import com.microsoft.csu.cdbhack.azure.cosmosdb.DocumentDbUtil;

import com.microsoft.csu.cdbhack.utils.JsonUtil;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 * Implementation of Challenge #1.
 *
 * @author Chris Joakim, Microsoft
 * @date   2019/04/20
 */
public class Challenge1 extends Challenge  {

    // Class variables:
    private static final Logger logger = LoggerFactory.getLogger(Challenge1.class);
    private static String[] selectedCountries = {"United States", "United Kingdom", "Japan"};

    // Instance variables:
    ArrayList<HashMap> filteredCsvRows = new ArrayList<HashMap>();


    public Challenge1(String[] args) {

        super(args);
    }

    public void execute() throws Exception {

        try {
            readFilterCsvRows();
            addGeoJson();
            insertCosmosDbDocuments();
        }
        catch (Exception e) {
            e.printStackTrace();
            throw e;
        }
    }

    private void readFilterCsvRows() throws Exception {

        // AirportId,Name,City,Country,IataCode,IcaoCode,Latitude,Longitude,Altitude,TimezoneNum,Dst,TimezoneCode
        List<Map> rawCsvRows = this.readCsvFile(OPENFLIGHTS_AIRPORTS_CSV, true, ',');
        logger.warn("raw csv rows read: " + rawCsvRows.size());

        JsonUtil jsonUtil = new JsonUtil();

        for (int i = 0; i < rawCsvRows.size(); i++) {
            Map row = rawCsvRows.get(i);
            String country = (String) row.get("Country");
            if (isInSelectedCountry(country)) {
                try {
                    row.put("pk", row.get("IataCode"));
                    this.filteredCsvRows.add((HashMap) row);
                    //logger.warn(jsonUtil.pretty(row));
                }
                catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }
        logger.warn("filtered csv rows: " + this.filteredCsvRows.size());
    }

    private boolean isInSelectedCountry(String countryName) {

        if (countryName == null) {
            return false;
        }
        for (int i = 0; i < selectedCountries.length; i++) {
            if (selectedCountries[i].equalsIgnoreCase(countryName)) {
                return true;
            }
        }
        return false;
    }

    private void addGeoJson() {

        // Add GeoJSON like this to each row/airport:
        //  "location": {
        //      "type": "Point",
        //      "coordinates": [-80.943139, 35.214]
        //  }

        JsonUtil jsonUtil = new JsonUtil();

        for (int i = 0; i < this.filteredCsvRows.size(); i++) {
            Map row = this.filteredCsvRows.get(i);
            try {
                Double lat = Double.parseDouble((String) row.get("Latitude"));
                Double lng = Double.parseDouble((String) row.get("Longitude"));
                Double[] coordinates = {lng, lat};
                Map loc = new HashMap();
                loc.put("type", "Point");
                loc.put("coordinates", coordinates);
                row.put("location", loc);
                logger.warn(jsonUtil.pretty(row));
            }
            catch (NumberFormatException e) {
                e.printStackTrace();
            }
        }
    }

    private void insertCosmosDbDocuments() throws Exception {

        JsonUtil jsonUtil = new JsonUtil();

        String uri = Config.getCosmosSqlDbUri();
        String key = Config.getCosmosSqlDbKey();
        DocumentDbUtil dbUtil = new DocumentDbUtil(uri, key);

        for (int i = 0; i < this.filteredCsvRows.size(); i++) {
            Map row = this.filteredCsvRows.get(i);
            try {
                if (i < 3) {
                    dbUtil.upsertDocument("hackathon", "airports", row);
                }
                logger.warn(jsonUtil.pretty(row));
            }
            catch (NumberFormatException e) {
                e.printStackTrace();
            }
        }
    }
}
