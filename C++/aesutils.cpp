// DCommon.cpp : Defines the exported functions for the DLL application.
//

#include <string>
#include "stdafx.h"
#include "aes256.hpp"
#include "DCommon.h"

#define KEY_LEN    32

using namespace std;


//
// The Following String is the Specific Key used for Base64 Encoding/Decoding
//
static const std::string base64_chars =
"ILJTUVWKMFGNEHPQROSXYZABCD"
"hpqrosxyzabcdiljtuvwkmfgne"
"5670123489+/";

//---------------------------------------------------------------------------

static inline bool is_base64(unsigned char c) {
	return (isalnum(c) || (c == '+') || (c == '/'));
}
//---------------------------------------------------------------------------

string base64_encode(char const* bytes_to_encode, unsigned int in_len) {
	string ret;
	int i = 0;
	int j = 0;
	unsigned char char_array_3[3];
	unsigned char char_array_4[4];

	while (in_len--) {
		char_array_3[i++] = *(bytes_to_encode++);
		if (i == 3) {
			char_array_4[0] = (char_array_3[0] & 0xfc) >> 2;
			char_array_4[1] = ((char_array_3[0] & 0x03) << 4) + ((char_array_3[1] & 0xf0) >> 4);
			char_array_4[2] = ((char_array_3[1] & 0x0f) << 2) + ((char_array_3[2] & 0xc0) >> 6);
			char_array_4[3] = char_array_3[2] & 0x3f;

			for (i = 0; (i < 4); i++)
				ret += base64_chars[char_array_4[i]];
			i = 0;
		}
	}

	if (i)
	{
		for (j = i; j < 3; j++)
			char_array_3[j] = '\0';

		char_array_4[0] = (char_array_3[0] & 0xfc) >> 2;
		char_array_4[1] = ((char_array_3[0] & 0x03) << 4) + ((char_array_3[1] & 0xf0) >> 4);
		char_array_4[2] = ((char_array_3[1] & 0x0f) << 2) + ((char_array_3[2] & 0xc0) >> 6);
		char_array_4[3] = char_array_3[2] & 0x3f;

		for (j = 0; (j < i + 1); j++)
			ret += base64_chars[char_array_4[j]];

		while ((i++ < 3))
			ret += '=';

	}

	return ret;

}
//---------------------------------------------------------------------------
ByteArray base64_decode(std::string const& encoded_string) {
	int in_len = encoded_string.size();
	int i = 0;
	int j = 0;
	int in_ = 0;
	BYTE char_array_4[4], char_array_3[3];
	ByteArray ret;

	while (in_len-- && (encoded_string[in_] != '=') && is_base64(encoded_string[in_])) {
		char_array_4[i++] = encoded_string[in_]; in_++;
		if (i == 4) {
			for (i = 0; i < 4; i++)
				char_array_4[i] = base64_chars.find(char_array_4[i]);

			char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
			char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
			char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

			for (i = 0; (i < 3); i++)
				ret.push_back(char_array_3[i]);
			i = 0;
		}
	}

	if (i) {
		for (j = i; j < 4; j++)
			char_array_4[j] = 0;

		for (j = 0; j < 4; j++)
			char_array_4[j] = base64_chars.find(char_array_4[j]);

		char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
		char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
		char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

		for (j = 0; (j < i - 1); j++) ret.push_back(char_array_3[j]);
	}

	return ret;
}


void InitKey(const char* s, ByteArray& key)
{
	for (unsigned char i = 0; i < KEY_LEN; i++)
	{
		if (i < strlen(s))
			key.push_back(s[i]);
		else
			key.push_back(0);
	}
}

void StringToByteArray(string s, ByteArray& txt)
{
	for (unsigned char i = 0; i < s.length(); ++i)
		txt.push_back(s[i]);
}

string ByteArrayToString(ByteArray& txt)
{
	char *s = new char[txt.size()];
	for (int i = 0; i < txt.size(); ++i)
		s[i] = txt[i];

	string ans(s, txt.size());
	return ans;
}

//---------------------------------------------------------------------------

void InitKey(char* s, ByteArray& key)
{
	for (unsigned char i = 0; i < KEY_LEN; i++)
	{
		if (i < strlen(s))
			key.push_back(s[i + 1]);
		else
			key.push_back(0);
	}
}

int Add(int num1, int num2)
{
	return num1 + num2;
}

string EncryptAES(const char* password, const char* plain)
{
	ByteArray key;
	InitKey(password, key);

	ByteArray txt;
	StringToByteArray(plain, txt);

	ByteArray enc;
	ByteArray::size_type enc_len = Aes256::encrypt(key, txt, enc);

	string encrypted = ByteArrayToString(enc);
	return base64_encode(encrypted.c_str(), encrypted.size());	
}

string DecryptAES(const char* password, const char* encrypted)
{
	ByteArray key;
	InitKey(password, key);

	ByteArray enc, dec;
	enc = base64_decode(encrypted);
	ByteArray::size_type dec_len = Aes256::decrypt(key, enc, dec);

	return ByteArrayToString(dec);	
}


