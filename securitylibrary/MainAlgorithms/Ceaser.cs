using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {

        public string Encrypt(string plainText, int key)
        {
            //empty string to add in it .
            string cipher_text = "";
            foreach(char c in plainText)
            {
                if (char.IsLetter(c))
                {
                    char lower = char.ToLower(c);
                    char offset = (char)((((lower + key) - 'a') % 26) + 'a');
                    cipher_text += offset;
                    cipher_text = cipher_text.ToUpper();
                }
                else
                {
                    cipher_text += c;
                }

                
            }
            
            return cipher_text;

        }

        public string Decrypt(string cipherText, int key)
        {

            return Encrypt(cipherText, 26 - key);
        }

        public int Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            // Convert both texts to uppercase 
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();

            
            for (int key = 0; key < 26; key++)
            {
                
                string decryptedText = Decrypt(cipherText, key);

                if (decryptedText == plainText)
                {
                    return key;
                }

            }

            // If no key matches
            return -1;
        }
    }
}