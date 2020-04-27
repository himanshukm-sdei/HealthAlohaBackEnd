using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.CheckLists;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.CheckLists
{
    public class CheckListRepository : RepositoryBase<CheckList>,ICheckListRepository
    {
        private HCOrganizationContext _context;
        public CheckListRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public string AddUpdateCheckList(CheckList checkListobj,TokenModel tokenModel)
        {
            string isSuccess = "";
            try
            {
                if (checkListobj.CheckListID > 0)
                {
                    checkListobj.OrganizationID = tokenModel.OrganizationID;
                    checkListobj.UpdatedBy = tokenModel.UserID;
                    checkListobj.UpdatedDate = DateTime.Now;
                    _context.CheckList.Update(checkListobj);
                    isSuccess = "Updated Successfully";
                }
                else
                {
                   
                    checkListobj.CreatedBy = 2;
                    checkListobj.CreatedDate = DateTime.Now;
                    checkListobj.OrganizationID = tokenModel.OrganizationID;
                    checkListobj.IsActive = true;
                    checkListobj.IsDeleted = false;
                    checkListobj.DeletedDate = null;
                    checkListobj.UpdatedDate = null;
                    _context.CheckList.Add(checkListobj);
                    isSuccess = "Inserted Successfull";
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccess = "Something went wrong. Try again later";
                throw;
            }

            return isSuccess;
        }
        public CheckList CheckExistingCheckListPoint(CheckList checkListobj, TokenModel tokenModel) 
        {
            var category = _context.CheckList.Where(m => m.CheckListPoints.ToLower() == checkListobj.CheckListPoints.ToLower() &&
            m.CheckListCategoryID == checkListobj.CheckListCategoryID
            && m.IsActive == true && m.IsDeleted == false).FirstOrDefault();

            return category;

        }
        public void DeleteCheckList(int CheckListID,TokenModel tokenModel)
        {
            try
            {
                var CheckListVAR = _context.CheckList.Where(m => (m.CheckListID == CheckListID)).FirstOrDefault();
                CheckListVAR.IsDeleted = true;
                CheckListVAR.DeletedBy = tokenModel.UserID;
                CheckListVAR.DeletedDate = DateTime.Now;
                _context.Update(CheckListVAR);
                _context.SaveChanges();
                return;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IQueryable<T> GetAllCheckList<T>(CheckListFilterModel searchFilterModel,TokenModel tokenModel) where T : class, new()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText),
                new SqlParameter("@Type",searchFilterModel.Type),
                new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize",searchFilterModel.pageSize),
                new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
                return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CHK_GetAllCheckLists, parameters.Length, parameters).AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
