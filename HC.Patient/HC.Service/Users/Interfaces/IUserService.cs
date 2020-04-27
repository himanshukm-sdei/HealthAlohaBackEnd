using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Model.Payer;
using HC.Patient.Model.SecurityQuestion;
using HC.Patient.Model.Users;
using HC.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HC.Patient.Service.Users.Interfaces
{
    public interface IUserService : IBaseService
    {
        User GetUserByUserName(string userName);
        AuthenticationToken AuthenticationToken(AuthenticationToken authenticationToken);
        int SetPasswordForUser(UserPassword userPassword);
        bool CheckIfRecordExists(RecordExistFilter recordExistFilter);
        List<PayerInfoDropDownModel> GetPayerByPatientID(int patientID, string Key);
        JsonModel UpdateAccessFailedCount(int userID, TokenModel tokenModel);
        void ResetUserAccess(int userID, TokenModel tokenModel);
        JsonModel UpdateStaffActiveStatus(int userID, bool isActive);
        bool SaveMachineLoginUser(MachineLoginLog machineLoginLog);
        bool UserAlreadyLoginFromSameMachine(MachineLoginLog machineLoginLog);
        bool CheckIfRecordExistsMasterDB(RecordExistFilter recordExistFilter);
        //JsonModel VerifyClientContactNumber(int inviteId,string contactNumber , TokenModel tokenModel);
        JsonModel VerifyOTP(OtpModel otp);
        
    }
}
