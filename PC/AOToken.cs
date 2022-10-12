using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APISERVER
{
    internal class AOToken
    {
        private static Random random = new Random();
        public string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 32)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
