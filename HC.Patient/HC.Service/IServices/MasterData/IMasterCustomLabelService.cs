using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface IMasterCustomLabelService:IBaseService
    {
        JsonModel GetCustomLabel(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SaveCustomLabel(MasterCustomLabelModel masterCustomLabelModel, TokenModel tokenModel);
        JsonModel GetCustomLabelById(int id, TokenModel tokenModel);
        JsonModel DeleteCustomLabel(int id, TokenModel tokenModel);
    }
}
