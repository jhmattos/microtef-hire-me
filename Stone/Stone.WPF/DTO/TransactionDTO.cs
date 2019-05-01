using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.WPF
{
    public class TransactionDTO
    {
        public string Nome { get; set; }
        public double Valor { get; set; }
        public string Tipo { get; set; }
        public object Parcelas { get; set; }

    }
}
