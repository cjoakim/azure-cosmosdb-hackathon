## Design with the Document/SQL Database

### Comparison; the evolution of the Telephone

![rotary-phone](img/rotary-phone.jpg)

---

![jobs1](img/steve-jobs-iphone-1.jpg)

---

![jobs2](img/steve-jobs-iphone-2.jpg)

---

![jobs2](img/info-database-schema.jpg)

---

## Don't

- Don't create collection designs like the above schema
  - there is no referential integrity
  - No need for [Third normal form (3NF) ](https://en.wikipedia.org/wiki/Third_normal_form)
  - storage is inexpensive now
- Don't think you have to put differently-shaped documents in different collections
- **Don't think relationally**; this is a NoSQL Document-Oriented database

## Best Practices

- **Think first about how you will query your data**
  - Identify the use-cases for your application
  - Identify the queries your application will execute

- Use a generic name for the **partition key** in each collection
  - **pk**, partition_key, partitionKey - these are recommended names
  - The name of the partition key is fixed; can't be changed after creation
  - pk values should have a high cardinality - thousands of values or more
  - pk values should also be well-distributed
    - Don't, for example, use country_code as a pk!
  - The values that you use to populate the pk can and will evolve over time
  - **Most of you queries should use the partition key**

- Put differently-shaped documents in the same collection
  - Use a **doctype** attribute to identify the type of the Document
  - It's **schemaless!** 
  - There is no need to conform your documents to a given shape
    - Other than **pk** and perhaps **doctype**

- Think of collections as **unit-for-scaling** rather than a type of data
  - The Request Units (RU) can be configured separately for each collection

- Do **"partition key joins"** rather than **"cross-collection joins"**
  - Read the related documents within one pk value in one query

- If your Documents contain collections, be aware of their **bounds**
  - Be aware of the **2MB size limit for a Document**
  - GameOfBaseball Document - typically 9 innings, Ok
  - RoundOfGolf Document - typically 18 holes, Ok
  - WeatherHistoryForCity Document - hourly entries = unbounded!!
  - Generally prefer many smaller documents vs fewer larger documents

- Be aware of the **Request Charge** for each of your queries
  - The SDKs will make this response value available

## Example 1 - Inventory Lookup by SKU and Location

Your documents look like this.  
```

```

How would you design this in CosmosDB?
- Assume 1 million queries by **sku** per month
- Assume 1 million queries by **location** per month

## Example 2 - Online Retail

The conceptual objects are: Order, LineItem, Delivery, Customer, Location

How would you design this in CosmosDB?






