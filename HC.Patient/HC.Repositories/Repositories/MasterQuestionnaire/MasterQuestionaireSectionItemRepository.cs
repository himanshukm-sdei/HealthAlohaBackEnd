using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Questionnaire;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HC.Patient.Repositories.IRepositories.Questionnaire;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Repositories.IRepositories.MasterQuestionnaire;

namespace HC.Patient.Repositories.Repositories.MasterQuestionnaire
{
     public class MasterQuestionaireSectionItemRepository : MasterRepositoryBase<MasterDFA_SectionItem>, IMasterQuestionnaireSectionItemRepository
    {
        private HCMasterContext _context;
        public MasterQuestionaireSectionItemRepository(HCMasterContext context) : base(context)
        {
            this._context = context;
        }

        public MasterSectionItemlistingModel GetSectionItems(MasterSectionFilterModel masterSectionFilterModel, TokenModel tokenModel)
        {
            SqlParameter[] parameters = {new SqlParameter("@DocumentId",masterSectionFilterModel.DocumentId),
                                          new SqlParameter("@PageNumber", masterSectionFilterModel.pageNumber),
                                          new SqlParameter("@PageSize", masterSectionFilterModel.pageSize),
                                          new SqlParameter("@SortColumn",masterSectionFilterModel.sortColumn),
                                          new SqlParameter("@SortOrder",masterSectionFilterModel.sortOrder),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),};
            return _context.ExecStoredProcedureListWithOutputForSectionItems(SQLObjects.MasterDFA_GetSectionItems.ToString(), parameters.Length, parameters);
        }

        public MasterSectionItemlistingModel GetSectionItemsForForm(int DocumentId, TokenModel tokenModel)
        {
            SqlParameter[] parameters = {new SqlParameter("@DocumentId",DocumentId),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),};
            return _context.ExecStoredProcedureListWithOutputForSectionItems(SQLObjects.MasterDFA_GetSectionItemsForForm.ToString(), parameters.Length, parameters);
        }

        public MasterSectionItemDDValueModel GetSectionItemDDValues(MasterSectionFilterModel sectionFilterModel, TokenModel tokenModel)
        {
            SqlParameter[] parameters = {new SqlParameter("@DocumentId",sectionFilterModel.DocumentId),
                                          new SqlParameter("@OrganizationId", tokenModel.OrganizationID),};
            return _context.ExecStoredProcedureListWithOutputForSectionItemDDValues(SQLObjects.MasterDFA_GetSectionItemDDValues.ToString(), parameters.Length, parameters);
        }

        public IQueryable<T> GetSectionItemsByID<T>(int id, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@SectionItemId", id) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MasterDFA_GetSectionItemsByID.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}

