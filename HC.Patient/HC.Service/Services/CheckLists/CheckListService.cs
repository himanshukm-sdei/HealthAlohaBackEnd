using AutoMapper;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CheckLists;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Repositories.IRepositories.CheckLists;
using HC.Patient.Service.IServices.CheckLists;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.CheckLists
{
    public class CheckListService: BaseService, ICheckListService
    {
        private readonly ICheckListCategoryRepository _checkListCategoryRepository;
        private readonly ICheckListRepository _checkListRepository;
        private JsonModel response;

        public CheckListService(ICheckListCategoryRepository checkListCategoryRepository,
                                 ICheckListRepository checkListRepository)
        {
            _checkListCategoryRepository = checkListCategoryRepository;
            _checkListRepository = checkListRepository;
        }
        #region CheckListCategory
        public JsonModel AddUpdateChecklistCategory(CheckListCategoryModel checkListCategory, TokenModel token)
        {
            CheckListCategory categoryEntity = new CheckListCategory();
             response = new JsonModel();
            categoryEntity.CategoryName = checkListCategory.CategoryName;
            categoryEntity = _checkListCategoryRepository.CheckExistingCategory<CheckListCategory>(categoryEntity);
            if (categoryEntity != null)
            {
               response.data = null;
                response.Message = StatusMessage.NameAlreadyExist;
                response.StatusCode = (int)HttpStatusCodes.BadRequest;
            }
            else
            {
                if (checkListCategory.CheckListCategoryID > 0)
                {
                    checkListCategory.UpdatedBy = token.UserID;
                    checkListCategory.UpdatedDate = DateTime.Now;
                    checkListCategory.DeletedDate = null;
                }
                else
                {
                    checkListCategory.CreatedBy = token.UserID;
                    checkListCategory.CreatedDate = DateTime.Now;
                    checkListCategory.OrganizationID = token.OrganizationID;
                    checkListCategory.IsActive = true;
                    checkListCategory.IsDeleted = false;
                    checkListCategory.DeletedDate = null;
                    checkListCategory.UpdatedDate = null;
                }
                CheckListCategory checkListCategoryEntity = _checkListCategoryRepository.AddUpdateChecklistCategory(AutoMapper.Mapper.Map<CheckListCategory>(checkListCategory));
                AutoMapper.Mapper.Map(checkListCategoryEntity, checkListCategory);
                response.data = checkListCategory;
                response.StatusCode = (int)HttpStatusCodes.OK;
                response.Message = StatusMessage.Success;
            }
            return response;
        }
        public JsonModel DeleteChecklistCategory(int checkListCategoryId, string checklistCategoryName, TokenModel token)
        {
            CheckListCategoryModel categoryModel = new CheckListCategoryModel();
            categoryModel.CheckListCategoryID = checkListCategoryId;
            categoryModel.CategoryName = checklistCategoryName;
            categoryModel.IsDeleted = true;
            categoryModel.DeletedBy = token.UserID;
            categoryModel.DeletedDate = DateTime.Now;
            _checkListCategoryRepository.DeleteChecklistCategory(AutoMapper.Mapper.Map<CheckListCategory>(categoryModel));
            return new JsonModel()
            {
                data = null,
                Message = StatusMessage.Delete,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        public JsonModel GetAllCheckListCategories(CheckListFilterModel searchFilter, TokenModel token)
        {
            List<CheckListCategoryModel> categoryList = new List<CheckListCategoryModel>();
            List<CheckListCategory> response = _checkListCategoryRepository.GetAllCheckListCategories<CheckListCategory>(searchFilter, token).ToList();
            return new JsonModel()
            {
                data = response,
                Message = StatusMessage.SuccessFul,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        #endregion

        #region CheckList
        public JsonModel AddUpdateCheckList(CheckListModel checkListModel, TokenModel token)
        {
            response = new JsonModel();
            CheckList _dataExists = _checkListRepository.CheckExistingCheckListPoint(AutoMapper.Mapper.Map<CheckList>(checkListModel), token);
            if (_dataExists != null)
            {
                response.data = null;
                response.Message = StatusMessage.NameAlreadyExist;
                response.StatusCode = (int)HttpStatusCodes.BadRequest;
            }
            else
            {
                string responseMessage = _checkListRepository.AddUpdateCheckList(AutoMapper.Mapper.Map<CheckList>(checkListModel), token);
                response.data = checkListModel;
                response.Message = responseMessage;
                response.StatusCode = (int)HttpStatusCodes.OK;
            }
            return response;
        }
        public JsonModel DeleteCheckList(int Id, TokenModel token)
        {

            _checkListRepository.DeleteCheckList(Id, token);
            return new JsonModel()
            {
                data = null,
                Message = StatusMessage.Delete,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        public JsonModel GetAllCheckList(CheckListFilterModel searchFilterModel, TokenModel tokenModel)
        {

            List<CheckListModel> response = _checkListRepository.GetAllCheckList<CheckListModel>(searchFilterModel, tokenModel).ToList();
            return new JsonModel()
            {
                data = response,
                Message = StatusMessage.Success,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        #endregion

    }

}
