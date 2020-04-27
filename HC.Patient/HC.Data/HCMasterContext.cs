using HC.Patient.Entity;
using HC.Patient.Model.Common;
using HC.Patient.Model.Questionnaire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace HC.Patient.Data
{
    public class HCMasterContext : DbContext
    {
        /// <summary>
        /// Db context for Patient module
        /// </summary>
        /// <param name="options"></param>
        public HCMasterContext(DbContextOptions<HCMasterContext> options) : base(options) { }
        public DbSet<MasterOrganization> MasterOrganization { get; set; }
        public DbSet<OrganizationDatabaseDetail> OrganizationDatabaseDetail { get; set; }
        public DbSet<MasterAppConfiguration> MasterAppConfiguration { get; set; }
        public DbSet<MasterSecurityQuestions> MasterSecurityQuestions { get; set; }
        public DbSet<SuperUser> SuperUser { get; set; }
        public DbSet<MasterSurveyCategory> MasterSurveyCategory { get; set; }
        public DbSet<MasterDFA_Category> MasterDFA_Category { get; set; }      
        public DbSet<MasterDFA_CategoryCode> MasterDFA_CategoryCode { get; set; }        
        public DbSet<MasterDFA_Document> MasterDFA_Document { get; set; }
        public DbSet<MasterDiseaseManagementProgram> MasterDiseaseManagementProgram { get; set; }
        public DbSet<MasterAssessmentType> MasterAssessmentType { get; set; }
        public DbSet<MasterQuestionnaireBenchmarkRange> MasterQuestionnaireBenchmarkRange { get; set; }
        public DbSet<MasterBenchmark> MasterBenchmark { get; set; }
        public DbSet<MasterDFA_Section> MasterDFA_Section { get; set; }        
        public DbSet<MasterCheckListCategory> MasterCheckListCategory { get; set; }
       public DbSet<MasterCheckList> MasterCheckList { get; set; }
        public DbSet<MasterDFA_SectionItem> MasterDFA_SectionItem { get; set; }
        public DbSet<MasterGlobalCodeCategory> MasterGlobalCodeCategory { get; set; }
        public DbSet<MasterGlobalCode> MasterGlobalCode { get; set; }
        public DbSet<MasterHRACategoryRisk> MasterHRACategoryRisk { get; set; }
        public DbSet<MasterMappingHRACategoryRisk> MasterMappingHRACategoryRisk { get; set; }
        public DbSet<MasterHRACategoryRiskBenchmarkMapping> MasterHRACategoryRiskBenchmarkMapping { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Organization
            modelBuilder.Entity<MasterOrganization>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterOrganization>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterOrganization>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            modelBuilder.Entity<SuperUser>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);


            modelBuilder.Entity<SuperUser>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<SuperUser>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //MasterDFA_SectionItem
            //modelBuilder.Entity<MasterDFA_SectionItems>()
            //.Property(b => b.CreatedDate)
            //.HasDefaultValueSql("GetUtcDate()");

            //modelBuilder.Entity<MasterDFA_SectionItems>()
            //.Property(b => b.IsDeleted)
            //.HasDefaultValue(false);

            //modelBuilder.Entity<MasterDFA_SectionItems>()
            //.Property(b => b.IsActive)
            //.HasDefaultValue(true);

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

            //MasterAppConfirguration
            modelBuilder.Entity<MasterAppConfiguration>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterAppConfiguration>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterAppConfiguration>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //MasterSecurityQuestion
            modelBuilder.Entity<MasterSecurityQuestions>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<MasterSecurityQuestions>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<MasterSecurityQuestions>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

            //OrganizationDatabaseDetail
            modelBuilder.Entity<OrganizationDatabaseDetail>()
            .Property(b => b.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder.Entity<OrganizationDatabaseDetail>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("GetUtcDate()");

            modelBuilder.Entity<OrganizationDatabaseDetail>()
            .Property(b => b.IsActive)
            .HasDefaultValue(true);

          

        }


        public IList<TEntity> ExecStoredProcedureListWithOutput<TEntity>(string commandText, int totalOutputParams, params object[] parameters) where TEntity : class, new()
        {
            var connection = this.Database.GetDbConnection();

            //  var context = ((Microsoft.AspNetCore. AspNet.DynamicData.ModelProviders.EFDataModelProvide)(this)).ObjectContext;
            IList<TEntity> result = new List<TEntity>();
            try
            {
                totalOutputParams = totalOutputParams == 0 ? 1 : totalOutputParams;
                //int o = 0;

                //Don't close the connection after command execution

                //open the connection for use
                if (connection.State == ConnectionState.Closed) { connection.Open(); }

                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1000;
                    if (parameters != null)
                    {
                        // move parameters to command object
                        foreach (var p in parameters)
                        {
                            if (p != null)
                            {
                                cmd.Parameters.Add(p);
                            }
                        }
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        result = DataReaderMapToList<TEntity>(reader);
                        reader.NextResult();
                    }
                }
                return result;
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
        public MasterSectionItemlistingModel ExecStoredProcedureListWithOutputForSectionItems(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            MasterSectionItemlistingModel result = new MasterSectionItemlistingModel();
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                using (var cmd = connection.CreateCommand())
                {
                    //////Please make sure reader should not open for this much time as in the below code to get n number of datasets.please avoid
                    AddParametersToDbCommand(commandText, parameters, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result.SectionItems = DataReaderMapToList<MasterSectionItemModel>(reader).ToList();
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

        public MasterSectionItemDDValueModel ExecStoredProcedureListWithOutputForSectionItemDDValues(string commandText, int totalOutputParams, params object[] parameters)
        {
            var connection = this.Database.GetDbConnection();
            MasterSectionItemDDValueModel result = new MasterSectionItemDDValueModel();
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

    }
}
