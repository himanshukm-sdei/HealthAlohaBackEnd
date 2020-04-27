using HC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Chat
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsSeen { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public DateTime ChatDate { get; set; }
        public bool IsRecieved { get; set; }
    }

    public class ChatParmModel : FilterModel
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int OrganizationId { get; set; }
    }

    public class ChatConnectedUserModel
    {
        public string ConnectionId { get; set; }
        public int UserId { get; set; }
    }
}
