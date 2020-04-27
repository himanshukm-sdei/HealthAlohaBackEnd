using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.VideoAndArticle
{
    public interface IVideoAndArticlesCategoriesRepository : IRepositoryBase<VideoAndArticlesCategories>
    {
        #region VideoAndArticlesCategoriesRepository
        string AddUpdateVideoAndArticleCategories(VideoAndArticlesCategories VideoAndArticlesCategoriesObj, TokenModel tokenModel);
        VideoAndArticlesCategories CheckExistingVideoArticleCategory(VideoAndArticlesCategories VideoAndArticlesCategoriesObj, TokenModel tokenModel);
        VideoAndArticlesCategories GetVideoAndArticlesCategoriesByID(int CategoryID, TokenModel tokenModel);
        int DeleteVideoAndArticlesCategories(int CategoryID, TokenModel tokenModel);
        IQueryable<T> GetAllVideoAndArticlesCategories<T>(VideoAndArticlesCategoriesFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
        #endregion
    }
}
