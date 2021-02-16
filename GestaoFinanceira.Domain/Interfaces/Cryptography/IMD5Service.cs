using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Cryptography
{
    public interface IMD5Service
    {
        string Encrypt(string value);
    }
}
