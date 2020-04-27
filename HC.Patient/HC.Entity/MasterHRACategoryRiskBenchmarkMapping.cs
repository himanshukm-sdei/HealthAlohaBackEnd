using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
   public class MasterHRACategoryRiskBenchmarkMapping : MasterBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int? Id { get; set; }

        [ForeignKey("MasterHRACategoryRisk")]
        public int? MasterHRACategoryRiskId { get; set; }

        [ForeignKey("MasterBenchmark")]
        public int? MasterBenchmarkId { get; set; }

        public string Description { get; set; }

        public virtual MasterHRACategoryRisk MasterHRACategoryRisk { get; set; }
        public virtual MasterBenchmark MasterBenchmark { get; set; }
    }
}
