using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CheckLists;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Service.IServices.CheckLists
{
    public interface ICheckListService: IBaseService
    {
        #region CheckListCategory
        JsonModel AddUpdateChecklistCategory(CheckListCategoryModel checkListCategory, TokenModel token);
        JsonModel DeleteChecklistCategory(int checkListCategoryId, string checklistCategoryName, TokenModel token);
        JsonModel GetAllCheckListCategories(CheckListFilterModel searchFilter, TokenModel token);
        #endregion
        #region CheckList
        JsonModel AddUpdateCheckList(CheckListModel checkListModel, TokenModel token);
        JsonModel DeleteCheckList(int Id, TokenModel token);
        JsonModel GetAllCheckList(CheckListFilterModel searchFilterModel, TokenModel tokenModel);
        #endregion


    }
}
