using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
using GestaoFinanceira.Infra.CrossCutting.ValidationAdapters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Post(List<MovimentacaoRealizadaCommand> command)
        {
            try
            {
                CreateMovimentacaoRealizadaCommand cmd = new CreateMovimentacaoRealizadaCommand
                {
                    MovimentacaoRealizadaCommand = command
                };

                await movimentacaoRealizadaApplicationService.Add(cmd);
                return Ok(new { message = "Movimentação(ões) cadastrada(s) com sucesso!" });
            }
            catch (MovPrevAlteraStatus e)
            {
                return StatusCode(200, e.Message);
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
                await movimentacaoRealizadaApplicationService.Update(command);
                return Ok(new { message = "Movimentação atualizada com sucesso!" });
            }
            catch (MovPrevAlteraStatus e)
            {
                return StatusCode(200, e.Message);
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
                return StatusCode(200, e.Message.Replace("gravada", "excluída"));
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
                return Ok(movimentacaoRealizadaApplicationService.GetId(id));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetByDataReferencia/{idItemMovimentacao}/{dataReferencia}")]
        public IActionResult GetByDataReferencia(int idItemMovimentacao, DateTime dataReferencia)
        {
            try
            {
                return Ok(movimentacaoRealizadaApplicationService.GetByDataReferencia(idItemMovimentacao, dataReferencia));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetByDataMovimentacaoRealizada/{idItemMovimentacao}/{idUsuario}/{dataMovRealIni}/{dataMovRealFim}")]
        public IActionResult GetByDataMovimentacaoRealizada(int? idItemMovimentacao, int idUsuario, DateTime dataMovRealIni, DateTime dataMovRealFim)
        {
            try
            {
                return Ok(movimentacaoRealizadaApplicationService.GetByDataMovimentacaoRealizada(idItemMovimentacao, idUsuario, dataMovRealIni, dataMovRealFim));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
