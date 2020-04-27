using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.MasterCheckLists
{
    public interface IMasterCheckListRepository : IMasterRepositoryBase<MasterCheckList>
    {
        string AddUpdateMasterCheckList(MasterCheckList checkListobj, TokenModel tokenModel);
        MasterCheckList CheckExistingMasterCheckListPoint(MasterCheckList checkListobj, TokenModel tokenModel);
        void DeleteMasterCheckList(int CheckListID, TokenModel tokenModel);
        IQueryable<T> GetAllMasterCheckList<T>(CheckListFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
