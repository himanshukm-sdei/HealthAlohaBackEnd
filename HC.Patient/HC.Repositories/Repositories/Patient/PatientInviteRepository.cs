using HC.Model;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Model.SecurityQuestion;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientInviteRepository: RepositoryBase<PatientInvite>, IPatientInviteRepository
    {

        private HCOrganizationContext _context;
        JsonModel response = new JsonModel();
        public PatientInviteRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public bool SaveOtp(int userId, string otp)
        {
            bool isUpdated = false;
            PatientInvite user = _context.Set<PatientInvite>().Where(x => x.Id == userId).FirstOrDefault();
            if (user != null)
            {
                user.Otp = otp;
                _context.SaveChanges();
                isUpdated = true;
            }
            return isUpdated;
        }
        public JsonModel VerifyClientContactNumber(int patientId, string contactNumber , TokenModel token)
        {
            try
            {
                var contacts = _context.PatientInvite.Where(x => x.Id == patientId && x.Phone == contactNumber && x.IsDeleted == false && x.IsActive == true).FirstOrDefault();
                if (contacts != null)
                {

                    return new JsonModel()
                    {
                        data = contacts,
                        Message = StatusMessage.SuccessFul,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                else
                {
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }

            }
            catch (Exception)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ErrorOccured,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public PatientInvite VerifyOTP(PatientInvite otpEntity)
        {
            return _context.Set<PatientInvite>().Where(x => x.Id== otpEntity.Id && x.Otp == otpEntity.Otp).FirstOrDefault();
        }
    }
}
