#!/bin/bash

# Recreate the virtual environment and reinstall libs.
# Requires Python 3; version 3.7 or higher recommended.
# Chris Joakim, Microsoft, 2019/04/23

echo 'deleting previous venv...'
rm -rf bin/
rm -rf include/
rm -rf lib/
rm -rf man/

echo 'creating new venv ...'
python3 -m venv .
source bin/activate
python --version
pip --version

echo 'installing/upgrading pip...'
pip install --upgrade pip
pip --version

echo 'installing/upgrading pip-tools...'
pip install --upgrade pip-tools

echo 'pip-compile requirements.in ...'
pip-compile --output-file requirements.txt requirements.in

echo 'pip install requirements.txt ...'
pip install -r requirements.txt

pip list --format=columns

echo 'done'
