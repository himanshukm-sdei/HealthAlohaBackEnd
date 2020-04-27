using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.MasterQuestionnaire;
using HC.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.MasterQuestionnaire
{
    public class MasterQuestionnaireCategoryRepository : MasterRepositoryBase<MasterDFA_Category>, IMasterQuestionnaireCategoryRepository
    {
        private HCMasterContext _context;
        public MasterQuestionnaireCategoryRepository(HCMasterContext context) : base(context)
        {
            this._context = context;
        }

        #region Category
        public IQueryable<T> GetCategories<T>(CommonFilterModel categoryFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter("@SearchText",categoryFilterModel.SearchText),
                                          new SqlParameter("@PageNumber", categoryFilterModel.pageNumber),
                                          new SqlParameter("@PageSize", categoryFilterModel.pageSize),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                          new SqlParameter("@SortColumn",categoryFilterModel.sortColumn),
                                          new SqlParameter("@SortOrder",categoryFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MasterDFA_GetCategories.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public List<int> GetMasterHRACategoryRisk(int categoryId)
        {
            return _context.MasterMappingHRACategoryRisk.Where(a => a.HRACategoryId == categoryId).Select(a => a.HRACategoryRiskId).ToList();
        }

        public void Save(MasterDFA_Category masterDFA_Category, bool IsUpdate)
        {
            if (!IsUpdate)
            {
                _context.MasterDFA_Category.Add(masterDFA_Category);
            }
            else
            {
                _context.MasterDFA_Category.Update(masterDFA_Category);
            }
            _context.SaveChanges();
        }
        public void DeleteMasterHRACategoryRiskMapping(int categoryId)
        {
            List<MasterMappingHRACategoryRisk> mappingHRACategoryRisk = _context.MasterMappingHRACategoryRisk.Where(a => a.HRACategoryId == categoryId).ToList();
            if (mappingHRACategoryRisk != null && mappingHRACategoryRisk.Count > 0)
            {
                _context.RemoveRange(mappingHRACategoryRisk.ToArray());
                _context.SaveChanges();
            }
        }
        #endregion
    }
}