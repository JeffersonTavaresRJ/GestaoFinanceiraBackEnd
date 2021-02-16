using GestaoFinanceira.Application.Commands.Usuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IUsuarioApplicationService
    {
        void Add(CreateUsuarioCommand command);
        void Update(UpdateUsuarioCommand command);
        void Delete(DeleteUsuarioCommand command);
        string Authenticate(AuthenticateUsuarioCommand command);
    }       
    
}
