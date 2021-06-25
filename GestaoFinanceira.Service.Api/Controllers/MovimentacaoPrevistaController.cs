using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using GestaoFinanceira.Application.Exceptions.MovimentacaoPrevista;
using GestaoFinanceira.Application.Interfaces;
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
            catch(StatusMovimentacaoInvalidoException e)
            {
                return StatusCode(418, e.Message);
            }
            catch(Exception e)
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

        [HttpGet("GetByDataReferencia/{idItemMovimentacao?}/{dataRefIni}/{dataRefFim}")]
        public IActionResult GetAll(int? idItemMovimentacao, DateTime dataRefIni, DateTime dataRefFim)
        {
            try
            {
                if(dataRefFim.Subtract(dataRefIni).TotalDays > 90)
                {
                    return StatusCode(418, "O período excedeu o limite máximo de 90 dias");
                }
                
                return Ok(movimentacaoPrevistaApplicationService.GetByDataReferencia(idItemMovimentacao, dataRefIni, dataRefFim));
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
    }
}
