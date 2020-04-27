using HC.Model;
using HC.Patient.Model.Payer;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.Payer
{
    public interface IPayerService : IBaseService
    {
        JsonModel GetPayerList(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SavePayerData(InsuranceCompanyModel insuranceCompanyModel, TokenModel tokenModel);
        JsonModel GetPayerDataById(int id, TokenModel tokenModel);
        JsonModel DeletePayerData(int id, TokenModel tokenModel);
    }
}
