using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.VideoAndArticle
{
    public interface IVideoAndArticlesRepository : IMasterRepositoryBase<VideoAndArticles>
    {
        string AddUpdateVideoAndArticles(VideoAndArticles VideoAndArticlesObj, TokenModel tokenModel);
        string CheckExistingVideoArticle(string Title, bool IsVideo, TokenModel tokenModel);
        int DeleteVideoAndArticles(int ID, TokenModel tokenModel);
        VideoAndArticles GetVideoAndArticlesByID(int Id, TokenModel tokenModel);
        IQueryable<T> GetAllVideoAndArticles<T>(VideoAndArticlesCategoriesFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
