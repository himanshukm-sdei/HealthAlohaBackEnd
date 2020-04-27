using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Common;
using HC.Patient.Model.Staff;
using HC.Patient.Model.Users;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Repositories.IRepositories.SecurityQuestion;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices.User;
using HC.Service;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.User
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private JsonModel response;
        private readonly IUserSecurityQuestionAnswerRepository _userSecurityQuestionAnswerRepository;        
        private readonly HCOrganizationContext _context;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUserPasswordHistoryService _userPasswordHistoryService;
        private readonly IUserPasswordHistoryRepository _userPasswordHistoryRepository;

        public UserService(IUserRepository userRepository, IUserSecurityQuestionAnswerRepository userSecurityQuestionAnswerRepository, IAuditLogRepository auditLogRepository, IUserPasswordHistoryService userPasswordHistoryService, IUserPasswordHistoryRepository userPasswordHistoryRepository, HCOrganizationContext context)
        {
            _userRepository = userRepository;
            _userSecurityQuestionAnswerRepository = userSecurityQuestionAnswerRepository;
            _auditLogRepository = auditLogRepository;
            _context = context;
            _userPasswordHistoryService = userPasswordHistoryService;
            _userPasswordHistoryRepository = userPasswordHistoryRepository;
        }
        public List<StaffModels> GetFilteredStaff(string LocationIds, string RoleIds, string SearchKey, string StartWith, string Tags, string sortColumn, string sortOrder, int pageNumber, int pageSize, TokenModel tokenModel)
        {
            return _userRepository.GetFilteredStaff<StaffModels>(LocationIds, RoleIds, SearchKey, StartWith, Tags, sortColumn, sortOrder, pageNumber, pageSize, tokenModel).ToList();
        }

        public JsonModel UpdateUserPassword(UpdatePasswordModel updatePassword, TokenModel token)
        {
            try
            {
                Entity.User user = _userRepository.Get(a => a.Id == updatePassword.UserId && a.Password == CommonMethods.Encrypt(updatePassword.CurrentPassword));
                if (user != null)
                {
                    if (updatePassword.NewPassword == updatePassword.ConfirmNewPassword)
                    {
                        List<UserPasswordHistoryModel> passwordHistory = _userPasswordHistoryService.GetUserPasswordHistory(updatePassword.UserId);
                        if (passwordHistory != null && passwordHistory.Count > 0 && passwordHistory.FindAll(x => x.Password == updatePassword.NewPassword).Count() > 0)
                        {
                            return new JsonModel(null, UserAccountNotification.PasswordMatch, (int)HttpStatusCodes.UnprocessedEntity, string.Empty);
                        }
                        using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                user.Password = CommonMethods.Encrypt(updatePassword.NewPassword);
                                user.PasswordResetDate = DateTime.UtcNow;
                                _userRepository.Update(user);
                                _userPasswordHistoryService.SaveUserPasswordHistory(updatePassword.UserId, DateTime.UtcNow, user.Password);
                                _userRepository.SaveChanges();
                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                return new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, ex.Message);
                            }
                        }

                        return response = new JsonModel()  //Sucess//
                        {
                            data = user,
                            Message = UserAccountNotification.YourPasswordChanged,
                            StatusCode = (int)HttpStatusCodes.OK
                        };
                    }
                    else // if new current password and confirm password doesn't match
                    {
                        return response = new JsonModel()
                        {
                            data = new object(),
                            Message = UserAccountNotification.YourPasswordNotMatchWithConfirmPassword,
                            StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                        };
                    }

                }
                else // if your current password doesn't match
                {
                    return response = new JsonModel()
                    {
                        data = new object(),
                        Message = UserAccountNotification.YourCurrentPassword,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    };
                }
            }
            catch (Exception ex) // On Error
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ErrorOccured,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }

        public JsonModel UpdateUserStatus(int userId, bool status, TokenModel token)
        {
            try
            {
                Entity.User user = _userRepository.GetByID(userId);
                //
                if (status)//block
                {
                    user.IsBlock = status;
                    user.BlockDateTime = DateTime.UtcNow;
                    user.AccessFailedCount = 3;
                    //save
                    _userRepository.UpdateUser(user);
                    _userRepository.SaveChanges();

                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.UserBlocked.Replace("[RoleName]", user.UserName),
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                else
                {//unblock
                    user.IsBlock = status;
                    user.BlockDateTime = null;
                    user.AccessFailedCount = 0;
                    //save
                    _userRepository.UpdateUser(user);
                    _userRepository.SaveChanges();

                    //on unblock time 
                    List<UserSecurityQuestionAnswer> userQuestionAnswer = _userSecurityQuestionAnswerRepository.GetAll(a => a.IsActive == true && a.IsDeleted == false && a.UserID == userId).ToList();
                    userQuestionAnswer.ForEach(a => { a.IsDeleted = true; a.DeletedDate = DateTime.UtcNow; a.DeletedBy = token.UserID; });
                    _userSecurityQuestionAnswerRepository.Update(userQuestionAnswer.ToArray());
                    _userSecurityQuestionAnswerRepository.SaveChanges();

                    return new JsonModel
                    {
                        Message = StatusMessage.UserUnblocked.Replace("[RoleName]", user.UserName),
                        data = new object(),
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
            }
            catch (Exception ex)
            {

                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }


        public JsonModel UploadUserDocuments(UserDocumentsModel userDocuments, TokenModel token)
        {
            try
            {
                List<UserDocuments> userDocList = new List<UserDocuments>();
                string organizationName = _context.Organization.Where(a => a.Id == token.OrganizationID).FirstOrDefault().OrganizationName;

                #region saveDoc
                foreach (var item in userDocuments.Base64)
                {
                    UserDocuments userDoc = new UserDocuments();

                    item.Value.Replace("\"", "");
                    string[] extensionArr = { "jpg", "jpeg", "png", "txt", "docx", "doc", "xlsx", "pdf", "pptx" };
                    //getting data from base64 url                    
                    string base64Data = item.Value.Replace("\"", "").Split(':')[0].ToString().Trim();
                    //getting extension of the image
                    string extension = item.Value.Replace("\"", "").Split(':')[1].ToString().Trim();

                    //out from the loop if document extenstion not exist in list of extensionArr
                    if (!extensionArr.Contains(extension)) { goto Finish; }

                    //create directory
                    //string webRootPath = Directory.GetCurrentDirectory()+ "\\PatientDocuments";
                    string webRootPath = Directory.GetCurrentDirectory();

                    //save folder
                    string DirectoryUrl = userDocuments.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper() ? ImagesPath.UploadClientDocuments : ImagesPath.UploadStaffDocuments;

                    if (!Directory.Exists(webRootPath + DirectoryUrl))
                    {
                        Directory.CreateDirectory(webRootPath + DirectoryUrl);
                    }

                    string fileName = organizationName + "_" + DateTime.UtcNow.TimeOfDay.ToString();

                    //update file name remove unsupported attr.
                    fileName = fileName.Replace(" ", "_").Replace(":", "_");

                    //create path for save location
                    string path = webRootPath + DirectoryUrl + fileName + "." + extension;

                    //convert files into base
                    Byte[] bytes = Convert.FromBase64String(base64Data);
                    //save int the directory
                    File.WriteAllBytes(path, bytes);



                    //create db path
                    //string uploadPath = @"/Documents/ClientDocuments/" + fileName + "." + extension;

                    userDoc.CreatedBy = token.UserID;
                    userDoc.CreatedDate = DateTime.UtcNow;
                    userDoc.IsActive = true;
                    userDoc.IsDeleted = false;
                    userDoc.UserId = userDocuments.UserId;
                    userDoc.DocumentName = userDocuments.DocumentTitle;
                    userDoc.Expiration = userDocuments.Expiration;
                    userDoc.OtherDocumentType = userDocuments.OtherDocumentType;

                    if (userDocuments.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper())
                    {
                        userDoc.DocumentTypeId = userDocuments.DocumentTypeId;
                    }
                    else
                    {
                        userDoc.DocumentTypeIdStaff = userDocuments.DocumentTypeIdStaff;
                    }
                    userDoc.CreatedDate = DateTime.UtcNow;
                    userDoc.UploadPath = fileName + "." + extension;
                    userDoc.Key = userDocuments.Key;
                    userDocList.Add(userDoc);

                }
                //save into db
                _context.UserDocuments.AddRange(userDocList);
                _context.SaveChanges();

                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.DocumentUploaded,
                    StatusCode = (int)HttpStatusCodes.OK
                };
                #endregion

                //return with invaild format message
                Finish:;
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.InvaildFormat,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = e.Message,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel GetUserDocuments(int userId, DateTime? from, DateTime? to, TokenModel token)
        {
            try
            {
                List<UserDocuments> userDocs = _context.UserDocuments.Where(a => a.UserId == userId && a.IsActive == true && a.IsDeleted == false && (a.CreatedDate.Value.Date >= from && a.CreatedDate.Value.Date <= to)).OrderByDescending(a => a.CreatedDate).ToList();
                //string siteURL = _context.AppConfigurations.Where(a => a.Key == "SITE_URL" && a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).FirstOrDefault().Value;
                if (userDocs != null && userDocs.Count() > 0)
                {
                    List<UserDocumentsModel> userDocListModel = new List<UserDocumentsModel>();
                    foreach (var item in userDocs)
                    {
                        UserDocumentsModel userDocModel = new UserDocumentsModel();

                        userDocModel.Id = item.Id;
                        userDocModel.UserId = userId;
                        //userDocModel.Url = CommonMethods.CreateImageUrl(token.Request, ImagesPath.UploadDocuments, item.UploadPath);
                        userDocModel.DocumentTitle = item.DocumentName;

                        userDocModel.Url = item.UploadPath;

                        if (item.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper())
                        {
                            userDocModel.DocumentTypeId = item.DocumentTypeId;
                            userDocModel.DocumentTypeName = _context.MasterDocumentTypes.Where(a => a.Id == item.DocumentTypeId).FirstOrDefault().Type;
                            userDocModel.DocumentTypeName = userDocModel.DocumentTypeName.ToUpper() == "OTHER" ? userDocModel.DocumentTypeName + " (" + item.OtherDocumentType + ") " : userDocModel.DocumentTypeName;
                        }
                        else
                        {
                            userDocModel.DocumentTypeIdStaff = item.DocumentTypeIdStaff;
                            userDocModel.DocumentTypeNameStaff = _context.MasterDocumentTypesStaff.Where(a => a.Id == item.DocumentTypeIdStaff).FirstOrDefault().Type;
                            userDocModel.DocumentTypeNameStaff = userDocModel.DocumentTypeNameStaff.ToUpper() == "OTHER" ? userDocModel.DocumentTypeNameStaff + " (" + item.OtherDocumentType + ") " : userDocModel.DocumentTypeNameStaff;
                        }

                        userDocModel.Extenstion = Path.GetExtension(item.UploadPath);
                        userDocModel.CreatedDate = CommonMethods.ConvertFromUtcTime((DateTime)item.CreatedDate, token);
                        userDocModel.Key = item.Key;
                        userDocModel.Expiration = item.Expiration;
                        userDocModel.OtherDocumentType = item.OtherDocumentType;
                        userDocListModel.Add(userDocModel);
                    }
                    return new JsonModel()
                    {
                        data = userDocListModel,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = e.Message
                };
            }
        }

        public UserDocumentsResponseModel GetUserDocument(int id, TokenModel token)
        {
            try
            {
                UserDocuments userDoc = _context.UserDocuments.Where(a => a.Id == id && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();

                //Save folder Directory
                string DirectoryUrl = userDoc.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper() ? ImagesPath.UploadClientDocuments : ImagesPath.UploadStaffDocuments;


                if (File.Exists(Directory.GetCurrentDirectory() + DirectoryUrl + userDoc.UploadPath))
                {
                    UserDocumentsResponseModel userDocumentModel = new UserDocumentsResponseModel();
                    string base64string = Directory.GetCurrentDirectory() + DirectoryUrl + userDoc.UploadPath;

                    Byte[] bytes = File.ReadAllBytes(base64string);
                    MemoryStream memoryFile = new MemoryStream(bytes);
                    String file = Convert.ToBase64String(bytes);

                    userDocumentModel.Id = userDoc.Id;
                    userDocumentModel.UserId = userDoc.UserId;
                    userDocumentModel.Base64 = file;
                    userDocumentModel.DocumentTitle = userDoc.DocumentName;
                    userDocumentModel.CreatedDate = CommonMethods.ConvertFromUtcTime((DateTime)userDoc.CreatedDate, token);

                    if (userDoc.Key.ToUpper() == DocumentUserTypeEnum.PATIENT.ToString().ToUpper())
                        userDocumentModel.DocumentTypeName = _context.MasterDocumentTypes.Where(a => a.Id == userDoc.DocumentTypeId).FirstOrDefault().Type;
                    else
                        userDocumentModel.DocumentTypeNameStaff = _context.MasterDocumentTypesStaff.Where(a => a.Id == userDoc.DocumentTypeIdStaff).FirstOrDefault().Type;

                    //userDocumentModel.DocumentTypeName = _context.MasterDocumentTypes.Where(a => a.Id == userDoc.DocumentTypeId).FirstOrDefault().Type;
                    userDocumentModel.Extenstion = Path.GetExtension(userDoc.UploadPath);
                    userDocumentModel.File = memoryFile;
                    userDocumentModel.FileName = userDoc.UploadPath;
                    userDocumentModel.Expiration = userDoc.Expiration;
                    userDocumentModel.OtherDocumentType = userDoc.OtherDocumentType;

                    return userDocumentModel;                    
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonModel DeleteUserDocument(int id, TokenModel token)
        {
            try
            {
                UserDocuments userDoc = _context.UserDocuments.Where(a => a.Id == id && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();
                if (userDoc != null)
                {
                    userDoc.IsDeleted = true;
                    userDoc.DeletedDate = DateTime.UtcNow;
                    _context.Update(userDoc);
                    _context.SaveChanges();

                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.DocumentDelete,
                        StatusCode = (int)HttpStatusCodes.NoContent
                    };
                }
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
            catch (Exception ex)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ErrorOccured,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }

        public JsonModel Logout(TokenModel token)
        {
            Entity.User user = _userRepository.Get(a => a.Id == token.UserID && a.IsActive == true && a.IsDeleted == false);
            if (user != null)
            {
                user.IsOnline = false;
                _userRepository.Update(user);
                //audit logs
                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.Login, AuditLogAction.Logout, null, token.UserID, "", token);

                return new JsonModel()
                {
                    data = new object(),
                    Message = UserAccountNotification.LoggedOut,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = UserAccountNotification.NoDataFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }

        public bool SetOnline(int userId)
        {
            try
            {
                Entity.User user = _userRepository.Get(a => a.Id == userId && a.IsActive == true && a.IsDeleted == false);
                if (user != null)
                {
                    user.IsOnline = true;
                    _userRepository.Update(user);
                    _userRepository.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public JsonModel VerifyOTP(string otp)
        {
            JsonModel result = new JsonModel(); //apiResponse = new ApiResponse();
            //try
            //{
                // result = _userRepository.VerifyOTP(otp);
                //result = _userService.VerifyOTP(otp);
            //    if (result.StatusCode == (int)HttpStatusCode.OK)
            //    {
            //        // string otp = GenerateRandomOTP();
            //        result = new JsonModel
            //        {
            //            data = otp,
            //            Message = StatusMessage.SuccessFul,
            //            StatusCode = (int)HttpStatusCodes.OK
            //        };
            //    }

            //}

            //catch (Exception ex)
            //{
            //    //apiResponse.error.Add(new Error()
            //    //{
            //    //    Code = (int)HttpStatusCode.BadRequest,
            //    //    Status = HttpStatusCode.BadRequest,
            //    //    Detail = ex.Message,
            //    //    Title = CommonMessage.OperationFailed,
            //    //    Message = CommonMessage.Error
            //    //});
            //}
            return result;
        }

        public PasswordCheckModel CheckUserPasswordStatus(TokenModel token)
        {
            PasswordCheckModel passwordCheckModel = null;
            Entity.User user = _userRepository.Get(x => x.Id == token.UserID && x.IsActive);
            if (user != null && user.PasswordResetDate != null)
            {
                int lockNotificationDays = (int)AccountConfiguration.LockNotification;
                passwordCheckModel = new PasswordCheckModel();
                TimeSpan days = user.PasswordResetDate.Value.Date.AddDays(lockNotificationDays) - DateTime.UtcNow.Date;

                if (days.Days <= lockNotificationDays && days.Days > lockNotificationDays - 2)
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Sucess, (int)HttpStatusCodes.OK);
                }
                //if <= 15 or > 10 days left then just show warning to user
                else if (days.Days <= Math.Abs((lockNotificationDays / 4)) && days.Days > Math.Abs((lockNotificationDays / 6)))
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Warning, (int)HttpStatusCodes.OK);
                }
                //if <= 10 or > 5 days left then just show warning to user
                else if (days.Days <= Math.Abs((lockNotificationDays / 6)) && days.Days > Math.Abs((lockNotificationDays / 12))) //warning
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Warning, (int)HttpStatusCodes.OK);
                }
                else if (days.Days <= Math.Abs((lockNotificationDays / 12)) && days.Days > Math.Abs((1))) //warning
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Danger, (int)HttpStatusCodes.Unauthorized);
                }
                //if <= 0 then block the user and show warning to user with red color
                else if (days.Days <= 0)//block 
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.AccountLockNotification + " " + days.Days + " day(s).", true, CssClass.Danger);
                }
                else
                {
                    passwordCheckModel = new PasswordCheckModel(UserAccountNotification.Success, false);
                }
            }
            return passwordCheckModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatePassword"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonModel UpdateExpiredPassword(UpdatePasswordModel updatePassword, TokenModel token)
        {
            try
            {
                Entity.User user = _userRepository.Get(a => a.Id == updatePassword.UserId && a.Password == CommonMethods.Encrypt(updatePassword.CurrentPassword));
                if (user != null)
                {
                    if (updatePassword.NewPassword == updatePassword.ConfirmNewPassword)
                    {
                        List<UserPasswordHistoryModel> passwordHistory = _userPasswordHistoryService.GetUserPasswordHistory(updatePassword.UserId);
                        if (passwordHistory != null && passwordHistory.Count > 0 && passwordHistory.FindAll(x => x.Password == updatePassword.NewPassword).Count() > 0)
                        {
                            return new JsonModel(null, UserAccountNotification.PasswordMatch, (int)HttpStatusCodes.UnprocessedEntity, string.Empty);
                        }
                        using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                user.Password = CommonMethods.Encrypt(updatePassword.NewPassword);
                                user.PasswordResetDate = DateTime.UtcNow;
                                user.UpdatedBy = updatePassword.UserId;//own self
                                user.UpdatedDate = DateTime.UtcNow;
                                _userRepository.Update(user);

                                _userPasswordHistoryService.SaveUserPasswordHistory(updatePassword.UserId, DateTime.UtcNow, user.Password);
                                _userRepository.SaveChanges();

                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                return new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, ex.Message);
                            }
                        }

                        return response = new JsonModel()  //Sucess//
                        {
                            data = user,
                            Message = UserAccountNotification.YourPasswordChanged,
                            StatusCode = (int)HttpStatusCodes.OK
                        };
                    }
                    else // if new current password and confirm password doesn't match
                    {
                        return response = new JsonModel(new object(), UserAccountNotification.YourPasswordNotMatchWithConfirmPassword, (int)HttpStatusCodes.UnprocessedEntity);
                    }

                }
                else // if your current password doesn't match
                {
                    return response = new JsonModel(new object(), UserAccountNotification.YourCurrentPassword, (int)HttpStatusCodes.NotFound);
                }
            }
            catch (Exception ex) // On Error
            {
                return response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCodes.InternalServerError, ex.Message);
            }
        }

    }
}
