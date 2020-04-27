using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterBenchmark : MasterBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName="varchar(250)")]
        public string Name { get; set; }
        [ForeignKey("MasterOrganization")]
        public int OrganizationId { get; set; }
        public virtual MasterOrganization MasterOrganization { get; set;}
    }
}
