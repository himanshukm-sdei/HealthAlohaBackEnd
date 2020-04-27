using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterTemplates : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public string TemplateName { get; set; }

        [Column(TypeName = "text")]
        public string TemplateJson { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        [Obsolete]
        public virtual Organization Organization { get; set; }

    }
}