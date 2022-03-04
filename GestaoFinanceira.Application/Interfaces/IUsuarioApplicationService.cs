using GestaoFinanceira.Application.Commands.Usuario;
using GestaoFinanceira.Domain.DTOs;
using System.Security.Claims;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IUsuarioApplicationService
    {
        void Add(CreateUsuarioCommand command);
        void Update(UpdateUsuarioCommand command);
        void Update(TrocaSenhaUsuarioCommand command);
        void Delete(string id);
        UsuarioDTO Authenticate(LoginUsuarioCommand command);
    }       
    
}
