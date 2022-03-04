using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace GestaoFinanceira.Infra.CrossCutting.Security
{
    public static class UserEntity
    {
        private static int _IdUsuario;
        public static int IdUsuario { get { return _IdUsuario; } }

        public static void SetUsuarioID(ClaimsPrincipal User)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst("ID_USUARIO");
            _IdUsuario = int.Parse(claim.Value);
        }
    }
}
