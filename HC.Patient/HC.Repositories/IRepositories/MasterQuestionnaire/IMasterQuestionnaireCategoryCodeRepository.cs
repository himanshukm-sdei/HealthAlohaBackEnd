using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.MasterQuestionnaire
{
    public interface IMasterQuestionnaireCategoryCodeRepository : IMasterRepositoryBase<MasterDFA_CategoryCode>
    {
        IQueryable<T> GetCategoryCodes<T>(CategoryCodesFilterModel categoryCodesFilterModel, TokenModel tokenModel) where T : class, new();

    }
}
