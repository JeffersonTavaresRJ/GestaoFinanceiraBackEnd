using FluentValidation;
using GestaoFinanceira.Application.Commands.Conta;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Infra.CrossCutting.ValidationAdapters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace GestaoFinanceira.Service.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        private readonly IContaApplicationService contaApplicationService;

        public ContaController(IContaApplicationService contaApplicationService)
        {
            this.contaApplicationService = contaApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateContaCommand command)
        {
            try
            {
                await contaApplicationService.Add(command);
                return Ok(new { message = "Conta cadastrada com sucesso!" });
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
        public async Task<IActionResult> Put(UpdateContaCommand command)
        {
            try
            {
                await contaApplicationService.Update(command);
                return Ok(new { message = "Conta alterada com sucesso!" });
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
                DeleteContaCommand command = new DeleteContaCommand { Id = id };
                await contaApplicationService.Delete(command);
                return Ok(new { message = "Conta excluída com sucesso!" });
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
                return Ok(contaApplicationService.GetId(id));
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("GetAll/{idUsuario}")]
        public IActionResult GetAll(int idUsuario)
        {

            try
            {
                return Ok(contaApplicationService.GetAll(idUsuario));
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }

        }
    }
}
