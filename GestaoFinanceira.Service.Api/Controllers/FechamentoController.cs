using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Infra.CrossCutting.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using GestaoFinanceira.Application.Commands.Fechamento;
using GestaoFinanceira.Application.Services;
using System.Threading.Tasks;

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

        [HttpPut("{dataReferencia}/{status}")]
        public async Task<IActionResult> Put(DateTime dataReferencia, string status)
        {
            try
            {
                UserEntity.SetUsuarioID(this.User);
                CreateFechamentoCommand fechamentoCreateCommand = new CreateFechamentoCommand
                {
                    IdUsuario = UserEntity.IdUsuario,
                    DataReferencia = dataReferencia,
                    Status = status
                };

                await fechamentoApplicationService.Executar(fechamentoCreateCommand);

                var descricaoStatus = fechamentoCreateCommand.Status == "A" ? "reaberta" : "fechada";
                return Ok(new { message = $"Movimentação {descricaoStatus} com sucesso!" });
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {

            try
            {
                UserEntity.SetUsuarioID(this.User);
                return Ok(fechamentoApplicationService.GetAll());
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }

        }
    }
}
