using HC.Model;
using HC.Patient.Model.Common;
using HC.Patient.Model.Staff;
using HC.Patient.Model.Users;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.User
{
    public interface IUserService : IBaseService
    {
        List<StaffModels> GetFilteredStaff(string @LocationIds, string RoleIds, string SearchKey, string StartWith, string Tags, string sortColumn, string sortOrder, int pageNumber, int pageSize, TokenModel tokenModel);
        JsonModel UpdateUserPassword(UpdatePasswordModel updatePassword, TokenModel token);
        JsonModel UpdateUserStatus(int userId, bool status, TokenModel token);
        JsonModel UploadUserDocuments(UserDocumentsModel userDocuments, TokenModel token);
        JsonModel GetUserDocuments(int userID, DateTime? from, DateTime? to, TokenModel token);
        UserDocumentsResponseModel GetUserDocument(int id, TokenModel token);
        JsonModel DeleteUserDocument(int id, TokenModel token);
        JsonModel Logout(TokenModel token);
        bool SetOnline(int userId);
        PasswordCheckModel CheckUserPasswordStatus(TokenModel token);
        JsonModel UpdateExpiredPassword(UpdatePasswordModel updatePassword, TokenModel token);
    }
}
