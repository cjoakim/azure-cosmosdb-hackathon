# azure-cosmosdb-hackathon

Hackathon with CosmosDB

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
- Install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) 
- Bring your Azure Subscription that you can add resources to
- Git source control system

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

### Node.js

"dependencies": {
    "azure": "^2.3.1-preview",
    "documentdb": "^1.14.5",
    "sleep": "^6.0.0",
    "uuid": "^3.3.2"
}

### Python

requirements.in:

```
pydocumentdb 
```

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

[Hackathon Challenges](challenges.md)
