using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class CheckList : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CheckListID")]
        public int CheckListID { get; set; }

        [Required]
        [ForeignKey("CheckListCategory")]
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

        public int OrganizationID { get; set; }
        public virtual CheckListCategory CheckListCategory { get; set; }
    }
}
