using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterDFA_Section : MasterBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("MasterDFA_Document")]
        public int DocumentId { get; set; }
        public string SectionName { get; set; }
        public int? DisplayOrder { get; set; }
        [ForeignKey("MasterGlobalCode")]
        public int? HRAGenderCriteria { get; set; }
        public virtual MasterDFA_Document MasterDFA_Document { get; set; }
        public virtual MasterGlobalCode MasterGlobalCode { get; set; }

    }
}
