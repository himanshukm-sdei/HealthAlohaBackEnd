using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.PatientEncounters;
using HC.Patient.Service.IServices.PatientEncounters;
using HC.Patient.Service.PatientCommon.Interfaces;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Route("patient-encounter")]
    [ActionFilter]
    public class PatientEncounterController : BaseController
    {
        private readonly IPatientCommonService _patientCommonService;
        private readonly IPatientEncounterService _patientEncounterService;

        #region Construtor of the class
        public PatientEncounterController(IPatientCommonService patientCommonService, IPatientEncounterService patientEncounterService)
        {
            _patientEncounterService = patientEncounterService;
            this._patientCommonService = patientCommonService;

        }
        #endregion

        /// <summary>
        /// Get patient encounter details whether it is saved or during add time to get data related to appointment type
        /// Added appointment details to remove the data flow from scheduler to encounter screen in front end
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="encounterId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpGet("GetPatientEncounterDetails/{appointmentId}/{encounterId}")]
        public JsonResult GetPatientEncounterDetails(int appointmentId, int encounterId, bool isAdmin = false)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.GetPatientEncounterDetails(appointmentId, encounterId, isAdmin, GetToken(HttpContext))));
        }

        /// <summary>
        /// Get patient non billable encounter details whether it is saved or during add time to get data related to appointment type
        /// Added appointment details to remove the data flow from scheduler to encounter screen in front end
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="encounterId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpGet("GetPatientNonBillableEncounterDetails/{appointmentId}/{encounterId}")]
        public JsonResult GetPatientNonBillableEncounterDetails(int appointmentId, int encounterId, bool isAdmin = false)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.GetPatientNonBillableEncounterDetails(appointmentId, encounterId, isAdmin, GetToken(HttpContext))));
        }

        /// <summary>
        /// Save patient encounter
        /// </summary>
        /// <param name="requestObj"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpPost("SavePatientEncounter")]
        public JsonResult SavePatientEncounter([FromBody]PatientEncounterModel requestObj, bool isAdmin = false)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SavePatientEncounter(requestObj, isAdmin, GetToken(HttpContext))));
        }

        /// <summary>
        /// Save patient non billable encounter
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost("SavePatientNonBillableEncounter")]
        public JsonResult SavePatientNonBillableEncounter([FromBody]PatientEncounterModel requestObj, bool isAdmin = false)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SavePatientNonBillableEncounter(requestObj, isAdmin, GetToken(HttpContext))));
        }
        /// <summary>
        /// Get Patient encounters
        /// </summary>
        /// <param name="patientID"></param>
        /// <param name="appointmentType"></param>
        /// <param name="staffName"></param>
        /// <param name="status"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        [HttpGet("GetPatientEncounter")]
        public JsonResult GetPatientEncounter(int? patientID, string appointmentType = "", string staffName = "", string status = "", string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            List<PatientEncounterModel> response = _patientEncounterService.GetPatientEncounter(patientID, appointmentType, staffName, status, fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder, GetToken(HttpContext));
            if (response != null && response.Count > 0)
            {
                return Json(new
                {
                    data = response,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = response[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(response[0].TotalRecords / pageSize))
                    },
                    StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
                });
            }
            else
            {
                return Json(new
                {
                    data = new object(),
                    Message=StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound//(Unprocessable Entity)
                });
            }
        }

        [HttpPost]
        [Route("SaveEncounterSignature")]
        public JsonResult SaveEncounterSignature([FromBody]EncounterSignatureModel encounterSignatureModel)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SaveEncounterSignature(encounterSignatureModel)));
        }

        /// <summary>
        /// This method will download encounter in pdf format
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns></returns>
        [HttpGet("DownloadEncounter")]
        public ActionResult DownloadEncounter(int encounterId)
        {
            MemoryStream ms = _patientEncounterService.DownloadEncounter(encounterId, GetToken(HttpContext));
            return File(ms, "application/pdf", "EncounterFile.pdf");
        }

        /// <summary>
        /// This method will save Patient Encounter Template form data
        /// </summary>
        /// <param name="patientEncounterTemplateModel"></param>
        /// <returns></returns>
        [HttpPost("SaveEncounterTemplateData")]
        public JsonResult SaveEncounterTemplateData([FromBody] PatientEncounterTemplateModel patientEncounterTemplateModel)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.SavePatientEncounterTemplateData(patientEncounterTemplateModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// This method will get Patient Encounter Template form with data
        /// </summary>
        /// <param name="patientEncounterId"></param>
        /// <param name="masterTemplateId"></param>
        /// <returns></returns>
        [HttpGet("GetPatientEncounterTemplateData")]
        public JsonResult GetPatientEncounterTemplateData(int patientEncounterId, int masterTemplateId)
        {
            return Json(_patientEncounterService.ExecuteFunctions(() => _patientEncounterService.GetPatientEncounterTemplateData(patientEncounterId, masterTemplateId, GetToken(HttpContext))));
        }

    }
}
