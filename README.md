# AES256

This is C# port of https://github.com/Urban82/Aes256

The Urban82 is writtin in c++ and can be used in many legacy apps which does not have modern AES256 library. The program arise that the encrypted values can't be decrypted by programs writen in other languages. So I've ported the same code to C# to solve this problem.

Some helper class are also added in case you want to use Base64 encoding for the encrypted values.

# Node
Use the default RijnDael class instead if you don't need cross language support.


# C# Code Sample

* Encrytion
var encrypted = AesUtils.EncryptAES("your password", "your plain text");

* Decryption
var plain = AesUtils.DecryptAES("your password", "your encryped text");

