using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class TCacheKeysUtils
    {
        public static string KeyOperation(string id)
        {
            return $"KeyOperation_{id}";
        }

        public static string KeyDateClosed(string id)
        {
            return $"KeyDateClosed_{id}";
        }
    }
