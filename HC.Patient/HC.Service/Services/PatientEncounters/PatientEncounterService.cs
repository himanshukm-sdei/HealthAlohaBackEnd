using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.PatientEncounters;
using HC.Patient.Repositories.IRepositories.Appointment;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.PatientEncounters;
using HC.Patient.Service.IServices.PatientEncounters;
using System;
using System.Collections.Generic;
using System.Linq;
using HC.Common.Enums;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Common.HC.Common;
using HC.Patient.Model.Payer;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Service.IServices.Images;
using HC.Common;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Xml.Linq;
using System.Reflection;
using HC.Patient.Repositories.IRepositories.Staff;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Net;
using Newtonsoft.Json;
using HC.Patient.Model.Staff;
using HC.Service;
using Microsoft.AspNetCore.Mvc;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories.Locations;

namespace HC.Patient.Service.Services.PatientEncounters
{
    public class PatientEncounterService : BaseService, IPatientEncounterService
    {
        private HCOrganizationContext _context;
        private IPatientEncounterRepository _patientEncounterRepository;
        private ISoapNoteRepository _soapNoteRepository;
        private IPatientEncounterServiceCodesRepository _patientEncounterServiceCodesRepository;
        private IPatientEncounterICDCodesRepository _patientEncounterICDCodesRepository;
        private IPatientEncounterCodesMappingsRepository _patientEncounterCodesMappingsRepository;
        private IAppointmentRepository _appointmentRepository;
        private IPatientRepository _patientRepository;
        private IAuditLogRepository _auditLogRepository;
        private IPatientAuthorizationProcedureCPTLinkRepository _patientAuthorizationProcedureCPTLinkRepository;
        private IUserTimesheetRepository _userTimesheetRepository;
        private IUserTimesheetByAppointmentTypeRepository _userTimesheetByAppointmentTypeRepository;
        private IUserDriveTimeRepository _userDriveTimeRepository;
        private IUserDetailedDriveTimeRepository _userDetailedDriveTimeRepository;
        private IImageService _imageService;
        private readonly ILocationRepository _locationRepository;
        private IPatientEncounterTemplateRepository _patientEncounterTemplateRepository;
        private Nullable<int> pId;        
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public PatientEncounterService(HCOrganizationContext context, IPatientEncounterRepository patientEncounterRepository, ISoapNoteRepository soapNoteRepository,
            IPatientEncounterServiceCodesRepository patientEncounterServiceCodesRepository, IPatientEncounterICDCodesRepository patientEncounterICDCodesRepository,
            IPatientEncounterCodesMappingsRepository patientEncounterCodesMappingsRepository, IPatientRepository patientRepository, IAppointmentRepository appointmentRepository
            , IAuditLogRepository auditLogRepository, IPatientAuthorizationProcedureCPTLinkRepository patientAuthorizationProcedureCPTLinkRepository, IImageService imageService, IUserTimesheetRepository userTimesheetRepository, IUserTimesheetByAppointmentTypeRepository userTimesheetByAppointmentTypeRepository, IUserDriveTimeRepository userDriveTimeRepository, IUserDetailedDriveTimeRepository userDetailedDriveTimeRepository, ILocationRepository locationRepository,
            IPatientEncounterTemplateRepository patientEncounterTemplateRepository)
        {
            _context = context;
            _patientEncounterRepository = patientEncounterRepository;
            _soapNoteRepository = soapNoteRepository;
            _patientEncounterServiceCodesRepository = patientEncounterServiceCodesRepository;
            _patientEncounterICDCodesRepository = patientEncounterICDCodesRepository;
            _patientEncounterCodesMappingsRepository = patientEncounterCodesMappingsRepository;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _auditLogRepository = auditLogRepository;
            _patientAuthorizationProcedureCPTLinkRepository = patientAuthorizationProcedureCPTLinkRepository;
            _imageService = imageService;
            _userTimesheetRepository = userTimesheetRepository;
            _userTimesheetByAppointmentTypeRepository = userTimesheetByAppointmentTypeRepository;
            _userDriveTimeRepository = userDriveTimeRepository;
            _userDetailedDriveTimeRepository = userDetailedDriveTimeRepository;
            _locationRepository = locationRepository;
            _patientEncounterTemplateRepository = patientEncounterTemplateRepository;
        }

        public JsonModel GetPatientNonBillableEncounterDetails(int appointmentId, int encounterId, bool isAdmin, TokenModel token)
        {
            PatientEncounterModel response = new PatientEncounterModel();
            PatientAppointment pat = new PatientAppointment();
            LocationModel locationModal = new LocationModel();

            if (appointmentId > 0)
            {
                pat = _appointmentRepository.GetByID(appointmentId);
                locationModal = GetLocationOffsets(pat.ServiceLocationID);
            }
            else
            {
                PatientEncounter pe = _patientEncounterRepository.GetByID(encounterId);
                locationModal = GetLocationOffsets(pe.ServiceLocationID);
            }
            
            //PatientAppointment pat = _appointmentRepository.GetByID(appointmentId);
            //LocationModel locationModal = GetLocationOffsets(pat.ServiceLocationID);
            if (isAdmin && encounterId > 0 && appointmentId > 0)
            {
                response = _patientEncounterRepository.GetPatientEncounterDetails(encounterId, false);
                GetAppointmentDetail(locationModal, response, appointmentId, token);
            }
            else if (encounterId > 0)
            {
                response = _patientEncounterRepository.GetPatientEncounterDetails(encounterId, false);
                GetAppointmentDetail(locationModal, response, appointmentId, token);
            }
            else
            {
                if (appointmentId > 0)
                    GetAppointmentDetail(locationModal, response, appointmentId, token);
            }

            if (response != null && response.StartDateTime != null && response.EndDateTime != null)
            {
                response.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);
                response.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);
            }

