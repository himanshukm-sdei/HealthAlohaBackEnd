using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Common;
using HC.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Authorize(Policy = "AuthorizedUser")]
    [Produces("application/json")]
    public class BaseController : Controller
    {
        [NonAction]
        public TokenModel GetToken(HttpContext httpContext)
        {
            return CommonMethods.GetTokenDataModel(httpContext);
        }
    }
}