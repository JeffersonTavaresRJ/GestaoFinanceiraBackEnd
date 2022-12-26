using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoRealizada;
using GestaoFinanceira.Infra.CrossCutting.Security;
using GestaoFinanceira.Infra.CrossCutting.ValidationAdapters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GestaoFinanceira.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovimentacaoRealizadaController : ControllerBase
    {
        private readonly IMovimentacaoRealizadaApplicationService movimentacaoRealizadaApplicationService;
 
        public MovimentacaoRealizadaController(IMovimentacaoRealizadaApplicationService movimentacaoRealizadaApplicationService)
        {
            this.movimentacaoRealizadaApplicationService = movimentacaoRealizadaApplicationService;            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateMovimentacaoRealizadaCommand command)
        {
            try
            {
                UserEntity.SetUsuarioID(this.User);
                await movimentacaoRealizadaApplicationService.Add(command);
                return Ok();
            }
            catch (MovRealSucessoException e )
            {
                return Ok(new { message = e.Message, id = e.Id });
            }

            catch (MovPrevAlteraStatus e)
            {
                return Ok(new { message = e.Message, id = e.Id });
            }
            catch (ValidationException e)
            {
                return BadRequest(ValidationAdapter.Parse(e.Errors));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateMovimentacaoRealizadaCommand command)
        {
            try
            {
                UserEntity.SetUsuarioID(this.User);
                await movimentacaoRealizadaApplicationService.Update(command);
                return Ok(new { message = "Movimentação atualizada com sucesso!" });
            }
            catch (MovPrevAlteraStatus e)
            {
                return Ok(new { message = e.Message });
            }
            catch (ValidationException e)
            {
                return BadRequest(ValidationAdapter.Parse(e.Errors));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                DeleteMovimentacaoRealizadaCommand command = new DeleteMovimentacaoRealizadaCommand
                {
                    Id = id
                };
                await movimentacaoRealizadaApplicationService.Delete(command);
                return Ok(new { message = "Movimentação excluída com sucesso!" });
            }
            catch (MovPrevAlteraStatus e)
            {
                  return Ok(new { message = e.Message.Replace("gravada", "excluída") });
            }
            catch (ValidationException e)
            {
                return BadRequest(ValidationAdapter.Parse(e.Errors));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("GetId/{id}")]
        public IActionResult GetId(int id)
        {
            try
            {
                UserEntity.SetUsuarioID(this.User);
                return Ok(movimentacaoRealizadaApplicationService.GetId(id));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetByDataReferencia/{idItemMovimentacao?}/{dataReferencia?}")]
        public IActionResult GetByDataReferencia(int? idItemMovimentacao, DateTime? dataReferencia)
        {
            try
            {
                UserEntity.SetUsuarioID(this.User);
                return Ok(movimentacaoRealizadaApplicationService.GetByDataReferencia(idItemMovimentacao, dataReferencia));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetByDataMovimentacaoRealizada/{dataMovRealIni}/{dataMovRealFim}/{idItemMovimentacao?}")]
        public IActionResult GetByDataMovimentacaoRealizada(DateTime dataMovRealIni, DateTime dataMovRealFim, int? idItemMovimentacao=null)
        {
            try
            {
                if (dataMovRealFim.Subtract(dataMovRealIni).TotalDays > 366)
                {
                    return StatusCode(418, "O período excedeu o limite máximo de 366 dias");
                }
                UserEntity.SetUsuarioID(this.User);
                return Ok(movimentacaoRealizadaApplicationService.GetByDataMovimentacaoRealizada(idItemMovimentacao, dataMovRealIni, dataMovRealFim));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetGroupBySaldoDiario/{dataMovRealIni}/{dataMovRealFim}")]
        public IActionResult GetGroupBySaldoDiario(DateTime dataMovRealIni, DateTime dataMovRealFim)
        {
            try
            {
                if (dataMovRealFim.Subtract(dataMovRealIni).TotalDays > 31)
                {
                    return StatusCode(418, "O período excedeu o limite máximo de 31 dias");
                }
                UserEntity.SetUsuarioID(this.User);
                return Ok(movimentacaoRealizadaApplicationService.GetGroupBySaldoDiario(dataMovRealIni, dataMovRealFim));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetMaxGroupBySaldoConta/{dataReferencia?}")]
        public IActionResult GetMaxGroupBySaldoConta(DateTime? dataReferencia)
        {
            try
            {
                UserEntity.SetUsuarioID(this.User);
                return Ok(movimentacaoRealizadaApplicationService.GetMaxGroupBySaldoConta(dataReferencia));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
