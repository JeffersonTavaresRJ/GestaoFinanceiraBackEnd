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
    public class MovimentacaoRealizadaCaching :IMovimentacaoRealizadaCaching
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
            return mongoDBContext.VwMovimentacoesRealizadas.Find(filter).FirstOrDefault();
        }

        public List<MovimentacaoRealizadaDTO> GetAll()
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter
               .Where(mr => mr.FormaPagamento.IdUsuario == UserEntity.IdUsuario);
            return mongoDBContext.VwMovimentacoesRealizadas.Find(filter).ToList();

        }

        public List<MovimentacaoRealizadaDTO> GetByDataMovimentacaoRealizada(int? idItemMovimentacao, DateTime dataMovRealIni, DateTime dataMovRealFim)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter
               .Where(mr => mr.DataMovimentacaoRealizada >= DateTimeClass.DataHoraIni(dataMovRealIni)
                         && mr.DataMovimentacaoRealizada <= DateTimeClass.DataHoraFim(dataMovRealFim)
                         && mr.FormaPagamento.IdUsuario == UserEntity.IdUsuario
                         && (mr.ItemMovimentacao.Id == idItemMovimentacao || idItemMovimentacao == null));

            return mongoDBContext.VwMovimentacoesRealizadas.Find(filter).ToList();
        }

        public List<MovimentacaoRealizadaDTO> GetByDataMovimentacaoRealizada(List<int> idContas, DateTime dataMovRealIni, DateTime dataMovRealFim)
        {
            if (idContas.Count == 0) { throw new Exception("Conta é campo obrigatório"); };
            
            var builder = Builders<MovimentacaoRealizadaDTO>.Filter;
            var filter =  builder.Where(mr => mr.DataMovimentacaoRealizada >= DateTimeClass.DataHoraIni(dataMovRealIni)
                                           && mr.DataMovimentacaoRealizada <= DateTimeClass.DataHoraFim(dataMovRealFim)
                                           && mr.FormaPagamento.IdUsuario == UserEntity.IdUsuario) &
                          builder.In(mr => mr.Conta.Id, idContas);

            return mongoDBContext.VwMovimentacoesRealizadas.Find(filter).ToList();
        }

        public List<MovimentacaoRealizadaDTO> GetByDataMovimentacaoRealizada( int idConta, DateTime dataMovReal)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter
               .Where(mr=> mr.DataMovimentacaoRealizada >= DateTimeClass.DataHoraIni(dataMovReal)
                        && mr.DataMovimentacaoRealizada <= DateTimeClass.DataHoraFim(dataMovReal)
                        && (mr.Conta.Id == idConta));

            return mongoDBContext.VwMovimentacoesRealizadas.Find(filter).ToList();
         }

        public List<MovimentacaoRealizadaDTO> GetByDataReferencia(int? idItemMovimentacao, DateTime dataReferencia)
        {
            var filter = Builders<MovimentacaoRealizadaDTO>.Filter
               .Where(mr => mr.DataReferencia >= DateTimeClass.DataHoraIni(dataReferencia) &&
                            mr.DataReferencia <= DateTimeClass.DataHoraFim(dataReferencia) &&
                            (mr.ItemMovimentacao.Id == idItemMovimentacao || idItemMovimentacao == null));

            return mongoDBContext.VwMovimentacoesRealizadas.Find(filter).ToList();
        }
        
    }
}
