# NullChecker.Core

GreenShell.NullChecker is a lightweight, re-usable way to validate that a created class has all of it's properties set. Useful when unit testing factory methods, or any methods that involve the creation of complex objects no matter how simple or convoluted.

## The Problem To Solve
When unit testing, an often overlooked test case is to verify that every property on an object is set. This leads to time wasted investigating what has gone wrong across your entire code base when the output from your UI has nulls inside a property that was supposed to be populated.  NullChecker solves this by using reflection to read every property on a class and ensure that it is not null. This means that in the future, a unit test will fail if a new property is added but is not mapped correctly.

## Usage
Using NullChecker is easy and uses a fluent interface to ensure a clean codebase throughout. A very basic check using NullChecker would be `NullChecker.For(myObject).Validate()`

### Options

**For**
Creates a NullChecker class, the entry point into nullcheckers functionality
```C#
NullChecker.For(MyObject)
```

**Ignore** 
Removes the property from the list of properties to validate against            
```C#
 NullChecker.For(myObject).Ignore(obj => obj.MyProperty).Validate()
 ```
**AllowValueTypeValidation**
Validates value objects, ensures the validate method will return false if any value objects are set to their default value (if they are not explicitly ignored)
```C# 
NullChecker.For(myObject).AllowValueTypeValidation().Validate()
```
**Validate**
Validates against every property on the class that is not ignored. Returns true if nothing is null, false if anything is invalid.
```C# 
NullChecker.For(myObject).Validate()
```
