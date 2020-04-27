using HC.Model;
using HC.Patient.Model.Staff;
using HC.Patient.Model.Users;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.User
{
    public interface IUserRoleService : IBaseService
    {
        JsonModel GetRoles(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SaveRole(UserRoleModel userRoleModel, TokenModel tokenModel);
        JsonModel GetRoleById(int id, TokenModel tokenModel);
        JsonModel DeleteRole(int id, TokenModel tokenModel);
    }
}
