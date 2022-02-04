using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Caching.Repositories
{
    public class MovimentacaoRealizadaCaching : IMovimentacaoRealizadaCaching
    {
        private readonly MongoDBContext mongoDBContext;

        public MovimentacaoRealizadaCaching(MongoDBContext mongoDBContext)
        {
            this.mongoDBContext = mongoDBContext;
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
            return mongoDBContext.MovimentacoesRealizadas.Find(filter).FirstOrDefault();
        }

        public List<MovimentacaoRealizadaDTO> GetByDataMovimentacaoRealizada(int? idItemMovimentacao, int idUsuario, DateTime dataMovRealIni, DateTime dataMovRealFim)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter
               .Where(mr => (mr.ItemMovimentacao.Id == idItemMovimentacao || idItemMovimentacao == null)
                   && mr.DataMovimentacaoRealizada >= dataMovRealIni
                   && mr.DataMovimentacaoRealizada <= dataMovRealFim
                   && mr.FormaPagamento.IdUsuario == idUsuario);
            return mongoDBContext.MovimentacoesRealizadas.Find(filter).ToList();
        }

        public List<MovimentacaoRealizadaDTO> GetByDataMovimentacaoRealizada( int idConta, DateTime dataMovReal)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter
               .Where(mr => (mr.Conta.Id == idConta)
                   && mr.DataMovimentacaoRealizada == dataMovReal);
            return mongoDBContext.MovimentacoesRealizadas.Find(filter).ToList();
        }

        public List<MovimentacaoRealizadaDTO> GetByDataReferencia(int idItemMovimentacao, DateTime dataReferencia)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter
               .Where(mr => mr.ItemMovimentacao.Id == idItemMovimentacao && mr.DataReferencia == dataReferencia);
            return mongoDBContext.MovimentacoesRealizadas.Find(filter).ToList();
        }
    }
}
