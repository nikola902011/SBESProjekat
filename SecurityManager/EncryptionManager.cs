using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public static class EncryptionManager
    {
        public static string EncryptMessage(string message)
        {

            byte[] encrypted = null;

            if (string.IsNullOrEmpty(message))
            {
                throw new FaultException("No message to encrypt");
            }

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.ECB;
                aes.Key = SecretKey.GenerateKey();

                ICryptoTransform encryptor = aes.CreateEncryptor();


                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(message);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }

            }
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptMessage(string message)
        {

            if (string.IsNullOrEmpty(message))
            {
                throw new FaultException("No message to decrypt");
            }

            byte[] encrypted = Convert.FromBase64String(message);
            string plaintext = "";
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.ECB;
                aes.Key = Convert.FromBase64String(SecretKey.LoadKey());
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream msDecrypt = new MemoryStream(encrypted))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
