using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.Patient.Model.Users
{
   public class UserOtpModel
    {
        [Required]
        public int UserId { get; set; }
        public string Otp { get; set; }
    }
}
