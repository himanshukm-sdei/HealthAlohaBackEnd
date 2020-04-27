using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Staffs")]
    [ActionFilter]
    public class StaffsController : BaseController
    {
        private readonly IStaffService _staffService;

        private JsonModel response = new JsonModel();

        #region  Construtor of the class
        public StaffsController(IStaffService staffService)
        {
            _staffService = staffService;            
        }
        #endregion

        #region Class Methods

        /// <summary>
        /// Description: this method is used to get staffs listing with filters
        /// </summary>
        /// <param name="staffFiltterModel"></param>
        /// <returns></returns>
        [HttpGet("GetStaffs")]
        public JsonResult GetStaffs(ListingFiltterModel staffFiltterModel)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetStaffs(staffFiltterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// this method is used to create and update staff
        /// </summary>
        /// <param name="staffs"></param>
        /// <returns></returns>
        [HttpPost("CreateUpdateStaff")]
        public JsonResult CreateUpdateStaff([FromBody]Staffs staffs)
        {   
            return Json(_staffService.ExecuteFunctions(() => _staffService.CreateUpdateStaff(staffs, GetToken(HttpContext))));
        }
        
        /// <summary>
        /// this method is used to get staff by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetStaffById")]
        public JsonResult GetStaffById(int id)
        {   
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetStaffById(id, GetToken(HttpContext))));
        }
        
        /// <summary>
        /// this method is used to delete staff by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeleteStaff")]
        public JsonResult DeleteStaff(int id)
        {   
            return Json(_staffService.ExecuteFunctions(() => _staffService.DeleteStaff(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// this method is used to update staff Status
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPatch("UpdateStaffActiveStatus")]
        public JsonResult UpdateStaffActiveStatus(int staffId, bool isActive)
        {   
            return Json(_staffService.ExecuteFunctions(() => _staffService.UpdateStaffActiveStatus(staffId, isActive,GetToken(HttpContext))));
        }
        
        /// <summary>
        /// this method is used to get staff by tags filter
        /// </summary>
        /// <param name="listingFiltterModel"></param>
        /// <returns></returns>
        [HttpGet("GetStaffByTag")]
        public JsonResult GetStaffByTag(ListingFiltterModel listingFiltterModel)
        {   
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetStaffByTags(listingFiltterModel,GetToken(HttpContext))));
        }

        /// <summary>
        /// Get doctor details from NPI number
        /// </summary>
        /// <param name="npiNumber"></param>
        /// <param name="enumerationType"></param>
        /// <returns></returns>
        [HttpGet("GetDoctorDetailsFromNPI")]
        public JsonResult GetDoctorDetailsFromNPI(string npiNumber,string enumerationType)
        {
            return Json(_staffService.ExecuteFunctions<JsonModel>(() => _staffService.GetDoctorDetailsFromNPI(npiNumber, enumerationType)));
        }

        /// <summary>
        /// <Description> Get Staff Profile Data</Description>        
        /// </summary>
        /// <param name="Id">Staff Id</param>
        /// <returns></returns>
        [HttpGet("GetStaffProfileData/{Id}")]
        public JsonResult GetStaffProfileData(int Id)
        {
            return Json(_staffService.ExecuteFunctions<JsonModel>(() => _staffService.GetStaffProfileData(Id,GetToken(HttpContext))));
        }

        /// <summary>
        /// this method is used to get staff lcoation by staff id
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetAssignedLocationsById")]
        public JsonResult GetAssignedLocationsById(int id)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetAssignedLocationsById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// this method is used to get staff header data
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetStaffHeaderData")]
        public JsonResult GetStaffHeaderData(int id)
        {
            return Json(_staffService.ExecuteFunctions(() => _staffService.GetStaffHeaderData(id, GetToken(HttpContext))));
        }
        #endregion
    }
}