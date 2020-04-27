using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System;
using System.Net;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientSocialHistoryService : BaseService, IPatientSocialHistoryService
    {
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private readonly IPatientSocialHistoryRepository _patientSocialHistoryRepository;

        public PatientSocialHistoryService(IPatientSocialHistoryRepository patientSocialHistoryRepository)
        {
            _patientSocialHistoryRepository = patientSocialHistoryRepository;
        }

        public JsonModel SavePatientSocialHistory(PatientSocialHistoryModel patientSocialHistoryModel, TokenModel tokenModel)
        {
            PatientSocialHistory patientSocialHistory = null;
            if (patientSocialHistoryModel.Id == 0)
            {
                patientSocialHistory = new PatientSocialHistory();
                AutoMapper.Mapper.Map(patientSocialHistoryModel, patientSocialHistory);
                patientSocialHistory.CreatedDate = DateTime.UtcNow;
                patientSocialHistory.CreatedBy = tokenModel.UserID;
                patientSocialHistory.IsActive = true;
                patientSocialHistory.IsDeleted = false;
                _patientSocialHistoryRepository.Create(patientSocialHistory);
                _patientSocialHistoryRepository.SaveChanges();
                response = new JsonModel(patientSocialHistory, StatusMessage.ClientSocialHistoryCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                patientSocialHistory = _patientSocialHistoryRepository.Get(a => a.Id == patientSocialHistoryModel.Id && a.IsActive == true && a.IsDeleted == false);
                AutoMapper.Mapper.Map(patientSocialHistoryModel, patientSocialHistory);
                patientSocialHistory.UpdatedDate = DateTime.UtcNow;
                patientSocialHistory.UpdatedBy = tokenModel.UserID;
                _patientSocialHistoryRepository.Update(patientSocialHistory);
                _patientSocialHistoryRepository.SaveChanges();
                response = new JsonModel(patientSocialHistory, StatusMessage.ClientSocialHistoryUpdated, (int)HttpStatusCode.OK);
            }
            return response;
        }

        public JsonModel GetPatientSocialHistory(int patientId, TokenModel tokenModel)
        {
            PatientSocialHistory patientSocialHistory = _patientSocialHistoryRepository.Get(a => a.PatientID == patientId && a.IsActive == true && a.IsDeleted == false);
            if (patientSocialHistory != null)
            {
                PatientSocialHistoryModel patientSocialHistoryModel = new PatientSocialHistoryModel();
                AutoMapper.Mapper.Map(patientSocialHistory, patientSocialHistoryModel);
                response = new JsonModel(patientSocialHistoryModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }
    }
}
