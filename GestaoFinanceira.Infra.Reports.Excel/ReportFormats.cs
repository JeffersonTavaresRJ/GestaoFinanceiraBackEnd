using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.Reports.Excel
{
    public static class ReportFormats
    {
        public static string FormatCurrency { get { return "#,##0.00"; } }
        public static string FormatPercent { get { return "0.00%"; } }
    }
}
