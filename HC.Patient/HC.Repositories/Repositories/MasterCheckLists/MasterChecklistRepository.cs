using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.MasterCheckLists;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.MasterCheckLists
{
    public class MasterCheckListRepository : MasterRepositoryBase<MasterCheckList>, IMasterCheckListRepository
    {
        private HCMasterContext _context;
        public MasterCheckListRepository(HCMasterContext context) : base(context)
        {
            _context = context;
        }
        public string AddUpdateMasterCheckList(MasterCheckList checkListobj, TokenModel tokenModel)
        {
            string isSuccess = "";
            try
            {
                if (checkListobj.CheckListID > 0)
                {                   
                    checkListobj.UpdatedBy = tokenModel.UserID;
                    checkListobj.UpdatedDate = DateTime.Now;
                    _context.MasterCheckList.Update(checkListobj);
                    isSuccess = "Updated Successfully";
                }
                else
                {
                    checkListobj.CreatedBy = tokenModel.UserID;
                    checkListobj.CreatedDate = DateTime.Now;
                    checkListobj.IsActive = true;
                    checkListobj.IsDeleted = false;
                    _context.MasterCheckList.Add(checkListobj);
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
        public MasterCheckList CheckExistingMasterCheckListPoint(MasterCheckList checkListobj, TokenModel tokenModel)
        {
            var category = _context.MasterCheckList.Where(m => m.CheckListPoints.ToLower() == checkListobj.CheckListPoints.ToLower() && 
            m.CheckListCategoryID == checkListobj.CheckListCategoryID
            && m.IsActive == true && m.IsDeleted == false).FirstOrDefault();
            return category;

        }
        public void DeleteMasterCheckList(int CheckListID, TokenModel tokenModel)
        {
            try
            {
                var CheckListVAR = _context.MasterCheckList.Where(m => (m.CheckListID == CheckListID)).FirstOrDefault();
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
        public IQueryable<T> GetAllMasterCheckList<T>(CheckListFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText),
                new SqlParameter("@Type",searchFilterModel.Type),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize",searchFilterModel.pageSize),
                new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CHK_GetAllMasterCheckLists, parameters.Length, parameters).AsQueryable();
        }
    }
}
