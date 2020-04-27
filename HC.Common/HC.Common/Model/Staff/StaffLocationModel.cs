namespace HC.Common.Model.Staff
{
    public class StaffLocationModel
    {
        public int Id { get; set; } // LocationID
        public bool IsDefault { get; set; }
    }

    public class StaffTeamModel
    {   
        public int id { get; set; }        
        public int staffid { get; set; }        
        public int staffteamid { get; set; }
        public bool isdeleted { get; set; }
    }

    public class StaffTagsModel
    {
        public int Id { get; set; }
        public int StaffID { get; set; }
        public int TagID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
