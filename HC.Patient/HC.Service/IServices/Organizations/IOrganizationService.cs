using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Organizations;
using HC.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.Organizations
{
    public interface IOrganizationService : IBaseService
    {
        JsonModel SaveOrganization(OrganizationModel organizationModel, TokenModel token, IHttpContextAccessor contextAccessor);
        JsonModel GetOrganizationById(int Id, IHttpContextAccessor contextAccessor);
        JsonModel GetOrganizations(string businessName = "",string orgName = "", string country = "", string sortOrder = "", string sortColumn="", int page = 1, int pageSize = 10);
        JsonModel DeleteOrganization(int Id, TokenModel token, IHttpContextAccessor contextAccessor);
        JsonModel CheckOrganizationBusinessName(string BusinessName);
        JsonModel GetOrganizationDatabaseDetails(string databaseName, string organizationName, int organizationID, string sortColumn, string sortOrder, int pageNumber, int pageSize);
        JsonModel SaveOrganizationDatabaseDetail(OrganizationDatabaseDetail organizationDatabaseDetail);
        JsonModel UpdateOrganizationDatabaseDetail(int id, OrganizationDatabaseDetail organizationDatabaseDetail);
        JsonModel DeleteOrganizationDatabaseDetail(int id, int userID);
        JsonModel GetOrganizationDetailsById(TokenModel token);
    }
}
