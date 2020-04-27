using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Model.Chat;
using HC.Patient.Service.IServices.Chats;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Chat")]
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        
        [HttpGet]
        [Route("GetChatHistory")]
        public JsonResult GetChatHistory(ChatParmModel chatParmModel)
        {
            return Json(_chatService.ExecuteFunctions<JsonModel>(() => _chatService.GetChatHistory(chatParmModel, GetToken(HttpContext))));
        }
    }
}