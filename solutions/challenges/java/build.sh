#!/bin/bash

# Compile and package the Java code with Maven.
# Chris Joakim, Microsoft, 2019/04/20

date > src/main/resources/build_date.txt

mvn clean compile package

jar tvf target/azure-java-1.0-SNAPSHOT.jar > tmp/jar-contents.txt

echo 'done'
