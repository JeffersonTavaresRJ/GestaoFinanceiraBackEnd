using GestaoFinanceira.Application.Commands.Usuario;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Infra.CrossCutting.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IUsuarioApplicationService
    {
        void Add(CreateUsuarioCommand command);
        void Update(UpdateUsuarioCommand command);
        void Delete(string id);
        UsuarioDTO Authenticate(LoginUsuarioCommand command);
    }       
    
}
