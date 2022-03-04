using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
using GestaoFinanceira.Infra.CrossCutting.Security;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Caching.Repositories
{
    public class FormaPagamentoCaching : IFormaPagamentoCaching
    {
        private readonly MongoDBContext mongoDBContext;

        public FormaPagamentoCaching(MongoDBContext mongoDBContext)
        {
            this.mongoDBContext = mongoDBContext;
        }

        public void Add(FormaPagamentoDTO obj)
        {
            mongoDBContext.FormasPagamento.InsertOne(obj);
        }

        public void Update(FormaPagamentoDTO obj)
        {
            var filter = Builders<FormaPagamentoDTO>.Filter.Eq(f => f.Id, obj.Id);
            mongoDBContext.FormasPagamento.ReplaceOne(filter, obj);
        }

        public void Delete(FormaPagamentoDTO obj)
        {
            var filter = Builders<FormaPagamentoDTO>.Filter.Eq(f => f.Id, obj.Id);
            mongoDBContext.FormasPagamento.DeleteOne(filter);
        }

        public FormaPagamentoDTO GetId(int id)
        {
            var filter = Builders<FormaPagamentoDTO>.Filter.Where(f => f.Id == id && f.IdUsuario== UserEntity.IdUsuario);
            return mongoDBContext.FormasPagamento.Find(filter).FirstOrDefault();
        }

        public List<FormaPagamentoDTO> GetAll()
        {
            var filter = Builders<FormaPagamentoDTO>.Filter.Eq(f => f.IdUsuario, UserEntity.IdUsuario);
            return mongoDBContext.FormasPagamento.Find(filter).ToList();
        }
    }
}
