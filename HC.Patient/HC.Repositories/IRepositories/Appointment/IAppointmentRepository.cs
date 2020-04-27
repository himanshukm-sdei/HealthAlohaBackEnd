using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.PatientAppointment;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Appointment
{
    public interface IAppointmentRepository :IRepositoryBase<PatientAppointment>
    {
        IQueryable<T> GetAppointmentList<T>(string locationIds, string staffIds, string patientIds, DateTime? fromDate, DateTime? toDate, string patientTags, string staffTags,int organizationId) where T : class, new();
        IQueryable<T> GetAppointmentDetails<T>(int appointmentId) where T : class, new();
        IQueryable<T> CheckIsValidAppointment<T>(string staffIds, DateTime startDate, DateTime endDate, Nullable<DateTime> currentDate,Nullable<int> patientAppointmentId,Nullable<int> patientId, Nullable<int> appointmentTypeID, TokenModel token) where T : class, new();
        IQueryable<T> CheckIsValidAppointmentWithLocation<T>(string staffIds, DateTime startDate, DateTime endDate, Nullable<DateTime> currentDate, Nullable<int> patientAppointmentId, Nullable<int> patientId, Nullable<int> appointmentTypeID, decimal currentOffset, TokenModel token) where T : class, new();
        
        IQueryable<T> DeleteAppointment<T>(int appointmentId, bool isAdmin, bool deleteSeries, TokenModel token) where T : class, new();
    }

    public interface IPatientAppointmentRepository//:IRepositoryBase<PatientAppointment>
    {
        List<PatientAppointmentsModel> UpdatePatientAppointment(PatientAppointmentFilter patientAppointmentFilter);
        List<StaffAvailabilityModel> GetStaffAvailability(string StaffID, DateTime FromDate, DateTime ToDate);
        StaffPatientModel GetStaffAndPatientByLocation(string locationIds,string permissionKey, string isActiveCheckRequired, TokenModel token);        
        IQueryable<T> GetStaffByLocation<T>(string locationIds, string isActiveCheckRequired, TokenModel token) where T : class, new();
    }
}
