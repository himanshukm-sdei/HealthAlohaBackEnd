using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.VideoAndArticle;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.VideoAndArticle
{
    public class VideoAndArticlesCategoriesRepository : RepositoryBase<VideoAndArticlesCategories>, IVideoAndArticlesCategoriesRepository
    {
        #region Constructor
        private HCOrganizationContext _context;
        public VideoAndArticlesCategoriesRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }
        #endregion
        #region VideoAndArticleCategories
        public string AddUpdateVideoAndArticleCategories(VideoAndArticlesCategories VideoAndArticlesCategoriesObj, TokenModel tokenModel)
        {
            string isSuccess = "";
            try
            {
                if (VideoAndArticlesCategoriesObj.CategoryID > 0)
                {
                    VideoAndArticlesCategoriesObj.OrganizationID = tokenModel.OrganizationID;
                    VideoAndArticlesCategoriesObj.UpdatedBy = tokenModel.UserID;
                    VideoAndArticlesCategoriesObj.UpdatedDate = DateTime.Now;
                    _context.VideoAndArticlesCategories.Update(VideoAndArticlesCategoriesObj);
                    isSuccess = "Updated Successfully";
                }
                else
                {

                    VideoAndArticlesCategoriesObj.CreatedBy = tokenModel.UserID;
                    VideoAndArticlesCategoriesObj.CreatedDate = DateTime.Now;
                    VideoAndArticlesCategoriesObj.OrganizationID = tokenModel.OrganizationID;
                    VideoAndArticlesCategoriesObj.IsActive = true;
                    VideoAndArticlesCategoriesObj.IsDeleted = false;
                    VideoAndArticlesCategoriesObj.DeletedDate = null;
                    VideoAndArticlesCategoriesObj.UpdatedDate = null;
                    _context.VideoAndArticlesCategories.Add(VideoAndArticlesCategoriesObj);
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
        public VideoAndArticlesCategories CheckExistingVideoArticleCategory(VideoAndArticlesCategories VideoAndArticlesCategoriesObj, TokenModel tokenModel)
        {
            var category = _context.VideoAndArticlesCategories.Where(m => m.CategoryName.ToLower() == VideoAndArticlesCategoriesObj.CategoryName.ToLower() 
            && m.IsActive == true && m.IsDeleted == false && m.OrganizationID == tokenModel.OrganizationID).FirstOrDefault();
            return category;

        }
        public VideoAndArticlesCategories GetVideoAndArticlesCategoriesByID(int CategoryID, TokenModel tokenModel)
        {
            var category = _context.VideoAndArticlesCategories.Where(m => m.CategoryID == CategoryID
            && m.IsActive == true && m.IsDeleted == false && m.OrganizationID == tokenModel.OrganizationID).FirstOrDefault();
            return category;
        }
        public int DeleteVideoAndArticlesCategories(int CategoryID, TokenModel tokenModel)
        {
            try
            {
                var VideoAndArticlesVAR = _context.VideoAndArticlesCategories.Where(m => (m.CategoryID == CategoryID)).FirstOrDefault();
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
        public IQueryable<T> GetAllVideoAndArticlesCategories<T>(VideoAndArticlesCategoriesFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText),
                new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize",searchFilterModel.pageSize),
                new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
                return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.Video_GetVideoAndArticlesCategories, parameters.Length, parameters).AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
