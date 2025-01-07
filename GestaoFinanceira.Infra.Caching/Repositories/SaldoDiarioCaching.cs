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
    public class SaldoDiarioCaching : ISaldoDiarioCaching
    {
        private readonly MongoDBContext mongoDBContext;
        private readonly IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching;
        private readonly IContaCaching contaCaching;

        public SaldoDiarioCaching(MongoDBContext mongoDBContext, 
                                  IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching,
                                  IContaCaching contaCaching)
        {
            this.mongoDBContext = mongoDBContext;
            this.movimentacaoRealizadaCaching = movimentacaoRealizadaCaching;
            this.contaCaching= contaCaching;
        }

        public void Add(SaldoDiarioDTO obj)
        {
            mongoDBContext.SaldosDiario.InsertOne(obj);
        }

        public void Update(SaldoDiarioDTO obj)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter.Where(sa => sa.Conta.Id == obj.Conta.Id && 
            sa.DataSaldo >= DateTimeClass.DataHoraIni(obj.DataSaldo) &&
            sa.DataSaldo <= DateTimeClass.DataHoraFim(obj.DataSaldo));
            mongoDBContext.SaldosDiario.ReplaceOne(filter, obj);
        }

        public void Delete(SaldoDiarioDTO obj)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
                 .Where(sa => sa.Conta.Id == obj.Conta.Id &&
                 sa.DataSaldo >= DateTimeClass.DataHoraIni(obj.DataSaldo) &&
                 sa.DataSaldo <= DateTimeClass.DataHoraFim(obj.DataSaldo));
            mongoDBContext.SaldosDiario.DeleteOne(filter);
        }

        public List<SaldoDiarioDTO> GetBySaldosDiario( DateTime dataSaldoIni, DateTime dataSaldoFim)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
               .Where(sa => (sa.Conta.IdUsuario == UserEntity.IdUsuario) &&
                             sa.DataSaldo >= DateTimeClass.DataHoraIni(dataSaldoIni) && 
                             sa.DataSaldo <= DateTimeClass.DataHoraFim(dataSaldoFim));
            var saldosDiarioDTO =  mongoDBContext.VwSaldosDiario.Find(filter).ToList();

            return SetMovimentacoesRealizadas(saldosDiarioDTO);
        }

        public SaldoDiarioDTO GetByKey(int idConta, DateTime dataSaldo)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
               .Where(sa => sa.Conta.Id == idConta && 
               sa.DataSaldo >= DateTimeClass.DataHoraIni(dataSaldo) &&
               sa.DataSaldo <= DateTimeClass.DataHoraFim(dataSaldo));
            var saldoDiarioDTO =  mongoDBContext.VwSaldosDiario.Find(filter).FirstOrDefault<SaldoDiarioDTO>();

            if(saldoDiarioDTO != null)
            {
                saldoDiarioDTO.MovimentacoesRealizadas = movimentacaoRealizadaCaching
                .GetByDataMovimentacaoRealizada(saldoDiarioDTO.Conta.Id, saldoDiarioDTO.DataSaldo);
            }           

            return saldoDiarioDTO;
        }

        public List<SaldoDiarioDTO> GetAll()
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
               .Where(sa => sa.Conta.IdUsuario == UserEntity.IdUsuario);
            var saldosDiarioDTO = mongoDBContext.VwSaldosDiario.Find(filter).ToList();

            return SetMovimentacoesRealizadas(saldosDiarioDTO);
        }

        public List<SaldoDiarioDTO> GetGroupBySaldoDiario(DateTime dataIni, DateTime dataFim)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
               .Where(sa => (sa.Conta.IdUsuario == UserEntity.IdUsuario)
                   && sa.DataSaldo >= DateTimeClass.DataHoraIni(dataIni)
                   && sa.DataSaldo <= DateTimeClass.DataHoraFim(dataFim));
            var saldosDiariosDTO = mongoDBContext.VwSaldosDiario.Find(filter).ToList();
            saldosDiariosDTO = SetMovimentacoesRealizadas(saldosDiariosDTO);

            var saldosDiariosDTOAnterior = GetMaxGroupBySaldoConta(dataIni.AddMonths(-1));
            saldosDiariosDTOAnterior = SetMovimentacoesRealizadas(saldosDiariosDTOAnterior);

            saldosDiariosDTO.AddRange(saldosDiariosDTOAnterior);

            return saldosDiariosDTO.ToList().OrderByDescending(sd => sd.DataSaldo).ToList();
        }


        public List<SaldoDiarioDTO> GetMaxGroupBySaldoConta(DateTime dataReferencia)
        {
            var dataIni = new DateTime(dataReferencia.Year, dataReferencia.Month, 1);
            var dataFim = new DateTime(dataReferencia.Year, dataReferencia.Month, DateTime.DaysInMonth(dataReferencia.Year, dataReferencia.Month));

            List<SaldoDiarioDTO> saldosDiario = GetBySaldosDiario(dataIni, dataFim);

            var listGroupConta = saldosDiario.GroupBy(x => new { x.Conta.Id })
                                    .Select(grp => new
                                    {
                                        grp.Key,
                                        ultimoLancamento = grp.OrderByDescending(x => x.DataSaldo)
                                                              .Select(x => x.DataSaldo)
                                                              .FirstOrDefault()
                                    }).ToList();

            saldosDiario.Clear();

            foreach (var item in listGroupConta)
            {
                SaldoDiarioDTO saldoDiarioDTO = GetByKey(item.Key.Id, item.ultimoLancamento);
                saldoDiarioDTO.MovimentacoesRealizadas = null;
                saldosDiario.Add(saldoDiarioDTO);
            }


            //retornando na lista todas as contas ativas, mesmo que não tenha saldo no mês..
            var contas = contaCaching.GetAll().Where(c=>c.Status==true).ToList();
            foreach (var conta in contas)
            {
                var count = saldosDiario.Where(sd => sd.Conta.Id == conta.Id).Count();
                if (count == 0)
                {
                    saldosDiario.Add(new SaldoDiarioDTO { Conta = conta, 
                                                          DataSaldo = dataFim, 
                                                          MovimentacoesRealizadas = null, 
                                                          Status = null, 
                                                          Valor = 0});
                }
            }
            return saldosDiario.OrderBy(sd=>sd.Conta.Descricao).ToList();
        }

        public double GetSaldoConta(int idConta, DateTime dataReferencia)
        {
            var dataIni = new DateTime(dataReferencia.Year, dataReferencia.Month, 1);
            var dataFim = new DateTime(dataReferencia.Year, dataReferencia.Month, DateTime.DaysInMonth(dataReferencia.Year, dataReferencia.Month));

            List<SaldoDiarioDTO> saldosDiarioDTO = GetBySaldosDiario(idConta, dataIni, dataFim).ToList();

            if(saldosDiarioDTO.Count > 0)
            {
                DateTime dataSaldoMax = saldosDiarioDTO.Select(s => s.DataSaldo).Max();

                return GetByKey(idConta, dataSaldoMax).Valor;
            }
            else
            {
                return 0;
            }         
            
        }

        private List<SaldoDiarioDTO> GetBySaldosDiario(int idConta, DateTime dataIni, DateTime dataFim)
        {
            var filter = Builders<SaldoDiarioDTO>.Filter
               .Where(sa => (sa.Conta.Id == idConta) &&
                      sa.DataSaldo >= DateTimeClass.DataHoraIni(dataIni) &&
                      sa.DataSaldo <= DateTimeClass.DataHoraFim(dataFim));

            return mongoDBContext.VwSaldosDiario.Find(filter).ToList();
        }

        private List<SaldoDiarioDTO> SetMovimentacoesRealizadas(List<SaldoDiarioDTO> saldosDiarioDTO)
        {
            List<SaldoDiarioDTO> result = new List<SaldoDiarioDTO>();  
            foreach (var item in saldosDiarioDTO)
            {
                item.MovimentacoesRealizadas = movimentacaoRealizadaCaching.GetByDataMovimentacaoRealizada(item.Conta.Id, item.DataSaldo);
                result.Add(item);
            }

            return result;
        }
    }
}
