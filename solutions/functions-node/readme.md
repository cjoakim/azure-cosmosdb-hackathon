# azure-cosmosdb-hackathon - Azure Functions

Sample Azure Function App, created with CLI tools, and written in TypeScript for Node.js runtime

![functionsapp](img/functionsapp.png)

## Azure Functions

**Azure Functions = Serverless, Event-Driven Compute Service**

Author code in **C#, F#, JavaScript, TypeScript, Java, PowerShell, or Python**

Multiple Deployment Options: CI/CD from repository, Azure DevOps, Zip/CLI, etc

## Links

- https://docs.microsoft.com/en-us/azure/azure-functions/
- https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local
- https://docs.microsoft.com/en-us/azure/azure-functions/scripts/functions-cli-create-serverless
- https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-node
- https://www.typescriptlang.org/index.html
- https://code.visualstudio.com/docs/typescript/typescript-tutorial

## Setup

### Create an Azure Function App

In Azure Portal, you can provision a new Function App like this:

![provision-function-app](img/provision-function-app.png)

Alternatively, you can create a Function app via the CLI like this:

```
#!/bin/bash

# Function app and storage account names must be unique.
storageName=mystorageaccount$RANDOM
functionAppName=myserverlessfunc$RANDOM

# Create a resource group.
az group create --name myResourceGroup --location westeurope

# Create an Azure storage account in the resource group.
az storage account create \
  --name $storageName \
  --location westeurope \
  --resource-group myResourceGroup \
  --sku Standard_LRS

# Create a serverless function app in the resource group.
az functionapp create \
  --name $functionAppName \
  --storage-account $storageName \
  --consumption-plan-location westeurope \
  --resource-group myResourceGroup
```

### Windows Development Workstation:

```
npm install -g azure-functions-core-tools
```

### macOS Development Workstation:

```
$ brew tap azure/functions
$ brew install azure-functions-core-tools
```

### Additional npm libraries:

I personally use the JavaScript **grunt** tool in most projects; it isn't required for Azure Function apps.
```
$ npm install -g grunt
$ npm install -g grunt-cli
```

### GitHub

Create a Node.js repo 'azure-functions-cli-typescript', then:
```
git clone git@github.com:cjoakim/azure-functions-cli-typescript.git
```

### Create Project with the CLI tools

```
$ func init hackathon
Select a worker runtime:
1. dotnet
2. node
3. python (preview)
4. powershell (preview)
Choose option: 2
node
Select a Language:
1. javascript
2. typescript
Choose option: 2
typescript
Writing .funcignore
Writing package.json
Writing tsconfig.json
Writing .gitignore
Writing host.json
Writing local.settings.json
Writing /Users/cjoakim/github/azure-cosmosdb-hackathon/solutions/functions-node/hackathon/.vscode/extensions.json
```

Then add the extensionBundle to **host.json**, like this:
```
{
    "version": "2.0",
    "extensionBundle": {
        "id": "Microsoft.Azure.Functions.ExtensionBundle",
        "version": "[1.*, 2.0.0)"
    }
}
```

Note that a **.gitignore** file was created.  It includes several entries, including:
```
...
appsettings.json
local.settings.json
...
```

Notice also that a **tsconfig.json** file was created, since **TypeScript** 
was specified as the programming language.

Get your function app settings from Azure, this updates your git-ignored **local.settings.json** file.
```
$ cd hackathon

$ npm install
$ npm install --save-dev grunt grunt-cli load-grunt-tasks   <-- additional libraries I use

$ func azure functionapp fetch-app-settings cjoakim-functions-js
App Settings:
Loading FUNCTIONS_WORKER_RUNTIME = *****
Loading AzureWebJobsStorage = *****
Loading FUNCTIONS_EXTENSION_VERSION = *****
Loading WEBSITE_NODE_DEFAULT_VERSION = *****
Loading APPINSIGHTS_INSTRUMENTATIONKEY = *****

Connection Strings:

$ cat local.settings.json
{
  "IsEncrypted": false,
  "Values": {
    "FUNCTIONS_WORKER_RUNTIME": "node",
    "AzureWebJobsStorage": "DefaultEndpointsProtocol=https;AccountName=cjoakimfunctionsjs;AccountKey=rEZfZ+/GBOEKZEDYSiLkEw/mWzsyjU0eb9fFZrBDK/PGyh9yo0GPaaHCswNaV1qQtADC6c7787UQqRvFRRvSOg==",
    "FUNCTIONS_EXTENSION_VERSION": "~2",
    "WEBSITE_NODE_DEFAULT_VERSION": "10.14.1",
    "APPINSIGHTS_INSTRUMENTATIONKEY": "f59f1dbf-e9e9-465c-9a56-38d2df375117"
  },
  "ConnectionStrings": {}
}
```

