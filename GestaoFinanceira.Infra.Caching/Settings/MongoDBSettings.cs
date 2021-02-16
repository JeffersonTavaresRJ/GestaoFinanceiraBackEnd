using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Caching.Settings
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DataBaseName { get; set; }
        public bool IsSSL { get; set; }
    }
}
