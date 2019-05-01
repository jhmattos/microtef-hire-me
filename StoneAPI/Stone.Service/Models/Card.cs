using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stone.Service.Extensions;

namespace Stone.Service
{
    public partial class Card
    {
        public RetornoTransacao ValidarTransacao(decimal valor, int tipo, int parcelas, string guid)
        {
            if (guid != this.guid)
            {
                return RetornoTransacao.TRANSACAO_NEGADA;
            }
            if (valor < (decimal)0.1)
            {
                return RetornoTransacao.VALOR_INVALIDO;
            }
            if (valor > Client.Saldo)
            {
                return RetornoTransacao.SALDO_INSUFICIENTE;
            }

            Client.Saldo -= valor;
            Transaction.Add(new Service.Transaction { Amount = valor, CardID = CardID, Number = tipo == 2 ? 1 : parcelas, Type = tipo });

            return RetornoTransacao.APROVADO;
        }

        public RetornoTransacao ValidarSenha(string senha)
        {
            if (senha.GerarHashMd5() == Password)
            {
                guid = Guid.NewGuid().ToString();
                QtdTentativas = 0;
                return RetornoTransacao.AUTENTICADO;
            }
            if (QtdTentativas.HasValue && QtdTentativas.Value > 3)
            {
                QtdTentativas++;
                return RetornoTransacao.CARTAO_BLOQUEADO;
            }
            if (senha.Length < 4 || senha.Length > 6)
            {
                QtdTentativas = QtdTentativas.HasValue ? QtdTentativas.Value + 1 : 1;
                return RetornoTransacao.ERRO_TAMANHO_SENHA;
            }
            if (senha.GerarHashMd5() != Password)
            {
                QtdTentativas = QtdTentativas.HasValue ? QtdTentativas.Value + 1 : 1;
                return RetornoTransacao.SENHA_INVALIDA;
            }
            QtdTentativas = QtdTentativas.HasValue ? QtdTentativas.Value + 1 : 1;
            return RetornoTransacao.TRANSACAO_NEGADA;
        }
    }
}
