using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stone.API
{
    public class PostAuthenticateDTO
    {
        public string NumeroCartao { get; set; }
        public string Senha { get; set; }
    }
}
