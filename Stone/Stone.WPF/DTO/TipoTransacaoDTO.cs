using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.WPF
{
    public class TipoTransacaoDTO
    {
        public int TipoTransacaoID { get; set; }
        public string Nome { get; set; }

        public TipoTransacaoDTO(int tipoId, string nome)
        {
            TipoTransacaoID = tipoId;
            Nome = nome;
        }
        public override string ToString()
        {
            return string.Format("{0} - {1}",TipoTransacaoID,Nome);
        }
    }
}
