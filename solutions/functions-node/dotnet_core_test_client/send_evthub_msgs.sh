#!/bin/bash

# Bash shell script to execute send 5 sets of random event messages to the Azure EventHub.
# Usage: 
#   ./send_evthub_msgs.sh <count> <sleep-seconds>
#   ./send_evthub_msgs.sh 20 5
# Chris Joakim, Microsoft, 2019/07/08

echo 'sending set 1, count '$1
dotnet run send_event_hub_messsages $1
echo 'sleeping '$2
sleep $2

echo 'sending set 2, count '$1
dotnet run send_event_hub_messsages $1
echo 'sleeping '$2
sleep $2

echo 'sending set 3, count '$1
dotnet run send_event_hub_messsages $1
echo 'sleeping '$2
sleep $2

echo 'sending set 4, count '$1
echo 'sleeping '$2
sleep $2

echo 'sending set 5, count '$1
dotnet run send_event_hub_messsages $1

echo 'done'
