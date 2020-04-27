using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.VideoAndArticle;
using HC.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.VideoAndArticle
{
    public class VideoAndArticlesRepository : RepositoryBase<VideoAndArticles>, IVideoAndArticlesRepository
    {
        #region Constructor
        private HCOrganizationContext _context;
        public VideoAndArticlesRepository(HCOrganizationContext context) : base (context)
        {
            _context = context;
        }
        #endregion

        #region VideoAndArticles
        public string AddUpdateVideoAndArticles(VideoAndArticles VideoAndArticlesObj, TokenModel tokenModel)
        {
            string isSuccess = "";
            try
            {
                if (VideoAndArticlesObj.ID > 0)
                {
                    VideoAndArticlesObj.OrganizationID = tokenModel.OrganizationID;
                    VideoAndArticlesObj.UpdatedBy = tokenModel.UserID;
                    VideoAndArticlesObj.UpdatedDate = DateTime.Now;
                    _context.VideoAndArticles.Update(VideoAndArticlesObj);
                    isSuccess = "Updated Successfully";
                }
                else
                {

                    VideoAndArticlesObj.CreatedBy = tokenModel.UserID;
                    VideoAndArticlesObj.CreatedDate = DateTime.Now;
                    VideoAndArticlesObj.OrganizationID = tokenModel.OrganizationID;
                    VideoAndArticlesObj.IsActive = true;
                    VideoAndArticlesObj.IsDeleted = false;
                    VideoAndArticlesObj.DeletedDate = null;
                    VideoAndArticlesObj.UpdatedDate = null;
                    _context.VideoAndArticles.Add(VideoAndArticlesObj);
                    isSuccess = "Inserted Successfull";
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                isSuccess = "Something went wrong. Try again later";
                throw;
            }

            return isSuccess;
        }
        public string CheckExistingVideoArticle(string Title, bool IsVideo, TokenModel tokenModel)
        {
            string IsExists;
            var videoVAR = _context.VideoAndArticles.Where(m => m.Title == Title && m.IsVideo == true
            && m.IsActive == true && m.IsDeleted == false && m.OrganizationID == tokenModel.OrganizationID).FirstOrDefault();

            var articleVAR = _context.VideoAndArticles.Where(m => m.Title == Title && m.IsVideo == false
            && m.IsActive == true && m.IsDeleted == false && m.OrganizationID == tokenModel.OrganizationID).FirstOrDefault();

            if (videoVAR != null && articleVAR != null)
            {
                IsExists = "Both";
            }
            else if (videoVAR != null && articleVAR == null)
            {
                if (IsVideo == true)
                {
                    IsExists = "Video";
                }
                else
                {
                    IsExists = "";
                }
               
            }
            else if (videoVAR == null && articleVAR != null)
            {
                if (IsVideo == false)
                {
                    IsExists = "Article";
                }
                else
                {
                    IsExists = "";
                }
               
            }
            else
            {
                IsExists = "";
            }

            return IsExists;
        }

        public VideoAndArticles GetVideoAndArticlesByID(int Id,TokenModel tokenModel)
        {
            var category = _context.VideoAndArticles.Where(m => m.ID == Id
            && m.IsActive == true && m.IsDeleted == false && m.OrganizationID == tokenModel.OrganizationID).FirstOrDefault();
            return category;
        }
        public int DeleteVideoAndArticles(int ID, TokenModel tokenModel)
        {
            try
            {
                var VideoAndArticlesVAR = _context.VideoAndArticles.Where(m => (m.ID == ID)).FirstOrDefault();
                VideoAndArticlesVAR.IsDeleted = true;
                VideoAndArticlesVAR.DeletedBy = tokenModel.UserID;
                VideoAndArticlesVAR.DeletedDate = DateTime.Now;
                _context.Update(VideoAndArticlesVAR);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IQueryable<T> GetAllVideoAndArticles<T>(VideoAndArticlesCategoriesFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText),
                new SqlParameter("@Description",searchFilterModel.Description),
                new SqlParameter("@CategoryID",searchFilterModel.ID),
                new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize",searchFilterModel.pageSize),
                new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
                return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.Video_GetVideoAndArticles, parameters.Length, parameters).AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
