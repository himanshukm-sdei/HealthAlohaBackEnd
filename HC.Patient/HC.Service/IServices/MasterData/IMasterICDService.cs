using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface IMasterICDService : IBaseService
    {
        JsonModel GetMasterICDCodes(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SaveMasterICDCodes(MasterICDModel masterICDModel, TokenModel tokenModel);
        JsonModel GetMasterICDCodesById(int id, TokenModel tokenModel);
        JsonModel DeleteMasterICDCodes(int id, TokenModel tokenModel);
    }
}
