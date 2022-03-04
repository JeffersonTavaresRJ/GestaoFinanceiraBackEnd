using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using GestaoFinanceira.Application.Interfaces;
using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Domain.Models.Enuns;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.Services
{
    public class MovimentacaoPrevistaApplicationService : IMovimentacaoPrevistaApplicationService
    {

        private readonly IMediator mediator;
        private readonly IMovimentacaoPrevistaCaching movimentacaoPrevistaCaching;

        public MovimentacaoPrevistaApplicationService(IMediator mediator, IMovimentacaoPrevistaCaching movimentacaoPrevistaCaching)
        {
            this.mediator = mediator;
            this.movimentacaoPrevistaCaching = movimentacaoPrevistaCaching;
        }

        public Task Add(CreateMovimentacaoPrevistaCommand command)
        {
            return mediator.Send(command);
        }

        public Task Update(UpdateMovimentacaoPrevistaCommand command)
        {
            return mediator.Send(command);
        }
        
        public Task Delete(DeleteMovimentacaoPrevistaCommand command)
        {
            return mediator.Send(command);
        }

        public MovimentacaoPrevistaDTO GetByKey(int idItemMovimentacao, DateTime dataReferencia)
        {
            return movimentacaoPrevistaCaching.GetByKey(idItemMovimentacao, dataReferencia);
        }

        public List<MovimentacaoPrevistaDTO> GetByDataVencimento(DateTime dataVencIni, DateTime dataVencFim, int? idItemMovimentacao)
        {
            return movimentacaoPrevistaCaching.GetByDataVencimento(dataVencIni, dataVencFim, idItemMovimentacao);
        }

        public IList GetAllStatus()
        {
            return ExtensionEnum.Listar(typeof(StatusMovimentacaoPrevista));
        }

        public IList GetAllPrioridades()
        {
            return ExtensionEnum.Listar(typeof(TipoPrioridade));
        }

        public IList GetAllTipoRecorrencias()
        {
            return ExtensionEnum.Listar(typeof(TipoRecorrencia));
        }

        
    }
}
