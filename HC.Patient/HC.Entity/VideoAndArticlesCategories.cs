using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace HC.Patient.Entity
{
    public class VideoAndArticlesCategories : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CategoryID")]
        public int CategoryID { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string CategoryName { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string Description { get; set; }

        public int OrganizationID { get; set; }
    }
}
