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
    public class MasterCheckListCategoryRepository : MasterRepositoryBase<MasterCheckListCategory>, IMasterCheckListCategoryRepository
    {
        private HCMasterContext _context;
        public MasterCheckListCategoryRepository(HCMasterContext context) : base(context)
        {
            _context = context;
        }
        public MasterCheckListCategory AddUpdateMasterChecklistCategory(MasterCheckListCategory checkListCategory)
        {
            string isSuccess = "";
            try
            {
                if (checkListCategory.CheckListCategoryID > 0)
                {
                    _context.MasterCheckListCategory.Update(checkListCategory);
                }
                else
                {
                    _context.MasterCheckListCategory.Add(checkListCategory);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            return checkListCategory;
        }
        public MasterCheckListCategory CheckExistingMasterCategory<T>(MasterCheckListCategory checkListCategory) where T : class, new()
        {
            var category = _context.MasterCheckListCategory.Where(m => m.CategoryName.ToLower() == checkListCategory.CategoryName.ToLower() && m.IsActive == true && m.IsDeleted == false).FirstOrDefault();
            return category;

        }
        public void DeleteMasterChecklistCategory(MasterCheckListCategory checkListCategory)
        {
            var category = _context.MasterCheckListCategory.Where(m => (m.CheckListCategoryID == checkListCategory.CheckListCategoryID || m.CategoryName == checkListCategory.CategoryName)).FirstOrDefault();
            if (category != null)
            {
                category.IsDeleted = checkListCategory.IsDeleted;
                category.DeletedBy = checkListCategory.DeletedBy;
                category.DeletedDate = checkListCategory.DeletedDate;
                _context.Update(category);
                _context.SaveChanges();
            }
            return;
        }
       
        public IQueryable<T> GetMasterCheckListCategories<T>(CheckListFilterModel searchFilterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize",searchFilterModel.pageSize),
                new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder",searchFilterModel.sortOrder),
            new SqlParameter("@CategoryName", searchFilterModel.CategoryName)};

            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CHK_MasterCheckListCategoryList, parameters.Length, parameters).AsQueryable();
        }
    }
}
