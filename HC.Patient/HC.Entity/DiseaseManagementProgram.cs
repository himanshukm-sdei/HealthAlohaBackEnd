using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterDiseaseManagementProgram : MasterBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Description { get; set; }
        [ForeignKey("MasterOrganization")]
        public int OrganizationId { get; set; }
        public virtual MasterOrganization MasterOrganization { get; set; }

    }
}
