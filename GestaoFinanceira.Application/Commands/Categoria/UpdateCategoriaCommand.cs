﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Categoria
{
    public class UpdateCategoriaCommand : IRequest
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public int IdUsuario { get; set; }
    }
}