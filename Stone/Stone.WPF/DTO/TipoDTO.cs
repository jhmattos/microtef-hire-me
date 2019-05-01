using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.WPF
{
public    class TipoDTO
    {

        public int TipoID { get; set; }
        public string Nome { get; set; }

        public TipoDTO(int tipoId, string nome)
        {
            TipoID = tipoId;
            Nome = nome;
        }
        public override string ToString()
        {
            return string.Format("{0} - {1}", TipoID, Nome);
        }
    }
}
