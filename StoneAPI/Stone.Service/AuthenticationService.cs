using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.Service
{
    public class AuthenticationService
    {
        public AuthenticateDTO Authencticate(string numeroCartao, string senha)
        {
            StoneEntities db = new StoneEntities();

            Card card = db.Card.FirstOrDefault(f => f.Number == numeroCartao);
            if (card != null)
            {
                RetornoTransacao retornoTransacao = card.ValidarSenha(senha);
                db.SaveChanges();
                return new AuthenticateDTO(retornoTransacao, retornoTransacao == RetornoTransacao.AUTENTICADO ? card.guid : null);
            }


            return new AuthenticateDTO(RetornoTransacao.TRANSACAO_NEGADA, null);
        }
    }
}
