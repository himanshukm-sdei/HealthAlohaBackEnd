using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterCheckListCategory: MasterBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CheckListCategoryID")]
        public int CheckListCategoryID { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string CategoryName { get; set; }
    }
}
