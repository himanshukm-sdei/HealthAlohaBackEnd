using HC.Model;
using HC.Patient.Model.AuditLog;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.AuditLog
{
    public interface IAuditLogService :IBaseService
    {
        void AccessLogs(string screenName,string action,Nullable<int> patientId,Nullable<int> userId, TokenModel tokenModel,string LoginAttempt);
        JsonModel GetAuditLogList(string createdBy, string patientName, string action, string fromDate, string toDate, int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token);
        JsonModel GetLoginLogList(string createdBy, string patientName, string action, string fromDate, string toDate, int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token, int roleID);
    }
}
