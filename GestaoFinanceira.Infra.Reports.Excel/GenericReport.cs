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
    public abstract class GenericReport<GenericDTO> where GenericDTO : class
    {
        
        public static byte[] GetAll(string title, List<GenericDTO> generics) {

            //definir o tipo de licença..
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var white = ColorTranslator.FromHtml("#FFFFFF");
            var darkGrey = ColorTranslator.FromHtml("#363636");
            var lighGrey = ColorTranslator.FromHtml("#DCDCDC");


            //abrindo o conteúdo do arquivo excel..
            using (var excel = new ExcelPackage())
            {
                //criando a planilha..
                var sheet = excel.Workbook.Worksheets.Add(title);

                //largura das colunas..
                sheet.Column(1).Width = 12;                
                sheet.Column(2).Width = 40;
                sheet.Column(3).Width = 12;

                sheet.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //título do relátório
                sheet.Cells["A1"].Value = title;
                
                var titulo = sheet.Cells["A1:C1"];
                titulo.Merge = true; //mesclar as celulas..
                titulo.Style.Font.Size = 16;
                titulo.Style.Font.Bold = true;
                titulo.Style.Font.Color.SetColor(white);
                titulo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titulo.Style.Fill.BackgroundColor.SetColor(darkGrey);
                titulo.Style.HorizontalAlignment= ExcelHorizontalAlignment.Center;


                sheet.Cells["A3"].Value = "Código";
                sheet.Cells["B3"].Value = "Nome";
                sheet.Cells["C3"].Value = "Status";

                var cabecalho = sheet.Cells["A3:C3"];
                cabecalho.Style.Font.Size = 12;
                cabecalho.Style.Font.Bold = true;
                cabecalho.Style.Font.Color.SetColor(white);
                cabecalho.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cabecalho.Style.Fill.BackgroundColor.SetColor(darkGrey);
                cabecalho.Style.HorizontalAlignment= ExcelHorizontalAlignment.Center;

               
                var linha = 4;
                foreach (GenericDTO item in generics)
                {

                    sheet.Cells[$"A{linha}"].Value = item.GetType().GetProperty("Id").GetValue(item, null);
                    sheet.Cells[$"B{linha}"].Value = item.GetType().GetProperty("Descricao").GetValue(item, null);
                    sheet.Cells[$"C{linha}"].Value = bool.Parse(item.GetType().GetProperty("Status").GetValue(item, null).ToString())==true?"Ativo":"Inativo";

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