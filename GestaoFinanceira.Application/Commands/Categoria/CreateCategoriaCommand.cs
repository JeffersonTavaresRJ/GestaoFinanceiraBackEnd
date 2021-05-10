using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Categoria
{
    public class CreateCategoriaCommand :IRequest
    {
        public string Descricao { get; set; }
        public int IdUsuario { get; set; }
    }
}
