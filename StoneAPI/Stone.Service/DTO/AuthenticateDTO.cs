using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.Service
{
    public class AuthenticateDTO
    {
        public AuthenticateDTO()
        {
        }

        public AuthenticateDTO(RetornoTransacao retornoTransacao, string guid)
        {
            RetornoTransacao = retornoTransacao;
            Guid = guid;
        }

        public RetornoTransacao RetornoTransacao { get; set; }
        public string Guid { get; set; }
    }
}
