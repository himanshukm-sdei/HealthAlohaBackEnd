using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterDFA_Category : MasterBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public decimal? PerfectScore { get; set; }
        [ForeignKey("MasterOrganization")]
        public int OrganizationID { get; set; }

        public virtual MasterOrganization MasterOrganization { get; set; }
        public virtual List<MasterMappingHRACategoryRisk> MappingHRACategoryRisks { get; set; }
    }
}
