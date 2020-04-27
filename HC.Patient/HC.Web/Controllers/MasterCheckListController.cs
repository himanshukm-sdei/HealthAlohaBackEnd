using HC.Model;
using HC.Patient.Model.MasterCheckLists;
using HC.Patient.Model.CheckLists;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HC.Patient.Service.IServices.MasterCheckLists;
using HC.Patient.Service.IServices.CheckLists;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("MasterCheckList")]
    [ActionFilter]
    public class MasterCheckListController : BaseController
    {
        private readonly IMasterCheckListService _masterCheckListService;
        private JsonModel response;
        #region Construtor of the class
        public MasterCheckListController(IMasterCheckListService masterCheckListService)
        {
            _masterCheckListService = masterCheckListService;
        }
        #endregion

        #region MasterCheckListCategory
        [HttpPost("AddUpdateMasterChecklistCategory")]
        public async Task<IActionResult> AddUpdateMasterChecklistCategory([FromBody]MasterCheckListCategoryModel masterCheckListCategories)
        {
            response = await Task.Run(() => _masterCheckListService.ExecuteFunctions<JsonModel>(() => _masterCheckListService.AddUpdateMasterChecklistCategory(masterCheckListCategories, GetToken(HttpContext))));
            ///create entry in client checklist table
            if (response.StatusCode == 200)
            {
                CheckListCategoryModel checkListCategoryModel = new CheckListCategoryModel();
                checkListCategoryModel.CategoryName = masterCheckListCategories.CategoryName;
                var response1 = await Task.Run(() => _masterCheckListService.ExecuteFunctions<JsonModel>(() => _masterCheckListService.AddUpdateMasterChecklistCategory(masterCheckListCategories, GetToken(HttpContext))));
            }
            
            return Json(response);
        }
        [HttpPost("DeleteMasterChecklistCategory")]
        public async Task<IActionResult> DeleteMasterChecklistCategory(int checkListCategoryId, string checklistCategoryName)
        {
            response = await Task.Run(() => _masterCheckListService.ExecuteFunctions<JsonModel>(() => _masterCheckListService.DeleteMasterChecklistCategory(checkListCategoryId, checklistCategoryName, GetToken(HttpContext))));
            return Json(response);
        }
        [HttpPost("GetMasterChecklistCategories")]
        public async Task<IActionResult> GetMasterChecklistCategories(CheckListFilterModel searchFilter)
        {
            response = await Task.Run(() => _masterCheckListService.ExecuteFunctions<JsonModel>(() => _masterCheckListService.GetMasterChecklistCategories(searchFilter, GetToken(HttpContext))));
            return Json(response);
        }
        #endregion

        #region MasterCheckList
        [HttpPost("AddUpdateMasterCheckList")]
        public async Task<IActionResult> AddUpdateMasterCheckList([FromBody]MasterCheckListModel _checkListModel)
        {
            response = await Task.Run(() => _masterCheckListService.ExecuteFunctions<JsonModel>(() => _masterCheckListService.AddUpdateMasterCheckList(_checkListModel, GetToken(HttpContext))));
            return Json(response);
        }
        [HttpPost("DeleteMasterCheckList")]
        public async Task<IActionResult> DeleteMasterCheckList(int Id)
        {
            response = await Task.Run(() => _masterCheckListService.ExecuteFunctions<JsonModel>(() => _masterCheckListService.DeleteMasterCheckList(Id, GetToken(HttpContext))));
            return Json(response);
        }
        [HttpPost("GetAllMasterCheckList")]
        public async Task<IActionResult> GetAllMasterCheckList([FromBody]CheckListFilterModel _checkListSearchFilterModel)
        {
            response = await Task.Run(() => _masterCheckListService.ExecuteFunctions<JsonModel>(() => _masterCheckListService.GetAllMasterCheckList(_checkListSearchFilterModel, GetToken(HttpContext))));
            return Json(response);
        }
        #endregion
    }
}