import { AzureFunction, Context, HttpRequest } from "@azure/functions"

import * as data from '../build_timestamp.json';
const build_timestamp = data['build_timestamp'];  

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    context.log('HTTP trigger function processed a request.');
    const name = (req.query.name || (req.body && req.body.name));

    if (name) {
        context.log('name: ' + name);
        context.log('build_timestamp: ' + build_timestamp);

        // Write a document to CosmosDB
        var doc = {};
        var date = new Date();
        var epoch = date.getTime();
        doc['pk'] = name + '-' + epoch;  // pk is the Partition-Key attribute in the CosmosDB collection
        doc['name'] = name;
        doc['date'] = date.toDateString();
        doc['epoch'] = date.getTime();
        doc['build_timestamp'] = build_timestamp;
        doc['function_name'] = context.executionContext.functionName;
        doc['function_invocation_id'] = context.executionContext.invocationId;
        doc['function_directory'] = context.executionContext.functionDirectory;

        var jstr = JSON.stringify(doc, null, 2);
        context.log('doc: ' + JSON.stringify(doc));
        context.bindings.outDoc = doc;  // <-- this is all that is needed to write the document to CosmosDB!

        context.res = {
            // status: 200, /* Defaults to 200 */
            //body: "Hello " + (req.query.name || req.body.name)
            body: jstr
        };
    }
    else {
        context.res = {
            status: 400,
            body: "Please pass a name on the query string or in the request body"
        };
    }
};

export default httpTrigger;
