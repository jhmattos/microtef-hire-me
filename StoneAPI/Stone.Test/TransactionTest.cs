using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stone.Service;
using System.Linq;

namespace Stone.Test
{
    [TestClass]
    [DeploymentItem("EntityFramework.SqlServer.dll")]
    public class TransactionTest
    {
        private const string NUMERO_CARTAO = "4532437582098235";
        private const string NOME = "João Henrique Liberato Dias de Mattos";
        private const string BANDEIRA = "VISA";
        private const int SALDO = 1000000;
        private const string SENHA = "1379";
        private const int TIPO = 1;
        private const double VALOR_INVALIDO = 0.01;

        TransactionService transactionService = new TransactionService();
        ClientService clientService = new ClientService();
        AuthenticationService authenticationService = new AuthenticationService();

        [TestMethod]
        public void TesteValorInvalido()
        {
            var card = PrepararTeste();

            var authenticate = authenticationService.Authencticate(card.Number, SENHA);
            Assert.AreEqual(authenticate.RetornoTransacao, RetornoTransacao.AUTENTICADO);
            Assert.IsNotNull(authenticate.Guid);

            RetornoTransacao retorno = transactionService.RealizarTransacao(card.Number, Convert.ToDecimal(VALOR_INVALIDO), 1, 1, authenticate.Guid);

            Assert.AreEqual(retorno, RetornoTransacao.VALOR_INVALIDO);
        }

        [TestMethod]
        public void TesteSaldoInsuficiente()
        {
            var card = PrepararTeste();
            int tipo = 1;
            int parcelas = 1;


            var authenticate = authenticationService.Authencticate(card.Number, SENHA);
            Assert.AreEqual(authenticate.RetornoTransacao, RetornoTransacao.AUTENTICADO);
            Assert.IsNotNull(authenticate.Guid);
            RetornoTransacao retorno = transactionService.RealizarTransacao(card.Number, card.Client.Saldo + 1, tipo, parcelas, authenticate.Guid);

            Assert.AreEqual(retorno, RetornoTransacao.SALDO_INSUFICIENTE);
        }

        [TestMethod]
        public void TesteErroTamanhoSenha()
        {
            var card = PrepararTeste();


            var authenticate = authenticationService.Authencticate(card.Number, SENHA + "111111");
            Assert.AreEqual(authenticate.RetornoTransacao, RetornoTransacao.ERRO_TAMANHO_SENHA);
            Assert.IsNull(authenticate.Guid);
        }

        [TestMethod]
        public void TesteSenhaInvalida()
        {
            var card = PrepararTeste();

            string senhaIncorreta = "12345";
     


            var authenticate = authenticationService.Authencticate(card.Number, senhaIncorreta);
            Assert.AreEqual(authenticate.RetornoTransacao, RetornoTransacao.SENHA_INVALIDA);
            Assert.IsNull(authenticate.Guid);
        }

        [TestMethod]
        public void TesteTransacaoNegada()
        {
            string numeroCartaoErrado = "AAAAA";
            string senhaIncorreta = "1111";
        

            var authenticate = authenticationService.Authencticate(numeroCartaoErrado, senhaIncorreta);
            Assert.AreEqual(authenticate.RetornoTransacao, RetornoTransacao.TRANSACAO_NEGADA);
            Assert.IsNull(authenticate.Guid);
        }

        [TestMethod]
        public void TransacaoAprovada()
        {
            Card card = PrepararTeste();
            decimal saldo = card.Client.Saldo;
            int totalTransacao = card.Transaction.Count;

            decimal valor = 1;
            int tipo = 1;
            int parcelas = 1;


            var authenticate = authenticationService.Authencticate(card.Number, SENHA);
            Assert.AreEqual(authenticate.RetornoTransacao, RetornoTransacao.AUTENTICADO);
            Assert.IsNotNull(authenticate.Guid);

            RetornoTransacao retorno = transactionService.RealizarTransacao(NUMERO_CARTAO, valor, tipo, parcelas, authenticate.Guid);

            card = PrepararTeste();

            Assert.AreEqual(retorno, RetornoTransacao.APROVADO);
            Assert.AreEqual(saldo - valor, card.Client.Saldo);
            Assert.AreEqual(totalTransacao + 1, card.Transaction.Count);
            Assert.AreEqual(valor, card.Transaction.Last().Amount);
        }

        [TestMethod]
        public void TesteCartaoBloqueado()
        {
            Card card = PrepararTeste();
            int qtdTentantivas = card.QtdTentativas.HasValue ? card.QtdTentativas.Value : 0;
  
            AuthenticateDTO authenticate;
            for (int i = qtdTentantivas; i < 4; i++)
            {
                authenticate = authenticationService.Authencticate(card.Number, "12345");
                Assert.AreEqual(authenticate.RetornoTransacao, RetornoTransacao.SENHA_INVALIDA);
                Assert.IsNull(authenticate.Guid);
            }

            authenticate = authenticationService.Authencticate(card.Number, "12345");
            Assert.AreEqual(authenticate.RetornoTransacao, RetornoTransacao.CARTAO_BLOQUEADO);
            Assert.IsNull(authenticate.Guid);
        }

        private Card PrepararTeste()
        {
            var card = clientService.Get(NUMERO_CARTAO);
            if (card != null)
            {
                return card;
            }
            clientService.SalvarCliente(NUMERO_CARTAO, NOME, BANDEIRA, SALDO, SENHA, TIPO);
            return clientService.Get(NUMERO_CARTAO);

        }
    }
}
