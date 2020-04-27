using HC.Common;
using HC.Common.HC.Common;
using HC.Common.Model.Staff;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Staff;
using HC.Patient.Model.Users;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices.Images;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.User;
using HC.Service;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Patient
{
    public class StaffService : BaseService, IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IImageService _imageService;
        private readonly IUserRepository _userRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        private readonly HCOrganizationContext _context;
        private readonly IStaffTagRepository _staffTagRepository;
        private readonly IUserTimesheetByAppointmentTypeRepository _userTimesheetByAppointmentTypeRepository;
        private readonly IUserDetailedDriveTimeRepository _userDetailedDriveTimeRepository;
        private readonly IUserPasswordHistoryService _userPasswordHistoryService;
        private JsonModel response = new JsonModel();

        public StaffService(IStaffRepository staffRepository, IImageService imageService, IUserRepository userRepository, IUserCommonRepository userCommonRepository, IStaffTagRepository staffTagRepository, HCOrganizationContext context, IUserTimesheetByAppointmentTypeRepository userTimesheetByAppointmentTypeRepository, IUserDetailedDriveTimeRepository userDetailedDriveTimeRepository, IUserPasswordHistoryService userPasswordHistoryService)
        {
            _staffRepository = staffRepository;
            _imageService = imageService;
            _userRepository = userRepository;
            _userCommonRepository = userCommonRepository;
            _staffTagRepository = staffTagRepository;
            _context = context;
            _userTimesheetByAppointmentTypeRepository = userTimesheetByAppointmentTypeRepository;
            _userDetailedDriveTimeRepository = userDetailedDriveTimeRepository;
            _userPasswordHistoryService = userPasswordHistoryService;
        }

        public JsonModel GetStaffByTags(ListingFiltterModel listingFiltterModel, TokenModel tokenModel)
        {
            try
            {
                List<StaffModel> staffModels = _staffRepository.GetStaffByTags<StaffModel>(listingFiltterModel, tokenModel).ToList();
                if (staffModels != null && staffModels.Count > 0)
                {
                    staffModels.ForEach(a =>
                    {
                        if (!string.IsNullOrEmpty(a.PhotoThumbnailPath))
                        {
                            a.PhotoThumbnailPath = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, a.PhotoThumbnailPath);
                        }
                    });
                    response = new JsonModel(staffModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                }
                else { response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound); }
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCode.InternalServerError, e.Message);
            }
            return response;
        }

        public JsonModel GetStaffs(ListingFiltterModel listingFiltterModel, TokenModel token)
        {
            try
            {
                //not found
                response.data = new object();
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                /////

                List<StaffModels> staffModels = _staffRepository.GetStaff<StaffModels>(listingFiltterModel, token).ToList();
                if (staffModels != null && staffModels.Count > 0)
                {
                    response.data = staffModels;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.meta = new Meta(staffModels, listingFiltterModel);
                }
            }
            catch (Exception e)
            {
                //not error
                response.data = new object();
                response.Message = StatusMessage.ErrorOccured;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.AppError = e.Message;
                /////
            }
            return response;
        }

        public JsonModel CreateUpdateStaff(Staffs staff, TokenModel token)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                DateTime Currentdate = DateTime.UtcNow;
                try
                {
                    //encrypt password
                    if (!string.IsNullOrEmpty(staff.Password)) { staff.Password = CommonMethods.Encrypt(staff.Password); }
                    //

                    if (staff.Id == 0) //new case
                    {
                        //check duplicate staff
                        Staffs staffDB = _staffRepository.Get(m => (m.Email == staff.Email || m.UserName == staff.UserName) && m.OrganizationID == token.OrganizationID);
                        if (staffDB != null) //if user try to enter duplicate records
                        {
                            response = new JsonModel(new object(), StatusMessage.StaffAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                        }
                        else // insert new staff
                        {
                            staff.OrganizationID = token.OrganizationID;
                            staff.CreatedBy = token.UserID;
                            staff.CreatedDate = Currentdate;
                            staff.IsActive = true;
                            staff.IsDeleted = false;
                            _imageService.ConvertBase64ToImageForUser(staff);

                            //Save User
                            Entity.User requestUser = SaveUser(staff, token, Currentdate);
                            staff.UserID = requestUser.Id;

                            //Map Staff Entity
                            MapStaffEntity(staff, token, Currentdate);

                            //save staff
                            _context.Staffs.Add(staff);
                            _context.SaveChanges();
                            response = new JsonModel(staff, StatusMessage.StaffCreated, (int)HttpStatusCode.OK);
                        }
                    }
                    else
                    {
                        Staffs staffDB = _context.Staffs.Where(a => a.Id == staff.Id).FirstOrDefault();

                        if (staffDB != null)
                        {
                            Entity.User user = _context.User.Where(a => a.Id == staffDB.UserID).FirstOrDefault();
                            bool isPwdUpdated = CommonMethods.Decrypt(user.Password) == CommonMethods.Decrypt(staff.Password) ? false : true;
                            List<UserPasswordHistoryModel> passwordHistory = _userPasswordHistoryService.GetUserPasswordHistory(staff.UserID);
                            if (isPwdUpdated == true && passwordHistory != null && passwordHistory.Count > 0 && passwordHistory.FindAll(x => x.Password == CommonMethods.Decrypt(staff.Password)).Count() > 0)
                            {
                                transaction.Rollback();
                                return new JsonModel(null, UserAccountNotification.PasswordMatch, (int)HttpStatusCodes.UnprocessedEntity, string.Empty);
                            }

                            staffDB = MapStaff(staff, staffDB);

                            //remove staff location
                            List<StaffLocation> staffLocationList = _context.StaffLocation.Where(a => a.StaffId == staffDB.Id).ToList();
                            _context.StaffLocation.RemoveRange(staffLocationList);

                            user.Password = staffDB.Password;
                            user.RoleID = staffDB.RoleID;
                            if (isPwdUpdated)
                                user.PasswordResetDate = DateTime.UtcNow;
                            _context.User.Update(user);

                            //Map Staff Location
                            MapStaffLoaction(staffDB);


                            ///////update staff team////////////////////
                            if (staffDB.StaffTeamList != null && staffDB.StaffTeamList.Count() > 0)
                            { UpdateStaffTeam(staffDB, token, Currentdate); }

                            ///////update staff tag////////////////////
                            if (staffDB.StaffTagsModel != null && staffDB.StaffTagsModel.Count() > 0)
                            { UpdateStaffTags(staffDB, token, Currentdate); }

                            _context.Staffs.Update(staffDB);
                            _context.SaveChanges();
                            if (isPwdUpdated)
                                _userPasswordHistoryService.SaveUserPasswordHistory(staffDB.UserID, DateTime.UtcNow, staffDB.Password);
                            response = new JsonModel(staffDB, StatusMessage.StaffUpdated, (int)HttpStatusCode.OK);
                        }
                        else
                        {
                            response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCode.InternalServerError, e.Message);
                }
                return response;
            }
        }

        private Staffs MapStaff(Staffs staff, Staffs staffDB)
        {
            staffDB.FirstName = staff.FirstName;
            staffDB.LastName = staff.LastName;
            staffDB.MiddleName = staff.MiddleName;
            staffDB.Address = staff.Address;
            staffDB.CountryID = staff.CountryID;
            staffDB.City = staff.City;
            staffDB.StateID = staff.StateID;
            staffDB.Zip = staff.Zip;
            staffDB.Latitude = staff.Latitude;
            staffDB.Longitude = staff.Longitude;
            staffDB.PhoneNumber = staff.PhoneNumber;
            staffDB.NPINumber = staff.NPINumber;
            staffDB.TaxId = staff.TaxId;
            staffDB.DOB = staff.DOB;
            staffDB.DOJ = staff.DOJ;
            staffDB.RoleID = staff.RoleID;
            staffDB.Email = staff.Email;
            staffDB.Gender = staff.Gender;
            staffDB.ApartmentNumber = staff.ApartmentNumber;
            staffDB.CAQHID = staff.CAQHID;
            staffDB.Language = staff.Language;
            staffDB.DegreeID = staff.DegreeID;
            staffDB.EmployeeID = staff.EmployeeID;
            staffDB.Password = staff.Password;
            staffDB.PhotoBase64 = staff.PhotoBase64;
            staffDB.StaffTeamList = staff.StaffTeamList;
            staffDB.StaffLocationList = staff.StaffLocationList;
            staffDB.StaffTagsModel = staff.StaffTagsModel;
            staffDB.PayRate = staff.PayRate;
            //staffDB.PayrollGroupID = staff.PayrollGroupID;
            staffDB.IsRenderingProvider= staff.IsRenderingProvider;
            if (!string.IsNullOrEmpty(staffDB.PhotoBase64))
            {
                staffDB = _imageService.ConvertBase64ToImageForUser(staffDB);
            }

            return staffDB;
        }

        public JsonModel GetStaffById(int id, TokenModel token)
        {
            try
            {
                Staffs staffs = _context.Staffs
                                        .Include(z => z.StaffTeam)
                                        .Include(z => z.StaffLocation)
                                        .Include(z => z.StaffTags)
                                        .Include(z => z.MasterGender)
                                        .Include(z => z.UserRoles)
                                        .Where(a => a.Id == id && a.IsActive == true && a.IsDeleted == false)
                                        .FirstOrDefault();

                if (staffs != null)
                {
                    // with include we can only include the records, but can't execute any condition so condition are separately perform
                    staffs.StaffTeamList = staffs.StaffTeam.Where(z => z.IsDeleted == false).Select(a => new StaffTeamModel { id = a.Id, staffid = a.StaffId, staffteamid = a.StaffTeamID, isdeleted = a.IsDeleted }).ToList();
                    staffs.StaffTeam = null;
                    staffs.StaffLocationList = staffs.StaffLocation.Select(z => new StaffLocationModel { Id = z.LocationID, IsDefault = z.IsDefault }).ToList();
                    staffs.StaffLocation = null;
                    staffs.StaffTagsModel = staffs.StaffTags.Where(a => a.IsDeleted == false).Select(x => new StaffTagsModel { Id = x.Id, IsDeleted = x.IsDeleted, StaffID = x.StaffID, TagID = x.TagID }).ToList();
                    staffs.StaffTags = null;
                    staffs.GenderName = staffs.MasterGender != null ? staffs.MasterGender.Gender : "";
                    staffs.MasterGender = null;
                    staffs.RoleName = staffs.UserRoles != null ? staffs.UserRoles.RoleName : "";
                    staffs.UserRoles = null;
                    //

                    if (staffs != null && !string.IsNullOrEmpty(staffs.PhotoPath) && !string.IsNullOrEmpty(staffs.PhotoThumbnailPath))
                    {
                        staffs.PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffPhotos, staffs.PhotoPath);
                        staffs.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, staffs.PhotoThumbnailPath);
                    }

                    HC.Patient.Entity.User user = _context.User
                                   .Where(a => a.Id == staffs.UserID && a.IsActive == true && a.IsDeleted == false)
                                   .FirstOrDefault();

                    if (!string.IsNullOrEmpty(user.UserName)) { staffs.UserName = user.UserName; }
                    //Decrypt password                    
                    if (!string.IsNullOrEmpty(user.Password)) { staffs.Password = CommonMethods.Decrypt(user.Password); }
                    //

                    response = new JsonModel(staffs, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
                }
                else
                {
                    response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
                }
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, e.Message);
            }
            return response;
        }

        public JsonModel DeleteStaff(int id, TokenModel token)
        {
            try
            {
                List<RecordDependenciesModel> recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.Staffs, true, token).ToList();
                if (recordDependenciesModel != null && recordDependenciesModel.Exists(a => a.TotalCount > 0))
                { response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized); }
                else
                {
                    Staffs staff = _staffRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
                    if (staff != null)
                    {
                        staff.IsDeleted = true;
                        staff.DeletedBy = token.UserID;
                        staff.UpdatedBy = token.UserID;
                        staff.DeletedDate = DateTime.UtcNow;
                        _staffRepository.Update(staff);
                        _staffRepository.SaveChanges();
                        response = new JsonModel(new object(), StatusMessage.StaffDelete, (int)HttpStatusCodes.OK);
                    }
                    else { response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound); }
                }
            }
            catch (Exception e)
            { response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCode.InternalServerError, e.Message); }
            return response;
        }

        public JsonModel UpdateStaffActiveStatus(int staffId, bool isActive, TokenModel token)
        {
            try
            {
                bool status = _userCommonRepository.UpdateStaffActiveStatus(staffId, isActive);
                if (isActive)
                {
                    response = new JsonModel(status, StatusMessage.UserActivation, (int)HttpStatusCodes.OK);
                }
                else
                {
                    response = new JsonModel(status, StatusMessage.UserDeactivation, (int)HttpStatusCodes.OK);
                }
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, e.Message);
            }
            return response;
        }


        #region Helping Method
        private Entity.User SaveUser(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            Entity.User requestUser = new Entity.User();
            requestUser.UserName = entity.UserName;
            requestUser.Password = entity.Password;
            requestUser.RoleID = entity.RoleID; // TO DO it will be dynamic
            requestUser.CreatedBy = token.UserID;
            requestUser.CreatedDate = Currentdate;
            requestUser.IsActive = true;
            requestUser.IsDeleted = false;
            requestUser.OrganizationID = entity.OrganizationID;
            requestUser.PasswordResetDate = DateTime.UtcNow;
            _userRepository.Create(requestUser);
            _userRepository.SaveChanges();
            return requestUser;
        }
        private void MapStaffEntity(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            MapStaffLoaction(entity);

            if (entity.StaffTeamList != null && entity.StaffTeamList.Count() > 0)
            {
                #region Map Staff Team
                List<StaffTeam> staffTeamList = new List<StaffTeam>();
                foreach (StaffTeamModel staffteam in entity.StaffTeamList)
                {
                    StaffTeam team = new StaffTeam();
                    //team.StaffId = savedStaff.Id;
                    team.StaffTeamID = staffteam.staffteamid;
                    team.CreatedBy = token.UserID;
                    team.CreatedDate = Currentdate;
                    staffTeamList.Add(team);
                }
                entity.StaffTeam = staffTeamList;
                #endregion
            }

            if (entity.StaffTagsModel != null && entity.StaffTagsModel.Count > 0)
            {

                #region Map Staff Tags
                List<StaffTags> staffTagList = new List<StaffTags>();
                foreach (StaffTagsModel staffTag in entity.StaffTagsModel)
                {
                    StaffTags tag = new StaffTags();
                    tag.IsActive = true;
                    tag.IsDeleted = false;
                    tag.CreatedBy = token.UserID;
                    tag.CreatedDate = Currentdate;
                    tag.TagID = staffTag.TagID;
                    tag.CreatedBy = token.UserID;
                    tag.CreatedDate = Currentdate;
                    staffTagList.Add(tag);
                }
                entity.StaffTags = staffTagList;
                #endregion}
            }
        }
        private void MapStaffLoaction(Staffs entity)
        {
            if (entity.StaffLocationList != null && entity.StaffLocationList.Count() > 0)
            {
                #region Map Staff Location
                List<StaffLocation> staffLocationList = new List<StaffLocation>();
                foreach (StaffLocationModel staffLoc in entity.StaffLocationList)
                {
                    StaffLocation staffLocation = new StaffLocation();
                    //staffLocation.StaffId = savedStaff.Id;
                    staffLocation.LocationID = staffLoc.Id;
                    staffLocation.IsDefault = staffLoc.IsDefault;
                    staffLocation.OrganizationID = entity.OrganizationID;
                    staffLocationList.Add(staffLocation);
                }
                entity.StaffLocation = staffLocationList;
                #endregion
            }
        }
        private Staffs UpdateStaffTeam(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            List<StaffTeam> staffTeamList = new List<StaffTeam>();
            foreach (StaffTeamModel staffteam in entity.StaffTeamList)
            {
                StaffTeam team = new StaffTeam();
                if (staffteam.id > 0)
                {
                    team = _context.StaffTeam.Where(a => a.Id == staffteam.id).FirstOrDefault();
                    //team.StaffId = staffID;
                    //team.StaffTeamID = staffteam.staffteamid;
                    if (staffteam.isdeleted)
                    {
                        team.IsDeleted = staffteam.isdeleted;
                        team.DeletedBy = token.UserID;
                        team.DeletedDate = Currentdate;
                    }
                    team.UpdatedBy = token.UserID;
                    team.UpdatedDate = Currentdate;
                }
                else
                {
                    team.StaffId = entity.Id;
                    team.StaffTeamID = staffteam.staffteamid;
                    team.CreatedBy = token.UserID;
                    team.CreatedDate = Currentdate;
                }
                staffTeamList.Add(team);
            }
            entity.StaffTeam = staffTeamList;
            return entity;
        }
        private Staffs UpdateStaffTags(Staffs entity, TokenModel token, DateTime Currentdate)
        {
            List<StaffTags> staffTagList = new List<StaffTags>();
            StaffTags team = null;
            foreach (StaffTagsModel stafftag in entity.StaffTagsModel)
            {
                team = new StaffTags();
                if (stafftag.Id > 0)
                {
                    team = _staffTagRepository.Get(a => a.Id == stafftag.Id && a.IsDeleted == false && a.IsActive == true);
                    if (stafftag.IsDeleted)
                    {
                        team.IsDeleted = stafftag.IsDeleted;
                        team.DeletedBy = token.UserID;
                        team.DeletedDate = Currentdate;
                    }
                    team.UpdatedBy = token.UserID;
                    team.UpdatedDate = Currentdate;
                }
                else
                {
                    team.CreatedBy = token.UserID;
                    team.CreatedDate = Currentdate;
                    team.IsActive = true;
                    team.IsDeleted = false;
                    team.StaffID = entity.Id;
                    team.TagID = stafftag.TagID;
                }
                staffTagList.Add(team);
            }
            entity.StaffTags = staffTagList;
            return entity;
        }

        public JsonModel GetDoctorDetailsFromNPI(string npiNumber, string enumerationType)
        {
            //string html = string.Empty;
            //string city = "baltimore";
            //string enumeration_type = "";   //NPI-1 or NPI-2
            //string limit = "200";  //default=10 , max= 200
            //string skip = "";
            //string state = "";
            //string country = "US";
            // var query = "enumeration_type="+enumeration_type + "&limit=" + limit+"&skip="+skip+ "&state="+state+ "&country=" + country+"&city="+city;
            var query = "number=" + npiNumber + "&enumeration_type=" + enumerationType;//1083617021;
            string url = "https://npiregistry.cms.hhs.gov/api?" + query;
            string response = CommonMethods.CreateHTTPRequest(url, null, "GET", "application/json");
            NPIDetailsRootObject root = JsonConvert.DeserializeObject<NPIDetailsRootObject>(response);
            return new JsonModel(root, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, "");
        }

        public JsonModel GetStaffProfileData(int staffId, TokenModel token)
        {
            StaffProfileModel staffProfileModel = _staffRepository.GetStaffProfileData(staffId, token);
            staffProfileModel.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, staffProfileModel.PhotoThumbnailPath);
            staffProfileModel.PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffPhotos, staffProfileModel.PhotoPath);
            return new JsonModel(staffProfileModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
        }




        public JsonModel GetAssignedLocationsById(int staffId, TokenModel tokenModel)
        {
            List<StaffAssignedLocationsModel> staffLocations = _staffRepository.GetAssignedLocationsById<StaffAssignedLocationsModel>(staffId, tokenModel).ToList();
            if (staffLocations != null && staffLocations.Count > 0)
            {
                response = new JsonModel(staffLocations, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);               
            }
            return response;
        }

        public JsonModel GetStaffHeaderData(int staffId, TokenModel tokenModel)
        {
            StaffHeaderDataModel staffHeaderDataModels = _staffRepository.GetStaffHeaderData<StaffHeaderDataModel>(staffId, tokenModel).FirstOrDefault();
            if (staffHeaderDataModels != null)
            {
                staffHeaderDataModels.ProfileImage = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.StaffThumbPhotos, staffHeaderDataModels.ProfileImage);
                response = new JsonModel(staffHeaderDataModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }
        #endregion
    }
}