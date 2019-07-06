import { AzureFunction, Context } from "@azure/functions"

const eventHubTrigger: AzureFunction = async function (context: Context, eventHubMessages: any[]): Promise<void> {
    context.log(`Eventhub trigger function called for message array, length: ${eventHubMessages.length}`);
    
    eventHubMessages.forEach((message, index) => {
        //context.log(`Processed message ${message}`);
        context.log('doc: ' + JSON.stringify(message));
        context.bindings.outDoc = message;  // <-- this is all that is needed to write the message to CosmosDB!
    });
};

export default eventHubTrigger;
