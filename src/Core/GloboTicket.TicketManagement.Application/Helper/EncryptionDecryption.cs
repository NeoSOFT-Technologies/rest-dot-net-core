using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GloboTicket.TicketManagement.Infrastructure.EncryptDecrypt
{
    public static class EncryptionDecryption
    {
        //This function for Encryption which accepts the plain text and Key and return Encrypted string
        public static string EncryptString(string clearText)
        {
            string EncryptionKey = Environment.GetEnvironmentVariable("EncryptionDeckryptionKey")?.ToString().Length > 0  ? Environment.GetEnvironmentVariable("EncryptionDeckryptionKey") : "";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                var salt = Encoding.UTF8.GetBytes("0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76");
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }


        //This function for Decryption which accepts Encrypted string and Key and return plain text string
        public static string DecryptString(string cipherText)
        {
            string EncryptionKey = Environment.GetEnvironmentVariable("EncryptionDeckryptionKey")?.ToString().Length > 0 ? Environment.GetEnvironmentVariable("EncryptionDeckryptionKey") : "";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                var salt = Encoding.UTF8.GetBytes("0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76");
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
