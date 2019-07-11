#!/bin/bash

# Execute a search vs Azure Search with the curl program.
# Chris Joakim, Microsoft, 2019/07/11

curl -X GET \
    -H 'Content-Type: application/json' \
    -H 'Accept: application/json' \
    -H 'api-key: '$AZURE_SEARCH_QUERY_KEY \
    'https://cjoakim-search.search.windows.net/indexes/cosmosdb-index/docs?api-version=2019-05-06&search=Atlanta&$select=iata_code,name,city,latitude,longitude,altitude'
