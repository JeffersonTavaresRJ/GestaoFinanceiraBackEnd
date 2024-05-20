using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFinanceira.Domain.Models.Enuns
{
    public class GenericEnum
    {
        public GenericEnum(string id, string descricao)
        {
            this.id = id;
            this.descricao = descricao;
        }

        public string id { get; set; }
        public string descricao { get; set; }
    }
}
