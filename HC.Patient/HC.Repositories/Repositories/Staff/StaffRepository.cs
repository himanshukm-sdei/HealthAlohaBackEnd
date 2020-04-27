using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Staff;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Staff
{
    public class StaffRepository : RepositoryBase<Staffs>, IStaffRepository
    {
        private HCOrganizationContext _context;
        public StaffRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
        public IQueryable<T> GetStaffByTags<T>(ListingFiltterModel listingFiltterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@Tags", listingFiltterModel.Tags),
                                          new SqlParameter("@StartWith",listingFiltterModel.StartWith),
                                          new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@LocationIDs",listingFiltterModel.LocationIDs),
                                          new SqlParameter("@IsActive",listingFiltterModel.IsActive),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetStaffByTags.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetStaff<T>(ListingFiltterModel listingFiltterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@LocationIds",listingFiltterModel.LocationIDs),
                                          new SqlParameter("@RoleIds",listingFiltterModel.RoleIds),
                                          new SqlParameter("@SearchKey",listingFiltterModel.SearchKey),
                                          new SqlParameter("@StartWith",listingFiltterModel.StartWith),
                                          new SqlParameter("@Tags",listingFiltterModel.Tags),
                                          new SqlParameter("@SortColumn",listingFiltterModel.sortColumn),
                                          new SqlParameter("@SortOrder",listingFiltterModel.sortOrder),
                                          new SqlParameter("@PageNumber",listingFiltterModel.pageNumber),
                                          new SqlParameter("@PageSize",listingFiltterModel.pageSize)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetStaffUsers.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public StaffProfileModel GetStaffProfileData(int staffId, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@StaffId", staffId),
                                          new SqlParameter("@OrganizationId",token.OrganizationID)
            };
            return _context.ExecStoredProcedureForStaffProfileData(SQLObjects.STF_GetProfileData, parameters.Length, parameters);
        }

        public IQueryable<T> GetAssignedLocationsById<T>(int staffId, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@staffId",staffId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetAssignedLocationsById.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetStaffHeaderData<T>(int staffId, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                                          new SqlParameter("@staffId",staffId)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.STF_GetHeaderData.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
