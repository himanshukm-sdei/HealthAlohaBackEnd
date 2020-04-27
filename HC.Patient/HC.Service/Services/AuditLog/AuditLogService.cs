using HC.Common;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Service.IServices.AuditLog;
using System;
using System.Collections.Generic;
using System.Text;
using HC.Model;
using HC.Patient.Model.AuditLog;
using System.Linq;
using static HC.Common.Enums.CommonEnum;
using HC.Common.HC.Common;
using HC.Service;

namespace HC.Patient.Service.Services.AuditLog
{
    public class AuditLogService : BaseService, IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }
        public void AccessLogs(string screenName, string action, int? patientId, int? userId, TokenModel tokenModel, string LoginAttempt)
        {
            AuditLogs auditLog = new AuditLogs();
            auditLog.ScreenName = screenName;
            auditLog.Action = action;
            auditLog.CreatedById = userId;
            auditLog.CreatedDate = DateTime.UtcNow;
            auditLog.EncryptionCode = CommonMethods.GetHashValue(
                          screenName.Trim() +
                          action.Trim() +
                          (auditLog.OldValue != null ? auditLog.OldValue : string.Empty) +
                          (auditLog.NewValue != null ? auditLog.NewValue : string.Empty) +
                          (auditLog.AuditLogColumnId != null ? Convert.ToString(auditLog.AuditLogColumnId) : string.Empty) +
                          (auditLog.CreatedById != null ? Convert.ToString(auditLog.CreatedById) : string.Empty) +
                          Convert.ToString(auditLog.CreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt")) +
                          (patientId != null ? Convert.ToString(patientId) : string.Empty) +
                          (tokenModel.IPAddress != null ? tokenModel.IPAddress.Trim() : string.Empty) +
                          Convert.ToString(tokenModel.OrganizationID) +
                          Convert.ToString(tokenModel.LocationID)
                          );
            auditLog.PatientId = patientId;
            auditLog.IPAddress = !string.IsNullOrEmpty(tokenModel.IPAddress) ? tokenModel.IPAddress : null;
            auditLog.LocationID = tokenModel.LocationID > 0 ? tokenModel.LocationID : 1;
            auditLog.OrganizationID = tokenModel.OrganizationID;
            auditLog.LoginAttempt = LoginAttempt;
            _auditLogRepository.Create(auditLog);
            _auditLogRepository.SaveChanges();
        }

        public JsonModel GetAuditLogList(string createdBy, string patientName, string action, string fromDate, string toDate, int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token)
        {
            List<AuditLogModel> auditLogList = _auditLogRepository.GetAuditLogList<AuditLogModel>(createdBy, patientName, action, fromDate, toDate, token.OrganizationID, token.LocationID, pageNumber, pageSize, sortColumn, sortOrder).ToList();
            if (auditLogList != null && auditLogList.Count > 0)
            {
                auditLogList.ForEach(x => { x.LogDate = CommonMethods.ConvertFromUtcTime(x.LogDate, token); });
                return new JsonModel()
                {
                    data = auditLogList,
                    meta = new Meta()
                    {
                        TotalRecords = auditLogList[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(auditLogList[0].TotalRecords / pageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = null,
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }

        public JsonModel GetLoginLogList(string createdBy, string patientName, string action, string fromDate, string toDate, int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token, int roleID)
        {
            List<AuditLogModel> auditLogList = _auditLogRepository.GetLoginLogList<AuditLogModel>(createdBy, patientName, action, fromDate, toDate, token.OrganizationID, token.LocationID, pageNumber, pageSize, sortColumn, sortOrder, roleID).ToList();
            if (auditLogList != null && auditLogList.Count > 0)
            {
                auditLogList.ForEach(x => { x.LogDate = CommonMethods.ConvertFromUtcTime(x.LogDate, token); });

                return new JsonModel()
                {
                    data = auditLogList,
                    meta = new Meta()
                    {
                        TotalRecords = auditLogList[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(auditLogList[0].TotalRecords / pageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = null,
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }
    }
}
