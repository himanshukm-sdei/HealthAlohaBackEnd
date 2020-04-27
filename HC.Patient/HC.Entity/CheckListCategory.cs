using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class CheckListCategory: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CheckListCategoryID")]
        public int CheckListCategoryID { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string CategoryName { get; set; }
        public int OrganizationID { get; set; }
    }
}
