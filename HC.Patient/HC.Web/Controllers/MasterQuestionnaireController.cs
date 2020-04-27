using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.MasterQuestionnaire;
using HC.Patient.Service.IServices.MasterQuestionnaire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("MasterQuestionnaire")]
    public class MasterQuestionnaireController : BaseController
    {
        #region Constructor Method
        private readonly IMasterQuestionnaireService _masterQuesionnaireService;
        public MasterQuestionnaireController(IMasterQuestionnaireService masterQuesionnaireService)
        {
            _masterQuesionnaireService = masterQuesionnaireService;

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
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetCategories(categoryFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCategoryById")]
        public JsonResult GetCategoryById(int id)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetCategoryById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// save and update categories
        /// </summary>
        /// <param name="categoryModel"></param>
        /// <returns></returns>
        [HttpPost("SaveCategory")]
        public JsonResult SaveCategory([FromBody]MasterCategoryModel categoryModel)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.SaveCategory(categoryModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// delete the category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteCategory")]
        public JsonResult DeleteCategory(int id)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.DeleteCategory(id, GetToken(HttpContext))));
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
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetCategoryCodes(categoryCodesFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the category code by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCategoryCodeById")]
        public JsonResult GetCategoryCodeById(int id)
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetCategoryCodeById(id, GetToken(HttpContext), httpContextAccessor)));
        }

        /// <summary>
        /// save and update category code
        /// </summary>
        /// <param name="categoryCodeModel"></param>
        /// <returns></returns>
        [HttpPost("SaveCategoryCodes")]
        public JsonResult SaveCategoryCodes([FromBody]MasterCategoryCodeModel categoryCodeModel)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.SaveCategoryCodes(categoryCodeModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// delete the category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteCategoryCode")]
        public JsonResult DeleteCategoryCode(int id)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.DeleteCategoryCode(id, GetToken(HttpContext))));
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
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetDocuments(commonFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the list of all questionnaire types
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetQuestionnaireTypes")]
        public JsonResult GetQuestionnaireTypes()
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetQuestionnaireTypes(GetToken(HttpContext))));
        }

        /// <summary>
        /// get the document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDocumentById")]
        public JsonResult GetDocumentById(int id)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetDocumentById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// save and update documents
        /// </summary>
        /// <param name="masterQuestionnaireDocumentModel"></param>
        /// <returns></returns>
        [HttpPost("SaveDocument")]
        public JsonResult SaveDocument([FromBody]MasterQuestionnaireDocumentModel masterQuestionnaireDocumentModel)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.SaveDocument(masterQuestionnaireDocumentModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// delete the document by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteDocument")]
        public JsonResult DeleteDocument(int id)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.DeleteDocument(id, GetToken(HttpContext))));
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
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetSections(sectionFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the section by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetSectionById")]
        public JsonResult GetSectionById(int id)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetSectionById(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// save and update section
        /// </summary>
        /// <param name="questionnaireSectionModel"></param>
        /// <returns></returns>
        [HttpPost("SaveSection")]
        public JsonResult SaveSection([FromBody]MasterQuestionnaireSectionModel questionnaireSectionModel)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.SaveSection(questionnaireSectionModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// delete the section by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("DeleteSection")]
        public JsonResult DeleteSection(int id)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.DeleteSection(id, GetToken(HttpContext))));
        }
        #endregion


        #region Section Item        
        /// <summary>
        /// save and update section item
        /// </summary>
        /// <param name="questionnaireSectionItemModel"></param>
        /// <returns></returns>
        [HttpPost("SaveSectionItem")]
        public JsonResult SaveSectionItem([FromBody]MasterQuestionnaireSectionItemModel questionnaireSectionItemModel)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.SaveSectionItem(questionnaireSectionItemModel, GetToken(HttpContext))));
        }

        [HttpPatch("deleteSectionItem")]
        public JsonResult DeleteSectionItem(int id)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.DeleteSectionItem(id, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the list of all section items for form
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <returns></returns>
        [HttpGet("GetSectionItemsForForm")]
        public JsonResult GetSectionItemsForForm(int DocumentId)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetSectionItemsForForm(DocumentId, GetToken(HttpContext))));
        }
        /// <summary>
        /// get the list of all section items
        /// </summary>
        /// <param name="sectionFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetSectionItems")]
        public JsonResult GetSectionItems(MasterSectionFilterModel sectionFilterModel)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetSectionItem(sectionFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the dropdown values
        /// </summary>
        /// <param name="sectionFilterModel"></param>
        /// <returns></returns>
        [HttpGet("GetSectionItemDDValues")]
        public JsonResult GetSectionItemDDValues(MasterSectionFilterModel sectionFilterModel)
        {
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetSectionItemDDValues(sectionFilterModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// get the section item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetSectionItemById")]
        public JsonResult GetSectionItemById(int id)
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            return Json(_masterQuesionnaireService.ExecuteFunctions(() => _masterQuesionnaireService.GetSectionItemById(id, GetToken(HttpContext), httpContextAccessor)));
        }
        #endregion


        #endregion

        #region Helping Methods
        #endregion
    }
}