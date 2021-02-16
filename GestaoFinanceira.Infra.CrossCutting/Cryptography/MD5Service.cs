using GestaoFinanceira.Domain.Interfaces.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;

namespace GestaoFinanceira.Infra.CrossCutting.Cryptography
{
    public class MD5Service : IMD5Service
    {
        public string Encrypt(string value)
        {
            var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(value));

            var result = new StringBuilder();

            foreach (var item in hash)
            {
                //x2 -> Hexadecimal
                result.Append(item.ToString("x2"));
            }

            return result.ToString();
        }
    }
}
