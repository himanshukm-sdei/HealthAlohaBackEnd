using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientsInsuranceService :IBaseService
    {
        JsonModel SavePatientInsurance(List<PatientInsuranceModel> patientInsuranceListModel, TokenModel tokenModel);
        JsonModel GetPatientInsurances(int patientId, TokenModel tokenModel);
    }
}
