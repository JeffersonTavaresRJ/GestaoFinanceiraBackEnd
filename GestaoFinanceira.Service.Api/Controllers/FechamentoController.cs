using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Infra.CrossCutting.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GestaoFinanceira.Service.Api.Controllers
{
    [Authorize] //executam somente com token válido
    [Route("api/[controller]")]
    [ApiController]
    public class FechamentoController : Controller
    {
        private readonly IFechamentoApplicationService fechamentoApplicationService;

        public FechamentoController(IFechamentoApplicationService fechamentoApplicationService)
        {
            this.fechamentoApplicationService = fechamentoApplicationService;
        }

        [HttpPut]
        public IActionResult Put(DateTime dataReferencia )
        {
            try
            {
                UserEntity.SetUsuarioID(this.User);
                fechamentoApplicationService.Executar(UserEntity.IdUsuario, dataReferencia);
                return Ok(new { message = "Fechamento executado com sucesso!" });
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }
    }
}
