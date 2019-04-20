#!/bin/bash

# Ad-hoc Java program execution with mvn.
# Chris Joakim, Microsoft, 2019/04/20

mvn clean compile package

class="com.microsoft.csu.cdbhack.Program"

mvn exec:java -Dexec.mainClass="com.microsoft.csu.cdbhack.Program" -Dexec.args="challenge1"

echo 'done'
