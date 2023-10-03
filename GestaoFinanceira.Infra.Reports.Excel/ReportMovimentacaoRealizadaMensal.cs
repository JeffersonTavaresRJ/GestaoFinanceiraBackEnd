﻿using GestaoFinanceira.Domain.DTOs;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;

namespace GestaoFinanceira.Infra.Reports.Excel
{
    public class ReportMovimentacaoRealizadaMensal
    {
        
        public static byte[]? GetAll(List<MovimentacaoRealizadaMensalDTO> movimentacaoRealizadaMensalDTOs) {

            try
            {
                if (movimentacaoRealizadaMensalDTOs.Count == 0)
                {
                    return null;
                }

                //definir o tipo de licença..
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


                //abrindo o conteúdo do arquivo excel..
                using (var excel = new ExcelPackage())
                {
                    string cellRef;
                    int coluna;
                    int linha;
                    ExcelRange cellExc;

                    //criando a planilha..
                    var title = "MOVIMENTAÇÃO REALIZADA";
                    var sheet = excel.Workbook.Worksheets.Add(title);

                    //largura das colunas..
                    sheet.Column(1).Width = 20;
                    sheet.Column(2).Width = 20;
                    sheet.Column(3).Width = 34;

                    var larguraValores = 15;
                    sheet.Column(4).Width = larguraValores;
                    sheet.Column(5).Width = larguraValores;
                    sheet.Column(6).Width = larguraValores;
                    sheet.Column(7).Width = larguraValores;
                    sheet.Column(8).Width = larguraValores;
                    sheet.Column(9).Width = larguraValores;
                    sheet.Column(10).Width = larguraValores;
                    sheet.Column(11).Width = larguraValores;
                    sheet.Column(12).Width = larguraValores;
                    sheet.Column(13).Width = larguraValores;
                    sheet.Column(14).Width = larguraValores;
                    sheet.Column(15).Width = larguraValores;
                    sheet.Column(16).Width = larguraValores;
                    sheet.Column(17).Width = larguraValores;
                    sheet.Column(18).Width = larguraValores;

                    var totalColunas = 13;
                    List<Totalizador> celsSaldoAntPorConta = GetListTotalizador(totalColunas);
                    List<Totalizador> celsSaldoMenPorConta = GetListTotalizador(totalColunas);
                    List<Totalizador> celsRecPorConta = GetListTotalizador(totalColunas);
                    List<Totalizador> celsDesPorConta = GetListTotalizador(totalColunas);
                    List<Totalizador> celsSaldoAntTotal = GetListTotalizador(totalColunas);
                    List<Totalizador> celsSaldoMenTotal = GetListTotalizador(totalColunas);


                    sheet.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Column(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Column(2).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Column(3).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //título do relátório
                    sheet.Cells["A1"].Value = title;

                    var titulo = sheet.Cells["A1:R1"];
                    titulo.Merge = true; //mesclar as celulas..
                    titulo.Style.Font.Size = 16;
                    titulo.Style.Font.Bold = true;
                    titulo.Style.Font.Color.SetColor(Color.White);
                    titulo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    titulo.Style.Fill.BackgroundColor.SetColor(Color.Black);
                    titulo.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //cabeçalho..
                    sheet.Cells["A3"].Value = "CONTA";
                    sheet.Cells["B3"].Value = "TIPO DE MOVIMENTAÇÃO";
                    sheet.Cells["C3"].Value = "MOVIMENTAÇÃO";

                    coluna = 1;
                    foreach (var item in movimentacaoRealizadaMensalDTOs[0].SaldoContaDTOs)
                    {
                        sheet.Cells[GetCellMesAno(coluna, 3)].Value = ConvertMesAno(item.Ano, item.Mes);
                        coluna++;
                    }

                    sheet.Cells[GetCellMesAno(coluna, 3)].Value = "TOTAL";
                    coluna++;
                    sheet.Cells[GetCellMesAno(coluna, 3)].Value = "MÉDIA";

                    var cabecalho = sheet.Cells["A3:R3"];
                    cabecalho.Style.Font.Size = 10;
                    cabecalho.Style.Font.Bold = true;
                    cabecalho.Style.Font.Color.SetColor(Color.White);
                    cabecalho.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cabecalho.Style.Fill.BackgroundColor.SetColor(Color.Black);
                    cabecalho.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;




                    //================POPULATE CONTA================

                    linha = 4;
                    var contaContas = 1;
                    var linhaIniConta = 0;
                    foreach (MovimentacaoRealizadaMensalDTO movimentacaoRealizadaMensal in movimentacaoRealizadaMensalDTOs)
                    {
                        linhaIniConta = linha;
                        sheet.Cells[$"A{linha}"].Value = movimentacaoRealizadaMensal.Conta.Descricao;

                        int totalItensReceitas = movimentacaoRealizadaMensal.TiposMovimentacao.Where(t => t.Tipo.Equals("R")).Select(t => t.ItemDTOs.Count()).FirstOrDefault();
                        int totalItensDespesas = movimentacaoRealizadaMensal.TiposMovimentacao.Where(t => t.Tipo.Equals("D")).Select(t => t.ItemDTOs.Count()).FirstOrDefault();

                        var conta = sheet.Cells[$"A{linha}:A{linha + totalItensReceitas + totalItensDespesas + 1}"];


                        //SALDOS POR CONTA (SALDO ANTERIOR E SALDO MENSAL)..
                        var linhaSaldoAnterior = linha;
                        var saldoAnterior = sheet.Cells[$"C{linhaSaldoAnterior}"];
                        saldoAnterior.Value = "SALDO ANTERIOR";
                        saldoAnterior.Style.Font.Color.SetColor(Color.DarkGray);
                        saldoAnterior.Style.Font.Italic = true;
                        saldoAnterior.Style.Font.Bold = true;
                        saldoAnterior.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        var linhaSaldoMensal = linhaSaldoAnterior + totalItensReceitas + totalItensDespesas + 1;
                        var saldoMensal = sheet.Cells[$"C{linhaSaldoMensal}"];
                        saldoMensal.Value = "SALDO MENSAL";
                        saldoMensal.Style.Font.Bold = true;
                        saldoMensal.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        coluna = 1;
                        foreach (SaldoContaDTO saldoContaDTO in movimentacaoRealizadaMensal.SaldoContaDTOs)
                        {

                            cellRef = GetCellMesAno(coluna, linhaSaldoAnterior);
                            cellExc = sheet.Cells[cellRef];

                            cellExc.Value = saldoContaDTO.SaldoAnterior;
                            SetCellNumberProperties(cellExc, Color.DarkGray);
                            cellExc.Style.Font.Italic = true;
                            cellExc.Style.Font.Bold = true;
                            celsSaldoAntPorConta.Find(c => c.Coluna.Equals(coluna)).Celulas += cellRef + "+";

                            cellRef = GetCellMesAno(coluna, linhaSaldoMensal);
                            cellExc = sheet.Cells[cellRef];

                            cellExc.Value = saldoContaDTO.SaldoAtual;
                            SetCellNumberProperties(cellExc, Color.Black);
                            celsSaldoMenPorConta.Find(c => c.Coluna.Equals(coluna)).Celulas += cellRef + "+";

                            cellExc.Style.Font.Bold = true;

                            coluna++;
                        }


                        //ITENS DE MOVIMENTAÇÃO POR CONTA (RECEITA E DESPESA)..
                        linha = PopulateItensMovimentacao(movimentacaoRealizadaMensal, "R", sheet, celsRecPorConta, linha, totalColunas);
                        linha = PopulateItensMovimentacao(movimentacaoRealizadaMensal, "D", sheet, celsDesPorConta, linha, totalColunas);


                        //FORMATAÇÃO DO CONTEÚDO POR CONTA..
                        linha++;
                        var conteudo = sheet.Cells[$"A{linhaIniConta}:R{linha}"];
                        conteudo.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        if (contaContas % 2 == 0)
                        {
                            conteudo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            conteudo.Style.Fill.BackgroundColor.SetColor(Color.AntiqueWhite);
                        }

                        conta.Merge = true;
                        conta.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        linha++;
                        contaContas++;                        
                    }




                    //===============TOTAL GERAL===============/

                    var linhaIniTotalizador = linha;
                    sheet.Cells[$"A{linha}"].Value = "TOTAL GERAL";
                    var totalGeral = sheet.Cells[$"A{linha}:A{linha + 3}"];
                    totalGeral.Style.Border.BorderAround(ExcelBorderStyle.Thin);



                    //SALDO ANTERIOR..
                    PopulateTotalizadorSaldo("SA", sheet, celsSaldoAntPorConta, celsSaldoAntTotal, linha, totalColunas);

                    //RECEITA E DESPESA..
                    linha = PopulateTotalizadorTipo("R", sheet, celsRecPorConta, linha, totalColunas);
                    linha = PopulateTotalizadorTipo("D", sheet, celsDesPorConta, linha, totalColunas);

                    //SALDO MENSAL..
                    linha++;
                    PopulateTotalizadorSaldo("SM", sheet, celsSaldoMenPorConta, celsSaldoMenTotal, linha, totalColunas);


                    //FORMATAÇÃO DO CONTEÚDO DO TOTAL GERAL..
                    totalGeral.Merge = true;
                    totalGeral.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    totalGeral.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var conteudoTotalizador = sheet.Cells[$"A{linhaIniTotalizador}:R{linha}"];
                    conteudoTotalizador.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    conteudoTotalizador.Style.Font.Bold = true;
                    conteudoTotalizador.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    conteudoTotalizador.Style.Border.DiagonalUp = true;
                    conteudoTotalizador.Style.Fill.BackgroundColor.SetColor(Color.Gainsboro);




                    //===============VARIAÇÃO===============/
                    linha++;
                    var cellVariacaoPercentual = sheet.Cells[$"C{linha}"];
                    cellVariacaoPercentual.Value = "VARIAÇÃO (%)";
                    cellVariacaoPercentual.Style.Font.Bold = true;
                    cellVariacaoPercentual.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    coluna = 1;
                    while (coluna <= totalColunas)
                    {
                        var cellSaldoAntTotal = celsSaldoAntTotal.Find(c => c.Coluna.Equals(coluna)).Celulas;
                        var cellSaldoMenTotal = celsSaldoMenTotal.Find(c => c.Coluna.Equals(coluna)).Celulas;
                        cellRef = GetCellMesAno(coluna, linha);
                        cellExc = sheet.Cells[cellRef];
                        cellExc.Formula = $"({cellSaldoMenTotal}-{cellSaldoAntTotal})/{cellSaldoAntTotal}";
                        SetCellPercentProperties(cellExc);

                        //formatação condicional da célula: percentual > 0..
                        var condFormat01 = sheet.ConditionalFormatting.AddExpression(cellExc);
                        condFormat01.Formula = $"{cellRef}>0";
                        condFormat01.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                        condFormat01.Style.Font.Color.SetColor(Color.DarkGreen);

                        //formatação condicional da célula: percentual < 0..
                        var condFormat02 = sheet.ConditionalFormatting.AddExpression(cellExc);
                        condFormat02.Formula = $"{cellRef}<0";
                        condFormat02.Style.Fill.BackgroundColor.SetColor(Color.LightPink);
                        condFormat02.Style.Font.Color.SetColor(Color.Red);
                        coluna++;
                    }


                    sheet.View.FreezePanes(4, 4);
                    var tabela = sheet.Cells[$"A3:R{linha}"];
                    tabela.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                    return excel.GetAsByteArray();
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
            
        }

        private static List<Totalizador> GetListTotalizador(int totalColunas)
        {
            List<Totalizador> lista = new List<Totalizador>();
            var coluna = 1;

            while (totalColunas >= coluna)
            {
                Totalizador totalizador = new Totalizador(coluna);
                lista.Add(totalizador);
                coluna++;
            }

            return lista;            
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
                case 13: return $"P{linha}";
                case 14: return $"Q{linha}";
                case 15: return $"R{linha}";
                default: return "";
            }
        }

        private static string GetFormulaSumRow(int linha)
        {
            return $"=SUM(D{linha}:P{linha})";
        }

        private static string GetFormulaMediaRow(int linha)
        {
            return $"=SUM(D{linha}:P{linha})/COUNTIF(D{linha}:P{linha},\">0\")";
        }

        private static void SetCellNumberProperties(ExcelRange cell, Color fontColor)
        {
           cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
           cell.Style.Numberformat.Format = ReportFormats.FormatCurrency;
           cell.Style.Font.Color.SetColor(fontColor);
        }

        private static void SetCellPercentProperties(ExcelRange cell)
        {
            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            cell.Style.Numberformat.Format = ReportFormats.FormatPercent;
            cell.Style.Font.Bold = true;
        }

        private static int PopulateItensMovimentacao(MovimentacaoRealizadaMensalDTO movimentacaoRealizadaMensal, string tipo, ExcelWorksheet sheet, List<Totalizador> cellsTotalizador, int linha, int totalColunas)
        {

            List<ItemDTO> itemDTOs = movimentacaoRealizadaMensal.TiposMovimentacao.Where(t => t.Tipo.Equals(tipo)).FirstOrDefault().ItemDTOs;
            
            
            if(itemDTOs.Count > 0)
            {
                linha++;
                Color color;
                if (tipo.Equals("R"))
                {
                    sheet.Cells[$"B{linha}"].Value = "RECEITA";
                    color = Color.Blue;
                }
                else
                {
                    sheet.Cells[$"B{linha}"].Value = "DESPESA";
                    color = Color.Red;
                }


                var tipoConteudo = sheet.Cells[$"B{linha}:B{linha + itemDTOs.Count() - 1}"];
                tipoConteudo.Merge = true;
                tipoConteudo.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                tipoConteudo.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                tipoConteudo.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                tipoConteudo.Style.Font.Color.SetColor(color);

                foreach (ItemDTO itemDTO in itemDTOs)
                {
                    sheet.Cells[$"C{linha}"].Value = itemDTO.ItemMovimentacaoDTO.Descricao;
                    sheet.Cells[$"C{linha}"].Style.Font.Color.SetColor(color);
                    sheet.Cells[$"C{linha}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    var coluna = 1;
                    string cellRef;
                    ExcelRange cellExc;
                    foreach (MesItemDTO mesItemDTO in itemDTO.Meses)
                    {
                        cellRef = GetCellMesAno(coluna, linha);
                        cellExc = sheet.Cells[cellRef];
                        cellExc.Value = mesItemDTO.Valor;
                        SetCellNumberProperties(cellExc, color);
                        //Células para cálculo no agrupamento do TOTAL GERAL..
                        cellsTotalizador.Find(c => c.Coluna.Equals(coluna)).Celulas += cellRef + "+";

                        coluna++;
                    }
                    //TOTAL POR ITEM..
                    cellRef = GetCellMesAno(coluna, linha);
                    cellExc = sheet.Cells[cellRef];

                    cellExc.FormulaR1C1 = GetFormulaSumRow(linha);
                    cellExc.Style.Font.Bold = true;
                    SetCellNumberProperties(cellExc, color);

                    //MÉDIA POR ITEM..
                    coluna++;
                    cellRef = GetCellMesAno(coluna, linha);
                    cellExc = sheet.Cells[cellRef];
                    cellExc.FormulaR1C1 = GetFormulaMediaRow(linha);
                    cellExc.Style.Font.Bold = true;
                    SetCellNumberProperties(cellExc, color);

                    linha++;
                }
                --linha;
            }
            return linha;
        }

        private static int PopulateTotalizadorTipo(string tipo, ExcelWorksheet sheet, List<Totalizador> cellsTotalizador, int linha, int totalColunas)
        {

            if (cellsTotalizador[0].Celulas != null)
            {
                linha++;
                Color color;
                if (tipo.Equals("R"))
                {
                    sheet.Cells[$"B{linha}"].Value = "RECEITA";
                    color = Color.Blue;
                }
                else
                {
                    sheet.Cells[$"B{linha}"].Value = "DESPESA";
                    color = Color.Red;
                }

                var tipoConteudo = sheet.Cells[$"B{linha}:C{linha}"];
                tipoConteudo.Merge = true;
                tipoConteudo.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                tipoConteudo.Style.Font.Color.SetColor(color);

                //PERÍODO..
                var coluna = 1;
                string cellRef;
                ExcelRange cellExc;
                while (coluna <= totalColunas)
                {
                    cellRef = GetCellMesAno(coluna, linha);
                    cellExc = sheet.Cells[cellRef];
                    string sumCells = cellsTotalizador.Find(c => c.Coluna.Equals(coluna)).Celulas;
                    cellExc.Formula = sumCells.Substring(0, sumCells.Length - 1);
                    SetCellNumberProperties(cellExc, color);
                    coluna++;
                }

                //TOTAL..
                cellRef = GetCellMesAno(coluna, linha);
                cellExc = sheet.Cells[cellRef];
                cellExc.FormulaR1C1 = GetFormulaSumRow(linha);
                cellExc.Style.Font.Bold = true;
                SetCellNumberProperties(cellExc, color);

                //MÉDIA..
                coluna++;
                cellExc = sheet.Cells[GetCellMesAno(coluna, linha)];
                cellExc.FormulaR1C1 = GetFormulaMediaRow(linha);
                cellExc.Style.Font.Bold = true;
                SetCellNumberProperties(cellExc, color);

            }
            return linha;
        }

        private static void PopulateTotalizadorSaldo(string tipo, ExcelWorksheet sheet, List<Totalizador> cellsTotalizador, List<Totalizador> cellVariacao, int linha, int totalColunas)
        {
            Color color;
            var saldoConteudo = sheet.Cells[$"B{linha}:C{linha}"];
            if (tipo.Equals("SA"))
            {
                saldoConteudo.Value = "SALDO ANTERIOR";                
                saldoConteudo.Style.Font.Italic = true;
                color = Color.DarkGray;
            }
            else
            {
                saldoConteudo.Value = "SALDO MENSAL";
                color = Color.Black;
            }            
            saldoConteudo.Merge = true;
            saldoConteudo.Style.Font.Color.SetColor(color);
            saldoConteudo.Style.Border.BorderAround(ExcelBorderStyle.Thin);


            var coluna = 1;
            string cellRef;
            ExcelRange cellExc;
            while (coluna <= totalColunas)
            {
                cellRef = GetCellMesAno(coluna, linha);
                cellExc = sheet.Cells[cellRef];

                string sumCells = cellsTotalizador.Find(c => c.Coluna.Equals(coluna)).Celulas;
                cellExc.Formula = sumCells.Substring(0, sumCells.Length - 1);
                SetCellNumberProperties(cellExc, color);

                cellVariacao.Find(c => c.Coluna.Equals(coluna)).Celulas = cellRef;

                coluna++;
            }
        }
    }

    

    public class Totalizador
    {
        public Totalizador(int coluna)
        {
            Coluna = coluna;
        }

        public int Coluna { get; set; }
        public string Celulas { get; set; }    
     
    }
}