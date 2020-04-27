using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.VideoAndArticle
{
    public class VideoAndArticlesCategoriesModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int OrganizationID { get; set; }
        public int? UpdatedBy { get; set; }        
        public DateTime? UpdatedDate { get; set; }
        public int? TotalRecords { get; set; }


    }
}
