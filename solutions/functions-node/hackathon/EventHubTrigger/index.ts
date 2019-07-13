import { AzureFunction, Context } from "@azure/functions"

const eventHubTrigger: AzureFunction = async function (context: Context, eventHubMessages: any[]): Promise<void> {
    context.log(`EventHubTrigger message array length: ${eventHubMessages.length}`);
    
    eventHubMessages.forEach((message, index) => {
        context.bindings.outDoc = message;  
    });

    context.done();
};

export default eventHubTrigger;
