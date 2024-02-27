# SourceGenDemo

Demo application which uses .net source generator to generate code.

## SQLFileToConstantsGenerator.cs

Takes all .sql files in a project which have build action set to "C# analyzer additional file" and generates a c# file with constants. One constant for each .sql file with the file name as the constant name.

## FileNameGenerator.cs

Finds all c# classes which contain a partial method with the name `GetFileName()` and generates the implementation of the method which is to return the file name and path of the actual file at build time.
