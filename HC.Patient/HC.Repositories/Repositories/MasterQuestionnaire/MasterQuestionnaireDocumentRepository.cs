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
    public class MasterQuestionnaireDocumentRepository : MasterRepositoryBase<MasterDFA_Document>, IMasterQuestionnaireDocumentRepository
    {
        private HCMasterContext _context;
        public MasterQuestionnaireDocumentRepository(HCMasterContext context) : base(context)
        {
            this._context = context;
        }

        #region Documents
        public IQueryable<T> GetDocuments<T>(CommonFilterModel categoryFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {new SqlParameter("@SearchText",categoryFilterModel.SearchText),
                                          new SqlParameter("@PageNumber", categoryFilterModel.pageNumber),
                                          new SqlParameter("@PageSize", categoryFilterModel.pageSize),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                          new SqlParameter("@SortColumn",categoryFilterModel.sortColumn),
                                          new SqlParameter("@SortOrder",categoryFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MasterDFA_GetDocuments.ToString(), parameters.Length, parameters).AsQueryable();
        }
        #endregion
        public IQueryable<T> GetQuestionnaireTypes<T>(TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MasterDFA_GetQuestionType.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
