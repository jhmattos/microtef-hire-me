﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.WPF
{
    public class ClienteDTO
    {
        public string Nome { get; set; }

        public string NumeroCartao { get; set; }
        public string Bandeira { get; set; }
        public int Tipo { get; set; }
        public string Senha { get; set; }
        public decimal Saldo { get; set; }
    }
}
