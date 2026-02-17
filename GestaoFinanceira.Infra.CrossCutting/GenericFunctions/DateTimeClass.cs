using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Infra.CrossCutting.GenericFunctions
{
    public static class DateTimeClass
    {
        public static DateTime DataHoraIni(DateTime dateTime)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0);
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day) + ts;
        }

        public static DateTime DataHoraFim(DateTime dateTime)
        {
            TimeSpan ts = new TimeSpan(11,0,0);
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day) + ts; 
        }

        public static DateTime DataHoraIniUTC(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);
        }

        public static DateTime DataHoraFimUTC(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,23,59,59, DateTimeKind.Utc);
        }
    }
}
