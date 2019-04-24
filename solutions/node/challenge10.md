# azure-cosmosdb-hackathon

## Challenge 10 with Node.js

```
npm install
npm list

node challenge10.js get_document_by_pk_and_id hackathon airports3 LAX 6b8f0729-60bf-4e75-8caf-70f4f905da06
...

{
  "date": "Wed, 24 Apr 2019 15:17:55 GMT",
  "signature": "3bbZbmkHRhMbiCRL/0poNvSCM9//unEzE4hpHEKoBUE=",
  "hmac": "type%3Dmaster%26ver%3D1.0%26sig%3D3bbZbmkHRhMbiCRL%2F0poNvSCM9%2F%2FunEzE4hpHEKoxxxxxx",
  "vers": "2017-02-22"
}
headers:
{
  "Authorization": "type%3Dmaster%26ver%3D1.0%26sig%3D3bbZbmkHRhMbiCRL%2F0poNvSCM9%2F%2FunEzE4hpHEKoxxxxxx",
  "x-ms-date": "Wed, 24 Apr 2019 15:17:55 GMT",
  "x-ms-version": "2017-02-22",
  "x-ms-documentdb-partitionkey": "[\"LAX\"]"
}
request options:
{
  "method": "GET",
  "url": "https://cjoakimcosmosdbsql.documents.azure.com:443/dbs/hackathon/colls/airports3/docs/6b8f0729-60bf-4e75-8caf-70f4f905da06",
  "headers": {
    "Authorization": "type%3Dmaster%26ver%3D1.0%26sig%3D3bbZbmkHRhMbiCRL%2F0poNvSCM9%2F%2FunEzE4hpHEKoxxxxxx",
    "x-ms-date": "Wed, 24 Apr 2019 15:17:55 GMT",
    "x-ms-version": "2017-02-22",
    "x-ms-documentdb-partitionkey": "[\"LAX\"]"
  }
}
response data:
{
  "statusCode": 200,
  "body": "{\"name\":\"Los Angeles Intl\",\"city\":\"Los Angeles\",\"country\":\"United States\",\"iata_code\":\"LAX\",\"latitude\":\"33.942536\",\"longitude\":\"-118.408075\",\"altitude\":\"126\",\"timezone_num\":\"-8\",\"timezone_code\":\"America/Los_Angeles\",\"location\":{\"type\":\"Point\",\"coordinates\":[-118.408075,33.942536]},\"pk\":\"LAX\",\"id\":\"6b8f0729-60bf-4e75-8caf-70f4f905da06\",\"_rid\":\"VPMYAPjD+JI5AAAAAAAAAA==\",\"_self\":\"dbs\\/VPMYAA==\\/colls\\/VPMYAPjD+JI=\\/docs\\/VPMYAPjD+JI5AAAAAAAAAA==\\/\",\"_etag\":\"\\\"020034a1-0000-0100-0000-5cc041870000\\\"\",\"_attachments\":\"attachments\\/\",\"_ts\":1556103559}",
  "headers": {
    "cache-control": "no-store, no-cache",
    "pragma": "no-cache",
    "transfer-encoding": "chunked",
    "content-type": "application/json",
    "content-location": "https://cjoakimcosmosdbsql.documents.azure.com/dbs/hackathon/colls/airports3/docs/6b8f0729-60bf-4e75-8caf-70f4f905da06",
    "server": "Microsoft-HTTPAPI/2.0",
    "strict-transport-security": "max-age=31536000",
    "x-ms-last-state-change-utc": "Wed, 24 Apr 2019 09:25:10.171 GMT",
    "etag": "\"020034a1-0000-0100-0000-5cc041870000\"",
    "x-ms-resource-quota": "documentSize=10240;documentsSize=10485760;documentsCount=-1;collectionSize=10485760;",
    "x-ms-resource-usage": "documentSize=1;documentsSize=963;documentsCount=731;collectionSize=1155;",
    "lsn": "732",
    "x-ms-schemaversion": "1.8",
    "x-ms-alt-content-path": "dbs/hackathon/colls/airports3",
    "x-ms-content-path": "VPMYAPjD+JI=",
    "x-ms-quorum-acked-lsn": "732",
    "x-ms-current-write-quorum": "3",
    "x-ms-current-replica-set-size": "4",
    "x-ms-xp-role": "1",
    "x-ms-global-committed-lsn": "732",
    "x-ms-number-of-read-regions": "0",
    "x-ms-item-lsn": "58",
    "x-ms-transport-request-id": "1",
    "x-ms-cosmos-llsn": "732",
    "x-ms-cosmos-quorum-acked-llsn": "732",
    "x-ms-cosmos-item-llsn": "58",
    "x-ms-session-token": "0:732",
    "x-ms-request-charge": "1",
    "x-ms-serviceversion": "version=2.2.0.0",
    "x-ms-activity-id": "91e9259c-4b1d-4f57-b541-af0d0ac7553f",
    "x-ms-gatewayversion": "version=2.2.0.0",
    "date": "Wed, 24 Apr 2019 15:17:55 GMT",
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
      "pathname": "/dbs/hackathon/colls/airports3/docs/6b8f0729-60bf-4e75-8caf-70f4f905da06",
      "path": "/dbs/hackathon/colls/airports3/docs/6b8f0729-60bf-4e75-8caf-70f4f905da06",
      "href": "https://cjoakimcosmosdbsql.documents.azure.com:443/dbs/hackathon/colls/airports3/docs/6b8f0729-60bf-4e75-8caf-70f4f905da06"
    },
    "method": "GET",
    "headers": {
      "Authorization": "type%3Dmaster%26ver%3D1.0%26sig%3D3bbZbmkHRhMbiCRL%2F0poNvSCM9%2F%2FunEzE4hpHEKoxxxxxx",
      "x-ms-date": "Wed, 24 Apr 2019 15:17:55 GMT",
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
  id: '6b8f0729-60bf-4e75-8caf-70f4f905da06',
  _rid: 'VPMYAPjD+JI5AAAAAAAAAA==',
  _self: 'dbs/VPMYAA==/colls/VPMYAPjD+JI=/docs/VPMYAPjD+JI5AAAAAAAAAA==/',
  _etag: '"020034a1-0000-0100-0000-5cc041870000"',
  _attachments: 'attachments/',
  _ts: 1556103559 }
```

## Notes

This solution uses the **'request' npm library** to execute the REST/HTTP call.
See https://www.npmjs.com/package/request
