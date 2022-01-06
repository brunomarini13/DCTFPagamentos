using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCTFPagamentos.Entities.Enums;
using DCTFPagamentos.Entities;

namespace DCTFPagamentos.Services
{
    static class CalculadoraDeVencimento
    {
        public static string CalcularVencimento(Impostos imp, DateTime competencia)
        {
            competencia = competencia.AddMonths(1);
            if (imp == Impostos.PIS || imp == Impostos.COFINS || imp == Impostos.IPI)
            {
                DateTime vencimento = DateTime.Parse("25" + "/" + competencia.Month + "/" + competencia.Year);
                while (vencimento.DayOfWeek == DayOfWeek.Saturday || vencimento.DayOfWeek == DayOfWeek.Sunday || éFeriado(vencimento) == true)
                {
                    vencimento.AddDays(-1);
                }
                return vencimento.ToString("dd/MM/yyyy");
            }
            else if (imp == Impostos.IRPJ || imp == Impostos.CSLL)
            {
                int daysInMonth = DateTime.DaysInMonth(competencia.Year, competencia.Month);
                DateTime vencimento = DateTime.Parse(daysInMonth + "/" + competencia.Month + "/" + competencia.Year);
                while (vencimento.DayOfWeek == DayOfWeek.Saturday || vencimento.DayOfWeek == DayOfWeek.Sunday || éFeriado(vencimento) == true)
                {
                    vencimento.AddDays(-1);
                }
                return vencimento.ToString("dd/MM/yyyy");
            }
            else if (imp == Impostos.IRRF || imp == Impostos.CSRF)
            {
                DateTime vencimento = DateTime.Parse("20" + "/" + competencia.Month + "/" + competencia.Year);
                while (vencimento.DayOfWeek == DayOfWeek.Saturday || vencimento.DayOfWeek == DayOfWeek.Sunday || éFeriado(vencimento) == true)
                {
                    vencimento.AddDays(-1);
                }
                return vencimento.ToString("dd/MM/yyyy");
            }
            else
            {
                return null;
            }
        }
        public static bool éFeriado(DateTime teste)
        {
            List<DateTime> feriados = new List<DateTime>();
            feriados.Add(DateTime.Parse("01/01/2022"));
            feriados.Add(DateTime.Parse("26/02/2022"));
            feriados.Add(DateTime.Parse("15/04/2022"));
            feriados.Add(DateTime.Parse("17/04/2022"));
            feriados.Add(DateTime.Parse("21/04/2022"));
            feriados.Add(DateTime.Parse("01/05/2022"));
            feriados.Add(DateTime.Parse("16/06/2022"));
            feriados.Add(DateTime.Parse("07/09/2022"));
            feriados.Add(DateTime.Parse("12/10/2022"));
            feriados.Add(DateTime.Parse("02/11/2022"));
            feriados.Add(DateTime.Parse("15/11/2022"));
            feriados.Add(DateTime.Parse("25/12/2022"));

            foreach (DateTime dt in feriados)
            {
                if (teste == dt)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
