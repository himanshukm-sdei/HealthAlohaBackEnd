using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Telehealth;
using HC.Repositories;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Telehealth
{
    public class TelehealthRepository : ITelehealthRepository
    {
        private HCOrganizationContext _context;
        public TelehealthRepository(HCOrganizationContext context) 
        {
            this._context = context;
        }

        public TelehealthSessionDetails GetTelehealthSession(int patientID, int staffID, DateTime startTime, DateTime endTime)
        {
            try
            {
                TelehealthSessionDetails telehealthSessionDetails = _context.TelehealthSessionDetails
                .Where(k => k.IsActive == true && k.IsDeleted == false && k.PatientID == patientID &&
                k.StaffId == staffID && k.StartTime == startTime && k.EndTime == endTime).FirstOrDefault();
                return telehealthSessionDetails;
            }
            catch (Exception ex)
            {
                TelehealthSessionDetails telehealthSessionDetails = new TelehealthSessionDetails();
                telehealthSessionDetails.exception = ex;
                return telehealthSessionDetails;
            }
        }

        public TelehealthTokenDetails GetTelehealthToken(int id, TokenModel tokenModel)
        {
            try
            {
                UserRoles userroles = _context.UserRoles.Where(j => j.Id == tokenModel.RoleID).FirstOrDefault();
                if (userroles.UserType.ToLower() == UserTypeEnum.CLIENT.ToString().ToLower())
                {
                    TelehealthTokenDetails telehealthTokenDetails = _context.TelehealthTokenDetails
                        .Where(k => k.IsActive == true && k.IsDeleted == false && k.TelehealthSessionDetailID == id && k.IsStaffToken==false).FirstOrDefault();
                    return telehealthTokenDetails;
                }
                else
                {
                    TelehealthTokenDetails telehealthTokenDetails = _context.TelehealthTokenDetails
                        .Where(k => k.IsActive == true && k.IsDeleted == false && k.TelehealthSessionDetailID == id && k.IsStaffToken == true).FirstOrDefault();
                    return telehealthTokenDetails;
                }
            }
            catch (Exception ex)
            {
                TelehealthTokenDetails telehealthTokenDetails = new TelehealthTokenDetails();
                telehealthTokenDetails.exception = ex;
                return telehealthTokenDetails;
            }
        }

        public TelehealthTokenDetails CreateTelehealthToken(int telehealthSessionDetailID, string token, double duration, TokenModel tokenModel)
        {
            using (var transaction = _context.Database.BeginTransaction()) //TO DO do this with SP
            {
                try
                {
                    bool IsStaffToken = true;
                    UserRoles userroles = _context.UserRoles.Where(j => j.Id == tokenModel.RoleID).FirstOrDefault();
                    if (userroles.UserType.ToLower() == UserTypeEnum.CLIENT.ToString().ToLower())
                    {
                        IsStaffToken = false;
                    }
                    var telehealthTokenDetails = new TelehealthTokenDetails()
                    {
                        CreatedBy = tokenModel.UserID,
                        CreatedDate = DateTime.UtcNow,
                        TelehealthSessionDetailID = telehealthSessionDetailID,
                        IsActive = true,
                        IsDeleted = false,
                        Token = token,
                        TokenExpiry = duration,
                        IsStaffToken = IsStaffToken,
                    };
                    _context.TelehealthTokenDetails.Add(telehealthTokenDetails);
                    int result = _context.SaveChanges();
                    transaction.Commit();
                    telehealthTokenDetails.result = result;
                    return telehealthTokenDetails;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TelehealthTokenDetails telehealthTokenDetails = new TelehealthTokenDetails();
                    telehealthTokenDetails.exception = ex;
                    return telehealthTokenDetails;
                }
            }
        }

        public TelehealthSessionDetails CreateTelehealthSession(string sessionID, int patientID, int staffID, DateTime startTime, DateTime endTime, TokenModel tokenModel)
        {
            using (var transaction = _context.Database.BeginTransaction()) //TO DO do this with SP
            {
                try
                {
                    TelehealthSessionDetails telehealthSessionDetails = new TelehealthSessionDetails()
                    {
                        CreatedBy = tokenModel.UserID,
                        CreatedDate = DateTime.UtcNow,
                        PatientID = patientID,
                        StaffId = staffID,
                        IsActive = true,
                        IsDeleted = false,
                        EndTime = endTime,
                        StartTime = startTime,
                        SessionID = sessionID
                    };
                    _context.TelehealthSessionDetails.Add(telehealthSessionDetails);
                    int result = _context.SaveChanges();
                    transaction.Commit();
                    telehealthSessionDetails.result = result;
                    return telehealthSessionDetails;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TelehealthSessionDetails telehealthSessionDetails = new TelehealthSessionDetails();
                    telehealthSessionDetails.exception = ex;
                    return telehealthSessionDetails;
                }
            }
        }

        public string GetUserNameByUserID(TokenModel tokenModel)
        {
            try
            {
                UserRoles userroles = _context.UserRoles.Where(j => j.Id == tokenModel.RoleID).FirstOrDefault();
                string UserName = string.Empty;
                if (userroles.UserType.ToLower() == UserTypeEnum.CLIENT.ToString().ToLower())
                {
                    var patient = _context.Patients.Where(l => l.UserID == tokenModel.UserID).FirstOrDefault();
                    if(patient!=null)
                    {
                        UserName = patient.FirstName + " " + patient.LastName;
                    }
                }
                else
                {
                    var staff = _context.Staffs.Where(l => l.UserID == tokenModel.UserID).FirstOrDefault();
                    if (staff != null)
                    {
                        UserName = staff.FirstName + " " + staff.LastName;
                    }
                }
                return UserName;
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}
