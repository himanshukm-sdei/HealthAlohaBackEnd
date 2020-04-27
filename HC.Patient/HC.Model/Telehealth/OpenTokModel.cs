using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Telehealth
{
    public class OpenTokModel
    {
        public OpenTokModel()
        {
            ApiKey = 46084802;
            ApiSecret = "11793d1afb60cd8c99a9a549f287e67ebd25feea";
        }
        public int ApiKey { get; set; }
        public string ApiSecret { get; set; }

        public string Token { get; set; }
        public string SessionID { get; set; }

        public string Name { get; set; }
    }
}
