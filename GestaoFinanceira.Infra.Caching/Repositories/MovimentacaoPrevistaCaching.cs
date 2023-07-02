using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
using GestaoFinanceira.Infra.CrossCutting.GenericFunctions;
using GestaoFinanceira.Infra.CrossCutting.Security;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Infra.Caching.Repositories
{
    public class MovimentacaoPrevistaCaching : IMovimentacaoPrevistaCaching
    {

        private readonly MongoDBContext mongoDBContext;
        private readonly IItemMovimentacaoCaching itemMovimentacaoCaching;
        private readonly IFormaPagamentoCaching formaPagamentoCaching;
        private readonly ISaldoDiarioCaching saldoDiarioCaching;

        public MovimentacaoPrevistaCaching(MongoDBContext mongoDBContext, IItemMovimentacaoCaching itemMovimentacaoCaching, IFormaPagamentoCaching formaPagamentoCaching, ISaldoDiarioCaching saldoDiarioCaching)
        {
            this.mongoDBContext = mongoDBContext;
            this.itemMovimentacaoCaching = itemMovimentacaoCaching;
            this.formaPagamentoCaching = formaPagamentoCaching;
            this.saldoDiarioCaching = saldoDiarioCaching;
        }

        public void Add(MovimentacaoPrevistaDTO obj)
        {
            mongoDBContext.MovimentacoesPrevistas.InsertOne(obj);
        }

        public void Update(MovimentacaoPrevistaDTO obj)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.ItemMovimentacao.Id == obj.ItemMovimentacao.Id && 
                       mp.DataReferencia >= DateTimeClass.DataHoraIni(obj.DataReferencia.Date) &&
                       mp.DataReferencia <= DateTimeClass.DataHoraFim(obj.DataReferencia.Date));
            mongoDBContext.MovimentacoesPrevistas.ReplaceOne(filter, obj);  
        }

        public void Delete(MovimentacaoPrevistaDTO obj)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.ItemMovimentacao.Id == obj.ItemMovimentacao.Id &&
                       mp.DataReferencia >= DateTimeClass.DataHoraIni(obj.DataReferencia.Date) &&
                       mp.DataReferencia <= DateTimeClass.DataHoraFim(obj.DataReferencia.Date)); 
            mongoDBContext.MovimentacoesPrevistas.DeleteOne(filter);        }

        public List<MovimentacaoPrevistaDTO> GetAll()
        {
            DateTime dataIni = DateTimeClass.DataHoraIni(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
            DateTime dataFim = DateTimeClass.DataHoraFim(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)));

            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.DataVencimento >= dataIni &&
                             mp.DataVencimento <= dataFim &&
                             mp.FormaPagamento.IdUsuario == UserEntity.IdUsuario);
            List<MovimentacaoPrevistaDTO>  movimentacoesPrevistas = mongoDBContext.MovimentacoesPrevistas.Find(filter).ToList();            

            return Query(movimentacoesPrevistas);
        }

        public MovimentacaoPrevistaDTO GetByKey(int idItemMovimentacao, DateTime dataReferencia)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.ItemMovimentacao.Id == idItemMovimentacao && 
                       mp.DataReferencia >= DateTimeClass.DataHoraIni(dataReferencia.Date) &&
                       mp.DataReferencia <= DateTimeClass.DataHoraFim(dataReferencia.Date) &&
                       mp.FormaPagamento.IdUsuario == UserEntity.IdUsuario);
            List<MovimentacaoPrevistaDTO> movimentacoesPrevistas = mongoDBContext.MovimentacoesPrevistas.Find(filter).ToList();
            
            return Query(movimentacoesPrevistas).FirstOrDefault();
        }

        public List<MovimentacaoPrevistaDTO> GetByDataVencimento(DateTime? dataVencIni, DateTime? dataVencFim, int? idItemMovimentacao)
        {

            var date = dataVencIni.HasValue ? dataVencIni.Value : saldoDiarioCaching.GetAll().Max(x => x.DataSaldo);
            var dataIni = DateTimeClass.DataHoraIni(new DateTime(date.Year, date.Month, 1));
            var dataFim = DateTimeClass.DataHoraFim(new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)));

            if (dataVencIni.HasValue && dataVencFim.HasValue)
            {
                dataIni = dataVencIni.Value;
                dataFim = dataVencFim.Value;
            }

            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.DataVencimento >= dataIni.Date && 
                             mp.DataVencimento <= dataFim.Date &&
                             mp.FormaPagamento.IdUsuario== UserEntity.IdUsuario &&
                            (mp.ItemMovimentacao.Id == idItemMovimentacao || idItemMovimentacao == null) );
            List<MovimentacaoPrevistaDTO> movimentacoesPrevistas = mongoDBContext.MovimentacoesPrevistas.Find(filter).ToList();
            
            return Query(movimentacoesPrevistas);            
        }

        private List<MovimentacaoPrevistaDTO> Query(List<MovimentacaoPrevistaDTO> movimentacoesPrevistas)
        {
            List<FormaPagamentoDTO> formasPagamento = formaPagamentoCaching.GetAll();
            List<ItemMovimentacaoDTO> itensMovimentacao = itemMovimentacaoCaching.GetAll();

            var query = from fp in formasPagamento
                        join mp in movimentacoesPrevistas on fp.Id equals mp.FormaPagamento.Id
                        join im in itensMovimentacao on mp.ItemMovimentacao.Id equals im.Id
                        select new MovimentacaoPrevistaDTO
                        {
                            DataReferencia = mp.DataReferencia,
                            DataVencimento = mp.DataVencimento,
                            FormaPagamento = fp,
                            ItemMovimentacao= im,          
                            Observacao = mp.Observacao,
                            Parcela = mp.Parcela,
                            Status = mp.Status,
                            StatusDescricao = mp.StatusDescricao,
                            TipoPrioridade = mp.TipoPrioridade,
                            TipoPrioridadeDescricao = mp.TipoPrioridadeDescricao,
                            Valor = mp.Valor
                        };

            return query.ToList();
        }
    }
}