            return new JsonModel()
            {
                data = response,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
            };
        }

        private void GetAppointmentDetail(LocationModel locationModal, PatientEncounterModel response, int appointmentId, TokenModel token)
        {
            response.PatientAppointment = _appointmentRepository.GetAppointmentDetails<PatientAppointmentModel>(appointmentId).FirstOrDefault();
            if (response.PatientAppointment != null)
            {
                response.PatientAppointment.AppointmentStaffs = !string.IsNullOrEmpty(response.PatientAppointment.XmlString) ? XDocument.Parse(response.PatientAppointment.XmlString).Descendants("Child").Select(y => new AppointmentStaffs()
                {
                    StaffId = Convert.ToInt32(y.Element("StaffId").Value),
                    StaffName = y.Element("StaffName").Value,
                }).ToList() : new List<AppointmentStaffs>(); response.PatientAppointment.XmlString = null;
                response.PatientAppointment.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.PatientAppointment.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);
                response.PatientAppointment.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.PatientAppointment.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);
            }
        }

        public JsonModel GetPatientEncounterDetails(int appointmentId, int encounterId, bool isAdmin, TokenModel token)
        {
            PatientEncounterModel response = new PatientEncounterModel();
            PatientAppointment pat = new PatientAppointment();
            LocationModel locationModal = new LocationModel();

            bool isAuthMandatory = _patientRepository.CheckAuthorizationSetting();

            if (appointmentId > 0)
            {
                pat = _appointmentRepository.GetByID(appointmentId);
                locationModal = GetLocationOffsets(pat.ServiceLocationID);
            }
            else
            {
                PatientEncounter pe = _patientEncounterRepository.GetByID(encounterId);
                locationModal = GetLocationOffsets(pe.ServiceLocationID);
            }

            if (isAdmin && encounterId > 0 && appointmentId > 0)
            {
                response = _patientEncounterRepository.GetPatientEncounterDetails(encounterId, true);
                response.PatientEncounterServiceCodes = _patientEncounterRepository.GetServiceCodeForEncounterByAppointmentType<PatientEncounterServiceCodesModel>(appointmentId).ToList();
            }
            else if (encounterId > 0)
                response = _patientEncounterRepository.GetPatientEncounterDetails(encounterId, true);

            else
            {
                if (appointmentId > 0)
                {
                    response.PatientEncounterServiceCodes = _patientEncounterRepository.GetServiceCodeForEncounterByAppointmentType<PatientEncounterServiceCodesModel>(appointmentId).ToList();
                    int? patientId = pat.PatientID;// _appointmentRepository.GetByID(appointmentId).PatientID;
                    if (patientId != null && patientId > 0)
                        response.PatientEncounterDiagnosisCodes = _patientRepository.GetPatientDiagnosisCodes<PatientEncounterDiagnosisCodesModel>(Convert.ToInt32(patientId)).ToList();
                }
            }
            if (response != null && response.PatientEncounterServiceCodes != null && response.PatientEncounterServiceCodes.Count > 0)//Added in the child as front end team may need to chnage json
            {
                if (appointmentId > 0)
                    GetAppointmentDetail(locationModal, response, appointmentId, token);
                response.PatientEncounterServiceCodes.Select(x => { x.IsAuthorizationMandatory = isAuthMandatory; return x; }).ToList();
                if (response.StartDateTime != null && response.EndDateTime != null)
                {
                    response.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);
                    response.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(response.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);
                }
            }
            return new JsonModel()
            {
                data = response,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
            };
        }
        public JsonModel SavePatientNonBillableEncounter(PatientEncounterModel requestObj, bool isAdmin, TokenModel token)
        {
            pId = requestObj.PatientID;
            PatientEncounter patientEncounter = null;
            int? appointmentTypeId = _appointmentRepository.GetByID(requestObj.PatientAppointmentId).AppointmentTypeID;
            LocationModel locationModal = GetLocationOffsets(requestObj.ServiceLocationID);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (requestObj.Id == 0)
                    {   
                        patientEncounter = new PatientEncounter();
                        AutoMapper.Mapper.Map(requestObj, patientEncounter);
                        patientEncounter.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientEncounter.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                        patientEncounter.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientEncounter.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                        patientEncounter.CreatedBy = token.UserID;
                        patientEncounter.OrganizationID = token.OrganizationID;
                        patientEncounter.CreatedDate = DateTime.UtcNow;
                        patientEncounter.IsActive = true;
                        patientEncounter.IsDeleted = false;
                        patientEncounter.IsBillableEncounter = Convert.ToBoolean(requestObj.IsBillableEncounter);
                        patientEncounter.NonBillableNotes = requestObj.NonBillableNotes;
                        //mark encounter rendered
                        patientEncounter.Status = _context.GlobalCode.Where(a => a.GlobalCodeName.ToUpper() == "RENDERED" && a.OrganizationID == token.OrganizationID).FirstOrDefault().Id;
                        //
                        _patientEncounterRepository.Create(patientEncounter);
                        _patientEncounterRepository.SaveChanges();

                        requestObj.EncounterSignature.ForEach(a => { a.PatientEncounterId = patientEncounter.Id; });
                        SaveSignature(requestObj.EncounterSignature);

                    }
                    else
                    {
                        patientEncounter = _patientEncounterRepository.Get(x => x.Id == requestObj.Id && x.IsActive == true && x.IsDeleted == false);
                        if (!ReferenceEquals(patientEncounter, null))
                        {
                            //Update patient encounter
                            patientEncounter.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(requestObj.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                            patientEncounter.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(requestObj.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                            //patientEncounter.ClinicianSign = requestObj.ClinicianSign;
                            //patientEncounter.ClinicianSignDate = requestObj.ClinicianSignDate;
                            patientEncounter.IsBillableEncounter = Convert.ToBoolean(requestObj.IsBillableEncounter);
                            patientEncounter.NonBillableNotes = requestObj.NonBillableNotes;
                            patientEncounter.AppointmentTypeId = appointmentTypeId;
                            patientEncounter.CustomAddressID = requestObj.CustomAddressID;
                            patientEncounter.CustomAddress = requestObj.CustomAddress;
                            patientEncounter.PatientAddressID = requestObj.PatientAddressID;
                            patientEncounter.OfficeAddressID = requestObj.OfficeAddressID;
                            patientEncounter.ServiceLocationID = requestObj.ServiceLocationID;
                            patientEncounter.StaffID = requestObj.StaffID;
                            //TODO
                            //_imageService.SaveImages(requestObj.ClinicianSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.ClinicianSign.ToString());
                            //_imageService.SaveImages(requestObj.GuardianSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.GuardianSign.ToString());
                            //_imageService.SaveImages(requestObj.PatientSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.PatientSign.ToString());


                            ////mark encounter rendered
                            //patientEncounter.Status = _context.GlobalCode.Where(a => a.GlobalCodeName.ToUpper() == "RENDERED" && a.OrganizationID == token.OrganizationID).FirstOrDefault().Id;
                            //
                            patientEncounter.UpdatedBy = token.UserID;
                            patientEncounter.UpdatedDate = DateTime.UtcNow;
                            _patientRepository.SaveChanges();
                            //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token);
                            SaveSignature(requestObj.EncounterSignature);
                        }
                    }
                    SaveTimesheetData(token, patientEncounter);
                    SaveDriveTimeData(token, patientEncounter);
                    transaction.Commit();
                    return new JsonModel()
                    {
                        data = patientEncounter,
                        Message = Common.HC.Common.StatusMessage.SoapSuccess,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.OK//Success
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = ex.Message,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                    };
                }
            }

        }
        public JsonModel SavePatientEncounter(PatientEncounterModel requestObj, bool isAdmin, TokenModel token)
        {
            pId = requestObj.PatientID;
            PatientEncounter patientEncounter = null;
            SoapNotes soapNote = null;
            int? appointmentTypeId = _appointmentRepository.GetByID(requestObj.PatientAppointmentId).AppointmentTypeID;
            PatientEncounterCodesMapping CodesMapping = null;
            List<PatientEncounterDiagnosisCodes> ICDCodesList = null;
            List<PatientEncounterServiceCodes> serviceCodesList = null;
            List<PatientEncounterCodesMapping> codesMappingList = null;
            List<PatientEncounterServiceCodes> serviceCodesInsertList = null;
            PatientEncounterServiceCodes serviceCodeObj = null;
            List<PatientEncounterDiagnosisCodes> ICDCodesInsertList = null;
            PatientEncounterDiagnosisCodes ICDCodeObj = null;

            //if (requestObj.PatientID != null && requestObj.PatientID > 0 && _patientRepository.CheckAuthorizationSetting())
            //{
            //    string serviceCodesString = string.Join(",", requestObj.PatientEncounterServiceCodes.Where(p => p.Id > 0)
            //                         .Select(p => p.ServiceCode.ToString()));
            //    List<AppointmentAuthModel> authDetails = _patientRepository.CheckServiceCodesAuthorizationForPatient<AppointmentAuthModel>(Convert.ToInt32(requestObj.PatientID), InsurancePlanType.Primary.ToString(), serviceCodesString, requestObj.StartDateTime).ToList();
            //    if (authDetails != null && authDetails.Count > 0 && authDetails.First().AuthorizationMessage.ToLower() != "valid")
            //        return new JsonModel()
            //        {
            //            data = new object(),
            //            Message = authDetails.First().AuthorizationMessage,
            //            StatusCode = (int)HttpStatusCodes.UnprocessedEntity
            //        };
            //}
            LocationModel locationModal = GetLocationOffsets(requestObj.ServiceLocationID);
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (requestObj.Id == 0)
                    {
                        //  if (requestObj.ParentAppointmentId != null && requestObj.ParentAppointmentId != 0)
                        //    CreateAppointment(requestObj, token);

                        patientEncounter = new PatientEncounter(); soapNote = new SoapNotes();
                        CodesMapping = new PatientEncounterCodesMapping(); ICDCodesList = new List<PatientEncounterDiagnosisCodes>();
                        serviceCodesList = new List<PatientEncounterServiceCodes>(); codesMappingList = new List<PatientEncounterCodesMapping>();
                        AutoMapper.Mapper.Map(requestObj, patientEncounter);
                        patientEncounter.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientEncounter.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                        patientEncounter.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(patientEncounter.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                        AutoMapper.Mapper.Map(requestObj.SOAPNotes, soapNote);
                        AutoMapper.Mapper.Map(requestObj.PatientEncounterServiceCodes, serviceCodesList);
                        AutoMapper.Mapper.Map(requestObj.PatientEncounterDiagnosisCodes, ICDCodesList);
                        patientEncounter.CreatedBy = token.UserID;
                        patientEncounter.OrganizationID = token.OrganizationID;
                        patientEncounter.CreatedDate = DateTime.UtcNow;
                        patientEncounter.IsActive = true;
                        patientEncounter.IsDeleted = false;
                        //mark encounter rendered
                        patientEncounter.Status = _context.GlobalCode.Where(a => a.GlobalCodeName.ToUpper() == "PENDING" && a.OrganizationID == token.OrganizationID).FirstOrDefault().Id;
                        //
                        _patientEncounterRepository.Create(patientEncounter);
                        _patientEncounterRepository.SaveChanges();
                        if (patientEncounter.Id > 0)
                        {
                            //Save Soap Note
                            soapNote.PatientEncounterId = patientEncounter.Id;
                            soapNote.CreatedBy = token.UserID;
                            soapNote.CreatedDate = DateTime.UtcNow;
                            soapNote.IsActive = true; soapNote.IsDeleted = false;
                            _soapNoteRepository.Create(soapNote);
                            _soapNoteRepository.SaveChanges();
                            //Save Encounter Service Codes
                            if (serviceCodesList != null && serviceCodesList.Count > 0)
                            {
                                serviceCodesList.ForEach(x =>
                                {
                                    x.CreatedBy = token.UserID; x.CreatedDate = DateTime.UtcNow; x.PatientEncounterId = patientEncounter.Id; x.IsActive = true; x.IsDeleted = false;
                                    if (!_patientRepository.CheckAuthorizationSetting())
                                    { x.AuthProcedureCPTLinkId = null; x.AuthorizationNumber = null; }
                                });
                                _patientEncounterServiceCodesRepository.Create(serviceCodesList.ToArray());
                                _patientEncounterServiceCodesRepository.SaveChanges();
                                if (_patientRepository.CheckAuthorizationSetting())
                                    BlockServiceCodeUnits(token, serviceCodesList);
                            }
                            //Save Encounter ICD Codes
                            if (ICDCodesList != null && ICDCodesList.Count > 0)
                            {
                                ICDCodesList.ForEach(x =>
                                {
                                    x.CreatedBy = token.UserID; x.CreatedDate = DateTime.UtcNow; x.PatientEncounterId = patientEncounter.Id; x.IsActive = true; x.IsDeleted = false;
                                });
                                _patientEncounterICDCodesRepository.Create(ICDCodesList.ToArray());
                                _patientEncounterICDCodesRepository.SaveChanges();
                            }
                            //Save Codes Mappings
                            CreateEncounterCodesMappings(token, patientEncounter, ICDCodesList, serviceCodesList, codesMappingList);
                        }
                    }
                    else
                    {
                        //#region check config settings
                        //List<AppConfigurations> appCon = _context.AppConfigurations.Where(a => a.OrganizationID == token.OrganizationID).ToList();
                        //foreach (var item in appCon)
                        //{

                        //    if (item.Key.ToUpper() == CommonEnum.AppConfigurationsEnum.CLINICIAN_SIGN.ToString() && item.Value == "true" && (requestObj.ClinicianSign == null || requestObj.ClinicianSign.Length == 0 || requestObj.ClinicianSignDate == null))
                        //    {
                        //        transaction.Rollback();
                        //        return new JsonModel()
                        //        {
                        //            data = new object(),
                        //            Message = "Clinician sign required",
                        //            StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                        //        };
                        //    }
                        //    else if (item.Key.ToUpper() == CommonEnum.AppConfigurationsEnum.PATIENT_SIGN.ToString() && item.Value == "true" && (requestObj.PatientSign == null || requestObj.PatientSign.Length == 0 || requestObj.PatientSignDate == null))
                        //    {
                        //        transaction.Rollback();
                        //        return new JsonModel()
                        //        {
                        //            data = new object(),
                        //            Message = "Patient sign required",
                        //            StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                        //        };

                        //    }
                        //    else if (item.Key.ToUpper() == CommonEnum.AppConfigurationsEnum.GUARDIAN_SIGN.ToString() && item.Value == "true" && (requestObj.GuardianSign == null || requestObj.GuardianSign.Length == 0 || requestObj.GuardianSignDate == null || string.IsNullOrEmpty(requestObj.GuardianName)))
                        //    {
                        //        transaction.Rollback();
                        //        return new JsonModel()
                        //        {
                        //            data = new object(),
                        //            Message = "Guardian sign required",
                        //            StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                        //        };
                        //    }
                        //}
                        //#endregion
                        patientEncounter = _patientEncounterRepository.Get(x => x.Id == requestObj.Id && x.IsActive == true && x.IsDeleted == false);
                        if (!ReferenceEquals(patientEncounter, null))
                        {
                            //Update patient encounter
                            patientEncounter.StartDateTime = CommonMethods.ConvertToUtcTimeWithOffset(requestObj.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                            patientEncounter.EndDateTime = CommonMethods.ConvertToUtcTimeWithOffset(requestObj.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset);
                            //patientEncounter.ClinicianSign = requestObj.ClinicianSign;
                            //patientEncounter.ClinicianSignDate = requestObj.ClinicianSignDate;
                            //patientEncounter.PatientSign = requestObj.PatientSign;
                            //patientEncounter.PatientSignDate = requestObj.PatientSignDate;
                            //patientEncounter.GuardianSign = requestObj.GuardianSign;
                            //patientEncounter.GuardianSignDate = requestObj.GuardianSignDate;
                            //patientEncounter.GuardianName = requestObj.GuardianName;
                            patientEncounter.AppointmentTypeId = appointmentTypeId;
                            patientEncounter.CustomAddressID = requestObj.CustomAddressID;
                            patientEncounter.CustomAddress = requestObj.CustomAddress;
                            patientEncounter.PatientAddressID = requestObj.PatientAddressID;
                            patientEncounter.OfficeAddressID = requestObj.OfficeAddressID;
                            patientEncounter.ServiceLocationID = requestObj.ServiceLocationID;
                            patientEncounter.StaffID = requestObj.StaffID;

                            //TODO
                            //_imageService.SaveImages(requestObj.ClinicianSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.ClinicianSign.ToString());
                            //_imageService.SaveImages(requestObj.GuardianSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.GuardianSign.ToString());
                            //_imageService.SaveImages(requestObj.PatientSign, ImagesPath.EncounterSignImages, ImagesFolderEnum.PatientSign.ToString());


                            //mark encounter rendered
                            patientEncounter.Status = _context.GlobalCode.Where(a => a.GlobalCodeName.ToUpper() == "RENDERED" && a.OrganizationID == token.OrganizationID).FirstOrDefault().Id;
                            //
                            patientEncounter.UpdatedBy = token.UserID;
                            patientEncounter.UpdatedDate = DateTime.UtcNow;
                            _patientRepository.SaveChanges();
                            //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token);
                            if (requestObj.SOAPNotes != null)
                            {
                                //Update patient soap note
                                soapNote = _soapNoteRepository.Get(x => x.Id == requestObj.SOAPNotes.Id && x.IsDeleted == false && x.IsActive == true);
                                if (!ReferenceEquals(soapNote, null))
                                {
                                    soapNote.Subjective = string.IsNullOrEmpty(requestObj.SOAPNotes.Subjective) ? null : requestObj.SOAPNotes.Subjective;
                                    soapNote.Objective = string.IsNullOrEmpty(requestObj.SOAPNotes.Objective) ? null : requestObj.SOAPNotes.Objective;
                                    soapNote.Assessment = string.IsNullOrEmpty(requestObj.SOAPNotes.Assessment) ? null : requestObj.SOAPNotes.Assessment;
                                    soapNote.Plans = string.IsNullOrEmpty(requestObj.SOAPNotes.Plans) ? null : requestObj.SOAPNotes.Plans;
                                    soapNote.UpdatedBy = token.UserID;
                                    soapNote.UpdatedDate = DateTime.UtcNow;
                                    _soapNoteRepository.Update(soapNote);
                                    _soapNoteRepository.SaveChanges();
                                    //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token);
                                }
                            }
                            if (requestObj.PatientEncounterServiceCodes != null)
                            {
                                if (isAdmin)
                                {
                                    UpdatePreviousServiceCodesAndMappings(requestObj, token);
                                }
                                //List to insert new cptcodes
                                serviceCodesInsertList = new List<PatientEncounterServiceCodes>();
                                serviceCodesList = _patientEncounterServiceCodesRepository.GetAll(x => x.PatientEncounterId == requestObj.Id && x.IsActive == true && x.IsDeleted == false).ToList();

                                foreach (PatientEncounterServiceCodesModel serviceCodeModel in requestObj.PatientEncounterServiceCodes)
                                {
                                    if (serviceCodeModel.Id > 0)
                                    {
                                        //Update service codes if exist in Soap
                                        serviceCodeObj = serviceCodesList.Find(x => x.Id == serviceCodeModel.Id);
                                        if (!ReferenceEquals(serviceCodeObj, null) && serviceCodeModel.IsDeleted == true)
                                        {
                                            serviceCodeObj.IsDeleted = serviceCodeModel.IsDeleted;
                                            serviceCodeObj.DeletedDate = DateTime.UtcNow;
                                            serviceCodeObj.DeletedBy = token.UserID;
                                        }
                                        else if (!ReferenceEquals(serviceCodeObj, null) && serviceCodeModel.IsDeleted == false)
                                        {
                                            serviceCodeObj.Modifier1 = serviceCodeModel.Modifier1 == null ? null : serviceCodeModel.Modifier1;
                                            serviceCodeObj.Modifier2 = serviceCodeModel.Modifier2 == null ? null : serviceCodeModel.Modifier2;
                                            serviceCodeObj.Modifier3 = serviceCodeModel.Modifier3 == null ? null : serviceCodeModel.Modifier3;
                                            serviceCodeObj.Modifier4 = serviceCodeModel.Modifier4 == null ? null : serviceCodeModel.Modifier4;
                                            serviceCodeObj.UpdatedBy = token.UserID;
                                            serviceCodeObj.UpdatedDate = DateTime.UtcNow;
                                        }
                                    }
                                    else
                                    {
                                        //Insert new cptcodes to soap
                                        serviceCodeObj = new PatientEncounterServiceCodes()
                                        {
                                            ServiceCodeId = serviceCodeModel.ServiceCodeId,
                                            Modifier1 = serviceCodeModel.Modifier1 == null ? null : serviceCodeModel.Modifier1,
                                            Modifier2 = serviceCodeModel.Modifier2 == null ? null : serviceCodeModel.Modifier2,
                                            Modifier3 = serviceCodeModel.Modifier3 == null ? null : serviceCodeModel.Modifier3,
                                            Modifier4 = serviceCodeModel.Modifier4 == null ? null : serviceCodeModel.Modifier4,
                                            AuthorizationNumber = (_patientRepository.CheckAuthorizationSetting() == false || string.IsNullOrEmpty(serviceCodeModel.AuthorizationNumber)) ? null : serviceCodeModel.AuthorizationNumber,
                                            AuthProcedureCPTLinkId = (_patientRepository.CheckAuthorizationSetting() == false || serviceCodeModel.AuthProcedureCPTLinkId == null || serviceCodeModel.AuthProcedureCPTLinkId == 0) ? null : serviceCodeModel.AuthProcedureCPTLinkId,
                                            PatientEncounterId = Convert.ToInt32(requestObj.Id),
                                            CreatedBy = token.UserID,
                                            CreatedDate = DateTime.UtcNow,
                                            IsDeleted = false,
                                            IsActive = true
                                        };
                                        serviceCodesInsertList.Add(serviceCodeObj);
                                    }
                                }
                                if (serviceCodesList != null && serviceCodesList.Count() > 0)
                                {
                                    _patientEncounterServiceCodesRepository.Update(serviceCodesList.ToArray());
                                    _auditLogRepository.SaveChanges();
                                    //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token);
                                }
                                if (serviceCodesInsertList != null && serviceCodesInsertList.Count > 0)
                                {
                                    _patientEncounterServiceCodesRepository.Create(serviceCodesInsertList.ToArray());
                                    _patientEncounterServiceCodesRepository.SaveChanges();
                                    if (_patientRepository.CheckAuthorizationSetting())
                                        BlockServiceCodeUnits(token, serviceCodesInsertList);
                                }
                            }
                            if (requestObj.PatientEncounterDiagnosisCodes != null && requestObj.PatientEncounterDiagnosisCodes.Count > 0)
                            {
                                ICDCodesInsertList = new List<PatientEncounterDiagnosisCodes>();
                                ICDCodesList = _patientEncounterICDCodesRepository.GetAll(x => x.PatientEncounterId == requestObj.Id && x.IsActive == true && x.IsDeleted == false).ToList();

                                foreach (PatientEncounterDiagnosisCodesModel ICDCodeModel in requestObj.PatientEncounterDiagnosisCodes)
                                {
                                    if (ICDCodeModel.Id > 0 && ICDCodeModel.IsDeleted == true)
                                    {
                                        //Update service codes if exist in Soap
                                        ICDCodeObj = ICDCodesList.Find(x => x.Id == ICDCodeModel.Id);
                                        if (!ReferenceEquals(ICDCodeObj, null))
                                        {
                                            ICDCodeObj.IsDeleted = ICDCodeModel.IsDeleted;
                                            ICDCodeObj.DeletedDate = DateTime.UtcNow;
                                            ICDCodeObj.DeletedBy = token.UserID;
                                        }
                                    }
                                    else if (ICDCodeModel.Id == 0)
                                    {
                                        //Insert new cptcodes to soap
                                        ICDCodeObj = new PatientEncounterDiagnosisCodes()
                                        {
                                            ICDCodeId = ICDCodeModel.ICDCodeId,
                                            PatientEncounterId = Convert.ToInt32(requestObj.Id),
                                            CreatedBy = token.UserID,
                                            CreatedDate = DateTime.UtcNow,
                                            IsDeleted = false,
                                            IsActive = true
                                        };
                                        ICDCodesInsertList.Add(ICDCodeObj);
                                    }
                                }
                                if (ICDCodesList != null && ICDCodesList.Where(x => x.IsDeleted == true).Count() > 0)
                                {
                                    _patientEncounterICDCodesRepository.Update(ICDCodesList.Where(x => x.IsDeleted == true).ToArray());
                                    _auditLogRepository.SaveChanges();
                                    //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.PatientEncounter, AuditLogAction.Modify, pId, token.UserID, "EncounterId/" + requestObj.Id, token);
                                }
                                if (ICDCodesInsertList != null && ICDCodesInsertList.Count > 0)
                                {
                                    _patientEncounterICDCodesRepository.Create(ICDCodesInsertList.ToArray());
                                    _patientEncounterICDCodesRepository.SaveChanges();
                                }
                                // if ((ICDCodesList != null && ICDCodesList.Where(x => x.IsDeleted == true).Count() > 0) || (ICDCodesInsertList != null && ICDCodesInsertList.Count > 0))
                            }
                            if (serviceCodesInsertList != null && serviceCodesInsertList.Count > 0)
                                CreateEncounterCodesMappings(token, patientEncounter, ICDCodesList, serviceCodesInsertList, codesMappingList);
                            if (ICDCodesInsertList != null && ICDCodesInsertList.Count > 0)
                                CreateEncounterCodesMappings(token, patientEncounter, ICDCodesInsertList, serviceCodesList, codesMappingList);
                            if ((serviceCodesInsertList != null && serviceCodesInsertList.Count > 0) && (ICDCodesInsertList != null && ICDCodesInsertList.Count > 0))
                                CreateEncounterCodesMappings(token, patientEncounter, ICDCodesInsertList, serviceCodesInsertList, codesMappingList);
                        }
                    }
                    SaveTimesheetData(token, patientEncounter);
                    SaveDriveTimeData(token, patientEncounter);
                    //transaction.Rollback();
                    transaction.Commit();
                    return new JsonModel()
                    {
                        data = patientEncounter,
                        Message = Common.HC.Common.StatusMessage.SoapSuccess,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.OK//Success
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = ex.Message,
                        StatusCode = (int)CommonEnum.HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                    };
                }
            }
        }

        private int GetTimesheetStatusId(string status, TokenModel token)
        {
            return _context.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == "timesheetstatus" && a.OrganizationID == token.OrganizationID && a.GlobalCodeName.ToLower() == status.ToLower()).OrderBy(a => a.DisplayOrder).FirstOrDefault().Id;

        }
        private void SaveTimesheetData(TokenModel token, PatientEncounter patientEncounter)
        {
            int? appoinmentTypeId = _context.PatientEncounter.Join(_context.PatientAppointment, enc => enc.PatientAppointmentId, apt => apt.Id, (enc, apt) => new { enc, apt }).Where(x => x.enc.Id == patientEncounter.Id).FirstOrDefault().apt.AppointmentTypeID;
            decimal duration = Convert.ToDecimal(patientEncounter.EndDateTime.Subtract(patientEncounter.StartDateTime).TotalMinutes / 60);
            SaveTimesheet(token, patientEncounter.StartDateTime, patientEncounter.EndDateTime, 0, patientEncounter.Id, (int)patientEncounter.StaffID, patientEncounter.DateOfService, false, appoinmentTypeId, duration,"Activity Notes");
        }
        private void SaveDriveTimeData(TokenModel token, PatientEncounter patientEncounter)
        {
            int? appoinmentTypeId = _context.AppointmentType.Where(x => x.Name.ToLower() == "travel" && x.OrganizationID == token.OrganizationID).First().Id;
            double? lat1 = 0;
            double? long1 = 0;
            double? lat2 = 0;
            double? long2 = 0;
            decimal distance = 0;
            decimal driveTime = 0;

            PatientAppointmentModel apt1 = null, apt2 = null;

            apt2 = _context.PatientEncounter.Join(_context.PatientAppointment, enc => enc.PatientAppointmentId, apt => apt.Id, (enc, apt) => new
            {
                enc,
                apt
            }).Where(y => y.enc.Id == patientEncounter.Id && y.enc.IsDeleted == false && y.apt.IsDeleted == false).Select(z => new PatientAppointmentModel()
            {
                PatientAppointmentId = z.apt.Id,
                StartDateTime = z.apt.StartDateTime,
                EndDateTime = z.apt.EndDateTime,
                Latitude = z.apt.Latitude,
                Longitude = z.apt.Longitude
            }).FirstOrDefault();

            apt1 = _context.PatientEncounter.Join(_context.PatientAppointment, enc => enc.PatientAppointmentId, apt => apt.Id, (enc, apt) => new
            {
                enc,
                apt
            }).Where(y => y.enc.StaffID == patientEncounter.StaffID
                && y.enc.DateOfService.Date == patientEncounter.DateOfService.Date
                && y.enc.IsDeleted == false
                && y.apt.IsDeleted == false
                && y.apt.IsExcludedFromMileage == false
                && y.apt.StartDateTime < apt2.StartDateTime)
            .OrderByDescending(k => k.apt.StartDateTime)
            .Select(z => new PatientAppointmentModel()
            {
                PatientAppointmentId = z.apt.Id,
                StartDateTime = z.apt.StartDateTime,
                EndDateTime = z.apt.EndDateTime,
                Latitude = z.apt.Latitude,
                Longitude = z.apt.Longitude
            }).FirstOrDefault();
            if (apt1 != null && apt1.Latitude != 0 && apt1.Longitude != 0 && apt2 != null && apt2.Latitude != 0 && apt2.Longitude != 0)
            {
                lat2 = apt2.Latitude;
                long2 = apt2.Longitude;
                lat1 = apt1.Latitude;
                long1 = apt1.Longitude;
                GetDriveTimeAndDistance(lat1, long1, lat2, long2, "driving", "en-EN", "metric", ref distance, ref driveTime);
                SaveTimesheet(token, apt1.EndDateTime, apt2.StartDateTime, distance, patientEncounter.Id, (int)patientEncounter.StaffID, patientEncounter.DateOfService, true, appoinmentTypeId, driveTime,"Drive Time Notes");
            }
        }

        private void SaveTimesheet(TokenModel token, DateTime startDateTime, DateTime endDateTime, decimal distance, int patientEncounterId, int staffId, DateTime dateOfService, bool isTravelTime, int? appointmentTypeId, decimal duration,string notes)
        {
            UserTimesheetByAppointmentType userTimesheetByAppointmentType = _userTimesheetByAppointmentTypeRepository.GetAll(x => x.PatientEncounterId == patientEncounterId && x.IsTravelTime == isTravelTime).FirstOrDefault();
            if (userTimesheetByAppointmentType != null)
            {
                userTimesheetByAppointmentType.ActualTimeDuration = duration;
                userTimesheetByAppointmentType.ExpectedTimeDuration = duration;
                userTimesheetByAppointmentType.StartDateTime = startDateTime;
                userTimesheetByAppointmentType.EndDateTime = endDateTime;
                userTimesheetByAppointmentType.UpdatedBy = token.UserID;
                userTimesheetByAppointmentType.Distance = distance;
                //userTimesheetByAppointmentType.IsTravelTime = isTravelTime;
                userTimesheetByAppointmentType.UpdatedDate = DateTime.UtcNow;
                _userTimesheetByAppointmentTypeRepository.Update(userTimesheetByAppointmentType);
            }
            else
            {
                userTimesheetByAppointmentType = new UserTimesheetByAppointmentType();
                userTimesheetByAppointmentType.ActualTimeDuration = duration;
                userTimesheetByAppointmentType.ExpectedTimeDuration = duration;
                userTimesheetByAppointmentType.AppointmentTypeId = appointmentTypeId;
                userTimesheetByAppointmentType.PatientEncounterId = patientEncounterId;
                userTimesheetByAppointmentType.StaffId = staffId;
                userTimesheetByAppointmentType.DateOfService = dateOfService;
                userTimesheetByAppointmentType.StartDateTime = startDateTime;
                userTimesheetByAppointmentType.EndDateTime = endDateTime;
                userTimesheetByAppointmentType.Distance = distance;
                userTimesheetByAppointmentType.CreatedBy = token.UserID;
                userTimesheetByAppointmentType.CreatedDate = DateTime.UtcNow;
                userTimesheetByAppointmentType.OrganizationId = token.OrganizationID;
                userTimesheetByAppointmentType.LocationId = token.LocationID; //Can be location id from appointment table
                userTimesheetByAppointmentType.IsActive = true;
                userTimesheetByAppointmentType.IsDeleted = false;
                userTimesheetByAppointmentType.IsTravelTime = isTravelTime;
                userTimesheetByAppointmentType.StatusId = GetTimesheetStatusId("pending", token);
                userTimesheetByAppointmentType.Notes = notes;
                _userTimesheetByAppointmentTypeRepository.Create(userTimesheetByAppointmentType);

            }
            _userTimesheetByAppointmentTypeRepository.SaveChanges();
        }

        private void UpdatePreviousServiceCodesAndMappings(PatientEncounterModel requestObj, TokenModel token)
        {
            List<PatientEncounterServiceCodes> deleteList = _patientEncounterServiceCodesRepository.GetAll(x => x.PatientEncounterId == requestObj.Id).ToList();
            deleteList.ForEach(x => { x.IsDeleted = true; x.DeletedBy = token.UserID; x.DeletedDate = DateTime.UtcNow; });
            _patientEncounterServiceCodesRepository.Update(deleteList.ToArray());
            _patientEncounterServiceCodesRepository.SaveChanges();
            List<PatientEncounterCodesMapping> deleteMappings = _patientEncounterCodesMappingsRepository.GetAll(x => x.PatientEncounterId == requestObj.Id).ToList();
            deleteMappings.ForEach(x => { x.IsDeleted = true; x.DeletedBy = token.UserID; x.DeletedDate = DateTime.UtcNow; });
            _patientEncounterCodesMappingsRepository.Update(deleteMappings.ToArray());
            _patientEncounterCodesMappingsRepository.SaveChanges();
        }

        private void BlockServiceCodeUnits(TokenModel token, List<PatientEncounterServiceCodes> serviceCodesList)
        {
            //May be needed in future
            //int[] authds = serviceCodesList.Where(x=>(x.AuthProcedureCPTLinkId!=null && x.AuthProcedureCPTLinkId>0)).Select(y => (int)y.AuthProcedureCPTLinkId).ToArray();
            //List<AuthProcedureCPT> AuthCPT = _patientAuthorizationProcedureCPTLinkRepository.GetAll().Where(c => authds.Contains(c.Id)).ToList();
            //if (AuthCPT != null && AuthCPT.Count > 0)
            //{
            //    AuthCPT.ForEach(z => { z.BlockedUnit = (z.BlockedUnit == null ? 0 : z.BlockedUnit) + 1; z.UpdatedBy = token.UserID; z.UpdatedDate = DateTime.UtcNow; });
            //    _patientAuthorizationProcedureCPTLinkRepository.Update(AuthCPT.ToArray());
            //    _patientAuthorizationProcedureCPTLinkRepository.SaveChanges();
            //}
        }

        private void CreateEncounterCodesMappings(TokenModel token, PatientEncounter patientEncounter, List<PatientEncounterDiagnosisCodes> ICDCodesList, List<PatientEncounterServiceCodes> serviceCodesList, List<PatientEncounterCodesMapping> codesMappingList)
        {
            if (serviceCodesList != null && serviceCodesList.Count > 0 && ICDCodesList != null && ICDCodesList.Count > 0)
            {
                codesMappingList = serviceCodesList.Join(ICDCodesList, SC => SC.PatientEncounter, ICD => ICD.PatientEncounter, (SC, DXCode) => new PatientEncounterCodesMapping
                {
                    PatientEncounterId = patientEncounter.Id,
                    PatientEncounterServiceCodeId = SC.Id,
                    PatientEncounterDiagnosisCodeId = DXCode.Id,
                    IsMapped = true,
                    CreatedBy = token.UserID,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false,
                    IsActive = true
                }).Where(x => x.Id == 0).ToList();

                _patientEncounterCodesMappingsRepository.Create(codesMappingList.ToArray());
                _patientEncounterCodesMappingsRepository.SaveChanges();
            }
        }

        public List<PatientEncounterModel> GetPatientEncounter(int? patientID, string appointmentType = "", string staffName = "", string status = "", string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "", TokenModel token = null)
        {
            List<PatientEncounterModel> response = _patientEncounterRepository.GetAllEncounters<PatientEncounterModel>(patientID, appointmentType, staffName, status, fromDate, toDate, pageNumber, pageSize, sortColumn, sortOrder).ToList();
            if (response != null && response.Count > 0)
                response.ForEach(x =>
                {
                    LocationModel locationModal = GetLocationOffsets(x.ServiceLocationID);
                    x.StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.StartDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);
                    x.EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(x.EndDateTime, locationModal.DaylightOffset, locationModal.StandardOffset, token);
                });
            return response;
        }

        private void CreateAppointment(PatientEncounterModel requestObj, TokenModel token)
        {
            PatientAppointment appointment = _appointmentRepository.Get(x => x.Id == requestObj.ParentAppointmentId && x.IsDeleted == false && x.IsActive == true);
            if (appointment != null)
            {
                PatientAppointment apptReqObj = new PatientAppointment()
                {
                    AppointmentTypeID = appointment.AppointmentTypeID,
                    PatientID = appointment.PatientID,
                    PatientAddressID = appointment.PatientAddressID,
                    StartDateTime = requestObj.AppointmentStartDateTime,
                    EndDateTime = requestObj.AppointmentEndDateTime,
                    CreatedBy = token.UserID,
                    CreatedDate = DateTime.UtcNow,
                    ServiceLocationID = appointment.ServiceLocationID,
                    IsTelehealthAppointment = appointment.IsTelehealthAppointment,
                    IsActive = true,
                    IsDeleted = false,
                    ParentAppointmentID = requestObj.ParentAppointmentId
                };
                _appointmentRepository.Create(apptReqObj);
                _appointmentRepository.SaveChanges();
                requestObj.PatientAppointmentId = apptReqObj.Id;
            }
        }


        public int? GetPatientIdFromEncounterId(int patientEncounterId)
        {
            return _patientEncounterRepository.Get(x => x.Id == patientEncounterId && x.IsActive == true && x.IsDeleted == false).PatientID;
        }

        public JsonModel SavePatientSignForPatientEncounter(int patientEncounterId, PatientEncounterModel patientEncounterModel)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel SaveEncounterSignature(EncounterSignatureModel encounterSignatureModel)
        {
            List<EncounterSignatureModel> encounterSignatureModels = new List<EncounterSignatureModel>();
            encounterSignatureModels.Add(encounterSignatureModel);
            List<EncounterSignature> encounterSignature = null;
            encounterSignature = SaveSignature(encounterSignatureModels);
            if (encounterSignatureModel.Id > 0)
            {
                response = new JsonModel(encounterSignature.FirstOrDefault(), StatusMessage.SignatureUpdated, (int)HttpStatusCode.OK);
            }
            else
            {
                response = new JsonModel(encounterSignature.FirstOrDefault(), StatusMessage.SignatureCreated, (int)HttpStatusCode.OK);
            }
            return response;
        }

        private List<EncounterSignature> SaveSignature(List<EncounterSignatureModel> encounterSignatureModels)
        {
            List<EncounterSignature> encounterSignatures = new List<EncounterSignature>();
            foreach (var encounterSignatureModel in encounterSignatureModels)
            {
                EncounterSignature encounterSignature = null;
                if (encounterSignatureModel.Id > 0)
                {
                    encounterSignature = _context.EncounterSignature.Where(a => a.PatientEncounterId == encounterSignatureModel.PatientEncounterId && a.Id == encounterSignatureModel.Id).FirstOrDefault();
                    encounterSignature.ClinicianSign = encounterSignatureModel.ClinicianSign;
                    encounterSignature.ClinicianSignDate = encounterSignatureModel.ClinicianSignDate;
                    encounterSignature.PatientSign = encounterSignatureModel.PatientSign;
                    encounterSignature.PatientSignDate = encounterSignatureModel.PatientSignDate;
                    encounterSignature.GuardianSign = encounterSignatureModel.GuardianSign;
                    encounterSignature.GuardianSignDate = encounterSignatureModel.GuardianSignDate;
                    encounterSignature.GuardianName = encounterSignatureModel.GuardianName;
                    encounterSignatures.Add(encounterSignature);
                }
                else
                {
                    encounterSignature = new EncounterSignature();
                    encounterSignature.ClinicianSign = encounterSignatureModel.ClinicianSign;
                    encounterSignature.ClinicianSignDate = encounterSignatureModel.ClinicianSignDate;
                    encounterSignature.PatientSign = encounterSignatureModel.PatientSign;
                    encounterSignature.PatientSignDate = encounterSignatureModel.PatientSignDate;
                    encounterSignature.GuardianSign = encounterSignatureModel.GuardianSign;
                    encounterSignature.GuardianSignDate = encounterSignatureModel.GuardianSignDate;
                    encounterSignature.GuardianName = encounterSignatureModel.GuardianName;
                    encounterSignature.PatientId = encounterSignatureModel.PatientId;
                    encounterSignature.StaffId = encounterSignatureModel.StaffId;
                    encounterSignature.PatientEncounterId = encounterSignatureModel.PatientEncounterId;
                    encounterSignatures.Add(encounterSignature);
                }
            }
            _context.UpdateRange(encounterSignatures);
            _context.SaveChanges();

            return encounterSignatures;
        }

        public MemoryStream DownloadEncounter(int encounterId, TokenModel token)
        {
            PdfDocument document = new PdfDocument();

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

            // Draw the text
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
              new XRect(0, 0, page.Width, page.Height),
              XStringFormat.Center);

            // Save the document...
            //string filename = "HelloWorld.pdf";
            MemoryStream memoryStream = new MemoryStream();
            document.Save(memoryStream, false);
            return memoryStream;
        }

        private void GetDriveTimeAndDistance(double? lat1, double? long1, double? lat2, double? long2, string mode, string language, string units, ref decimal distance, ref decimal driveTime)
        {
            string URL = "http://maps.googleapis.com/maps/api/distancematrix/json?origins=" + lat1 + "," + long1 + "&destinations=" + lat2 + "," + long2 + "&mode=driving&language=en-EN&units=metric";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json";
            WebResponse webResponse = request.GetResponse();
            using (Stream webStream = webResponse.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        RootObject root = JsonConvert.DeserializeObject<RootObject>(response);
                        if (root != null && root.rows != null && root.rows.Count > 0 && root.rows.FirstOrDefault().elements != null && root.rows.FirstOrDefault().elements.Count > 0 && root.rows.FirstOrDefault().elements.FirstOrDefault().distance != null && root.rows.FirstOrDefault().elements.FirstOrDefault().duration != null)
                        {
                            distance = Math.Round(Convert.ToDecimal((root.rows.FirstOrDefault().elements.FirstOrDefault().distance.value) / 1000.0), 2);
                            driveTime = Math.Round(Convert.ToDecimal((root.rows.FirstOrDefault().elements.FirstOrDefault().duration.value) / 3600.0), 2);
                        }
                    }
                }
            }
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

        public JsonModel SavePatientEncounterTemplateData(PatientEncounterTemplateModel patientEncounterTemplateModel, TokenModel token)
        {
            PatientEncounterTemplates patientEncounterTemplates = null;
            if(patientEncounterTemplateModel.Id == 0)
            {
                patientEncounterTemplates = new PatientEncounterTemplates();
                AutoMapper.Mapper.Map(patientEncounterTemplateModel, patientEncounterTemplates);
                patientEncounterTemplates.OrganizationID = token.OrganizationID;
                patientEncounterTemplates.CreatedBy = token.UserID;
                _patientEncounterTemplateRepository.Create(patientEncounterTemplates);
                _patientEncounterTemplateRepository.SaveChanges();
                response = new JsonModel(patientEncounterTemplates, StatusMessage.PatientEncounterTemplateCreated, (int)HttpStatusCodes.OK);
            }
            else
            {
                patientEncounterTemplates = _patientEncounterTemplateRepository.Get(p => p.Id == patientEncounterTemplateModel.Id && p.IsDeleted == false && p.IsActive == true && p.OrganizationID == token.OrganizationID);
                if(patientEncounterTemplates != null)
                {
                    AutoMapper.Mapper.Map(patientEncounterTemplateModel, patientEncounterTemplates);
                    patientEncounterTemplates.UpdatedBy = token.UserID;
                    patientEncounterTemplates.UpdatedDate = DateTime.UtcNow;
                    _patientEncounterTemplateRepository.Update(patientEncounterTemplates);
                    _patientEncounterTemplateRepository.SaveChanges();
                    response = new JsonModel(patientEncounterTemplates, StatusMessage.PatientEncounterTemplateUpdated, (int)HttpStatusCodes.OK);
                }
            }
            return response;
        }

        public JsonModel GetPatientEncounterTemplateData(int patientEncounterId, int masterTemplateId, TokenModel tokenModel)
        {
            PatientEncounterTemplateModel patientEncounterTemplateModel = _patientEncounterTemplateRepository.GetPatientEncounterTemplateData<PatientEncounterTemplateModel>(patientEncounterId, masterTemplateId, tokenModel).FirstOrDefault();
            if(patientEncounterTemplateModel != null)
            {
                response = new JsonModel(patientEncounterTemplateModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
    }
}