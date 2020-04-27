using HC.Patient.Data;
using HC.Patient.Data.ViewModel;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientEncounters;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using HC.Patient.Model.PatientEncounters;
using HC.Model;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.PatientEncounters
{
    public class PatientEncounterRepository:RepositoryBase<PatientEncounter>, IPatientEncounterRepository
    {
        private HCOrganizationContext _context;
        public PatientEncounterRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }

        public PatientEncounterModel GetPatientEncounterDetails(int encounterId, bool isBillable)
        {
            SqlParameter[] parameters = { new SqlParameter ("@EncounterId", encounterId ) };
            return _context.ExecStoredProcedureForPatientEncounterDetail("ENC_GetPatientEncounterDetails",parameters.Length, isBillable,parameters);
        }

        public IQueryable<T> GetServiceCodeForEncounterByAppointmentType<T>(int appointmentId) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@AppointmentId", appointmentId) };
            return _context.ExecStoredProcedureListWithOutput<T>("ENC_GetServiceCodeForEncounterByAppointmentType", parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetAllEncounters<T>(int? patientID, string appointmentType ="", string staffName = "", string status = "", string fromDate = "", string toDate = "", int pageNumber=1, int pageSize=10, string sortColumn="", string sortOrder="") where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", (int?)patientID),
                                          new SqlParameter("@AppointmentType", appointmentType),
                                          new SqlParameter("@StaffName", staffName),
                                          new SqlParameter("@Status", status),
                                          new SqlParameter("@FromDate", fromDate),
                                          new SqlParameter("@ToDate", toDate),
                                          new SqlParameter("@PageNumber", pageNumber),
                                          new SqlParameter("@PageSize", pageSize),
                                          new SqlParameter("@SortColumn", sortColumn),
                                          new SqlParameter("@SortOrder", sortOrder)};
            return _context.ExecStoredProcedureListWithOutput<T>("ENC_GetAllEncounters", parameters.Length, parameters).AsQueryable();
        }

        public PatientEncounterModel DownloadEncounter(int encounterId, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@EncounterId", encounterId) };
            return _context.ExecStoredProcedureForPatientEncounterDetail(SQLObjects.RPT_DownloadEncounter.ToString() , parameters.Length,true, parameters);
        }
    }
}