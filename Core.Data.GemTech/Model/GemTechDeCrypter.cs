using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Core.Data.GemTech.Model.Poco;
using Newtonsoft.Json;

namespace Core.Data.GemTech.Model
{
    public static class GemTechDecrypter
    {
        public static GemTechData DecrypGemTechDataString(this string sourceStr,string keyString)
        {
            byte[] dataByte = DecodeUrlBase64(sourceStr);
            var phpDes = new PhpSerializer();
            string decodedString = Encoding.UTF8.GetString(dataByte);
            var data = JsonConvert.DeserializeObject<GemTechEncryptData>(decodedString);
            var result = Decrypt(data.Value, keyString, data.Iv);
            return JsonConvert.DeserializeObject<GemTechData>(new PhpSerializer().Deserialize(result).ToString());
        }
        private static string Decrypt(string cipherData, string keyString, string ivString)
        {
            byte[] key = Encoding.UTF8.GetBytes(keyString);
            byte[] iv = Convert.FromBase64String(ivString);
            using (var rijndaelManaged = new RijndaelManaged { Key = key, IV = iv, Mode = CipherMode.CBC })
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(cipherData)))
            using (var cryptoStream =new CryptoStream(memoryStream,rijndaelManaged.CreateDecryptor(key, iv),CryptoStreamMode.Read))
            {
                return new StreamReader(cryptoStream).ReadToEnd();
            }
        }

        public static byte[] DecodeUrlBase64(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/').PadRight(4 * ((s.Length + 3) / 4), '=');
            return Convert.FromBase64String(s);
        }
    }
}
