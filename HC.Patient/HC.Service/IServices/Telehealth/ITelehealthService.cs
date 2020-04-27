using System;
using System.Collections.Generic;
using System.Text;
using HC.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Telehealth
{
    public interface ITelehealthService : IBaseService
    {
        JsonModel GetTelehealthSession(int patientID, int staffID, DateTime startTime, DateTime endTime, TokenModel tokenModel);
    }
}
