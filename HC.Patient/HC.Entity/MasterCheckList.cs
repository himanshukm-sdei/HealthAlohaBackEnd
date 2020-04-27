using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class MasterCheckList : MasterBaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CheckListID")]
        public int CheckListID { get; set; }

        [Required]
        [ForeignKey("MasterCheckListCategory")]
        public int CheckListCategoryID { get; set; }

        [Column(TypeName = "varchar(4)")]
        public string CheckListType { get; set; }
        [Required]
        [RequiredNumber]
        public int DaysBeforeAndAfter { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string CheckListPoints { get; set; }
        [Required]
        [RequiredNumber]
        public int CheckListPointsOrder { get; set; }

        public virtual MasterCheckListCategory CheckListCategory { get; set; }

    }
}
