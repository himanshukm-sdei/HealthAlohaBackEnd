using AutoMapper;
using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.MasterQuestionnaire;
using HC.Patient.Model.Questionnaire;
using HC.Patient.Repositories.IRepositories.MasterQuestionnaire;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Patient.Service.IServices.Images;
using HC.Patient.Service.IServices.MasterQuestionnaire;
using HC.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.MasterQuestionnaire
{
    public class MasterQuestionnaireService : BaseService, IMasterQuestionnaireService
    {
        #region Constructor method
        private JsonModel response = null;
        private readonly IMasterQuestionnaireCategoryRepository _masterQuestionnaireCategoryRepository;
        private readonly IMasterQuestionnaireCategoryCodeRepository _masterQuestionnaireCategoryCodeRepository;
        private readonly IMasterQuestionnaireDocumentRepository _masterQuestionnaireDocumentRepository;
        private readonly IMasterQuestionnaireSectionRepository _masterQuestionnaireSectionRepository;
        private readonly IMasterQuestionnaireSectionItemRepository _masterQuestionnaireSectionitemRepository;
        private HCMasterContext _masterContext;
        private IMasterQuestionnaireBenchmarkRangeRepository _masterQuestionnaireBenchmarkRangeRepository;
        private IImageService _imageService;

        public MasterQuestionnaireService(IMasterQuestionnaireDocumentRepository masterQuestionnaireDocumentRepository, HCMasterContext masterContext, IMasterQuestionnaireBenchmarkRangeRepository masterQuestionnaireBenchmarkRangeRepository,
            IMasterQuestionnaireCategoryRepository masterQuestionnaireCategoryRepository, IMasterQuestionnaireCategoryCodeRepository masterQuestionnaireCategoryCodeRepository, IMasterQuestionnaireSectionRepository masterQuestionnaireSectionRepository,
            IMasterQuestionnaireSectionItemRepository masterQuestionnaireSectionitemRepository, IImageService imageService)
        {
            response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            _masterQuestionnaireCategoryRepository = masterQuestionnaireCategoryRepository;
            _masterQuestionnaireCategoryCodeRepository = masterQuestionnaireCategoryCodeRepository;
            _masterQuestionnaireDocumentRepository = masterQuestionnaireDocumentRepository;
            _masterContext = masterContext;
            _masterQuestionnaireBenchmarkRangeRepository = masterQuestionnaireBenchmarkRangeRepository;
            _masterQuestionnaireSectionRepository = masterQuestionnaireSectionRepository;
            _masterQuestionnaireSectionitemRepository = masterQuestionnaireSectionitemRepository;
            _imageService = imageService;

    }
        #endregion

        #region Categories
        public JsonModel GetCategories(CommonFilterModel categoryFilterModel, TokenModel tokenModel)
        {
            List<MasterCategoryModel> categoryModels = _masterQuestionnaireCategoryRepository.GetCategories<MasterCategoryModel>(categoryFilterModel, tokenModel).ToList();
            if (categoryModels != null && categoryModels.Count > 0)
            {
                response = new JsonModel(categoryModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(categoryModels, categoryFilterModel);
            }
            return response;
        }
        public JsonModel SaveCategory(MasterCategoryModel categoryModel, TokenModel tokenModel)
        {
            MasterDFA_Category masterDFA_Category = null;
            List<MasterMappingHRACategoryRisk> mappingHRACategoryList = new List<MasterMappingHRACategoryRisk>();
            if (categoryModel.Id == 0)
            {
                masterDFA_Category = _masterQuestionnaireCategoryRepository.Get(l => l.CategoryName == categoryModel.CategoryName && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (masterDFA_Category != null)//duplicate check on new insertion
                {
                    response = new JsonModel(new object(), StatusMessage.CategoryAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else // new insert
                {
                    masterDFA_Category = new MasterDFA_Category { MappingHRACategoryRisks = new List<MasterMappingHRACategoryRisk>() };
                    masterDFA_Category = new MasterDFA_Category();
                    Mapper.Map(categoryModel, masterDFA_Category);
                    masterDFA_Category.OrganizationID = tokenModel.OrganizationID;
                    masterDFA_Category.CreatedBy = tokenModel.UserID;
                    masterDFA_Category.CreatedDate = DateTime.UtcNow;
                    masterDFA_Category.IsDeleted = false;
                    masterDFA_Category.IsActive = true;

                    #region HRACategoryRisk
                    if (categoryModel.HRACategoryRiskIds != null && categoryModel.HRACategoryRiskIds.Length > 0)
                    {
                        for (int i = 0; i < categoryModel.HRACategoryRiskIds.Length; i++)
                        {
                            MasterMappingHRACategoryRisk mappingHRACategoryRisk = new MasterMappingHRACategoryRisk();
                            mappingHRACategoryRisk.HRACategoryId = masterDFA_Category.Id;
                            mappingHRACategoryRisk.HRACategoryRiskId = categoryModel.HRACategoryRiskIds[i];
                            mappingHRACategoryList.Add(mappingHRACategoryRisk);
                        }
                        masterDFA_Category.MappingHRACategoryRisks = mappingHRACategoryList;
                    }
                    #endregion

                    _masterQuestionnaireCategoryRepository.Save(masterDFA_Category, false);//false means its not updated                    
                    response = new JsonModel(masterDFA_Category, StatusMessage.CategorySave, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                masterDFA_Category = _masterQuestionnaireCategoryRepository.Get(l => l.CategoryName == categoryModel.CategoryName && l.Id != categoryModel.Id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (masterDFA_Category != null) //duplicate check
                {
                    response = new JsonModel(new object(), StatusMessage.CategoryAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else
                {
                    masterDFA_Category = _masterQuestionnaireCategoryRepository.Get(a => a.Id == categoryModel.Id && a.IsDeleted == false && a.IsActive == true);

                    if (masterDFA_Category != null)
                    {

                        Mapper.Map(categoryModel, masterDFA_Category);
                        masterDFA_Category.UpdatedBy = tokenModel.UserID;
                        masterDFA_Category.UpdatedDate = DateTime.UtcNow;

                        #region HRACategoryRisk

                        //delete previous records
                        _masterQuestionnaireCategoryRepository.DeleteMasterHRACategoryRiskMapping(masterDFA_Category.Id);

                        if (categoryModel.HRACategoryRiskIds != null && categoryModel.HRACategoryRiskIds.Length > 0)
                        {
                            masterDFA_Category.MappingHRACategoryRisks = new List<MasterMappingHRACategoryRisk>();
                            for (int i = 0; i < categoryModel.HRACategoryRiskIds.Length; i++)
                            {
                                MasterMappingHRACategoryRisk mappingHRACategoryRisk = new MasterMappingHRACategoryRisk
                                {
                                    HRACategoryId = masterDFA_Category.Id,
                                    HRACategoryRiskId = categoryModel.HRACategoryRiskIds[i]
                                };
                                mappingHRACategoryList.Add(mappingHRACategoryRisk);
                            }
                            masterDFA_Category.MappingHRACategoryRisks = mappingHRACategoryList;
                        }
                        #endregion

                        _masterQuestionnaireCategoryRepository.Save(masterDFA_Category, true);
                        response = new JsonModel(masterDFA_Category, StatusMessage.CategoryUpdated, (int)HttpStatusCode.OK);
                    }
                }
            }
            return response;
        }
        public JsonModel GetCategoryById(int id, TokenModel tokenModel)
        {
            MasterDFA_Category masterDFA_Category = _masterQuestionnaireCategoryRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true && a.OrganizationID == tokenModel.OrganizationID);
            if (masterDFA_Category != null)
            {
                int[] CategoyIds = _masterQuestionnaireCategoryRepository.GetMasterHRACategoryRisk(masterDFA_Category.Id).ToArray();

                MasterCategoryModel masterCategoryModel = new MasterCategoryModel();
                AutoMapper.Mapper.Map(masterDFA_Category, masterCategoryModel);
                masterCategoryModel.HRACategoryRiskIds = CategoyIds;
                response = new JsonModel(masterCategoryModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel DeleteCategory(int id, TokenModel tokenModel)
        {
            MasterDFA_Category masterDFA_Category = _masterQuestionnaireCategoryRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (masterDFA_Category != null)
            {
                masterDFA_Category.IsDeleted = true;
                masterDFA_Category.DeletedBy = tokenModel.UserID;
                masterDFA_Category.DeletedDate = DateTime.UtcNow;
                _masterQuestionnaireCategoryRepository.Update(masterDFA_Category);
                _masterQuestionnaireCategoryRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.CategoryDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        #endregion

        #region Categories Code
        public JsonModel GetCategoryCodes(CategoryCodesFilterModel categoryCodesFilterModel, TokenModel tokenModel)
        {
            List<MasterCategoryCodeModel> categoryCodeModels = _masterQuestionnaireCategoryCodeRepository.GetCategoryCodes<MasterCategoryCodeModel>(categoryCodesFilterModel, tokenModel).ToList();
            if (categoryCodeModels != null && categoryCodeModels.Count > 0)
            {
                response = new JsonModel(categoryCodeModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(categoryCodeModels, categoryCodesFilterModel);
            }
            return response;
        }
        public JsonModel SaveCategoryCodes(MasterCategoryCodeModel masterCategoryCodeModel, TokenModel tokenModel)
        {
            MasterDFA_CategoryCode masterDFA_CategoryCode = new MasterDFA_CategoryCode();
            if (masterCategoryCodeModel.Id == 0)
            {
                ////Duplicate check is removed because to add same category code provision is available (14-10-2019)
                //masterDFA_CategoryCode = _masterQuestionnaireCategoryCodeRepository.Get(l => l.CodeName == masterCategoryCodeModel.CodeName && l.IsDeleted == false && l.IsActive == true);
                //if (dFA_CategoryCode != null)//duplicate check on new insertion
                //{
                //    response = new JsonModel(new object(), StatusMessage.CategoryCodeAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                //}
                //else // new insert
                //{
                //masterDFA_CategoryCode = new MasterDFA_CategoryCode();

                Mapper.Map(masterCategoryCodeModel, masterDFA_CategoryCode);
                //images                
                string LogoUrl = _imageService.SaveImages(masterCategoryCodeModel.PhotoBase64, ImagesPath.MasterQuestionaireImages, ImagesFolderEnum.OptionLogo.ToString());
                masterDFA_CategoryCode.OptionLogo = LogoUrl;
                masterDFA_CategoryCode.CreatedBy = tokenModel.UserID;
                masterDFA_CategoryCode.CreatedDate = DateTime.UtcNow;
                masterDFA_CategoryCode.IsDeleted = false;
                masterDFA_CategoryCode.IsActive = true;
                _masterQuestionnaireCategoryCodeRepository.Create(masterDFA_CategoryCode);
                _masterQuestionnaireCategoryCodeRepository.SaveChanges();
                response = new JsonModel(masterDFA_CategoryCode, StatusMessage.CategoryCodeSave, (int)HttpStatusCode.OK);
                //}
            }
            else
            {
                ////Duplicate check is removed because to add same category code provision is available (14-10-2019)
                //dFA_CategoryCode = _questionnaireCategoryCodeRepository.Get(l => l.CodeName == categoryCodeModel.CodeName && l.Id != categoryCodeModel.Id && l.IsDeleted == false && l.IsActive == true);
                //if (dFA_CategoryCode != null) //duplicate check
                //{
                //    response = new JsonModel(new object(), StatusMessage.CategoryCodeAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                //}
                //else
                //{
                masterDFA_CategoryCode = _masterQuestionnaireCategoryCodeRepository.Get(a => a.Id == masterCategoryCodeModel.Id && a.IsDeleted == false && a.IsActive == true);
                if (masterDFA_CategoryCode != null)
                {
                    Mapper.Map(masterCategoryCodeModel, masterDFA_CategoryCode);
                    //images
                    if (!string.IsNullOrEmpty(masterCategoryCodeModel.PhotoBase64))
                    masterDFA_CategoryCode.OptionLogo = masterCategoryCodeModel.PhotoBase64.Contains("http") ? masterDFA_CategoryCode.OptionLogo : _imageService.SaveImages(masterCategoryCodeModel.PhotoBase64, ImagesPath.MasterQuestionaireImages, ImagesFolderEnum.OptionLogo.ToString());
                    masterDFA_CategoryCode.UpdatedBy = tokenModel.UserID;
                    masterDFA_CategoryCode.UpdatedDate = DateTime.UtcNow;
                    _masterQuestionnaireCategoryCodeRepository.Update(masterDFA_CategoryCode);
                    _masterQuestionnaireCategoryCodeRepository.SaveChanges();
                    response = new JsonModel(masterDFA_CategoryCode, StatusMessage.CategoryCodeUpdated, (int)HttpStatusCode.OK);
                }
                //}
            }
            return response;
        }
        public JsonModel GetCategoryCodeById(int id, TokenModel tokenModel, IHttpContextAccessor contextAccessor)
        {
            MasterDFA_CategoryCode masterDFA_CategoryCode = _masterQuestionnaireCategoryCodeRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (masterDFA_CategoryCode != null)
            {
                MasterCategoryCodeModel masterCategoryCodeModel = new MasterCategoryCodeModel();
                AutoMapper.Mapper.Map(masterDFA_CategoryCode, masterCategoryCodeModel);
                if (!string.IsNullOrEmpty(masterCategoryCodeModel.OptionLogo))
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.MasterQuestionaireImages + "OptionLogo//" + masterCategoryCodeModel.OptionLogo))
                        masterCategoryCodeModel.PhotoBase64 = CommonMethods.CreateImageUrl(contextAccessor.HttpContext, ImagesPath.MasterQuestionaireImages + "OptionLogo//", masterCategoryCodeModel.OptionLogo); //contextAccessor.HttpContext.Request.Scheme+"://"+ contextAccessor.HttpContext.Request.Host + ImagesPath.OrganizationImages + "Logo\\"+  responseOrganizationObj.Logo;
                    else
                        masterCategoryCodeModel.PhotoBase64 = string.Empty;
                }
                else
                {
                    masterCategoryCodeModel.PhotoBase64 = string.Empty;
                }
                response = new JsonModel(masterCategoryCodeModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel DeleteCategoryCode(int id, TokenModel tokenModel)
        {
            MasterDFA_CategoryCode masterDFA_CategoryCode = _masterQuestionnaireCategoryCodeRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (masterDFA_CategoryCode != null)
            {
                masterDFA_CategoryCode.IsDeleted = true;
                masterDFA_CategoryCode.DeletedBy = tokenModel.UserID;
                masterDFA_CategoryCode.DeletedDate = DateTime.UtcNow;
                _masterQuestionnaireCategoryCodeRepository.Update(masterDFA_CategoryCode);
                _masterQuestionnaireCategoryCodeRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.CategoryCodeDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        #endregion

        #region Documents/Form
        public JsonModel GetDocuments(CommonFilterModel commonFilterModel, TokenModel tokenModel)
        {
            List<MasterQuestionnaireDocumentModel> questionnaireDocumentModels = _masterQuestionnaireDocumentRepository.GetDocuments<MasterQuestionnaireDocumentModel>(commonFilterModel, tokenModel).ToList();
            if (questionnaireDocumentModels != null && questionnaireDocumentModels.Count > 0)
            {
                response = new JsonModel(questionnaireDocumentModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(questionnaireDocumentModels, commonFilterModel);
            }
            return response;
        }
        public JsonModel GetDocumentById(int id, TokenModel tokenModel)
        {
            MasterDFA_Document masterDFA_Document = _masterContext.MasterDFA_Document
                    .Where(x => x.Id == id && x.IsDeleted == false && x.OrganizationID == tokenModel.OrganizationID)
                    .Include(x => x.MasterQuestionnaireBenchmarkRange)
                    .FirstOrDefault();
            if (masterDFA_Document != null)
            {
                MasterQuestionnaireDocumentModel masterMuestionnaireDocumentModel = new MasterQuestionnaireDocumentModel();
                AutoMapper.Mapper.Map(masterDFA_Document, masterMuestionnaireDocumentModel);
                List<BenchmarkRangeModel> benchmarkRangeModel = masterDFA_Document.MasterQuestionnaireBenchmarkRange.Where(z => z.IsDeleted == false && z.IsActive == true).Select(a => new BenchmarkRangeModel { Id = a.Id, BenchmarkId = a.BenchmarkId, MinRange = a.MinRange, MaxRange = a.MaxRange, QuestionnaireId = a.QuestionnaireId }).ToList();
                masterMuestionnaireDocumentModel.BenchmarkRangeModel = benchmarkRangeModel;
                response = new JsonModel(masterMuestionnaireDocumentModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        
       
        public JsonModel SaveDocument(MasterQuestionnaireDocumentModel masterQuestionnaireDocumentModel, TokenModel tokenModel)
        {
            MasterDFA_Document masterDFA_Document = null;
            if (masterQuestionnaireDocumentModel.Id == 0)
            {
                masterDFA_Document = _masterQuestionnaireDocumentRepository.Get(l => l.DocumentName == masterQuestionnaireDocumentModel.DocumentName && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (masterDFA_Document != null)//duplicate check on new insertion
                {
                    response = new JsonModel(new object(), StatusMessage.DocumentAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else // new insert
                {
                    masterDFA_Document = new MasterDFA_Document();
                    masterDFA_Document.MasterQuestionnaireBenchmarkRange = new List<MasterQuestionnaireBenchmarkRange>();
                    Mapper.Map(masterQuestionnaireDocumentModel, masterDFA_Document);
                    Mapper.Map(masterQuestionnaireDocumentModel.BenchmarkRangeModel, masterDFA_Document.MasterQuestionnaireBenchmarkRange);
                    masterDFA_Document.OrganizationID = tokenModel.OrganizationID;
                    masterDFA_Document.CreatedBy = tokenModel.UserID;
                    masterDFA_Document.CreatedDate = DateTime.UtcNow;
                    masterDFA_Document.IsDeleted = false;
                    masterDFA_Document.IsActive = true;
                    _masterQuestionnaireDocumentRepository.Create(masterDFA_Document);
                    _masterQuestionnaireDocumentRepository.SaveChanges();

                    masterDFA_Document.MasterQuestionnaireBenchmarkRange.ForEach(a =>
                    {
                        a.QuestionnaireId = masterDFA_Document.Id;
                        a.CreatedBy = tokenModel.UserID;
                        a.CreatedDate = DateTime.UtcNow;
                        a.IsActive = true;
                        a.IsDeleted = false;
                    });

                    _masterQuestionnaireBenchmarkRangeRepository.Create(masterDFA_Document.MasterQuestionnaireBenchmarkRange.ToArray());
                    _masterQuestionnaireBenchmarkRangeRepository.SaveChanges();

                    response = new JsonModel(masterDFA_Document, StatusMessage.DocumentSave, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                masterDFA_Document = _masterQuestionnaireDocumentRepository.Get(l => l.DocumentName == masterQuestionnaireDocumentModel.DocumentName && l.Id != masterQuestionnaireDocumentModel.Id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (masterDFA_Document != null) //duplicate check
                {
                    response = new JsonModel(new object(), StatusMessage.DocumentAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else
                {
                    masterDFA_Document = _masterContext.MasterDFA_Document
                    .Where(x => x.Id == masterQuestionnaireDocumentModel.Id && x.IsDeleted == false && x.OrganizationID == tokenModel.OrganizationID)
                    .Include(x => x.MasterQuestionnaireBenchmarkRange)
                    .FirstOrDefault();
                    if (masterDFA_Document != null)
                    {
                        Mapper.Map(masterQuestionnaireDocumentModel, masterDFA_Document);

                        //Mapper.Map(questionnaireDocumentModel.BenchmarkRangeModel, dfaDocument.QuestionnaireBenchmarkRange);

                        if (masterQuestionnaireDocumentModel.BenchmarkRangeModel != null && masterQuestionnaireDocumentModel.BenchmarkRangeModel.Count > 0)
                        {

                            foreach (var item in masterQuestionnaireDocumentModel.BenchmarkRangeModel)
                            {

                                if (masterDFA_Document.MasterQuestionnaireBenchmarkRange.Any(x => x.IsDeleted == false && x.Id == item.Id && x.CreatedDate != DateTime.UtcNow))
                                {
                                    var Type = masterDFA_Document.MasterQuestionnaireBenchmarkRange.Where(x => x.IsDeleted == false && x.Id == item.Id && x.CreatedDate != DateTime.UtcNow).Single();
                                    AutoMapper.Mapper.Map(item, Type);
                                    Type.UpdatedBy = tokenModel.UserID;
                                    Type.UpdatedDate = DateTime.UtcNow;
                                    _masterQuestionnaireBenchmarkRangeRepository.Update(Type);
                                    _masterQuestionnaireBenchmarkRangeRepository.SaveChanges();
                                }
                                else
                                {
                                    MasterQuestionnaireBenchmarkRange benchmarkRange = new MasterQuestionnaireBenchmarkRange();
                                    AutoMapper.Mapper.Map(item, benchmarkRange);
                                    benchmarkRange.QuestionnaireId = masterDFA_Document.Id;
                                    benchmarkRange.CreatedBy = tokenModel.UserID;
                                    benchmarkRange.CreatedDate = DateTime.UtcNow;
                                    benchmarkRange.IsDeleted = false;
                                    _masterQuestionnaireBenchmarkRangeRepository.Create(benchmarkRange);
                                    _masterQuestionnaireBenchmarkRangeRepository.SaveChanges();

                                }
                            }

                            masterDFA_Document.UpdatedBy = tokenModel.UserID;
                            masterDFA_Document.UpdatedDate = DateTime.UtcNow;
                            _masterQuestionnaireDocumentRepository.Update(masterDFA_Document);
                            _masterQuestionnaireDocumentRepository.SaveChanges();
                            response = new JsonModel(masterDFA_Document, StatusMessage.DocumentUpdated, (int)HttpStatusCode.OK);
                        }
                    }
                }
            }
            return response;
        }


        public JsonModel DeleteDocument(int id, TokenModel tokenModel)
        {
            MasterDFA_Document masterDFA_Document = _masterContext.MasterDFA_Document
                    .Where(x => x.Id == id && x.IsDeleted == false && x.OrganizationID == tokenModel.OrganizationID)
                    .Include(x => x.MasterQuestionnaireBenchmarkRange)
                    .FirstOrDefault();
            if (masterDFA_Document != null)
            {
                masterDFA_Document.IsDeleted = true;
                masterDFA_Document.DeletedBy = tokenModel.UserID;
                masterDFA_Document.DeletedDate = DateTime.UtcNow;
                if (masterDFA_Document.MasterQuestionnaireBenchmarkRange != null && masterDFA_Document.MasterQuestionnaireBenchmarkRange.Count > 0)
                {
                    masterDFA_Document.MasterQuestionnaireBenchmarkRange.ForEach(a =>
                    {
                        a.IsDeleted = true;
                        a.DeletedBy = tokenModel.UserID;
                        a.DeletedDate = DateTime.UtcNow;
                    });

                }
                _masterQuestionnaireDocumentRepository.Update(masterDFA_Document);
                _masterQuestionnaireDocumentRepository.SaveChanges();

                _masterQuestionnaireBenchmarkRangeRepository.Update(masterDFA_Document.MasterQuestionnaireBenchmarkRange.ToArray());
                _masterQuestionnaireBenchmarkRangeRepository.SaveChanges();

                response = new JsonModel(new object(), StatusMessage.DocumentDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        #endregion

        #region Section
        public JsonModel GetSections(SectionFilterModel sectionFilterModel, TokenModel tokenModel)
        {
            List<MasterQuestionnaireSectionModel> questionnaireSectionModels = _masterQuestionnaireSectionRepository.GetSections<MasterQuestionnaireSectionModel>(sectionFilterModel, tokenModel).ToList();
            if (questionnaireSectionModels != null && questionnaireSectionModels.Count > 0)
            {
                response = new JsonModel(questionnaireSectionModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(questionnaireSectionModels, sectionFilterModel);
            }
            return response;
        }
        public JsonModel SaveSection(MasterQuestionnaireSectionModel questionnaireSectionModel, TokenModel tokenModel)
        {
            MasterDFA_Section masterDFA_Section = null;
            if (questionnaireSectionModel.Id == 0)
            {
                masterDFA_Section = new MasterDFA_Section();
                Mapper.Map(questionnaireSectionModel, masterDFA_Section);
                masterDFA_Section.CreatedBy = tokenModel.UserID;
                masterDFA_Section.CreatedDate = DateTime.UtcNow;
                masterDFA_Section.IsDeleted = false;
                masterDFA_Section.IsActive = true;
                _masterQuestionnaireSectionRepository.Create(masterDFA_Section);
                _masterQuestionnaireSectionRepository.SaveChanges();
                response = new JsonModel(masterDFA_Section, StatusMessage.SectionSave, (int)HttpStatusCode.OK);
            }
            else
            {

                masterDFA_Section = _masterQuestionnaireSectionRepository.Get(a => a.Id == questionnaireSectionModel.Id && a.IsDeleted == false && a.IsActive == true);
                if (masterDFA_Section != null)
                {
                    Mapper.Map(questionnaireSectionModel, masterDFA_Section);
                    masterDFA_Section.UpdatedBy = tokenModel.UserID;
                    masterDFA_Section.UpdatedDate = DateTime.UtcNow;
                    _masterQuestionnaireSectionRepository.Update(masterDFA_Section);
                    _masterQuestionnaireSectionRepository.SaveChanges();
                    response = new JsonModel(masterDFA_Section, StatusMessage.SectionUpdated, (int)HttpStatusCode.OK);
                }
            }
            return response;
        }
        public JsonModel GetSectionById(int id, TokenModel tokenModel)
        {
            MasterDFA_Section masterDFA_Section = _masterQuestionnaireSectionRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (masterDFA_Section != null)
            {
                MasterQuestionnaireSectionModel questionnaireSectionModel = new MasterQuestionnaireSectionModel();
                AutoMapper.Mapper.Map(masterDFA_Section, questionnaireSectionModel);
                response = new JsonModel(questionnaireSectionModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel DeleteSection(int id, TokenModel tokenModel)
        {
            MasterDFA_Section masterDFA_Section = _masterQuestionnaireSectionRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (masterDFA_Section != null)
            {
                masterDFA_Section.IsDeleted = true;
                masterDFA_Section.DeletedBy = tokenModel.UserID;
                masterDFA_Section.DeletedDate = DateTime.UtcNow;
                _masterQuestionnaireSectionRepository.Update(masterDFA_Section);
                _masterQuestionnaireSectionRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.SectionDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        #endregion

        #region Section Item
        public JsonModel SaveSectionItem(MasterQuestionnaireSectionItemModel masterQuestionnaireSectionItemModel, TokenModel tokenModel)
        {

            MasterDFA_SectionItem masterDFA_SectionItem = null;
            if (masterQuestionnaireSectionItemModel.Id == 0)
            {
                masterDFA_SectionItem = new MasterDFA_SectionItem();
                Mapper.Map(masterQuestionnaireSectionItemModel, masterDFA_SectionItem);
                //images                
                string LogoUrl = _imageService.SaveImages(masterQuestionnaireSectionItemModel.PhotoBase64, ImagesPath.MasterQuestionaireImages, ImagesFolderEnum.QuestionLogo.ToString());
                masterDFA_SectionItem.QuestionLogo = LogoUrl;
                masterDFA_SectionItem.CreatedBy = tokenModel.UserID;
                masterDFA_SectionItem.CreatedDate = DateTime.UtcNow;
                masterDFA_SectionItem.IsDeleted = false;
                masterDFA_SectionItem.IsActive = true;
                _masterQuestionnaireSectionitemRepository.Create(masterDFA_SectionItem);
                _masterQuestionnaireSectionitemRepository.SaveChanges();
                response = new JsonModel(masterDFA_SectionItem, StatusMessage.SectionItemSave, (int)HttpStatusCode.OK);
            }
            else
            {

                masterDFA_SectionItem = _masterQuestionnaireSectionitemRepository.Get(a => a.Id == masterQuestionnaireSectionItemModel.Id && a.IsDeleted == false && a.IsActive == true);
                if (masterDFA_SectionItem != null)
                {
                    Mapper.Map(masterQuestionnaireSectionItemModel, masterDFA_SectionItem);
                    //images
                    if (!string.IsNullOrEmpty(masterQuestionnaireSectionItemModel.PhotoBase64))
                    masterDFA_SectionItem.QuestionLogo = masterQuestionnaireSectionItemModel.PhotoBase64.Contains("http") ? masterDFA_SectionItem.QuestionLogo : _imageService.SaveImages(masterQuestionnaireSectionItemModel.PhotoBase64, ImagesPath.MasterQuestionaireImages, ImagesFolderEnum.QuestionLogo.ToString());
                    masterDFA_SectionItem.UpdatedBy = tokenModel.UserID;
                    masterDFA_SectionItem.UpdatedDate = DateTime.UtcNow;
                    _masterQuestionnaireSectionitemRepository.Update(masterDFA_SectionItem);
                    _masterQuestionnaireSectionitemRepository.SaveChanges();
                    response = new JsonModel(masterDFA_SectionItem, StatusMessage.SectionItemUpdated, (int)HttpStatusCode.OK);
                }
            }
            return response;
        }
        public JsonModel GetSectionItem(MasterSectionFilterModel masterSectionFilterModel, TokenModel tokenModel)
        {
            MasterSectionItemlistingModel sectionItemlistingModel = _masterQuestionnaireSectionitemRepository.GetSectionItems(masterSectionFilterModel, tokenModel);
            if (sectionItemlistingModel != null)
            {
                response = new JsonModel(sectionItemlistingModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(sectionItemlistingModel.SectionItems, masterSectionFilterModel);
            }
            return response;
        }

        public JsonModel GetSectionItemsForForm(int DocumentId, TokenModel tokenModel)
        {
            MasterSectionItemlistingModel sectionItemlistingModel = _masterQuestionnaireSectionitemRepository.GetSectionItemsForForm(DocumentId, tokenModel);
            if (sectionItemlistingModel != null)
            {
                response = new JsonModel(sectionItemlistingModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }

        public JsonModel GetSectionItemById(int id, TokenModel tokenModel, IHttpContextAccessor contextAccessor)
        {
            MasterQuestionnaireSectionItemModel masterQuestionnaireSectionItemModel = _masterQuestionnaireSectionitemRepository.GetSectionItemsByID<MasterQuestionnaireSectionItemModel>(id, tokenModel).FirstOrDefault();
            if (masterQuestionnaireSectionItemModel != null)
            {
                if (!string.IsNullOrEmpty(masterQuestionnaireSectionItemModel.QuestionLogo))
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.MasterQuestionaireImages + "QuestionLogo//" + masterQuestionnaireSectionItemModel.QuestionLogo))
                        masterQuestionnaireSectionItemModel.PhotoBase64 = CommonMethods.CreateImageUrl(contextAccessor.HttpContext, ImagesPath.MasterQuestionaireImages + "QuestionLogo//", masterQuestionnaireSectionItemModel.QuestionLogo); //contextAccessor.HttpContext.Request.Scheme+"://"+ contextAccessor.HttpContext.Request.Host + ImagesPath.OrganizationImages + "Logo\\"+  responseOrganizationObj.Logo;
                    else
                        masterQuestionnaireSectionItemModel.PhotoBase64 = string.Empty;
                }
                else
                {
                    masterQuestionnaireSectionItemModel.PhotoBase64 = string.Empty;
                }
                response = new JsonModel(masterQuestionnaireSectionItemModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel GetSectionItemDDValues(MasterSectionFilterModel sectionFilterModel, TokenModel tokenModel)
        {
            MasterSectionItemDDValueModel sectionItemDDValueModel = _masterQuestionnaireSectionitemRepository.GetSectionItemDDValues(sectionFilterModel, tokenModel);
            if (sectionItemDDValueModel != null)
            {
                response = new JsonModel(sectionItemDDValueModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }
        public JsonModel DeleteSectionItem(int id, TokenModel tokenModel)
        {
            MasterDFA_SectionItem masterDFA_SectionItem = _masterQuestionnaireSectionitemRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (masterDFA_SectionItem != null)
            {
                masterDFA_SectionItem.IsDeleted = true;
                masterDFA_SectionItem.DeletedBy = tokenModel.UserID;
                masterDFA_SectionItem.DeletedDate = DateTime.UtcNow;
                _masterQuestionnaireSectionitemRepository.Update(masterDFA_SectionItem);
                _masterQuestionnaireSectionitemRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.SectionItemDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        #endregion

        #region Questionnaire Type
        public JsonModel GetQuestionnaireTypes(TokenModel tokenModel)
        {
            List<MasterQuestionnaireTypeModel> questionnaireTypeModel = _masterQuestionnaireDocumentRepository.GetQuestionnaireTypes<MasterQuestionnaireTypeModel>(tokenModel).ToList();
            if (questionnaireTypeModel != null && questionnaireTypeModel.Count > 0)
            {
                response = new JsonModel(questionnaireTypeModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);

            }
            return response;
        }
        #endregion
    }
}
