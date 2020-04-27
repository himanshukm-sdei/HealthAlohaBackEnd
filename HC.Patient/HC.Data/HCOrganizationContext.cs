using HC.Patient.Data.Configuration.Organization;
using HC.Patient.Entity;
using HC.Patient.Model.Claim;
using HC.Patient.Model.Common;
using HC.Patient.Model.Message;
using HC.Patient.Model.Patient;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Model.PatientEncounters;
using HC.Patient.Model.Payer;
using HC.Patient.Model.Questionnaire;
using HC.Patient.Model.RolePermission;
using HC.Patient.Model.Staff;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Data
{
    public class HCOrganizationContext : DbContext
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private ConfigurationBuilderContext _configurationBuilderContext = new ConfigurationBuilderContext();
        /// <summary>
        /// Db context for Patient module
        /// </summary>
        /// <param name="options"></param>
        public HCOrganizationContext(DbContextOptions<HCOrganizationContext> options, IHttpContextAccessor contextAccessor) : base(options) { _contextAccessor = contextAccessor; }

        /// <summary>
        /// override the context from http request
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var NewDbConnection = new ConfigurationBuilderContext();

            //check http request is not null
            if (_contextAccessor.HttpContext != null)
            {
                //create new connection string from the http context
                string con = NewDbConnection.GetNewConnection(_contextAccessor.HttpContext);
                if (!string.IsNullOrEmpty(con))
                {
                    //create new option builder with new connection string
                    optionsBuilder.UseSqlServer(con);
                }
            }
            else
            {
                //first time when application load the "HttpContext" is null so we bind database wirth static one
                //if http context null then bind the context with default connection string
                optionsBuilder.UseSqlServer(_configurationBuilderContext.CreateOrganizationConnectionString());
            }
        }


        public DbSet<Patients> Patients { get; set; }
        public DbSet<PatientInvite> PatientInvite { get; set; }
        public DbSet<PatientsInvite> PatientsInvite { get; set; }
        public DbSet<PatientSurgery> PatientSurgery { get; set; }
        public DbSet<PatientSurgeon> PatientSurgeon { get; set; }
        public DbSet<PatientSurgeryImages> PatientSurgeryImages { get; set; }
        
        public DbSet<User> User { get; set; }
        public DbSet<UserDocuments> UserDocuments { get; set; }
        //public DbSet<PatientEmploymentDetail> PatientEmploymentDetail { get; set; }
        public DbSet<PatientGuardian> PatientGuardian { get; set; }
        public DbSet<PatientInsuranceDetails> PatientInsuranceDetails { get; set; }
        public DbSet<PatientAddress> PatientAddress { get; set; }        
        public DbSet<MasterReferral> MasterReferral { get; set; }
        public DbSet<PhoneNumbers> PhoneNumbers { get; set; }
        public DbSet<PatientVitals> PatientVitals { get; set; }
        public DbSet<MasterICD> MasterICD { get; set; }
        public DbSet<MasterDegree> MasterDegree { get; set; }
        public DbSet<MasterServiceCode> MasterServiceCode { get; set; }
        public DbSet<MasterUnitType> MasterUnitType { get; set; }
        public DbSet<PatientDiagnosis> PatientDiagnosis { get; set; }
        public DbSet<PatientEncounter> PatientEncounter { get; set; }
        public DbSet<PatientMedicalFamilyHistory> PatientMedicalFamilyHistory { get; set; }

        public DbSet<MasterVFCEligibility> MasterVFCEligibility { get; set; }
        public DbSet<MasterAdministrationSite> MasterAdministrationSite { get; set; }

        public DbSet<PatientImmunization> PatientImmunization { get; set; }
        public DbSet<PatientSocialHistory> PatientSocialHistory { get; set; }        
        public DbSet<MasterLonic> MasterLonic { get; set; }
        public DbSet<PatientLabTest> PatientLabTest { get; set; }
        public DbSet<MasterLabs> MasterLabs { get; set; }

        public DbSet<MasterRelationship> MasterRelationship { get; set; }
        public DbSet<MasterModifiers> MasterModifiers { get; set; }

        public DbSet<AppointmentType> AppointmentType { get; set; }
        public DbSet<PatientAppointment> PatientAppointment { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<MasterRejectionReason> MasterRejectionReason { get; set; }
        public DbSet<OrganizationSubscriptionPlan> OrganizationSubscriptionPlan { get; set; }
        public DbSet<OrganizationSMTPDetails> OrganizationSMTPDetails { get; set; }

        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<Modules> Modules { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<AuditLogs> AuditLogs { get; set; }
        public DbSet<Location> Location { get; set; }

        public DbSet<PatientAllergies> PatientAllergies { get; set; }
        //Favourite
        public DbSet<UserFavourites> UserFavourites { get; set; }
        public DbSet<Authorization> Authorization { get; set; }
        public DbSet<AuthorizationProcedures> AuthorizationProcedures { get; set; }
        //
        public DbSet<PatientTags> PatientTags { get; set; }
        public DbSet<MasterTags> MasterTags { get; set; }
        public DbSet<MasterCustomLabels> MasterCustomLabels { get; set; }
        public DbSet<PatientCustomLabels> PatientCustomLabels { get; set; }
        public DbSet<AuthenticationToken> AuthenticationToken { get; set; }
        public DbSet<InsuredPerson> InsuredPerson { get; set; }
        public DbSet<MasterPatientLocation> MasterPatientLocation { get; set; }
        public DbSet<AuthProcedureCPT> AuthProcedureCPT { get; set; }
        public DbSet<AuthProcedureCPTModifiers> AuthProcedureCPTModifiers { get; set; }
        public DbSet<Staffs> Staffs { get; set; }
        public DbSet<StaffLocation> StaffLocation { get; set; }
        public DbSet<MasterTemplates> MasterTemplates { get; set; }      


        public DbSet<PatientEncounterDiagnosisCodes> PatientEncounterDiagnosisCodes { get; set; }
        //public DbSet<StaffAddress> StaffAddress { get; set; }

        public DbSet<PatientEncounterCodesMapping> PatientEncounterCodesMapping { get; set; }

        public DbSet<PatientEncounterServiceCodes> PatientEncounterServiceCodes { get; set; }

        public DbSet<StaffCustomLabels> StaffCustomLabels { get; set; }
        public DbSet<PatientMedication> PatientMedication { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        //public DbSet<GlobalCodeCategory> GlobalCodeCategory { get; set; }
        //public DbSet<GlobalCode> GlobalCode { get; set; }      
        public DbSet<MasterRoundingRules> MasterRoundingRules { get; set; }

        public DbSet<RoundingRuleDetails> RoundingRuleDetails { get; set; }
        //public DbSet<AppointmentFilter> AppointmentFilter { get; set; }
        //public DbSet<AppointmentFilterLocation> AppointmentFilterLocation { get; set; }
        //public DbSet<AppointmentFilterStaff> AppointmentFilterStaff { get; set; }
        //public DbSet<AppointmentFilterPatient> AppointmentFilterPatient { get; set; }

        public DbSet<MasterInsuranceType> MasterInsuranceTypes { get; set; }

        public DbSet<PayerServiceCodes> PayerServiceCodes { get; set; }
        public DbSet<PayerServiceCodeModifiers> PayerServiceCodeModifiers { get; set; }

        public DbSet<PayerAppointmentTypes> PayerAppointmentTypes { get; set; }
        public DbSet<SoapNotes> SoapNotes { get; set; }
        public DbSet<MasterNoteType> MasterNoteType { get; set; }
        public DbSet<ClaimDiagnosisCode> ClaimDiagnosisCode { get; set; }
        public DbSet<ClaimEncounters> ClaimEncounters { get; set; }
        public DbSet<Claims> Claims { get; set; }
        public DbSet<ClaimServiceLine> ClaimServiceLine { get; set; }
        public DbSet<MasterClaimStatus> MasterClaimStatus { get; set; }
        public DbSet<EDIGateway> EDIGateway { get; set; }
        public DbSet<EDI999AcknowledgementMaster> EDI999AcknowledgementMaster { get; set; }

        public DbSet<InsuranceCompanies> InsuranceCompanies { get; set; }
        public DbSet<UserRoleType> UserRoleType { get; set; }        
        public DbSet<AppointmentStaff> AppointmentStaff { get; set; }
        public DbSet<MasterAllergies> MasterAllergies { get; set; }
        public DbSet<MasterEthnicity> MasterEthnicity { get; set; }

        //public DbSet<MasterPatientCommPreferences> MasterPatientCommPreferences { get; set; }

        public DbSet<StaffTags> StaffTags { get; set; }
        public DbSet<PatientMedicalFamilyHistoryDiseases> PatientMedicalFamilyHistoryDiseases { get; set; }


        //Masters
        public DbSet<MasterCountry> MasterCountry { get; set; }        
        public DbSet<MasterRace> MasterRace { get; set; }
        public DbSet<MasterState> MasterState { get; set; }
        //public DbSet<MasterOccupation> MasterOccupation { get; set; }
        public DbSet<MasterProgram> MasterProgram { get; set; }
        public DbSet<MasterGender> MasterGender { get; set; }
        public DbSet<MasterImmunization> MasterImmunization { get; set; }
        //public DbSet<MasterPreferredLanguage> MasterPreferredLanguage { get; set; }
        public DbSet<MasterReaction> MasterReaction { get; set; }
        public DbSet<SecurityQuestions> SecurityQuestions { get; set; }
        public DbSet<UserSecurityQuestionAnswer> UserSecurityQuestionAnswer { get; set; }

        public DbSet<MasterManufacture> MasterManufacture { get; set; }
        //public DbSet<MasterLabs> MasterLabs { get; set; }        
        //public DbSet<MasterOrganization> MasterOrganization { get; set; }

        public DbSet<MasterRouteOfAdministration> MasterRouteOfAdministration { get; set; }
        public DbSet<MasterImmunityStatus> MasterImmunityStatus { get; set; }
        public DbSet<GlobalCodeCategory> GlobalCodeCategory { get; set; }
        public DbSet<GlobalCode> GlobalCode { get; set; }
        public DbSet<MasterWeekDays> MasterWeekDays { get; set; }

        public DbSet<SubcriptionPlan> SubcriptionPlan { get; set; }
        public DbSet<AppConfigurations> AppConfigurations { get; set; }

        public DbSet<StaffAvailability> StaffAvailability { get; set; }

        public DbSet<AuditLogTable> AuditLogTable { get; set; }

        public DbSet<AuditLogColumn> AuditLogColumn { get; set; }

        public DbSet<Claim837Batch> Claim837Batch { get; set; }
        public DbSet<ClaimResubmissionReason> ClaimResubmissionReason { get; set; }

        public DbSet<Claim837Claims> Claim837Claims { get; set; }

        public DbSet<Claim837ServiceLine> Claim837ServiceLine { get; set; }

        public DbSet<Screens> Screens { get; set; }

        public DbSet<Actions> Actions { get; set; }

        public DbSet<ModulePermissions> ModulePermissions { get; set; }

        public DbSet<ScreenPermissions> ScreenPermissions { get; set; }

        public DbSet<ActionPermissions> ActionPermissions { get; set; }
        public DbSet<MachineLoginLog> MachineLoginLog { get; set; }
        public DbSet<StaffTeam> StaffTeam { get; set; }
        public DbSet<MasterPaymentType> MasterPaymentType { get; set; }
        public DbSet<MasterPaymentDescription> MasterPaymentDescription { get; set; }
        public DbSet<PaymentCheckDetail> PaymentCheckDetail { get; set; }
        public DbSet<ClaimServiceLinePaymentDetails> ClaimServiceLinePaymentDetails { get; set; }

        public DbSet<Claim835Batch> Claim835Batch { get; set; }

        public DbSet<Claim835Claims> Claim835Claims { get; set; }

        public DbSet<Claim835ServiceLine> Claim835ServiceLine { get; set; }

        public DbSet<Claim835ServiceLineAdjustmentDetail> Claim835ServiceLineAdjustmentDetail { get; set; }

        public DbSet<MasterClaimStatusCode> MasterClaimStatusCode { get; set; }

        public DbSet<MasterDiscipline> MasterDiscipline { get; set; }
        public DbSet<MasterPayrollGroup> MasterPayrollGroup { get; set; }
        //public DbSet<PatientGuardianContactType> PatientGuardianContactType { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuPermissions> MenuPermissions { get; set; }
        public DbSet<TelehealthSessionDetails> TelehealthSessionDetails { get; set; }

        public DbSet<TelehealthTokenDetails> TelehealthTokenDetails { get; set; }
        public DbSet<ClaimHistory> ClaimHistory { get; set; }
        public DbSet<MasterDocumentTypes> MasterDocumentTypes { get; set; }
        public DbSet<MasterDocumentTypesStaff> MasterDocumentTypesStaff { get; set; }

        public DbSet<AppointmentAuthorization> AppointmentAuthorization { get; set; }

        //Message System
        public DbSet<Message> Message { get; set; }
        public DbSet<MessageRecepient> MessageRecepient { get; set; }
        public DbSet<MasterAdjustmentGroupCode> MasterAdjustmentGroupCode { get; set; }
        public DbSet<MasterServiceCodeModifiers> MasterServiceCodeModifiers { get; set; }
        public DbSet<MasterPayrollBreakTime> MasterPayrollBreakTime { get; set; }        
        public DbSet<StaffLeave> StaffLeave { get; set; }
        public DbSet<Holidays> Holidays { get; set; }
        // public DbSet<UserTimesheet> UserTimesheet { get; set; }
        public DbSet<UserTimesheetByAppointmentType> UserTimesheetByAppointmentType { get; set; }
        //public DbSet<UserDriveTime> UserDriveTime { get; set; }
        //public DbSet<UserDetailedDriveTime> UserDetailedDriveTime { get; set; }        
        public DbSet<EncounterSignature> EncounterSignature { get; set; }
        public DbSet<UserPasswordHistory> UserPasswordHistory { get; set; }
        public DbSet<ClaimProviders> ClaimProviders { get; set; }
        public DbSet<EligibilityEnquiryServiceTypeMaster> EligibilityEnquiryServiceTypeMaster { get; set; }
        public DbSet<EligibilityEnquiry270Master> EligibilityEnquiry270Master { get; set; }
        public DbSet<EligibilityEnquiry270ServiceTypeDetails> EligibilityEnquiry270ServiceTypeDetails { get; set; }
        public DbSet<EligibilityEnquiry270ServiceCodesDetails> EligibilityEnquiry270ServiceCodesDetails { get; set; }
        public DbSet<StaffPayrollRateForActivity> StaffPayrollRateForActivity { get; set; }
        public DbSet<MasterEligibilityEnquiryStatus> MasterEligibilityEnquiryStatus { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<ChatConnectedUser> ChatConnectedUser { get; set; }
        //Questionnaire tables
        public DbSet<DFA_Category> DFA_Category { get; set; }
        public DbSet<DFA_CategoryCode> DFA_CategoryCode { get; set; }
        public DbSet<DFA_Document> DFA_Document { get; set; }
        public DbSet<DFA_Section> DFA_Section { get; set; }
        public DbSet<DFA_SectionItem> DFA_SectionItem { get; set; }
        public DbSet<DFA_DocumentAnswer> DFA_DocumentAnswer { get; set; }
        public DbSet<DFA_PatientDocuments> DFA_PatientDocuments { get; set; }
    
        public DbSet<PatientEncounterTemplates> PatientEncounterTemplates { get; set; }
        public DbSet<CheckListCategory> CheckListCategory { get; set; }
        public DbSet<CheckList> CheckList { get; set; }       
        public DbSet<VideoAndArticlesCategories> VideoAndArticlesCategories { get; set; }
        public DbSet<VideoAndArticles> VideoAndArticles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MessageConfig());
            modelBuilder.ApplyConfiguration(new MessageRecepientConfig());
            modelBuilder.ApplyConfiguration(new MessageDocumentsConfig());
            modelBuilder.ApplyConfiguration(new ModifiersConfig());
            modelBuilder.ApplyConfiguration(new PayerModifiersConfig());
            modelBuilder.ApplyConfiguration(new StaffTeamConfig());
            modelBuilder.ApplyConfiguration(new StaffPayrollRateForActivityConfig());
            modelBuilder.ApplyConfiguration(new ChatConnectedUserConfig());

            modelBuilder.Entity<StaffLeave>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<StaffLeave>()

            .Property(b => b.IsDeleted)

            .HasDefaultValue(false);

            modelBuilder.Entity<StaffLeave>()

            .Property(b => b.IsActive)

            .HasDefaultValue(true);

            modelBuilder.Entity<StaffLeave>()
            .Property(b => b.ApprovalDate)
            .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<PayerAppointmentTypes>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<PayerAppointmentTypes>()

            .Property(b => b.IsDeleted)

            .HasDefaultValue(false);


            modelBuilder.Entity<PayerAppointmentTypes>()

            .Property(b => b.IsActive)

            .HasDefaultValue(true);


            modelBuilder.Entity<PayerServiceCodes>()

            .Property(b => b.CreatedDate)

            .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<PayerServiceCodes>()

            .Property(b => b.IsDeleted)

            .HasDefaultValue(false);


            modelBuilder.Entity<PayerServiceCodes>()

            .Property(b => b.IsActive)

            .HasDefaultValue(true);

            modelBuilder.Entity<PayerServiceCodes>()
            .Property(b => b.IsRequiredAuthorization)
            .HasDefaultValue(false);

            modelBuilder.Entity<PayerServiceCodes>()
            .Property(b => b.RuleID)
            .HasDefaultValue(1);

            //modelBuilder.Entity<PayerServiceCodes>()
            //.Property(b => b.NewRatePerUnit)
            //.HasDefaultValue(Convert.ToDecimal(0.00));
            //modelBuilder.Entity<PayerServiceCodes>()
            //.Property(b => b.EffectiveDate)
            //.HasDefaultValue(DateTime.Now);


            modelBuilder.Entity<AppointmentStaff>()

            .Property(b => b.CreatedDate)

            .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<AppointmentStaff>()

            .Property(b => b.IsDeleted)

            .HasDefaultValue(false);


            modelBuilder.Entity<AppointmentStaff>()

            .Property(b => b.IsActive)

            .HasDefaultValue(true);            


            modelBuilder.Entity<UserRoleType>()

            .Property(b => b.CreatedDate)

            .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<UserRoleType>()

            .Property(b => b.IsDeleted)

            .HasDefaultValue(false);


            modelBuilder.Entity<UserRoleType>()

            .Property(b => b.IsActive)

            .HasDefaultValue(true);

            modelBuilder.Entity<AppointmentType>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<AppointmentType>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<AppointmentType>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<AppointmentType>()
            .Property(b => b.FontColor)
            .HasDefaultValue("#ffffff");

            modelBuilder.Entity<EDIGateway>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<EDIGateway>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<EDIGateway>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);
            //

            modelBuilder.Entity<AuditLogs>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<MasterCustomLabels>()

            .Property(b => b.IsDeleted)

            .HasDefaultValue(false);


            modelBuilder.Entity<MasterCustomLabels>()

           .Property(b => b.CreatedDate)

           .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<MasterCustomLabels>()

            .Property(b => b.IsActive)

            .HasDefaultValue(true);


            modelBuilder.Entity<InsuranceCompanies>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<InsuranceCompanies>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<InsuranceCompanies>()
           .Property(b => b.IsPractitionerIsRenderingProvider)
           .HasDefaultValue(false);

            modelBuilder.Entity<InsuranceCompanies>()
           .Property(b => b.Form1500PrintFormat)
           .HasDefaultValue(1);


            // //AppointmentFilter
            // modelBuilder.Entity<AppointmentFilter>()
            // .Property(b => b.CreatedDate)
            // .HasDefaultValueSql("GetUtcDate()");

            // modelBuilder.Entity<AppointmentFilter>()
            //.Property(b => b.IsDeleted)
            // .HasDefaultValue(false);

            // modelBuilder.Entity<AppointmentFilter>()
            // .Property(b => b.IsActive)
            // .HasDefaultValue(true);

            //MasterInsuranceType
            modelBuilder.Entity<MasterInsuranceType>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterInsuranceType>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterInsuranceType>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //Location
            modelBuilder.Entity<Location>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<Location>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<Location>()
           .Property(b => b.IsActive)
           .HasDefaultValue(true);

            //MasterAdministrationSite
            modelBuilder.Entity<MasterAdministrationSite>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterAdministrationSite>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterAdministrationSite>()
           .Property(b => b.IsActive)
           .HasDefaultValue(true);

            ////MasterCountry
            //modelBuilder.Entity<MasterCountry>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            //modelBuilder.Entity<MasterCountry>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //MasterServiceCode
            modelBuilder.Entity<MasterServiceCode>()

            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterServiceCode>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterServiceCode>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<MasterServiceCode>()
            .Property(b => b.IsRequiredAuthorization)
            .HasDefaultValue(false);

            //MasterICD
            modelBuilder.Entity<MasterICD>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterICD>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterICD>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<MasterICD>()
            .Property(b => b.CodeType)
            .HasDefaultValue("ICD10");

            //MasterDegree
            modelBuilder.Entity<MasterDegree>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterDegree>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterDegree>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //MasterLabs
            modelBuilder.Entity<MasterLabs>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterLabs>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //MasterLonic
            modelBuilder.Entity<MasterLonic>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterLonic>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //MasterReferral
            modelBuilder.Entity<MasterReferral>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterReferral>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);


            //MasterVFCEligibility
            modelBuilder.Entity<MasterVFCEligibility>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterVFCEligibility>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);


            //Organization
            modelBuilder.Entity<Organization>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<Organization>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<Organization>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            // //OrganizationDatabaseDetail
            // modelBuilder.Entity<OrganizationDatabaseDetail>()
            //  .Property(b => b.IsDeleted)
            //  .HasDefaultValue(false);

            // modelBuilder.Entity<OrganizationDatabaseDetail>()
            //.Property(b => b.IsActive)
            //.HasDefaultValue(true);

            // modelBuilder.Entity<OrganizationDatabaseDetail>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //OrganizationSMTPDetails
            modelBuilder.Entity<OrganizationSMTPDetails>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<OrganizationSMTPDetails>()
           .Property(b => b.IsActive)
           .HasDefaultValue(true);

            modelBuilder.Entity<OrganizationSMTPDetails>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            //OrganizationSubscriptionPlan
            modelBuilder.Entity<OrganizationSubscriptionPlan>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<OrganizationSubscriptionPlan>()
           .Property(b => b.IsActive)
           .HasDefaultValue(true);

            modelBuilder.Entity<OrganizationSubscriptionPlan>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");



            //PatientAddress
            modelBuilder.Entity<PatientAddress>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientAddress>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientAddress>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<PatientAddress>()
            .Property(b => b.IsPrimary)
            .HasDefaultValue(false);

            //PatientAppointment
            modelBuilder.Entity<PatientAppointment>()
           .Property(b => b.IsDeleted)
           .HasDefaultValue(false);

            modelBuilder.Entity<PatientAppointment>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<PatientAppointment>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientAppointment>()
            .Property(b => b.IsRecurrence)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientAppointment>()
            .Property(b => b.IsExcludedFromMileage)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientAppointment>()
            .Property(b => b.IsTelehealthAppointment)
            .HasDefaultValue(false);

            //modelBuilder.Entity<PatientAppointment>()
            //.Property(b => b.IsDirectService)
            //.HasDefaultValue(true);

            //PatientDiagnosis
            modelBuilder.Entity<PatientDiagnosis>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientDiagnosis>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //modelBuilder.Entity<PatientDiagnosis>()
            //.Property(b => b.IsActive)
            //.HasDefaultValue(true);

            modelBuilder.Entity<PatientDiagnosis>()
            .Property(b => b.IsPrimary)
            .HasDefaultValue(false);

            //UserDocuments
            modelBuilder.Entity<UserDocuments>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<UserDocuments>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<UserDocuments>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            ////PatientEmploymentDetail
            //modelBuilder.Entity<PatientEmploymentDetail>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //modelBuilder.Entity<PatientEmploymentDetail>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            //PatientEncounter
            modelBuilder.Entity<PatientEncounter>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientEncounter>()
            .Property(b => b.DateOfService)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientEncounter>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientEncounter>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            // modelBuilder.Entity<PatientEncounter>()
            //.Property(b => b.IsBillableEncounter)
            //.HasDefaultValue(true);
            //PatientGuardian
            modelBuilder.Entity<PatientGuardian>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientGuardian>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientGuardian>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //PatientImmunization
            modelBuilder.Entity<PatientImmunization>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientImmunization>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientImmunization>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //PatientInsuranceDetails
            modelBuilder.Entity<PatientInsuranceDetails>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientInsuranceDetails>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientInsuranceDetails>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //PatientLabTest
            modelBuilder.Entity<PatientLabTest>()
           .Property(b => b.IsDeleted)
           .HasDefaultValue(false);

            modelBuilder.Entity<PatientLabTest>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientLabTest>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //PatientMedicalFamilyHistory
            modelBuilder.Entity<PatientMedicalFamilyHistory>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientMedicalFamilyHistory>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientMedicalFamilyHistory>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //PatientMedicalFamilyHistoryDiseases
            modelBuilder.Entity<PatientMedicalFamilyHistoryDiseases>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientMedicalFamilyHistoryDiseases>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientMedicalFamilyHistoryDiseases>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);            
                        
            //Patients
            modelBuilder.Entity<Patients>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //modelBuilder.Entity<Patients>()
            //.Property(b => b.ClientStatus)
            //.HasDefaultValue(2);//2 is for Active Status

            modelBuilder.Entity<Patients>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUTCDate()");

            modelBuilder.Entity<Patients>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<Patients>()
            .Property(b => b.IsPortalActivate)
            .HasDefaultValue(false);

            modelBuilder.Entity<Patients>()
            .Property(b => b.IsPortalRequired)
            .HasDefaultValue(false);

            //PatientSocialHistory
            modelBuilder.Entity<PatientSocialHistory>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientSocialHistory>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientSocialHistory>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //PatientVitals
            modelBuilder.Entity<PatientVitals>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientVitals>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientVitals>()
           .Property(b => b.IsActive)
           .HasDefaultValue(true);

            //PhoneNumbers
            modelBuilder.Entity<PhoneNumbers>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PhoneNumbers>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PhoneNumbers>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //PatientAllergies
            modelBuilder.Entity<PatientAllergies>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientAllergies>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //UserFavourites           
            modelBuilder.Entity<UserFavourites>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<UserFavourites>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<UserFavourites>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //MasterTags
            modelBuilder.Entity<MasterTags>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterTags>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterTags>()
           .Property(b => b.IsActive)
           .HasDefaultValue(true);

            modelBuilder.Entity<MasterTags>()
           .Property(b => b.FontColorCode)
           .HasDefaultValue("#000000");

            //PatientTags
            modelBuilder.Entity<PatientTags>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<PatientTags>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientTags>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //Staff Tags
            modelBuilder.Entity<StaffTags>()
            .Property(b => b.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<StaffTags>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<StaffTags>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //User
            modelBuilder.Entity<User>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<User>()
            .HasIndex(x => new { x.UserName });

            modelBuilder.Entity<User>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<User>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<User>()
            .Property(b => b.AccessFailedCount)
            .HasDefaultValue(0);

            modelBuilder.Entity<User>()
            .Property(b => b.IsBlock)
            .HasDefaultValue(false);

            //UserRoles
            modelBuilder.Entity<UserRoles>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //modelBuilder.Entity<UserRoles>()
            //.Property(b => b.IsActive)
            //.HasDefaultValue(true);

            //Authorization
            modelBuilder.Entity<Authorization>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<Authorization>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<Authorization>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //AuthorizationProcedures
            modelBuilder.Entity<AuthorizationProcedures>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<AuthorizationProcedures>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<AuthorizationProcedures>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //AuthorizationProcedures
            modelBuilder.Entity<AuthProcedureCPT>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<AuthProcedureCPT>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<AuthProcedureCPT>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //AuthProcedureCPTModifiers
            modelBuilder.Entity<AuthProcedureCPTModifiers>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<AuthProcedureCPTModifiers>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<AuthProcedureCPTModifiers>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);
            
            //PatientCustomLabels
            modelBuilder.Entity<PatientCustomLabels>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientCustomLabels>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientCustomLabels>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //PatientCustomLabels
            modelBuilder.Entity<AuthenticationToken>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<AuthenticationToken>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<AuthenticationToken>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //InsuredPerson
            modelBuilder.Entity<InsuredPerson>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<InsuredPerson>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<InsuredPerson>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //InsuredPerson
            modelBuilder.Entity<MasterPatientLocation>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterPatientLocation>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterPatientLocation>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //modelBuilder.Entity<Authorization>().HasMany<AuthorizationProcedures>()
            //    .WithOne(o => o.Authorization).HasForeignKey(p => p.Id);

            //modelBuilder.Entity<AuthorizationProcedures>().HasMany<AuthProcedureCPTLink>()
            //     .WithOne(o => o.AuthorizationProcedures).HasForeignKey(p => p.Id);



            //Staff
            modelBuilder.Entity<Staffs>()
          .Property(b => b.CreatedDate)
          .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<Staffs>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<Staffs>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<Staffs>()
            .Property(b => b.IsRenderingProvider)
            .HasDefaultValue(false);

            //StaffCustomLabels
            modelBuilder.Entity<StaffCustomLabels>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<StaffCustomLabels>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<StaffCustomLabels>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);




            //Patient Medication
            modelBuilder.Entity<PatientMedication>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientMedication>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);


            //modelBuilder.Entity<PatientMedication>()

            //MasterWeekDays
            modelBuilder.Entity<MasterWeekDays>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterWeekDays>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterWeekDays>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //MasterTemplates
            //modelBuilder.Entity<MasterTemplates>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //modelBuilder.Entity<MasterTemplates>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            //modelBuilder.Entity<MasterTemplates>()
            //.Property(b => b.IsActive)
            //.HasDefaultValue(true);

            //AvailabilityTemplate
            modelBuilder.Entity<StaffAvailability>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<StaffAvailability>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<StaffAvailability>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //RoundingRules
            modelBuilder.Entity<MasterRoundingRules>()
          .Property(b => b.CreatedDate)
          .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterRoundingRules>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterRoundingRules>()

            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RolePermission>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<RolePermission>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //InsuranceCompanies
            modelBuilder.Entity<InsuranceCompanies>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<InsuranceCompanies>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //modelBuilder.Entity<InsuranceCompanies>()
            //.Property(b => b.IsActive)
            //.HasDefaultValue(true);


            //MasterImmunityStatus
            modelBuilder.Entity<MasterImmunityStatus>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterImmunityStatus>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterImmunityStatus>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //MasterRouteOfAdministration
            modelBuilder.Entity<MasterRouteOfAdministration>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterRouteOfAdministration>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterRouteOfAdministration>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            // modelBuilder.Entity<Modules>()
            //.Property(b => b.IsDeleted)
            // .HasDefaultValue(false);

            // modelBuilder.Entity<Modules>()
            // .Property(b => b.OrganizationID)
            // .HasDefaultValue(1);

            // modelBuilder.Entity<Modules>()
            // .Property(b => b.IsActive)
            // .HasDefaultValue(true);

            //MasterCountry
            modelBuilder.Entity<MasterCountry>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterCountry>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");


            ////MasterFundingSource
            //modelBuilder.Entity<MasterFundingSource>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            //modelBuilder.Entity<MasterFundingSource>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //MasterGender
            modelBuilder.Entity<MasterGender>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterGender>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //MasterImmunization
            modelBuilder.Entity<MasterImmunization>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterImmunization>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            ////MasterImmunization
            //modelBuilder.Entity<MasterLabs>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //modelBuilder.Entity<MasterLabs>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            //MasterManufacture
            modelBuilder.Entity<MasterManufacture>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterManufacture>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            ////MasterOccupation
            //modelBuilder.Entity<MasterOccupation>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetDate()");

            //modelBuilder.Entity<MasterOccupation>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            ////MasterPreferredLanguage
            //modelBuilder.Entity<MasterPreferredLanguage>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            //modelBuilder.Entity<MasterPreferredLanguage>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetDate()");

            //MasterProgram
            modelBuilder.Entity<MasterProgram>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterProgram>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //MasterRace
            modelBuilder.Entity<MasterRace>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterRace>()
           .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            //MasterState
            modelBuilder.Entity<MasterState>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterState>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            //Organization
            modelBuilder.Entity<Organization>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<Organization>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            //InsuranceCompanies
            modelBuilder.Entity<GlobalCode>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<GlobalCode>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //InsuranceCompanies
            modelBuilder.Entity<GlobalCodeCategory>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<GlobalCodeCategory>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RolePermission>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<RolePermission>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //InsuranceCompanies
            modelBuilder.Entity<InsuranceCompanies>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<InsuranceCompanies>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<InsuranceCompanies>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);


            //InsuranceCompanies
            modelBuilder.Entity<MasterImmunityStatus>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterImmunityStatus>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterImmunityStatus>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //MasterRouteOfAdministration
            modelBuilder.Entity<MasterRouteOfAdministration>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterRouteOfAdministration>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterRouteOfAdministration>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //MasterCountry
            modelBuilder.Entity<MasterCountry>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterCountry>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");


            ////MasterFundingSource
            //modelBuilder.Entity<MasterFundingSource>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            //modelBuilder.Entity<MasterFundingSource>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //MasterGender
            modelBuilder.Entity<MasterGender>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterGender>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //MasterImmunization
            modelBuilder.Entity<MasterImmunization>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterImmunization>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            ////MasterImmunization
            //modelBuilder.Entity<MasterLabs>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //modelBuilder.Entity<MasterLabs>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            //MasterManufacture
            modelBuilder.Entity<MasterManufacture>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterManufacture>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            ////MasterOccupation
            //modelBuilder.Entity<MasterOccupation>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //modelBuilder.Entity<MasterOccupation>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            ////MasterPreferredLanguage
            modelBuilder.Entity<MasterPreferredLanguage>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterPreferredLanguage>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            //MasterProgram
            modelBuilder.Entity<MasterProgram>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterProgram>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //MasterRace
            modelBuilder.Entity<MasterRace>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterRace>()
           .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            ////MasterReferral
            //modelBuilder.Entity<MasterReferral>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //modelBuilder.Entity<MasterReferral>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            //MasterState
            modelBuilder.Entity<MasterState>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterState>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            //Organization
            modelBuilder.Entity<Organization>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<Organization>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            //OrganizationConnectionstring
            // modelBuilder.Entity<OrganizationConnectionstring>()
            // .Property(b => b.IsDeleted)
            // .HasDefaultValue(false);

            // modelBuilder.Entity<OrganizationConnectionstring>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //InsuranceCompanies
            modelBuilder.Entity<GlobalCode>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<GlobalCode>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //InsuranceCompanies
            modelBuilder.Entity<GlobalCodeCategory>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<GlobalCodeCategory>()
           .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //AppConfirguration
            modelBuilder.Entity<AppConfigurations>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<AppConfigurations>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<AppConfigurations>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);


            //StaffLocation
            modelBuilder.Entity<StaffLocation>()
            .Property(b => b.IsDefault)
            .HasDefaultValue(false);

            //SecurityQuestion
            modelBuilder.Entity<SecurityQuestions>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<SecurityQuestions>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            //modelBuilder.Entity<SecurityQuestions>()
            //.Property(b => b.IsActive)
            //.HasDefaultValue(true);

            //UserSecurityQuestionAnswer
            modelBuilder.Entity<UserSecurityQuestionAnswer>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<UserSecurityQuestionAnswer>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<UserSecurityQuestionAnswer>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //StaffTeam Group
            modelBuilder.Entity<StaffTeam>()
          .Property(b => b.CreatedDate)
          .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<StaffTeam>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<StaffTeam>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //Master Payment Type
            modelBuilder.Entity<MasterPaymentType>()
          .Property(b => b.CreatedDate)
          .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterPaymentType>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterPaymentType>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //Master Payment Description
            modelBuilder.Entity<MasterPaymentDescription>()
          .Property(b => b.CreatedDate)
          .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterPaymentDescription>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterPaymentDescription>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //Payment Check Detail
            modelBuilder.Entity<PaymentCheckDetail>()
          .Property(b => b.CreatedDate)
          .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PaymentCheckDetail>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PaymentCheckDetail>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //Claim Service Line Payment Details
            modelBuilder.Entity<ClaimServiceLinePaymentDetails>()
          .Property(b => b.CreatedDate)
          .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<ClaimServiceLinePaymentDetails>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<ClaimServiceLinePaymentDetails>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //Claim Service Line Payment Details
            modelBuilder.Entity<MasterDiscipline>()
          .Property(b => b.CreatedDate)
          .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterDiscipline>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterDiscipline>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //Claim Service Line Payment Details
            modelBuilder.Entity<MasterPayrollGroup>()
          .Property(b => b.CreatedDate)
          .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterPayrollGroup>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterPayrollGroup>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //  //Claim Service Line Payment Details
            //  modelBuilder.Entity<PatientGuardianContactType>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //  modelBuilder.Entity<PatientGuardianContactType>()
            //  .Property(b => b.IsDeleted)
            //  .HasDefaultValue(false);

            //  modelBuilder.Entity<PatientGuardianContactType>()
            //  .Property(b => b.IsActive)
            //  .HasDefaultValue(true);

            //AuditLogTable
            modelBuilder.Entity<AuditLogTable>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<AuditLogTable>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //AuditLogColumn
            modelBuilder.Entity<AuditLogColumn>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<AuditLogColumn>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //MenuPermission
            modelBuilder.Entity<MenuPermissions>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<MenuPermissions>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<MenuPermissions>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<TelehealthSessionDetails>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<TelehealthSessionDetails>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);


            modelBuilder.Entity<TelehealthSessionDetails>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<TelehealthTokenDetails>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<TelehealthTokenDetails>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);


            modelBuilder.Entity<TelehealthTokenDetails>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //modelBuilder.Entity<TelehealthTokenDetails>()
            //.Property(b => b.IsStaffToken)
            //.HasDefaultValue(true);

            //ClaimHistory
            modelBuilder.Entity<ClaimHistory>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<ClaimHistory>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);


            modelBuilder.Entity<ClaimHistory>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<ClaimResubmissionReason>()
           .Property(a => a.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<ClaimResubmissionReason>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);


            modelBuilder.Entity<ClaimResubmissionReason>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //MasterDocumentTypes
            modelBuilder.Entity<MasterDocumentTypes>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");


            modelBuilder.Entity<MasterDocumentTypes>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);


            modelBuilder.Entity<MasterDocumentTypes>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);


            //MasterDocumentTypesStaff
            modelBuilder.Entity<MasterDocumentTypesStaff>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterDocumentTypesStaff>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterDocumentTypesStaff>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //Chat
            modelBuilder.Entity<Chat>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<Chat>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<Chat>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //DFA_Category
            modelBuilder.Entity<DFA_Category>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<DFA_Category>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<DFA_Category>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //DFA_CategoryCode
            modelBuilder.Entity<DFA_CategoryCode>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<DFA_CategoryCode>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<DFA_CategoryCode>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //DFA_Document
            modelBuilder.Entity<DFA_Document>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<DFA_Document>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<DFA_Document>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //DFA_Section
            modelBuilder.Entity<DFA_Section>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<DFA_Section>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<DFA_Section>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //DFA_SectionItem
            modelBuilder.Entity<DFA_SectionItem>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<DFA_SectionItem>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<DFA_SectionItem>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //DFA_DocumentAnswer
            modelBuilder.Entity<DFA_DocumentAnswer>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<DFA_DocumentAnswer>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<DFA_DocumentAnswer>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //DFA_PatientDocuments
            modelBuilder.Entity<DFA_PatientDocuments>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<DFA_PatientDocuments>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<DFA_PatientDocuments>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);


            //PatientEncounterTemplates
            modelBuilder.Entity<PatientEncounterTemplates>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<PatientEncounterTemplates>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<PatientEncounterTemplates>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);
        }
        public IList<TEntity> ExecStoredProcedureListWithOutput<TEntity>(string commandText, int totalOutputParams, params object[] parameters) where TEntity : class, new()
        {
            var connection = this.Database.GetDbConnection();
            IList<TEntity> result = new List<TEntity>();
            try
            {
                totalOutputParams = totalOutputParams == 0 ? 1 : totalOutputParams;
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result = DataReaderMapToList<TEntity>(reader);
                        reader.NextResult();
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
        private void AddParametersToDbCommand(string commandText, object[] parameters, System.Data.Common.DbCommand cmd)
        {
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    if (p != null)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
            }
        }
        public void ExecStoredProcedureListWithoutOutput(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    cmd.ExecuteReader();
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public PatientInfoDetails ExecStoredProcedureListWithOutputForPatientInfo(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            PatientInfoDetails result = new PatientInfoDetails();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    //////Please make sure reader should not open for this much time as in the below code to get n number of datasets.please avoid
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.PatientInfo = DataReaderMapToList<PatientInfo>(reader).ToList();
                        reader.NextResult();
                        result.PatientVitals = DataReaderMapToList<PatientVital>(reader).ToList();
                        reader.NextResult();
                        result.LastAppointmentDetails = DataReaderMapToList<LastAppointmentDetails>(reader).ToList();
                        reader.NextResult();
                        result.UpcomingAppointmentDetails = DataReaderMapToList<UpcomingAppointmentDetails>(reader).ToList();
                        reader.NextResult();
                        result.PatientDiagnosisDetails = DataReaderMapToList<PatientDiagnosisDetails>(reader).ToList();
                        reader.NextResult();
                        //added additional data (Prince Kumar 30-01-2018)
                        result.PhoneNumberModel = DataReaderMapToList<PhoneNumberModel>(reader).ToList();
                        reader.NextResult();
                        result.PatientTagsModel = DataReaderMapToList<PatientTagsModel>(reader).ToList();
                        reader.NextResult();
                        result.PatientAddressesModel = DataReaderMapToList<PatientAddressesModel>(reader).ToList();
                        reader.NextResult();
                        result.PatientAllergyModel = DataReaderMapToList<PatientsAllergyModel>(reader).ToList();
                        reader.NextResult();
                        result.PatientLabTestModel = DataReaderMapToList<PatientLabTestModel>(reader).ToList();
                        reader.NextResult();
                        result.PatientMedicalFamilyHistoryModel = DataReaderMapToList<PatientMedicalFamilyHistoryModel>(reader).ToList();
                        reader.NextResult();
                        result.PatientCustomLabelModel = DataReaderMapToList<PatientCustomLabelModel>(reader).ToList();
                        reader.NextResult();
                        result.PatientMedicationModel = DataReaderMapToList<PatientMedicationModel>(reader).ToList();
                        reader.NextResult();
                        result.Staffs = DataReaderMapToList<Staffs>(reader).ToList();
                        reader.NextResult();
                        result.PatientSocialHistory = DataReaderMapToList<PatientSocialHistory>(reader).ToList();
                        reader.NextResult();
                        result.PatientEncounter = DataReaderMapToList<PatientEncounter>(reader).ToList();
                        reader.NextResult();
                        result.PatientImmunization = DataReaderMapToList<PatientImmunization>(reader).ToList();
                        reader.NextResult();
                        result.ClaimServiceLine = DataReaderMapToList<ClaimServiceLine>(reader).ToList();
                        reader.NextResult();
                        result.Organization = DataReaderMapToList<Organization>(reader).ToList();
                        reader.NextResult();
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }

        public PatientHeaderModel ExecStoredProcedureListWithOutputForPatientHeaderInfo(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            PatientHeaderModel result = new PatientHeaderModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    //////Please make sure reader should not open for this much time as in the below code to get n number of datasets.please avoid
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.PatientBasicHeaderInfo = DataReaderMapToList<PatientBasicHeaderInfoModel>(reader).FirstOrDefault();
                        reader.NextResult();
                        result.PatientTagsHeaderInfo = DataReaderMapToList<PatientTagsHeaderInfoModel>(reader).ToList();
                        reader.NextResult();
                        result.PatientPhoneHeaderInfo = DataReaderMapToList<PatientPhoneHeaderInfoModel>(reader).ToList();
                        reader.NextResult();
                        result.PatientAllergyHeaderInfo = DataReaderMapToList<PatientAllergyHeaderInfoModel>(reader).ToList();
                        reader.NextResult();                        
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public NotificationModel ExecStoredProcedureListWithOutputForNotificationInfo(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            NotificationModel result = new NotificationModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    //////Please make sure reader should not open for this much time as in the below code to get n number of datasets.please avoid
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.MessageNotification = DataReaderMapToList<MessageNotificationModel>(reader).ToList();
                        reader.NextResult();
                        result.UserDocumentNotification = DataReaderMapToList<UserDocumentNotificationModel>(reader).ToList();
                        reader.NextResult();
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public SectionItemlistingModel ExecStoredProcedureListWithOutputForSectionItems(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            SectionItemlistingModel result = new SectionItemlistingModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    //////Please make sure reader should not open for this much time as in the below code to get n number of datasets.please avoid
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.SectionItems = DataReaderMapToList<SectionItemModel>(reader).ToList();
                        reader.NextResult();
                        result.Codes = DataReaderMapToList<CodeModel>(reader).ToList();
                        reader.NextResult();
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public SectionItemDDValueModel ExecStoredProcedureListWithOutputForSectionItemDDValues(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            SectionItemDDValueModel result = new SectionItemDDValueModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    //////Please make sure reader should not open for this much time as in the below code to get n number of datasets.please avoid
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.SectionItems = DataReaderMapToList<MasterDropDown>(reader).ToList();
                        reader.NextResult();
                        result.ControlTypes = DataReaderMapToList<MasterDropDown>(reader).ToList();
                        reader.NextResult();
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public AnswersModel ExecStoredProcedureGetPatientDocumentAnswer(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            AnswersModel result = new AnswersModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    //////Please make sure reader should not open for this much time as in the below code to get n number of datasets.please avoid
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.SectionItems = DataReaderMapToList<SectionItemModel>(reader).ToList();
                        reader.NextResult();
                        result.Codes = DataReaderMapToList<CodeModel>(reader).ToList();
                        reader.NextResult();
                        result.Answer = DataReaderMapToList<AnswerModel>(reader).ToList();
                        reader.NextResult();
                        result.DocumentSignature = DataReaderMapToList<DocumentSignatureModel>(reader).ToList();
                        reader.NextResult();
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public PatientEncounterModel ExecStoredProcedureForPatientEncounterDetail(string commandText, int totalOutputParams, bool isBillable, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            PatientEncounterModel result = new PatientEncounterModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result = DataReaderMapToList<PatientEncounterModel>(reader).FirstOrDefault();
                        reader.NextResult();
                        if (isBillable)
                        {
                            result.SOAPNotes = DataReaderMapToList<SOAPNotesModel>(reader).FirstOrDefault();
                            reader.NextResult();                            
                            result.PatientEncounterServiceCodes = DataReaderMapToList<PatientEncounterServiceCodesModel>(reader).ToList();
                            reader.NextResult();
                            result.PatientEncounterDiagnosisCodes = DataReaderMapToList<PatientEncounterDiagnosisCodesModel>(reader).ToList();
                            reader.NextResult();
                            //result.PatientEncounterCodesMapping = DataReaderMapToList<PatientEncounterCodesMappingModel>(reader).ToList();
                            //reader.NextResult();
                        }
                        if (((System.Data.SqlClient.SqlParameter)parameters[0]).ParameterName == "@EncounterId" && Convert.ToInt32(((System.Data.SqlClient.SqlParameter)parameters[0]).Value) > 0)
                        {
                            result.EncounterSignature = DataReaderMapToList<EncounterSignatureModel>(reader).ToList();
                            reader.NextResult();
                        }
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public ClaimsFullDetailModel ExecStoredProcedureForAllClaimDetailsWithServiceLine(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            ClaimsFullDetailModel result = new ClaimsFullDetailModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Claims = DataReaderMapToList<ClaimModel>(reader).ToList();
                        reader.NextResult();
                        result.ClaimServiceLines = DataReaderMapToList<ClaimServiceLineModel>(reader).ToList();
                        reader.NextResult();
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public GetAuthorizationByIdModel ExecStoredProcedureForGetAutorizationById(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            GetAuthorizationByIdModel result = new GetAuthorizationByIdModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Authorization = DataReaderMapToList<AuthModel>(reader).FirstOrDefault();
                        reader.NextResult();
                        result.AuthorizationProcedures = DataReaderMapToList<AuthProceduresModel>(reader).ToList();
                        reader.NextResult();
                        result.AuthorizationProcedureCPT = DataReaderMapToList<AuthProcedureCPTModel>(reader).ToList();
                        reader.NextResult();
                        result.AuthorizationProcedureCPTModifiers = DataReaderMapToList<AuthProcedureCPTModifierModel>(reader).ToList();
                        reader.NextResult();
                    }
                    return result;
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public StaffPatientModel ExecStoredProcedureForStaffPatientLocation(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            StaffPatientModel result = new StaffPatientModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Staff = DataReaderMapToList<HC.Patient.Model.PatientAppointment.StaffModel>(reader).ToList();
                        reader.NextResult();
                        result.Patients = DataReaderMapToList<PatModel>(reader).ToList();
                        reader.NextResult();
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public RolePermissionsModel ExecStoredProcedureListWithOutputForRolePermissions(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            RolePermissionsModel result = new RolePermissionsModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.ModulePermissions = DataReaderMapToList<ModulePermissionsModel>(reader).ToList();
                        reader.NextResult();
                        result.ScreenPermissions = DataReaderMapToList<ScreenPermissionsModel>(reader).ToList();
                        reader.NextResult();
                        result.ActionPermissions = DataReaderMapToList<ActionPermissonsModel>(reader).ToList();
                        reader.NextResult();
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }

        public Dictionary<string,object> ExecStoredProcedureListWithOutputForSubmittedClaimHistory(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Add("Claims", DataReaderMapToList<SubmittedClaimDetailsModel>(reader).ToList());
                        reader.NextResult();
                        result.Add("ServiceLines",DataReaderMapToList<SubmittedClaimServiceLineModel>(reader).ToList());
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public static IList<T> DataReaderMapToList<T>(IDataReader dr)
        {
            IList<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())               //Solution - Check if property is there in the reader and then try to remove try catch code
                {
                    try
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    catch { continue; }
                }
                list.Add(obj);
            }
            return list;
        }
        public PaperClaimModel ExecStoredProcedureForPaperClaimInfo(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            PaperClaimModel result = new PaperClaimModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.ClaimDetails = DataReaderMapToList<ClaimDetailsModel>(reader).ToList();
                        reader.NextResult();
                        result.InsuredDetails = DataReaderMapToList<InsuredModel>(reader).ToList();
                        reader.NextResult();
                        result.OtherInsuredDetails = DataReaderMapToList<InsuredModel>(reader).ToList();
                        reader.NextResult();
                        result.ServiceCodes = DataReaderMapToList<ServiceCodesModel>(reader).ToList();
                        reader.NextResult();
                        result.DiagnosisCodes = DataReaderMapToList<DiagnosisCodesModel>(reader).ToList();
                        reader.NextResult();
                        result.OrganizationDetails = DataReaderMapToList<OrganizationDetailsModel>(reader).FirstOrDefault();
                        reader.NextResult();
                        result.ServiceLocation = DataReaderMapToList<LocationAddressModel>(reader).FirstOrDefault();
                        reader.NextResult();
                        result.BillingLocation = DataReaderMapToList<LocationAddressModel>(reader).FirstOrDefault();
                        reader.NextResult();
                        if (commandText == SQLObjects.CLM_GetPaperClaimInfo_Secondary)
                        {
                            result.PayerPayments = DataReaderMapToList<PayerPaymentModel>(reader).ToList();
                            reader.NextResult();
                        }
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public EDI837FileModel ExecStoredProcedureForEDIInfo(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            EDI837FileModel result = new EDI837FileModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {

                        result.EDIDiagnosisCodes = DataReaderMapToList<EDIDiagnosisCodesModel>(reader).ToList();
                        reader.NextResult();
                        result.EDIInterchangeHeaders = DataReaderMapToList<EDIInterchangeHeaders>(reader).FirstOrDefault();
                        reader.NextResult();
                        result.EDIClaims = DataReaderMapToList<EDIClaimModel>(reader).ToList();
                        reader.NextResult();
                        result.EDIClaimServiceLines = DataReaderMapToList<EDIClaimsServiceLines>(reader).ToList();
                        reader.NextResult();
                        result.EDIGateway = DataReaderMapToList<EDIGatewayModel>(reader).FirstOrDefault();
                        reader.NextResult();
                        result.EDIServiceAddress = DataReaderMapToList<EDILocationAddressModel>(reader).FirstOrDefault();
                        reader.NextResult();
                        result.EDIBillingAddress = DataReaderMapToList<EDILocationAddressModel>(reader).FirstOrDefault();
                        reader.NextResult();
                        if (commandText != SQLObjects.CLM_GetEDIInfo || commandText != SQLObjects.CLM_GetBatchEDIInfo || commandText != SQLObjects.CLM_GetEDIInfoResubmit ||  commandText!= SQLObjects.CLM_GetBatchEDIInfoResubmit)
                        {
                            result.EDIOtherPayerInformationModel = DataReaderMapToList<EDIOtherPayerInformationModel>(reader).ToList();
                            reader.NextResult();
                            result.EDIPayerPaymentModel = DataReaderMapToList<EDIPayerPaymentModel>(reader).ToList();
                            reader.NextResult();
                        }
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public EDI270FileModel ExecStoredProcedureForEDI270Info(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            EDI270FileModel result = new EDI270FileModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {

                        result.EDI270InterchangeHeaders = DataReaderMapToList<EDI270InterchangeHeaders>(reader).FirstOrDefault();
                        reader.NextResult();
                        result.EDI270EligibilityEnquiryDetails = DataReaderMapToList<EDI270EligibilityEnquiryDetailsModel>(reader).FirstOrDefault();
                        reader.NextResult();
                        result.EDI270EligibilityEnquiryServiceTypesList = DataReaderMapToList<EDI270EligibilityEnquiryServiceTypesModel>(reader).ToList();
                        reader.NextResult();
                        result.EDI270EligibilityEnquiryServiceCodesList = DataReaderMapToList<EDI270EligibilityEnquiryServiceCodesModel>(reader).ToList();
                        reader.NextResult();
                        result.EDIGateway = DataReaderMapToList<EDIGatewayModel>(reader).FirstOrDefault();
                        reader.NextResult();
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public Dictionary<string, object> ExecStoredProcedureForAuthInfo(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Add("Authorization", DataReaderMapToList<AuthorizationModel>(reader).ToList());
                        reader.NextResult();
                        result.Add("AuthorizationProcedure", DataReaderMapToList<AuthorizationProceduresModel>(reader).ToList());
                        reader.NextResult();
                        result.Add("AuthorizationProceduresCPT", DataReaderMapToList<AuthorizationProceduresCPTModel>(reader).ToList());
                        reader.NextResult();
                    }
                    return result;
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public IList<TEntity> ExecuteQuery<TEntity>(string commandText, int totalOutputParams, params object[] parameters) where TEntity : class, new()
        {
            var connection = this.Database.GetDbConnection();
            IList<TEntity> result = new List<TEntity>();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result = DataReaderMapToList<TEntity>(reader);
                        reader.NextResult();
                    }
                }
                return result;
            }
            finally
            {
                connection.Close();
            }
        }
        public Dictionary<string, object> ExecStoredProcedureForProcessedClaims(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Add("ProcessedClaims", DataReaderMapToList<ProcessedClaimModel>(reader).ToList());
                        reader.NextResult();
                        result.Add("ProcessedClaimServiceLines", DataReaderMapToList<ProcessedClaimServiceLineModel>(reader).ToList());
                        reader.NextResult();
                        result.Add("ProcessedClaimServiceLineAdjustments", DataReaderMapToList<ProcessedClaimServiceLineAdjustmentsModel>(reader).ToList());
                        reader.NextResult();
                    }
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
        public Dictionary<string, object> ExecStoredProcedureForPatientPhoneAddress(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Add("PatientAddress", DataReaderMapToList<AddressModel>(reader).ToList());
                        reader.NextResult();
                        result.Add("PhoneNumbers", DataReaderMapToList<PhoneModel>(reader).ToList());
                        reader.NextResult();
                    }
                    return result;
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public Dictionary<string, object> ExecStoredProcedureForPatientInsuranceInsuredPerson(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Add("PatientInsurance", DataReaderMapToList<PatientInsuranceModel>(reader).ToList());
                        reader.NextResult();
                        result.Add("InsuredPerson", DataReaderMapToList<InsuredPersonModel>(reader).ToList());
                        reader.NextResult();
                    }
                    return result;
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public Dictionary<string, object> ExecStoredProcedureForPatientCustomLabels(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Add("MasterCustomLabels", DataReaderMapToList<MasterCustomLabelModel>(reader).ToList());
                        reader.NextResult();
                        result.Add("PatientCustomLabels", DataReaderMapToList<CustomLabelModel>(reader).ToList());
                        reader.NextResult();
                    }
                    return result;
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public Dictionary<string, object> ExecStoredProcedureForStaffCustomLabels(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Add("MasterCustomLabels", DataReaderMapToList<MasterCustomLabelModel>(reader).ToList());
                        reader.NextResult();
                        result.Add("StaffCustomLabels", DataReaderMapToList<StaffCustomLabelModel>(reader).ToList());
                        reader.NextResult();
                    }
                    return result;
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public StaffProfileModel ExecStoredProcedureForStaffProfileData(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            StaffProfileModel result = new StaffProfileModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result= DataReaderMapToList<StaffProfileModel>(reader).FirstOrDefault();
                        reader.NextResult();
                        result.StaffTags= DataReaderMapToList<StaffProfileTagsModel>(reader).ToList();
                        reader.NextResult();
                        result.StaffLocations = DataReaderMapToList<StaffProfileLocationModel>(reader).ToList();
                    }
                    return result;
                }
            }
            finally
            {
                connection.Close();
            }
        }
        public IList<TEntity> ExecuteSqlQuery<TEntity>(HCOrganizationContext context, string commandText) where TEntity : class, new()
        {
            IList<TEntity> result = new List<TEntity>();
            try
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = commandText;
                    context.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        result = DataReaderMapToList<TEntity>(reader);
                        reader.NextResult();
                    }
                }
                return result;
            }
            finally
            {
                //TO DO
            }
        }

        public Dictionary<string, object> ExecStoredProcedureForPayerServiceCodesAndModifers(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.Add("ServiceCodes", DataReaderMapToList<ServiceCodeSearchModel>(reader).ToList());
                        reader.NextResult();
                        result.Add("ServiceCodeModifiers", DataReaderMapToList<ServiceCodeModifiersSearchModel>(reader).ToList());
                        reader.NextResult();                        
                    }
                    return result;
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}





