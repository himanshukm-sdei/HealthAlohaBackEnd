using HC.Model;
using HC.Patient.Model.PatientEncounters;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HC.Patient.Service.IServices.PatientEncounters
{
    public interface IPatientEncounterService : IBaseService
    {
        JsonModel GetPatientEncounterDetails(int appointmentId,int encounterId,bool isAdmin,TokenModel token);
        JsonModel GetPatientNonBillableEncounterDetails(int appointmentId,int encounterId,bool isAdmin,TokenModel token);
        JsonModel SavePatientEncounter(PatientEncounterModel patientEncounter,bool isAdmin, TokenModel token);
        JsonModel SavePatientNonBillableEncounter(PatientEncounterModel patientEncounter, bool isAdmin, TokenModel token);

        List<PatientEncounterModel> GetPatientEncounter(int? patientID, string appointmentType = "", string staffName = "", string status = "", string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "", TokenModel token= null);
        JsonModel SavePatientSignForPatientEncounter(int patientEncounterId, PatientEncounterModel patientEncounterModel);
        MemoryStream DownloadEncounter(int encounterId, TokenModel token);
        JsonModel SaveEncounterSignature(EncounterSignatureModel encounterSignatureModel);
        JsonModel SavePatientEncounterTemplateData(PatientEncounterTemplateModel patientEncounterTemplateModel, TokenModel token);
        JsonModel GetPatientEncounterTemplateData(int patientEncounterId, int masterTemplateId, TokenModel tokenModel);
    }
}
