using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Stone.Service
{
    public class TransactionService
    {
        public IEnumerable<Transaction> GetAll(int? card)
        {
            StoneEntities db = new StoneEntities();
            return db.Transaction.Where(t => (!card.HasValue || t.CardID == card.Value)).OrderByDescending(f => f.TransactionID);
        }

        public RetornoTransacao RealizarTransacao(string cartao, decimal valor, int tipo, int parcelas, string guid)
        {
            try
            {
                StoneEntities db = new StoneEntities();
                Card card = db.Card.FirstOrDefault(f => f.Number == cartao);
                if (card != null)
                {
                    RetornoTransacao retorno = card.ValidarTransacao(valor, tipo, parcelas, guid);
                    if (retorno == RetornoTransacao.APROVADO)
                    {
                        db.SaveChanges();
                    }
                    return retorno;
                }
                return RetornoTransacao.TRANSACAO_NEGADA;
            }
            catch
            {
                return RetornoTransacao.TRANSACAO_NEGADA;
            }
        }


    }
}
