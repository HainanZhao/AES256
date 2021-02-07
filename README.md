# AES256

This is C# port of https://github.com/Urban82/Aes256

The Urban82 Aes256 is writtin in c++ and can be used in many legacy apps which does not have modern AES256 library. The problem arise that the encrypted values can't be decrypted by programs writen in other languages, since it's different with the language default implementation. So I've ported the same code to C# to solve this problem.

Some helper class are also added in case you want to use Base64 encoding for the encrypted values.

## Note
Use the default RijnDael class instead if you don't need cross language support.


## C# Code Sample
To use the code in C#, you can reference the C# project in your solution. 

`using AES256;`

* Encrytion
```
var encrypted = AesUtils.EncryptAES("your password", "your plain text");
```

* Decryption
```
var plain = AesUtils.DecryptAES("your password", "your encryped text");
```
