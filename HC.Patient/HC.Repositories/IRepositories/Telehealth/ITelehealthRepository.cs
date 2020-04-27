using System;
using System.Collections.Generic;
using System.Text;
using HC.Model;
using HC.Patient.Entity;

namespace HC.Patient.Repositories.IRepositories.Telehealth
{
    public interface ITelehealthRepository
    {
        TelehealthSessionDetails GetTelehealthSession(int patientID, int staffID, DateTime startTime, DateTime endTime);
        TelehealthTokenDetails GetTelehealthToken(int id,TokenModel tokenModel);
        TelehealthTokenDetails CreateTelehealthToken(int telehealthSessionDetailID, string token, double duration, TokenModel tokenModel);
        TelehealthSessionDetails CreateTelehealthSession(string sessionID, int patientID, int staffID, DateTime startTime, DateTime endTime, TokenModel tokenModel);
        string GetUserNameByUserID(TokenModel tokenModel);
    }
}
