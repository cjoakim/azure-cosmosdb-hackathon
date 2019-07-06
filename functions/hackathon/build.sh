#!/bin/bash

# Bash shell script to compile/transpile the TypeScript code into JavaScript
# and list the output files:
# Chris Joakim, Microsoft, 2019/06/14

rm -rf dist/HttpTrigger/*.*
rm -rf dist/EventHubTrigger/*.*

# create the build_timestamp.json file with grunt
grunt

# npm run-script build
tsc --resolveJsonModule

ls -alR dist

echo 'done'
