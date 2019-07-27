
# Python script with the pandas and matplotlib libraries to wrangle and visualize
# candidate CosmosDB PartitionKey values from a given csv data file.
# Chris Joakim, Microsoft, 2019/07/27

import json
import os
import sys

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

def postal_codes_raw_csv_file():
    return '/Users/cjoakim/github/cj-data/postal_codes/postal_codes.csv'

def postal_codes_wrangled_csv_file():
    return '/Users/cjoakim/github/cj-data/postal_codes/postal_codes_wrangled.csv'


# python pandas_pk.py wrangle_postal_codes
# python pandas_pk.py explore_postal_codes

if __name__ == "__main__":

    if len(sys.argv) > 1:
        func = sys.argv[1].lower()

        if func == 'wrangle_postal_codes':
            df = pd.read_csv(postal_codes_raw_csv_file(), delimiter=',')
            print('')
            print('Input Data:')
            print('type(df): {}'.format(type(df)))
            #print('input df column names: {}'.format(list(df.columns.values)))
            print(df.head())

            print('')
            print('Delete the columns of the DataFrame that we dont need')
            del df['latitude']
            del df['longitude']
            #print('pruned df column names: {}'.format(list(df.columns.values)))
            print(df.head())

            print('')
            print('Add columns to the DataFrame based on other values in the same row')
            df['city_st'] = df.apply(lambda row: row.city_name + ':' + row.state_abbrv, axis=1)
            df['city_st_country'] = df.apply(lambda row: row.city_st + ':' + row.country_cd, axis=1)
            #print('augmented df column names: {}'.format(list(df.columns.values)))
            print(df.head())

            print('Writing the wrangled DataFrame to another CSV file...')
            outfile = postal_codes_wrangled_csv_file()
            df.to_csv(outfile, index=False, header=True)
            print('done')

        elif func == 'explore_postal_codes':
            df = pd.read_csv(postal_codes_wrangled_csv_file(), delimiter=',')
            print('')
            print('Input Data:')
            print('type(df): {}'.format(type(df)))
            print('input df column names: {}'.format(list(df.columns.values)))
            # id,postal_cd,country_cd,city_name,state_abbrv,city_st,city_st_country
            print(df.head())

            print('input df column names: {}'.format(list(df.columns.values)))
            col_names = list(df.columns.values)
            for col_name in col_names:
                print('number of unique values in column : {:15} -> {:8}'.format(
                    col_name, df[col_name].nunique()))

            try:
                candidate_columns = 'country_cd,state_abbrv,city_name,city_st,city_st_country,postal_cd'.split(',')
                print(candidate_columns)
                for candidate_column in candidate_columns:
                    key = input('Press any key for plot by {} ...'.format(candidate_column))
                    pd.value_counts(df[candidate_column]).plot()
                    plt.title('Value Distribution by {}'.format(candidate_column))
                    plt.savefig('plots/by_{}_orig.pdf'.format(candidate_column))
                    plt.show()
            except KeyboardInterrupt:
                print("\nterminating\n")
                sys.exit()
    else:
        print("no command-line function given")