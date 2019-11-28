"""
Usage:
    python challenge3.py load_azure_sql_collection <dbname> <collname> <infile>
    python challenge3.py load_azure_sql_collection hackathon airports3 data/mongoexport_airports.json
    python challenge3.py load_azure_sql_collection dev world_airports data/world_airports_flat.json --to-numerics
    python challenge3.py count_docs_in_collection hackathon airports3
    python challenge3.py query_by_iata_code hackathon airports3 ATL
    python challenge3.py delete_document dev airports ebb42868-2931-4b30-a777-632b30dff1dc BDL
    python challenge3.py list_databases
    python challenge3.py list_collections dev
Options:
    -h --help     Show this screen.
    --version     Show version.
"""

# Chris Joakim, Microsoft, 2019/11/28


import json
import os
import sys
import time
import uuid

import arrow

from docopt import docopt



from src.joakim import cosmos

VERSION='November 2019'


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

            elif func == 'list_databases':
                self.list_databases()

            elif func == 'list_collections':
                dbname    = sys.argv[2]
                self.list_collections(dbname)

            elif func == 'delete_document':
                dbname    = sys.argv[2]
                collname  = sys.argv[3]
                doc_id    = sys.argv[4]
                pk        = sys.argv[5]
                self.delete_document(dbname, collname, doc_id, pk)

            else:
                self.print_options('invalid function')
        else:
            self.print_options('no function given on command-line')

    def load_azure_sql_collection(self, dbname, collname, infile):
        util = cosmos.CosmosSqlUtil()

        with open(infile, 'rt') as f:
            for idx, line in enumerate(f):
                if idx < 99999:
                    doc = json.loads(line)
                    if '_id' in doc.keys():
                         doc['_id']
                    doc['pk'] = doc['iata_code']

                    if self.convert_to_numerics():
                        doc['latitude']  = float(doc['latitude'])
                        doc['longitude'] = float(doc['longitude'])
                        doc['altitude']  = float(doc['altitude'])
                        doc['timezone_num'] = float(doc['timezone_num'])
                    
                    print(json.dumps(doc, sort_keys=True, indent=2))

                    try:
                        db_doc = util.insert_document(dbname, collname, doc)
                        print(db_doc)
                        #time.sleep(0.2)
                    except:
                        print("Unexpected error:{}".format(sys.exc_info()[0]))
                        raise

    def convert_to_numerics(self):
        for arg in self.args:
            if arg == '--to-numerics':
                return True
        return False

    def count_docs_in_collection(self, dbname, collname):
        util = cosmos.CosmosSqlUtil()
        sql = "SELECT VALUE COUNT(1) FROM c"
        results = util.execute_query(dbname, collname, sql, True)
        for doc in results:
            print(json.dumps(doc, sort_keys=False, indent=2))

    def query_by_iata_code(self, dbname, collname, iata_code):
        util = cosmos.CosmosSqlUtil()
        sql = "SELECT * FROM c where c.pk = '{}'".format(iata_code)
        results = util.execute_query(dbname, collname, sql, False)
        for doc in results:
            print(json.dumps(doc, sort_keys=False, indent=2))

    def list_databases(self):
        util = cosmos.CosmosSqlUtil()
        results = util.list_databases()
        print(json.dumps(results, sort_keys=False, indent=2))

    def list_collections(self, dbname):
        util = cosmos.CosmosSqlUtil()
        results = util.list_collections(dbname)
        print(json.dumps(results, sort_keys=False, indent=2))

    def delete_document(self, dbname, collname, doc_id, pk):
        util = cosmos.CosmosSqlUtil()
        results = util.delete_document(dbname, collname, doc_id, pk)
        if results:
            print(json.dumps(results, sort_keys=False, indent=2))


if __name__ == "__main__":
    Main(sys.argv).execute()
