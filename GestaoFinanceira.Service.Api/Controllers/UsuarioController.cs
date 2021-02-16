using GestaoFinanceira.Application.Commands.Usuario;
using GestaoFinanceira.Application.Interfaces;
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
                return Ok(new { message = "Usuário atualizado com sucesso!" });
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(DeleteUsuarioCommand command)
        {
            try
            {
                usuarioApplicationService.Delete(command);
                return Ok(new { Message = "Usuário excluído com sucesso!" });
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
                return Ok(User.Identity.Name);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
                throw;
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Autenthicate")]
        public IActionResult Autenthicate(AuthenticateUsuarioCommand command)
        {
            try
            {
                var token = usuarioApplicationService.Authenticate(command);
                if (token != null)
                {
                    return Ok(new
                    {
                        message = "Usuário autenticado com sucesso!",
                        acessToken = token
                    });
                }

                return BadRequest(new { message = "e-mail e/ou senha inválidos" });

            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

     }
}