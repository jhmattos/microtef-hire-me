using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stone.Service.Extensions;

namespace Stone.Service
{
    public class ClientService
    {
        public IEnumerable<Card> GetAll()
        {
            StoneEntities db = new StoneEntities();
            return db.Card.OrderBy(f => f.CardHolderName).ToList();
        }

        public Card Get(string numeroCartao)
        {
            StoneEntities db = new StoneEntities();
            return db.Card.FirstOrDefault(f => f.Number == numeroCartao);
        }

        public void ExcluirCliente(string numeroCartao)
        {
            StoneEntities db = new StoneEntities();
            Card card = db.Card.FirstOrDefault(f => f.Number == numeroCartao);
            if (card != null)
            {
                db.Transaction.RemoveRange(card.Transaction);
                db.Client.Remove(card.Client);
                db.Card.Remove(card);
            }
            db.SaveChanges();
        }

        public void SalvarCliente(string numeroCartao, string nome, string bandeira, decimal saldo, string senha, int tipo)
        {
            StoneEntities db = new StoneEntities();
            Card card = db.Card.FirstOrDefault(f => f.Number == numeroCartao);
            if (card == null)
            {
                db.Card.Add(new Card
                {
                    ExpirationDate = DateTime.Now.Date.AddYears(13),
                    CardBrand = bandeira,
                    Type = tipo,
                    HasPassword = tipo == 1,
                    CardHolderName = nome,
                    Number = numeroCartao,
                    Password = senha.GerarHashMd5(),
                    Client = new Client
                    {
                        Nome = nome,
                        Saldo = saldo
                    }
                });
                db.SaveChanges();
            }
            else
            {

                card.ExpirationDate = DateTime.Now.Date.AddYears(13);
                card.CardBrand = bandeira;
                card.Type = tipo;
                card.HasPassword = tipo == 1;
                card.CardHolderName = nome;
                card.Password = senha.GerarHashMd5();

                card.Client.Nome = nome;
                card.Client.Saldo = saldo;

                db.SaveChanges();
            }
        }
    }
}
