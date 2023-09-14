using FluentValidation;
using GestaoFinanceira.Application.Commands.Conta;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Infra.CrossCutting.Security;
using GestaoFinanceira.Infra.CrossCutting.ValidationAdapters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
                UserEntity.SetUsuarioID(this.User);
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
                UserEntity.SetUsuarioID(this.User);
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

        [HttpGet]
        public IActionResult GetAll()
        {

            try
            {
                UserEntity.SetUsuarioID(this.User);
                return Ok(contaApplicationService.GetAll());
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("GetReport")]
        public IActionResult GetReport() {
            try
            {
                var file = contaApplicationService.GetReport();

                if (file != null)
                {
                    return File(file,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
