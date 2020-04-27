using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientImmunizationRepository : RepositoryBase<PatientImmunization>, IPatientImmunizationRepository
    {
        private HCOrganizationContext _context;
        public PatientImmunizationRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public IQueryable<T> GetImmunization<T>(int patientId, TokenModel tokenModel) where T : class, new()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@PatientId",patientId)};
                return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetImmunization.ToString(), parameters.Length, parameters).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
