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
    public class SaldoDiarioCaching : ISaldoDiarioCaching
    {
        private readonly MongoDBContext mongoDBContext;

        public SaldoDiarioCaching(MongoDBContext mongoDBContext)
        {
            this.mongoDBContext = mongoDBContext;
        }

        public void Add(SaldoDiarioDTO obj)
        {
            mongoDBContext.SaldosDiario.InsertOne(obj);
        }

        public void Update(SaldoDiarioDTO obj)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter.Where(sa => sa.Conta.Id == obj.Conta.Id && sa.DataSaldo == obj.DataSaldo);
            mongoDBContext.SaldosDiario.ReplaceOne(filter, obj);
        }

        public void Delete(SaldoDiarioDTO obj)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
                 .Where(sa => sa.Conta.Id == obj.Conta.Id && sa.DataSaldo == obj.DataSaldo);
            mongoDBContext.SaldosDiario.DeleteOne(filter);
        }

        public List<SaldoDiarioDTO> GetBySaldosDiario(int idConta, DateTime dataSaldo)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
               .Where(sa => (sa.Conta.Id == idConta )
                   && sa.DataSaldo >= dataSaldo);
            return mongoDBContext.SaldosDiario.Find(filter).ToList();
        }

        public List<SaldoDiarioDTO> GetBySaldosDiario( DateTime dataSaldoIni, DateTime dataSaldoFim)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
               .Where(sa => sa.DataSaldo >= dataSaldoIni && sa.DataSaldo <= dataSaldoFim);
            return mongoDBContext.SaldosDiario.Find(filter).ToList();
        }

        public SaldoDiarioDTO GetByKey(int idConta, DateTime dataSaldo)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
               .Where(sa => sa.Conta.Id == idConta && sa.DataSaldo == dataSaldo);
            return mongoDBContext.SaldosDiario.Find(filter).FirstOrDefault<SaldoDiarioDTO>();
        }

        public List<SaldoDiarioDTO> GetAll()
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
               .Where(sa => sa.Conta.IdUsuario == UserEntity.IdUsuario);
            return mongoDBContext.SaldosDiario.Find(filter).ToList();
        }


        public List<SaldoDiarioDTO> GetGroupBySaldoDiario(DateTime dataIni, DateTime dataFim)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
               .Where(sa => (sa.Conta.IdUsuario == UserEntity.IdUsuario)
                   && sa.DataSaldo >= dataIni
                   && sa.DataSaldo <= dataFim);
            return mongoDBContext.SaldosDiario.Find(filter).ToList().OrderByDescending(sd => sd.DataSaldo).ToList();
        }
    }
}
