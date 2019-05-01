using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.Service
{
    public enum RetornoTransacao
    {
        [Description("Transação aprovada")] APROVADO = 1,
        [Description("Transação negada")] TRANSACAO_NEGADA = 2,
        [Description("Portador do cartão não possui saldo")] SALDO_INSUFICIENTE = 3,
        [Description("Mínimo de 10 centavos")] VALOR_INVALIDO = 4,
        [Description("Cartão bloqueado")] CARTAO_BLOQUEADO = 5,
        [Description("Senha deve ter entre 4 e 6 dítigos")] ERRO_TAMANHO_SENHA = 6,
        [Description("A senha enviada é inválida")] SENHA_INVALIDA = 7,
        [Description("Autenticado")] AUTENTICADO = 8
    }
}