Also get the storage connection string:
```
$ func azure storage fetch-connection-string cjoakimfunctionsjs
Secret saved locally in local.settings.json under name cjoakimfunctionsjs_STORAGE.
```

### Create, Develop, and Test a HTTP Trigger Function on your Workstation

Create a HTTP request triggered Function:
```
$ func new
Select a template:
1. Azure Blob Storage trigger
2. Azure Cosmos DB trigger
3. Durable Functions activity
4. Durable Functions HTTP starter
5. Durable Functions orchestrator
6. Azure Event Grid trigger
7. Azure Event Hub trigger
8. HTTP trigger                        <--
9. IoT Hub (Event Hub)
10. Azure Queue Storage trigger
11. SendGrid
12. Azure Service Bus Queue trigger
13. Azure Service Bus Topic trigger
14. Timer trigger
Choose option: 8
HTTP trigger
Function name: [HttpTrigger]
Writing /Users/cjoakim/github/azure-cosmosdb-hackathon/solutions/functions-node/hackathon/HttpTrigger/index.ts
Writing /Users/cjoakim/github/azure-cosmosdb-hackathon/solutions/functions-node/hackathon/HttpTrigger/function.json
The function "HttpTrigger" was created successfully from the "HTTP trigger" template.
```

Create an EventHub message triggered Function:
```
$ func new
Select a template:
1. Azure Blob Storage trigger
2. Azure Cosmos DB trigger
3. Durable Functions activity
4. Durable Functions HTTP starter
5. Durable Functions orchestrator
6. Azure Event Grid trigger
7. Azure Event Hub trigger             <--
8. HTTP trigger
9. IoT Hub (Event Hub)
10. Azure Queue Storage trigger
11. SendGrid
12. Azure Service Bus Queue trigger
13. Azure Service Bus Topic trigger
14. Timer trigger
Choose option: 7
Azure Event Hub trigger
Function name: [EventHubTrigger]
Writing /Users/cjoakim/github/azure-cosmosdb-hackathon/solutions/functions-node/hackathon/EventHubTrigger/index.ts
Writing /Users/cjoakim/github/azure-cosmosdb-hackathon/solutions/functions-node/hackathon/EventHubTrigger/function.json
The function "EventHubTrigger" was created successfully from the "Azure Event Hub trigger" template.
```

#### Compile/Transpile the TypeScript into JavaScript

```
$ npm run-script build
```

Alternatively, execute my **build.sh** script, which lists the output files:
```
$ ./build.sh
Running "build-timestamp" task
build-timestamp: Sat Jul 06 2019 06:45:04 GMT-0400 (EDT)

Done.
total 0
drwxr-xr-x   4 cjoakim  staff  128 Jul  6 06:43 .
drwxr-xr-x  21 cjoakim  staff  672 Jul  6 06:45 ..
drwxr-xr-x   4 cjoakim  staff  128 Jul  6 06:45 EventHubTrigger
drwxr-xr-x   4 cjoakim  staff  128 Jul  6 06:45 HttpTrigger

dist/EventHubTrigger:
total 16
drwxr-xr-x  4 cjoakim  staff   128 Jul  6 06:45 .
drwxr-xr-x  4 cjoakim  staff   128 Jul  6 06:43 ..
-rw-r--r--  1 cjoakim  staff  1097 Jul  6 06:45 index.js
-rw-r--r--  1 cjoakim  staff   400 Jul  6 06:45 index.js.map

dist/HttpTrigger:
total 16
drwxr-xr-x  4 cjoakim  staff   128 Jul  6 06:45 .
drwxr-xr-x  4 cjoakim  staff   128 Jul  6 06:43 ..
-rw-r--r--  1 cjoakim  staff  1371 Jul  6 06:45 index.js
-rw-r--r--  1 cjoakim  staff   629 Jul  6 06:45 index.js.map
done
```

#### Test the Function App Locally on your Workstation

```
$ func host start

                  %%%%%%
                 %%%%%%
            @   %%%%%%    @
          @@   %%%%%%      @@
       @@@    %%%%%%%%%%%    @@@
     @@      %%%%%%%%%%        @@
       @@         %%%%       @@
         @@      %%%       @@
           @@    %%      @@
                %%
                %

Azure Functions Core Tools (2.7.1373 Commit hash: cd9bfca26f9c7fe06ce245f5bf69bc6486a685dd)
Function Runtime Version: 2.0.12507.0
[6/14/19 2:53:25 PM] Starting Rpc Initialization Service.
[6/14/19 2:53:25 PM] Initializing RpcServer
[6/14/19 2:53:25 PM] Building host: startup suppressed:False, configuration suppressed: False
[6/14/19 2:53:25 PM] Initializing Host.
...
Content root path: /Users/cjoakim/github/azure-functions-cli-typescript/project
Now listening on: http://0.0.0.0:7071
Application started. Press Ctrl+C to shut down.

Http Functions:

	HttpTrigger: [GET,POST] http://localhost:7071/api/HttpTrigger
```

