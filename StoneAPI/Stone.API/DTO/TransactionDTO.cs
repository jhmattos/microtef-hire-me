using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stone.API
{
    public class TransactionDTO
    {
        public decimal Valor { get; internal set; }
        public int? Parcelas { get; internal set; }
        public string Nome { get; internal set; }
        public string Tipo { get; internal set; }
    }
}
