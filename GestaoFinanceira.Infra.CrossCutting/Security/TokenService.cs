using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace GestaoFinanceira.Infra.CrossCutting.Security
{
    public class TokenService
    {
        private readonly AppSetting appSetting;

        public TokenService(AppSetting appSetting)
        {
            this.appSetting = appSetting;
        }

        public string GenerateToken(int idUsuario, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSetting.Secret);

            //conteudo do token..
            var tokenDescription = new SecurityTokenDescriptor
            {
                //definições do usuário..
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, username),
                    new Claim("ID_USUARIO", idUsuario.ToString())
                }),

                //tempo de expiração..
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(appSetting.ValidForMinutes)),

                //criptografia do token..
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);

        }
    }
}
