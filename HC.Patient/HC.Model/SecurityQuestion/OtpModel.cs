using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.Patient.Model.SecurityQuestion
{
   public  class OtpModel
    {
        [Required]
        public string OTP { get; set; }
        public int Id { get; set; }
    }
}
