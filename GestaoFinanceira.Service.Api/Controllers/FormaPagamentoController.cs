using FluentValidation;
using GestaoFinanceira.Application.Commands.FormaPagamento;
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
    public class FormaPagamentoController : ControllerBase
    {
        private readonly IFormaPagamentoApplicationService formaPagamentoApplicationService;

        public FormaPagamentoController(IFormaPagamentoApplicationService formaPagamentoApplicationService)
        {
            this.formaPagamentoApplicationService = formaPagamentoApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateFormaPagamentoCommand command)
        {
            try
            {
               await formaPagamentoApplicationService.Add(command);
               return Ok(new { message = "Forma de Pagamento cadastrada com sucesso!" });
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
        public async Task<IActionResult> Put(UpdateFormaPagamentoCommand command)
        {
            try
            {
                await formaPagamentoApplicationService.Update(command);
                return Ok(new { message = "Forma de Pagamento atualizada com sucesso!" });
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
        public async Task<IActionResult> Delete(int id )
        {
            try
            {
                DeleteFormaPagamentoCommand command = new DeleteFormaPagamentoCommand { Id = id };
                await formaPagamentoApplicationService.Delete(command);
                return Ok(new { message = "Forma de Pagamento excluída com sucesso!" });
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
                return Ok(formaPagamentoApplicationService.GetById(id));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("idUsuario")]
        public IActionResult GetAll(int idUsuario)
        {
            try
            {
                return Ok(formaPagamentoApplicationService.GetAll(idUsuario));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
