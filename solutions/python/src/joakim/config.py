# This class is responsible for returning configuration values for the app.
# The values come from either the 'config.json' file, or from environment variables.
# Chris Joakim, Microsoft, 2019/04/13

import os

from src.joakim import fs


class Config(object):

    def __init__(self):
        infile = 'config.json'
        # self.json_config = fs.FSUtil().read_json_file(infile)
        #print("Config.json_config from infile: {}".format(infile))
        #print(self.json_config)

    # def dbname(self):
    #     return self.json_config['database_name']

    # def collname(self):
    #     return self.json_config['collection_name']

    # def pk_name(self):
    #     return self.json_config['partition_key_name']

    # def pk_attribute(self):
    #     return self.json_config['partition_key_attribute']

    def cosmosdb_uri(self):
        # example: https://cjoakimcosmosddb.documents.azure.com:443/
        return os.environ['AZURE_COSMOSDB_DOCDB_URI']

    def cosmosdb_key(self):
        return os.environ['AZURE_COSMOSDB_DOCDB_KEY']
