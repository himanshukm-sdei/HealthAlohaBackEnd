using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.MasterQuestionnaire
{
    public interface IMasterQuestionnaireSectionRepository : IMasterRepositoryBase<MasterDFA_Section>
    {
        IQueryable<T> GetSections<T>(SectionFilterModel sectionFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
