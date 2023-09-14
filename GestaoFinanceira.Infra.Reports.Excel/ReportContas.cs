﻿using GestaoFinanceira.Domain.DTOs;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Drawing;

namespace GestaoFinanceira.Infra.Reports.Excel
{
    public class ReportContas
    {
        public static byte[] Get(List<ContaDTO> contas) {

            //definir o tipo de licença..
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var white = ColorTranslator.FromHtml("#FFFFFF");
            var darkGrey = ColorTranslator.FromHtml("#363636");
            var lighGrey = ColorTranslator.FromHtml("#DCDCDC");


            //abrindo o conteúdo do arquivo excel..
            using(var excel = new ExcelPackage())
            {
                //criando a planilha..
                var sheet = excel.Workbook.Worksheets.Add("Contas");

                //título do relátório
                sheet.Cells["A1"].Value = "Relatório de Contas";

                var titulo = sheet.Cells["A1:C1"];
                titulo.Merge = true; //mesclar as celulas..
                titulo.Style.Font.Size = 16;
                titulo.Style.Font.Bold = true;
                titulo.Style.Font.Color.SetColor(white);
                titulo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titulo.Style.Fill.BackgroundColor.SetColor(darkGrey);
                titulo.Style.HorizontalAlignment= ExcelHorizontalAlignment.Center;


                sheet.Cells["A3"].Value = "Código da Conta";
                sheet.Cells["B3"].Value = "Nome da Conta";
                sheet.Cells["C3"].Value = "Status";

                var cabecalho = sheet.Cells["A3:C3"];
                cabecalho.Style.Font.Size = 12;
                cabecalho.Style.Font.Bold = true;
                cabecalho.Style.Font.Color.SetColor(white);
                cabecalho.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cabecalho.Style.Fill.BackgroundColor.SetColor(darkGrey);
                cabecalho.Style.HorizontalAlignment= ExcelHorizontalAlignment.Center;


                var linha = 4;
                foreach (var item in contas)
                {
                    sheet.Cells[$"A{linha}"].Value = item.Id;
                    sheet.Cells[$"B{linha}"].Value = item.Descricao;
                    sheet.Cells[$"C{linha}"].Value = item.Status.ToString();

                    if (linha % 2 == 0)
                    {
                        var conteudo = sheet.Cells[$"A{linha}:C{linha}"];
                        conteudo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        conteudo.Style.Fill.BackgroundColor.SetColor(lighGrey);
                    }

                    linha++;
                }

                var tabela = sheet.Cells[$"A3:C{linha - 1}"];
                tabela.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                return excel.GetAsByteArray();
            }           

        }

    }
}