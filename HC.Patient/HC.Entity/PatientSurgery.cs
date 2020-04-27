using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
   public class PatientSurgery :BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }
        public int SurgeryId { get; set; }

        public DateTime SurgeryDateTime { get; set; }
        [StringLength(100)]
        public int Status { get; set; }


    }
}
