using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Payer;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Patient.Service.IServices.Payer;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Payer
{
    public class PayerService : BaseService, IPayerService
    {
        private readonly IPayerRepository _payerRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response=null;
        public PayerService(IPayerRepository payerRepository, IUserCommonRepository userCommonRepository)
        {
            _payerRepository = payerRepository;
            _userCommonRepository = userCommonRepository;
            response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
        }
        public JsonModel DeletePayerData(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.DatabaseEntityName("MasterICD"), false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
                response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            else
            {
                InsuranceCompanies insuranceCompanies = _payerRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
                if (insuranceCompanies != null)
                {
                    insuranceCompanies.IsDeleted = true;
                    insuranceCompanies.DeletedBy = tokenModel.UserID;
                    insuranceCompanies.DeletedDate = DateTime.UtcNow;
                    _payerRepository.Update(insuranceCompanies);
                    _payerRepository.SaveChanges();
                    response = new JsonModel(new object(), StatusMessage.PayerDeleted, (int)HttpStatusCodes.OK);
                }
                else
                {
                    response = new JsonModel(null, StatusMessage.PayerDoesNotExist, (int)HttpStatusCode.BadRequest);
                }
            }
            return response;
        }

        public JsonModel GetPayerDataById(int id, TokenModel tokenModel)
        {
            InsuranceCompanies insuranceCompanies = _payerRepository.GetByID(id);
            if (insuranceCompanies != null)
            {
                InsuranceCompanyModel insuranceCompanyModel = new InsuranceCompanyModel();
                AutoMapper.Mapper.Map(insuranceCompanies, insuranceCompanyModel);
                response = new JsonModel(insuranceCompanyModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            }
            return response;
        }

        public JsonModel GetPayerList(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<InsuranceCompanyModel> insuranceCompanyModel = _payerRepository.GetPayerList<InsuranceCompanyModel>(searchFilterModel, tokenModel).ToList();
            if (insuranceCompanyModel != null && insuranceCompanyModel.Count > 0)
            {
                response = new JsonModel(insuranceCompanyModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(insuranceCompanyModel, searchFilterModel);
            }
            return response;
        }

        public JsonModel SavePayerData(InsuranceCompanyModel insuranceCompanyModel, TokenModel tokenModel)
        {
            InsuranceCompanies insuranceCompanies = null;
            if (insuranceCompanyModel.Id == 0)
            {
                insuranceCompanies = new InsuranceCompanies();
                AutoMapper.Mapper.Map(insuranceCompanyModel, insuranceCompanies);
                insuranceCompanies.CreatedBy = tokenModel.UserID;
                insuranceCompanies.CreatedDate = DateTime.UtcNow;
                insuranceCompanies.IsDeleted = false;
                insuranceCompanies.IsActive = true;
                insuranceCompanies.OrganizationID = tokenModel.OrganizationID;
                _payerRepository.Create(insuranceCompanies);
                _payerRepository.SaveChanges();
                response = new JsonModel(insuranceCompanies, StatusMessage.PayerCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                insuranceCompanies = _payerRepository.Get(a => a.Id == insuranceCompanyModel.Id && a.IsActive == true && a.IsDeleted == false);
                if (insuranceCompanies != null)
                {
                    AutoMapper.Mapper.Map(insuranceCompanyModel, insuranceCompanies);
                    insuranceCompanies.UpdatedBy = tokenModel.UserID;
                    insuranceCompanies.UpdatedDate = DateTime.UtcNow;
                    _payerRepository.Update(insuranceCompanies);
                    _payerRepository.SaveChanges();
                    response = new JsonModel(insuranceCompanies, StatusMessage.PayerUpdated, (int)HttpStatusCode.OK);
                }
                else
                {
                    response = new JsonModel(null, StatusMessage.PayerDoesNotExist, (int)HttpStatusCode.BadRequest);
                }
            }
            return response;
        }
    }
}
