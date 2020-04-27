using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
   public class PushNotificationsModel
    {
        public string EndPoint { get; set; }
        public string ServerKey { get; set; }
        public string[] DeviceToken { get; set; }
        public string Message { get; set; }
      
    }
}
