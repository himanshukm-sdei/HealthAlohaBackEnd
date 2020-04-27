using HC.Model;
using HC.Patient.Model.VideoAndArticle;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.VideoAndArticle
{
    public interface IVideoAndArticlesService : IBaseService
    {
        #region VideoAndArticlesCategories
        JsonModel AddUpdateVideoAndArticlesCategories(VideoAndArticlesCategoriesModel videoAndArticlesCategoriesModel, TokenModel token);
        JsonModel DeleteVideoAndArticlesCategories(int Id, TokenModel token);
        JsonModel GetVideoAndArticlesCategoriesByID(int Id, TokenModel token);
        JsonModel GetAllVideoAndArticlesCategories(VideoAndArticlesCategoriesFilterModel searchFilterModel, TokenModel tokenModel);
        #endregion
        #region VideoAndArticles
        JsonModel AddUpdateVideoAndArticles(VideoAndArticlesModel videoAndArticlesModel, TokenModel token);
        JsonModel DeleteVideoAndArticles(int Id, TokenModel token);
        JsonModel GetVideoAndArticleByID(int Id, TokenModel token);
        JsonModel GetAllVideoAndArticles(VideoAndArticlesCategoriesFilterModel searchFilterModel, TokenModel tokenModel);
        #endregion
    }
}
