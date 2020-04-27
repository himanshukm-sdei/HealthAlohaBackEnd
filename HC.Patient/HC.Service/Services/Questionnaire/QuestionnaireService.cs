using AutoMapper;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Questionnaire;
using HC.Patient.Repositories.IRepositories.Questionnaire;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Patient.Service.IServices.Questionnaire;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Questionnaire
{
    public class QuestionnaireService : BaseService, IQuestionnaireService
    {
        #region Constructor method
        private JsonModel response = null;
        private readonly IQuestionnaireCategoryRepository _questionnaireCategoryRepository;
        private readonly IQuestionnaireCategoryCodeRepository _questionnaireCategoryCodeRepository;
        private readonly IQuestionnaireDocumentRepository _questionnaireDocumentRepository;
        private readonly IQuestionnaireSectionRepository _questionnaireSectionRepository;
        private readonly IQuestionnaireSectionItemRepository _questionnaireSectionItemRepository;
        private readonly IDocumentAnswerRepository _documentAnswerRepository;
        private readonly IPatientDocumentsRepository _patientDocumentsRepository;
        private readonly IGlobalCodeService _globalCodeService;
        public QuestionnaireService(IQuestionnaireCategoryRepository questionnaireCategoryRepository, IQuestionnaireCategoryCodeRepository questionnaireCategoryCodeRepository, IQuestionnaireDocumentRepository questionnaireDocumentRepository, IQuestionnaireSectionRepository questionnaireSectionRepository, IQuestionnaireSectionItemRepository questionnaireSectionItemRepository, IDocumentAnswerRepository documentAnswerRepository, IPatientDocumentsRepository patientDocumentsRepository, IGlobalCodeService globalCodeService)
        {
            response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            _questionnaireCategoryRepository = questionnaireCategoryRepository;
            _questionnaireCategoryCodeRepository = questionnaireCategoryCodeRepository;
            _questionnaireDocumentRepository = questionnaireDocumentRepository;
            _questionnaireSectionRepository = questionnaireSectionRepository;
            _questionnaireSectionItemRepository = questionnaireSectionItemRepository;
            _documentAnswerRepository = documentAnswerRepository;
            _patientDocumentsRepository = patientDocumentsRepository;
            _globalCodeService = globalCodeService;
        }
        #endregion

        #region Categories
        public JsonModel GetCategories(CommonFilterModel categoryFilterModel, TokenModel tokenModel)
        {
            List<CategoryModel> categoryModels = _questionnaireCategoryRepository.GetCategories<CategoryModel>(categoryFilterModel, tokenModel).ToList();
            if (categoryModels != null && categoryModels.Count > 0)
            {
                response = new JsonModel(categoryModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(categoryModels, categoryFilterModel);
            }
            return response;
        }
        public JsonModel SaveCategory(CategoryModel categoryModel, TokenModel tokenModel)
        {
            DFA_Category dFA_Category = null;
            if (categoryModel.Id == 0)
            {
                dFA_Category = _questionnaireCategoryRepository.Get(l => l.CategoryName == categoryModel.CategoryName && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (dFA_Category != null)//duplicate check on new insertion
                {
                    response = new JsonModel(new object(), StatusMessage.CategoryAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else // new insert
                {
                    dFA_Category = new DFA_Category();
                    Mapper.Map(categoryModel, dFA_Category);
                    dFA_Category.OrganizationID = tokenModel.OrganizationID;
                    dFA_Category.CreatedBy = tokenModel.UserID;
                    dFA_Category.CreatedDate = DateTime.UtcNow;
                    dFA_Category.IsDeleted = false;
                    dFA_Category.IsActive = true;
                    _questionnaireCategoryRepository.Create(dFA_Category);
                    _questionnaireCategoryRepository.SaveChanges();
                    response = new JsonModel(dFA_Category, StatusMessage.CategorySave, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                dFA_Category = _questionnaireCategoryRepository.Get(l => l.CategoryName == categoryModel.CategoryName && l.Id != categoryModel.Id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (dFA_Category != null) //duplicate check
                {
                    response = new JsonModel(new object(), StatusMessage.CategoryAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else
                {
                    dFA_Category = _questionnaireCategoryRepository.Get(a => a.Id == categoryModel.Id && a.IsDeleted == false && a.IsActive == true);
                    if (dFA_Category != null)
                    {
                        Mapper.Map(categoryModel, dFA_Category);
                        dFA_Category.UpdatedBy = tokenModel.UserID;
                        dFA_Category.UpdatedDate = DateTime.UtcNow;
                        _questionnaireCategoryRepository.Update(dFA_Category);
                        _questionnaireCategoryRepository.SaveChanges();
                        response = new JsonModel(dFA_Category, StatusMessage.CategoryUpdated, (int)HttpStatusCode.OK);
                    }
                }
            }
            return response;
        }
        public JsonModel GetCategoryById(int id, TokenModel tokenModel)
        {
            DFA_Category dFA_Category = _questionnaireCategoryRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (dFA_Category != null)
            {
                CategoryModel categoryModel = new CategoryModel();
                AutoMapper.Mapper.Map(dFA_Category, categoryModel);
                response = new JsonModel(categoryModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel DeleteCategory(int id, TokenModel tokenModel)
        {
            DFA_Category dFA_Category = _questionnaireCategoryRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (dFA_Category != null)
            {
                dFA_Category.IsDeleted = true;
                dFA_Category.DeletedBy = tokenModel.UserID;
                dFA_Category.DeletedDate = DateTime.UtcNow;
                _questionnaireCategoryRepository.Update(dFA_Category);
                _questionnaireCategoryRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.CategoryDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        #endregion

        #region Categories Code
        public JsonModel GetCategoryCodes(CategoryCodesFilterModel categoryCodesFilterModel, TokenModel tokenModel)
        {
            List<CategoryCodeModel> categoryCodeModels = _questionnaireCategoryCodeRepository.GetCategoryCodes<CategoryCodeModel>(categoryCodesFilterModel, tokenModel).ToList();
            if (categoryCodeModels != null && categoryCodeModels.Count > 0)
            {
                response = new JsonModel(categoryCodeModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(categoryCodeModels, categoryCodesFilterModel);
            }
            return response;
        }
        public JsonModel SaveCategoryCodes(CategoryCodeModel categoryCodeModel, TokenModel tokenModel)
        {
            DFA_CategoryCode dFA_CategoryCode = null;
            if (categoryCodeModel.Id == 0)
            {
                dFA_CategoryCode = _questionnaireCategoryCodeRepository.Get(l => l.CodeName == categoryCodeModel.CodeName && l.IsDeleted == false && l.IsActive == true);
                if (dFA_CategoryCode != null)//duplicate check on new insertion
                {
                    response = new JsonModel(new object(), StatusMessage.CategoryCodeAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else // new insert
                {
                    dFA_CategoryCode = new DFA_CategoryCode();
                    Mapper.Map(categoryCodeModel, dFA_CategoryCode);
                    dFA_CategoryCode.CreatedBy = tokenModel.UserID;
                    dFA_CategoryCode.CreatedDate = DateTime.UtcNow;
                    dFA_CategoryCode.IsDeleted = false;
                    dFA_CategoryCode.IsActive = true;
                    _questionnaireCategoryCodeRepository.Create(dFA_CategoryCode);
                    _questionnaireCategoryCodeRepository.SaveChanges();
                    response = new JsonModel(dFA_CategoryCode, StatusMessage.CategoryCodeSave, (int)HttpStatusCode.OK);
                }
            }
            else
            {

                dFA_CategoryCode = _questionnaireCategoryCodeRepository.Get(l => l.CodeName == categoryCodeModel.CodeName && l.Id != categoryCodeModel.Id && l.IsDeleted == false && l.IsActive == true);
                if (dFA_CategoryCode != null) //duplicate check
                {
                    response = new JsonModel(new object(), StatusMessage.CategoryCodeAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else
                {
                    dFA_CategoryCode = _questionnaireCategoryCodeRepository.Get(a => a.Id == categoryCodeModel.Id && a.IsDeleted == false && a.IsActive == true);
                    if (dFA_CategoryCode != null)
                    {
                        Mapper.Map(categoryCodeModel, dFA_CategoryCode);
                        dFA_CategoryCode.UpdatedBy = tokenModel.UserID;
                        dFA_CategoryCode.UpdatedDate = DateTime.UtcNow;
                        _questionnaireCategoryCodeRepository.Update(dFA_CategoryCode);
                        _questionnaireCategoryCodeRepository.SaveChanges();
                        response = new JsonModel(dFA_CategoryCode, StatusMessage.CategoryCodeUpdated, (int)HttpStatusCode.OK);
                    }
                }
            }
            return response;
        }
        public JsonModel GetCategoryCodeById(int id, TokenModel tokenModel)
        {
            DFA_CategoryCode dFA_CategoryCode = _questionnaireCategoryCodeRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (dFA_CategoryCode != null)
            {
                CategoryCodeModel categoryCodeModel = new CategoryCodeModel();
                AutoMapper.Mapper.Map(dFA_CategoryCode, categoryCodeModel);
                response = new JsonModel(categoryCodeModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel DeleteCategoryCode(int id, TokenModel tokenModel)
        {
            DFA_CategoryCode dFA_CategoryCode = _questionnaireCategoryCodeRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (dFA_CategoryCode != null)
            {
                dFA_CategoryCode.IsDeleted = true;
                dFA_CategoryCode.DeletedBy = tokenModel.UserID;
                dFA_CategoryCode.DeletedDate = DateTime.UtcNow;
                _questionnaireCategoryCodeRepository.Update(dFA_CategoryCode);
                _questionnaireCategoryCodeRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.CategoryCodeDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        #endregion

        #region Documents/Form
        public JsonModel GetDocuments(CommonFilterModel commonFilterModel, TokenModel tokenModel)
        {
            List<QuestionnaireDocumentModel> questionnaireDocumentModels = _questionnaireDocumentRepository.GetDocuments<QuestionnaireDocumentModel>(commonFilterModel, tokenModel).ToList();
            if (questionnaireDocumentModels != null && questionnaireDocumentModels.Count > 0)
            {
                response = new JsonModel(questionnaireDocumentModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(questionnaireDocumentModels, commonFilterModel);
            }
            return response;
        }
        public JsonModel SaveDocument(QuestionnaireDocumentModel questionnaireDocumentModel, TokenModel tokenModel)
        {
            DFA_Document dFA_Document = null;
            if (questionnaireDocumentModel.Id == 0)
            {
                dFA_Document = _questionnaireDocumentRepository.Get(l => l.DocumentName == questionnaireDocumentModel.DocumentName && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (dFA_Document != null)//duplicate check on new insertion
                {
                    response = new JsonModel(new object(), StatusMessage.DocumentAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else // new insert
                {
                    dFA_Document = new DFA_Document();
                    Mapper.Map(questionnaireDocumentModel, dFA_Document);
                    dFA_Document.OrganizationID = tokenModel.OrganizationID;
                    dFA_Document.CreatedBy = tokenModel.UserID;
                    dFA_Document.CreatedDate = DateTime.UtcNow;
                    dFA_Document.IsDeleted = false;
                    dFA_Document.IsActive = true;
                    _questionnaireDocumentRepository.Create(dFA_Document);
                    _questionnaireDocumentRepository.SaveChanges();
                    response = new JsonModel(dFA_Document, StatusMessage.DocumentSave, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                dFA_Document = _questionnaireDocumentRepository.Get(l => l.DocumentName == questionnaireDocumentModel.DocumentName && l.Id != questionnaireDocumentModel.Id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (dFA_Document != null) //duplicate check
                {
                    response = new JsonModel(new object(), StatusMessage.DocumentAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else
                {
                    dFA_Document = _questionnaireDocumentRepository.Get(a => a.Id == questionnaireDocumentModel.Id && a.IsDeleted == false && a.IsActive == true && a.OrganizationID == tokenModel.OrganizationID);
                    if (dFA_Document != null)
                    {
                        Mapper.Map(questionnaireDocumentModel, dFA_Document);
                        dFA_Document.UpdatedBy = tokenModel.UserID;
                        dFA_Document.UpdatedDate = DateTime.UtcNow;
                        _questionnaireDocumentRepository.Update(dFA_Document);
                        _questionnaireDocumentRepository.SaveChanges();
                        response = new JsonModel(dFA_Document, StatusMessage.DocumentUpdated, (int)HttpStatusCode.OK);
                    }
                }
            }
            return response;
        }
        public JsonModel GetDocumentById(int id, TokenModel tokenModel)
        {
            DFA_Document dFA_Document = _questionnaireDocumentRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (dFA_Document != null)
            {
                QuestionnaireDocumentModel questionnaireDocumentModel = new QuestionnaireDocumentModel();
                AutoMapper.Mapper.Map(dFA_Document, questionnaireDocumentModel);
                response = new JsonModel(dFA_Document, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel DeleteDocument(int id, TokenModel tokenModel)
        {
            DFA_Document dFA_Document = _questionnaireDocumentRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (dFA_Document != null)
            {
                dFA_Document.IsDeleted = true;
                dFA_Document.DeletedBy = tokenModel.UserID;
                dFA_Document.DeletedDate = DateTime.UtcNow;
                _questionnaireDocumentRepository.Update(dFA_Document);
                _questionnaireCategoryRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.DocumentDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        #endregion

        #region Section
        public JsonModel GetSections(SectionFilterModel sectionFilterModel, TokenModel tokenModel)
        {
            List<QuestionnaireSectionModel> questionnaireSectionModels = _questionnaireSectionRepository.GetSections<QuestionnaireSectionModel>(sectionFilterModel, tokenModel).ToList();
            if (questionnaireSectionModels != null && questionnaireSectionModels.Count > 0)
            {
                response = new JsonModel(questionnaireSectionModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(questionnaireSectionModels, sectionFilterModel);
            }
            return response;
        }
        public JsonModel SaveSection(QuestionnaireSectionModel questionnaireSectionModel, TokenModel tokenModel)
        {
            DFA_Section dFA_Section = null;
            if (questionnaireSectionModel.Id == 0)
            {
                dFA_Section = new DFA_Section();
                Mapper.Map(questionnaireSectionModel, dFA_Section);
                dFA_Section.CreatedBy = tokenModel.UserID;
                dFA_Section.CreatedDate = DateTime.UtcNow;
                dFA_Section.IsDeleted = false;
                dFA_Section.IsActive = true;
                _questionnaireSectionRepository.Create(dFA_Section);
                _questionnaireSectionRepository.SaveChanges();
                response = new JsonModel(dFA_Section, StatusMessage.SectionSave, (int)HttpStatusCode.OK);
            }
            else
            {

                dFA_Section = _questionnaireSectionRepository.Get(a => a.Id == questionnaireSectionModel.Id && a.IsDeleted == false && a.IsActive == true);
                if (dFA_Section != null)
                {
                    Mapper.Map(questionnaireSectionModel, dFA_Section);
                    dFA_Section.UpdatedBy = tokenModel.UserID;
                    dFA_Section.UpdatedDate = DateTime.UtcNow;
                    _questionnaireSectionRepository.Update(dFA_Section);
                    _questionnaireSectionRepository.SaveChanges();
                    response = new JsonModel(dFA_Section, StatusMessage.SectionUpdated, (int)HttpStatusCode.OK);
                }
            }
            return response;
        }
        public JsonModel GetSectionById(int id, TokenModel tokenModel)
        {
            DFA_Section dFA_Section = _questionnaireSectionRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (dFA_Section != null)
            {
                QuestionnaireSectionModel questionnaireSectionModel = new QuestionnaireSectionModel();
                AutoMapper.Mapper.Map(dFA_Section, questionnaireSectionModel);
                response = new JsonModel(questionnaireSectionModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel DeleteSection(int id, TokenModel tokenModel)
        {
            DFA_Section dFA_Section = _questionnaireSectionRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (dFA_Section != null)
            {
                dFA_Section.IsDeleted = true;
                dFA_Section.DeletedBy = tokenModel.UserID;
                dFA_Section.DeletedDate = DateTime.UtcNow;
                _questionnaireSectionRepository.Update(dFA_Section);
                _questionnaireSectionRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.SectionDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        #endregion

        #region Section Item
        public JsonModel SaveSectionItem(QuestionnaireSectionItemModel questionnaireSectionItemModel, TokenModel tokenModel)
        {
            DFA_SectionItem dFA_SectionItem = null;
            if (questionnaireSectionItemModel.Id == 0)
            {
                dFA_SectionItem = new DFA_SectionItem();
                Mapper.Map(questionnaireSectionItemModel, dFA_SectionItem);
                dFA_SectionItem.CreatedBy = tokenModel.UserID;
                dFA_SectionItem.CreatedDate = DateTime.UtcNow;
                dFA_SectionItem.IsDeleted = false;
                dFA_SectionItem.IsActive = true;
                _questionnaireSectionItemRepository.Create(dFA_SectionItem);
                _questionnaireSectionRepository.SaveChanges();
                response = new JsonModel(dFA_SectionItem, StatusMessage.SectionItemSave, (int)HttpStatusCode.OK);
            }
            else
            {

                dFA_SectionItem = _questionnaireSectionItemRepository.Get(a => a.Id == questionnaireSectionItemModel.Id && a.IsDeleted == false && a.IsActive == true);
                if (dFA_SectionItem != null)
                {
                    Mapper.Map(questionnaireSectionItemModel, dFA_SectionItem);
                    dFA_SectionItem.UpdatedBy = tokenModel.UserID;
                    dFA_SectionItem.UpdatedDate = DateTime.UtcNow;
                    _questionnaireSectionItemRepository.Update(dFA_SectionItem);
                    _questionnaireSectionItemRepository.SaveChanges();
                    response = new JsonModel(dFA_SectionItem, StatusMessage.SectionItemUpdated, (int)HttpStatusCode.OK);
                }
            }
            return response;
        }
        public JsonModel GetSectionItem(SectionFilterModel sectionFilterModel, TokenModel tokenModel)
        {
            SectionItemlistingModel sectionItemlistingModel = _questionnaireSectionItemRepository.GetSectionItems(sectionFilterModel, tokenModel);
            if (sectionItemlistingModel != null)
            {
                response = new JsonModel(sectionItemlistingModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(sectionItemlistingModel.SectionItems, sectionFilterModel);
            }
            return response;
        }

        public JsonModel GetSectionItemById(int id, TokenModel tokenModel)
        {
            QuestionnaireSectionItemModel questionnaireSectionItemModel = _questionnaireSectionItemRepository.GetSectionItemsByID<QuestionnaireSectionItemModel>(id, tokenModel).FirstOrDefault();
            if (questionnaireSectionItemModel != null)
            {
                response = new JsonModel(questionnaireSectionItemModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel GetSectionItemDDValues(SectionFilterModel sectionFilterModel, TokenModel tokenModel)
        {
            SectionItemDDValueModel sectionItemDDValueModel = _questionnaireSectionItemRepository.GetSectionItemDDValues(sectionFilterModel, tokenModel);
            if (sectionItemDDValueModel != null)
            {
                response = new JsonModel(sectionItemDDValueModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }
        #endregion

        #region Patient Question Answer 
        public JsonModel SavePatientDocumentAnswer(AnswersModel answersModel, TokenModel tokenModel)
        {
            XElement questionAnswerXML = null;
            if (answersModel.PatientID > 0 && answersModel.DocumentId > 0 && answersModel.Answer != null && answersModel.Answer.Count > 0)
            {
                //create xml element
                questionAnswerXML = new XElement("Parent", answersModel.Answer.Select((c, index) => new XElement("Child",
                            new XElement("AnswerId", c.AnswerId),
                            new XElement("Id", c.Id),
                            new XElement("SectionItemId", c.SectionItemId),
                            new XElement("TextAnswer", c.TextAnswer))));

                //reverse assignment xml element into answer model
                answersModel.questionAnswerXML = questionAnswerXML;

                //save request
                SQLResponseModel answerResponse = _documentAnswerRepository.SaveQuestionAnswer<SQLResponseModel>(answersModel, tokenModel).FirstOrDefault();

                //response
                response = new JsonModel()
                {
                    data = new object(),
                    Message = answerResponse.Message,
                    StatusCode = answerResponse.StatusCode
                };
            }
            else
            {
                response = new JsonModel(null, StatusMessage.QuestionnaireAnswerInvalidData, (int)HttpStatusCode.BadRequest);
            }
            return response;
        }

        public JsonModel GetPatientDocumentAnswer(PatientDocumentAnswerFilterModel patientDocumentFilterModel, TokenModel tokenModel)
        {
            AnswersModel answersModel = _documentAnswerRepository.GetPatientAnswer(patientDocumentFilterModel, tokenModel);
            if (answersModel != null)
            {
                answersModel.PatientID = patientDocumentFilterModel.PatientId;
                answersModel.DocumentId = patientDocumentFilterModel.DocumentId;
                response = new JsonModel(answersModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }

        public JsonModel GetPatientDocuments(PatientDocumentFilterModel patientDocumentFilterModel, TokenModel tokenModel)
        {
            List<PatientDocumentModel> patientDocumentFilterModels = _patientDocumentsRepository.GetPatientDocuments<PatientDocumentModel>(patientDocumentFilterModel, tokenModel).ToList();
            if (patientDocumentFilterModels != null && patientDocumentFilterModels.Count > 0)
            {
                response = new JsonModel(patientDocumentFilterModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(patientDocumentFilterModels, patientDocumentFilterModel);

            }
            return response;
        }

        public JsonModel AssignDocumentToPatient(AssignDocumentToPatientModel assignDocumentToPatientModel, TokenModel tokenModel)
        {
            DFA_PatientDocuments dFA_PatientDocuments = null;
            if (assignDocumentToPatientModel.Id == 0)
            {
                dFA_PatientDocuments = _patientDocumentsRepository.Get(a => a.PatientId == assignDocumentToPatientModel.PatientId && a.DocumentId == assignDocumentToPatientModel.DocumentId && a.OrganizationID == tokenModel.OrganizationID);
                if (dFA_PatientDocuments == null) // check doument not repeatly assignto same patient
                {
                    dFA_PatientDocuments = new DFA_PatientDocuments();
                    Mapper.Map(assignDocumentToPatientModel, dFA_PatientDocuments);
                    dFA_PatientDocuments = new DFA_PatientDocuments
                    {
                        AssignedBy = assignDocumentToPatientModel.AssignedBy,
                        PatientId = assignDocumentToPatientModel.PatientId,
                        DocumentId = assignDocumentToPatientModel.DocumentId,
                        OrganizationID = tokenModel.OrganizationID,
                        Status = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.DocumentStatus, DocumentStatus.ToDo, tokenModel),
                        CreatedBy = tokenModel.UserID,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true,
                        IsDeleted = false,

                    };
                    _patientDocumentsRepository.Create(dFA_PatientDocuments);
                    _patientDocumentsRepository.SaveChanges();
                    response = new JsonModel(dFA_PatientDocuments, StatusMessage.QuestionnaireAssignment, (int)HttpStatusCode.OK);
                }
                else
                {
                    response = new JsonModel(null, StatusMessage.AlreadyAssigned, (int)HttpStatusCode.NotModified);
                }

            }
            else
            {
                dFA_PatientDocuments = _patientDocumentsRepository.Get(a => a.Id == assignDocumentToPatientModel.Id && a.OrganizationID == tokenModel.OrganizationID && a.IsActive == true && a.IsDeleted == false);
                if (dFA_PatientDocuments != null)
                {
                    Mapper.Map(assignDocumentToPatientModel, dFA_PatientDocuments);
                    dFA_PatientDocuments.UpdatedBy = tokenModel.UserID;
                    dFA_PatientDocuments.UpdatedDate = DateTime.UtcNow;
                    _patientDocumentsRepository.Update(dFA_PatientDocuments);
                    _patientDocumentsRepository.SaveChanges();
                    response = new JsonModel(dFA_PatientDocuments, StatusMessage.QuestionnaireAssignmentUpdated, (int)HttpStatusCode.OK);
                }
            }
            return response;
        }

        public JsonModel GetPatientDocumentById(int id, TokenModel tokenModel)
        {
            DFA_PatientDocuments dFA_PatientDocuments = _patientDocumentsRepository.Get(a => a.Id == id && a.OrganizationID == tokenModel.OrganizationID && a.IsActive == true && a.IsDeleted == false);
            AssignDocumentToPatientModel assignDocumentToPatientModel = null;
            if (dFA_PatientDocuments != null)
            {
                assignDocumentToPatientModel = new AssignDocumentToPatientModel();
                Mapper.Map(dFA_PatientDocuments, assignDocumentToPatientModel);
                response = new JsonModel(assignDocumentToPatientModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }

        public JsonModel UpdateStatus(SaveSignatureModel saveSignatureModel,TokenModel tokenModel)
        {
            DFA_PatientDocuments dFA_PatientDocuments = _patientDocumentsRepository.Get(a => a.Id == saveSignatureModel.PatientDocumentId && a.OrganizationID == tokenModel.OrganizationID && a.IsActive == true && a.IsDeleted == false);
            if (dFA_PatientDocuments != null)
            {                                
                if(saveSignatureModel.PatientSign != null)
                {
                    dFA_PatientDocuments.PatientSign = saveSignatureModel.PatientSign;
                    dFA_PatientDocuments.Status = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.DocumentStatus, DocumentStatus.InProgress, tokenModel);
                    response = new JsonModel(dFA_PatientDocuments, StatusMessage.ClientDocumentSigned, (int)HttpStatusCode.OK);
                    _patientDocumentsRepository.Update(dFA_PatientDocuments);
                    _patientDocumentsRepository.SaveChanges();

                }
                else if (dFA_PatientDocuments.PatientSign!=null && saveSignatureModel.ClinicianSign!=null)
                {
                    dFA_PatientDocuments.ClinicianSign = saveSignatureModel.ClinicianSign;
                    dFA_PatientDocuments.Status = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.DocumentStatus, DocumentStatus.Completed, tokenModel);
                    response = new JsonModel(dFA_PatientDocuments, StatusMessage.StaffDocumentSigned, (int)HttpStatusCode.OK);
                    _patientDocumentsRepository.Update(dFA_PatientDocuments);
                    _patientDocumentsRepository.SaveChanges();
                }
                else
                {
                    response = new JsonModel(null, StatusMessage.ClientSignRequired, (int)HttpStatusCodes.UnprocessedEntity);
                }
               
            }
            return response;
        }
        #endregion
    }
}