Invoke the HTTP Function with curl:
```
$ curl http://localhost:7071/api/HttpTrigger?name=MollyMcKay
Hello MollyMcKay
```

### Deploy to Azure

```
$ func azure functionapp publish cjoakim-functions-js
Getting site publishing info...
Creating archive for current directory...
Uploading 10.46 MB [##############################################################################]
Upload completed successfully.
Deployment completed successfully.
Syncing triggers...
Functions in cjoakim-functions-js:
    HttpTrigger - [httpTrigger]
        Invoke url: https://cjoakim-functions-js.azurewebsites.net/api/httptrigger?code=...secret...==
```

Invoke the deployed Function with curl:
```
$ curl "https://cjoakim-functions-js.azurewebsites.net/api/httptrigger?code=...secret...==&name=Miles"
Hello Miles
```

Alternatively, see deploy.sh

#### Iterate - edit, compile, deploy

```
$ ./build.sh

$ func azure functionapp publish cjoakim-functions-js
Getting site publishing info...
Creating archive for current directory...
Uploading 10.46 MB [##############################################################################]
Upload completed successfully.
Deployment completed successfully.
Syncing triggers...
Functions in cjoakim-functions-js:
    HttpTrigger - [httpTrigger]
        Invoke url: https://cjoakim-functions-js.azurewebsites.net/api/httptrigger?code=...secret...==
```

### Add CosmosDB Integration

For each HTTP request, log a document to CosmosDB.

First, add an output binding of type **cosmosDB** to file **function.json**.  The CosmosDB 
connection string value will be obtained from **environment variable** named **cjoakimcosmosdbsql_DOCUMENTDB**.
```
    {
      "type": "cosmosDB",
      "name": "outDoc",
      "databaseName": "dev",
      "collectionName": "function",
      "createIfNotExists": false,
      "connectionStringSetting": "cjoakimcosmosdbsql_DOCUMENTDB",
      "partitionKey": "/pk",
      "direction": "out"
    }
```

Next, add an entry to your **local.settings.json** file like this.  Note that cjoakimcosmosdbsql
is my CosmosDB account name; specify your database account name and key.
```
"cjoakimcosmosdbsql_DOCUMENTDB": "AccountEndpoint=https://cjoakimcosmosdbsql.documents.azure.com:443/;AccountKey=...secret...==;"
```

Next, edit and recompile the application code.  Add the following:
```
    // Write a document to CosmosDB
    var doc = {};
    var date = new Date();
    var epoch = date.getTime();
    doc['pk'] = name + '-' + epoch;  // pk is the Partition-Key attribute in the CosmosDB collection
    doc['date'] = date.toDateString();
    doc['epoch'] = date.getTime();
    context.log('doc: ' + JSON.stringify(doc));
    context.bindings.outDoc = doc;  // <-- this is all that is needed to write the document to CosmosDB
```

Next, test the Function locally:
```
$ local_test.sh

$ curl "http://localhost:7071/api/HttpTrigger?name=miles"
```

You should then see documents added to your CosmosDB like this:
```
{
    "pk": "miles-1560787893266",
    "name": "miles",
    "date": "Mon Jun 17 2019",
    "epoch": 1560787893266,
    "build_timestamp": "Mon Jun 17 2019 12:11:01 GMT-0400 (EDT)",
    "m26_version": "0.4.0",
    "id": "14a39bed-7f89-4973-aedb-de3a9ae55769",
    "_rid": "M2ZjALte+XAMAAAAAAAAAA==",
    "_self": "dbs/M2ZjAA==/colls/M2ZjALte+XA=/docs/M2ZjALte+XAMAAAAAAAAAA==/",
    "_etag": "\"f901d95f-0000-0100-0000-5d07bbb60000\"",
    "_attachments": "attachments/",
    "_ts": 1560787894
}
```

Deploy to Azure after **updating your Function App settings**, add environment variable
cjoakimcosmosdbsql_DOCUMENTDB as was done to local.settings.json.

![function-app-settings](img/function-app-settings.png)

```
$ func azure functionapp publish cjoakim-functions-js
```

### TypeScript and Visual Studio Code

Visual Studio Code is an excellent lightweight, free, and cross-platform editor.
It, itself, is built with Node.js, TypeScript, and JavaScript.

It offers many features, including **IntelliSense**, as shown below:

![vsc-intellisense](img/code-intellisense.png)
