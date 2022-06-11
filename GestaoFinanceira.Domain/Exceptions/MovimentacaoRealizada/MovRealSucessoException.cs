using System;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoRealizada
{
    public class MovRealSucessoException : Exception
    {
        private string id;

        public MovRealSucessoException(int Id)
        {
            this.id = Id.ToString();
        }

        public override string Message => this.id;
    }
}
