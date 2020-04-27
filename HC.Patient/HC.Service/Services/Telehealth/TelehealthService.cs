using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Availability;
using HC.Patient.Model.Telehealth;
using HC.Patient.Repositories.IRepositories.Telehealth;
using HC.Patient.Service.IServices.Telehealth;
using HC.Service;
using OpenTok.Server;
using OpenTok.Server.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Telehealth
{
    public class TelehealthService : BaseService, ITelehealthService
    {
        private readonly ITelehealthRepository _telehealthRepository;
        private readonly HCOrganizationContext _context;
        public TelehealthService(ITelehealthRepository telehealthRepository, HCOrganizationContext context)
        {
            _telehealthRepository = telehealthRepository;
            _context = context;
        }

        public JsonModel GetTelehealthSession(int patientID, int staffID, DateTime startTime, DateTime endTime, TokenModel tokenModel)
        {
            OpenTokModel openTokModel = new OpenTokModel();
            OpenTok.Server.OpenTok opentok = new OpenTok.Server.OpenTok(OpenTokAPIDetails.APIKey, OpenTokAPIDetails.APISecret, OpenTokAPIDetails.APIUrl);
            opentok.Client = new HttpClient(OpenTokAPIDetails.APIKey, OpenTokAPIDetails.APISecret, OpenTokAPIDetails.APIUrl);
            var UserName = _telehealthRepository.GetUserNameByUserID(tokenModel);
            openTokModel.Name = UserName;

            TelehealthSessionDetails telehealthSessionDetails = _telehealthRepository.GetTelehealthSession(patientID, staffID, startTime, endTime);
            if (telehealthSessionDetails != null)
            {
                TelehealthTokenDetails telehealthTokenDetails = _telehealthRepository.GetTelehealthToken(telehealthSessionDetails.Id, tokenModel);
                if (telehealthTokenDetails != null)
                {
                    openTokModel = new OpenTokModel()
                    {
                        ApiKey = OpenTokAPIDetails.APIKey,
                        SessionID = telehealthSessionDetails.SessionID,
                        Token = telehealthTokenDetails.Token
                    };
                    return new JsonModel()
                    {
                        data = openTokModel,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK//Success
                    };
                }
                else
                {
                    DateTime d = DateTime.Now.AddDays(30);
                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    var duration = (d.ToUniversalTime() - epoch).TotalSeconds;

                    string token = string.Empty;
                    UserRoles userroles = _context.UserRoles.Where(j => j.Id == tokenModel.RoleID).FirstOrDefault();
                    if (userroles.UserType.ToLower() == UserTypeEnum.CLIENT.ToString().ToLower())
                        token = opentok.GenerateToken(telehealthSessionDetails.SessionID, Role.PUBLISHER, duration);
                    else
                        token = opentok.GenerateToken(telehealthSessionDetails.SessionID, Role.PUBLISHER, duration);

                    telehealthTokenDetails = _telehealthRepository.CreateTelehealthToken(telehealthSessionDetails.Id, token, duration, tokenModel);
                    if (telehealthTokenDetails.result > 0)
                    {
                        openTokModel = new OpenTokModel()
                        {
                            ApiKey = OpenTokAPIDetails.APIKey,
                            SessionID = telehealthSessionDetails.SessionID,
                            Token = telehealthTokenDetails.Token
                        };
                        return new JsonModel()
                        {
                            data = openTokModel,
                            Message = StatusMessage.FetchMessage,
                            StatusCode = (int)HttpStatusCodes.OK//Success
                        };
                    }
                    else
                    {
                        if (telehealthTokenDetails.exception != null)
                        {
                            return new JsonModel()
                            {
                                data = new object(),
                                Message = telehealthTokenDetails.exception.Message,
                                StatusCode = (int)HttpStatusCodes.InternalServerError
                            };
                        }
                        else
                        {
                            return new JsonModel()
                            {
                                data = new object(),
                                Message = StatusMessage.ErrorOccured,
                                StatusCode = (int)HttpStatusCodes.InternalServerError
                            };
                        }
                   }
               }
            }
            else
            {
                var session = opentok.CreateSession();
                telehealthSessionDetails = _telehealthRepository.CreateTelehealthSession(session.Result.Id, patientID, staffID, startTime, endTime, tokenModel);
                if (telehealthSessionDetails.result > 0)
                {
                    DateTime d = DateTime.Now.AddDays(30);
                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    var duration = (d.ToUniversalTime() - epoch).TotalSeconds;
                    string token = string.Empty;
                    UserRoles userroles = _context.UserRoles.Where(j => j.Id == tokenModel.RoleID).FirstOrDefault();
                    if (userroles.UserType.ToLower() == UserTypeEnum.CLIENT.ToString().ToLower())
                        token = opentok.GenerateToken(session.Result.Id, Role.PUBLISHER, duration);
                    else
                        token = opentok.GenerateToken(session.Result.Id, Role.PUBLISHER, duration);

                    TelehealthTokenDetails telehealthTokenDetails = _telehealthRepository.CreateTelehealthToken(telehealthSessionDetails.Id, token, duration, tokenModel);
                    if (telehealthTokenDetails.result > 0)
                    {
                        openTokModel = new OpenTokModel()
                        {
                            ApiKey = OpenTokAPIDetails.APIKey,
                            SessionID = telehealthSessionDetails.SessionID,
                            Token = token
                        };
                        return new JsonModel()
                        {
                            data = openTokModel,
                            Message = StatusMessage.FetchMessage,
                            StatusCode = (int)HttpStatusCodes.OK//Success
                        };
                    }
                    else
                    {
                        if (telehealthTokenDetails.exception != null)
                        {
                            return new JsonModel()
                            {
                                data = new object(),
                                Message = telehealthTokenDetails.exception.Message,
                                StatusCode = (int)HttpStatusCodes.InternalServerError
                            };

                        }
                        else
                        {
                            return new JsonModel()
                            {
                                data = new object(),
                                Message = StatusMessage.ErrorOccured,
                                StatusCode = (int)HttpStatusCodes.InternalServerError
                            };

                        }
                    }
                }
                else
                {
                    if (telehealthSessionDetails.exception != null)
                    {
                        return new JsonModel()
                        {
                            data = new object(),
                            Message = telehealthSessionDetails.exception.Message,
                            StatusCode = (int)HttpStatusCodes.InternalServerError
                        };
                    }
                    else
                    {
                        return new JsonModel()
                        {
                            data = new object(),
                            Message = StatusMessage.ErrorOccured,
                            StatusCode = (int)HttpStatusCodes.InternalServerError
                        };

                    }
                }
            }
        }
    }
}
