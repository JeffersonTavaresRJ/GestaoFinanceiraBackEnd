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
                .Where(mp => mp.ItemMovimentacao.Id == obj.ItemMovimentacao.Id && 
                       mp.DataReferencia >= DateTimeClass.DataHoraIni(obj.DataReferencia.Date) &&
                       mp.DataReferencia <= DateTimeClass.DataHoraFim(obj.DataReferencia.Date));
            mongoDBContext.MovimentacoesPrevistas.ReplaceOne(filter, obj);  
        }

        public void Delete(MovimentacaoPrevistaDTO obj)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.ItemMovimentacao.Id == obj.ItemMovimentacao.Id &&
                       mp.DataReferencia >= DateTimeClass.DataHoraIni(obj.DataReferencia.Date) &&
                       mp.DataReferencia <= DateTimeClass.DataHoraFim(obj.DataReferencia.Date)); 
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

        public MovimentacaoPrevistaDTO GetByKey(int idItemMovimentacao, DateTime dataReferencia)
        {
            var filter = Builders<MovimentacaoPrevistaDTO>.Filter
                .Where(mp => mp.ItemMovimentacao.Id == idItemMovimentacao && 
                       mp.DataReferencia >= DateTimeClass.DataHoraIni(dataReferencia.Date) &&
                       mp.DataReferencia <= DateTimeClass.DataHoraFim(dataReferencia.Date) &&
                       mp.FormaPagamento.IdUsuario == UserEntity.IdUsuario);
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