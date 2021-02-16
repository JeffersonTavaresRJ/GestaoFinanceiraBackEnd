using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Infra.Caching.Repositories
{
    public class CategoriaCaching :ICategoriaCaching
    {
        private readonly MongoDBContext mongoDBContext;

        public CategoriaCaching(MongoDBContext mongoDBContext)
        {
            this.mongoDBContext = mongoDBContext;
        }

        public void Add(CategoriaDTO obj)
        {
            mongoDBContext.Categorias.InsertOne(obj);
        }

        public void Update(CategoriaDTO obj)
        {
            var filter = Builders<CategoriaDTO>.Filter.Eq(c => c.Id, obj.Id);
            mongoDBContext.Categorias.ReplaceOne(filter, obj);
        }

        public void Delete(CategoriaDTO obj)
        {
            var filter = Builders<CategoriaDTO>.Filter.Eq(c => c.Id, obj.Id);
            mongoDBContext.Categorias.DeleteOne(filter);
        }

        public CategoriaDTO GetId(int id)
        {
            var filter = Builders<CategoriaDTO>.Filter.Eq(c => c.Id, id);
            return mongoDBContext.Categorias.Find(filter).FirstOrDefault();
        }

        public List<CategoriaDTO> GetAll(int idUsuario)
        {
            var filter = Builders<CategoriaDTO>.Filter.Eq(c => c.IdUsuario, idUsuario);
            return mongoDBContext.Categorias.Find(filter).ToList();
        }        
    }
}
