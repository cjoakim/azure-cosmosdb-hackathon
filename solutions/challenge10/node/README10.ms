# azure-cosmosdb-samples - rest

## Links

- https://docs.microsoft.com/en-us/rest/api/azure/
- https://docs.microsoft.com/en-us/rest/api/cosmos-db/
- https://docs.microsoft.com/en-us/rest/api/cosmos-db/access-control-on-cosmosdb-resources 
- https://docs.microsoft.com/en-us/rest/api/cosmos-db/restful-interactions-with-cosmosdb-resources
- https://docs.microsoft.com/en-us/rest/api/cosmos-db/common-cosmosdb-rest-request-headers
- https://docs.microsoft.com/en-us/rest/api/cosmos-db/querying-cosmosdb-resources-using-the-rest-api
- https://github.com/request/request
- https://docs.microsoft.com/en-us/python/api/overview/azure/cosmosdb?view=azure-python

## Authorization

Access to resources in the SQL API is governed by a **master key token** or a resource token.
To access a resource, the selected token is included in the REST authorization header, as part of the authorization string.

A HMAC key is generated for each HTTP request with the REST API.

### Authorization Header

Format:
```
type={typeoftoken}&ver={tokenversion}&sig={hashsignature}

{typeoftoken} denotes the type of token: master or resource.
{tokenversion} denotes the version of the token, currently 1.0.
{hashsignature} denotes the hashed token signature.
```
Example:
type=master&ver=1.0&sig=5mDuQBYA0kb70WDJoTUzSBMTG3owkC0/cEN4fqa18/s=


## Example Command-Line Use

See Node.js script **main.js** in this directory.  Notice how it generates a HMAC key
for each HTTP request.  Notice also the various HTTP Headers that are set.
The npm **request** library is used to make the actual HTTP requests.

Execute the following command in Terminal to get the document with **id** 72d3d5e7-313d-4c03-ae6c-f6a330e9fcb8
with **partition key** CLT in **collection** airports within **database** dev. 

Assumes that the following environment variables have been set:
- AZURE_COSMOSDB_SQLDB_URI  (for example: https://cjoakim-cosmosdb-sql.documents.azure.com:443/)
- AZURE_COSMOSDB_SQLDB_KEY  (for example: 5FdF3Wcg9TB7ON7T ... RfUSicw== )

See Settings -> Keys in your CosmosDB account in Azure Portal to get the above two values.

```
$ npm install

$ node main.js get_document_by_pk_and_id dev airports CLT 72d3d5e7-313d-4c03-ae6c-f6a330e9fcb8

request options:
{
  "method": "GET",
  "url": "https://cjoakim-cosmosdb-sql.documents.azure.com:443/dbs/dev/colls/airports/docs/72d3d5e7-313d-4c03-ae6c-f6a330e9fcb8",
  "headers": {
    "Authorization": "type%3Dmaster%26ver%3D1.0%26sig%3DXlNuPapfts ... Qx07%2BqJp4wN%2B2A%3D",
    "x-ms-date": "Sat, 15 Dec 2018 20:54:26 GMT",
    "x-ms-version": "2017-02-22",
    "x-ms-documentdb-partitionkey": "[\"CLT\"]"
  }
}

response data:
{ name: 'Charlotte Douglas Intl',
  city: 'Charlotte',
  country: 'United States',
  iata_code: 'CLT',
  latitude: '35.214',
  longitude: '-80.943139',
  altitude: '748',
  timezone_num: '-5',
  timezone_code: 'America/New_York',
  location: { type: 'Point', coordinates: [ -80.943139, 35.214 ] },
  pk: 'CLT',
  seq: 3778,
  last_update: 0,
  temperature: 20.35801744984134,
  humidity: 91.04754455082039,
  id: '72d3d5e7-313d-4c03-ae6c-f6a330e9fcb8',
  _rid: '8SxQAKvbYoXvAQAAAAAAAA==',
  _self: 'dbs/8SxQAA==/colls/8SxQAKvbYoU=/docs/8SxQAKvbYoXvAQAAAAAAAA==/',
  _etag: '"0000f550-0000-0100-0000-5c151b2e0000"',
  _attachments: 'attachments/',
  _ts: 1544887086 }
```
