using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.SecurityQuestion;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientInviteRepository : IRepositoryBase<PatientInvite>
    {
        JsonModel VerifyClientContactNumber(int inviteId, string contactNumber , TokenModel token);

        bool SaveOtp(int userId, string otp);

        PatientInvite VerifyOTP(PatientInvite otpModel);
    }
}
