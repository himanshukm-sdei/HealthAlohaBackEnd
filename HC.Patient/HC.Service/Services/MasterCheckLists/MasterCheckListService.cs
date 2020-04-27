using AutoMapper;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.MasterCheckLists;
using HC.Patient.Repositories.IRepositories.MasterCheckLists;
using HC.Patient.Service.IServices.MasterCheckLists;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.MasterCheckLists
{
    public class MasterCheckListService: BaseService, IMasterCheckListService
    {
        private readonly IMasterCheckListCategoryRepository _masterCheckListCategoryRepository;
        private readonly IMasterCheckListRepository _masterCheckListRepository;
        private JsonModel response;

        public MasterCheckListService(IMasterCheckListCategoryRepository masterCheckListCategoryRepository,
            IMasterCheckListRepository masterCheckListRepository)
        {
            _masterCheckListCategoryRepository = masterCheckListCategoryRepository;
            _masterCheckListRepository = masterCheckListRepository;
        }
        #region MasterCheckListCategory
        public JsonModel AddUpdateMasterChecklistCategory(MasterCheckListCategoryModel masterCheckListCategory, TokenModel token)
        {
            MasterCheckListCategory masterCategory = new MasterCheckListCategory();
            response = new JsonModel();
            masterCategory.CategoryName = masterCheckListCategory.CategoryName;
            masterCategory = _masterCheckListCategoryRepository.CheckExistingMasterCategory<MasterCheckListCategory>(masterCategory);
            if (masterCategory != null)
            {
                response.data = null;
                response.Message = StatusMessage.NameAlreadyExist;
                response.StatusCode = (int)HttpStatusCodes.BadRequest;
            }
            else
            {
                if (masterCheckListCategory.CheckListCategoryID > 0)
                {
                    masterCheckListCategory.UpdatedBy = token.UserID;
                    masterCheckListCategory.UpdatedDate = DateTime.Now;
                    masterCheckListCategory.DeletedDate = null;
                }
                else
                {
                    masterCheckListCategory.CategoryName = masterCheckListCategory.CategoryName;
                    masterCheckListCategory.CreatedBy = token.UserID;
                    masterCheckListCategory.CreatedDate = DateTime.Now;
                    masterCheckListCategory.IsActive = true;
                    masterCheckListCategory.IsDeleted = false;
                    masterCheckListCategory.DeletedDate = null;
                    masterCheckListCategory.UpdatedDate = null;
                }
                MasterCheckListCategory masterCategoryEntity = _masterCheckListCategoryRepository.AddUpdateMasterChecklistCategory(AutoMapper.Mapper.Map<MasterCheckListCategory>(masterCheckListCategory));
                AutoMapper.Mapper.Map(masterCategoryEntity, masterCheckListCategory);
                response.data = masterCheckListCategory;
                response.StatusCode = (int)HttpStatusCodes.OK;
                response.Message = StatusMessage.Success;
            }
            return response;
        }
        public JsonModel DeleteMasterChecklistCategory(int checkListCategoryId, string checklistCategoryName, TokenModel token)
        {
            MasterCheckListCategoryModel categoryModel = new MasterCheckListCategoryModel();
            categoryModel.CheckListCategoryID = checkListCategoryId;
            categoryModel.CategoryName = checklistCategoryName;
            categoryModel.IsDeleted = true;
            categoryModel.DeletedBy = token.UserID;
            categoryModel.DeletedDate = DateTime.Now;
            _masterCheckListCategoryRepository.DeleteMasterChecklistCategory(AutoMapper.Mapper.Map<MasterCheckListCategory>(categoryModel));
            return new JsonModel()
            {
                data = null,
                Message = StatusMessage.Delete,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        public JsonModel GetMasterChecklistCategories(CheckListFilterModel searchFilter, TokenModel token)
        {
            List<MasterCheckListCategoryModel> categoryList = new List<MasterCheckListCategoryModel>();
            List<MasterCheckListCategory> response = _masterCheckListCategoryRepository.GetMasterCheckListCategories<MasterCheckListCategory>(searchFilter, token).ToList();
            return new JsonModel()
            {
                data = response,
                Message = StatusMessage.SuccessFul,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        #endregion
        #region MasterCheckList
        public JsonModel AddUpdateMasterCheckList(MasterCheckListModel checkListModel, TokenModel token)
        {
            response = new JsonModel();
            
            MasterCheckList _dataExists = _masterCheckListRepository.CheckExistingMasterCheckListPoint(AutoMapper.Mapper.Map<MasterCheckList>(checkListModel), token);
            if (_dataExists != null)
            {
                response.data = null;
                response.Message = StatusMessage.NameAlreadyExist;
                response.StatusCode = (int)HttpStatusCodes.BadRequest;
            }
            else
            {
                string responseMessage = _masterCheckListRepository.AddUpdateMasterCheckList(AutoMapper.Mapper.Map<MasterCheckList>(checkListModel), token);
                response.data = checkListModel;
                response.Message = responseMessage;
                response.StatusCode = (int)HttpStatusCodes.OK;
            }
            return response;
        }
        public JsonModel DeleteMasterCheckList(int Id, TokenModel token)
        {
            _masterCheckListRepository.DeleteMasterCheckList(Id, token);
            return new JsonModel()
            {
                data = null,
                Message = StatusMessage.Delete,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        public JsonModel GetAllMasterCheckList(CheckListFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<MasterCheckListModel> response = _masterCheckListRepository.GetAllMasterCheckList<MasterCheckListModel>(searchFilterModel, tokenModel).ToList();
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
