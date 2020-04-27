using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface IMasterTemplatesService: IBaseService
    {
        JsonModel GetMasterTemplates(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SaveMasterTemplate(MasterTemplatesModel masterTemplatesModel, TokenModel tokenModel);
        JsonModel GetMasterTemplateById(int id, TokenModel tokenModel);
        JsonModel DeleteMasterTemplate(int id, TokenModel tokenModel);
    }
}
