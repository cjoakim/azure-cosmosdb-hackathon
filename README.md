# azure-cosmosdb-hackathon

Hackathon with CosmosDB

Alpha version 1

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
- Bring your favorite IDE: [Visual Studio Code](https://code.visualstudio.com) recommended  
- Install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) 
- Bring your Azure Subscription that you can add resources to
- Git source control system

### Challenge-Specific

- For Challenge 2, either a local or remote MongoDB.

### Suggested Libraries

#### .Net Core

```
    <PackageReference Include="CsvHelper" Version="12.1.2" />
    <PackageReference Include="Microsoft.Azure.DocumentDB.Core" Version="2.3.0" />
```

#### Java

### Node.js

### Python

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

## Let's Go

[Hackathon Outline](outline.md)
