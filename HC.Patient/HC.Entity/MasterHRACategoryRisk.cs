using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
   public class MasterHRACategoryRisk :MasterBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ReferralLinks { get; set; }
        [ForeignKey("MasterOrganization")]
        public int? OrganizationId { get; set; }
        public int? SortOrder { get; set; }
        [ForeignKey("MasterGlobalCode")]
        public int? HRAGenderCriteria { get; set; }
        public virtual MasterOrganization MasterOrganization { get; set; }
        public virtual MasterGlobalCode MasterGlobalCode { get; set; }

        public string ExecutiveDescription { get; set; }
    }
}
