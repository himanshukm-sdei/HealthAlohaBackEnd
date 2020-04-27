using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.VideoAndArticle
{
    public class VideoAndArticlesModel
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int Days { get; set; }
        public string BeforeAndAfter { get; set; } 
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Image { get; set; }
        public string ImageBase64 { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsVideo { get; set; }
        public int OrganizationID { get; set; }
        public string ShortDescription { get; set; }
        public string Title { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Url { get; set; }
        public int? TotalRecords { get; set; }

    }
}
