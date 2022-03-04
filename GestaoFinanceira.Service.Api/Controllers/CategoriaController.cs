using GestaoFinanceira.Application.Commands.Categoria;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Infra.CrossCutting.ValidationAdapters;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using GestaoFinanceira.Infra.CrossCutting.Security;

namespace GestaoFinanceira.Service.Api.Controllers
{
    [Authorize] //executam somente com token válido
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaApplicationService categoriaApplicationService;

        public CategoriaController(ICategoriaApplicationService categoriaApplicationService)
        {
            this.categoriaApplicationService = categoriaApplicationService;            
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCategoriaCommand command)
        {
            try
            {
                UserEntity.SetUsuarioID(this.User);
                await categoriaApplicationService.Add(command);
                return Ok(new { message = "Categoria cadastrada com sucesso!" });
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
        public async Task<IActionResult> Put(UpdateCategoriaCommand command)
        {
            try
            {
                UserEntity.SetUsuarioID(this.User);
                await categoriaApplicationService.Update(command);
                return Ok(new { message = "Categoria alterada com sucesso!" });
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
                DeleteCategoriaCommand command = new DeleteCategoriaCommand() { Id = id };
                await categoriaApplicationService.Delete(command);
                return Ok(new { message = "Categoria excluída com sucesso!" });
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
                return Ok(categoriaApplicationService.GetId(id));
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
                return Ok(categoriaApplicationService.GetAll());
            }
            catch (Exception e)
            {
                
                return StatusCode(500, e.Message);
            }

        }       

    }
}