using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientService : IBaseService
    {
        List<PatientModel> GetPatientsByTags(string tags, string startWith, int? locationID, bool? isActive);
        JsonModel GetPatientByTags(ListingFiltterModel patientFiltterModel,TokenModel token);
        JsonModel GetPatientsDetails(int PatientID,TokenModel token);
        JsonModel GetPatientById(int patientId, TokenModel token);
        JsonModel UpdatePatientPortalVisibility(int patientID, int userID, bool isPortalActive, TokenModel token);
        JsonModel UpdatePatientActiveStatus(int patientID, bool isActive, TokenModel token);
        JsonModel GetActivitiesForPatientPayer(int patientId, DateTime startDate, DateTime endDate, TokenModel token);
        JsonModel GetPatientAuthorizationData(Nullable<int> appointmentId,int patientId,int appointmentTypeId,DateTime startDate,DateTime endDate, bool isAdmin,Nullable<int> patientInsuranceId,Nullable<int> authorizationId, TokenModel token);
        AuthorizationValidityModel CheckAuthorizationForServiceCodes(List<string> serviceCodesList, int patientId, int appointentTypeId, DateTime startDate, string payerPreference);
        JsonModel GetPatientPayerServiceCodes(int patientId,string payerPreference,DateTime date,int payerId, int patientInsuranceId, TokenModel token);
        JsonModel GetPatientPayerServiceCodesAndModifiers(int patientId, string payerPreference, DateTime date, int payerId, int patientInsuranceId, TokenModel token);
                
        JsonModel GetAllAuthorizationsForPatient(int patientId, int pageNumber,int pageSize,string authType, TokenModel token);
        MemoryStream GetPatientCCDA(int patientID, TokenModel token);
        JsonModel GetPatientGuarantor(int patientId, TokenModel token);
        JsonModel ImportPatientCCDA(JObject file, TokenModel token);
        JsonModel CreateUpdatePatient(PatientDemographicsModel patients, TokenModel token);
        JsonModel GetPatients(ListingFiltterModel patientFiltterModel,TokenModel token);
        JsonModel GetAuthorizationsForPatientPayer(int patientId, int patientInsuranceId, DateTime startDate, TokenModel token);
        JsonModel GetPatientHeaderInfo(int PatientID, TokenModel token);
        
    }
}
