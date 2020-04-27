using HC.Model;
using HC.Patient.Model.MasterQuestionnaire;
using HC.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HC.Patient.Service.IServices.MasterQuestionnaire
{
    public interface IMasterQuestionnaireService : IBaseService
    {
        //Category
        JsonModel GetCategories(CommonFilterModel categoryFilterModel, TokenModel tokenModel);
        JsonModel SaveCategory(MasterCategoryModel categoryModel, TokenModel tokenModel);
        JsonModel DeleteCategory(int id, TokenModel tokenModel);
        JsonModel GetCategoryById(int id, TokenModel tokenModel);

        //Category Codes
        JsonModel GetCategoryCodes(CategoryCodesFilterModel categoryCodesFilterModel, TokenModel tokenModel);
        JsonModel SaveCategoryCodes(MasterCategoryCodeModel categoryCodeModel, TokenModel tokenModel);
        JsonModel GetCategoryCodeById(int id, TokenModel tokenModel, IHttpContextAccessor contextAccessor);
        JsonModel DeleteCategoryCode(int id, TokenModel tokenModel);

        //Documents
        JsonModel GetDocuments(CommonFilterModel commonFilterModel, TokenModel tokenModel);
        JsonModel GetDocumentById(int id, TokenModel tokenModel);
        JsonModel SaveDocument(MasterQuestionnaireDocumentModel masterQuestionnaireDocumentModel, TokenModel tokenModel);
        JsonModel DeleteDocument(int id, TokenModel tokenModel);

        //Sections
        JsonModel GetSections(SectionFilterModel sectionFilterModel, TokenModel tokenModel);
        JsonModel SaveSection(MasterQuestionnaireSectionModel questionnaireSectionModel, TokenModel tokenModel);
        JsonModel GetSectionById(int id, TokenModel tokenModel);
        JsonModel DeleteSection(int id, TokenModel tokenModel);

        //Section Items
        JsonModel SaveSectionItem(MasterQuestionnaireSectionItemModel questionnaireSectionItemModel, TokenModel tokenModel);
        JsonModel GetSectionItem(MasterSectionFilterModel sectionFilterModel, TokenModel tokenModel);
        JsonModel GetSectionItemsForForm(int DocumentId, TokenModel tokenModel);
        JsonModel GetSectionItemDDValues(MasterSectionFilterModel sectionFilterModel, TokenModel tokenModel);
        JsonModel GetSectionItemById(int id, TokenModel tokenModel, IHttpContextAccessor contextAccessor);
        JsonModel DeleteSectionItem(int id, TokenModel tokenModel);

        // Get Questionnaire type
        JsonModel GetQuestionnaireTypes(TokenModel tokenModel);

    }

}
