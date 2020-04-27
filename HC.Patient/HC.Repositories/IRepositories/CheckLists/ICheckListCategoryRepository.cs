using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.IRepositories.CheckLists
{
    public interface ICheckListCategoryRepository: IRepositoryBase<CheckListCategory>
    {
        CheckListCategory CheckExistingCategory<T>(CheckListCategory checkListCategory) where T : class, new();
        CheckListCategory AddUpdateChecklistCategory(CheckListCategory checkListCategory);
        void DeleteChecklistCategory(CheckListCategory checkListCategory);
        IQueryable<T> GetAllCheckListCategories<T>(CheckListFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
