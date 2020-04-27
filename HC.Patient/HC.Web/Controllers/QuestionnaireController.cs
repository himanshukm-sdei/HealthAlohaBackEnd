using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Questionnaire;
using HC.Patient.Service.IServices.Questionnaire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Questionnaire")]
    public class QuestionnaireController : BaseController
    {
        #region Constructor Method
        private readonly IQuestionnaireService _quesionnaireService;
        public QuestionnaireController(IQuestionnaireService quesionnaireService)
        {
            _quesionnaireService = quesionnaireService;

        }
        #endregion

        #region Class Methods

        #region Category
        /// <summary>
        /// get the list of all categories
        /// </summary>
        /// <param name="categoryFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetCategories")]
        public JsonResult GetCategories(CommonFilterModel categoryFilterModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetCategories(categoryFilterModel, GetToken(HttpContext))));
        }
        
        /// <summary>
        /// get the category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCategoryById")]
        public JsonResult GetCategoryById(int id)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetCategoryById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// save and update categories
        /// </summary>
        /// <param name="categoryModel"></param>
        /// <returns></returns>
        [HttpPost("SaveCategory")]        
        public JsonResult SaveCategory([FromBody]CategoryModel categoryModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.SaveCategory(categoryModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// delete the category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteCategory")]
        public JsonResult DeleteCategory(int id)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.DeleteCategory(id, GetToken(HttpContext))));
        }
        #endregion

        #region Category Codes 
        /// <summary>
        /// get the listing of category codes
        /// </summary>
        /// <param name="categoryCodesFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetCategoryCodes")]
        public JsonResult GetCategoryCodes(CategoryCodesFilterModel categoryCodesFilterModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetCategoryCodes(categoryCodesFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the category code by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCategoryCodeById")]
        public JsonResult GetCategoryCodeById(int id)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetCategoryCodeById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// save and update category code
        /// </summary>
        /// <param name="categoryCodeModel"></param>
        /// <returns></returns>
        [HttpPost("SaveCategoryCodes")]
        public JsonResult SaveCategoryCodes([FromBody]CategoryCodeModel categoryCodeModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.SaveCategoryCodes(categoryCodeModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// delete the category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteCategoryCode")]
        public JsonResult DeleteCategoryCode(int id)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.DeleteCategoryCode(id, GetToken(HttpContext))));
        }
        #endregion

        #region Documents
        /// <summary>
        /// get the list of all documents
        /// </summary>
        /// <param name="commonFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetDocuments")]
        public JsonResult GetDocuments(CommonFilterModel commonFilterModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetDocuments(commonFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDocumentById")]
        public JsonResult GetDocumentById(int id)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetDocumentById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// save and update documents
        /// </summary>
        /// <param name="questionnaireDocumentModel"></param>
        /// <returns></returns>
        [HttpPost("SaveDocument")]
        public JsonResult SaveDocument([FromBody]QuestionnaireDocumentModel questionnaireDocumentModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.SaveDocument(questionnaireDocumentModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// delete the document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteDocument")]
        public JsonResult DeleteDocument(int id)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.DeleteDocument(id, GetToken(HttpContext))));
        }
        #endregion

        #region Section
        /// <summary>
        /// get the list of all section
        /// </summary>
        /// <param name="sectionFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetSections")]
        public JsonResult GetSections(SectionFilterModel sectionFilterModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetSections(sectionFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the section by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetSectionById")]
        public JsonResult GetSectionById(int id)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetSectionById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// save and update section
        /// </summary>
        /// <param name="questionnaireSectionModel"></param>
        /// <returns></returns>
        [HttpPost("SaveSection")]
        public JsonResult SaveSection([FromBody]QuestionnaireSectionModel questionnaireSectionModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.SaveSection(questionnaireSectionModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// delete the section by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteSection")]
        public JsonResult DeleteSection(int id)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.DeleteSection(id, GetToken(HttpContext))));
        }
        #endregion

        #region Section Item        
        /// <summary>
        /// save and update section item
        /// </summary>
        /// <param name="questionnaireSectionItemModel"></param>
        /// <returns></returns>
        [HttpPost("SaveSectionItem")]
        public JsonResult SaveSectionItem([FromBody]QuestionnaireSectionItemModel questionnaireSectionItemModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.SaveSectionItem(questionnaireSectionItemModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the list of all section items
        /// </summary>
        /// <param name="sectionFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetSectionItems")]
        public JsonResult GetSectionItems(SectionFilterModel sectionFilterModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetSectionItem(sectionFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the dropdown values
        /// </summary>
        /// <param name="sectionFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetSectionItemDDValues")]
        public JsonResult GetSectionItemDDValues(SectionFilterModel sectionFilterModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetSectionItemDDValues(sectionFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the section item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetSectionItemById")]
        public JsonResult GetSectionItemById(int id)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetSectionItemById(id, GetToken(HttpContext))));
        }
        #endregion

        #region Patient Question Answer
        /// <summary>
        /// save question answer
        /// </summary>
        /// <param name="answersModel"></param>
        /// <returns></returns>
        [HttpPost("SavePatientDocumentAnswer")]
        public JsonResult SavePatientDocumentAnswer([FromBody]AnswersModel answersModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.SavePatientDocumentAnswer(answersModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the patient document with answer
        /// </summary>
        /// <param name="patientDocumentFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetPatientDocumentAnswer")]
        public JsonResult GetPatientDocumentAnswer(PatientDocumentAnswerFilterModel patientDocumentFilterModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetPatientDocumentAnswer(patientDocumentFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the patient documents
        /// </summary>
        /// <param name="patientDocumentFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetPatientDocuments")]
        public JsonResult GetPatientDocuments(PatientDocumentFilterModel patientDocumentFilterModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetPatientDocuments(patientDocumentFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// save question answer
        /// </summary>
        /// <param name="assignDocumentToPatientModel"></param>
        /// <returns></returns>
        [HttpPost("AssignDocumentToPatient")]
        public JsonResult AssignDocumentToPatient([FromBody]AssignDocumentToPatientModel assignDocumentToPatientModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.AssignDocumentToPatient(assignDocumentToPatientModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// save patient's document 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetPatientDocumentById")]
        public JsonResult GetPatientDocumentById(int id)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.GetPatientDocumentById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// add signature of client and staff
        /// </summary>
        /// <param name="saveSignatureModel"></param>
        /// <returns></returns>
        [HttpPost("UpdateStatus")]
        public JsonResult UpdateStatus([FromBody]SaveSignatureModel saveSignatureModel)
        {
            return Json(_quesionnaireService.ExecuteFunctions(() => _quesionnaireService.UpdateStatus(saveSignatureModel, GetToken(HttpContext))));
        }
        #endregion

        #endregion

        #region Helping Methods
        #endregion
    }
}