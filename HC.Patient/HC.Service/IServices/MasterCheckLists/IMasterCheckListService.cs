using HC.Model;
using HC.Patient.Model.MasterCheckLists;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.MasterCheckLists
{
    public interface IMasterCheckListService : IBaseService
    {
        #region MasterCheckListCategory
        JsonModel AddUpdateMasterChecklistCategory(MasterCheckListCategoryModel masterCheckListCategory, TokenModel token);
        JsonModel DeleteMasterChecklistCategory(int checkListCategoryId, string checklistCategoryName, TokenModel token);
        JsonModel GetMasterChecklistCategories(CheckListFilterModel searchFilter, TokenModel token);
        #endregion
        #region MasterCheckList
        JsonModel AddUpdateMasterCheckList(MasterCheckListModel checkListModel, TokenModel token);
        JsonModel DeleteMasterCheckList(int Id, TokenModel token);
        JsonModel GetAllMasterCheckList(CheckListFilterModel searchFilterModel, TokenModel tokenModel);
        #endregion

    }
}
