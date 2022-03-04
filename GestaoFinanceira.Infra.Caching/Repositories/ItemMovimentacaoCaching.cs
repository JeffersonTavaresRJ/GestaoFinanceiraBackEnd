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
        private readonly ICategoriaCaching categoriaCaching;

        public ItemMovimentacaoCaching(MongoDBContext mongoDBContext, ICategoriaCaching categoriaCaching)
        {
            this.mongoDBContext = mongoDBContext;
            this.categoriaCaching = categoriaCaching;
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
            List<ItemMovimentacaoDTO> itemMovimentacao = GetListItemMovimentacao(id, UserEntity.IdUsuario);
            List<CategoriaDTO> categorias = categoriaCaching.GetAll();

            return Query(itemMovimentacao, categorias).FirstOrDefault();
        }

        public List<ItemMovimentacaoDTO> GetAll()
        {
            List<ItemMovimentacaoDTO> itensMovimentacao = GetListItemMovimentacao(null, UserEntity.IdUsuario);
            List<CategoriaDTO> categorias = categoriaCaching.GetAll();

            return Query(itensMovimentacao, categorias);
        }
        
        private List<ItemMovimentacaoDTO> GetListItemMovimentacao(int? id, int idUsuario)
        {
            var filter = Builders<ItemMovimentacaoDTO>.Filter.Where(i => (i.Id == id || id==null) && i.Categoria.IdUsuario==idUsuario);
            return mongoDBContext.ItensMovimentacao.Find(filter).ToList();
        }

        private List<ItemMovimentacaoDTO> Query(List<ItemMovimentacaoDTO> itensMovimentacao, List<CategoriaDTO> categorias)
        {
            var query = from c in categorias
                        join i in itensMovimentacao on c.Id equals i.Categoria.Id
                        select new ItemMovimentacaoDTO
                        {
                            Id = i.Id,
                            Descricao = i.Descricao,
                            Status = i.Status,
                            Tipo = i.Tipo,
                            TipoDescricao = i.TipoDescricao,
                            Categoria = c
                        };

            return query.ToList();
        }
    }
}
