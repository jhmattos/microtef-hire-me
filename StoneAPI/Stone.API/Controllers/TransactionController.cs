using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stone.Service;

namespace Stone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        //GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<TransactionDTO>> Get(int? card)
        {
            TransactionService service = new TransactionService();
            var result = service.GetAll(card)
                    .Select(t =>
                        new TransactionDTO()
                        {
                            Valor = t.Amount,
                            Nome = t.Card.CardHolderName,
                            Tipo = t.Type == 1 ? "Crédito" : "Débito",
                            Parcelas = t.Type == 1 ? t.Number : (int?)null
                        }).ToList();
            return result;
        }

        // POST api/values
        [HttpPost]
        public ActionResult<RetornoTransacaoDTO> Post(TransacaoDTO transacao)
        {
            RetornoTransacao retorno = new TransactionService().RealizarTransacao(transacao.cartao, transacao.valor, transacao.tipo, transacao.parcelas, transacao.guid);
            return new RetornoTransacaoDTO { Codigo = (int)retorno, Descricao = GetEnumDescription(retorno) };
        }

        private static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
