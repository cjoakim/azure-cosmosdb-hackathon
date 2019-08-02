
# Recreate the virtual environment and reinstall libs.
# Requires Python 3; version 3.7 or higher recommended.
# Chris Joakim, Microsoft, 2019/08/02

# see https://www.bing.com/search?q=python+3+virtualenv+windows+10&form=EDNTHT&mkt=en-us&httpsmsn=1&plvar=0&refig=98ffd9a2ce6f4685e53044c7312ec008&sp=-1&pq=python+3+virtualenv+windows+10&sc=0-30&qs=n&sk=&cvid=98ffd9a2ce6f4685e53044c7312ec008

echo 'deleting previous venv...'
del C:\Users\chjoakim\venv_hackathon

echo 'creating new venv ...'
python -m venv C:\Users\chjoakim\venv_hackathon

echo 'activating venv...'
C:\Users\chjoakim\venv_hackathon\Scripts\activate.bat

echo 'displaying python and pip versions...'
python --version
pip --version
# Python 3.7.0
# pip 19.2.1 from c:\users\chjoakim\appdata\local\programs\python\python37\lib\site-packages\pip

# echo 'installing/upgrading pip...'
# python -m pip install --upgrade pip
# pip --version

# echo 'installing/upgrading pip-tools...'
# pip install --upgrade pip-tools

# 
echo 'pip-compile requirements.in ...'
pip-compile --output-file requirements.txt requirements.in

echo 'pip install requirements.txt ...'
pip install -r requirements.txt

pip list --format=columns

echo 'done'

