"""
Usage:
    python main.py stub_junit_test_files
Options:
    -h --help     Show this screen.
    --version     Show version.
"""
# This class is invoked from the command-line and contains the main program logic.
# Chris Joakim, Microsoft, 2018/10/24

import json
import os
import sys
import time
import uuid

import arrow

from docopt import docopt

from pysrc.chrisjoakim import fs
from pysrc.chrisjoakim import gen

VERSION='Nov 2018'


class Main(object):

    def __init__(self, args):
        self.args = args

    def print_options(self, msg):
        print(msg)
        arguments = docopt(__doc__, version=VERSION)
        print(arguments)

    def stub_junit_test_files(self, infile, which):
        # gls > tmp/git_files_list.txt
        # python main.py stub_junit_test_files tmp/git_files_list.txt Config
        print("stub_junit_test_files: {}".format(infile))
        fsUtil = fs.FSUtil()
        lines  = fsUtil.read_text_file(infile)
        gen.Generator().generate_junit_test_stubs(which, lines)

    def execute(self):

        if len(self.args) > 1:
            func = sys.argv[1].lower()

            if func == 'stub_junit_test_files':
                infile = sys.argv[2]
                which  = sys.argv[3]
                self.stub_junit_test_files(infile, which)
                
            else:
                self.print_options('invalid function')
        else:
            self.print_options('no function given on command-line')


if __name__ == "__main__":
    Main(sys.argv).execute()
