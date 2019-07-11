## Design with the Document/SQL Database

### Comparison: the evolution of the Telephone

![rotary-phone](img/rotary-phone.jpg)

---

### Steve Jobs introducing the iPhone in 2007!?

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
- Don't think that you have to put differently-shaped documents in different collections
- **Don't think relationally**; this is a NoSQL Document-Oriented database

## Best Practices

- **Think first about how you will query your data**
  - Identify the use-cases for your application
  - Identify the queries your application will execute

- **Use a generic name for the partition key in each collection**
  - **pk**, partition_key, partitionKey - these are recommended names
  - The **name** of the partition key is fixed; it can't be changed after creation
  - pk values should have a high cardinality - thousands of values or more
  - pk values should also be well-distributed
    - Don't, for example, use country_code as a pk!
  - The **values** that you use to populate the pk can and often will evolve over time
  - **Most of you queries should use the partition key**

- **Put differently-shaped documents in the same collection**
  - It's **schemaless!**  This is allowed
  - Use a **doctype** attribute to identify the type of the Document
  - There is no need to conform your documents to a given shape
    - Other than **pk** and perhaps **doctype**

- **Think of collections as a Unit-for-Scaling rather than a type of data**
  - The Request Units (RU) can be configured separately for each collection

- **Do "partition key joins" rather than "cross-collection joins"**
  - Read the related documents within one pk value in one query

- **If your Documents contain collections, be aware of their Bounds**
  - Be aware of the **2MB size limit for a Document**
  - GameOfBaseball Document - typically 9 innings, Ok
  - RoundOfGolf Document - typically 18 holes, Ok
  - WeatherHistoryForCity Document - hourly entries = unbounded!!
  - Generally prefer many smaller documents vs fewer larger documents

- **Be aware of the Request Charge for each of your queries**
  - The SDKs will make this response value available

- **Use Time-to-Live (TTL) to automatically delete old documents**
  - Saves you the RU costs of the delete operations you'd otherwise have to do

- **Consider Aggregating Summary-Data in Advance**
  - Store Aggregated or Summary documents from your many Detail Documents
  - This saves the cost of numerous aggregation queries
  - Another term for this is a **Materialized View**
  - These can be **Normalized Documents for PowerBI**

- **Remember that Documents are JavaScript JSON Documents; beware of Truncation**
```
Node.js - Number.MAX_SAFE_INTEGER =    9007199254740991
Java    - Long.MAX_VALUE          = 9223372036854775807
```

- **Use the Change Feed functionally to trigger downstream operations**
  - Typically implemented with an **Azure Function** observing the collection
  - DotNet and Java SDKs for this also

---

## Exercise 1 - Inventory Lookup by SKU and Location

Your Documents look like this; actual product at https://www.homedepot.com/p/DEWALT-20-oz-Hammer-DWHT51054/205594063  

```
{
  "pk": "???",               <-- what value to use?
  "sku": "DWHT51054",
  "location": "store-485",   <-- hundreds of stores, warehouses
  "current_inventory": 33,
  "description": "hammer, 20oz, steel, rubber handle",
  "vendor": "DeWalt",
  "features": {
      ... cool info about the product...
  }
  "state": "active"
}
```

How would you design this in CosmosDB?
- Assume 1 million queries by **sku** per month
- Assume 1 million queries by **location** per month

---

## Exercise 2 - Online Retail

The conceptual objects are: Order, LineItem, Delivery, Customer

How would you design this in CosmosDB?


```






















Exercise 1:

- Consider Duplicating the data into Two Documents
- The cost of the queries can be > cost of the storage
 
{
  "pk": "DWHT51054",
  "sku": "DWHT51054",
  "location": "store-485",
  "current_inventory": 33,
  "description": "hammer, 20oz, steel, rubber handle",
  "vendor": "DeWalt",
  "features": {
      ... cool info about the product...
  }
  "state": "active"
}

{
  "pk": "store-485",
  "sku": "DWHT51054",
  "location": "store-485",
  "current_inventory": 33,
  "description": "hammer, 20oz, steel, rubber handle",
  "vendor": "DeWalt",
  "features": {
      ... cool info about the product...
  }
  "state": "active"
}















Exercise 2:

Two Collections:

- Customers Collection
  - Relatively static and smaller number of Documents and RUs
  - No pk relationship to the Orders

- Orders Collection
  - for the Order, LineItems, Deliveries; use a doctype attribute
  - Relatively dynamic, with more documents and RUs
  - Could use TTL to purge old orders

{
  "pk": "XK1123",
  "doctype": "order",
  "orderNumber": "XK1123",
  ... order attributes ...
}

{
  "pk": "XK1123",
  "doctype": "lineitem",
  "orderNumber": "XK1123",
  "lineItem": 1,
  ... lineitem attributes ...
}

{
  "pk": "XK1123",
  "doctype": "lineitem",
  "orderNumber": "XK1123",
  "lineItem": 2,
  ... lineitem attributes ...
}

{
  "pk": "XK1123",
  "doctype": "delivery",
  "orderNumber": "XK1123",
  "lineItem": 1,
  "deliveryNumber": 1,
  ... delivery attributes ...
}

select * from c where c.pk = "XK1123"
select * from c where c.pk = "XK1123" and c.doctype = "order"
select * from c where c.pk = "XK1123" and c.doctype in ("order", "lineitem")

Alternatively:

{
  "pk": "XK1123",
  "doctype": "order",
  "orderNumber": "XK1123",
  ... order attributes ...
  "lineItems: [
    ... an array or collection of line items ...
  ],
  "deliveries: [
    ... an array or collection of deliveries ...
  ]
}

```



