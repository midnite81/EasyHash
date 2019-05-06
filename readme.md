# EasyHash
_Making Hashing Simpler in C Sharp_   
[![Build](https://travis-ci.org/midnite81/EasyHash.svg?branch=master)](https://travis-ci.org/midnite81/EasyHash) 

## Introduction

This is a simple project in which it make hashing simpler and cleaner
in your own code bases. This is not intended to be a complex project
however it may grow in the next period. 

## Usage

EasyHash comes with an Interface `(Midnite81.EasyHash.Contracts.IHasher)` 
should you wish to bind it with the concrete class 
`(Midnite81.EasyHash.Hasher)`.

### Creating a Hash

To make the hashing process more secure the Salt, unless specified, 
is randomly generated to make the process more secure. 

```c#
public void CreateAHash()
{
    var hasher = new Hasher();

    Hash generatedHash = hasher.MakeHash("password");
    
    // you will need to save both the hash string and salt string to the database, 
    // so that when you come to check them you're able to pass both to the verify function
    ExampleSaveToDatabaseFunction(
        generatedHash.HashString, 
        generatedHash.Salt.String);
}
```

### Generating a salt

```c#
public void GenerateSalt()
{
    var hasher = new Hasher();

     Salt generatedSalt = hasher.GenerateSalt(24);
    
    // returns Salt
    // generatedSalt.String is the base64 string you can store in the database should you wish
    // generatedSalt.Bytes is the array of bytes which form the salt
}
```


### Verifying a hash

```c#
public void CheckHashesMatch()
{
    var hasher = new Hasher();

    byte[] salt = hasher.ConvertStringToBytes(GetUsersSaltFromDatabase());
    byte[] hash = hasher.ConvertStringToBytes(GetUsersHashFromDatabase());

    bool compare = hasher.VerifyHash("password_I_want_to_check", salt, hash);

    if (compare)
    {
        // they matched
    }
    else
    {
        // they didn't match
    }
}
```