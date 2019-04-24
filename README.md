# azure-cosmosdb-hackathon

A CosmosDB Hackathon

---

![azure-cosmos-db](img/azure-cosmos-db.png)

---

## NoSQL and CosmosDB

- [What is NoSQL?](what-is-nosql.md)
- [What is CosmosDB?](what-is-cosmosdb.md)

---

## System Requirements

### General

- Bring your favorite workstation: Windows, macOS, or Linux
- Bring your favorite langage(s): .Net Core, Java, Node.js, Python3, etc
- Bring your favorite IDE: [Visual Studio Code](https://code.visualstudio.com) is suggested 
- Bring your Azure Subscription that you can add resources to
- Install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) 
- Use the [Git](https://git-scm.com/downloads) source control system 

### Challenge-Specific

- For Challenge 2, MongoDB installed locally.  This is required for Challenge 2 only.
- For Challenge 9, Cassandra installed locally.  This is optional.

### Suggested Libraries

#### .Net Core

```
<PackageReference Include="CsvHelper" Version="12.1.2" />
<PackageReference Include="Microsoft.Azure.DocumentDB.Core" Version="2.3.0" />
```

#### Java

Apache Maven 3.6.x and Java version 1.8.x are used in this repository.

See the **Maven** dependencies in file **solutions/java/pom.xml**

### Node.js

Node v8.11.x is used in this repository.

See the **NPM** dependencies in file **solutions/node/package.json**

### Python

Python 3.7.x is used in this repository.

See the **PyPI** dependencies in file **solutions/python/requirements.in**

File **venv.sh** is used to create a Python **virtual environment** per the requirements.in file.

---

## Getting Started

Use the [git](https://git-scm.com) program to clone this repository to a 
**hackathon base directory** on your workstation:

```
git clone https://github.com/cjoakim/azure-cosmosdb-hackathon.git
```

Then, set environment variable **COSMOSDB_HACKATHON_BASE_DIR** to the root
directory of this GitHub repository you cloned.  The reason for doing this 
is that the provided solutions use this environment variable to locate data
files.

---

## Let's Go Code!

[Hackathon Challenges](challenges/challenges_list.md)
