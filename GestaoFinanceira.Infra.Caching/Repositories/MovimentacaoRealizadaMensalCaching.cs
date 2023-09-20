using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
using GestaoFinanceira.Infra.CrossCutting.GenericFunctions;
using GestaoFinanceira.Infra.CrossCutting.Security;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Infra.Caching.Repositories
{
    public class MovimentacaoRealizadaMensalCaching :IMovimentacaoRealizadaMensalCaching
    {
        private readonly IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching;

        public MovimentacaoRealizadaMensalCaching(IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching)
        {
            this.movimentacaoRealizadaCaching = movimentacaoRealizadaCaching;
        }


        public List<MovimentacaoRealizadaMensalDTO> GetByDataMovimentacaoRealizada(DateTime dataMovRealIni, DateTime dataMovRealFim)
        {
            MovimentacaoRealizadaMensalDTO mov = new MovimentacaoRealizadaMensalDTO();

            mov.Conta = new ContaDTO();
            mov.Janeiro.SaldoAnterior = 100;
            var totReceitas = 100;
            var totDespesas = 100;
            mov.Janeiro.Receitas.Add(new MovimentacaoMensalDTO(new ItemMovimentacaoDTO(), totReceitas));
            mov.Janeiro.Despesas.Add(new MovimentacaoMensalDTO(new ItemMovimentacaoDTO(), totDespesas));
            mov.Janeiro.SaldoAtual = mov.Janeiro.SaldoAnterior + (totReceitas - totDespesas);

            mov.Fevereiro.SaldoAnterior = mov.Janeiro.SaldoAtual;
            totReceitas = 100;
            totDespesas = 100;
            mov.Fevereiro.Receitas.Add(new MovimentacaoMensalDTO(new ItemMovimentacaoDTO(), totReceitas));
            mov.Fevereiro.Despesas.Add(new MovimentacaoMensalDTO(new ItemMovimentacaoDTO(), totDespesas));
            mov.Fevereiro.SaldoAtual = mov.Fevereiro.SaldoAnterior + (totReceitas - totDespesas);

            return new List<MovimentacaoRealizadaMensalDTO>();
        }
    }
}
