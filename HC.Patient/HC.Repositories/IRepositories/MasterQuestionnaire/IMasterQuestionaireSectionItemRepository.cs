using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Questionnaire;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.MasterQuestionnaire
{
    public interface IMasterQuestionnaireSectionItemRepository : IRepositoryBase<MasterDFA_SectionItem>
    {
        MasterSectionItemlistingModel GetSectionItems(MasterSectionFilterModel sectionFilterModel, TokenModel tokenModel);
        MasterSectionItemlistingModel GetSectionItemsForForm(int DocumentId, TokenModel tokenModel);
        MasterSectionItemDDValueModel GetSectionItemDDValues(MasterSectionFilterModel sectionFilterModel, TokenModel tokenModel);
        IQueryable<T> GetSectionItemsByID<T>(int id, TokenModel tokenModel) where T : class, new();
    }
}
