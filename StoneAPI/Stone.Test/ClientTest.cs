using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stone.Service;

namespace Stone.Test
{
    [TestClass]
    public class ClientTest
    {

        private const string NUMERO_CARTAO = "4532437582098235";
        private const string NOME = "João Henrique Liberato Dias de Mattos";
        private const string BANDEIRA = "VISA";
        private const int SALDO = 1000000;
        private const string SENHA = "1379";
        private const int TIPO = 1;

        ClientService clientService = new ClientService();

        [TestMethod]
        public void TesteIncluir()
        {
            if (clientService.Get(NUMERO_CARTAO) != null)
            {
                TesteExcluir();
            }
            clientService.SalvarCliente(NUMERO_CARTAO, NOME, BANDEIRA, SALDO, SENHA, TIPO);

            Card card = clientService.Get(NUMERO_CARTAO);

            Assert.IsNotNull(card);
            Assert.AreEqual(NUMERO_CARTAO, card.Number);
            Assert.AreEqual(NOME, card.Client.Nome);
            Assert.AreEqual(NOME, card.CardHolderName);
            Assert.AreEqual(BANDEIRA, card.CardBrand);
            Assert.AreEqual(TIPO, card.Type);
        }

        [TestMethod]
        public void TesteExcluir()
        {
            if (clientService.Get(NUMERO_CARTAO) == null)
            {
                TesteIncluir();
            }
            clientService.ExcluirCliente(NUMERO_CARTAO);
            Card card = clientService.Get(NUMERO_CARTAO);

            Assert.IsNull(card);

        }

        [TestMethod]
        public void TesteObter()
        {
            if (clientService.Get(NUMERO_CARTAO) == null)
            {
                TesteIncluir();
            }
            Card card = clientService.Get(NUMERO_CARTAO);
            Assert.IsNotNull(card);

            IEnumerable<Card> cards = clientService.GetAll();

            Assert.IsTrue(cards.Count() > 0);
        }

        [TestMethod]
        public void TesteAlterar()
        {
            if (clientService.Get(NUMERO_CARTAO) == null)
            {
                TesteIncluir();
            }

            Card card = clientService.Get(NUMERO_CARTAO);
            Assert.IsNotNull(card);

            string bandeiraNova = "MasterCard";
            clientService.SalvarCliente(NUMERO_CARTAO, NOME, bandeiraNova, SALDO, SENHA, TIPO);

            card = clientService.Get(NUMERO_CARTAO);
            Assert.IsNotNull(card);

            Assert.AreEqual(bandeiraNova, card.CardBrand);

        }



    }
}
