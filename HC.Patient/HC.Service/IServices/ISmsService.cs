using HC.Patient.Model.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices
{
   public interface ISmsService
    {
        string SendSms(MessageModel messageModel);
        string GenerateSMSPin(int userId);
    }
}
