using FluentValidation;
using GestaoFinanceira.Application.Commands.ItemMovimentacao;
using GestaoFinanceira.Application.Interfaces;
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
    public class ItemMovimentacaoController : ControllerBase
    {

        private readonly IItemMovimentacaoApplicationService itemMovimentacaoApplicationService;       

        public ItemMovimentacaoController(IItemMovimentacaoApplicationService itemMovimentacaoApplicationService)
        {
            this.itemMovimentacaoApplicationService = itemMovimentacaoApplicationService;            
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateItemMovimentacaoCommand command)
        {
            try
            {
                UserEntity.SetUsuarioID(this.User);
                await itemMovimentacaoApplicationService.Add(command);
                return Ok(new { message = "Item de movimentação cadastrado com sucesso!" });

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
        public async Task<IActionResult> Put(UpdateItemMovimentacaoCommand command)
        {
            try
            {
                UserEntity.SetUsuarioID(this.User);
                await itemMovimentacaoApplicationService.Update(command);
                return Ok(new { message = "Item de movimentação alterado com sucesso!" });
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
                var command = new DeleteItemMovimentacaoCommand { Id = id };
                await itemMovimentacaoApplicationService.Delete(command);
                return Ok(new { message = "Item de movimentação excluído com sucesso!" });

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
                return Ok(itemMovimentacaoApplicationService.GetAll());
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
                return Ok(itemMovimentacaoApplicationService.GetId(id));
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }        

        [HttpGet("GetAllTipo")]
        public IActionResult GetAllTipo()
        {
            try
            {
                return Ok(itemMovimentacaoApplicationService.GetAllTipo());
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }        
    }
}
