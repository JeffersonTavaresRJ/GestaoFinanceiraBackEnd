using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
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

        public MovimentacaoPrevistaCaching(MongoDBContext mongoDBContext, IItemMovimentacaoCaching itemMovimentacaoCaching, IFormaPagamentoCaching formaPagamentoCaching)
        {
            this.mongoDBContext = mongoDBContext;
            this.itemMovimentacaoCaching = itemMovimentacaoCaching;
            this.formaPagamentoCaching = formaPagamentoCaching;
        }

        public void Add(MovimentacaoPrevistaDTO obj)
        {
            mongoDBContext.MovimentacoesPrevistas.InsertOne(obj);
        }

        public void Update(MovimentacaoPrevistaDTO obj)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.ItemMovimentacao.Id == obj.ItemMovimentacao.Id && mp.DataReferencia == obj.DataReferencia.Date);
            mongoDBContext.MovimentacoesPrevistas.ReplaceOne(filter, obj);  
        }

        public void Delete(MovimentacaoPrevistaDTO obj)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.ItemMovimentacao.Id == obj.ItemMovimentacao.Id && mp.DataReferencia == obj.DataReferencia.Date); 
            mongoDBContext.MovimentacoesPrevistas.DeleteOne(filter);        }

        public List<MovimentacaoPrevistaDTO> GetAll()
        {
            DateTime dataIni = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime dataFim = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

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
                       mp.DataReferencia == dataReferencia.Date &&
                       mp.FormaPagamento.IdUsuario == UserEntity.IdUsuario);
            List<MovimentacaoPrevistaDTO> movimentacoesPrevistas = mongoDBContext.MovimentacoesPrevistas.Find(filter).ToList();
            
            return Query(movimentacoesPrevistas).FirstOrDefault();
        }

        public List<MovimentacaoPrevistaDTO> GetByDataVencimento(DateTime dataVencIni, DateTime dataVencFim, int? idItemMovimentacao)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.DataVencimento >= dataVencIni.Date && 
                             mp.DataVencimento <= dataVencFim.Date &&
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
