using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientImmunizationService :IBaseService
    {
        JsonModel SavePatientImmunization(PatientImmunizationModel patientImmunizationModel, TokenModel tokenModel);
        JsonModel GetImmunization(int patientId, TokenModel tokenModel);
        JsonModel GetImmunizationById(int id, TokenModel tokenModel);
        JsonModel DeleteImmunization(int id, TokenModel tokenModel);
    }
}
