using System;
using System.Collections.Generic;
using System.Text;

//====================================================================================================
//The Free Edition of C++ to C# Converter limits conversion output to 100 lines per file.

//To purchase the Premium Edition, visit our website:
//https://www.tangiblesoftwaresolutions.com/order/order-cplus-to-csharp.html
//====================================================================================================

namespace AES256
{
    public static class AesUtils
    {
        public static string EncryptAES(string password, string plain)
        {
            byte[] key = InitKey(password);
            var txt = StringToByteArray(plain);
            var enc = new List<byte>();
            Aes256.encrypt(key, txt, enc);
            return base64_encode(enc.ToArray());            
        }

        public static string DecryptAES(string password, string encrypted)
        {
            byte[] key = InitKey(password);
            List<byte> dec = new List<byte>();            
            var enc = base64_decode(encrypted);
            var dec_len = Aes256.decrypt(key, enc, dec);
            return ByteArrayToString(dec.ToArray());
        }


        //
        // The Following String is the Specific Key used for Base64 Encoding/Decoding
        //
        internal const string base64_chars = "ILJTUVWKMFGNEHPQROSXYZABCD" + "hpqrosxyzabcdiljtuvwkmfgne" + "5670123489+/";

        //---------------------------------------------------------------------------

        internal static bool is_base64(byte c)
        {
            return (char.IsLetterOrDigit((char)c) || (c == '+') || (c == '/'));
        }
        //---------------------------------------------------------------------------

        //C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'bytes_to_encode', so pointers on this parameter are left unchanged:
        public static string base64_encode(byte[] bytes_to_encode)
        {
            StringBuilder ret = new StringBuilder();
            int i = 0;
            int j = 0;
            int in_len = bytes_to_encode.Length;
            int byteIndex = 0;
            byte[] char_array_3 = new byte[3];
            byte[] char_array_4 = new byte[4];

            while ((in_len--) != 0)
            {
                char_array_3[i++] = bytes_to_encode[byteIndex++];
                if (i == 3)
                {
                    char_array_4[0] = (byte)((char_array_3[0] & 0xfc) >> 2);
                    char_array_4[1] = (byte)(((char_array_3[0] & 0x03) << 4) + ((char_array_3[1] & 0xf0) >> 4));
                    char_array_4[2] = (byte)(((char_array_3[1] & 0x0f) << 2) + ((char_array_3[2] & 0xc0) >> 6));
                    char_array_4[3] = (byte)(char_array_3[2] & 0x3f);

                    for (i = 0; (i < 4); i++)
                    {
                        ret.Append(base64_chars[char_array_4[i]]);
                    }
                    i = 0;
                }
            }

            if (i != 0)
            {
                for (j = i; j < 3; j++)
                {
                    char_array_3[j] = (byte)'\0';
                }

                char_array_4[0] = (byte)((char_array_3[0] & 0xfc) >> 2);
                char_array_4[1] = (byte)(((char_array_3[0] & 0x03) << 4) + ((char_array_3[1] & 0xf0) >> 4));
                char_array_4[2] = (byte)(((char_array_3[1] & 0x0f) << 2) + ((char_array_3[2] & 0xc0) >> 6));
                char_array_4[3] = (byte)(char_array_3[2] & 0x3f);

                for (j = 0; (j < i + 1); j++)
                {
                    ret.Append(base64_chars[char_array_4[j]]);
                }

                while ((i++ < 3))
                {
                    ret.Append('=');
                }

            }

            return ret.ToString();
        }
        //---------------------------------------------------------------------------
        public static byte[] base64_decode(string s)
        {
            byte[] encoded_string = Encoding.ASCII.GetBytes(s);
            int in_len = encoded_string.Length;
            int i = 0;
            int j = 0;
            int in_ = 0;
            byte[] char_array_4 = new byte[4];
            byte[] char_array_3 = new byte[3];
            List<byte> ret = new List<byte>();

            while ((in_len--) != 0 && (encoded_string[in_] != '=') && is_base64(encoded_string[in_]))
            {
                char_array_4[i++] = encoded_string[in_];
                in_++;
                if (i == 4)
                {
                    for (i = 0; i < 4; i++)
                    {
                        char_array_4[i] = (byte)base64_chars.IndexOf(Convert.ToChar(char_array_4[i]));
                    }

                    char_array_3[0] = (byte)((char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4));
                    char_array_3[1] = (byte)(((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2));
                    char_array_3[2] = (byte)(((char_array_4[2] & 0x3) << 6) + char_array_4[3]);

                    for (i = 0; (i < 3); i++)
                    {
                        ret.Add(char_array_3[i]);
                    }
                    i = 0;
                }
            }

            if (i != 0)
            {
                for (j = i; j < 4; j++)
                {
                    char_array_4[j] = 0;
                }

                for (j = 0; j < 4; j++)
                {
                    char_array_4[j] = (byte)base64_chars.IndexOf(Convert.ToChar(char_array_4[j]));
                }

                char_array_3[0] = (byte)((char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4));
                char_array_3[1] = (byte)(((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2));
                char_array_3[2] = (byte)(((char_array_4[2] & 0x3) << 6) + char_array_4[3]);

                for (j = 0; (j < i - 1); j++)
                {
                    ret.Add(char_array_3[j]);
                }
            }

            return ret.ToArray();
        }
        private static byte[] InitKey(string s)
        {
            var bytes = new byte[Constants.KEY_LEN];
            for (var i = 0; i < Constants.KEY_LEN; i++)
            {
                if (i < s.Length)
                    bytes[i] = (byte)s[i];

                else
                    bytes[i] = 0;
            }
            return bytes;
        }

        public static byte[] StringToByteArray(string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

    }
}