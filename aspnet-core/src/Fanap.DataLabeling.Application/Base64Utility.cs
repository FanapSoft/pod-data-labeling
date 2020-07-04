using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling
{


    public class Base64Uilities
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
