using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace RefundWindowsService.Security
{
    public class CryptorEngine
    {
        private static readonly UnicodeEncoding Encoder = new UnicodeEncoding();

        /// <summary>
        ///     Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            var settingsReader = new AppSettingsReader();
            // Get the key from config file
            var key = (string) settingsReader.GetValue("SecurityKey", typeof(string));
            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            } else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tdes.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        ///     DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            var toEncryptArray = Convert.FromBase64String(cipherString);

            var settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            var key = (string) settingsReader.GetValue("SecurityKey", typeof(string));

            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            } else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return Encoding.UTF8.GetString(resultArray);
        }

        public static string PasswordHash(string password, string salt = "123456")
        {
            var byteText = new UTF8Encoding().GetBytes(password);
            var byteSalt = new UTF8Encoding().GetBytes(salt);

            HashAlgorithm algorithm = new SHA256Managed();

            var plainTextWithSaltBytes =
                new byte[byteText.Length + byteSalt.Length];

            for (var i = 0; i < byteText.Length; i++) plainTextWithSaltBytes[i] = byteText[i];
            for (var i = 0; i < byteSalt.Length; i++) plainTextWithSaltBytes[byteText.Length + i] = byteSalt[i];

            return Convert.ToBase64String(algorithm.ComputeHash(plainTextWithSaltBytes));
        }

        public static bool VerifyData(string originalMessage, string signedMessage, string publicKey)
        {
            var success = false;
            using (var rsa = new RSACryptoServiceProvider())
            {
                var encoder = new UTF8Encoding();
                var bytesToVerify = encoder.GetBytes(originalMessage);
                var signedBytes = Convert.FromBase64String(signedMessage);
                try
                {
                    rsa.FromXmlString(publicKey);
                    success = rsa.VerifyData(bytesToVerify, CryptoConfig.MapNameToOID("SHA512"), signedBytes);
                }
                catch (CryptographicException)
                {
                    return false;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return success;
        }

        public static string Encryption(string strText, string publicKey)
        {
            var dataBytes = Encoding.UTF8.GetBytes(strText);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // client encrypting data with public key issued by server                    
                    rsa.FromXmlString(publicKey);

                    var encryptedData = rsa.Encrypt(dataBytes, true);

                    var base64Encrypted = Convert.ToBase64String(encryptedData);

                    return base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public static string Decryption(string strText, string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    var base64Encrypted = strText;

                    // server decrypting data with private key                    
                    rsa.FromXmlString(privateKey);
                    var resultBytes = Convert.FromBase64String(base64Encrypted);
                    var decryptedBytes = rsa.Decrypt(resultBytes, true);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    return decryptedData;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}