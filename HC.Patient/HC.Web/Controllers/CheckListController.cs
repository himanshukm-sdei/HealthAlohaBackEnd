using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HC.Model;
using HC.Common.HC.Common;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Web.Filters;
using HC.Patient.Service.IServices.CheckLists;
using HC.Patient.Model.CheckLists;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("CheckList")]
    [ActionFilter]
    public class CheckListController : BaseController
    {
        private readonly ICheckListService _checkListService;
        private JsonModel response;
        #region Construtor of the class
        public CheckListController(ICheckListService checkListService)
        {
            _checkListService = checkListService;
        }
        #endregion
        #region CheckListCategory
        [HttpPost("AddUpdateChecklistCategory")]
        public async Task<IActionResult> AddUpdateChecklistCategory([FromBody]CheckListCategoryModel checkListCategories)
        {
            response = await Task.Run(() => _checkListService.ExecuteFunctions<JsonModel>(() => _checkListService.AddUpdateChecklistCategory(checkListCategories, GetToken(HttpContext))));
            return Json(response);
        }
        [HttpPost("DeleteChecklistCategory")]
        public async Task<IActionResult> DeleteChecklistCategory(int checkListCategoryId, string checklistCategoryName)
        {
            response = await Task.Run(() => _checkListService.ExecuteFunctions<JsonModel>(() => _checkListService.DeleteChecklistCategory(checkListCategoryId,  checklistCategoryName, GetToken(HttpContext))));
            return Json(response);
        }
        [HttpPost("GetChecklistCategories")]
        public async Task<IActionResult> GetChecklistCategories(CheckListFilterModel searchFilter)
        {
            response = await Task.Run(() => _checkListService.ExecuteFunctions<JsonModel>(() => _checkListService.GetAllCheckListCategories(searchFilter, GetToken(HttpContext))));
            return Json(response);
        }
        #endregion
        #region CheckList
        [HttpPost("AddUpdateCheckList")]
        public async Task<IActionResult> AddUpdateCheckList([FromBody]CheckListModel _checkListModel)
        {
            response = await Task.Run(() => _checkListService.ExecuteFunctions<JsonModel>(() => _checkListService.AddUpdateCheckList(_checkListModel,GetToken(HttpContext))));
            return Json(response);
        }
        [HttpPost("DeleteCheckList")]
        public async Task<IActionResult> DeleteCheckList(int Id)
        {
            response = await Task.Run(() => _checkListService.ExecuteFunctions<JsonModel>(() => _checkListService.DeleteCheckList(Id, GetToken(HttpContext))));
            return Json(response);
        }
        [HttpPost("GetAllCheckList")]
        public async Task<IActionResult> GetAllCheckList([FromBody]CheckListFilterModel _checkListSearchFilterModel)
        {
            response = await Task.Run(() => _checkListService.ExecuteFunctions<JsonModel>(() => _checkListService.GetAllCheckList(_checkListSearchFilterModel,GetToken(HttpContext))));
            return Json(response);
        }
        #endregion
    }
}