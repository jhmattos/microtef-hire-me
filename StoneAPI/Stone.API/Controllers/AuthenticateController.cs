using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stone.Service;
namespace Stone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        // POST: api/Authenticate
        [HttpPost]
        public Stone.API.AuthenticateDTO Post(PostAuthenticateDTO postAuthenticate)
        {
            var retorno = new AuthenticationService().Authencticate(postAuthenticate.NumeroCartao, postAuthenticate.Senha);
            return new Stone.API.AuthenticateDTO { RetornoTransacao = (int)retorno.RetornoTransacao, Descricao = GetEnumDescription(retorno.RetornoTransacao), Guid = retorno.Guid };
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
