# This class performs CRUD operations vs CosmosDB with SQL/DocumentDB API.
# This class now uses the new Microsoft open-source library for CosmosDB w/SQL API.
# See https://pypi.org/project/azure-cosmos/
# See https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-sql-query
# Chris Joakim, Microsoft, 2019/11/28

import os
import sys

from src.joakim import config

import azure.cosmos.cosmos_client as cosmos_client
import azure.cosmos.errors as errors
import azure.cosmos.http_constants as http_constants


class CosmosSqlUtil(object):

    def __init__(self):
        c = config.Config()
        host = c.cosmosdb_uri()
        key  = c.cosmosdb_key()
        self.client = cosmos_client.CosmosClient(host, {'masterKey': key})

    def db_link(self, dbname):
        return 'dbs/' + dbname

    def collection_link(self, dbname, cname):
        return self.db_link(dbname) + '/colls/' + cname

    def document_link(self, dbname, cname, id):
        return self.collection_link(dbname, cname) + '/docs/' + id

    def list_databases(self):
        return list(self.client.ReadDatabases())

    def list_collections(self, dbname):
        db_link = self.db_link(dbname)
        return list(self.client.ReadContainers(db_link))

    def insert_document(self, dbname, cname, doc):
        link = self.collection_link(dbname, cname)
        try:
            return self.client.CreateItem(link, doc)
        except:
            print("Unexpected error:{}".format(sys.exc_info()[0]))
            raise

    def delete_document(self, dbname, cname, doc_id, pk):
        link = self.document_link(dbname, cname, doc_id)
        opts = dict()
        opts['partitionKey'] = pk
        try:
            return self.client.DeleteItem(link, opts)
        except:
            print("Unexpected error:{}".format(sys.exc_info()[0]))
            raise

    def execute_query(self, dbname, cname, sql, enable_cross_partition=False):
        opts = {'enableCrossPartitionQuery': enable_cross_partition}
        link = self.collection_link(dbname, cname)
        print('execute_query; link: {} sql: {} opts: {}'.format(link, sql, opts))
        try:
            return list(self.client.QueryItems(link, sql, opts))
        except:
            print("Unexpected error:{}".format(sys.exc_info()[0]))
            raise
