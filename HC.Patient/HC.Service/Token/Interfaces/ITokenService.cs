using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Model.Common;
using HC.Patient.Model.Organizations;
using HC.Patient.Model.SecurityQuestion;
using HC.Patient.Model.Staff;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.Token.Interfaces
{
    public interface ITokenService : IBaseService
    {
        User GetUserByUserName(string userName, int organizationID);
        Staffs GetDoctorByUserID(int UserID, TokenModel token);
        int GetDefaultLocationOfStaff(int UserID);
        List<UserLocationsModel> GetUserLocations(int UserID);
        Patients GetPaitentByUserID(int UserID, TokenModel tokenModel);
        SuperUser GetSupadminUserByUserName(string userName);
        List<AppConfigurationsModel> GetAppConfigurationByOrganizationByID(TokenModel tokenModel);
        DomainToken GetDomain(DomainToken domainToken);
        JsonModel AuthenticateSuperUser(ApplicationUser applicationUser, TokenModel token);
        //JsonModel AuthenticateAgency(ApplicationUser applicationUser, TokenModel token);
        //JsonModel SaveUserScurityQuestion(SecurityQuestionListModel questionListModel, TokenModel token);
        //JsonModel CheckQuestionAnswer(SecurityQuestionModel securityQuestion, TokenModel token);
        int GetOrganizationIDByName(string businessName);
        OrganizationModel GetOrganizationById(int id, TokenModel token);
        OrganizationModel GetOrganizationDetailsByBusinessName(string businessName);
        NotificationModel GetLoginNotification(TokenModel tokenModel);
        Patients GetLastPatientByOrganization(TokenModel tokenModel);
        bool GetDefaultClient(int UserID);
    }
}
