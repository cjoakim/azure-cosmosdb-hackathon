# azure-cosmosdb-hackathon

## Challenge 10 with Node.js

```
npm install
npm list

node main.js get_document_by_pk_and_id hackathon airports3 LAX 4edc3477-fd20-4dff-975d-123a11adf192

...

request options:
{
  "method": "GET",
  "url": "https://cjoakimcosmosdbsql.documents.azure.com:443/dbs/hackathon/colls/airports3/docs/4edc3477-fd20-4dff-975d-123a11adf192",
  "headers": {
    "Authorization": "type%3Dmaster%26ver%3D1.0%26sig%3DmL8lztnaiU75BrjNlamNNXeYNOAyDPWR02PlHVhjDXY%3D",
    "x-ms-date": "Sun, 14 Apr 2019 00:15:19 GMT",
    "x-ms-version": "2017-02-22",
    "x-ms-documentdb-partitionkey": "[\"LAX\"]"
  }
}
response data:
{
  "statusCode": 200,
  "body": "{\"name\":\"Los Angeles Intl\",\"city\":\"Los Angeles\",\"country\":\"United States\",\"iata_code\":\"LAX\",\"latitude\":\"33.942536\",\"longitude\":\"-118.408075\",\"altitude\":\"126\",\"timezone_num\":\"-8\",\"timezone_code\":\"America/Los_Angeles\",\"location\":{\"type\":\"Point\",\"coordinates\":[-118.408075,33.942536]},\"pk\":\"LAX\",\"id\":\"4edc3477-fd20-4dff-975d-123a11adf192\",\"_rid\":\"VPMYAKvQA9M5AAAAAAAAAA==\",\"_self\":\"dbs\\/VPMYAA==\\/colls\\/VPMYAKvQA9M=\\/docs\\/VPMYAKvQA9M5AAAAAAAAAA==\\/\",\"_etag\":\"\\\"0500570d-0000-0100-0000-5cb2706b0000\\\"\",\"_attachments\":\"attachments\\/\",\"_ts\":1555198059}",
  "headers": {
    "cache-control": "no-store, no-cache",
    "pragma": "no-cache",
    "transfer-encoding": "chunked",
    "content-type": "application/json",
    "content-location": "https://cjoakimcosmosdbsql.documents.azure.com/dbs/hackathon/colls/airports3/docs/4edc3477-fd20-4dff-975d-123a11adf192",
    "server": "Microsoft-HTTPAPI/2.0",
    "strict-transport-security": "max-age=31536000",
    "x-ms-last-state-change-utc": "Sat, 13 Apr 2019 14:44:28.359 GMT",
    "etag": "\"0500570d-0000-0100-0000-5cb2706b0000\"",
    "x-ms-resource-quota": "documentSize=10240;documentsSize=10485760;documentsCount=-1;collectionSize=10485760;",
    "x-ms-resource-usage": "documentSize=0;documentsSize=672;documentsCount=731;collectionSize=864;",
    "lsn": "732",
    "x-ms-schemaversion": "1.7",
    "x-ms-alt-content-path": "dbs/hackathon/colls/airports3",
    "x-ms-content-path": "VPMYAKvQA9M=",
    "x-ms-xp-role": "1",
    "x-ms-global-committed-lsn": "731",
    "x-ms-number-of-read-regions": "0",
    "x-ms-item-lsn": "58",
    "x-ms-transport-request-id": "1",
    "x-ms-cosmos-llsn": "732",
    "x-ms-cosmos-item-llsn": "58",
    "x-ms-session-token": "0:732",
    "x-ms-request-charge": "1",
    "x-ms-serviceversion": "version=2.2.0.0",
    "x-ms-activity-id": "e5604340-34d7-496a-b547-1b5dcb36e029",
    "x-ms-gatewayversion": "version=2.2.0.0",
    "date": "Sun, 14 Apr 2019 00:15:19 GMT",
    "connection": "close"
  },
  "request": {
    "uri": {
      "protocol": "https:",
      "slashes": true,
      "auth": null,
      "host": "cjoakimcosmosdbsql.documents.azure.com:443",
      "port": "443",
      "hostname": "cjoakimcosmosdbsql.documents.azure.com",
      "hash": null,
      "search": null,
      "query": null,
      "pathname": "/dbs/hackathon/colls/airports3/docs/4edc3477-fd20-4dff-975d-123a11adf192",
      "path": "/dbs/hackathon/colls/airports3/docs/4edc3477-fd20-4dff-975d-123a11adf192",
      "href": "https://cjoakimcosmosdbsql.documents.azure.com:443/dbs/hackathon/colls/airports3/docs/4edc3477-fd20-4dff-975d-123a11adf192"
    },
    "method": "GET",
    "headers": {
      "Authorization": "type%3Dmaster%26ver%3D1.0%26sig%3DmL8lztnaiU75BrjNlamNNXeYNOAyDPWR02PlHVhjDXY%3D",
      "x-ms-date": "Sun, 14 Apr 2019 00:15:19 GMT",
      "x-ms-version": "2017-02-22",
      "x-ms-documentdb-partitionkey": "[\"LAX\"]"
    }
  }
}
response body:
{ name: 'Los Angeles Intl',
  city: 'Los Angeles',
  country: 'United States',
  iata_code: 'LAX',
  latitude: '33.942536',
  longitude: '-118.408075',
  altitude: '126',
  timezone_num: '-8',
  timezone_code: 'America/Los_Angeles',
  location: { type: 'Point', coordinates: [ -118.408075, 33.942536 ] },
  pk: 'LAX',
  id: '4edc3477-fd20-4dff-975d-123a11adf192',
  _rid: 'VPMYAKvQA9M5AAAAAAAAAA==',
  _self: 'dbs/VPMYAA==/colls/VPMYAKvQA9M=/docs/VPMYAKvQA9M5AAAAAAAAAA==/',
  _etag: '"0500570d-0000-0100-0000-5cb2706b0000"',
  _attachments: 'attachments/',
  _ts: 1555198059 }
```
