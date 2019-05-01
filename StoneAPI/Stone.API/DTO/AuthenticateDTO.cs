using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stone.API
{
    public class AuthenticateDTO
    {
        public int RetornoTransacao { get; set; }
        public string Guid { get; set; }
        public string Descricao { get; set; }
    }
}
