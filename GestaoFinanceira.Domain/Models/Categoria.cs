namespace GestaoFinanceira.Domain.Models
{
    public class Categoria 
    {
        public int Id { get; set; }
        public string Descricao { get; set; }    
        public bool Status { get; set; }
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        public Categoria()
        {
            
        }

        public Categoria(int id, string descricao, bool status, int idUsuario)
        {
            Id = id;
            Descricao = descricao;
            Status = status;
            IdUsuario = idUsuario;
        }
    }
}
