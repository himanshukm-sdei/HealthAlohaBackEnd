using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientAppointment
{
    public class StaffPatientModel
    {
        public List<StaffModel> Staff { get; set; }
        public List<PatModel> Patients { get; set; }
    }
    public class StaffModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Value { get; set; }
        public int UserId { get; set; }
    }
    public class PatModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Value { get; set; }
        public int UserId { get; set; }

    }
}
