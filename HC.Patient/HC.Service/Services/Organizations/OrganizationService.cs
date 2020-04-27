using HC.Common;
using HC.Common.Enums;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Organizations;
using HC.Patient.Repositories.IRepositories.Locations;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Repositories.Repositories.Locations;
using HC.Patient.Repositories.Repositories.Organizations;
using HC.Patient.Repositories.Repositories.Staff;
using HC.Patient.Repositories.Repositories.User;
using HC.Patient.Service.IServices.Images;
using HC.Patient.Service.IServices.Organizations;
using HC.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Organizations
{
    public class OrganizationService : BaseService, IOrganizationService
    {
        private HCMasterContext _context;
        private IImageService _imageService;
        private IOrganizationDatabaseRepository _organizationDatabaseRepository;
        private IMasterOrganizationRepository _masterOrganizationRepository;
        private string _organizationRoles = "Admin,Client";
        string Message = string.Empty;
        int StatusCode = 404;
        public OrganizationService(HCMasterContext context, IMasterOrganizationRepository masterOrganizationRepository, IOrganizationDatabaseRepository organizationDatabaseRepository, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
            _organizationDatabaseRepository = organizationDatabaseRepository;
            _masterOrganizationRepository = masterOrganizationRepository;
        }
        /// <summary>
        /// save organization with their child tables
        /// </summary>
        /// <param name="organizationModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonModel SaveOrganization(OrganizationModel organizationModel, TokenModel token, IHttpContextAccessor contextAccessor)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HCOrganizationContext>();
            optionsBuilder.UseSqlServer(GetDomain(organizationModel.DatabaseDetailId));
            HCOrganizationContext _organizationContext = new HCOrganizationContext(optionsBuilder.Options, contextAccessor);
            OrganizationRepository _organizationRepository = new OrganizationRepository(_organizationContext);
            UserRepository _userRepository = new UserRepository(_organizationContext);
            StaffRepository _staffRepository = new StaffRepository(_organizationContext);
            LocationRepository _locationRepository = new LocationRepository(_organizationContext);
            OrganizationSubscriptionPlanRepository _organizationSubscriptionPlanRepository = new OrganizationSubscriptionPlanRepository(_organizationContext);
            OrganizationSMTPRepository _organizationSMTPRepository = new OrganizationSMTPRepository(_organizationContext);

            Organization requestOrganizationObj = null;
            //master
            MasterOrganization requestMasterOrganizationObj = null;
            //
            List<OrganizationSubscriptionPlan> requestSubscriptionObj = null;
            List<OrganizationSMTPDetails> requestSMTPObj = null;
            //update obj
            OrganizationSubscriptionPlan updateSubscriptionObj = null;
            OrganizationSubscriptionPlan newSubscriptionObj = null;
            List<OrganizationSubscriptionPlan> updateSubscriptionObjList = null;
            List<OrganizationSubscriptionPlan> newSubscriptionObjList = null;

            OrganizationSMTPDetails updateSMTPObj = null;
            OrganizationSMTPDetails newSMTPObj = null;
            List<OrganizationSMTPDetails> updateSMTPObjList = null;
            List<OrganizationSMTPDetails> newSMTPObjList = null;

            //User and Staff
            Entity.User requestUser = null;
            Entity.Staffs requestStaff = null;
            // Roles for this Organization
            List<UserRoles> requestUserRoles = null;
            //location
            Location requestLocation = null;

            using (var transaction = _organizationContext.Database.BeginTransaction()) //TO DO do this with SP
            {
                try
                {

                    Organization org = _organizationRepository.Get(x => x.BusinessName == organizationModel.BusinessName && x.IsDeleted == false);//&& x.IsActive == true
                    MasterOrganization masOrg = _masterOrganizationRepository.Get(x => x.BusinessName == organizationModel.BusinessName && x.IsDeleted == false); //&& x.IsActive == true
                    if ((org == null && masOrg == null && organizationModel.Id == 0) || (organizationModel.Id > 0)) //both case new insert and update
                    {
                        if (!string.IsNullOrEmpty(organizationModel.Password))//null password Check 
                        {
                            organizationModel.Password = CommonMethods.Encrypt(organizationModel.Password);
                            organizationModel.OrganizationSMTPDetail = EncryptMethod(organizationModel.OrganizationSMTPDetail);
                        }
                        if (!ReferenceEquals(organizationModel, null) && organizationModel.Id == 0) //insert
                        {
                            //master
                            requestMasterOrganizationObj = new MasterOrganization();
                            //

                            requestOrganizationObj = new Entity.Organization();
                            requestSubscriptionObj = new List<OrganizationSubscriptionPlan>();
                            requestSMTPObj = new List<OrganizationSMTPDetails>();

                            //User and Staff////
                            requestUser = new Entity.User();
                            requestStaff = new Staffs();
                            // Role for this Organization
                            requestUserRoles = new List<UserRoles>();
                            //////location
                            requestLocation = new Location();

                            //master
                            AutoMapper.Mapper.Map(organizationModel, requestMasterOrganizationObj);
                            //
                            AutoMapper.Mapper.Map(organizationModel, requestOrganizationObj);
                            AutoMapper.Mapper.Map(organizationModel.OrganizationSubscriptionPlans, requestSubscriptionObj);
                            AutoMapper.Mapper.Map(organizationModel.OrganizationSMTPDetail, requestSMTPObj);

                            #region Save Logo and Favicon
                            string LogoUrl = _imageService.SaveImages(organizationModel.LogoBase64, ImagesPath.OrganizationImages, ImagesFolderEnum.Logo.ToString());
                            string FaviconUrl = _imageService.SaveImages(organizationModel.FaviconBase64, ImagesPath.OrganizationImages, ImagesFolderEnum.Favicon.ToString());
                            #endregion

                            //master
                            requestMasterOrganizationObj.CreatedBy = null;//token.UserID;
                            requestMasterOrganizationObj.CreatedDate = DateTime.UtcNow;
                            requestMasterOrganizationObj.IsActive = true;
                            requestMasterOrganizationObj.IsDeleted = false;
                            requestMasterOrganizationObj.Logo = LogoUrl;
                            requestMasterOrganizationObj.Favicon = FaviconUrl;
                            //save master
                            _masterOrganizationRepository.Create(requestMasterOrganizationObj);
                            _masterOrganizationRepository.SaveChanges();

                            // organization save get id from master organization then assign to organziation
                            requestOrganizationObj.Id = requestMasterOrganizationObj.Id;
                            requestOrganizationObj.CreatedBy = null;// token.UserID;
                            requestOrganizationObj.CreatedDate = DateTime.UtcNow;
                            requestOrganizationObj.IsActive = true;
                            requestOrganizationObj.IsDeleted = false;
                            requestOrganizationObj.Logo = LogoUrl;
                            requestOrganizationObj.Favicon = FaviconUrl;

                            _organizationRepository.Create(requestOrganizationObj);
                            _organizationRepository.SaveChanges();

                            ////////////////////////Added 2 Default Roles //////////////
                            SaveRoles(requestMasterOrganizationObj, requestUserRoles, _organizationContext);
                            /////////////////////////User //////////////////
                            SaveUser(organizationModel, token, requestMasterOrganizationObj, requestUser, _userRepository, _organizationContext);
                            /////////////////////////Staff//////////////////
                            SaveStaff(organizationModel, token, requestMasterOrganizationObj, requestUser, requestStaff, _staffRepository, _organizationContext);
                            /////////////////////////Location///////////////
                            SaveLocation(organizationModel, token, requestMasterOrganizationObj, requestLocation, _locationRepository);
                            //////////////////////////Staff Location/////////////////////
                            SaveDefaultStaffLocation(requestStaff, requestLocation, _organizationContext);
                            /////////////////////// Copied App Configration ////////////
                            //CopiedMasterAppConfigurations(requestMasterOrganizationObj, _organizationContext);
                            ///////////////////////// Copied Security Question ///////////
                            //CopiedSecurityQuestionFromMaster(requestMasterOrganizationObj, _organizationContext);

                            //organization subscription
                            if (requestOrganizationObj.Id > 0)
                            {
                                requestSubscriptionObj.ForEach(x => { x.OrganizationID = requestMasterOrganizationObj.Id; x.CreatedBy = null; x.CreatedDate = DateTime.UtcNow; x.IsActive = true; x.IsDeleted = false; });
                                _organizationSubscriptionPlanRepository.Create(requestSubscriptionObj.ToArray());
                                _organizationSubscriptionPlanRepository.SaveChanges();
                            }
                            //organization smtp
                            if (requestOrganizationObj.Id > 0)
                            {
                                requestSMTPObj.ForEach(x => { x.OrganizationID = requestMasterOrganizationObj.Id; x.CreatedBy = null; x.CreatedDate = DateTime.UtcNow; x.IsActive = true; x.IsDeleted = false; });
                                _organizationSMTPRepository.Create(requestSMTPObj.ToArray());
                                _organizationSMTPRepository.SaveChanges();
                            }
                            //Message
                            Message = StatusMessage.AgencySaved;
                            StatusCode = (int)HttpStatusCodes.OK;   //Success
                        }
                        else//update case
                        {
                            //master
                            MasterOrganization UpMtrOrg = _masterOrganizationRepository.Get(x => x.BusinessName == organizationModel.BusinessName && x.Id != organizationModel.Id && x.IsDeleted == false); // && x.IsActive == true
                            //
                            Organization UpOrg = _organizationRepository.Get(x => x.BusinessName == organizationModel.BusinessName && x.Id != organizationModel.Id && x.IsDeleted == false);//&& x.IsActive == true

                            if (UpOrg == null && UpMtrOrg == null)
                            {
                                //master
                                requestMasterOrganizationObj = _masterOrganizationRepository.Get(x => x.Id == organizationModel.Id && x.IsDeleted == false);// && x.IsActive == true
                                //                                
                                requestOrganizationObj = _organizationRepository.Get(x => x.Id == organizationModel.Id && x.IsDeleted == false); //&& x.IsActive == true

                                updateSubscriptionObjList = new List<OrganizationSubscriptionPlan>();
                                newSubscriptionObjList = new List<OrganizationSubscriptionPlan>();
                                updateSMTPObjList = new List<OrganizationSMTPDetails>();
                                newSMTPObjList = new List<OrganizationSMTPDetails>();
                                if (requestOrganizationObj != null && requestMasterOrganizationObj != null)
                                {
                                    requestOrganizationObj.UpdatedBy = null;//token.UserID;
                                    requestOrganizationObj.UpdatedDate = DateTime.UtcNow;
                                    requestOrganizationObj.Address1 = organizationModel.Address1;
                                    requestOrganizationObj.Address2 = organizationModel.Address2;
                                    requestOrganizationObj.City = organizationModel.City;
                                    requestOrganizationObj.ContactPersonFirstName = organizationModel.ContactPersonFirstName;
                                    requestOrganizationObj.ContactPersonLastName = organizationModel.ContactPersonLastName;
                                    requestOrganizationObj.ContactPersonMiddleName = organizationModel.ContactPersonMiddleName;
                                    requestOrganizationObj.ContactPersonPhoneNumber = organizationModel.ContactPersonPhoneNumber;
                                    requestOrganizationObj.CountryID = organizationModel.CountryID;
                                    //update database
                                    requestOrganizationObj.DatabaseDetailId = organizationModel.DatabaseDetailId;
                                    requestOrganizationObj.Description = organizationModel.Description;
                                    requestOrganizationObj.Email = organizationModel.Email;
                                    requestOrganizationObj.PayrollStartWeekDay = organizationModel.PayrollStartWeekDay;
                                    requestOrganizationObj.PayrollEndWeekDay = organizationModel.PayrollEndWeekDay;
                                    //images
                                    if (!string.IsNullOrEmpty(organizationModel.LogoBase64))
                                        requestOrganizationObj.Logo = organizationModel.LogoBase64.Contains("http") ? requestOrganizationObj.Logo : _imageService.SaveImages(organizationModel.LogoBase64, ImagesPath.OrganizationImages, ImagesFolderEnum.Logo.ToString());
                                    else
                                        requestOrganizationObj.Logo = string.Empty;
                                    if (!string.IsNullOrEmpty(organizationModel.FaviconBase64))
                                        requestOrganizationObj.Favicon = organizationModel.FaviconBase64.Contains("http") ? requestOrganizationObj.Favicon : _imageService.SaveImages(organizationModel.FaviconBase64, ImagesPath.OrganizationImages, ImagesFolderEnum.Favicon.ToString());
                                    else
                                        requestOrganizationObj.Favicon = string.Empty;
                                    //For mapping
                                    organizationModel.Logo = requestOrganizationObj.Logo;
                                    organizationModel.Favicon = requestOrganizationObj.Favicon;
                                    //
                                    requestOrganizationObj.Fax = organizationModel.Fax;
                                    requestOrganizationObj.IsDeleted = organizationModel.IsDeleted;
                                    requestOrganizationObj.IsActive = organizationModel.IsActive;
                                    requestOrganizationObj.Latitude = organizationModel.Latitude;
                                    requestOrganizationObj.Longitude = organizationModel.Longitude;
                                    requestOrganizationObj.ApartmentNumber = organizationModel.ApartmentNumber;

                                    requestOrganizationObj.VendorIdDirect = organizationModel.VendorIdDirect;
                                    requestOrganizationObj.VendorNameDirect = organizationModel.VendorNameDirect;
                                    requestOrganizationObj.VendorIdIndirect = organizationModel.VendorIdIndirect;
                                    requestOrganizationObj.VendorNameIndirect = organizationModel.VendorNameIndirect;

                                    requestOrganizationObj.OrganizationName = organizationModel.OrganizationName;
                                    requestOrganizationObj.Password = organizationModel.Password;
                                    requestOrganizationObj.Phone = organizationModel.Phone;
                                    requestOrganizationObj.StateID = organizationModel.StateID;
                                    requestOrganizationObj.UserName = organizationModel.UserName;
                                    requestOrganizationObj.Zip = organizationModel.Zip;
                                    requestOrganizationObj.BusinessName = organizationModel.BusinessName;
                                    _organizationRepository.Update(requestOrganizationObj);
                                    _organizationRepository.SaveChanges();


                                    //User and Staff////
                                    requestUser = _organizationContext.User.Where(k => k.UserName == requestOrganizationObj.UserName).FirstOrDefault();
                                    requestStaff = _organizationContext.Staffs.Where(k => k.UserID == requestUser.Id).FirstOrDefault();
                                    /////////////////////////User //////////////////
                                    SaveUser(organizationModel, token, requestMasterOrganizationObj, requestUser, _userRepository, _organizationContext);
                                    /////////////////////////Staff//////////////////
                                    SaveStaff(organizationModel, token, requestMasterOrganizationObj, requestUser, requestStaff, _staffRepository, _organizationContext);

                                    MappingMasterOrganization(organizationModel, token, requestMasterOrganizationObj);
                                    //database subscription updates
                                    requestSubscriptionObj = _organizationSubscriptionPlanRepository.GetAll(a => a.OrganizationID == requestOrganizationObj.Id && a.IsDeleted == false && a.IsActive == true).ToList();//get all old records
                                    foreach (OrganizationSubscriptionPlanModel model in organizationModel.OrganizationSubscriptionPlans)
                                    {
                                        updateSubscriptionObj = requestSubscriptionObj.Where(a => a.Id == model.Id).FirstOrDefault();
                                        if (updateSubscriptionObj != null)
                                        {
                                            updateSubscriptionObj.UpdatedBy = null;// token.UserID;
                                            updateSubscriptionObj.UpdatedDate = DateTime.UtcNow;
                                            updateSubscriptionObj.AmountPerClient = model.AmountPerClient;
                                            updateSubscriptionObj.TotalNumberOfClients = model.TotalNumberOfClients;
                                            updateSubscriptionObj.IsDeleted = model.IsDeleted;
                                            updateSubscriptionObj.OrganizationID = requestOrganizationObj.Id;//from request
                                            updateSubscriptionObj.PlanName = model.PlanName;
                                            updateSubscriptionObj.PlanType = model.PlanType;
                                            updateSubscriptionObj.StartDate = model.StartDate;
                                            //add into list
                                            updateSubscriptionObjList.Add(updateSubscriptionObj);
                                        }
                                        else //insert into new
                                        {
                                            newSubscriptionObj = new OrganizationSubscriptionPlan();
                                            newSubscriptionObj.AmountPerClient = model.AmountPerClient;
                                            newSubscriptionObj.IsDeleted = false;
                                            newSubscriptionObj.IsActive = true;
                                            newSubscriptionObj.OrganizationID = requestOrganizationObj.Id;//from request 
                                            newSubscriptionObj.PlanName = model.PlanName;
                                            newSubscriptionObj.PlanType = model.PlanType;
                                            newSubscriptionObj.StartDate = model.StartDate;
                                            newSubscriptionObj.CreatedBy = null;// token.UserID;
                                            newSubscriptionObj.CreatedDate = DateTime.UtcNow;
                                            //add into list
                                            newSubscriptionObjList.Add(newSubscriptionObj);
                                        }
                                    }
                                    _organizationSubscriptionPlanRepository.Update(updateSubscriptionObjList.ToArray());
                                    _organizationSubscriptionPlanRepository.Create(newSubscriptionObjList.ToArray());

                                    _organizationSubscriptionPlanRepository.SaveChanges();
                                    //database smtp updates
                                    requestSMTPObj = _organizationSMTPRepository.GetAll(a => a.OrganizationID == requestOrganizationObj.Id && a.IsDeleted == false && a.IsActive == true).ToList();
                                    foreach (OrganizationSMTPDetailsModel model in organizationModel.OrganizationSMTPDetail)
                                    {
                                        updateSMTPObj = requestSMTPObj.Where(a => a.Id == model.Id).FirstOrDefault();
                                        if (updateSMTPObj != null)
                                        {
                                            updateSMTPObj.UpdatedBy = null;// token.UserID;
                                            updateSMTPObj.UpdatedDate = DateTime.UtcNow;
                                            updateSMTPObj.ConnectionSecurity = model.ConnectionSecurity;
                                            updateSMTPObj.IsDeleted = model.IsDeleted;
                                            updateSMTPObj.OrganizationID = requestOrganizationObj.Id;//from request
                                            updateSMTPObj.SMTPPassword = model.SMTPPassword;
                                            updateSMTPObj.Port = model.Port;
                                            updateSMTPObj.ServerName = model.ServerName;
                                            updateSMTPObj.SMTPUserName = model.SMTPUserName;
                                            //add into list
                                            updateSMTPObjList.Add(updateSMTPObj);
                                        }
                                        else //insert into new
                                        {
                                            newSMTPObj = new OrganizationSMTPDetails();
                                            newSMTPObj.CreatedBy = null;// token.UserID;
                                            newSMTPObj.CreatedDate = DateTime.UtcNow;
                                            newSMTPObj.ConnectionSecurity = model.ConnectionSecurity;
                                            newSMTPObj.IsDeleted = false;
                                            newSMTPObj.IsActive = true;
                                            newSMTPObj.OrganizationID = requestOrganizationObj.Id;//from request
                                            newSMTPObj.SMTPPassword = model.SMTPPassword;
                                            newSMTPObj.Port = model.Port;
                                            newSMTPObj.ServerName = model.ServerName;
                                            newSMTPObj.SMTPUserName = model.SMTPUserName;
                                            //add into list
                                            newSMTPObjList.Add(newSMTPObj);
                                        }
                                    }
                                    _organizationSMTPRepository.Update(updateSMTPObjList.ToArray());
                                    _organizationSMTPRepository.Create(newSMTPObjList.ToArray());
                                    _organizationSMTPRepository.SaveChanges();
                                    //Message
                                    Message = StatusMessage.AgencyUpdatedSuccessfully;
                                    StatusCode = (int)HttpStatusCodes.OK;   //Success
                                }
                            }
                            else
                            {
                                Message = StatusMessage.AgencyAlredyExist;
                                StatusCode = (int)HttpStatusCodes.Conflict;   //Conflict
                                return new JsonModel()
                                {
                                    data = new object(),
                                    Message = Message,
                                    StatusCode = StatusCode
                                };
                            }
                        }

                        //transaction commit
                        transaction.Commit();
                    }
                    else
                    {
                        Message = StatusMessage.AgencyAlredyExist;
                        StatusCode = (int)HttpStatusCodes.Conflict;   //Conflict
                    }
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = Message,
                        StatusCode = StatusCode
                    };
                }
                catch (DbUpdateException ex)
                {
                    //on error transaction rollback
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = ex.Message,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                    };
                }
            }
        }

        /// <summary>
        /// to get organization by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonModel GetOrganizationById(int Id, IHttpContextAccessor contextAccessor)
        {
            //response model
            OrganizationModel responseOrganizationObj = new OrganizationModel();
            responseOrganizationObj.OrganizationSubscriptionPlans = new List<OrganizationSubscriptionPlanModel>();
            responseOrganizationObj.OrganizationSMTPDetail = new List<OrganizationSMTPDetailsModel>();

            //master organization
            MasterOrganization organizationDbObj = _masterOrganizationRepository.Get(a => a.Id == Id && a.IsDeleted == false); //&& a.IsActive == true

            var optionsBuilder = new DbContextOptionsBuilder<HCOrganizationContext>();
            optionsBuilder.UseSqlServer(GetDomain(organizationDbObj.DatabaseDetailId));
            HCOrganizationContext _organizationContext = new HCOrganizationContext(optionsBuilder.Options, contextAccessor);
            OrganizationSubscriptionPlanRepository _organizationSubscriptionPlanRepository = new OrganizationSubscriptionPlanRepository(_organizationContext);
            OrganizationSMTPRepository _organizationSMTPRepository = new OrganizationSMTPRepository(_organizationContext);

            UserRepository _userRepository = new UserRepository(_organizationContext);

            if (organizationDbObj != null)
            {
                //here only come admin role id only
                int RoleId = _organizationContext.UserRoles.Where(a => a.OrganizationID == organizationDbObj.Id && a.UserType.ToLower() == UserTypeEnum.ADMIN.ToString().ToLower() && a.IsActive == true && a.IsDeleted == false).FirstOrDefault().Id;
                //password from user table for same admin
                Entity.User user = _userRepository.Get(a => a.OrganizationID == organizationDbObj.Id && a.RoleID == RoleId && a.IsActive == true && a.IsDeleted == false);
                organizationDbObj.Password = CommonMethods.Decrypt(user.Password);//password decrypt
                organizationDbObj.UserName = user.UserName;
                List<OrganizationSubscriptionPlan> organizationSubscriptionDbObj = _organizationSubscriptionPlanRepository.GetAll(a => a.OrganizationID == Id && a.IsActive == true && a.IsDeleted == false).ToList();
                List<OrganizationSMTPDetails> organizationSMTPDbObj = _organizationSMTPRepository.GetAll(a => a.OrganizationID == Id && a.IsActive == true && a.IsDeleted == false).ToList();

                AutoMapper.Mapper.Map(organizationDbObj, responseOrganizationObj);
                AutoMapper.Mapper.Map(organizationSubscriptionDbObj, responseOrganizationObj.OrganizationSubscriptionPlans);
                AutoMapper.Mapper.Map(organizationSMTPDbObj, responseOrganizationObj.OrganizationSMTPDetail);
                responseOrganizationObj.OrganizationSMTPDetail = DecryptMethod(responseOrganizationObj.OrganizationSMTPDetail);//password decrypt
                //get name of db
                responseOrganizationObj.DatabaseName = _context.OrganizationDatabaseDetail.Where(a => a.Id == responseOrganizationObj.DatabaseDetailId).FirstOrDefault().DatabaseName;
                #region get the image url
                if (!string.IsNullOrEmpty(responseOrganizationObj.Logo))
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Logo//" + responseOrganizationObj.Logo))
                        responseOrganizationObj.Logo = CommonMethods.CreateImageUrl(contextAccessor.HttpContext, ImagesPath.OrganizationImages + "Logo//", responseOrganizationObj.Logo); //contextAccessor.HttpContext.Request.Scheme+"://"+ contextAccessor.HttpContext.Request.Host + ImagesPath.OrganizationImages + "Logo\\"+  responseOrganizationObj.Logo;
                    else
                        responseOrganizationObj.Logo = string.Empty;
                }
                else
                {
                    responseOrganizationObj.Logo = string.Empty;
                }
                if (!string.IsNullOrEmpty(responseOrganizationObj.Favicon))
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Favicon//" + responseOrganizationObj.Favicon))
                        responseOrganizationObj.Favicon = CommonMethods.CreateImageUrl(contextAccessor.HttpContext, ImagesPath.OrganizationImages + "Favicon//", responseOrganizationObj.Favicon);//contextAccessor.HttpContext.Request.Scheme + "://" + contextAccessor.HttpContext.Request.Host + ImagesPath.OrganizationImages + "Favicon\\" + responseOrganizationObj.Favicon;
                    else
                        responseOrganizationObj.Favicon = string.Empty;
                }
                else
                {
                    responseOrganizationObj.Favicon = string.Empty;
                }
                #endregion

                return new JsonModel()
                {
                    data = responseOrganizationObj,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK//Success
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound//Success
                };
            }
        }
        /// <summary>
        /// get all organizations
        /// </summary>
        /// <returns></returns>
        public JsonModel GetOrganizations(string businessName = "", string orgName = "", string country = "", string sortOrder = "", string sortColumn = "", int page = 0, int pageSize = 10)
        {
            //response model

            List<MasterOrganizationModel> response = _masterOrganizationRepository.GetMasterOrganizations(businessName, orgName, country, sortOrder, sortColumn, page, pageSize);
            if (response != null && response.Count > 0)
            {
                return new JsonModel()
                {
                    data = response,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = response.Count > 0 ? response[0].TotalRecords : 0,
                        CurrentPage = page,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(response[0].TotalRecords / pageSize)) 
                    },
                    StatusCode = (int)HttpStatusCodes.OK//Success
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

        public JsonModel GetOrganizationDetailsById(TokenModel token)
        {
            //response model

            OrganizationDetailModel organisationList = _masterOrganizationRepository.GetOrganizationDetailsById(token);
            #region get the image url
            if (!string.IsNullOrEmpty(organisationList.Logo))
            {
                if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Logo//" + organisationList.Logo))
                    organisationList.Logo = CommonMethods.CreateImageUrl(token.Request, ImagesPath.OrganizationImages + "Logo//", organisationList.Logo); //contextAccessor.HttpContext.Request.Scheme+"://"+ contextAccessor.HttpContext.Request.Host + ImagesPath.OrganizationImages + "Logo\\"+  organisationList.Logo;
                else
                    organisationList.Logo = string.Empty;
            }
            else
            {
                organisationList.Logo = string.Empty;
            }
            if (!string.IsNullOrEmpty(organisationList.Favicon))
            {
                if (File.Exists(Directory.GetCurrentDirectory() + ImagesPath.OrganizationImages + "Favicon//" + organisationList.Favicon))
                    organisationList.Favicon = CommonMethods.CreateImageUrl(token.Request, ImagesPath.OrganizationImages + "Favicon//", organisationList.Favicon);//contextAccessor.HttpContext.Request.Scheme + "://" + contextAccessor.HttpContext.Request.Host + ImagesPath.OrganizationImages + "Favicon\\" + organisationList.Favicon;
                else
                    organisationList.Favicon = string.Empty;
            }
            else
            {
                organisationList.Favicon = string.Empty;
            }
            #endregion
            JsonModel response = new JsonModel(organisationList, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            return response;
        }
        /// <summary>
        /// delete organization by id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonModel DeleteOrganization(int Id, TokenModel token, IHttpContextAccessor contextAccessor)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //master
                    MasterOrganization masterOrganizationObj = _masterOrganizationRepository.Get(a => a.Id == Id && a.IsActive == true && a.IsDeleted == false);

                    var optionsBuilder = new DbContextOptionsBuilder<HCOrganizationContext>();
                    optionsBuilder.UseSqlServer(GetDomain(masterOrganizationObj.DatabaseDetailId));
                    HCOrganizationContext _organizationContext = new HCOrganizationContext(optionsBuilder.Options, contextAccessor);
                    OrganizationRepository _organizationRepository = new OrganizationRepository(_organizationContext);
                    OrganizationSubscriptionPlanRepository _organizationSubscriptionPlanRepository = new OrganizationSubscriptionPlanRepository(_organizationContext);
                    OrganizationSMTPRepository _organizationSMTPRepository = new OrganizationSMTPRepository(_organizationContext);

                    Organization organizationObj = _organizationRepository.Get(a => a.Id == Id && a.IsActive == true && a.IsDeleted == false);


                    List<OrganizationSubscriptionPlan> organizationSubscriptionObj = _organizationSubscriptionPlanRepository.GetAll(a => a.OrganizationID == Id && a.IsActive == true && a.IsDeleted == false).ToList();
                    List<OrganizationSMTPDetails> organizationSMTPObj = _organizationSMTPRepository.GetAll(a => a.OrganizationID == Id && a.IsActive == true && a.IsDeleted == false).ToList();

                    if (organizationObj != null)
                    {
                        //set IsDeleted = 1(true)
                        if (organizationSMTPObj.Count() > 0) { organizationSMTPObj.ForEach(a => { a.IsDeleted = false; a.DeletedBy = token.UserID; a.DeletedDate = DateTime.UtcNow; }); }
                        if (organizationSubscriptionObj.Count() > 0) { organizationSubscriptionObj.ForEach(a => { a.IsDeleted = false; a.DeletedBy = token.UserID; a.DeletedDate = DateTime.UtcNow; }); }
                        organizationObj.IsDeleted = true;
                        organizationObj.DeletedDate = DateTime.UtcNow;
                        organizationObj.DeletedBy = null;//token.UserID;
                        //master
                        masterOrganizationObj.IsDeleted = true;
                        masterOrganizationObj.DeletedDate = DateTime.UtcNow;
                        masterOrganizationObj.DeletedBy = null;// token.UserID;
                        //
                        //update
                        _organizationRepository.Update(organizationObj);
                        //master
                        _masterOrganizationRepository.Update(masterOrganizationObj);
                        //
                        _organizationSubscriptionPlanRepository.Update(organizationSubscriptionObj.ToArray());
                        _organizationSMTPRepository.Update(organizationSMTPObj.ToArray());
                        //save
                        _organizationRepository.SaveChanges();
                        //master
                        _masterOrganizationRepository.SaveChanges();
                        //
                        _organizationSubscriptionPlanRepository.SaveChanges();
                        _organizationSMTPRepository.SaveChanges();
                        //transaction commited
                        transaction.Commit();
                        //message
                        Message = StatusMessage.Delete;
                    }
                    else
                    {
                        Message = StatusMessage.NotFound;
                    }
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = Message,
                        StatusCode = (int)HttpStatusCodes.OK//Success
                    };
                }
                catch (Exception e)
                {

                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = e.Message,
                        StatusCode = (int)HttpStatusCodes.InternalServerError//Error
                    };
                }
            }
        }

        public JsonModel CheckOrganizationBusinessName(string BusinessName)
        {
            MasterOrganization organization = _masterOrganizationRepository.Get(a => a.BusinessName == BusinessName && a.IsActive == true && a.IsDeleted == false);
            bool Status = false;
            if (organization != null) { Status = true; }
            return new JsonModel()
            {
                data = Status,
                Message = Message,
                StatusCode = (int)HttpStatusCodes.OK//Success
            };
        }
        public List<OrganizationSMTPDetailsModel> EncryptMethod(List<OrganizationSMTPDetailsModel> list)
        {
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.SMTPPassword))
                {
                    item.SMTPPassword = CommonMethods.Encrypt(item.SMTPPassword);
                }
            }
            return list;
        }

        public List<OrganizationSMTPDetailsModel> DecryptMethod(List<OrganizationSMTPDetailsModel> list)
        {
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.SMTPPassword))
                {
                    item.SMTPPassword = CommonMethods.Decrypt(item.SMTPPassword);
                }
            }
            return list;
        }



        #region All Local Organization db
        private void MappingMasterOrganization(OrganizationModel organizationModel, TokenModel token, MasterOrganization requestMasterOrganizationObj)
        {
            requestMasterOrganizationObj.UpdatedBy = null;// token.UserID;
            requestMasterOrganizationObj.UpdatedDate = DateTime.UtcNow;
            requestMasterOrganizationObj.Address1 = organizationModel.Address1;
            requestMasterOrganizationObj.Address2 = organizationModel.Address2;
            requestMasterOrganizationObj.City = organizationModel.City;
            requestMasterOrganizationObj.ContactPersonFirstName = organizationModel.ContactPersonFirstName;
            requestMasterOrganizationObj.ContactPersonLastName = organizationModel.ContactPersonLastName;
            requestMasterOrganizationObj.ContactPersonMiddleName = organizationModel.ContactPersonMiddleName;
            requestMasterOrganizationObj.ContactPersonPhoneNumber = organizationModel.ContactPersonPhoneNumber;
            requestMasterOrganizationObj.CountryID = organizationModel.CountryID;
            //update database
            requestMasterOrganizationObj.DatabaseDetailId = organizationModel.DatabaseDetailId;
            requestMasterOrganizationObj.Description = organizationModel.Description;
            requestMasterOrganizationObj.Email = organizationModel.Email;
            requestMasterOrganizationObj.Fax = organizationModel.Fax;
            requestMasterOrganizationObj.IsDeleted = organizationModel.IsDeleted;
            requestMasterOrganizationObj.IsActive = organizationModel.IsActive;
            requestMasterOrganizationObj.Latitude = organizationModel.Latitude;
            requestMasterOrganizationObj.Longitude = organizationModel.Longitude;
            requestMasterOrganizationObj.ApartmentNumber = organizationModel.ApartmentNumber;

            requestMasterOrganizationObj.VendorIdDirect = organizationModel.VendorIdDirect;
            requestMasterOrganizationObj.VendorNameDirect = organizationModel.VendorNameDirect;
            requestMasterOrganizationObj.VendorIdIndirect = organizationModel.VendorIdIndirect;
            requestMasterOrganizationObj.VendorNameIndirect = organizationModel.VendorNameIndirect;
            requestMasterOrganizationObj.PayrollStartWeekDay = organizationModel.PayrollStartWeekDay;
            requestMasterOrganizationObj.PayrollEndWeekDay = organizationModel.PayrollEndWeekDay;
            ////images
            //if (!string.IsNullOrEmpty(organizationModel.LogoBase64))
            //    requestMasterOrganizationObj.Logo = _imageService.SaveImages(organizationModel.LogoBase64, ImagesPath.OrganizationImages, ImagesFolderEnum.Logo.ToString());
            //else
            //    requestMasterOrganizationObj.Logo = string.Empty;
            //if (!string.IsNullOrEmpty(organizationModel.FaviconBase64))
            //    requestMasterOrganizationObj.Favicon = _imageService.SaveImages(organizationModel.FaviconBase64, ImagesPath.OrganizationImages, ImagesFolderEnum.Favicon.ToString());
            //else
            //    requestMasterOrganizationObj.Favicon = string.Empty;
            ////
            requestMasterOrganizationObj.Logo = organizationModel.Logo;
            requestMasterOrganizationObj.Favicon = organizationModel.Favicon;
            requestMasterOrganizationObj.OrganizationName = organizationModel.OrganizationName;
            requestMasterOrganizationObj.Password = organizationModel.Password;
            requestMasterOrganizationObj.Phone = organizationModel.Phone;
            requestMasterOrganizationObj.StateID = organizationModel.StateID;
            requestMasterOrganizationObj.UserName = organizationModel.UserName;
            requestMasterOrganizationObj.Zip = organizationModel.Zip;
            requestMasterOrganizationObj.BusinessName = organizationModel.BusinessName;
            _masterOrganizationRepository.Update(requestMasterOrganizationObj);
            _masterOrganizationRepository.SaveChanges();
        }

        private void CopiedSecurityQuestionFromMaster(MasterOrganization requestMasterOrganizationObj, HCOrganizationContext con)
        {
            List<MasterSecurityQuestions> masterSecurityQuestions = _context.MasterSecurityQuestions.ToList();
            List<SecurityQuestions> securityQuestions = new List<SecurityQuestions>();
            AutoMapper.Mapper.Map(masterSecurityQuestions, securityQuestions);
            securityQuestions.ForEach(a => { a.OrganizationID = requestMasterOrganizationObj.Id; a.Id = 0; /*set 0 for new */ });
            con.SecurityQuestions.AddRange(securityQuestions);
            con.SaveChanges();
        }

        private void CopiedMasterAppConfigurations(MasterOrganization requestMasterOrganizationObj, HCOrganizationContext con)
        {
            List<MasterAppConfiguration> masterAppConfigSettings = _context.MasterAppConfiguration.ToList();
            List<AppConfigurations> appConfigSettings = new List<AppConfigurations>();
            AutoMapper.Mapper.Map(masterAppConfigSettings, appConfigSettings);
            appConfigSettings.ForEach(a => { a.OrganizationID = requestMasterOrganizationObj.Id; a.Id = 0; /*set 0 for new */ });
            con.AppConfigurations.AddRange(appConfigSettings);
            con.SaveChanges();
        }

        private void SaveRoles(MasterOrganization requestOrganizationObj, List<UserRoles> requestUserRoles, HCOrganizationContext con)
        {
            foreach (var item in _organizationRoles.Split(','))
            {
                UserRoles userRoles = new UserRoles();
                userRoles.UserType = item.ToUpper();
                userRoles.RoleName = item;
                userRoles.IsActive = true;
                userRoles.IsDeleted = false;
                userRoles.OrganizationID = requestOrganizationObj.Id;
                requestUserRoles.Add(userRoles);
            }
            con.UserRoles.AddRange(requestUserRoles.ToArray());
            con.SaveChanges();
        }

        private void SaveDefaultStaffLocation(Staffs requestStaff, Location requestLocation, HCOrganizationContext con)
        {
            StaffLocation staffLocation = new StaffLocation();
            staffLocation.IsDefault = true; //set default
            staffLocation.LocationID = requestLocation.Id; // location ID
            staffLocation.StaffId = requestStaff.Id;//staff ID
            staffLocation.OrganizationID = requestStaff.OrganizationID;//organizationID
            con.StaffLocation.Add(staffLocation);
            con.SaveChanges();
        }

        private void SaveUser(OrganizationModel organizationModel, TokenModel token, MasterOrganization requestOrganizationObj, Entity.User requestUser, UserRepository con, HCOrganizationContext orgCon)
        {
            if (organizationModel.Id != 0)
            {
                requestUser.UserName = organizationModel.UserName;
                requestUser.Password = organizationModel.Password;
                requestUser.OrganizationID = requestOrganizationObj.Id;
                requestUser.PasswordResetDate = DateTime.UtcNow;
                con.Update(requestUser);
                con.SaveChanges();
            }
            else
            {
                requestUser.UserName = organizationModel.UserName;
                requestUser.Password = organizationModel.Password;
                requestUser.PasswordResetDate = DateTime.UtcNow;
                //please don't change _organizationRoles[0] its specifically used for admin role for same organization
                requestUser.RoleID = orgCon.UserRoles.Where(a => a.RoleName == OrganizationRoles.Admin.ToString() && a.OrganizationID == requestOrganizationObj.Id).FirstOrDefault().Id; //1; // TO DO it will be dynamic
                requestUser.CreatedBy = null;// token.UserID;
                requestUser.CreatedDate = DateTime.UtcNow;
                requestUser.IsActive = true;
                requestUser.IsDeleted = false;
                requestUser.OrganizationID = requestOrganizationObj.Id;
                con.Create(requestUser);
                con.SaveChanges();
            }
        }

        private void SaveStaff(OrganizationModel organizationModel, TokenModel token, MasterOrganization requestOrganizationObj, Entity.User requestUser, Staffs requestStaff, StaffRepository con, HCOrganizationContext orgCon)
        {
            if (organizationModel.Id > 0)
            {
                requestStaff.Email = organizationModel.Email;
                requestStaff.FirstName = organizationModel.ContactPersonFirstName;
                requestStaff.LastName = organizationModel.ContactPersonLastName;
                requestStaff.MiddleName = organizationModel.ContactPersonMiddleName;
                requestStaff.OrganizationID = requestOrganizationObj.Id;
                //requestStaff.UserName = organizationModel.UserName;
                //requestStaff.Password = organizationModel.Password;
                requestStaff.City = organizationModel.City;
                requestStaff.CountryID = organizationModel.CountryID;
                requestStaff.StateID = organizationModel.StateID;
                requestStaff.Zip = organizationModel.Zip;
                requestStaff.PhoneNumber = organizationModel.ContactPersonPhoneNumber;
                requestStaff.Address = organizationModel.Address1;
                con.Update(requestStaff);
                con.SaveChanges();
            }
            else
            {
                requestStaff.CreatedBy = null;// token.UserID;
                requestStaff.CreatedDate = DateTime.UtcNow;
                requestStaff.IsActive = true;
                requestStaff.IsDeleted = false;
                requestStaff.DOB = DateTime.UtcNow; //TO DO DOB should be from front end 
                requestStaff.Email = organizationModel.Email;
                requestStaff.FirstName = organizationModel.ContactPersonFirstName;
                requestStaff.LastName = organizationModel.ContactPersonLastName;
                requestStaff.MiddleName = organizationModel.ContactPersonMiddleName;
                requestStaff.OrganizationID = requestOrganizationObj.Id;
                requestStaff.UserID = requestUser.Id;
                requestStaff.DOJ = DateTime.UtcNow; // TO Need to disscus
                requestStaff.UserName = organizationModel.UserName;
                requestStaff.Password = organizationModel.Password;
                //please don't change its specifically used for admin role from same organization
                requestStaff.RoleID = orgCon.UserRoles.Where(a => a.RoleName == OrganizationRoles.Admin.ToString() && a.OrganizationID == requestOrganizationObj.Id).FirstOrDefault().Id; //1; // TO DO it will be dynamic                
                requestStaff.City = organizationModel.City;
                requestStaff.CountryID = organizationModel.CountryID;
                requestStaff.StateID = organizationModel.StateID;
                requestStaff.Zip = organizationModel.Zip;
                requestStaff.PhoneNumber = organizationModel.ContactPersonPhoneNumber;
                requestStaff.Address = organizationModel.Address1;
                requestStaff.Gender = null;// organizationModel.ContactPersonGender > 0 ? organizationModel.ContactPersonGender : 1;
                requestStaff.MaritalStatus = null;// organizationModel.ContactPersonMaritalStatus > 0 ? organizationModel.ContactPersonMaritalStatus : 1;
                con.Create(requestStaff);
                con.SaveChanges();
            }
        }

        private void SaveLocation(OrganizationModel organizationModel, TokenModel token, MasterOrganization requestOrganizationObj, Location requestLocation, LocationRepository con)
        {
            requestLocation.CreatedBy = null;// token.UserID;
            requestLocation.CreatedDate = DateTime.UtcNow;
            requestLocation.IsActive = true;
            requestLocation.IsDeleted = false;
            requestLocation.Address = organizationModel.Address1;
            requestLocation.BillingNPINumber = 0;//dummy data
            requestLocation.BillingTaxId = 0;//dummy data
            requestLocation.City = organizationModel.City;
            requestLocation.Code = null;//dummy data
            requestLocation.CountryID = organizationModel.CountryID;
            requestLocation.FacilityCode = null;// 12;//dummy data
            requestLocation.FacilityNPINumber = 0;//dummy data
            requestLocation.FacilityProviderNumber = 0;//dummy data
            requestLocation.LocationDescription = organizationModel.Description;
            requestLocation.LocationName = organizationModel.OrganizationName;//dummy data
            requestLocation.OfficeEndHour = null;//dummy data
            requestLocation.OfficeStartHour = null;//dummy data
            requestLocation.OrganizationID = requestOrganizationObj.Id;
            requestLocation.Phone = organizationModel.Phone;
            requestLocation.StateID = organizationModel.StateID;
            requestLocation.Zip = organizationModel.Zip;
            requestLocation.Latitude = organizationModel.Latitude;
            requestLocation.Longitude = organizationModel.Longitude;
            requestLocation.ApartmentNumber = organizationModel.ApartmentNumber;
            con.Create(requestLocation);
            con.SaveChanges();
        }

        private string GetDomain(int databaseID)
        {
            string con = string.Empty;
            if (databaseID > 0)
            {
                //get the db credentials for new connection
                OrganizationDatabaseDetail orgData = _context.OrganizationDatabaseDetail.Where(a => a.Id == databaseID).FirstOrDefault();

                //initialize Domain token model to create new connection string
                DomainToken domainData = new DomainToken();
                domainData.ServerName = orgData.ServerName;
                domainData.DatabaseName = orgData.DatabaseName;
                domainData.Password = orgData.Password;
                domainData.UserName = orgData.UserName;
                con = ConnectionString(domainData);
            }
            //return new connection string
            return con;
        }
        /// <summary>
        /// create dynamically new connection string
        /// </summary>
        /// <param name="domainToken"></param>
        /// <returns></returns>
        private string ConnectionString(DomainToken domainToken)
        {
            string conn = @"Server=" + domainToken.ServerName + ";Database=" + domainToken.DatabaseName + ";Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=false;User ID=" + domainToken.UserName + ";Password=" + domainToken.Password + ";";
            return conn;
        }
        #endregion


        #region Organization DB
        public JsonModel GetOrganizationDatabaseDetails(string databaseName, string organizationName, int organizationID, string sortColumn, string sortOrder, int pageNumber, int pageSize)
        {
            var organizationDatabaseDetails = _organizationDatabaseRepository.GetOrganizationDatabaseDetails(databaseName, organizationName, organizationID, sortColumn, sortOrder, pageNumber, pageSize);
            return new JsonModel()
            {
                meta = new Meta()
                {
                    TotalRecords = organizationDatabaseDetails != null && Convert.ToDecimal(organizationDatabaseDetails.Count) > 0 ? Convert.ToDecimal(organizationDatabaseDetails[0].TotalRecords) : 0,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    DefaultPageSize = pageSize,
                    TotalPages = Math.Ceiling(Convert.ToDecimal((organizationDatabaseDetails != null && organizationDatabaseDetails.Count > 0 ? organizationDatabaseDetails[0].TotalRecords : 0) / pageSize))
                },
                data = organizationDatabaseDetails,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK//Success
            };
        }

        public JsonModel SaveOrganizationDatabaseDetail(OrganizationDatabaseDetail organizationDatabaseDetail)
        {
            var organizationDatabaseDetails = _organizationDatabaseRepository.SaveOrganizationDatabaseDetail(organizationDatabaseDetail);
            return new JsonModel()
            {
                data = organizationDatabaseDetails,
                Message = StatusMessage.APISavedSuccessfully.Replace("[controller]", "Organization database details"),
                StatusCode = (int)HttpStatusCodes.OK//Success
            };
        }

        public JsonModel UpdateOrganizationDatabaseDetail(int id, OrganizationDatabaseDetail organizationDatabaseDetail)
        {
            var organizationDatabaseDetails = _organizationDatabaseRepository.UpdateOrganizationDatabaseDetail(id, organizationDatabaseDetail);
            return new JsonModel()
            {
                data = organizationDatabaseDetails,
                Message = StatusMessage.APIUpdatedSuccessfully.Replace("[controller]", "Organization database details"),
                StatusCode = (int)HttpStatusCodes.OK//Success
            };
        }

        public JsonModel DeleteOrganizationDatabaseDetail(int id, int userID)
        {
            var organizationDatabaseDetails = _organizationDatabaseRepository.DeleteOrganizationDatabaseDetail(id, userID);
            return new JsonModel()
            {
                data = organizationDatabaseDetails,
                Message = StatusMessage.DeletedSuccessfully.Replace("[controller]", "Organization database details"),
                StatusCode = (int)HttpStatusCodes.NotFound//Success
            };
        }

        #endregion



    }
}
