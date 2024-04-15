using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
using GestaoFinanceira.Infra.CrossCutting.Security;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Infra.Caching.Repositories
{
    public class ItemMovimentacaoCaching : IItemMovimentacaoCaching
    {
        private readonly MongoDBContext mongoDBContext;

        public ItemMovimentacaoCaching(MongoDBContext mongoDBContext)
        {
            this.mongoDBContext = mongoDBContext;
        }

        public void Add(ItemMovimentacaoDTO obj)
        {
            mongoDBContext.ItensMovimentacao.InsertOne(obj);
        }

        public void Update(ItemMovimentacaoDTO obj)
        {
            var filter = Builders<ItemMovimentacaoDTO>.Filter.Eq(i=>i.Id,obj.Id);
            mongoDBContext.ItensMovimentacao.ReplaceOne(filter, obj);
        }

        public void Delete(ItemMovimentacaoDTO obj)
        {
            var filter = Builders<ItemMovimentacaoDTO>.Filter.Eq(i => i.Id, obj.Id);
            mongoDBContext.ItensMovimentacao.DeleteOne(filter);
        }

        public ItemMovimentacaoDTO GetId(int id)
        {
            var filter = Builders<ItemMovimentacaoDTO>.Filter.Eq(i => i.Id, id);
            return mongoDBContext.VwItensMovimentacao.Find(filter).First();
        }

        public List<ItemMovimentacaoDTO> GetAll()
        {
            var filter = Builders<ItemMovimentacaoDTO>.Filter.Eq(i => i.Categoria.IdUsuario, UserEntity.IdUsuario);
            return mongoDBContext.VwItensMovimentacao.Find(filter).ToList().OrderBy(c => c.Descricao).ToList();
        }        
        
    }
}
