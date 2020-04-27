using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class PatientSurgeryImages :BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }
        public int SurgeryId { get; set; }
        public int ImagesType { get; set; }

        public string PatientComments { get; set; }
        public string DoctorComments { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
    }
}
