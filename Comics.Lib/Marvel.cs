using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Comics.Lib
{
    public static class Marvel
    {
        private const string PublicKey = "930dd82b4ff47457583c5cff87577de3";
        private const string PrivateKey = "cb661795d7a8214d2776c95a0f4d623a2368b98b";
        private const string requestURL = "https://gateway.marvel.com:443/v1/public/comics";
        public static string CalculateMD5Hash(string input)
        {
            //// step 1, calculate MD5 hash from input

            //MD5 md5 = System.Security.Cryptography.MD5.Create();
            //byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            //byte[] hash = md5.ComputeHash(inputBytes);

            //// step 2, convert byte array to hex string

            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < hash.Length; i++)
            //{
            //    sb.Append(hash[i].ToString("X2"));
            //}
            //return sb.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            var gerador = MD5.Create();
            byte[] bytesHash = gerador.ComputeHash(bytes);
            return BitConverter.ToString(bytesHash).ToLower().Replace("-", String.Empty);
        }

        public static string GetMarvelHash(string searchName, string searchIssue)
        {
            var timeStamp = DateTime.Now.ToString("yyyyMMddTHH:mm");
            var retVal = CalculateMD5Hash(timeStamp + PrivateKey + PublicKey);
            retVal = requestURL + "?ts=" + timeStamp + "&apikey=" + PublicKey + "&hash=" + retVal;
            if (!string.IsNullOrEmpty(searchName))
            {
                retVal += "&title=" + searchName;
            }
            if (!string.IsNullOrEmpty(searchIssue))
            {
                retVal += "&issueNumber=" + searchIssue;
            }
            retVal += "&orderBy=title";
            Debug.WriteLine(retVal);
            return retVal;
        }
    }
}
