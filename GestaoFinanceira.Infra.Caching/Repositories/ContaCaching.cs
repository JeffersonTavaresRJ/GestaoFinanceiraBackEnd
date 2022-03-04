using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using GestaoFinanceira.Infra.CrossCutting.Security;

namespace GestaoFinanceira.Infra.Caching.Repositories
{
    public class ContaCaching : IContaCaching
    {
        private readonly MongoDBContext mongoDBContext;

        public ContaCaching(MongoDBContext mongoDBContext)
        {
            this.mongoDBContext = mongoDBContext;
        }

        public void Add(ContaDTO obj)
        {
            mongoDBContext.Contas.InsertOne(obj);
        }

        public void Update(ContaDTO obj)
        {
            var filter = Builders<ContaDTO>.Filter.Eq(c=>c.Id,obj.Id);
            mongoDBContext.Contas.ReplaceOne(filter, obj);
        }

        public void Delete(ContaDTO obj)
        {
            var filter = Builders<ContaDTO>.Filter.Eq(c => c.Id, obj.Id);
            mongoDBContext.Contas.DeleteOne(filter);
        }

        public ContaDTO GetId(int id)
        {
            var filter = Builders<ContaDTO>.Filter.Where(c => c.Id == id && c.IdUsuario == UserEntity.IdUsuario);
            return mongoDBContext.Contas.Find(filter).FirstOrDefault();
        }

        public List<ContaDTO> GetAll()
        {
            var filter = Builders<ContaDTO>.Filter.Eq(c => c.IdUsuario, UserEntity.IdUsuario);
            return mongoDBContext.Contas.Find(filter).ToList();
        }
    }
}
