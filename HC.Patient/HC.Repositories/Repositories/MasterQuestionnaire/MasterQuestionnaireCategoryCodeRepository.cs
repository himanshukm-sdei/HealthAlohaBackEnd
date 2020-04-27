using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.MasterQuestionnaire;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.MasterQuestionnaire
{
    public class MasterQuestionnaireCategoryCodeRepository : MasterRepositoryBase<MasterDFA_CategoryCode>, IMasterQuestionnaireCategoryCodeRepository
    {
        private HCMasterContext _context;
        public MasterQuestionnaireCategoryCodeRepository(HCMasterContext context) : base(context)
        {
            this._context = context;
        }
        #region Category Codes
        public IQueryable<T> GetCategoryCodes<T>(CategoryCodesFilterModel categoryCodesFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter("@SearchText",categoryCodesFilterModel.SearchText),
                                         new SqlParameter("@CategoryId", categoryCodesFilterModel.CategoryId),
                                         new SqlParameter("@PageNumber", categoryCodesFilterModel.pageNumber),
                                         new SqlParameter("@PageSize", categoryCodesFilterModel.pageSize),
                                         new SqlParameter("@SortColumn",categoryCodesFilterModel.sortColumn),
                                         new SqlParameter("@SortOrder",categoryCodesFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MasterDFA_GetCategoryCodes.ToString(), parameters.Length, parameters).AsQueryable();
        }
        #endregion
    }
}