using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
 
    public class MasterDFA_SectionItem : MasterBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("MasterDFA_Section")]   
        public int SectionId { get; set; }  

        [ForeignKey("MasterDFA_CategoryCode")]  
        public int Itemtype { get; set; }//type of the question like textbox or radiobutton etc.

        public string ItemLabel { get; set; }   

        [ForeignKey("MasterDFA_Category")]
        public int? CategoryId { get; set; }  

        public int? DisplayOrder { get; set; }
        public string QuestionLogo { get; set; }

        public virtual MasterDFA_Section MasterDFA_Section { get; set; }  
        public virtual MasterDFA_CategoryCode MasterDFA_CategoryCode { get; set; }  
        public virtual MasterDFA_Category MasterDFA_Category { get; set; }
    }
}
