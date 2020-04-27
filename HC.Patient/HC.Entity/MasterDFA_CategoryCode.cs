using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterDFA_CategoryCode : MasterBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("MasterDFA_Category")]
        public int CategoryId { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public decimal Score { get; set; }        
        public string OptionLogo { get; set; }

        //Forign key
        public virtual MasterDFA_Category MasterDFA_Category { get; set; }
    }
}
