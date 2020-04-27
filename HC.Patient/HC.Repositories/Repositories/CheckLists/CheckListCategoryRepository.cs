using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.CheckLists;
using HC.Repositories;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.CheckLists
{
    public class CheckListCategoryRepository : RepositoryBase<CheckListCategory>, ICheckListCategoryRepository
    {
        private HCOrganizationContext _context;
        JsonModel response;
        public CheckListCategoryRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public CheckListCategory AddUpdateChecklistCategory(CheckListCategory checkListCategory)
        {
            
            try
            {
                if (checkListCategory.CheckListCategoryID > 0)
                {
                    _context.CheckListCategory.Update(checkListCategory);
                }
                else
                {
                    _context.CheckListCategory.Add(checkListCategory);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return checkListCategory;
        }
        public CheckListCategory CheckExistingCategory<T>(CheckListCategory checkListCategory) where T : class, new()
        {
            var category = _context.CheckListCategory.Where(m =>m.CategoryName.ToLower() == checkListCategory.CategoryName.ToLower() && m.IsActive == true && m.IsDeleted == false).FirstOrDefault();
            return category;

        }
        public void DeleteChecklistCategory(CheckListCategory checkListCategory)
        {
            var category = _context.CheckListCategory.Where(m => (m.CheckListCategoryID == checkListCategory.CheckListCategoryID || m.CategoryName == checkListCategory.CategoryName)).FirstOrDefault();
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
       
             public IQueryable<T> GetAllCheckListCategories<T>(CheckListFilterModel searchFilterModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText),
                new SqlParameter("@OrganizationId",token.OrganizationID),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize",searchFilterModel.pageSize),
                new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder",searchFilterModel.sortOrder),
            new SqlParameter("@CategoryName", searchFilterModel.CategoryName)};
            
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CHK_CheckListCategoryList, parameters.Length, parameters).AsQueryable();
        }
    }
}
