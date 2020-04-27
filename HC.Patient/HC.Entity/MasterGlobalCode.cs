using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterGlobalCode : MasterBaseEntity
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("GlobalCodeID")]
        public int Id { get; set; }     
        public string GlobalCodeName { get; set; }
        
        public string GlobalCodeValue { get; set; }
                
        [ForeignKey("MasterGlobalCodeCategory")]
        public int GlobalCodeCategoryID { get; set; }
                
        public int? DisplayOrder { get; set; }

        [NotMapped]        
        public string value { get { return this.GlobalCodeValue; } set { this.GlobalCodeValue = value; } }

        [Required]        
        [ForeignKey("MasterOrganization")]
        public int? OrganizationID { get; set; }

        public virtual MasterOrganization MasterOrganization { get; set; }
        public virtual MasterGlobalCodeCategory MasterGlobalCodeCategory { get; set; }        
    }
}
