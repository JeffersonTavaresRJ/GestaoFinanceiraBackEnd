using System;

namespace GestaoFinanceira.Domain.Exceptions.MovimentacaoRealizada
{
    public class MovRealSucessoException : Exception
    {
        public readonly int Id;

        public MovRealSucessoException(int _id)
        {
            this.Id = _id;
        }

        public override string Message => "Movimentação cadastrada com sucesso!";
    }
}
