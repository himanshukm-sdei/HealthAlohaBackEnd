using System;

namespace HC.Patient.Model.Patient
{
    public class PatientModel
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MRN { get; set; }
        public string PhotoThumbnailPath { get; set; }
        public bool IsActive { get; set; }
        public DateTime DOB { get; set; }
        public bool IsBlock { get; set; }
    }
}
