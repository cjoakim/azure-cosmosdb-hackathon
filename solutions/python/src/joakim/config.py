# This class is responsible for returning configuration values for the app.
# Chris Joakim, Microsoft, 2019/04/24

import os

from src.joakim import fs


class Config(object):

    def __init__(self):
        pass

    def cosmosdb_uri(self):
        # example: https://cjoakimcosmosddb.documents.azure.com:443/
        return os.environ['AZURE_COSMOSDB_SQLDB_URI']

    def cosmosdb_key(self):
        return os.environ['AZURE_COSMOSDB_SQLDB_KEY']
