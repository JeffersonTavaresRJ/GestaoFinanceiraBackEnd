using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
using GestaoFinanceira.Infra.CrossCutting.GenericFunctions;
using GestaoFinanceira.Infra.CrossCutting.Security;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;

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

        public List<MovimentacaoPrevistaDTO> GetByKey(int idItemMovimentacao, DateTime dataReferencia)
        {
            var filterBuilder = Builders<MovimentacaoPrevistaDTO>.Filter;
            var dataIni = DateTimeClass.DataHoraIni(new DateTime(dataReferencia.Year, dataReferencia.Month, dataReferencia.Day));
            var dataFim = DateTimeClass.DataHoraFim(new DateTime(dataReferencia.Year, dataReferencia.Month, dataReferencia.Day));

            var filter = filterBuilder.Eq(x => x.ItemMovimentacao.Id, idItemMovimentacao) &
                         filterBuilder.Gte(x => x.DataReferencia, dataIni) &
                         filterBuilder.Lt(x => x.DataReferencia, dataFim);
            return mongoDBContext.VwMovimentacoesPrevistas.Find(filter).ToList();
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

            var filterBuilder = Builders<MovimentacaoPrevistaDTO>.Filter;
            var filtros = new List<FilterDefinition<MovimentacaoPrevistaDTO>>();

            filtros.Add(filterBuilder.Gte(mp => mp.DataVencimento, dataIni));//maior ou igual
            filtros.Add(filterBuilder.Lt(mp => mp.DataVencimento, dataFim));//menor ou igual
            filtros.Add(filterBuilder.Eq(mp => mp.FormaPagamento.IdUsuario, UserEntity.IdUsuario));//igual

            if (idItemMovimentacao != null)
            {
                filtros.Add(filterBuilder.Eq(mp=>mp.ItemMovimentacao.Id, idItemMovimentacao));
            }
            // Se houver filtros, combine-os usando AND (comportamento padrão de múltiplos critérios)
            // Se a intenção for OR, use filterBuilder.Or(filtros)
            FilterDefinition<MovimentacaoPrevistaDTO> filtroFinal = filterBuilder.And(filtros);

            return mongoDBContext.VwMovimentacoesPrevistas.Find(filtroFinal).ToList();
        }        
    }
}