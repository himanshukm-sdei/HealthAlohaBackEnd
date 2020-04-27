using HC.Patient.Model.PatientAppointment;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.PatientEncounters
{
    public class PatientEncounterModel
    {
        public int? Id { get; set; }
        public int? PatientID { get; set; }
        public int? PatientAppointmentId { get; set; }
        public int? ParentAppointmentId { get; set; }
        public DateTime DateOfService { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime AppointmentStartDateTime { get; set; }
        public DateTime AppointmentEndDateTime { get; set; }
        public int? StaffID { get; set; }
        public int? PatientAddressID { get; set; }
        public int? OfficeAddressID { get; set; }
        public int? ServiceLocationID { get; set; }
        public int? CustomAddressID { get; set; }
        public string CustomAddress { get; set; }
        //public Byte[] PatientSign { get; set; }
        //public DateTime? PatientSignDate { get; set; }
        //public Byte[] ClinicianSign { get; set; }
        //public DateTime? ClinicianSignDate { get; set; }
        //public Byte[] GuardianSign { get; set; }
        //public DateTime? GuardianSignDate { get; set; }
        //public string GuardianName { get; set; }
        public int? NotetypeId { get; set; }
        public String StaffName { get; set; }
        public String PatientName { get; set; }
        public String LocationName { get; set; }
        public string Duration { get; set; }
        public string AppointmentType { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public decimal TotalRecords { get; set; }

        public int UnitsBlocked { get; set; }
        public int UnitsConsumed { get; set; }
        public string ServiceFacility { get; set; }
        public bool IsDirectService { get; set; }
        public string NonBillableNotes { get; set; }
        public bool? IsBillableEncounter { get; set; }        
        #region add model in this region for stored Procedure
        public SOAPNotesModel SOAPNotes { get; set; }
        public List<PatientEncounterServiceCodesModel> PatientEncounterServiceCodes { get; set; }
        public List<PatientEncounterDiagnosisCodesModel> PatientEncounterDiagnosisCodes { get; set; }        
        public List<EncounterSignatureModel> EncounterSignature { get; set; }
        #endregion
        public PatientAppointmentModel PatientAppointment { get; set; }
        public List<PatientEncounterCodesMappingModel> PatientEncounterCodesMapping { get; set; }
    }
    public class SOAPNotesModel
    {
        public int Id { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public string Plans { get; set; }
    }
    public class PatientEncounterServiceCodesModel
    {
        public int Id { get; set; }
        public int ServiceCodeId { get; set; }
        public string ServiceCode { get; set; }
        public string Description { get; set; }
        public int? Modifier1 { get; set; }
        public int? Modifier2 { get; set; }
        public int? Modifier3 { get; set; }
        public int? Modifier4 { get; set; }
        public bool IsDeleted { get; set; }
        public string AuthorizationNumber { get; set; }
        public Nullable<int> AuthProcedureCPTLinkId { get; set; }
        public bool IsValid { get; set; }
        public bool IsRequiredAuthorization { get; set; }
        public string AuthorizationMessage { get; set; }

        public bool IsAuthorizationMandatory { get; set; }

        public string AttachedModifiers { get; set; }
    }
    public class PatientEncounterDiagnosisCodesModel
    {
        public int Id { get; set; }

        public int ICDCodeId { get; set; }
        public string ICDCode { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DiagnosisDate { get; set; }
    }
    public class PatientEncounterCodesMappingModel
    {
        public int Id { get; set; }
        public string ServiceCode { get; set; }
        public string ICDCode { get; set; }
        public bool IsMapped { get; set; }
        public bool IsDeleted { get; set; }
    }
}
