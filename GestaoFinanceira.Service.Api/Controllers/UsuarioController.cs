using FluentValidation;
using GestaoFinanceira.Application.Commands.Usuario;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.Exceptions.Usuario;
using GestaoFinanceira.Infra.CrossCutting.ValidationAdapters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GestaoFinanceira.Service.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioApplicationService usuarioApplicationService;

        public UsuarioController(IUsuarioApplicationService usuarioApplicationService)
        {
            this.usuarioApplicationService = usuarioApplicationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post(CreateUsuarioCommand command)
        {
            try
            {
                usuarioApplicationService.Add(command);
                return Ok(new { message = "Usuário cadastrado com sucesso!" });
            }
            catch(ValidationException e)
            {
                return BadRequest(ValidationAdapter.Parse(e.Errors));
            }

            catch(UsuaEmailJaCadastradoExcpetion e)
            {
                return StatusCode(418,e.Message);
            }

            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }


        [HttpPut]
        public IActionResult Put(UpdateUsuarioCommand command)
        {
            try
            {
                usuarioApplicationService.Update(command);
                return Ok(new { message = "Usuário alterado com sucesso!" });
            }
            catch (ValidationException e)
            {
                return BadRequest(ValidationAdapter.Parse(e.Errors));
            }
            catch (UsuaInvalidoException e)
            {
                return StatusCode(418, e.Message);
            }
            catch (UsuaEmailJaCadastradoExcpetion e)
            {
                return StatusCode(418, e.Message);
            }
            catch (UsuaSenhaInvalidaException e)
            {
                return StatusCode(418, e.Message);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Route("TrocaSenha")]
        public IActionResult Put(TrocaSenhaUsuarioCommand command)
        {
            try
            {
                usuarioApplicationService.Update(command);
                return Ok(new { message = "Senha alterada com sucesso!" });
            }
            catch (ValidationException e)
            {
                return BadRequest(ValidationAdapter.Parse(e.Errors));
            }
            catch (UsuaInvalidoException e)
            {
                return StatusCode(418, e.Message);
            }
            catch (UsuaSenhaInvalidaException e)
            {
                return StatusCode(418, e.Message);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                usuarioApplicationService.Delete(id);
                return Ok(new { message = "Usuário excluído com sucesso!" });
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetName()
        {
            try
            {
                return Ok( new { user = User.Identity.Name });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Autenthicate")]
        public IActionResult Autenthicate(LoginUsuarioCommand command)
        {
            try
            {
                var user = usuarioApplicationService.Authenticate(command);
                if (user != null)
                {
                    return Ok(new
                    {
                        message = "Usuário autenticado com sucesso: Docker Desktop Action Teste 13!",
                        user = user
                    });                    
                }

                return StatusCode(418,"e-mail e/ou senha inválidos");

            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }       

    }
}