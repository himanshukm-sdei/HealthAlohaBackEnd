using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.VideoAndArticle;
using HC.Patient.Repositories.IRepositories.VideoAndArticle;
using HC.Patient.Service.IServices.VideoAndArticle;
using HC.Service;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Service.IServices.Images;
using System.IO;
using HC.Common;

namespace HC.Patient.Service.Services.VideoAndArticle
{
    public class VideoAndArticlesService : BaseService, IVideoAndArticlesService
    {
        private readonly IVideoAndArticlesRepository _videoAndArticlesRepository;
        private readonly IVideoAndArticlesCategoriesRepository _videoAndArticlesCategoriesRepository;
        private IImageService _imageService;
        private JsonModel response;

        public VideoAndArticlesService(IVideoAndArticlesCategoriesRepository videoAndArticlesCategoriesRepository,
            IVideoAndArticlesRepository videoAndArticlesRepository,
            IImageService imageService)
        {
            _imageService = imageService;
            _videoAndArticlesCategoriesRepository = videoAndArticlesCategoriesRepository;
            _videoAndArticlesRepository = videoAndArticlesRepository;
        }

        #region VideoAndArticlesCategories
        public JsonModel AddUpdateVideoAndArticlesCategories(VideoAndArticlesCategoriesModel videoAndArticlesCategoriesModel, TokenModel token)
        {
            response = new JsonModel();
            VideoAndArticlesCategories VideoAndArticlesCategoriesEntity = new VideoAndArticlesCategories();
            if (videoAndArticlesCategoriesModel.CategoryID > 0)
            {
                VideoAndArticlesCategoriesEntity = null;
            }
            else
            {
                 VideoAndArticlesCategoriesEntity = _videoAndArticlesCategoriesRepository.CheckExistingVideoArticleCategory(AutoMapper.Mapper.Map<VideoAndArticlesCategories>(videoAndArticlesCategoriesModel), token);
            }
            if (VideoAndArticlesCategoriesEntity != null)
            {
                response.data = null;
                response.Message = StatusMessage.CategoryNameAlreadyExists;
                response.StatusCode = (int)HttpStatusCodes.BadRequest;
            }
            else
            {
                string message = _videoAndArticlesCategoriesRepository.AddUpdateVideoAndArticleCategories(AutoMapper.Mapper.Map<VideoAndArticlesCategories>(videoAndArticlesCategoriesModel),token);               
                response.data = videoAndArticlesCategoriesModel;
                response.StatusCode = (int)HttpStatusCodes.OK;
                response.Message = message;
            }
            return response;
        }
        public JsonModel DeleteVideoAndArticlesCategories(int Id, TokenModel token)
        {

            _videoAndArticlesCategoriesRepository.DeleteVideoAndArticlesCategories(Id, token);
            return new JsonModel()
            {
                data = null,
                Message = StatusMessage.Delete,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel GetVideoAndArticlesCategoriesByID(int Id, TokenModel token)
        {

            var  response = _videoAndArticlesCategoriesRepository.GetVideoAndArticlesCategoriesByID(Id, token);
           
            return new JsonModel()
            {
                data = response,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        public JsonModel GetAllVideoAndArticlesCategories(VideoAndArticlesCategoriesFilterModel searchFilterModel, TokenModel tokenModel)
        {

            List<VideoAndArticlesCategoriesModel> videoCategoryModel = _videoAndArticlesCategoriesRepository.GetAllVideoAndArticlesCategories<VideoAndArticlesCategoriesModel>(searchFilterModel, tokenModel).ToList();

            if (videoCategoryModel != null && videoCategoryModel.Count > 0)
            {
                response = new JsonModel(videoCategoryModel, StatusMessage.FetchMessage, (int)System.Net.HttpStatusCode.OK);
                response.meta = new Meta(videoCategoryModel, searchFilterModel);
            }
            else
            {
                response = new JsonModel();
                response.data = null;
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCodes.OK;
            }
            return response; 
        }
        #endregion
        #region VideoAndArticles
        public JsonModel AddUpdateVideoAndArticles(VideoAndArticlesModel videoAndArticlesModel,TokenModel token)
        {
            
            response = new JsonModel();
            string _dataExists = "";
            if (videoAndArticlesModel.ID == 0) {           
              _dataExists = _videoAndArticlesRepository.CheckExistingVideoArticle(videoAndArticlesModel.Title, videoAndArticlesModel.IsVideo, token);
            }
            if (_dataExists != "")
            {
                response.data = null;
                response.Message = StatusMessage.TitleAlreadyExists;
                response.StatusCode = (int)HttpStatusCodes.BadRequest;
            }
            else
            {
                if(videoAndArticlesModel.ID > 0)
                {                    
                    if (!string.IsNullOrEmpty(videoAndArticlesModel.ImageBase64))
                        videoAndArticlesModel.Image = videoAndArticlesModel.ImageBase64.Contains("http") ? videoAndArticlesModel.Image : _imageService.SaveImages(videoAndArticlesModel.ImageBase64, ImagesPath.VideoAndArticles, ImagesFolderEnum.VideoAndArticle.ToString());
                    else
                        videoAndArticlesModel.Image = string.Empty;                    
                }
                else
                {
                    string imageUrl;
                    if (!string.IsNullOrEmpty(videoAndArticlesModel.ImageBase64))               
                         imageUrl = _imageService.SaveImages(videoAndArticlesModel.ImageBase64, ImagesPath.VideoAndArticles, ImagesFolderEnum.VideoAndArticle.ToString());
                    else
                        imageUrl = string.Empty;

                    videoAndArticlesModel.Image = imageUrl;
                }
                string message = _videoAndArticlesRepository.AddUpdateVideoAndArticles(AutoMapper.Mapper.Map<VideoAndArticles>(videoAndArticlesModel), token);
                response.data = videoAndArticlesModel;
                response.StatusCode = (int)HttpStatusCodes.OK;
                response.Message = message;
            }
            return response;
        }
        public JsonModel DeleteVideoAndArticles(int Id, TokenModel token)
        {

            _videoAndArticlesRepository.DeleteVideoAndArticles(Id, token);
            return new JsonModel()
            {
                data = null,
                Message = StatusMessage.Delete,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel GetVideoAndArticleByID(int Id, TokenModel token)
        {
            VideoAndArticlesModel response = AutoMapper.Mapper.Map<VideoAndArticlesModel>( _videoAndArticlesRepository.GetVideoAndArticlesByID(Id, token));          
            if (!string.IsNullOrEmpty(response.Image))
            {
                if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.VideoAndArticles + "VideoAndArticle//" + response.Image))
                    response.ImageBase64 = CommonMethods.CreateImageUrl(token.Request, ImagesPath.VideoAndArticles + "VideoAndArticle//", response.Image); //contextAccessor.HttpContext.Request.Scheme+"://"+ contextAccessor.HttpContext.Request.Host + ImagesPath.OrganizationImages + "Logo\\"+  organisationList.Logo;
                else
                    response.ImageBase64 = string.Empty;
            }
            else
            {
                response.Image = string.Empty;
            }
            return new JsonModel()
            {
                data = response,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        public JsonModel GetAllVideoAndArticles(VideoAndArticlesCategoriesFilterModel searchFilterModel, TokenModel tokenModel)
        {            
            List<VideoAndArticlesModel> videoArticleModelList = _videoAndArticlesRepository.GetAllVideoAndArticles<VideoAndArticlesModel>(searchFilterModel, tokenModel).ToList();

            foreach (VideoAndArticlesModel obj in videoArticleModelList)
            {
                if (!string.IsNullOrEmpty(obj.Image))
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.VideoAndArticles + "VideoAndArticle//" + obj.Image))
                        obj.ImageBase64 = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.VideoAndArticles + "VideoAndArticle//", obj.Image); //contextAccessor.HttpContext.Request.Scheme+"://"+ contextAccessor.HttpContext.Request.Host + ImagesPath.OrganizationImages + "Logo\\"+  organisationList.Logo;
                    else
                        obj.ImageBase64 = string.Empty;
                }
                else
                {
                    obj.ImageBase64 = string.Empty;
                }
            }
            if (videoArticleModelList != null && videoArticleModelList.Count > 0)
            {
                response = new JsonModel(videoArticleModelList, StatusMessage.FetchMessage, (int)System.Net.HttpStatusCode.OK);
                response.meta = new Meta(videoArticleModelList, searchFilterModel);
            }
            else
            {
                response = new JsonModel();
                response.data = null;
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCodes.OK;
            }
            return response;
        }
        #endregion

    }
}
