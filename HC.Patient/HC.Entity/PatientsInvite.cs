using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
   public class PatientsInvite :BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string  LastName { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        public string Otp { get; set; }
        public int? UserID { get; set; }

        [Required]
        public Guid InvitetID { get; set; }

    }
}
