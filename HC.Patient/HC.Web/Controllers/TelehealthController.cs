using HC.Model;
using HC.Patient.Service.IServices.Telehealth;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Telehealth")]
    [ActionFilter]
    public class TelehealthController : BaseController
    {
        //private JsonModel response;
        private ITelehealthService _telehealthService;
        
        public TelehealthController(ITelehealthService telehealthService)
        {
            _telehealthService = telehealthService;
        }
        // GET: api/Telehealth
        [HttpGet("GetTelehealthSession")]
        public JsonResult GetTelehealthSession(int patientID, int staffID, DateTime startTime, DateTime endTime)
        {
            try
            {
                //DateTime startDateTime = new DateTime();
                //DateTime.TryParse(startTime,out startDateTime);
                //DateTime endDateTime = new DateTime();
                //DateTime.TryParse(endTime, out endDateTime);

                
                //startDateTime = CommonMethods.ConvertUtcTime(startDateTime, token.Timezone);
                //endDateTime = CommonMethods.ConvertUtcTime(endDateTime, token.Timezone);

                return Json(_telehealthService.GetTelehealthSession(patientID, staffID, startTime, endTime, GetToken(HttpContext)));
            }
            catch (Exception ex)
            {
                return Json(new JsonModel()
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                });

            }
        }

     
        [HttpGet("{id}", Name = "GetTelehealthSession")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Telehealth
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Telehealth/5
        [HttpPatch("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
