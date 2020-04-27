using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientDiagnosisModel
    {
        public int Id { get; set; }
        public int ICDID { get; set; }
        public int PatientID { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime? ResolveDate { get; set; }
        public bool IsDeleted { get; set; }
        //
        public string Diagnosis { get; set; }
    }
}
