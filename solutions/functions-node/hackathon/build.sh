#!/bin/bash

# Bash shell script to compile/transpile the TypeScript code into JavaScript
# and list the output files:
# Chris Joakim, Microsoft, 2019/07/06

rm -rf dist/HttpTrigger/*.*
rm -rf dist/EventHubTrigger/*.*

# create the build_timestamp.json file with grunt
grunt

# Transpile the TypeScript into JavaScript with the tsc compiler.
# npm run-script build
# tsc --resolveJsonModule
tsc

ls -alR dist

echo 'done'
