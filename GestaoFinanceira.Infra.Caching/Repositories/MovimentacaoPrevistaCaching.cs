using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Caching.Repositories
{
    public class MovimentacaoPrevistaCaching : IMovimentacaoPrevistaCaching
    {

        private readonly MongoDBContext mongoDBContext;

        public MovimentacaoPrevistaCaching(MongoDBContext mongoDBContext)
        {
            this.mongoDBContext = mongoDBContext;
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

        public MovimentacaoPrevistaDTO GetByKey(int idItemMovimentacao, DateTime dataReferencia)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.ItemMovimentacao.Id == idItemMovimentacao && mp.DataReferencia == dataReferencia.Date);
            return mongoDBContext.MovimentacoesPrevistas.Find(filter).FirstOrDefault();
        }

        public List<MovimentacaoPrevistaDTO> GetByDataVencimento(int? idItemMovimentacao, int idUsuario, DateTime dataVencIni, DateTime dataVencFim)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => ( mp.ItemMovimentacao.Id == idItemMovimentacao || idItemMovimentacao == null ) && 
                       mp.DataVencimento >= dataVencIni.Date && 
                       mp.DataVencimento <= dataVencFim.Date &&
                       mp.FormaPagamento.IdUsuario==idUsuario);
            return mongoDBContext.MovimentacoesPrevistas.Find(filter).ToList<MovimentacaoPrevistaDTO>();
        }
    }
}
