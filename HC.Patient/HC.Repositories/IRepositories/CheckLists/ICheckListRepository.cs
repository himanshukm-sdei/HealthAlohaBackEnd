using HC.Repositories.Interfaces;
using System.Collections.Generic;
using HC.Patient.Entity;
using System.Linq;
using HC.Model;

namespace HC.Patient.Repositories.IRepositories.CheckLists
{
    public interface ICheckListRepository: IRepositoryBase<CheckList>
    {
        CheckList CheckExistingCheckListPoint(CheckList checkListobj, TokenModel tokenModel);
        string AddUpdateCheckList(CheckList checkListobj, TokenModel tokenModel);
        void DeleteCheckList(int CheckListID, TokenModel tokenModel);
        IQueryable<T> GetAllCheckList<T>(CheckListFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
