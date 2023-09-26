using GestaoFinanceira.Domain.DTOs;
using MongoDB.Bson;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GestaoFinanceira.Infra.Reports.Excel
{
    public class ReportMovimentacaoRealizadaMensal
    {
        
        public static byte[] GetAll(List<MovimentacaoRealizadaMensalDTO> movimentacaoRealizadaMensalDTOs) {

            //definir o tipo de licença..
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var white = ColorTranslator.FromHtml("#FFFFFF");
            var darkGrey = ColorTranslator.FromHtml("#363636");
            var lighGrey = ColorTranslator.FromHtml("#DCDCDC");
            


            //abrindo o conteúdo do arquivo excel..
            using (var excel = new ExcelPackage())
            {
                //criando a planilha..
                var title = "MOVIMENTAÇÃO REALIZADA NO PERÍODO";
                var sheet = excel.Workbook.Worksheets.Add(title);

                //largura das colunas..
                sheet.Column(1).Width = 17;                
                sheet.Column(2).Width = 24;
                sheet.Column(3).Width = 20;

                sheet.Column(4).Width = 11;
                sheet.Column(5).Width = 11;
                sheet.Column(6).Width = 11;
                sheet.Column(7).Width = 11;
                sheet.Column(8).Width = 11;
                sheet.Column(9).Width = 11;
                sheet.Column(10).Width = 11;
                sheet.Column(11).Width = 11;
                sheet.Column(12).Width = 11;
                sheet.Column(13).Width = 11;
                sheet.Column(14).Width = 11;
                sheet.Column(15).Width = 11;
                sheet.Column(16).Width = 11;

                sheet.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Column(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                sheet.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Column(2).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                sheet.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //título do relátório
                sheet.Cells["A1"].Value = title;
                
                var titulo = sheet.Cells["A1:Q1"];
                titulo.Merge = true; //mesclar as celulas..
                titulo.Style.Font.Size = 16;
                titulo.Style.Font.Bold = true;
                titulo.Style.Font.Color.SetColor(white);
                titulo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titulo.Style.Fill.BackgroundColor.SetColor(darkGrey);
                titulo.Style.HorizontalAlignment= ExcelHorizontalAlignment.Center;

                //cabeçalho..
                sheet.Cells["A3"].Value = "CONTA";
                sheet.Cells["B3"].Value = "TIPO DE MOVIMENTAÇÃO";
                sheet.Cells["C3"].Value = "MOVIMENTAÇÃO";

                var coluna = 1;
                foreach (var item in movimentacaoRealizadaMensalDTOs[0].SaldoContaDTOs)
                {
                    sheet.Cells[GetCellMesAno(coluna, 3)].Value = ConvertMesAno(item.Ano, item.Mes);
                    coluna++;
                }
                
                var cabecalho = sheet.Cells["A3:Q3"];
                cabecalho.Style.Font.Size = 12;
                cabecalho.Style.Font.Bold = true;
                cabecalho.Style.Font.Color.SetColor(white);
                cabecalho.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cabecalho.Style.Fill.BackgroundColor.SetColor(darkGrey);
                cabecalho.Style.HorizontalAlignment= ExcelHorizontalAlignment.Center;

                List<Totalizador> totalizaSaldoAnterior = new List<Totalizador>();
                List<Totalizador> totalizaSaldoMensal = new List<Totalizador>();
                List<Totalizador> totalizaReceitas = new List<Totalizador>();
                List<Totalizador> totalizaDespesas = new List<Totalizador>();

                var linha = 4;
                foreach (MovimentacaoRealizadaMensalDTO movimentacaoRealizadaMensal in movimentacaoRealizadaMensalDTOs)
                {

                    //CONTA..
                    sheet.Cells[$"A{linha}"].Value = movimentacaoRealizadaMensal.Conta.Descricao;

                    int totalItensReceitas = movimentacaoRealizadaMensal.TiposMovimentacao.Where(t=>t.Tipo.Equals("R")).Select(t=>t.ItemDTOs).Count();
                    int totalItensDespesas = movimentacaoRealizadaMensal.TiposMovimentacao.Where(t=>t.Tipo.Equals("D")).Select(t=>t.ItemDTOs).Count();
                 
                    var conta = sheet.Cells[$"A{linha}:A{linha + totalItensReceitas + totalItensDespesas + 1}"];
                    conta.Merge = true;


                    //SALDOS DA CONTA..
                    var linhaSaldoAnterior = linha;
                    var saldoAnterior = sheet.Cells[$"C{linhaSaldoAnterior}"];
                    saldoAnterior.Value = "SALDO ANTERIOR";
                    saldoAnterior.Style.Font.Color.SetColor(darkGrey);
                    saldoAnterior.Style.Font.Italic = true;

                    var linhaSaldoMensal = linhaSaldoAnterior + totalItensReceitas + totalItensDespesas + 1;
                    var saldoMensal= sheet.Cells[$"C{linhaSaldoMensal}"];
                    saldoMensal.Value = "SALDO MENSAL";
                    saldoMensal.Style.Font.Bold = true;

                    coluna = 1;                    
                    foreach (SaldoContaDTO saldoContaDTO in movimentacaoRealizadaMensal.SaldoContaDTOs)
                    {
                        sheet.Cells[GetCellMesAno(coluna, linhaSaldoAnterior)].Value = saldoContaDTO.SaldoAnterior;
                        sheet.Cells[GetCellMesAno(coluna, linhaSaldoMensal)].Value = saldoContaDTO.SaldoAtual;

                        //Soma os valores para o agrupamento TOTAL GERAL..
                        totalizaSaldoAnterior.Add(new Totalizador(coluna, saldoContaDTO.SaldoAnterior));
                        totalizaSaldoMensal.Add(new Totalizador(coluna, saldoContaDTO.SaldoAtual));

                        coluna++;
                    }


                    //MOVIMENTAÇÕES (RECEITA)..
                    linha++;
                    sheet.Cells[$"B{linha}"].Value = "RECEITA"; 
                    
                    var tipoReceita = sheet.Cells[$"B{linha}:B{linha+totalItensReceitas - 1}"];
                    tipoReceita.Merge = true;
                    tipoReceita.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    tipoReceita.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    List<ItemDTO> itemDTOs = movimentacaoRealizadaMensal.TiposMovimentacao.Where(t => t.Tipo.Equals("R")).FirstOrDefault().ItemDTOs;

                    coluna = 1;
                    foreach (ItemDTO itemDTO in itemDTOs)
                    {
                        sheet.Cells[$"C{linha}"].Value = itemDTO.ItemMovimentacaoDTO.Descricao;

                        foreach (MesItemDTO mesItemDTO in itemDTO.Meses)
                        {
                            sheet.Cells[GetCellMesAno(coluna, linha)].Value = mesItemDTO.Valor;

                            //Soma os valores para o agrupamento TOTAL GERAL..
                            totalizaReceitas.Add(new Totalizador(coluna, mesItemDTO.Valor));
                            coluna++;
                        }                        
                        linha++;
                    }


                    //MOVIMENTAÇÕES (DESPESA)..
                    sheet.Cells[$"B{linha}"].Value = "DESPESA";

                    var tipoDespesa = sheet.Cells[$"B{linha}:B{linha + totalItensDespesas - 1}"];
                    tipoDespesa.Merge = true;
                    tipoDespesa.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    tipoDespesa.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    itemDTOs = movimentacaoRealizadaMensal.TiposMovimentacao.Where(t => t.Tipo.Equals("D")).FirstOrDefault().ItemDTOs;

                    coluna = 1;
                    foreach (ItemDTO itemDTO in itemDTOs)
                    {
                        sheet.Cells[$"C{linha}"].Value = itemDTO.ItemMovimentacaoDTO.Descricao;

                        foreach (MesItemDTO mesItemDTO in itemDTO.Meses)
                        {
                            sheet.Cells[GetCellMesAno(coluna, linha)].Value = mesItemDTO.Valor;
                            //Soma os valores para o agrupamento TOTAL GERAL..
                            totalizaDespesas.Add(new Totalizador(coluna, mesItemDTO.Valor));
                            coluna++;
                        }
                        linha++;
                    }
                }

                //TOTAL GERAL..
                sheet.Cells[$"A{linha}"].Value = "TOTAL GERAL";
                var totalGeral = sheet.Cells[$"A{linha}:A{linha + 3}"];
                totalGeral.Merge = true;
                totalGeral.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                totalGeral.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                sheet.Cells[$"B{linha}"].Value = "SALDO ANTERIOR";
                var saldoTotalAnterior = sheet.Cells[$"B{linha}:C{linha}"];
                saldoTotalAnterior.Merge = true;
                saldoTotalAnterior.Style.Font.Color.SetColor(darkGrey);
                saldoTotalAnterior.Style.Font.Italic = true;

                coluna = 1;
                while (coluna <= 12)
                {
                    sheet.Cells[GetCellMesAno(coluna, linha)].Value = totalizaSaldoAnterior.Where(t=>t.Coluna.Equals(coluna))
                                                                                           .Select(t=>t.Valor)
                                                                                           .Sum();
                    coluna++;
                }




                var tabela = sheet.Cells[$"A3:C{linha - 1}"];
                tabela.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                return excel.GetAsByteArray();
            }           

        }

        private static string ConvertMesAno(int ano, int mes)
        {
            switch(mes)
            {
                case 1: return $"JANEIRO/{ano}";
                case 2: return $"FEVEREIRO/{ano}";
                case 3: return $"MARÇO/{ano}";
                case 4: return $"ABRIL/{ano}";
                case 5: return $"MAIO/{ano}";
                case 6: return $"JUNHO/{ano}";
                case 7: return $"JULHO/{ano}";
                case 8: return $"AGOSTO/{ano}";
                case 9: return $"SETEMBRO/{ano}";
                case 10: return $"OUTUBRO/{ano}";
                case 11: return $"NOVEMBRO/{ano}";
                case 12: return $"DEZEMBRO/{ano}";
                default: return "";
            }
        }

        private static string GetCellMesAno(int coluna, int linha)
        {
            switch (coluna)
            {
                case 1: return $"D{linha}";
                case 2: return $"E{linha}";
                case 3: return $"F{linha}";
                case 4: return $"G{linha}";
                case 5: return $"H{linha}";
                case 6: return $"I{linha}";
                case 7: return $"J{linha}";
                case 8: return $"K{linha}";
                case 9: return $"L{linha}";
                case 10: return $"M{linha}";
                case 11: return $"N{linha}";
                case 12: return $"O{linha}"; 
                default: return "";
            }
        }
     
    }

    public class Totalizador
    {
        public Totalizador(int coluna, double valor)
        {
            Coluna = coluna;
            Valor = valor;
        }

        public int Coluna { get; set; }
        public double Valor { get; set; }
    }
}