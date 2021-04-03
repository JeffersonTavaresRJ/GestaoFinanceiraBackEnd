using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.CrossCutting.Security
{
    public class AppSetting
    {
        public string Secret { get; set; }
        public string ValidForMinutes { get; set; }
    }
}
