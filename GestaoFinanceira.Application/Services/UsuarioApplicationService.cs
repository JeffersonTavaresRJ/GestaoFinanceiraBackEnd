using GestaoFinanceira.Application.Commands.Usuario;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Validations;
using GestaoFinanceira.Infra.CrossCutting.Security;
using FluentValidation;
using System;
using AutoMapper;
using GestaoFinanceira.Application.Exceptions.Usuario;
using GestaoFinanceira.Domain.DTOs;

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
            var usuario = usuarioDomainService.Get(command.EMail);

            if (usuario != null)
            {
                throw new EmailJaCadastradoExcpetion(command.EMail);
            }

            usuario = mapper.Map<Usuario>(command);

            var validation = new UsuarioValidation().Validate(usuario);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            usuarioDomainService.Add(usuario);
        }

        public void Update(UpdateUsuarioCommand command)
        {
            var usuario = mapper.Map<Usuario>(command);

            var user = usuarioDomainService.Get(command.EMail);
            if (user != null && user.Id != usuario.Id && user.EMail == usuario.EMail)
            {
                throw new EmailJaCadastradoExcpetion(usuario.EMail);
            }

            var validation = new UsuarioValidation().Validate(usuario);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            usuarioDomainService.Update(usuario);

        }

        public void Delete(string id)
        {
            try
            {
                var usuario = usuarioDomainService.GetId(int.Parse(id));
                usuarioDomainService.Delete(usuario);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public UsuarioDTO Authenticate(LoginUsuarioCommand command)
        {
            try
            {
                Usuario usuario = usuarioDomainService.Get(command.EMail, command.Senha);
                var user = mapper.Map<UsuarioDTO>(usuario);

                if (user != null)
                {
                    user.AccessToken = tokenService.GenerateToken(command.EMail);
                    return user;

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