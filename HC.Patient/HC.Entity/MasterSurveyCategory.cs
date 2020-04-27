using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterSurveyCategory : MasterBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int SurveyCategoryID { get; set; }
        public string SurveyCategoryName { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        [ForeignKey("MasterOrganization")]
        public int OrganizationID { get; set; }        
        public virtual MasterOrganization MasterOrganization { get; set; }
    }
}
