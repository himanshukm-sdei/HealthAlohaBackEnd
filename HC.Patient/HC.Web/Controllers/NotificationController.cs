using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.Common;
using HC.Patient.Service.Token.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Notification")]
    public class NotificationController : BaseController
    {
        private readonly ITokenService _tokenService;
        public NotificationController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("GetHeaderNotification")]
        public JsonResult GetHeaderNotification()
        {
            NotificationModel notificationModel = _tokenService.GetLoginNotification(GetToken(HttpContext));

            return Json (new JsonModel() { data = notificationModel, Message = StatusMessage.FetchMessage, StatusCode = (int)HttpStatusCode.OK });
        }
    }
}