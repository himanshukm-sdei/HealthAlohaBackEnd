using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientDiagnosisService:IBaseService
    {
        JsonModel SavePatientDiagnosis(PatientDiagnosisModel patientDiagnosisModel, TokenModel tokenModel);
        JsonModel GetDiagnosis(int patientId, TokenModel tokenModel);
        JsonModel GetDiagnosisById(int id, TokenModel tokenModel);
        JsonModel DeleteDiagnosis(int id, TokenModel tokenModel);
    }
}
