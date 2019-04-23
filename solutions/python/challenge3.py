"""
Usage:
    python challenge3.py load_azure_sql_collection <dbname> <collname> <infile>
    python challenge3.py load_azure_sql_collection hackathon airports3 data/mongoexport_airports.json
Options:
    -h --help     Show this screen.
    --version     Show version.
"""

# Chris Joakim, Microsoft, 2019/04/23

import json
import os
import sys
import time
import uuid

import arrow

from docopt import docopt

# Microsoft open-source library for CosmosDB w/SQL API
import pydocumentdb.document_client as document_client 

from src.joakim import cosmos

VERSION='April 2019'


class Main(object):

    def __init__(self, args):
        self.args = args

    def print_options(self, msg):
        print(msg)
        arguments = docopt(__doc__, version=VERSION)
        print(arguments)

    def execute(self):

        if len(self.args) > 1:
            func = sys.argv[1].lower()

            if func == 'load_azure_sql_collection':
                dbname   = sys.argv[2]
                collname = sys.argv[3]
                infile   = sys.argv[4]
                self.load_azure_sql_collection(dbname, collname, infile)

            else:
                self.print_options('invalid function')
        else:
            self.print_options('no function given on command-line')

    def load_azure_sql_collection(self, dbname, collname, infile):
        host   = os.getenv('AZURE_COSMOSDB_SQLDB_URI')
        key    = os.getenv('AZURE_COSMOSDB_SQLDB_KEY')
        colllink = self.sql_collection_link(dbname, collname)
        client = document_client.DocumentClient(host, {'masterKey': key})
        client.default_headers['x-ms-documentdb-query-enablecrosspartition'] = True

        with open(infile, 'rt') as f:
            for idx, line in enumerate(f):
                if idx < 99999:
                    doc = json.loads(line)
                    del doc['_id']
                    doc['pk'] = doc['iata_code']
                    print(json.dumps(doc, sort_keys=True, indent=2))
                    try:
                        db_doc = client.UpsertDocument(colllink, doc)
                        print(db_doc)
                        time.sleep(0.2)
                    except:
                        print("Unexpected error:{}".format(sys.exc_info()[0]))
                        raise

    def sql_collection_link(self, dbname, collname):
        return 'dbs/' + dbname + '/colls/' + collname;


if __name__ == "__main__":
    Main(sys.argv).execute()
