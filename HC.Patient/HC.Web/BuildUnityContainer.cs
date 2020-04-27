using EDIGenerator.IServices;
using EDIGenerator.Services;
using EDIParser.IServices;
using EDIParser.Services;
using HC.Common.Services;
using HC.Patient.Repositories;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.AdminDashboard;
using HC.Patient.Repositories.IRepositories.AppConfiguration;
using HC.Patient.Repositories.IRepositories.Appointment;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Repositories.IRepositories.Authorization;
using HC.Patient.Repositories.IRepositories.Chats;
using HC.Patient.Repositories.IRepositories.CheckLists;
using HC.Patient.Repositories.IRepositories.Claim;
using HC.Patient.Repositories.IRepositories.EDI;
using HC.Patient.Repositories.IRepositories.GlobalCodes;
using HC.Patient.Repositories.IRepositories.Locations;
using HC.Patient.Repositories.IRepositories.MasterCheckLists;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Patient.Repositories.IRepositories.MasterQuestionnaire;
using HC.Patient.Repositories.IRepositories.MasterServiceCodes;
using HC.Patient.Repositories.IRepositories.Message;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.PatientEncounters;
using HC.Patient.Repositories.IRepositories.PatientMedicalFamilyHistories;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Patient.Repositories.IRepositories.Payment;
using HC.Patient.Repositories.IRepositories.Payroll;
using HC.Patient.Repositories.IRepositories.Questionnaire;
using HC.Patient.Repositories.IRepositories.RolePermission;
using HC.Patient.Repositories.IRepositories.SecurityQuestion;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Repositories.IRepositories.StaffAvailability;
using HC.Patient.Repositories.IRepositories.Telehealth;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Repositories.IRepositories.VideoAndArticle;
using HC.Patient.Repositories.Repositories;
using HC.Patient.Repositories.Repositories.AdminDashboard;
using HC.Patient.Repositories.Repositories.AppConfiguration;
using HC.Patient.Repositories.Repositories.Appointment;
using HC.Patient.Repositories.Repositories.AuditLog;
using HC.Patient.Repositories.Repositories.Authorization;
using HC.Patient.Repositories.Repositories.Chats;
using HC.Patient.Repositories.Repositories.CheckLists;
using HC.Patient.Repositories.Repositories.Claim;
using HC.Patient.Repositories.Repositories.EDI;
using HC.Patient.Repositories.Repositories.GlobalCodes;
using HC.Patient.Repositories.Repositories.Locations;
using HC.Patient.Repositories.Repositories.MasterCheckLists;
using HC.Patient.Repositories.Repositories.MasterData;
using HC.Patient.Repositories.Repositories.MasterQuestionnaire;
using HC.Patient.Repositories.Repositories.MasterServiceCodes;
using HC.Patient.Repositories.Repositories.Message;
using HC.Patient.Repositories.Repositories.Organizations;
using HC.Patient.Repositories.Repositories.Patient;
using HC.Patient.Repositories.Repositories.PatientEncounters;
using HC.Patient.Repositories.Repositories.PatientMedicalFamilyHistories;
using HC.Patient.Repositories.Repositories.Payer;
using HC.Patient.Repositories.Repositories.Payment;
//using HC.Patient.Repositories.Repositories.Payment;
using HC.Patient.Repositories.Repositories.Payroll;
using HC.Patient.Repositories.Repositories.Questionnaire;
using HC.Patient.Repositories.Repositories.RolePermission;
using HC.Patient.Repositories.Repositories.SecurityQuestion;
using HC.Patient.Repositories.Repositories.Staff;
using HC.Patient.Repositories.Repositories.StaffAvailability;
using HC.Patient.Repositories.Repositories.Telehealth;
using HC.Patient.Repositories.Repositories.User;
using HC.Patient.Repositories.Repositories.VideoAndArticle;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.AdminDashboard;
using HC.Patient.Service.IServices.AppConfiguration;
using HC.Patient.Service.IServices.AuditLog;
using HC.Patient.Service.IServices.Authorization;
using HC.Patient.Service.IServices.Chats;
using HC.Patient.Service.IServices.CheckLists;
using HC.Patient.Service.IServices.Claim;
using HC.Patient.Service.IServices.EDI;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Patient.Service.IServices.Images;
using HC.Patient.Service.IServices.Login;
using HC.Patient.Service.IServices.MasterCheckLists;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.MasterQuestionnaire;
using HC.Patient.Service.IServices.MasterServiceCodes;
using HC.Patient.Service.IServices.Message;
using HC.Patient.Service.IServices.Organizations;
using HC.Patient.Service.IServices.Password;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.PatientAppointment;
using HC.Patient.Service.IServices.PatientEncounters;
using HC.Patient.Service.IServices.PatientMedicalFamilyHistories;
using HC.Patient.Service.IServices.Payer;
//using HC.Patient.Service.IServices.Payment;
using HC.Patient.Service.IServices.Payroll;
using HC.Patient.Service.IServices.Questionnaire;
using HC.Patient.Service.IServices.RolePermission;
using HC.Patient.Service.IServices.SecurityQuestion;
using HC.Patient.Service.IServices.Staff;
using HC.Patient.Service.IServices.StaffAvailability;
using HC.Patient.Service.IServices.Telehealth;
using HC.Patient.Service.IServices.User;
using HC.Patient.Service.IServices.VideoAndArticle;
using HC.Patient.Service.MasterData;
using HC.Patient.Service.MasterData.Interfaces;
using HC.Patient.Service.PatientApp;
using HC.Patient.Service.PatientCommon;
using HC.Patient.Service.PatientCommon.Interfaces;
using HC.Patient.Service.Payer;
using HC.Patient.Service.Payer.Interfaces;
using HC.Patient.Service.Services.AdminDashboard;
using HC.Patient.Service.Services.AppConfiguration;
using HC.Patient.Service.Services.AuditLog;
using HC.Patient.Service.Services.Authorization;
using HC.Patient.Service.Services.Chats;
using HC.Patient.Service.Services.CheckLists;
using HC.Patient.Service.Services.Claim;
using HC.Patient.Service.Services.EDI;
using HC.Patient.Service.Services.GlobalCodes;
using HC.Patient.Service.Services.Images;
using HC.Patient.Service.Services.Login;
using HC.Patient.Service.Services.MasterCheckLists;
using HC.Patient.Service.Services.MasterData;
using HC.Patient.Service.Services.MasterQuestionnaire;
using HC.Patient.Service.Services.MasterServiceCodes;
using HC.Patient.Service.Services.Message;
using HC.Patient.Service.Services.Organizations;
using HC.Patient.Service.Services.Password;
using HC.Patient.Service.Services.Patient;
using HC.Patient.Service.Services.PatientEncounters;
using HC.Patient.Service.Services.PatientMedicalFamilyHistories;
using HC.Patient.Service.Services.Payer;
//using HC.Patient.Service.Services.Payment;
using HC.Patient.Service.Services.Payroll;
using HC.Patient.Service.Services.Questionnaire;
using HC.Patient.Service.Services.RolePermission;
using HC.Patient.Service.Services.SecurityQuestion;
using HC.Patient.Service.Services.Staff;
using HC.Patient.Service.Services.StaffAvailability;
using HC.Patient.Service.Services.Telehealth;
using HC.Patient.Service.Services.User;
using HC.Patient.Service.Services.VideoAndArticle;
using HC.Patient.Service.Token;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Service.Users;
//using Infrastructure.Implementations.Services;
using Microsoft.Extensions.DependencyInjection;
using Twilio.Clients;

