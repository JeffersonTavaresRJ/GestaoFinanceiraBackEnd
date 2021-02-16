using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Categoria
{
    public class DeleteCategoriaCommand: IRequest
    {
        public string Id { get; set; }
    }
}
