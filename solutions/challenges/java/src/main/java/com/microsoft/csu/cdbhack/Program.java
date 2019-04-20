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
 * This is the entry-point for all Challenges executed from the command-line
 * for this Hackathon.
 *
 * @author Chris Joakim, Microsoft
 * @date   2019/04/20
 */
public class Program {

    // Class variables:
    private static String runFunction = null;

    public static void main(String[] args) throws Exception {

        if (args.length > 1) {
            runFunction = args[0];
            System.out.println("runFunction:  " + runFunction);
        }
        else {
            System.err.println("Invalid command-line args, expected runFunction");
            System.exit(1);
        }

        switch (runFunction) {

            case "challenge1":
                Challenge1 c1 = new Challenge1(args);
                c1.execute();
                break;

            case "challenge3":
                Challenge3 c3 = new Challenge3(args);
                c3.execute();
                break;

            default:
                throw new IllegalArgumentException("Invalid runFunction: " + runFunction);
        }
    }
}
