using HC.Patient.Model.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Common
{
    public class NotificationModel
    {
        public List<MessageNotificationModel> MessageNotification { get; set; }
        public List<UserDocumentNotificationModel> UserDocumentNotification { get; set; }
    }
    public class MessageNotificationModel
    {
        public int MessageId { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public DateTime MessageDate { get; set; }
        public int ParentMessageId { get; set; }
        public string Thumbnail { get; set; }
        public bool IsPatient { get; set; }
    }
    public class UserDocumentNotificationModel
    {
        public int DocumentId { get; set; }
        public string Message { get; set; }
        public string DocumentName { get; set; }
        public string Expiration { get; set; }
    }
}
