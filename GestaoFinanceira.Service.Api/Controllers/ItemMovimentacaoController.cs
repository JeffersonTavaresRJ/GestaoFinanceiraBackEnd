using GestaoFinanceira.Application.Commands.ItemMovimentacao;
using GestaoFinanceira.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GestaoFinanceira.Infra.CrossCutting.ValidationAdapters;
using GestaoFinanceira.Application.Interfaces;

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

        [HttpGet("{idUsuario}")]
        public IActionResult GetAll(int idUsuario)
        {
            try
            {                
                return Ok(itemMovimentacaoApplicationService.GetAll(idUsuario));
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
