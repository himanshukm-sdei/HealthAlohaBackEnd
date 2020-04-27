using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientDiagnosisService :BaseService, IPatientDiagnosisService
    {
        private readonly IPatientDiagnosisRepository _patientDiagnosisRepository;
        JsonModel response = new JsonModel(new object(),StatusMessage.NotFound,(int)HttpStatusCode.NotFound);
        public PatientDiagnosisService(IPatientDiagnosisRepository patientDiagnosisRepository)
        {
            _patientDiagnosisRepository = patientDiagnosisRepository;
        }

        public JsonModel SavePatientDiagnosis(PatientDiagnosisModel patientDiagnosisModel,TokenModel tokenModel)
        {
            PatientDiagnosis patientDiagnosis = null;
            if(patientDiagnosisModel.Id == 0)
            {
                patientDiagnosis = new PatientDiagnosis();
                AutoMapper.Mapper.Map(patientDiagnosisModel, patientDiagnosis);
                patientDiagnosis.CreatedBy = tokenModel.UserID;
                patientDiagnosis.CreatedDate = DateTime.UtcNow;                
                patientDiagnosis.IsDeleted = false;
                _patientDiagnosisRepository.Create(patientDiagnosis);
                _patientDiagnosisRepository.SaveChanges();
                response = new JsonModel(patientDiagnosis, StatusMessage.ClientDiagnosisCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                patientDiagnosis = _patientDiagnosisRepository.Get(a => a.Id == patientDiagnosisModel.Id && a.IsDeleted == false);
                if (patientDiagnosis != null)
                {
                    AutoMapper.Mapper.Map(patientDiagnosisModel, patientDiagnosis);
                    patientDiagnosis.UpdatedBy = tokenModel.UserID;
                    patientDiagnosis.UpdatedDate = DateTime.UtcNow;
                    _patientDiagnosisRepository.Update(patientDiagnosis);
                    _patientDiagnosisRepository.SaveChanges();
                    response = new JsonModel(patientDiagnosis, StatusMessage.ClientDiagnosisUpdated, (int)HttpStatusCode.OK);
                }
            }
            return response;
        }

        public JsonModel GetDiagnosis(int patientId, TokenModel tokenModel)
        {
            List<PatientDiagnosisModel> patientDiagnosisModel = _patientDiagnosisRepository.GetDiagnosis<PatientDiagnosisModel>(patientId, tokenModel).ToList();
            if(patientDiagnosisModel!=null && patientDiagnosisModel.Count > 0)
            {
                response = new JsonModel(patientDiagnosisModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel GetDiagnosisById(int id, TokenModel tokenModel)
        {
            PatientDiagnosis patientDiagnosis = _patientDiagnosisRepository.Get(a=>a.Id==id && a.IsDeleted==false);
            if (patientDiagnosis != null)
            {
                PatientDiagnosisModel patientDiagnosisModel = new PatientDiagnosisModel();
                AutoMapper.Mapper.Map(patientDiagnosis, patientDiagnosisModel);
                response = new JsonModel(patientDiagnosisModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteDiagnosis(int id, TokenModel tokenModel)
        {
            PatientDiagnosis patientDiagnosis = _patientDiagnosisRepository.Get(a => a.Id == id && a.IsDeleted == false);
            if (patientDiagnosis != null)
            {
                patientDiagnosis.IsDeleted = true;
                patientDiagnosis.DeletedBy = tokenModel.UserID;
                patientDiagnosis.DeletedDate = DateTime.UtcNow;
                _patientDiagnosisRepository.Update(patientDiagnosis);
                _patientDiagnosisRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.ClientDiagnosisDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
    }
}
