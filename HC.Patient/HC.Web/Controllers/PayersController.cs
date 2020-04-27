using HC.Model;
using HC.Patient.Model.Payer;
using HC.Patient.Service.IServices.Payer;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Payers")]
    [ActionFilter]
    public class PayersController : BaseController
    {
        private readonly IPayerService _payerService;
        public PayersController(IPayerService payerService)
        {
            _payerService = payerService;
        }

        #region Payer information : Listing/Save/Update/Delete
        /// <summary>
        /// Get Payer List
        /// </summary>
        /// <param name="searchFilterModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayerList")]
        public JsonResult GetPayerList(SearchFilterModel searchFilterModel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetPayerList(searchFilterModel, GetToken(HttpContext))));
        }
      
        /// <summary>
        /// Save Payers
        /// </summary>
        /// <param name="insuranceCompanyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SavePayerData")]
        public JsonResult SavePayerData([FromBody]InsuranceCompanyModel insuranceCompanyModel)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.SavePayerData(insuranceCompanyModel, GetToken(HttpContext))));
        }
       
        /// <summary>
        /// Get Payer By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayerDataById/{Id}")]
        public JsonResult GetPayerDataById(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.GetPayerDataById(Id, GetToken(HttpContext))));
        }
      
        /// <summary>
        /// Delete Payer Data
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("DeletePayerData/{Id}")]
        public JsonResult DeletePayerData(int Id)
        {
            return Json(_payerService.ExecuteFunctions<JsonModel>(() => _payerService.DeletePayerData(Id, GetToken(HttpContext))));
        }

        #endregion
    }
}