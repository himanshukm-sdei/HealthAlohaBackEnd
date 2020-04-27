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
    public class MasterQuestionnaireSectionRepository : MasterRepositoryBase<MasterDFA_Section>, IMasterQuestionnaireSectionRepository
    {
        private HCMasterContext _context;
        public MasterQuestionnaireSectionRepository(HCMasterContext context) : base(context)
        {
            this._context = context;
        }

        #region Section
        public IQueryable<T> GetSections<T>(SectionFilterModel sectionFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter("@DocumentId",sectionFilterModel.DocumentId),
                                          new SqlParameter("@PageNumber", sectionFilterModel.pageNumber),
                                          new SqlParameter("@PageSize", sectionFilterModel.pageSize),                                          
                                          new SqlParameter("@SortColumn",sectionFilterModel.sortColumn),
                                          new SqlParameter("@SortOrder",sectionFilterModel.sortOrder),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MasterDFA_GetSections.ToString(), parameters.Length, parameters).AsQueryable();
        }
        #endregion
    }
}
