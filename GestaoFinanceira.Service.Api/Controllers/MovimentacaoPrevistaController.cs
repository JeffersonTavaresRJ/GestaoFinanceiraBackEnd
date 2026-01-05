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
using System.Collections.Generic;
using GestaoFinanceira.Infra.CrossCutting.Security;

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
        public async Task<IActionResult> Post([FromBody] List<MovimentacaoPrevistaCommand> command)
        {
            try
            {
                CreateMovimentacaoPrevistaCommand cmd = new CreateMovimentacaoPrevistaCommand
                {
                    MovimentacaoPrevistaCommand = command
                };

                UserEntity.SetUsuarioID(this.User);
                await movimentacaoPrevistaApplicationService.Add(cmd);
                return Ok(new { message = "Movimentação(ões) cadastrada(s) com sucesso!" });
            }
            catch (ValidationException e)
            {
                return BadRequest(ValidationAdapter.Parse(e.Errors));
            }
            catch (MovPrevParcelaInvalidaExclusaoException e)
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
                UserEntity.SetUsuarioID(this.User);
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
                UserEntity.SetUsuarioID(this.User);
                return Ok(movimentacaoPrevistaApplicationService.GetByKey(idItemMovimentacao, dataReferencia));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("GetByDataVencimento/{dataVencIni?}/{dataVencFim?}/{idItemMovimentacao?}")]
        public IActionResult GetAll(DateTime? dataVencIni, DateTime? dataVencFim, int? idItemMovimentacao)
        {
            try
            {
                if(dataVencIni.HasValue && dataVencFim.HasValue && dataVencFim.Value.Subtract(dataVencIni.Value).TotalDays > 731)
                {
                    return StatusCode(418, "O período excedeu o limite máximo de 731 dias");
                }
                UserEntity.SetUsuarioID(this.User);
                return Ok(movimentacaoPrevistaApplicationService.GetByDataVencimento(dataVencIni, dataVencFim, idItemMovimentacao));
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