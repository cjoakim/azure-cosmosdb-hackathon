"""
Usage:
  python challenge9.py load_major_airports hackathon major_airports
  python challenge9.py query_all_major_airports hackathon major_airports
Options:
  -h --help     Show this screen.
  --version     Show version.
"""

# Python 3 program used to load and query the CosmosDB Cassandra database.
# Chris Joakim, Microsoft, 2019/04/23

import csv, json, math, os, sys, time, ssl, traceback

import arrow
import cassandra

from cassandra.auth import PlainTextAuthProvider
from cassandra.cluster import Cluster
from cassandra.policies import *
from cassandra.query import BatchType, BatchStatement, SimpleStatement
from prettytable import PrettyTable
from requests.utils import DEFAULT_CA_BUNDLE_PATH
from ssl import PROTOCOL_TLSv1_2

from docopt import docopt

VERSION='2019/04/13'


class Main:

    def __init__(self):
        self.start_time = time.time()
        self.args = sys.argv
        self.cluster = None
        self.session = None

    def execute(self):
        if len(sys.argv) > 3:
            func = sys.argv[1].lower()
            db   = sys.argv[2]
            tbl  = sys.argv[3]
            print('function: {}'.format(func))

            if func == 'load_major_airports':
                self.load_major_airports(db, tbl)

            elif func == 'query_all_major_airports':
                self.query_all_major_airports()

            else:
                self.print_options('Error: invalid function: {}'.format(func))
        else:
            self.print_options('Error: no function argument provided.')

        self.shutdown()

    def create_major_airports(self, db, tbl):
        # sample: (userid int, name text, email text, PRIMARY KEY (userid))
        # 2019/02/27: pasted this ddl into Azure Portal and created the table successfully
        ddl = '(iata_code text, city text, country text, name text, rank int, passengers int, tz_num float, tz_code text, latitude float, longitude float, altitude float, PRIMARY KEY (iata_code))'


    def load_major_airports(self, db, tbl):
        infile = 'data/major_airports.json'
        airports = self.load_json_file(infile)
        session = self.get_cassandra_session()
        print(session)

        for idx, a in enumerate(airports):
            try:
                code = a['iata_code']
                city = a['city']
                ctry = a['country']
                name = a['name']
                rank = int(a['rank'])
                pcnt = int(a['passengers'])
                tz_num = float(a['timezone_num'])
                tz_code = a['timezone_code']
                lat = float(a['latitude'])
                lng = float(a['longitude'])
                alt = float(a['altitude'])
                print('loading idx: {} iata_code: {}'.format(idx, code))

                # from cassandra.query.BatchStatement unlogged
                # <Error from server: code=0000 [Server error] message="Logged batches are not supported by the service yet. Please use unlogged batches instead.">
                # Modified the following line on 2019/02/27 - added batch_type=BatchType.UNLOGGED
                # See https://docs.datastax.com/en/drivers/python/3.2/api/cassandra/query.html
                
                batch = BatchStatement(batch_type=BatchType.UNLOGGED)  
                insert_stmt = self.session.prepare("INSERT INTO hackathon.major_airports (iata_code, city, country, name, rank, passengers, tz_num, tz_code, latitude, longitude, altitude) VALUES (?,?,?,?,?,?,?,?,?,?,?)")
                batch.add(insert_stmt, (code, city, ctry, name, rank, pcnt, tz_num, tz_code, lat, lng, alt))
                session.execute(batch)
                print('inserted')
                time.sleep(0.2)
            except:
                print('exception on airport: {} {}'.format(idx, a))
                traceback.print_exc()

    def get_cassandra_session(self):
        if self.session is not None:
            return self.session
        else:
            user_name = os.environ['AZURE_COSMOSDB_CASSDB_ACCT']
            user_pass = os.environ['AZURE_COSMOSDB_CASSDB_PASS']
            uri  = os.environ['AZURE_COSMOSDB_CASSDB_URI']
            port = os.environ['AZURE_COSMOSDB_CASSDB_PORT']
            print('get_cassandra_session - {}|{}|{}|{}'.format(user_name, user_pass, uri, port))

            ssl_opts = dict()
            ssl_opts['ca_certs'] = DEFAULT_CA_BUNDLE_PATH
            ssl_opts['ssl_version'] = PROTOCOL_TLSv1_2        
           
            auth_provider = PlainTextAuthProvider(username=user_name, password=user_pass)
            self.cluster = Cluster([uri], port=port, auth_provider=auth_provider, ssl_options=ssl_opts)  #, connect_timeout=10)
            self.session = self.cluster.connect()
            return self.session

    def major_airports_col_names(self):
        return 'iata_code,city,country,name,rank,passengers,tz_num,tz_code,latitude,longitude,altitude'.split(',')

    def query_all_major_airports(self):
        sql = 'SELECT * FROM hackathon.major_airports'
        print('query_all_major_airports: {}'.format(sql))
        session = self.get_cassandra_session()
        rows = self.session.execute(sql)

        if False:
            t = PrettyTable(self.major_airports_col_names())
            for r in rows:
                t.add_row(r)
            print(t)

        if True:
            for r in rows:
                print(r)

    def shutdown(self):
        if self.cluster is not None:
            try:
                print('cluster shutdown in progress...')
                self.cluster.shutdown()
                print('cluster shutdown completed')
            except:
                print('cluster shutdown exception')
                traceback.print_exc()

    def load_json_file(self, infile):
        with open(infile, 'r') as f:
            return json.loads(f.read())

    def id_to_pk(self, id):
        # Return the square root of the digits of the given id, cast to an int, then a string.
        return id[0] + str(int(math.sqrt(float(id[2:]))))

    def query(self, db, coll):
        pass

    def scrub_str(self, s):
        return s.replace("'", '')

    def print_options(self, msg):
        print(msg)
        arguments = docopt(__doc__, version=VERSION)
        print(arguments)


Main().execute()
