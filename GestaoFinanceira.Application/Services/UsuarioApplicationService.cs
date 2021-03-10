using GestaoFinanceira.Application.Commands.Usuario;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Validations;
using GestaoFinanceira.Infra.CrossCutting.Security;
using FluentValidation;
using System;
using AutoMapper;

namespace GestaoFinanceira.Application.Services
{
    public class UsuarioApplicationService : IUsuarioApplicationService
    {

        private readonly IUsuarioDomainService usuarioDomainService;
        private readonly TokenService tokenService;
        private readonly IMapper mapper;


        public UsuarioApplicationService(IUsuarioDomainService usuarioDomainService, TokenService tokenService, IMapper mapper)
        {
            this.usuarioDomainService = usuarioDomainService;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public void Add(CreateUsuarioCommand command)
        {
            try
            {
                var usuario = usuarioDomainService.Get(command.EMail);

                if (usuario != null)
                {
                    throw new Exception("O e-mail já encontra-se cadastrado para outro usuário");
                }

                //usuario = new Usuario
                //{
                //    EMail = command.EMail,
                //    Nome = command.Nome,
                //    Senha = command.Senha
                //};

                usuario = mapper.Map<Usuario>(command);

                var validation = new UsuarioValidation().Validate(usuario);
                if (!validation.IsValid)
                {
                    throw new ValidationException(validation.Errors);
                }

                usuarioDomainService.Add(usuario);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public void Update(UpdateUsuarioCommand command)
        {
            try
            {
                //var usuario = usuarioDomainService.GetId(int.Parse(command.Id));
                //usuario.Nome = command.Nome;
                //usuario.EMail = command.EMail;
                //usuario.Senha = command.Senha;

                var usuario = mapper.Map<Usuario>(command);


                var user = usuarioDomainService.Get(command.EMail);
                if (user != null && user.Id != usuario.Id && user.EMail == usuario.EMail)
                {
                    throw new Exception("O e-mail já encontra-se cadastrado para outro usuário");
                }

                var validation = new UsuarioValidation().Validate(usuario);
                if (!validation.IsValid)
                {
                    throw new ValidationException(validation.Errors);
                }

                usuarioDomainService.Update(usuario);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public void Delete(DeleteUsuarioCommand command)
        {
            try
            {
                //var usuario = usuarioDomainService.GetId(int.Parse(command.Id));
                var usuario = mapper.Map<Usuario>(command);
                usuarioDomainService.Delete(usuario);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public string Authenticate(AuthenticateUsuarioCommand command)
        {
            try
            {
                var usuario = usuarioDomainService.Get(command.EMail, command.Senha);

                if (usuario != null)
                {
                    return tokenService.GenerateToken(command.EMail);
                }

                return null;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

        }

    }
}