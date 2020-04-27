using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.VideoAndArticle;
using HC.Patient.Service.IServices.VideoAndArticle;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("VideoAndArticles")]
    [ActionFilter]
    public class VideoAndArticlesController : BaseController
    {
        private readonly IVideoAndArticlesService _IVideoAndArticlesService;
        private JsonModel response;
        public VideoAndArticlesController(IVideoAndArticlesService IVideoAndArticlesService)
        {
            _IVideoAndArticlesService = IVideoAndArticlesService;
        }


        #region videoAndArticlesCategory
        [HttpPost("AddUpdateVideoAndArticlesCategory")]
        public async Task<IActionResult> AddUpdateVideoAndArticlesCategory([FromBody]VideoAndArticlesCategoriesModel objVideoAndArticlesCategoriesModel)
        {
            response = await Task.Run(() => _IVideoAndArticlesService.ExecuteFunctions<JsonModel>(() => _IVideoAndArticlesService.AddUpdateVideoAndArticlesCategories(objVideoAndArticlesCategoriesModel, GetToken(HttpContext))));
            return Json(response);
        }
       
        [HttpPatch]
        [Route("DeleteVideoAndArticlesCategory/{Id}")]
        public async Task<IActionResult> DeleteVideoAndArticlesCategory(int Id)
        {
            response = await Task.Run(() => _IVideoAndArticlesService.ExecuteFunctions<JsonModel>(() => _IVideoAndArticlesService.DeleteVideoAndArticlesCategories(Id, GetToken(HttpContext))));
            return Json(response);
        }

        [HttpGet]
        [Route("GetVideoAndArticlesCategoriesById/{Id}")]
        public JsonResult GetVideoAndArticlesCategoriesById(int Id)
        {
            return Json(_IVideoAndArticlesService.ExecuteFunctions<JsonModel>(() => _IVideoAndArticlesService.GetVideoAndArticlesCategoriesByID(Id, GetToken(HttpContext))));
        }

        [HttpGet]
        [Route("GetVideoAndArticlesCategory")]
        public JsonResult GetVideoAndArticlesCategory(VideoAndArticlesCategoriesFilterModel searchFilterModel)
        {
            return Json(_IVideoAndArticlesService.ExecuteFunctions<JsonModel>(() => _IVideoAndArticlesService.GetAllVideoAndArticlesCategories(searchFilterModel, GetToken(HttpContext))));
        }
        #endregion
        #region VideoAndArticles
        [HttpPost("AddUpdateVideoAndArticles")]
        public async Task<IActionResult> AddUpdateVideoAndArticles([FromBody]VideoAndArticlesModel objVideoAndArticlesModel)
        {
            response = await Task.Run(() => _IVideoAndArticlesService.ExecuteFunctions<JsonModel>(() => _IVideoAndArticlesService.AddUpdateVideoAndArticles(objVideoAndArticlesModel, GetToken(HttpContext))));
            return Json(response);
        }
       
        [HttpPatch]
        [Route("DeleteVideoAndArticles/{Id}")]
        public async Task<IActionResult> DeleteVideoAndArticles(int Id)
        {
            response = await Task.Run(() => _IVideoAndArticlesService.ExecuteFunctions<JsonModel>(() => _IVideoAndArticlesService.DeleteVideoAndArticles(Id, GetToken(HttpContext))));
            return Json(response);
        }

        [HttpGet]
        [Route("GetVideoAndArticlesById/{Id}")]
        public JsonResult GetVideoAndArticlesById(int Id)
        {
            return Json(_IVideoAndArticlesService.ExecuteFunctions<JsonModel>(() => _IVideoAndArticlesService.GetVideoAndArticleByID(Id, GetToken(HttpContext))));
        }

        [HttpGet]
        [Route("GetVideoAndArticles")]
        public JsonResult GetVideoAndArticles(VideoAndArticlesCategoriesFilterModel searchFilterModel)
        {
            return Json(_IVideoAndArticlesService.ExecuteFunctions<JsonModel>(() => _IVideoAndArticlesService.GetAllVideoAndArticles(searchFilterModel, GetToken(HttpContext))));
        }
        #endregion

    }
}