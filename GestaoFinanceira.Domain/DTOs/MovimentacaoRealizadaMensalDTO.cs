using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.DTOs
{
    public class MovimentacaoRealizadaMensalDTO
    {
        public ContaDTO Conta { get; set; }
        public Mes Janeiro { get; set; }
        public Mes Fevereiro { get; set; }
        public Mes Marco { get; set; }
        public Mes Abril { get; set; }
        public Mes Maio { get; set; }
        public Mes Junho { get; set; }
        public Mes Julho { get; set; }
        public Mes Agosto { get; set; }
        public Mes Setembro { get; set; }
        public Mes Outubro { get; set; }
        public Mes Novembro { get; set; }
        public Mes Dezembro { get; set; }

    }

    public class MovimentacaoMensalDTO
    {
        public MovimentacaoMensalDTO(ItemMovimentacaoDTO itemMovimentacaoDTO, double valor)
        {
            ItemMovimentacaoDTO = itemMovimentacaoDTO;
            Valor = valor;
        }

        public ItemMovimentacaoDTO ItemMovimentacaoDTO { get; set; }
        public double Valor { get; set; }
    }

    public class Mes
    {
        public double SaldoAnterior { get; set; }        
        public List<MovimentacaoMensalDTO> Receitas { get; set; }
        public List<MovimentacaoMensalDTO> Despesas { get; set; }
        public double SaldoAtual { get; set; }
    }

    public class Teste
    {
        public void te()
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
        }
    }

}
