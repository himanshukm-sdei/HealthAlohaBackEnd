using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface ILocationService : IBaseService
    {
        JsonModel GetLocations(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SaveLocation(LocationModel locationModel, TokenModel tokenModel);
        JsonModel GetLocationById(int id, TokenModel tokenModel);
        JsonModel DeleteLocation(int id, TokenModel tokenModel);
        JsonModel GetMinMaxOfficeTime(string locationIds, TokenModel tokenModel);
    }
}
