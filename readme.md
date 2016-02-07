Rosetta Object
==============

Library providing canonical cross-language mapping of certain data types to and from JSON.

Rationale
---------

Rosetta Object is a format for data structures useful in data analysis. The aim is to make it as easy as possible to transfer data sets which were created in one language for use in another language.

For example, within an organization it may be easiest to do data manipulation in Python, while more powerful statistical tools are available in R, but maybe the tooling to interact with databases is in C# or Python, and analysts are more comfortable using Excel. Each of these tools has their place, but transferring data between them is non-trivial. Rosetta Object aims to make it easy to ship data around, in a way that is comfortable in all these languages and tools, and more.

Rosetta Object is based around structures that are useful for real-world data work. This includes atomic types such as strings, integers, booleans and floating-point numbers. These atomic types are combined in more complex data structures including arrays, lists, dictionaries and data frames. The aim is to make Rosetta Object natural to use for real-world data sets.

A key goal for Rosetta Object is to support as many languages and tools as possible. This has two important implications. First, we decided that the data format should be  based on JSON, as high quality JSON libraries are already available in many languages. Second, the data structures are deliberately parsimonious, supporting a lowest-common denominator set of features - it is more important that all tools be able to comfortably use datasets than that any one implementation have all the features of its native data analysis tools supported.

Implentations are advised to follow the Jon Postel's Robustness principle: Be generous in what you accept, and conservative in what you send.

Specifically, implementations must be able to deserialize any valid Rosetta Object, and should be able to round-trip it. Where possible, implementations are encouraged to pick a standard set of data types using standard or popular libraries for the language they support, and to deserialize Rosetta Objects to those types.

Implementations are also expected to be generous in the set of types that they accept for serialization. Implementations are also encouraged to support a wider array of data types, and to support serializing data that uses native data structure features that are not available in Rosetta Objects. It will not be possible to round-trip in this case, but at least it will be possible to transfer data.

Current Solutions
-----------------

The defacto standards for transferring data sets are CSV and JSON.

CSVs have been around since at least the early 1970s. Unfortunately, there were no attempts at standardization until relatively recently. There are high quality CSV libraries for a number of languages, but, because of the simplicity of the format, there are a large number of home-grown implementations also, which may or may not interoperate correctly. It only supports transferring a single table-like data structure. Ad hoc methods to support more complex structures, such as concatenating multiple tables, require the CSV data to be processed whenever loading the data.

JSON was created in the early 2000s to transfer data between web servers and browsers, and was standardized early in its life. It provides support for list and dictionary data structures, which became supported in many popular languages through the 2000s. Its relative simplicity made it easy to implement JSON libraries, and today most languages have a high quality JSON library. However, there is no standardization of how to encode data sets as JSON. This is an advantage for web applications, where front- and back-end developers may come up with whatever schema they like to encode their particular data, but makes it difficult to build tools that are able to import any valid JSON. Typically JSON must be deserialized as low-level primitives such as list and dictionary types, and then processed into higher-level types suitable for data analysis. Rosetta Object attempts to provide a schema that bridges that gap.

Supported languages
-------------------
  * Python
  * C#

Supported datatypes
-------------------
  * bool
  * int
  * float
  * string
  * array
  * dataframe (with non-Multi index)
  * list
  * dictionary (with string keys)

