using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Entity
{
   public class UserOtp :BaseEntity
    {
        public int UserId { get; set; }

        public string Otp { get; set; }
    }
}
