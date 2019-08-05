# Azure CLI with CosmosDB

## What is the Azure CLI?

### About

- https://docs.microsoft.com/en-us/cli/azure/get-started-with-azure-cli?view=azure-cli-latest

### Install

- https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest

## Examples

```
az webapp --help
```

```
$ az cosmosdb --help

Group
    az cosmosdb : Manage Azure Cosmos DB database accounts.

Subgroups:
    collection               : Manage Azure Cosmos DB collections.
    database                 : Manage Azure Cosmos DB databases.
    keys                     : Manage Azure Comsos DB keys.
    network-rule             : Manage Azure Comsos DB network rules.

Commands:
    check-name-exists        : Checks if an Azure Cosmos DB account name exists.
    create                   : Creates a new Azure Cosmos DB database account.
    delete                   : Deletes an Azure Cosmos DB database account.
    failover-priority-change : Changes the failover priority for the Azure Cosmos DB database
                               account.
    list                     : List Azure Cosmos DB database accounts.
    list-connection-strings  : List the connection strings for a Azure Cosmos DB database account.
    list-read-only-keys      : List the read-only access keys for a Azure Cosmos DB database
                               account.
    regenerate-key           : Regenerate an access key for a Azure Cosmos DB database account.
    show                     : Get the details of an Azure Cosmos DB database account.
    update                   : Update an Azure Cosmos DB database account.
```

```
$ az cosmosdb database --help

Group
    az cosmosdb database : Manage Azure Cosmos DB databases.

Commands:
    create : Creates an Azure Cosmos DB database.
    delete : Deletes an Azure Cosmos DB database.
    exists : Returns a boolean indicating whether the database exists.
    list   : Lists all Azure Cosmos DB databases.
    show   : Shows an Azure Cosmos DB database.
```


```
$ az cosmosdb database show -n cjoakimcosmosdbsql -d dev -g cjoakim-cosmos -d dev --output json
```

