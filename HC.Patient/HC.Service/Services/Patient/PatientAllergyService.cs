using CreateClinicalReport.Model;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientAllergyService : BaseService, IPatientAllergyService
    {
        JsonModel response = new JsonModel(new object(),StatusMessage.NotFound,(int)HttpStatusCode.NotFound);
        private readonly IPatientAllergyRepository _patientAllergyRepository;
        public PatientAllergyService(IPatientAllergyRepository patientAllergyRepository)
        {
            _patientAllergyRepository = patientAllergyRepository;
        }

        public JsonModel GetAllergies(PatientFilterModel patientFilterModel,TokenModel tokenModel)
        {
            List<PatientAllergyModel> patientAllergies = _patientAllergyRepository.GetPatientAllergies<PatientAllergyModel>(patientFilterModel, tokenModel).ToList();
            if(patientAllergies!=null && patientAllergies.Count > 0)
            {
                response = new JsonModel(patientAllergies, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(patientAllergies, patientFilterModel);
            }
            return response;
        }

        public JsonModel SaveAllergy(PatientAllergyModel patientAllergyModel, TokenModel tokenModel)
        {
            Entity.PatientAllergies patientAllergies = null;

            if (patientAllergyModel.Id == 0)
            {
                patientAllergies = _patientAllergyRepository.Get(l => l.AllergyTypeID == patientAllergyModel.AllergyTypeId && l.PatientID == patientAllergyModel.PatientId && l.IsDeleted == false);

                if (patientAllergies != null)
                {
                    response = new JsonModel(new object(), StatusMessage.ClientAllergyAlreadyLink, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else
                {
                    patientAllergies = new Entity.PatientAllergies();
                    AutoMapper.Mapper.Map(patientAllergyModel, patientAllergies);
                    patientAllergies.CreatedBy = tokenModel.UserID;
                    patientAllergies.IsDeleted = false;
                    _patientAllergyRepository.Create(patientAllergies);
                    _patientAllergyRepository.SaveChanges();
                    response = new JsonModel(patientAllergies, StatusMessage.AllergySave, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                patientAllergies = _patientAllergyRepository.Get(l => l.AllergyTypeID == patientAllergyModel.AllergyTypeId && l.Id != patientAllergyModel.Id && l.PatientID == patientAllergyModel.PatientId && l.IsDeleted == false);
                if (patientAllergies != null) { response = new JsonModel(new object(), StatusMessage.ClientAllergyAlreadyLink, (int)HttpStatusCodes.UnprocessedEntity); }
                else
                {
                    patientAllergies = _patientAllergyRepository.Get(a => a.Id == patientAllergyModel.Id && a.IsDeleted == false);
                    if (patientAllergies != null)
                    {
                        AutoMapper.Mapper.Map(patientAllergyModel, patientAllergies);
                        patientAllergies.UpdatedBy = tokenModel.UserID;
                        patientAllergies.UpdatedDate = DateTime.UtcNow;
                        _patientAllergyRepository.Update(patientAllergies);
                        _patientAllergyRepository.SaveChanges();
                        response = new JsonModel(patientAllergies, StatusMessage.AllergyUpdated, (int)HttpStatusCode.OK);
                    }
                }
            }
            return response;
        }

        public JsonModel GetAllergyById(int id, TokenModel tokenModel)
        {
            Entity.PatientAllergies patientAllergies = _patientAllergyRepository.Get(a => a.Id == id && a.IsDeleted == false);
            if (patientAllergies != null)
            {
                PatientAllergyModel patientAllergyModel = new PatientAllergyModel();
                AutoMapper.Mapper.Map(patientAllergies, patientAllergyModel);
                response = new JsonModel(patientAllergyModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteAllergy(int id, TokenModel tokenModel)
        {
            Entity.PatientAllergies patientAllergy = _patientAllergyRepository.Get(a => a.Id == id && a.IsDeleted == false);
            if (patientAllergy != null)
            {
                patientAllergy.IsDeleted = true;
                patientAllergy.DeletedBy = tokenModel.UserID;
                patientAllergy.DeletedDate = DateTime.UtcNow;
                _patientAllergyRepository.Update(patientAllergy);
                _patientAllergyRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.AllergyDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
    }
}
