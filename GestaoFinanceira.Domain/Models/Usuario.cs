using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string EMail { get; set; }
        public string Senha { get; set; }

        public virtual List<Categoria> Categorias { get; set; }

        public Usuario()
        {

        }

        public Usuario(int id, string nome, string eMail, string senha)
        {
            Id = id;
            Nome = nome;
            EMail = eMail;
            Senha = senha;
        }
    }
}
