using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace HC.Patient.Entity
{
    public class VideoAndArticles : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Title { get; set; }

        [Required]
        [ForeignKey("VideoAndArticlesCategories")]
        public int CategoryID { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Image { get; set; }

        [Required]
        [RequiredNumber]
        public int Days { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string BeforeAndAfter { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string ShortDescription { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Url { get; set; }

        public bool IsVideo { get; set; }

        public int OrganizationID { get; set; }

        public virtual VideoAndArticlesCategories VideoAndArticlesCategories { get; set; }
    }
}
