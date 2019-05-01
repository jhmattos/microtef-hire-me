using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stone.Service;

namespace Stone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        // GET: api/Client
        [HttpGet]
        public IEnumerable<ClienteDTO> Get()
        {
            return new ClientService().GetAll().Select(card => new ClienteDTO
            {
                Bandeira = card.CardBrand,
                Nome = card.CardHolderName,
                NumeroCartao = card.Number,
                Saldo = card.Client.Saldo,
                Senha = string.Empty,
                Tipo = card.Type
            });
        }

        // GET: api/Client/5
        [HttpGet("{id}", Name = "Get")]
        public ClienteDTO Get(string id)
        {
            Card card = new ClientService().Get(id);
            if (card != null)
            {
                return new ClienteDTO { Bandeira = card.CardBrand, Nome = card.CardHolderName, NumeroCartao = card.Number, Saldo = card.Client.Saldo, Senha = string.Empty, Tipo = card.Type };
            }
            return new ClienteDTO();
        }

        // POST: api/Client
        [HttpPost]
        public RetornoTransacaoDTO Post(ClienteDTO cliente)
        {
            try
            {
                new ClientService().SalvarCliente(cliente.NumeroCartao, cliente.Nome, cliente.Bandeira, cliente.Saldo, cliente.Senha, cliente.Tipo);
                return new RetornoTransacaoDTO { Codigo = 1, Descricao = "Dados salvos com sucesso" };
            }
            catch (Exception ex)
            {
                return new RetornoTransacaoDTO()
                {
                    Codigo = 0,
                    Descricao = ex.Message
                };
            }
        }

      

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public RetornoTransacaoDTO Delete(string id)
        {
            try
            {
                new ClientService().ExcluirCliente(id);
                return new RetornoTransacaoDTO() { Codigo = 1, Descricao = "Exclusão efetuada com sucesso" };
            }
            catch (Exception ex)
            {
                return new RetornoTransacaoDTO() { Codigo = 0, Descricao = ex.Message };
            }
        }
    }
}
