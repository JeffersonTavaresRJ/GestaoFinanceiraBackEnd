using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Infra.CrossCutting.ValidationAdapters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using GestaoFinanceira.Domain.Exceptions.Movimentacao;

namespace GestaoFinanceira.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovimentacaoPrevistaController : ControllerBase
    {
        private readonly IMovimentacaoPrevistaApplicationService movimentacaoPrevistaApplicationService;

        public MovimentacaoPrevistaController(IMovimentacaoPrevistaApplicationService movimentacaoPrevistaApplicationService)
        {
            this.movimentacaoPrevistaApplicationService = movimentacaoPrevistaApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateMovimentacaoPrevistaCommand command)
        {
            try
            {
                await movimentacaoPrevistaApplicationService.Add(command);
                return Ok(new { message = "Movimentação cadastrada com sucesso!" });
            }
            catch (ValidationException e)
            {
                return BadRequest(ValidationAdapter.Parse(e.Errors));
            }
            catch (MovPrevParcelaInvalidaExclusaoException e)
            {
                return StatusCode(418, e.Message);
            }
            catch (MovPrevTotalParcelasInvalidoException e)
            {
                return StatusCode(418, e.Message);
            }
            catch (MovPrevRecorrenciaInvalidaException e)
            {
                return StatusCode(418, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateMovimentacaoPrevistaCommand command)
        {
            try
            {
                await movimentacaoPrevistaApplicationService.Update(command);
                return Ok(new { message = "Movimentação atualizada com sucesso!" });
            }
            catch (ValidationException e)
            {
                return BadRequest(ValidationAdapter.Parse(e.Errors));

            }
            catch (MovDataReferenciaException e)
            {
                return StatusCode(418, e.Message);
            }
            catch (MovPrevStatusInvalidoException e)
            {
                return StatusCode(418, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("{IdItemMovimentacao}/{dataReferencia}")]
        public async Task<IActionResult> Delete(int idItemMovimentacao, DateTime dataReferencia)
        {
            try
            {
                DeleteMovimentacaoPrevistaCommand command = new DeleteMovimentacaoPrevistaCommand
                {
                    IdItemMovimentacao = idItemMovimentacao,
                    DataReferencia = dataReferencia
                };

                await movimentacaoPrevistaApplicationService.Delete(command);
                return Ok(new { message = "Movimentação excluída com sucesso!" });
            }            
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("{idItemMovimentacao}/{dataReferencia}")]
        public IActionResult Get(int idItemMovimentacao, DateTime dataReferencia)
        {
            try
            {
                return Ok(movimentacaoPrevistaApplicationService.GetByKey(idItemMovimentacao, dataReferencia));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("GetByDataVencimento/{idItemMovimentacao?}/{idUsuario}/{dataVencIni}/{dataVencFim}")]
        public IActionResult GetAll(int? idItemMovimentacao, int idUsuario, DateTime dataVencIni, DateTime dataVencFim)
        {
            try
            {
                if(dataVencFim.Subtract(dataVencIni).TotalDays > 731)
                {
                    return StatusCode(418, "O período excedeu o limite máximo de 731 dias");
                }
                
                return Ok(movimentacaoPrevistaApplicationService.GetByDataVencimento(idItemMovimentacao, idUsuario, dataVencIni, dataVencFim));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("GetAllStatus")]
        public IActionResult GetAllStatus()
        {
            try
            {
                return Ok(movimentacaoPrevistaApplicationService.GetAllStatus());
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetAllPrioridades")]
        public IActionResult GetAllPrioridades()
        {
            try
            {
                return Ok(movimentacaoPrevistaApplicationService.GetAllPrioridades());
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetAllTipoRecorrencias")]
        public IActionResult GetAllTipoRecorrencias()
        {
            try
            {
                return Ok(movimentacaoPrevistaApplicationService.GetAllTipoRecorrencias());
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }
    }
}