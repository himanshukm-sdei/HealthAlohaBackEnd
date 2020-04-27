using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.IRepositories.MasterCheckLists
{
    public interface IMasterCheckListCategoryRepository : IMasterRepositoryBase<MasterCheckListCategory>
    {
        MasterCheckListCategory CheckExistingMasterCategory<T>(MasterCheckListCategory masterCategory) where T : class, new();
        MasterCheckListCategory AddUpdateMasterChecklistCategory(MasterCheckListCategory checkListCategory);
        void DeleteMasterChecklistCategory(MasterCheckListCategory checkListCategory);
        IQueryable<T> GetMasterCheckListCategories<T>(CheckListFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
