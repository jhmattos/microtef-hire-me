using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.WPF
{
    public class TransacaoDTO
    {
        public string cartao { get; set; }
        public decimal valor { get; set; }
        public int tipo { get; set; }
        public int parcelas { get; set; }
        public string guid { get; set; }
    }
}
