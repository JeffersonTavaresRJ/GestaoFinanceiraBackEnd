using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
using GestaoFinanceira.Infra.CrossCutting.GenericFunctions;
using GestaoFinanceira.Infra.CrossCutting.Security;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Infra.Caching.Repositories
{
    public class MovimentacaoRealizadaCaching : IMovimentacaoRealizadaCaching
    {
        private readonly MongoDBContext mongoDBContext;
        private readonly IFormaPagamentoCaching formaPagamentoCaching;
        private readonly IContaCaching contaCaching;
        private readonly IItemMovimentacaoCaching itemMovimentacaoCaching;
   
        public MovimentacaoRealizadaCaching(MongoDBContext mongoDBContext, 
                                            IFormaPagamentoCaching formaPagamentoCaching, 
                                            IContaCaching contaCaching, 
                                            IItemMovimentacaoCaching itemMovimentacaoCaching)
        {
            this.mongoDBContext = mongoDBContext;
            this.formaPagamentoCaching = formaPagamentoCaching;
            this.contaCaching = contaCaching;
            this.itemMovimentacaoCaching = itemMovimentacaoCaching;
        }

        public void Add(MovimentacaoRealizadaDTO obj)
        {
            mongoDBContext.MovimentacoesRealizadas.InsertOne(obj);
        }

        public void Update(MovimentacaoRealizadaDTO obj)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter.Eq(mr => mr.Id, obj.Id);
            mongoDBContext.MovimentacoesRealizadas.ReplaceOne(filter, obj);
        }

        public void Delete(MovimentacaoRealizadaDTO obj)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter.Eq(mr => mr.Id, obj.Id);
            mongoDBContext.MovimentacoesRealizadas.DeleteOne(filter);
        }

        public MovimentacaoRealizadaDTO GetId(int id)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter.Eq(c => c.Id, id);
            List<MovimentacaoRealizadaDTO> movimentacoesRealizadas = mongoDBContext.MovimentacoesRealizadas.Find(filter).ToList();



            return Query(movimentacoesRealizadas).FirstOrDefault();
        }

        public List<MovimentacaoRealizadaDTO> GetAll()
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter
               .Where(mr => mr.FormaPagamento.IdUsuario == UserEntity.IdUsuario);
            List<MovimentacaoRealizadaDTO> movimentacoesRealizadas = mongoDBContext.MovimentacoesRealizadas.Find(filter).ToList();

            return Query(movimentacoesRealizadas);

        }

        public List<MovimentacaoRealizadaDTO> GetByDataMovimentacaoRealizada(int? idItemMovimentacao, DateTime dataMovRealIni, DateTime dataMovRealFim)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter
               .Where(mr => (mr.ItemMovimentacao.Id == idItemMovimentacao || idItemMovimentacao == null)
                   && mr.DataMovimentacaoRealizada >= DateTimeClass.DataHoraIni(dataMovRealIni)
                   && mr.DataMovimentacaoRealizada <= DateTimeClass.DataHoraFim(dataMovRealFim)
                   && mr.FormaPagamento.IdUsuario == UserEntity.IdUsuario);
            List<MovimentacaoRealizadaDTO> movimentacoesRealizadas = mongoDBContext.MovimentacoesRealizadas.Find(filter).ToList();

            return Query(movimentacoesRealizadas);
        }

        public List<MovimentacaoRealizadaDTO> GetByDataMovimentacaoRealizada( int idConta, DateTime dataMovReal)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter
               .Where(mr => (mr.Conta.Id == idConta)
                   && mr.DataMovimentacaoRealizada >= DateTimeClass.DataHoraIni(dataMovReal)
                   && mr.DataMovimentacaoRealizada <= DateTimeClass.DataHoraFim(dataMovReal));
            List<MovimentacaoRealizadaDTO> movimentacoesRealizadas = mongoDBContext.MovimentacoesRealizadas.Find(filter).ToList();

            return Query(movimentacoesRealizadas);
        }

        public List<MovimentacaoRealizadaDTO> GetByDataReferencia(int? idItemMovimentacao, DateTime dataReferencia)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter
               .Where(mr => (mr.ItemMovimentacao.Id == idItemMovimentacao || idItemMovimentacao == null) && 
               mr.DataReferencia >= DateTimeClass.DataHoraIni(dataReferencia) &&
               mr.DataReferencia <= DateTimeClass.DataHoraFim(dataReferencia));
            List<MovimentacaoRealizadaDTO> movimentacoesRealizadas = mongoDBContext.MovimentacoesRealizadas.Find(filter).ToList();

            return Query(movimentacoesRealizadas).OrderBy(mr => mr.DataMovimentacaoRealizada).ToList();
        }

        private List<MovimentacaoRealizadaDTO> Query(List<MovimentacaoRealizadaDTO> movimentacoesRealizadas)
        {
            List<FormaPagamentoDTO> formasPagamento = formaPagamentoCaching.GetAll();
            List<ContaDTO> contas = contaCaching.GetAll();
            List<ItemMovimentacaoDTO> itensMovimentacao = itemMovimentacaoCaching.GetAll();

            var query = from fp in formasPagamento
                        join mr in movimentacoesRealizadas on fp.Id equals mr.FormaPagamento.Id
                        join im in itensMovimentacao on mr.ItemMovimentacao.Id equals im.Id
                        join co in contas on mr.Conta.Id equals co.Id
                        select new MovimentacaoRealizadaDTO
                        {
                            Conta = co,
                            FormaPagamento = fp,
                            ItemMovimentacao = im,
                            DataMovimentacaoRealizada = mr.DataMovimentacaoRealizada,
                            DataReferencia = mr.DataReferencia,
                            Id = mr.Id,
                            Observacao = mr.Observacao,
                            TipoPrioridade = mr.TipoPrioridade,
                            TipoPrioridadeDescricao = mr.TipoPrioridadeDescricao,
                            Valor = mr.Valor
                        };

            return query.ToList();
        }
    }
}
