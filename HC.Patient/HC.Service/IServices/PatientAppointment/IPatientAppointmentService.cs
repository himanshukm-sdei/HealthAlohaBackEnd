using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.PatientAppointment;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.PatientAppointment
{
    public interface IPatientAppointmentService :IBaseService
    {
        List<PatientAppointmentsModel> UpdatePatientAppointment(PatientAppointmentFilter patientAppointmentFilter);
        List<StaffAvailabilityModel> GetStaffAvailability(string StaffID, DateTime FromDate, DateTime ToDate,TokenModel token);
        JsonModel  GetPatientAppointmentList(string locationIds, string staffIds, string patientIds, DateTime? fromDate, DateTime? toDate,string patientTags,string staffTags, TokenModel token);
        JsonModel SaveAppointment(PatientAppointmentModel patientAppointmentModel,List<PatientAppointmentModel> patientAppointmentList, bool isAdmin, TokenModel tokenModel);

        JsonModel DeleteAppointment(int appointmentId, int? parentAppointmentId,bool deleteSeries, bool isAdmin, TokenModel token);

        JsonModel GetAppointmentDetails(int appointmentId,TokenModel token);
        JsonModel GetStaffAndPatientByLocation(string locationIds, string permissionKey, string isActiveCheckRequired, TokenModel token);
        JsonModel GetStaffByLocation(string locationIds, string isActiveCheckRequired, TokenModel token);
        List<AvailabilityMessageModel> CheckIsValidAppointment(string staffIds, DateTime startDate, DateTime endDate,Nullable<DateTime> currentDate, Nullable<int> patientAppointmentId,Nullable<int> patientId, Nullable<int> appointmentTypeID, TokenModel token);
        List<AvailabilityMessageModel> CheckIsValidAppointmentWithLocation(string staffIds, DateTime startDate, DateTime endDate, Nullable<DateTime> currentDate, Nullable<int> patientAppointmentId, Nullable<int> patientId, Nullable<int> appointmentTypeID, decimal currentOffset, TokenModel token);
        JsonModel GetDataForSchedulerByPatient(Nullable<int> patientId,int locationId, DateTime startDate, DateTime endDate,Nullable<int> patientInsuranceId,TokenModel token);
        JsonModel UpdateServiceCodeBlockedUnit(int authProcedureCPTLinkId, string action,TokenModel token);
        JsonModel CancelAppointments(int[] appointmentIds, int CancelTypeId, string reson, TokenModel token);
        JsonModel ActivateAppointments(int appointmentId, bool isAdmin, TokenModel token);
        JsonModel UpdateAppointmentStatus(AppointmentStatusModel appointmentStatusModel, TokenModel token);
        JsonModel SaveAppointmentFromPatientPortal(PatientAppointmentModel patientAppointmentModel,TokenModel token);
        JsonModel DeleteAppointment(int appointmentId,TokenModel token);
        LocationModel GetLocationOffsets(int? locationId);
    }
}
