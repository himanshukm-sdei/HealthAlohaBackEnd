using AutoMapper;
using HC.Common.Model.OrganizationSMTP;
using HC.Patient.Data.ViewModel;
using HC.Patient.Entity;
using HC.Patient.Model.AppConfiguration;
using HC.Patient.Model.Chat;
using HC.Patient.Model.CheckLists;
using HC.Patient.Model.Claim;
using HC.Patient.Model.MasterCheckLists;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.MasterQuestionnaire;
using HC.Patient.Model.MasterServiceCodes;
using HC.Patient.Model.Message;
using HC.Patient.Model.Organizations;
using HC.Patient.Model.Patient;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Model.PatientEncounters;
using HC.Patient.Model.PatientMedicalFamilyHistory;
using HC.Patient.Model.Payer;
using HC.Patient.Model.Payroll;
//using HC.Patient.Model.PayrollBreaktime;
using HC.Patient.Model.Questionnaire;
using HC.Patient.Model.SecurityQuestion;
using HC.Patient.Model.Staff;
using HC.Patient.Model.Users;
using HC.Patient.Model.VideoAndArticle;

namespace HC.Patient.Service.Automapper
{
    public class MapperProfileConfiguration : Profile
    {
        public MapperProfileConfiguration()
        {
            CreateMap<RoundingRuleModel, MasterRoundingRules>();
            CreateMap<MasterRoundingRules, RoundingRuleModel>();
            CreateMap<RoundingRuleDetailsModel, RoundingRuleDetails>();
            CreateMap<RoundingRuleDetails, RoundingRuleDetailsModel>();
            CreateMap<RoundingRuleDetails, RoundingRuleModel>();
            CreateMap<RoundingRuleModel, RoundingRuleDetails>();
            CreateMap<PatientEncounterModel, PatientEncounter>();
            CreateMap<PatientEncounter, PatientEncounterModel>();
            CreateMap<PatientEncounterDiagnosisCodes, PatientEncounterDiagnosisCodesModel>();
            CreateMap<PatientEncounterDiagnosisCodesModel, PatientEncounterDiagnosisCodes>();
            CreateMap<PatientEncounterServiceCodes, PatientEncounterServiceCodesModel>();
            CreateMap<PatientEncounterServiceCodesModel, PatientEncounterServiceCodes>();
            CreateMap<PatientEncounterCodesMapping, PatientEncounterCodesMappingModel>();
            CreateMap<PatientEncounterCodesMappingModel, PatientEncounterCodesMapping>();
            CreateMap<SoapNotes, SOAPNotesModel>();
            CreateMap<SOAPNotesModel, SoapNotes>();
            CreateMap<PatientEncounterViewModel, PatientEncounterModel>();
            CreateMap<PatientEncounterServiceCodesViewModel, PatientEncounterServiceCodesModel>();
            CreateMap<PatientEncounterCodesMappingViewModel, PatientEncounterCodesMappingModel>();
            CreateMap<PatientEncounterICDCodesViewModel, PatientEncounterDiagnosisCodesModel>();
            CreateMap<SOAPNotesViewModel, SOAPNotesModel>();
            CreateMap<PatientEncounterICDCodesViewModel, PatientEncounterDiagnosisCodesModel>();
            CreateMap<SOAPNotesViewModel, SOAPNotesModel>();
            CreateMap<Organization, OrganizationModel>();
            CreateMap<OrganizationModel, Organization>();
            CreateMap<MasterOrganization, OrganizationModel>();
            CreateMap<OrganizationModel, MasterOrganization>();
            CreateMap<OrganizationDatabaseDetail, OrganizationDatabaseDetailModel>();
            CreateMap<OrganizationDatabaseDetailModel, OrganizationDatabaseDetail>();
            CreateMap<OrganizationSubscriptionPlan, OrganizationSubscriptionPlanModel>();
            CreateMap<OrganizationSubscriptionPlanModel, OrganizationSubscriptionPlan>();
            CreateMap<OrganizationSMTPDetails, OrganizationSMTPDetailsModel>();
            CreateMap<OrganizationSMTPDetailsModel, OrganizationSMTPDetails>();
            CreateMap<OrganizationSMTPDetails, OrganizationSMTPCommonModel>();
            CreateMap<OrganizationSMTPCommonModel, OrganizationSMTPDetails>();

            
            CreateMap<ClaimServiceLine, ClaimServiceLineModel>();
            CreateMap<ClaimServiceLineModel, ClaimServiceLine>();
            CreateMap<Claims, ClaimModel>();
            CreateMap<ClaimModel, Claims>();

            CreateMap<PatientMedicalFamilyHistory, Model.PatientMedicalFamilyHistory.PatientMedicalFamilyHistoryModel>();
            CreateMap<Model.PatientMedicalFamilyHistory.PatientMedicalFamilyHistoryModel, PatientMedicalFamilyHistory>();
            CreateMap<PatientMedicalFamilyHistoryDiseases, PatientMedicalFamilyHistoryDiseasesModel>();
            CreateMap<PatientMedicalFamilyHistoryDiseasesModel, PatientMedicalFamilyHistoryDiseases>();
            CreateMap<PatientAppointment, PatientAppointmentModel>();
            CreateMap<PatientAppointmentModel, PatientAppointment>();
            CreateMap<AppConfigurations, MasterAppConfiguration>();
            CreateMap<MasterAppConfiguration, AppConfigurations>();
            CreateMap<SecurityQuestions, MasterSecurityQuestions>();
            CreateMap<MasterSecurityQuestions, SecurityQuestions>();

            CreateMap<AppConfigurations, AppConfigurationsModel>();
            CreateMap<AppConfigurationsModel, AppConfigurations>();

            CreateMap<MessageDetailModel, ForwardMessageDetailModel>();
            CreateMap<ForwardMessageDetailModel, MessageDetailModel>();

            CreateMap<MasterModifierModel, MasterModifiers>();
            CreateMap<MasterModifiers, MasterModifierModel>();

            CreateMap<ModifierModel, MasterServiceCodeModifiers>();
            CreateMap<MasterServiceCodeModifiers, ModifierModel>();

            CreateMap<MasterServiceCodesModel, MasterServiceCode>();
            CreateMap<MasterServiceCode, MasterServiceCodesModel>();


            CreateMap<PayerModifierModel, PayerServiceCodeModifiers>();
            CreateMap<PayerServiceCodeModifiers, PayerModifierModel>();

            CreateMap<PayerServiceCodesModel, PayerServiceCodes>();
            CreateMap<PayerServiceCodes, PayerServiceCodesModel>();

            CreateMap<StaffLeaveModel, StaffLeave>();
            CreateMap<StaffLeave, StaffLeaveModel>();
            CreateMap<PhoneNumbers, Model.Patient.PhoneModel>();
            CreateMap<Model.Patient.PhoneModel, PhoneNumbers>();
            CreateMap<PatientAddress, Model.Patient.AddressModel>();
            CreateMap<Model.Patient.AddressModel, PatientAddress>();

            CreateMap<PatientInsuranceDetails, PatientInsuranceModel>();
            CreateMap<PatientInsuranceModel, PatientInsuranceDetails>();
            CreateMap<InsuredPerson, InsuredPersonModel>();
            CreateMap<InsuredPersonModel, InsuredPerson>();

            CreateMap<CustomLabelModel, PatientCustomLabels>();
            CreateMap<PatientCustomLabels, CustomLabelModel>();
            CreateMap<StaffDetailedTimesheetModel, UserTimesheetByAppointmentType>();
            CreateMap<UserTimesheetByAppointmentType, StaffDetailedTimesheetModel>();

            //CreateMap<PayrollGroup, PayrollGroupModel>();
            //CreateMap<PayrollGroupModel, PayrollGroup>();
            CreateMap<PatientSocialHistory, PatientSocialHistoryModel>();
            CreateMap<PatientSocialHistoryModel, PatientSocialHistory>();

            CreateMap<PatientImmunization, PatientImmunizationModel>();
            CreateMap<PatientImmunizationModel, PatientImmunization>();

            CreateMap<PatientDiagnosis, PatientDiagnosisModel>();
            CreateMap<PatientDiagnosisModel, PatientDiagnosis>();

            CreateMap<AppointmentType, AppointmentTypesModel>();
            CreateMap<AppointmentTypesModel, AppointmentType>();

            CreateMap<MasterCustomLabels, MasterCustomLabelModel>();
            CreateMap<MasterCustomLabelModel, MasterCustomLabels>();

            CreateMap<MasterSecurityQuestionModel, SecurityQuestions>();
            CreateMap<SecurityQuestions, MasterSecurityQuestionModel>();

            CreateMap<MasterTagModel, MasterTags>();
            CreateMap<MasterTags, MasterTagModel>();

            //CreateMap<PayrollBreakTime, PayrollBreaktimeModel>();
            //CreateMap<PayrollBreaktimeModel,PayrollBreakTime>();

            //CreateMap<PayrollBreakTimeDetails, PayrollBreaktimeDetailsModel>();
            //CreateMap<PayrollBreaktimeDetailsModel, PayrollBreakTimeDetails>();

            // Payer 
            CreateMap<InsuranceCompanies, InsuranceCompanyModel>();
            CreateMap<InsuranceCompanyModel, InsuranceCompanies>();

            // Agency Holidays 
            CreateMap<Holidays, HolidaysModel>();
            CreateMap<HolidaysModel, Holidays>();

            // Allergies
            CreateMap<Entity.PatientAllergies, PatientAllergyModel>();
            CreateMap<PatientAllergyModel, Entity.PatientAllergies>();

            // Medication
            CreateMap<PatientMedication, PatientsMedicationModel>();
            CreateMap<PatientsMedicationModel, PatientMedication>();

            // vitals
            CreateMap<PatientVitals, PatientVitalModel>();
            CreateMap<PatientVitalModel, PatientVitals>();

            //Authorization
            CreateMap<Authorization, AuthModel>();
            CreateMap<AuthModel, Authorization>();
            CreateMap<AuthorizationProcedures, AuthProceduresModel>();
            CreateMap<AuthProceduresModel, AuthorizationProcedures>();
            CreateMap<AuthProcedureCPT, AuthProcedureCPTModel>();
            CreateMap<AuthProcedureCPTModel, AuthProcedureCPT>();            

            //EDI
            CreateMap<EDIGateway, EDIModel>();
            CreateMap<EDIModel, EDIGateway>();

            //Staff Custom Label
            CreateMap<StaffCustomLabels, StaffCustomLabelModel>();
            CreateMap<StaffCustomLabelModel, StaffCustomLabels>();


            //MasterICD
            CreateMap<MasterICD, MasterICDModel>();
            CreateMap<MasterICDModel, MasterICD>();

            //Master Insurance type
            CreateMap<MasterInsuranceType, MasterInsuranceTypeModel>();
            CreateMap<MasterInsuranceTypeModel, MasterInsuranceType>();

            //Master Insurance type
            CreateMap<Location, LocationModel>();
            CreateMap<LocationModel, Location>();

            //user roles
            CreateMap<UserRoles, UserRoleModel>();
            CreateMap<UserRoleModel, UserRoles>();
            CreateMap<Patients, PatientDemographicsModel>();
            CreateMap<PatientDemographicsModel, Patients>();

            CreateMap<PayerAppointmentTypes, PayerActivityCodeModel>();
            CreateMap<PayerActivityCodeModel, PayerAppointmentTypes>();

            CreateMap<AuthProcedureCPTModifiers, AuthProcedureCPTModifierModel>();
            CreateMap<AuthProcedureCPTModifierModel, AuthProcedureCPTModifiers>();

            CreateMap<Chat, ChatModel>();
            CreateMap<ChatModel, Chat>();

            //Questionnaire
            CreateMap<CategoryModel, DFA_Category>();
            CreateMap<DFA_Category, CategoryModel>();

            CreateMap<CategoryCodeModel, DFA_CategoryCode>();
            CreateMap<DFA_CategoryCode, CategoryCodeModel>();

            CreateMap<DFA_Document, QuestionnaireDocumentModel>();
            CreateMap<QuestionnaireDocumentModel, DFA_Document>();

            CreateMap<DFA_Section, QuestionnaireSectionModel>();
            CreateMap<QuestionnaireSectionModel, DFA_Section>();

            CreateMap<MasterDFA_SectionItem, MasterQuestionnaireSectionItemModel>();
            CreateMap<MasterQuestionnaireSectionItemModel, MasterDFA_SectionItem>(); 

            CreateMap<QuestionnaireSectionItemModel, DFA_SectionItem>();
            CreateMap<DFA_SectionItem, QuestionnaireSectionItemModel>();

            CreateMap<AssignDocumentToPatientModel, DFA_PatientDocuments>();
            CreateMap<DFA_PatientDocuments, AssignDocumentToPatientModel>();
            
            // Master Template
            CreateMap<MasterTemplatesModel, MasterTemplates>();
            CreateMap<MasterTemplates, MasterTemplatesModel>();

            // Patient Encounter Template
            CreateMap<PatientEncounterTemplateModel, PatientEncounterTemplates>();
            CreateMap<PatientEncounterTemplates, PatientEncounterTemplateModel>();

            //CheckList
            CreateMap<MasterCheckListModel, MasterCheckList>();
            CreateMap<MasterCheckList, MasterCheckListModel>();
            CreateMap<CheckListModel,CheckList>();
            CreateMap<CheckList, CheckListModel>();
            CreateMap<CheckListCategoryModel, CheckListCategory>();
            CreateMap<CheckListCategory, CheckListCategoryModel>();
            CreateMap<MasterCheckListCategory, MasterCheckListCategoryModel>();
            CreateMap<MasterCheckListCategoryModel, MasterCheckListCategory>();

            //MasterQuestionnaire
            CreateMap<MasterCategoryModel, MasterDFA_Category>();
            CreateMap<MasterDFA_Category, MasterCategoryModel>();

            CreateMap<MasterCategoryCodeModel, MasterDFA_CategoryCode>();
            CreateMap<MasterDFA_CategoryCode, MasterCategoryCodeModel>();

            CreateMap<MasterDFA_Document, MasterQuestionnaireDocumentModel>();
            CreateMap<MasterQuestionnaireDocumentModel, MasterDFA_Document>();

            CreateMap<MasterQuestionnaireBenchmarkRange, BenchmarkRangeModel>();
            CreateMap<BenchmarkRangeModel, MasterQuestionnaireBenchmarkRange>();

            CreateMap<MasterDFA_Section, MasterQuestionnaireSectionModel>();
            CreateMap<MasterQuestionnaireSectionModel, MasterDFA_Section>();























            // Video And Articles
            CreateMap<VideoAndArticles, VideoAndArticlesModel>();
            CreateMap<VideoAndArticlesModel, VideoAndArticles>();
            CreateMap<VideoAndArticlesCategories, VideoAndArticlesCategoriesModel>();
            CreateMap<VideoAndArticlesCategoriesModel, VideoAndArticlesCategories>();

        }
    }
}
