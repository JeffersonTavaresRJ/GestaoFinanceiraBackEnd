using GestaoFinanceira.Domain.Models.Enuns;

namespace GestaoFinanceira.Domain.Models
{
    public class Categoria 
    {
        public int Id { get; set; }
        public string Descricao { get; set; }    
        public TipoCategoria Tipo { get; set; }
        public bool Status { get; set; }
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        public Categoria()
        {
            
        }

        public Categoria(int id, string descricao, TipoCategoria tipo, bool status, int idUsuario)
        {
            Id = id;
            Descricao = descricao;
            Tipo = tipo;
            Status = status;
            IdUsuario = idUsuario;
        }
    }
}
