using HC.Model;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.AdminDashboard
{
    public interface IAdminDashboardService : IBaseService
    {
        JsonModel GetTotalClientCount(TokenModel token);
        JsonModel GetTotalRevenue(TokenModel token);
        JsonModel GetOrganizationAuthorization(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token);
        JsonModel GetOrganizationEncounter(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token);
        JsonModel GetStaffEncounter(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token);
        JsonModel GetRegiesteredClientCount(TokenModel token);
        JsonModel GetAdminDashboardData(TokenModel token);


        #region Angular Code
        JsonModel GetClientStatusChart(TokenModel token);
        JsonModel GetDashboardBasicInfo(TokenModel token);
        #endregion

    }
}
