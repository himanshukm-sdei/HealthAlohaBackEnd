using HC.Common;
using HC.Model;
using HC.Patient.Model.Organizations;
using HC.Patient.Service.IServices.Organizations;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Organization")]
    [ActionFilter]
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;        
        private JsonModel response;
        #region Construtor of the class
        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;            
        }
        #endregion

        #region Class Methods        

        #endregion

        #region Helping Methods       
        /// <summary>
        /// save organization and database and smtp of the organization
        /// </summary>
        /// <param name="organizationModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOrganization")]
        public JsonResult SaveOrganization([FromBody]OrganizationModel organizationModel)
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = HttpContext;
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            //TokenModel token = new TokenModel() { UserID = 1, OrganizationID = 1 };
            if (token != null)
                response = _organizationService.SaveOrganization(organizationModel, token, httpContextAccessor);
            return Json(response);
        }


        [HttpGet]
        [Route("GetOrganizationById")]
        public JsonResult GetOrganizationById(int Id)
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = HttpContext;
            response = _organizationService.GetOrganizationById(Id, httpContextAccessor);
            return Json(response);
        }
        /// <summary>
        /// Get Organization Details By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOrganizationDetailsById/{Id}")]
        public JsonResult GetOrganizationDetailsById(int Id)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_organizationService.ExecuteFunctions<JsonModel>(() => _organizationService.GetOrganizationDetailsById(token)));
        }

        [HttpGet]
        [Route("GetOrganizations")]
        public JsonResult GetOrganizations(string businessName = "", string orgName = "", string country = "", string sortOrder = "", string sortColumn = "", int pageNumber = 1, int pageSize = 10)
        {
            response = _organizationService.GetOrganizations(businessName,orgName,country,sortOrder,sortColumn,pageNumber,pageSize);
            return Json(response);
        }


        [HttpDelete]
        [Route("DeleteOrganization")]
        public JsonResult DeleteOrganization(int Id)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = HttpContext;
            response = _organizationService.DeleteOrganization(Id, token, httpContextAccessor);
            return Json(response);
        }

        [HttpGet]
        [Route("CheckOrganizationBusinessName")]
        public JsonResult CheckOrganizationBusinessName(string BusinessName)
        {
            response = _organizationService.CheckOrganizationBusinessName(BusinessName);
            return Json(response);
        }
        #endregion


        #region Class Methods        

        #endregion

    }
}