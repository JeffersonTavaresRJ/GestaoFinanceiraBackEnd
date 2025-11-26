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
    public class MovimentacaoPrevistaCaching : IMovimentacaoPrevistaCaching
    {

        private readonly MongoDBContext mongoDBContext;
        private readonly ISaldoDiarioCaching saldoDiarioCaching;

        public MovimentacaoPrevistaCaching(MongoDBContext mongoDBContext, ISaldoDiarioCaching saldoDiarioCaching)
        {
            this.mongoDBContext = mongoDBContext;
            this.saldoDiarioCaching = saldoDiarioCaching;
        }

        public void Add(MovimentacaoPrevistaDTO obj)
        {
            mongoDBContext.MovimentacoesPrevistas.InsertOne(obj);
        }

        public void Update(MovimentacaoPrevistaDTO obj)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.Id == obj.Id);
            mongoDBContext.MovimentacoesPrevistas.ReplaceOne(filter, obj);  
        }

        public void Delete(MovimentacaoPrevistaDTO obj)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.Id == obj.Id);  
            mongoDBContext.MovimentacoesPrevistas.DeleteOne(filter);        }

        public List<MovimentacaoPrevistaDTO> GetAll()
        {
            DateTime dataIni = DateTimeClass.DataHoraIni(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
            DateTime dataFim = DateTimeClass.DataHoraFim(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)));

            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.DataVencimento >= dataIni &&
                             mp.DataVencimento <= dataFim &&
                             mp.FormaPagamento.IdUsuario == UserEntity.IdUsuario);
            return mongoDBContext.VwMovimentacoesPrevistas.Find(filter).ToList();
        }

        public MovimentacaoPrevistaDTO GetId(int id)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.Id == id);
            return mongoDBContext.VwMovimentacoesPrevistas.Find(filter).FirstOrDefault();
        }

        public MovimentacaoPrevistaDTO GetByKey(int idItemMovimentacao, DateTime dataReferencia)
        {
            var filterBuilder = Builders<MovimentacaoPrevistaDTO>.Filter;
            var filter = filterBuilder.Gte(x => x.ItemMovimentacao.Id, idItemMovimentacao) &
                         filterBuilder.Lt(x => x.DataReferencia, dataReferencia.ToUniversalTime());
            return mongoDBContext.VwMovimentacoesPrevistas.Find(filter).FirstOrDefault();
        }

        public List<MovimentacaoPrevistaDTO> GetByDataVencimento(DateTime? dataVencIni, DateTime? dataVencFim, int? idItemMovimentacao)
        {

            var date = dataVencIni.HasValue ? dataVencIni.Value : saldoDiarioCaching.GetAll().Max(x => x.DataSaldo);
            var dataIni = DateTimeClass.DataHoraIni(new DateTime(date.Year, date.Month, 1));
            var dataFim = DateTimeClass.DataHoraFim(new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)));

            if (dataVencIni.HasValue && dataVencFim.HasValue)
            {
                dataIni = dataVencIni.Value;
                dataFim = dataVencFim.Value;
            }

            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.DataVencimento >= dataIni.Date && 
                             mp.DataVencimento <= dataFim.Date &&
                             mp.FormaPagamento.IdUsuario== UserEntity.IdUsuario &&
                            (mp.ItemMovimentacao.Id == idItemMovimentacao || idItemMovimentacao == null) );
            return mongoDBContext.VwMovimentacoesPrevistas.Find(filter).ToList();
        }        
    }
}