namespace HC.Patient.Web
{
    public static class BuildUnityContainer
    {
        public static IServiceCollection RegisterAddTransient(IServiceCollection services)
        {
            #region Repository

            services.AddTransient<IRepository, MasterDataRepository>();
            services.AddTransient<IPatientCommonRepository, PatientCommonRepository>();
            services.AddTransient<IUserCommonRepository, UserCommonRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IClaimRepository, ClaimRepository>();
            services.AddTransient<IRoundingRuleRepository, RoundingRuleRepository>();
            services.AddTransient<IRoundingRuleDetailRepository, RoundingRuleDetailRepository>();
            services.AddTransient<IPatientEncounterRepository, PatientEncounterRepository>();
            services.AddTransient<IPatientEncounterCodesMappingsRepository, PatientEncounterCodesMappingsRepository>();
            services.AddTransient<IPatientEncounterICDCodesRepository, PatientEncounterICDCodesRepository>();
            services.AddTransient<IPatientEncounterServiceCodesRepository, PatientEncounterServiceCodesRepository>();
            services.AddTransient<ISoapNoteRepository, SoapNoteRepository>();
            services.AddTransient<IPatientDiagnosisRepository, PatientDiagnosisRepository>();
            services.AddTransient<IRoundingRuleDetailRepository, RoundingRuleDetailRepository>();
            services.AddTransient<IRoundingRuleDetailRepository, RoundingRuleDetailRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IPatientAppointmentRepository, PatientAppointmentRepository>();
            //organization
            services.AddTransient<IOrganizationRepository, OrganizationRepository>();
            services.AddTransient<IMasterOrganizationRepository, MasterOrganizationRepository>();
            services.AddTransient<IOrganizationDatabaseRepository, OrganizationDatabaseRepository>();
            services.AddTransient<IOrganizationSubscriptionPlanRepository, OrganizationSubscriptionPlanRepository>();
            services.AddTransient<IOrganizationSMTPRepository, OrganizationSMTPRepository>();
            services.AddTransient<IClaimServiceLineRepository, ClaimServiceLineRepository>();
            services.AddTransient<IClaimDiagnosisCodeRepository, ClaimDiagnosisCodeRepository>();
            
            services.AddTransient<IPayerInformationRepository, PayerInformationRepository>();
            services.AddTransient<IPayerActivityRepository, PayerActivityRepository>();
            services.AddTransient<IPatientMedicalFamilyHistoryRepository, PatientMedicalFamilyHistoryRepository>();
            services.AddTransient<IPatientMedicalFamilyHistoryDiseasesRepository, PatientMedicalFamilyHistoryDiseasesRepository>();
            services.AddTransient<IAuditLogRepository, AuditLogRepository>();
            services.AddTransient<IAuditLogTableRepository, AuditLogTableRepository>();
            services.AddTransient<IAuditLogColumnRepository, AuditLogColumnRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IStaffRepository, StaffRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IAppointmentStaffRepository, AppointmentStaffRepository>();
            //services.AddTransient<IClaim837BatchRepository, Claim837BatchRepository>();
            //services.AddTransient<IClaim837ClaimRepository, Claim837ClaimRepository>();
            //services.AddTransient<IClaim837ServiceLineRepository, Claim837ServiceLineRepository>();
            services.AddTransient<IMasterSecurityQuestionsRepository, MasterSecurityQuestionsRepository>();
            services.AddTransient<IUserSecurityQuestionAnswerRepository, UserSecurityQuestionAnswerRepository>();
            services.AddTransient<ISecurityQuestionsRepository, SecurityQuestionsRepository>();
            services.AddTransient<IRolePermissionRepository, RolePermissionRepository>();

            services.AddTransient<IPatientAuthorizationRepository, PatientAuthorizationRepository>();
            services.AddTransient<IPatientAuthorizationProceduresRepository, PatientAuthorizationProceduresRepository>();
            services.AddTransient<IPatientAuthorizationProcedureCPTLinkRepository, PatientAuthorizationProcedureCPTLinkRepository>();
            //services.AddTransient<IClaim835BatchRepository, Claim835BatchRepository>();
            services.AddTransient<IEdiGatewayRepository, EdiGatewayRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IStaffAvailabilityRepository, StaffAvailabilityRepository>();
            services.AddTransient<ITelehealthRepository, TelehealthRepository>();
            services.AddTransient<IAdminDashboardRepository, AdminDashboardRepository>();
            services.AddTransient<IPatientAddressRepository, PatientAddressRepository>();
            services.AddTransient<IMasterPatientLocationRepository, MasterPatientLocationRepository>();
            services.AddTransient<IAppointmentAuthorizationRepository, AppointmentAuthorizationRepository>();


            services.AddTransient<IMessageRecepientsRepository, MessageRecepientsRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IMessageDocumentRepository, MessageDocumentRepository>();
            services.AddTransient<IMasterModifierRepository, MasterModifierRepository>();
            services.AddTransient<IMasterServiceCodesRepository, MasterServiceCodesRepository>();
            services.AddTransient<IPayerServiceCodesRepository, PayerServiceCodesRepository>();
            services.AddTransient<IAppointmentTypeRepository, AppointmentTypeRepository>();
            services.AddTransient<IStaffLeaveRepository, StaffLeaveRepository>();
            services.AddTransient<IStaffCustomLabelRepository, StaffCustomLabelRepository>();
            services.AddTransient<IStaffTagRepository, StaffTagRepository>();
            services.AddTransient<IPatientGuardianRepository, PatientGuardianRepository>();
            services.AddTransient<IUserTimesheetRepository, UserTimesheetRepository>();
            services.AddTransient<IUserTimesheetByAppointmentTypeRepository, UserTimesheetByAppointmentTypeRepository>();
            services.AddTransient<IPatientInsuranceRepository, PatientInsuranceRepository>();
            services.AddTransient<IPatientCustomLabelRepository, PatientCustomLabelRepository>();
            services.AddTransient<IUserDriveTimeRepository, UserDriveTimeRepository>();
            services.AddTransient<IUserDetailedDriveTimeRepository, UserDetailedDriveTimeRepository>();
            services.AddTransient<IGlobalCodeRepository, GlobalCodeRepository>();
            //services.AddTransient<IPayrollGroupRepository, PayrollGroupRepository>();
            services.AddTransient<IPatientSocialHistoryRepository, PatientSocialHistoryRepository>();
            services.AddTransient<IPatientImmunizationRepository, PatientImmunizationRepository>();
            services.AddTransient<IMasterCustomLabelRepository, MasterCustomLabelRepository>();
            services.AddTransient<IMasterTagRepository, MasterTagRepository>();
            //services.AddTransient<IPayrollBreaktimeRepository, PayrollBreaktimeRepository>();
            //services.AddTransient<IPayrollBreakTimeDetailsRepository, PayrollBreakTimeDetailsRepository>();
            services.AddTransient<IPatientAllergyRepository, PatientAllergyRepository>();
            services.AddTransient<IPatientMedicationRepository, PatientMedicationRepository>();
            services.AddTransient<IPatientVitalRepository, PatientVitalRepository>();
            services.AddTransient<IAppConfigurationRepository, AppConfigurationRepository>();
            services.AddTransient<IMasterICDRepository, MasterICDRepository>();
            services.AddTransient<IMasterInsuranceTypeRepository, MasterInsuranceTypeRepository>();
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IPayerRepository, PayerRespository>();
            services.AddTransient<IPatientInviteRepository, PatientInviteRepository>();

            // Agency Holidays
            services.AddTransient<IAgencyHolidaysRepository, AgencyHolidaysRepository>();
            services.AddTransient<IUserPasswordHistoryRepository, UserPasswordHistoryRepository>();
            // Master Security Question
            services.AddTransient<IMasterSecurityQuestionRepository, MasterSecurityQuestionRepository>();
            services.AddTransient<IPatientTagRepository, PatientTagRepository>();
            
            services.AddTransient<IStaffPayrollRateForActivityRepository, StaffPayrollRateForActivityRepository>();
            services.AddTransient<IAuthorizationRepository, AuthorizationRepository>();
            services.AddTransient<IEDI270Repository, EDI270Repository>();
            services.AddTransient<IEDI999Repository, EDI999Repository>();
            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<IChatConnectedUserRepository, ChatConnectedUserRepository>();

            //Questionnaire
            services.AddTransient<IQuestionnaireCategoryRepository, QuestionnaireCategoryRepository>();
            services.AddTransient<IQuestionnaireCategoryCodeRepository, QuestionnaireCategoryCodeRepository>();
            services.AddTransient<IQuestionnaireDocumentRepository, QuestionnaireDocumentRepository>();
            services.AddTransient<IQuestionnaireSectionRepository, QuestionnaireSectionRepository>();
            services.AddTransient<IQuestionnaireSectionItemRepository, QuestionnaireSectionItemRepository>();
            services.AddTransient<IDocumentAnswerRepository, DocumentAnswerRepository>();
            services.AddTransient<IPatientDocumentsRepository, PatientDocumentsRepository>();

            // MasterTempate
            services.AddTransient<IMasterTemplatesRepository, MasterTemplatesRepository>();
            services.AddTransient<IMasterQuestionnaireCategoryRepository, MasterQuestionnaireCategoryRepository>();
            services.AddTransient<IMasterQuestionnaireCategoryCodeRepository, MasterQuestionnaireCategoryCodeRepository>();
            services.AddTransient<IMasterQuestionnaireDocumentRepository, MasterQuestionnaireDocumentRepository>();
            services.AddTransient<IMasterQuestionnaireBenchmarkRangeRepository, MasterQuestionnaireBenchmarkRangeRepository>();
            services.AddTransient<IMasterQuestionnaireSectionRepository, MasterQuestionnaireSectionRepository>();
            services.AddTransient<IMasterQuestionnaireSectionItemRepository, MasterQuestionaireSectionItemRepository>();

            // PatientEncounterTemplateRepository
            services.AddTransient<IPatientEncounterTemplateRepository, PatientEncounterTemplateRepository>();
            services.AddTransient<ICheckListCategoryRepository, CheckListCategoryRepository>();
            services.AddTransient<IMasterCheckListCategoryRepository, MasterCheckListCategoryRepository>();

            services.AddTransient<ICheckListRepository, CheckListRepository>();
            services.AddTransient<IMasterCheckListRepository, MasterCheckListRepository>();





















            // Video And Articles Repository
            services.AddTransient<IVideoAndArticlesRepository, VideoAndArticlesRepository>();
            services.AddTransient<IVideoAndArticlesCategoriesRepository, VideoAndArticlesCategoriesRepository>();
            #endregion



            #region Services
            services.AddTransient<IPatientCommonService, PatientCommonService>();
            services.AddTransient<IMasterDataService, MasterDataService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<HC.Patient.Service.Users.Interfaces.IUserService, HC.Patient.Service.Users.UserService>();
            services.AddTransient<HC.Patient.Service.IServices.User.IUserService, HC.Patient.Service.Services.User.UserService>();
            services.AddTransient<IStaffService, StaffService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IClaimService, ClaimService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IRoundingRuleService, RoundingRuleService>();
            services.AddTransient<IPatientEncounterService, PatientEncounterService>();
            services.AddTransient<IPaperClaimService, PaperClaimService>();
            //organization
            services.AddTransient<IOrganizationService, OrganizationService>();

            services.AddTransient<IPayerInformationService, PayerInformationService>();
            services.AddTransient<IPatientAppointmentService, PatientAppointmentService>();
            services.AddTransient<IPatientMedicalFamilyHistoryService, PatientMedicalFamilyHistoryService>();

            services.AddTransient<IAuditLogService, AuditLogService>();

           //services.AddTransient<IEDI837GenerationService, EDI837GenerationService>();
            services.AddTransient<EDIGenerator.IServices.IEDI837Service, EDI837Service>();
            services.AddTransient<ISecurityQuestionService, SecurityQuestionService>();
            services.AddTransient<IRolePermissionService, RolePermissionService>();
            //services.AddTransient<IEDI835ParserService, EDI835ParserService>();
            services.AddTransient<IEDI835Service, EDI835Service>();
            services.AddTransient<IRolePermissionService, RolePermissionService>();
            services.AddTransient<IEdiGatewayService, EdiGatewayService>();
            //services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IStaffAvailabilityService, StaffAvailabilityService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<ITelehealthService, TelehealthService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IAdminDashboardService, AdminDashboardService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IMasterModifierService, MasterModifierService>();
            services.AddTransient<IMasterServiceCodesService, MasterServiceCodesService>();
            services.AddTransient<IPayerServiceCodesService, PayerServiceCodesService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IStaffLeaveService, StaffLeaveService>();
            services.AddTransient<IPatientGuardianService, PatientGuardianService>();
            services.AddTransient<IPatientPhoneAddressService, PatientPhoneAddressService>();
            services.AddTransient<IPatientsInsuranceService, PatientsInsuranceService>();
            services.AddTransient<IPatientCustomLabelService, PatientCustomLabelService>();
            services.AddTransient<IGlobalCodeService, GlobalCodeService>();
            //services.AddTransient<IPayrollGroupService, PayrollGroupService>();
            services.AddTransient<IPatientSocialHistoryService, PatientSocialHistoryService>();
            services.AddTransient<IPatientImmunizationService, PatientImmunizationService>();
            services.AddTransient<IPatientDiagnosisService, PatientDiagnosisService>();
            services.AddTransient<IStaffTimesheetService, StaffTimesheetService>();
            services.AddTransient<IStaffCustomLabelService, StaffCustomLabelService>();
            services.AddTransient<IAppointmentTypeService, AppointmentTypeService>();
            services.AddTransient<IMasterCustomLabelService, MasterCustomLabelService>();
            //services.AddTransient<IPayrollService, PayrollService>();
            services.AddTransient<IMasterTagService, MasterTagService>();
            services.AddTransient<IPatientAllergyService, PatientAllergyService>();
            services.AddTransient<IPatientMedicationService, PatientMedicationService>();
            services.AddTransient<IPatientVitalService, PatientVitalService>();
            services.AddTransient<IAppConfigurationService, AppConfigurationService>();

            services.AddTransient<IMasterICDService, MasterICDService>();
            services.AddTransient<IMasterInsuranceTypeService, MasterInsuranceTypeService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IUserRoleService, UserRoleService>();
            //Agency Holidays
            services.AddTransient<IAgencyHolidaysService, AgencyHolidaysService>();
            services.AddTransient<IMasterSecurityQuestionService, MasterSecurityQuestionService>();
            services.AddTransient<IPayerService, PayerService>();
            services.AddTransient<IPayerActivityService, PayerActivityService>();
            //services.AddTransient<IPayrollBreaktimeService, PayrollBreaktimeService>();
            services.AddTransient<IUserPasswordHistoryService, UserPasswordHistoryService>();
            services.AddTransient<IStaffPayrollRateForActivityService, StaffPayrollRateForActivityService>();
            services.AddTransient<IEDI270GenerationService, EDI270GenerationService>();
            services.AddTransient<IEDI270Service, EDI270Service>();
            services.AddTransient<IEDI999ParserService, EDI999ParserService>();
            services.AddTransient<IEDI999Service, EDI999Service>();
            services.AddTransient<IEDI271Service, EDI271Service>();
            services.AddTransient<IEDI271ParserService, EDI271ParserService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IQuestionnaireService, QuestionnaireService>();
            services.AddTransient<ISmsService, SmsService>();
           // services.AddTransient<ITwilioRestClient, CustomTwilioClient>();

            // MasterTempate
            services.AddTransient<IMasterTemplatesService, MasterTemplatesService>();
            services.AddTransient<ICheckListService, CheckListService>();
            services.AddTransient<IMasterCheckListService, MasterCheckListService>();
            services.AddTransient<IMasterQuestionnaireService, MasterQuestionnaireService>();
















            // Video And Articles
            services.AddTransient<IVideoAndArticlesService, VideoAndArticlesService>();
            #endregion
            return services;
        }
    }
}
