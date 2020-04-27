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
    public class PatientImmunizationService : BaseService, IPatientImmunizationService
    {
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private readonly IPatientImmunizationRepository _patientImmunizationRepository;
        public PatientImmunizationService(IPatientImmunizationRepository patientImmunizationRepository)
        {
            _patientImmunizationRepository = patientImmunizationRepository;
        }

        public JsonModel SavePatientImmunization(PatientImmunizationModel patientImmunizationModel, TokenModel tokenModel)
        {
            PatientImmunization patientImmunization = null;
            if (patientImmunizationModel.Id == 0)
            {
                patientImmunization = new PatientImmunization();
                AutoMapper.Mapper.Map(patientImmunizationModel, patientImmunization);
                patientImmunization.CreatedBy = tokenModel.UserID;
                patientImmunization.CreatedDate = DateTime.UtcNow;
                patientImmunization.IsActive = true;
                patientImmunization.IsDeleted = false;
                _patientImmunizationRepository.Create(patientImmunization);
                _patientImmunizationRepository.SaveChanges();
                response = new JsonModel(patientImmunization, StatusMessage.ClientImmunizationCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                patientImmunization = _patientImmunizationRepository.Get(a => a.Id == patientImmunizationModel.Id && a.IsActive == true && a.IsDeleted == false);
                if (patientImmunization != null)
                {
                    AutoMapper.Mapper.Map(patientImmunizationModel, patientImmunization);
                    patientImmunization.UpdatedBy = tokenModel.UserID;
                    patientImmunization.UpdatedDate = DateTime.UtcNow;
                    _patientImmunizationRepository.Update(patientImmunization);
                    _patientImmunizationRepository.SaveChanges();
                    response = new JsonModel(patientImmunization, StatusMessage.ClientImmunizationUpdated, (int)HttpStatusCode.OK);
                }
            }
            return response;
        }

        public JsonModel GetImmunization(int patientId, TokenModel tokenModel)
        {
            List<PatientImmunizationModel> patientImmunizationModel = _patientImmunizationRepository.GetImmunization<PatientImmunizationModel>(patientId, tokenModel).ToList();
            if (patientImmunizationModel != null && patientImmunizationModel.Count > 0)
            {
                response = new JsonModel(patientImmunizationModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel GetImmunizationById(int id, TokenModel tokenModel)
        {
            PatientImmunization patientImmunization = _patientImmunizationRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
            if (patientImmunization != null )
            {
                PatientImmunizationModel patientImmunizationModel = new PatientImmunizationModel();
                AutoMapper.Mapper.Map(patientImmunization, patientImmunizationModel);
                response = new JsonModel(patientImmunizationModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteImmunization(int id,TokenModel tokenModel)
        {
            PatientImmunization patientImmunization = _patientImmunizationRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
            if (patientImmunization != null)
            {
                patientImmunization.IsDeleted = true;
                patientImmunization.DeletedBy = tokenModel.UserID;
                patientImmunization.DeletedDate = DateTime.UtcNow;
                _patientImmunizationRepository.Update(patientImmunization);
                _patientImmunizationRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.ClientImmunizationDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
    }
}
