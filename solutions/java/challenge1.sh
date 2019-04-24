#!/bin/bash

# Execute the main Program via "mvn exec:java".
# Chris Joakim, Microsoft, 2019/04/24

mvn clean compile package

mvn exec:java -Dexec.mainClass="com.microsoft.csu.cdbhack.Program" -Dexec.args="challenge1"

echo 'done'
