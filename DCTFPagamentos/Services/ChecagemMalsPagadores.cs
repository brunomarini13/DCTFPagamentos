using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCTFPagamentos.Entities;
using DCTFPagamentos.Entities.Enums;

namespace DCTFPagamentos.Services
{
    static class ChecagemMalsPagadores
    {
        public static bool ChecarPagador(string cnpj)
        {
            MalsPagadores malPagador;
            if (Enum.TryParse<MalsPagadores>(cnpj, out malPagador))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
