using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
   public class MasterMappingHRACategoryRisk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("MasterDFA_Category")]
        public int HRACategoryId { get; set; }

        [ForeignKey("MasterHRACategoryRisk")]
        public int HRACategoryRiskId { get; set; }

        public virtual MasterHRACategoryRisk MasterHRACategoryRisk { get; set; }
        public virtual MasterDFA_Category MasterDFA_Category { get; set; }
    }
}
