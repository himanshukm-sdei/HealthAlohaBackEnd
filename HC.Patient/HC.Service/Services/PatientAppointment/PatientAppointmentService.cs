using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.AppointmentTypes;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Patient;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Repositories.IRepositories.Appointment;
using HC.Patient.Repositories.IRepositories.Locations;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Patient.Service.IServices.PatientAppointment;
using HC.Service;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.PatientApp
{
    public class PatientAppointmentService : BaseService, IPatientAppointmentService
    {
        #region Global Variables
        private readonly IPatientAppointmentRepository _patientAppointmentRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentStaffRepository _appointmentStaffRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IPatientAuthorizationProcedureCPTLinkRepository _patientAuthorizationProcedureCPTLinkRepository;
        private readonly IAppointmentTypeRepository _appointmentTypeRepository;
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
        private HCOrganizationContext _context;
        private readonly IPatientRepository _patientRepository;
        private readonly IAppointmentAuthorizationRepository _appointmentAuthorizationRepository;
        private readonly IGlobalCodeService _globalCodeService;
        private readonly ILocationRepository _locationRepository;
        #endregion
        public PatientAppointmentService(IPatientAppointmentRepository patientAppointmentRepository, IAppointmentRepository appointmentRepository, IAppointmentStaffRepository appointmentStaffRepository, IStaffRepository staffRepository, IPatientRepository patientRepository, IPatientAuthorizationProcedureCPTLinkRepository patientAuthorizationProcedureCPTLinkRepository, IAppointmentAuthorizationRepository appointmentAuthorizationRepository, IAppointmentTypeRepository appointmentTypeRepository, HCOrganizationContext context, IGlobalCodeService globalCodeService, ILocationRepository locationRepository)
        {
            _patientAppointmentRepository = patientAppointmentRepository;
            _appointmentRepository = appointmentRepository;
            _appointmentStaffRepository = appointmentStaffRepository;
            _staffRepository = staffRepository;
            _patientAuthorizationProcedureCPTLinkRepository = patientAuthorizationProcedureCPTLinkRepository;
            _context = context;
            _patientRepository = patientRepository;
            _appointmentTypeRepository = appointmentTypeRepository;
            _appointmentAuthorizationRepository = appointmentAuthorizationRepository;
            _globalCodeService = globalCodeService;
            _locationRepository = locationRepository;
        }
        public List<PatientAppointmentsModel> UpdatePatientAppointment(PatientAppointmentFilter patientAppointmentFilter)
        {
            return _patientAppointmentRepository.UpdatePatientAppointment(patientAppointmentFilter);
        }

        public List<StaffAvailabilityModel> GetStaffAvailability(string StaffID, DateTime FromDate, DateTime ToDate, TokenModel token)
        {
            List<StaffAvailabilityModel> availability = _patientAppointmentRepository.GetStaffAvailability(StaffID, FromDate, ToDate);
            if (availability != null && availability.Count > 0)
            {
                availability.ForEach(x => { x.StartDateTime = CommonMethods.ConvertFromUtcTime(Convert.ToDateTime(x.StartDateTime), token); x.EndDateTime = CommonMethods.ConvertFromUtcTime(Convert.ToDateTime(x.EndDateTime), token); });
            }
            return availability;
        }

        public JsonModel GetPatientAppointmentList(string locationIds, string staffIds, string patientIds, DateTime? fromDate, DateTime? toDate, string patientTags, string staffTags, TokenModel token)
        {
            try
            {
                List<PatientAppointmentModel> list = new List<PatientAppointmentModel>();
                if (!string.IsNullOrEmpty(locationIds) && (!string.IsNullOrEmpty(staffIds) || !string.IsNullOrEmpty(patientIds)))
                {
                    list = _appointmentRepository.GetAppointmentList<PatientAppointmentModel>(locationIds, staffIds, patientIds, fromDate, toDate, patientTags, staffTags, token.OrganizationID).ToList();
                    list.ForEach(x =>
                    {
                        LocationModel locationModal = GetLocationOffsets(x.ServiceLocationID);

                        x.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);
                        x.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);

                        if (!string.IsNullOrEmpty(x.PatientPhotoThumbnailPath)) { x.PatientPhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, x.PatientPhotoThumbnailPath); }
                        x.AppointmentStaffs = !string.IsNullOrEmpty(x.XmlString) ? XDocument.Parse(x.XmlString).Descendants("Child").Select(y => new AppointmentStaffs()
                        {
                            StaffId = Convert.ToInt32(y.Element("StaffId").Value),
                            StaffName = y.Element("StaffName").Value,
                        }).ToList() : new List<AppointmentStaffs>(); x.XmlString = null;
                    });
                }
                return response = new JsonModel()
                {
                    data = list,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }

        public JsonModel SaveAppointment(PatientAppointmentModel patientAppointmentModel, List<PatientAppointmentModel> patientAppointmentList, bool isAdmin, TokenModel tokenModel)
        {
            ///Sunny Bhardwaj - Move the whole code for this function to sql queries later as it will slow down the process in bulk data
            int[] aptIds = { };  //This has been added to remove transaction as it was giving conflicts in context and dbcommand transaction
            string action = "";//add,update
            PatientAppointment patientAppointment = null;
            List<PatientAppointment> patientAptList = null;
            List<AppointmentStaff> appointmentStaffList = new List<AppointmentStaff>();
            AppointmentStaff appointmentStaffs = null;
            List<AppointmentStaff> appointmentStaffNewList = new List<AppointmentStaff>();
            List<AppointmentAuthModel> list = null;
            //List<AppointmentAuthModel> updateList = null;
            bool isDelete = false;

            #region  Check Authorization whether insert case or update case

            if (_patientRepository.CheckAuthorizationSetting() == true && patientAppointmentModel.PatientID != null && patientAppointmentModel.PatientID > 0)
            {
                if (patientAppointmentModel.PatientAppointmentId == 0)
                {

                    list = _patientRepository.GetAuthDataForPatientAppointment<AppointmentAuthModel>((int)patientAppointmentModel.PatientID, patientAppointmentModel.AppointmentTypeID, patientAppointmentModel.StartDateTime, patientAppointmentModel.EndDateTime, InsurancePlanType.Primary.ToString(), patientAppointmentModel.PatientAppointmentId, isAdmin, patientAppointmentModel.PatientInsuranceId, patientAppointmentModel.AuthorizationId).Where(x => x.AuthProcedureCPTLinkId != null && x.AuthProcedureCPTLinkId > 0).ToList();
                    if (list != null && list.Count > 0 && list.First().AuthorizationMessage.ToLower() != "valid")
                        return new JsonModel()
                        {
                            data = new object(),
                            Message = list.First().AuthorizationMessage,
                            StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                        };
                }
                else
                {
                    patientAppointment = _appointmentRepository.Get(x => x.Id == patientAppointmentModel.PatientAppointmentId);
                    if (patientAppointment != null)
                    {
                        if (patientAppointment.AppointmentTypeID != patientAppointmentModel.AppointmentTypeID || patientAppointment.EndDateTime.Subtract(patientAppointment.StartDateTime).TotalMinutes != patientAppointmentModel.EndDateTime.Subtract(patientAppointmentModel.StartDateTime).TotalMinutes)
                        {
                            list = _patientRepository.GetAuthDataForPatientAppointment<AppointmentAuthModel>((int)patientAppointmentModel.PatientID, patientAppointmentModel.AppointmentTypeID, patientAppointmentModel.StartDateTime, patientAppointmentModel.EndDateTime, InsurancePlanType.Primary.ToString(), patientAppointmentModel.PatientAppointmentId, isAdmin, patientAppointmentModel.PatientInsuranceId, patientAppointmentModel.AuthorizationId).Where(x => x.AuthProcedureCPTLinkId != null && x.AuthProcedureCPTLinkId > 0).ToList();
                            if (list != null && list.Count > 0 && list.First().AuthorizationMessage.ToLower() != "valid")
                                return new JsonModel()
                                {
                                    data = new object(),
                                    Message = list.First().AuthorizationMessage,
                                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                                };
                            else
                            {
                                isDelete = true;
                                //updateList = _patientRepository.GetAuthDataForPatientAppointment<AppointmentAuthModel>((int)patientAppointment.PatientID, patientAppointment.AppointmentTypeID, patientAppointment.StartDateTime, patientAppointment.EndDateTime, InsurancePlanType.Primary.ToString(), patientAppointmentModel.PatientAppointmentId).Where(x => x.AuthProcedureCPTLinkId != null && x.AuthProcedureCPTLinkId > 0).ToList();
                                patientAppointment = null;
                            }
                        }
                    }
                    //}
                }
            }
            #endregion

            //using (var transaction = _context.Database.BeginTransaction())
            //{
            try
            {
                if (patientAppointmentModel.PatientAppointmentId == 0)
                {
                    action = "add";
                    patientAppointment = new PatientAppointment();
                    AutoMapper.Mapper.Map(patientAppointmentModel, patientAppointment);
                    patientAppointment.OrganizationID = tokenModel.OrganizationID;
                    patientAppointment.IsActive = true;
                    patientAppointment.IsDeleted = false;
                    patientAppointment.CreatedBy = tokenModel.UserID;
                    patientAppointment.CreatedDate = DateTime.UtcNow;
                    patientAppointment.ParentAppointmentID = null;
                    patientAppointment.IsDirectService = patientAppointmentModel.IsDirectService;


                    patientAppointment.IsExcludedFromMileage = patientAppointmentModel.IsExcludedFromMileage;
                    patientAppointment.DriveTime = patientAppointmentModel.DriveTime;
                    patientAppointment.Mileage = patientAppointmentModel.Mileage;
                    patientAppointment.Offset = patientAppointmentModel.OffSet;
                    patientAppointment.StatusId = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.AppointmentStatus, AppointmentStatus.APPROVED, tokenModel);

                    patientAppointment.RecurrenceRule = !string.IsNullOrEmpty(patientAppointmentModel.RecurrenceRule) ? patientAppointmentModel.RecurrenceRule : null;
                    if (patientAppointmentList != null && patientAppointmentList.Count > 0)
                        patientAppointment.IsRecurrence = true;
                    else patientAppointment.IsRecurrence = false;
                    _appointmentRepository.Create(patientAppointment);
                    _appointmentRepository.SaveChanges();
                    aptIds.Append(patientAppointment.Id);
                    if (list != null && list.Count > 0)
                        UpdateScheduledUnits(tokenModel, list, "add", patientAppointment.Id);
                    appointmentStaffList = patientAppointmentModel.AppointmentStaffs.Select(x => new AppointmentStaff() { StaffID = x.StaffId, PatientAppointmentID = patientAppointment.Id, CreatedBy = tokenModel.UserID, CreatedDate = DateTime.UtcNow, IsActive = true, IsDeleted = false }).ToList();
                    _appointmentStaffRepository.Create(appointmentStaffList.ToArray());
                    _appointmentStaffRepository.SaveChanges();
                    bool isBillableService = _appointmentTypeRepository.GetByID(patientAppointment.AppointmentTypeID).IsBillAble;
                    if (!string.IsNullOrEmpty(patientAppointmentModel.RecurrenceRule) && patientAppointmentList != null && patientAppointmentList.Count > 0)
                    {
                        patientAptList = new List<PatientAppointment>();
                        AutoMapper.Mapper.Map(patientAppointmentList, patientAptList);
                        patientAptList.ForEach(x =>
                        {
                            if (isBillableService)
                                list = _patientRepository.GetAuthDataForPatientAppointment<AppointmentAuthModel>((int)x.PatientID, x.AppointmentTypeID, x.StartDateTime, x.EndDateTime, InsurancePlanType.Primary.ToString(), null, isAdmin, x.PatientInsuranceId, x.AuthorizationId).Where(a => a.AuthProcedureCPTLinkId != null && a.AuthProcedureCPTLinkId > 0).ToList();
                            if (isBillableService == false || (list != null && list.Count > 0 && list.First().AuthorizationMessage.ToLower() == "valid"))
                            {
                                x.OrganizationID = tokenModel.OrganizationID;
                                x.CreatedDate = DateTime.UtcNow;
                                x.IsDeleted = false;
                                x.IsActive = true;
                                x.ParentAppointmentID = patientAppointment.Id;
                                x.RecurrenceRule = null;
                                x.IsRecurrence = true;
                                x.StatusId = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.AppointmentStatus, AppointmentStatus.APPROVED, tokenModel);
                                _appointmentRepository.Create(x);
                                _appointmentRepository.SaveChanges();
                                aptIds.Append(x.Id);
                                appointmentStaffList = new List<AppointmentStaff>();
                                appointmentStaffList.AddRange(patientAppointmentModel.AppointmentStaffs.Select(a => new AppointmentStaff() { StaffID = a.StaffId, PatientAppointmentID = x.Id, CreatedBy = tokenModel.UserID, CreatedDate = DateTime.UtcNow, IsActive = true, IsDeleted = false }).ToList());
                                _appointmentStaffRepository.Create(appointmentStaffList.ToArray());
                                _appointmentStaffRepository.SaveChanges();
                                if (isBillableService)
                                    UpdateScheduledUnits(tokenModel, list, "add", x.Id);
                            }
                            else
                            {

                            }
                        });
                        //_appointmentRepository.Create(patientAptList.ToArray());
                        //_appointmentRepository.SaveChanges();
                        //appointmentStaffList = new List<AppointmentStaff>();
                        //foreach (PatientAppointment patientApt in patientAptList)
                        //{
                        //    appointmentStaffList.AddRange(patientAppointmentModel.AppointmentStaffs.Select(x => new AppointmentStaff() { StaffID = x.StaffId, PatientAppointmentID = patientApt.Id, CreatedBy = tokenModel.UserID, CreatedDate = DateTime.UtcNow, IsActive = true, IsDeleted = false }).ToList());
                        //}
                        //_appointmentStaffRepository.Create(appointmentStaffList.ToArray());
                        //_appointmentStaffRepository.SaveChanges();
                    }
                    response = new JsonModel()
                    {
                        Message = StatusMessage.AddAppointment,
                        StatusCode = (int)HttpStatusCodes.OK,
                        data = new object()
                    };
                }
                else
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            patientAppointment = _appointmentRepository.Get(x => x.Id == patientAppointmentModel.PatientAppointmentId && x.IsActive == true && x.IsDeleted == false);
                            if (!ReferenceEquals(patientAppointment, null))
                            {
                                action = "update";
                                patientAppointment.StartDateTime = patientAppointmentModel.StartDateTime;
                                patientAppointment.EndDateTime = patientAppointmentModel.EndDateTime;
                                patientAppointment.AppointmentTypeID = patientAppointmentModel.AppointmentTypeID;
                                patientAppointment.Notes = patientAppointmentModel.Notes;
                                patientAppointment.PatientAddressID = patientAppointmentModel.PatientAddressID;
                                patientAppointment.ServiceLocationID = patientAppointmentModel.ServiceLocationID;
                                patientAppointment.OfficeAddressID = patientAppointmentModel.OfficeAddressID;
                                patientAppointment.CustomAddress = patientAppointmentModel.CustomAddress;
                                patientAppointment.CustomAddressID = patientAppointmentModel.CustomAddressID;
                                patientAppointment.Longitude = patientAppointmentModel.Longitude;
                                patientAppointment.Latitude = patientAppointmentModel.Latitude;
                                patientAppointment.ApartmentNumber = patientAppointmentModel.ApartmentNumber;
                                patientAppointment.IsDirectService = patientAppointmentModel.IsDirectService;
                                patientAppointment.UpdatedBy = tokenModel.UserID;
                                patientAppointment.UpdatedDate = DateTime.UtcNow;
                                patientAppointment.IsTelehealthAppointment = patientAppointmentModel.IsTelehealthAppointment;
                                patientAppointment.Offset = patientAppointmentModel.OffSet;

                                patientAppointment.IsExcludedFromMileage = patientAppointmentModel.IsExcludedFromMileage;
                                patientAppointment.DriveTime = patientAppointmentModel.DriveTime;
                                patientAppointment.Mileage = patientAppointmentModel.Mileage;
                                patientAppointment.PatientInsuranceId = patientAppointmentModel.PatientInsuranceId;
                                patientAppointment.AuthorizationId = patientAppointmentModel.AuthorizationId;
                                _appointmentRepository.Update(patientAppointment);
                                _appointmentRepository.SaveChanges();

                                if (isDelete)
                                    UpdateScheduledUnits(tokenModel, null, "delete", patientAppointment.Id);
                                if (list != null && list.Count > 0)
                                    UpdateScheduledUnits(tokenModel, list, "add", patientAppointment.Id);
                                appointmentStaffList = _appointmentStaffRepository.GetAll(x => x.PatientAppointmentID == patientAppointmentModel.PatientAppointmentId && x.IsActive == true && x.IsDeleted == false).ToList();
                                foreach (AppointmentStaffs aptStaff in patientAppointmentModel.AppointmentStaffs)
                                {
                                    appointmentStaffs = appointmentStaffList.Find(x => x.PatientAppointmentID == patientAppointmentModel.PatientAppointmentId && x.StaffID == aptStaff.StaffId);
                                    if (appointmentStaffs != null)
                                    {
                                        if (aptStaff.IsDeleted == true)
                                        {
                                            appointmentStaffs.IsDeleted = aptStaff.IsDeleted;
                                            appointmentStaffs.DeletedBy = tokenModel.UserID;
                                            appointmentStaffs.DeletedDate = DateTime.UtcNow;
                                        }
                                    }
                                    else
                                    {
                                        appointmentStaffs = new AppointmentStaff();
                                        appointmentStaffs.PatientAppointmentID = patientAppointmentModel.PatientAppointmentId;
                                        appointmentStaffs.StaffID = aptStaff.StaffId;
                                        appointmentStaffs.CreatedBy = tokenModel.UserID;
                                        appointmentStaffs.CreatedDate = DateTime.UtcNow;
                                        appointmentStaffNewList.Add(appointmentStaffs);
                                    }
                                }
                                if (appointmentStaffList.FindAll(x => x.IsDeleted == true).Count > 0)
                                    _appointmentStaffRepository.Update(appointmentStaffList.ToArray());
                                if (appointmentStaffNewList != null && appointmentStaffNewList.Count > 0)
                                    _appointmentStaffRepository.Create(appointmentStaffNewList.ToArray());

                                _appointmentStaffRepository.SaveChanges();
                                response = new JsonModel() { Message = StatusMessage.UpdateAppointment };
                            }
                            else
                                response = new JsonModel() { Message = StatusMessage.AppointmentNotExists };

                            response.data = new object();
                            response.StatusCode = (int)HttpStatusCodes.OK;
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            return response = new JsonModel()
                            {
                                data = new object(),
                                Message = StatusMessage.ServerError,
                                StatusCode = (int)HttpStatusCodes.InternalServerError
                            };
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                //This code has been added on 25April to avoid trnasaction conflicts in Context and DbCommand.
                //It should be removed once that issue will be resolved
                if (action != "" && action == "add")
                {
                    List<AppointmentAuthorization> authList = _appointmentAuthorizationRepository.GetAll(x => aptIds.Contains(x.AppointmentId)).ToList();
                    List<AppointmentStaff> aptStaff = _appointmentStaffRepository.GetAll(x => aptIds.Contains(x.PatientAppointmentID)).ToList();
                    List<PatientAppointment> aptList = _appointmentRepository.GetAll(x => aptIds.Contains(x.Id)).ToList();
                    if (aptStaff != null && aptStaff.Count > 0)
                        _appointmentStaffRepository.Delete(aptStaff.ToArray());
                    if (authList != null && authList.Count > 0)
                        _appointmentAuthorizationRepository.Delete(authList.ToArray());
                    if (aptList != null && aptList.Count > 0)
                        _appointmentRepository.Delete(aptList.ToArray());
                    _appointmentRepository.SaveChanges();
                }
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
            //}
        }

        private void UpdateScheduledUnits(TokenModel tokenModel, List<AppointmentAuthModel> list, string action, int appointmentId)
        {
            List<AppointmentAuthorization> authList = new List<AppointmentAuthorization>();
            AppointmentAuthorization appointmentAuthorization = null;
            if (action == "add")
            {
                list.ForEach(x =>
                {
                    appointmentAuthorization = new AppointmentAuthorization();
                    appointmentAuthorization.AppointmentId = appointmentId;
                    appointmentAuthorization.AuthProcedureCPTLinkId = Convert.ToInt32(x.AuthProcedureCPTLinkId);
                    appointmentAuthorization.ServiceCodeId = x.ServiceCodeId;
                    appointmentAuthorization.UnitsBlocked = x.UnitToBlock;
                    appointmentAuthorization.AuthScheduledDate = _appointmentRepository.Get(a => a.Id == appointmentId).StartDateTime;
                    appointmentAuthorization.CreatedBy = tokenModel.UserID;
                    appointmentAuthorization.CreatedDate = DateTime.UtcNow;
                    appointmentAuthorization.IsActive = true;
                    appointmentAuthorization.IsDeleted = false;
                    appointmentAuthorization.IsBlocked = true;
                    authList.Add(appointmentAuthorization);
                });
                _appointmentAuthorizationRepository.Create(authList.ToArray());
            }
            else if (action == "delete")
            {
                authList = _appointmentAuthorizationRepository.GetAll(x => x.AppointmentId == appointmentId && x.IsActive == true && x.IsDeleted == false).ToList();
                if (authList.Count > 0)
                    authList.ForEach(x => { x.IsDeleted = true; x.DeletedBy = tokenModel.UserID; x.DeletedDate = DateTime.UtcNow; });
                _appointmentAuthorizationRepository.Update(authList.ToArray());
            }
            else if (action == "update")
            {
                authList = _appointmentAuthorizationRepository.GetAll(x => x.AppointmentId == appointmentId && x.IsActive == true && x.IsDeleted == false).ToList();
                if (authList.Count > 0)
                    authList.ForEach(x => { x.IsBlocked = true; x.UpdatedBy = tokenModel.UserID; x.UpdatedDate = DateTime.UtcNow; });
                _appointmentAuthorizationRepository.Update(authList.ToArray());
            }
            else if (action == "cancel")
            {
                authList = _appointmentAuthorizationRepository.GetAll(x => x.AppointmentId == appointmentId && x.IsActive == true && x.IsDeleted == false).ToList();
                if (authList.Count > 0)
                    authList.ForEach(x => { x.IsBlocked = false; x.UpdatedBy = tokenModel.UserID; x.UpdatedDate = DateTime.UtcNow; });
                _appointmentAuthorizationRepository.Update(authList.ToArray());
            }
            if (authList.Count > 0)
                _appointmentAuthorizationRepository.SaveChanges();
        }

        public JsonModel DeleteAppointment(int appointmentId, int? parentAppointmentId, bool deleteSeries, bool isAdmin, TokenModel token)
        {
            try
            {
                List<AppointmentAuthModel> list = null;
                PatientAppointment patientAppointment = null;
                List<PatientAppointment> patientAppointmentList = null;
                if (appointmentId > 0)
                {
                    SQLResponseModel sqlResponse = _appointmentRepository.DeleteAppointment<SQLResponseModel>(appointmentId, isAdmin, deleteSeries, token).FirstOrDefault();
                    return new JsonModel()
                    {
                        data = new object(),
                        StatusCode = sqlResponse.StatusCode,
                        Message = sqlResponse.Message
                    };
                }

                if (!deleteSeries)
                {
                    patientAppointment = _appointmentRepository.Get(x => x.Id == appointmentId && x.IsActive == true && x.IsDeleted == false);
                    if (!ReferenceEquals(patientAppointment, null))
                    {
                        //if (patientAppointment.PatientID != null && patientAppointment.PatientID > 0)
                        //    //list = _patientRepository.GetAuthDataForPatientAppointment<AppointmentAuthModel>((int)patientAppointment.PatientID, patientAppointment.AppointmentTypeID, patientAppointment.StartDateTime, patientAppointment.EndDateTime, InsurancePlanType.Primary.ToString()).Where(x => x.AuthProcedureCPTLinkId != null && x.AuthProcedureCPTLinkId > 0).ToList();
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                //if (list != null && list.Count > 0)
                                //if (!isAdmin)
                                //{
                                    //patientAppointment.IsDeleted = true;
                                    //patientAppointment.DeletedBy = token.UserID;
                                    //patientAppointment.DeletedDate = DateTime.UtcNow;
                               
                                    //_appointmentRepository.Update(patientAppointment);
                                    //_appointmentRepository.SaveChanges();
                                //}

                                UpdateScheduledUnits(token, list, "delete", appointmentId);  ///please change this for scheduled units
                                response = new JsonModel() { Message = StatusMessage.DeleteAppointment };
                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Rollback();
                                return response = new JsonModel()
                                {
                                    data = new object(),
                                    Message = StatusMessage.ServerError,
                                    StatusCode = (int)HttpStatusCodes.InternalServerError
                                };
                            }
                        }
                    }
                    else
                        response = new JsonModel() { Message = StatusMessage.AppointmentNotExists };
                }
                else
                {
                    if (parentAppointmentId != null)
                    {
                        //patientAppointmentList = _appointmentRepository.GetAll(x => x.ParentAppointmentID == parentAppointmentId && x.IsActive == true && x.IsDeleted == false && x.Id >= appointmentId).ToList();  //DateTime.ParseExact(x.StartDateTime.ToShortDateString(), "dd-MM-yyyy", null).CompareTo(DateTime.ParseExact(DateTime.UtcNow.ToShortDateString(), "dd-MM-yyyy", null))

                        PatientAppointment resPatientAppointment = _appointmentRepository.GetAll(x => x.Id == appointmentId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                        patientAppointmentList = _appointmentRepository.GetAll(x => x.ParentAppointmentID == parentAppointmentId && x.IsActive == true && x.IsDeleted == false && x.StartDateTime >= resPatientAppointment.StartDateTime).ToList();  
                    }
                    //DateTime.Compare
                    else
                    {
                        patientAppointmentList = _appointmentRepository.GetAll(x => x.Id == appointmentId && x.IsActive == true && x.IsDeleted == false).ToList();
                        patientAppointmentList.AddRange(_appointmentRepository.GetAll(x => x.ParentAppointmentID == appointmentId && x.IsActive == true && x.IsDeleted == false).ToList());
                    }
                    if (patientAppointmentList != null && patientAppointmentList.Count > 0)
                    {
                        patientAppointmentList.ForEach(x => { x.IsDeleted = true; x.DeletedBy = token.UserID; x.DeletedDate = DateTime.UtcNow; });
                        //if (!isAdmin)
                        //{
                        //    _appointmentRepository.Update(patientAppointmentList.ToArray());
                        //    _appointmentRepository.SaveChanges();
                        //}
                        patientAppointmentList.ForEach(x =>
                        {
                            UpdateScheduledUnits(token, list, "delete", x.Id);  ///please change this for scheduled units
                        });
                        response = new JsonModel() { Message = StatusMessage.DeleteAppointmentRecurrence };
                    }
                    else
                        response = new JsonModel() { Message = StatusMessage.AppointmentNotExists };
                }
                response.data = new object();
                response.StatusCode = (int)HttpStatusCodes.OK;
                return response;
            }
            catch (Exception)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel GetAppointmentDetails(int appointmentId, TokenModel token)
        {
            try
            {
                PatientAppointment pat = _appointmentRepository.GetByID(appointmentId);
                LocationModel locationModal = GetLocationOffsets(pat.ServiceLocationID);
                PatientAppointmentModel patientAppointmentModel = _appointmentRepository.GetAppointmentDetails<PatientAppointmentModel>(appointmentId).FirstOrDefault();

                patientAppointmentModel.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(patientAppointmentModel.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);
                patientAppointmentModel.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(patientAppointmentModel.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);

                patientAppointmentModel.AppointmentStaffs = !string.IsNullOrEmpty(patientAppointmentModel.XmlString) ? XDocument.Parse(patientAppointmentModel.XmlString).Descendants("Child").Select(y => new AppointmentStaffs()
                {
                    StaffId = Convert.ToInt32(y.Element("StaffId").Value),
                    StaffName = y.Element("StaffName").Value,
                }).ToList() : new List<AppointmentStaffs>(); patientAppointmentModel.XmlString = null;
                return response = new JsonModel()
                {
                    data = patientAppointmentModel,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError = ex.Message
                };
            }
        }

        public JsonModel GetStaffAndPatientByLocation(string locationIds, string permissionKey, string isActiveCheckRequired, TokenModel token)
        {
            try
            {
                StaffPatientModel staffLocation = new StaffPatientModel();
                if (!string.IsNullOrEmpty(locationIds))
                    staffLocation = _patientAppointmentRepository.GetStaffAndPatientByLocation(locationIds, permissionKey, isActiveCheckRequired, token);

                return response = new JsonModel()
                {
                    data = staffLocation,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {

                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
        public JsonModel GetStaffByLocation(string locationIds, string isActiveCheckRequired, TokenModel token)
        {
            List<StaffModel> staffslist = new List<StaffModel>();
            if (!string.IsNullOrEmpty(locationIds))
                staffslist = _patientAppointmentRepository.GetStaffByLocation<StaffModel>(locationIds, isActiveCheckRequired, token).ToList();

            return response = new JsonModel()
            {
                data = staffslist,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }


        public List<AvailabilityMessageModel> CheckIsValidAppointment(string staffIds, DateTime startDate, DateTime endDate, Nullable<DateTime> currentDate, Nullable<int> patientAppointmentId, Nullable<int> patientId, Nullable<int> appointmentTypeID, TokenModel token)
        {
            return _appointmentRepository.CheckIsValidAppointment<AvailabilityMessageModel>(staffIds, startDate, endDate, currentDate, patientAppointmentId, patientId, appointmentTypeID, token).ToList();
        }

        public List<AvailabilityMessageModel> CheckIsValidAppointmentWithLocation(string staffIds, DateTime startDate, DateTime endDate, Nullable<DateTime> currentDate, Nullable<int> patientAppointmentId, Nullable<int> patientId, Nullable<int> appointmentTypeID, decimal currentOffset, TokenModel token)
        {
            return _appointmentRepository.CheckIsValidAppointmentWithLocation<AvailabilityMessageModel>(staffIds, startDate, endDate, currentDate, patientAppointmentId, patientId, appointmentTypeID, currentOffset, token).ToList();
        }

        public JsonModel GetDataForSchedulerByPatient(Nullable<int> patientId, int locationId, DateTime startDate, DateTime endDate, Nullable<int> patientInsuranceId, TokenModel token)
        {
            Dictionary<string, object> schedulerData = new Dictionary<string, object>();
            try
            {
                LocationModel locationModal = GetLocationOffsets(locationId);
                schedulerData.Add("PatientPayerActivities", _patientRepository.GetActivitiesForPatientPayer<AppointmentTypeModel>(patientId, InsurancePlanType.Primary.ToString(), CommonMethods.ConvertToUtcTimeWithOffset(startDate, locationModal.DaylightOffset, locationModal.StandardOffset), CommonMethods.ConvertToUtcTimeWithOffset(endDate, locationModal.DaylightOffset, locationModal.StandardOffset), patientInsuranceId, token).ToList());
                schedulerData.Add("PatientAddresses", _patientRepository.GetPatientAddressList<PatientAddressModel>(patientId, locationId).ToList());
                return response = new JsonModel()
                {
                    data = schedulerData,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {

                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel UpdateServiceCodeBlockedUnit(int authProcedureCPTLinkId, string action, TokenModel token)
        {
            try
            {
                int value = action.ToLower() == "add" ? 1 : -1;
                AuthProcedureCPT authProcedureCPT = _patientAuthorizationProcedureCPTLinkRepository.Get(x => x.Id == authProcedureCPTLinkId && x.IsActive == true && x.IsDeleted == false);
                if (!ReferenceEquals(authProcedureCPT, null))
                {
                    authProcedureCPT.BlockedUnit = (authProcedureCPT.BlockedUnit != null && authProcedureCPT.BlockedUnit > 0) ? authProcedureCPT.BlockedUnit + value : authProcedureCPT.BlockedUnit;
                    authProcedureCPT.UpdatedBy = token.UserID;
                    authProcedureCPT.UpdatedDate = DateTime.UtcNow;
                    _patientAuthorizationProcedureCPTLinkRepository.Update(authProcedureCPT);
                    _patientAuthorizationProcedureCPTLinkRepository.SaveChanges();
                }
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel CancelAppointments(int[] appointmentIds, int CancelTypeId, string reson, TokenModel token)
        {
            try
            {
                int statusId = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.AppointmentStatus, AppointmentStatus.CANCEL, token);
                List<PatientAppointment> dbAppointments = _appointmentRepository.GetAll(a => appointmentIds.Contains(a.Id)).ToList();
                dbAppointments.ForEach(a => { a.CancelReason = reson; a.CancelTypeId = CancelTypeId; a.StatusId = statusId; a.UpdatedBy = token.UserID; a.UpdatedDate = DateTime.UtcNow; UpdateScheduledUnits(token, null, "cancel", a.Id); });
                _appointmentRepository.Update(dbAppointments.ToArray());
                _appointmentRepository.SaveChanges();
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.CancelAppointment,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel ActivateAppointments(int appointmentId, bool isAdmin, TokenModel token)
        {
            PatientAppointment patientAppointment = _appointmentRepository.GetByID(appointmentId);
            List<AppointmentAuthModel> list = _patientRepository.GetAuthDataForPatientAppointment<AppointmentAuthModel>((int)patientAppointment.PatientID, patientAppointment.AppointmentTypeID, patientAppointment.StartDateTime, patientAppointment.EndDateTime, InsurancePlanType.Primary.ToString(), appointmentId, isAdmin, patientAppointment.PatientInsuranceId, patientAppointment.AuthorizationId).Where(x => x.AuthProcedureCPTLinkId != null && x.AuthProcedureCPTLinkId > 0).ToList();
            if (list != null && list.Count > 0 && list.First().AuthorizationMessage.ToLower() != "valid")
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = list.First().AuthorizationMessage,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                };
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    PatientAppointment dbAppointments = _appointmentRepository.Get(a => a.Id == appointmentId);
                    dbAppointments.CancelReason = string.Empty;
                    dbAppointments.CancelTypeId = null;
                    dbAppointments.UpdatedBy = token.UserID;
                    dbAppointments.UpdatedDate = DateTime.UtcNow;
                    _appointmentRepository.Update(dbAppointments);
                    _appointmentRepository.SaveChanges();

                    UpdateScheduledUnits(token, null, "update", appointmentId);  ///please change this for scheduled units

                    transaction.Commit();

                    return response = new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.UndoCancelAppointment,
                        StatusCode = (int)HttpStatusCodes.OK
                    };

                }
                catch
                {
                    transaction.Rollback();
                    return response = new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.ServerError,
                        StatusCode = (int)HttpStatusCodes.InternalServerError
                    };
                }
            }
        }

        public JsonModel UpdateAppointmentStatus(AppointmentStatusModel appointmentStatusModel, TokenModel token)
        {
            response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, string.Empty);
            List<AppointmentAuthModel> list = null;
            PatientAppointment patientAppointment = null;
            patientAppointment = _appointmentRepository.Get(a => a.Id == appointmentStatusModel.Id);
            if (patientAppointment != null)
            {
                if (appointmentStatusModel.Status.ToLower() == AppointmentStatus.APPROVED.ToLower())
                {
                    list = _patientRepository.GetAuthDataForPatientAppointment<AppointmentAuthModel>((int)patientAppointment.PatientID, patientAppointment.AppointmentTypeID, patientAppointment.StartDateTime, patientAppointment.EndDateTime, InsurancePlanType.Primary.ToString(), patientAppointment.Id, false, null, null).Where(x => x.AuthProcedureCPTLinkId != null && x.AuthProcedureCPTLinkId > 0).ToList();
                    if (list != null && list.Count > 0 && list.First().AuthorizationMessage.ToLower() != "valid")
                    {
                        return new JsonModel(null, list.First().AuthorizationMessage, (int)HttpStatusCodes.UnprocessedEntity, string.Empty);
                    }
                }
                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        patientAppointment.StatusId = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.AppointmentStatus, appointmentStatusModel.Status, token);
                        patientAppointment.UpdatedBy = token.UserID;
                        patientAppointment.UpdatedDate = DateTime.UtcNow;
                        _appointmentRepository.Update(patientAppointment);
                        _appointmentRepository.SaveChanges();
                        if (appointmentStatusModel.Status.ToLower() == AppointmentStatus.APPROVED.ToLower())
                            UpdateScheduledUnits(token, list, "add", appointmentStatusModel.Id);
                        transaction.Commit();
                        response = new JsonModel(new object(), StatusMessage.UpdateAppointmentStatus, (int)HttpStatusCodes.OK);
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            return response;
        }

        public JsonModel SaveAppointmentFromPatientPortal(PatientAppointmentModel patientAppointmentModel, TokenModel token)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    PatientAppointment patientAppointment = null;
                    AppointmentStaff appointmentStaff = null;
                    LocationModel locationModal = GetLocationOffsets(patientAppointmentModel.ServiceLocationID);
                    if (patientAppointmentModel.PatientAppointmentId == 0)
                    {
                        patientAppointment = new PatientAppointment();
                        AutoMapper.Mapper.Map(patientAppointmentModel, patientAppointment);
                        patientAppointment.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientAppointment.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                        patientAppointment.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientAppointment.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                        patientAppointment.OrganizationID = token.OrganizationID;
                        patientAppointment.IsActive = true;
                        patientAppointment.CreatedBy = token.UserID;
                        patientAppointment.CreatedDate = DateTime.UtcNow;
                        patientAppointment.Offset = (int)CommonMethods.GetCurrentOffset(patientAppointment.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                        patientAppointment.StatusId = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.AppointmentStatus, AppointmentStatus.PENDING, token);
                        patientAppointment.IsClientRequired = true;
                        _appointmentRepository.Create(patientAppointment);
                        _appointmentRepository.SaveChanges();

                        appointmentStaff = patientAppointmentModel.AppointmentStaffs.Select(x => new AppointmentStaff() { StaffID = x.StaffId, PatientAppointmentID = patientAppointment.Id, CreatedBy = token.UserID, CreatedDate = DateTime.UtcNow, IsActive = true }).FirstOrDefault();
                        _appointmentStaffRepository.Create(appointmentStaff);
                        _appointmentStaffRepository.SaveChanges();

                        response = new JsonModel(null, StatusMessage.AddPatientAppointment, (int)HttpStatusCodes.OK, string.Empty);
                    }
                    else
                    {
                        patientAppointment = _appointmentRepository.Get(x => x.Id == patientAppointmentModel.PatientAppointmentId && x.IsActive == true && x.IsDeleted == false);
                        if (!ReferenceEquals(patientAppointment, null))
                        {
                            patientAppointment.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientAppointmentModel.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                            patientAppointment.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientAppointmentModel.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                            patientAppointment.AppointmentTypeID = patientAppointmentModel.AppointmentTypeID;
                            patientAppointment.Notes = patientAppointmentModel.Notes;
                            patientAppointment.PatientAddressID = patientAppointmentModel.PatientAddressID;
                            patientAppointment.ServiceLocationID = patientAppointmentModel.ServiceLocationID;
                            patientAppointment.OfficeAddressID = patientAppointmentModel.OfficeAddressID;
                            patientAppointment.CustomAddress = patientAppointmentModel.CustomAddress;
                            patientAppointment.CustomAddressID = patientAppointmentModel.CustomAddressID;
                            patientAppointment.Longitude = patientAppointmentModel.Longitude;
                            patientAppointment.Latitude = patientAppointmentModel.Latitude;
                            patientAppointment.ApartmentNumber = patientAppointmentModel.ApartmentNumber;
                            patientAppointment.IsDirectService = patientAppointmentModel.IsDirectService;
                            patientAppointment.IsClientRequired = true;
                            patientAppointment.UpdatedBy = token.UserID;
                            patientAppointment.UpdatedDate = DateTime.UtcNow;
                            patientAppointment.Offset = (int)CommonMethods.GetCurrentOffset(patientAppointmentModel.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                            _appointmentRepository.Update(patientAppointment);
                            _appointmentRepository.SaveChanges();

                            appointmentStaff = _appointmentStaffRepository.GetAll(x => x.PatientAppointmentID == patientAppointmentModel.PatientAppointmentId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                            if (appointmentStaff != null)
                            {
                                appointmentStaff.StaffID = patientAppointmentModel.AppointmentStaffs.FirstOrDefault().StaffId;
                                appointmentStaff.UpdatedBy = token.UserID;
                                appointmentStaff.UpdatedDate = DateTime.UtcNow;
                                _appointmentStaffRepository.Update(appointmentStaff);
                                _appointmentStaffRepository.SaveChanges();
                            }
                            response = new JsonModel() { Message = StatusMessage.UpdatePatientAppointment };
                        }

                        response.data = null;
                        response.StatusCode = (int)HttpStatusCodes.OK;
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response = new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, ex.Message);
                }
            }
            return response;
        }

        public JsonModel DeleteAppointment(int appointmentId, TokenModel token)
        {
            PatientAppointment patientAppointment = _appointmentRepository.Get(x => x.Id == appointmentId && x.IsActive == true && x.IsDeleted == false);
            if (!ReferenceEquals(patientAppointment, null))
            {
                patientAppointment.IsDeleted = true;
                patientAppointment.DeletedBy = token.UserID;
                patientAppointment.DeletedDate = DateTime.UtcNow;
                _appointmentRepository.Update(patientAppointment);
                _appointmentRepository.SaveChanges();
                response = new JsonModel(null, StatusMessage.DeletePatientAppointment, (int)HttpStatusCodes.OK, string.Empty);
            }
            return response;
        }

        public LocationModel GetLocationOffsets(int? locationId)
        {
            LocationModel locationModal = new LocationModel();
            Location location = _locationRepository.GetByID(locationId);
            if (location != null)
            {
                locationModal.DaylightOffset = (((decimal)location.DaylightSavingTime) * 60);
                locationModal.StandardOffset = (((decimal)location.StandardTime) * 60);
            }

            return locationModal;
        }
    }
}