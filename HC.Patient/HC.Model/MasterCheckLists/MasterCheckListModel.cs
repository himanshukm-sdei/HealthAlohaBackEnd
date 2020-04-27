using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.MasterCheckLists
{
    public class MasterCheckListModel
    {
        public int CheckListID { get; set; }
        public int CheckListCategoryID { get; set; }
        public string CheckListPoints { get; set; }
        public int CheckListPointsOrder { get; set; }
        public string CheckListType { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int DaysBeforeAndAfter { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
