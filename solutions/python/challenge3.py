"""
Usage:
    python challenge3.py load_azure_sql_collection <dbname> <collname> <infile>
    python challenge3.py load_azure_sql_collection hackathon airports3 data/mongoexport_airports.json
    python challenge3.py count_docs_in_collection hackathon airports3
    python challenge3.py query_by_iata_code hackathon airports3 ATL
Options:
    -h --help     Show this screen.
    --version     Show version.
"""

# Chris Joakim, Microsoft, 2019/04/24

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

            elif func == 'count_docs_in_collection':
                dbname    = sys.argv[2]
                collname  = sys.argv[3]
                self.count_docs_in_collection(dbname, collname)

            elif func == 'query_by_iata_code':
                dbname    = sys.argv[2]
                collname  = sys.argv[3]
                iata_code = sys.argv[4]
                self.query_by_iata_code(dbname, collname, iata_code)

            else:
                self.print_options('invalid function')
        else:
            self.print_options('no function given on command-line')

    def load_azure_sql_collection(self, dbname, collname, infile):
        util = cosmos.DocDbUtil(True)

        with open(infile, 'rt') as f:
            for idx, line in enumerate(f):
                if idx < 99999:
                    doc = json.loads(line)
                    del doc['_id']
                    doc['pk'] = doc['iata_code']
                    print(json.dumps(doc, sort_keys=True, indent=2))
                    try:
                        db_doc = util.insert_document(dbname, collname, doc)
                        print(db_doc)
                        time.sleep(0.2)
                    except:
                        print("Unexpected error:{}".format(sys.exc_info()[0]))
                        raise

    def count_docs_in_collection(self, dbname, collname):
        util = cosmos.DocDbUtil(True)
        query_spec = dict()
        query_spec['query'] = "SELECT VALUE COUNT(1) FROM c"
        query_spec['parameters'] = [ ]
        results = util.execute_query(dbname, collname, query_spec, enable_cross_partition=True)
        for doc in results:
            print(json.dumps(doc, sort_keys=False, indent=2))

    def query_by_iata_code(self, dbname, collname, iata_code):
        util = cosmos.DocDbUtil(False)
        query_spec = dict()
        query_spec['query'] = "SELECT * FROM c where c.pk = '{}'".format(iata_code)
        query_spec['parameters'] = [ ]
        results = util.execute_query(dbname, collname, query_spec, enable_cross_partition=False)
        for doc in results:
            print(json.dumps(doc, sort_keys=False, indent=2))


if __name__ == "__main__":
    Main(sys.argv).execute()
