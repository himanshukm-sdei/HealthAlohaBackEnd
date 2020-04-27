using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Availability;
using HC.Patient.Repositories.IRepositories.Locations;
using HC.Patient.Repositories.IRepositories.StaffAvailability;
using HC.Patient.Service.IServices.StaffAvailability;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.StaffAvailability
{
    public class StaffAvailabilityService : BaseService, IStaffAvailabilityService
    {
        private readonly IStaffAvailabilityRepository _staffAvailabilityRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly HCOrganizationContext _context;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
        public StaffAvailabilityService(IStaffAvailabilityRepository staffAvailabilityRepository, ILocationRepository locationRepository, HCOrganizationContext context)
        {
            _staffAvailabilityRepository = staffAvailabilityRepository;
            _locationRepository = locationRepository;
            _context = context;
        }

        public JsonModel SaveStaffAvailabilty(AvailabilityModel entity, TokenModel token)
        {
            using (var transaction = _context.Database.BeginTransaction()) //TO DO do this with SP
            {

                try
                {
                    List<Entity.StaffAvailability> dbStaffAvailabilityInsertList = new List<Entity.StaffAvailability>();
                    List<Entity.StaffAvailability> dbStaffAvailabilityUpdateList = new List<Entity.StaffAvailability>();
                    List<int> availabilityIds = new List<int>();

                    //Days ids
                    entity.Days.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //Avaliable ids
                    entity.Available.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //Unavaliable ids
                    entity.Unavailable.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //for update
                    dbStaffAvailabilityUpdateList = _staffAvailabilityRepository.GetAll(a => availabilityIds.Contains(a.Id)).ToList();  //_context.StaffAvailability.Where(a => availabilityIds.Contains(a.Id)).ToList();

                    foreach (var day in entity.Days)
                    {
                        if (day.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == day.Id)
                                {
                                    a.IsDeleted = day.IsDeleted;
                                    a.DayId = day.DayId;
                                    a.StartTime = (day.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)day.StartTime, token) : (DateTime?)null);
                                    a.EndTime = (day.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)day.EndTime, token) : (DateTime?)null);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = day.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.StartTime = (day.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)day.StartTime, token) : (DateTime?)null);
                            staffAvailability.EndTime = (day.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)day.EndTime, token) : (DateTime?)null);
                            staffAvailability.DayId = day.DayId;
                            staffAvailability.StaffAvailabilityTypeID = day.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = day.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    foreach (var avaliable in entity.Available)
                    {
                        if (avaliable.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == avaliable.Id)
                                {
                                    a.IsDeleted = avaliable.IsDeleted;
                                    a.Date = (avaliable.Date != null) ? avaliable.Date : (DateTime?)null;  // (CommonMethods.ConvertUtcTime((DateTime)avaliable.Date.Value.AddHours(avaliable.StartTime.Value.Hour).AddMinutes(avaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                                    a.StartTime = (avaliable.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)avaliable.StartTime, token) : (DateTime?)null);
                                    a.EndTime = (avaliable.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)avaliable.EndTime, token) : (DateTime?)null);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = avaliable.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.Date = (avaliable.Date != null) ? avaliable.Date : (DateTime?)null;  // (CommonMethods.ConvertUtcTime((DateTime)avaliable.Date.Value.AddHours(avaliable.StartTime.Value.Hour).AddMinutes(avaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                            staffAvailability.StartTime = (avaliable.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)avaliable.StartTime, token) : (DateTime?)null);
                            staffAvailability.EndTime = (avaliable.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)avaliable.EndTime, token) : (DateTime?)null);
                            staffAvailability.StaffAvailabilityTypeID = avaliable.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = avaliable.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    foreach (var unavaliable in entity.Unavailable)
                    {
                        if (unavaliable.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == unavaliable.Id)
                                {
                                    a.IsDeleted = unavaliable.IsDeleted;
                                    a.Date = (unavaliable.Date != null) ? unavaliable.Date : null; // (CommonMethods.ConvertUtcTime((DateTime)unavaliable.Date.Value.AddHours(unavaliable.StartTime.Value.Hour).AddMinutes(unavaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                                    a.StartTime = (unavaliable.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)unavaliable.StartTime, token) : (DateTime?)null);
                                    a.EndTime = (unavaliable.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)unavaliable.EndTime, token) : (DateTime?)null);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = unavaliable.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.Date = (unavaliable.Date != null) ? unavaliable.Date : null; // (CommonMethods.ConvertUtcTime((DateTime)unavaliable.Date.Value.AddHours(unavaliable.StartTime.Value.Hour).AddMinutes(unavaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                            staffAvailability.StartTime = (unavaliable.StartTime != null ? CommonMethods.ConvertUtcTime((DateTime)unavaliable.StartTime, token) : (DateTime?)null);
                            staffAvailability.EndTime = (unavaliable.EndTime != null ? CommonMethods.ConvertUtcTime((DateTime)unavaliable.EndTime, token) : (DateTime?)null);
                            staffAvailability.StaffAvailabilityTypeID = unavaliable.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = unavaliable.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    if (dbStaffAvailabilityInsertList.Count > 0)
                        _staffAvailabilityRepository.Create(dbStaffAvailabilityInsertList.ToArray());

                    if (dbStaffAvailabilityUpdateList.Count > 0)
                        _staffAvailabilityRepository.Update(dbStaffAvailabilityUpdateList.ToArray());

                    //save changes
                    _staffAvailabilityRepository.SaveChanges();

                    //transaction commit
                    transaction.Commit();

                    //Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.SavedStaffAvailability,
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                    };
                }
                catch (Exception ex)
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

        public JsonModel GetStaffAvailabilty(string staffID, TokenModel token, bool isLeaveNeeded)
        {
            try
            {
                List<int> staffIds = staffID.Split(',').Select(Int32.Parse).ToList();
                AvailabilityModel list = new AvailabilityModel();
                IQueryable<Entity.StaffAvailability> dbList = _staffAvailabilityRepository.GetAll(a => staffIds.Contains(a.StaffID) && a.IsDeleted == false && a.IsActive == true).AsQueryable();
                IQueryable<MasterWeekDays> weekDays = _context.MasterWeekDays.Where(a => a.OrganizationID == token.OrganizationID).AsQueryable();
                //list.StaffID = staffID;

                list.Days = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.WEEKDAY.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new DayModel()
                {
                    Id = y.Id,
                    DayId = y.DayId,
                    DayName = weekDays.Where(z => z.Id == y.DayId).FirstOrDefault().Day,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTime(Convert.ToDateTime(y.StartTime), token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTime(Convert.ToDateTime(y.EndTime), token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                list.Available = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.AVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? CommonMethods.ConvertFromUtcTime(Convert.ToDateTime(y.Date), token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTime(Convert.ToDateTime(y.StartTime), token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTime(Convert.ToDateTime(y.EndTime), token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                list.Unavailable = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.UNAVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? CommonMethods.ConvertFromUtcTime(Convert.ToDateTime(y.Date), token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTime(Convert.ToDateTime(y.StartTime), token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTime(Convert.ToDateTime(y.EndTime), token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                if (isLeaveNeeded)
                    list.Unavailable.AddRange(_staffAvailabilityRepository.GetAllDatesForLeaveDateRange<AvailabilityStatusModel>(staffID, null, null).ToList());

                if (list.Days.Count() > 0 || list.Available.Count() > 0 || list.Unavailable.Count() > 0)
                {
                    response = new JsonModel(list, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);//(Status Ok)
                }
                return response;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public JsonModel SaveStaffAvailabiltyWithLocation(AvailabilityModel entity, TokenModel token)
        {
            using (var transaction = _context.Database.BeginTransaction()) //TO DO do this with SP
            {
                try
                {
                    List<Entity.StaffAvailability> dbStaffAvailabilityInsertList = new List<Entity.StaffAvailability>();
                    List<Entity.StaffAvailability> dbStaffAvailabilityUpdateList = new List<Entity.StaffAvailability>();
                    List<int> availabilityIds = new List<int>();
                    Location location = new Location();
                    decimal daylightOffset = 0;
                    decimal standardOffset = 0;

                    //Days ids
                    entity.Days.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //Avaliable ids
                    entity.Available.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //Unavaliable ids
                    entity.Unavailable.ForEach(a => { if (a.Id > 0) availabilityIds.Add(a.Id); });

                    //for update
                    dbStaffAvailabilityUpdateList = _staffAvailabilityRepository.GetAll(a => availabilityIds.Contains(a.Id)).ToList();  //_context.StaffAvailability.Where(a => availabilityIds.Contains(a.Id)).ToList();

                    location = _locationRepository.GetByID(entity.LocationId);
                    if (location != null)
                    {
                        daylightOffset = (((decimal)location.DaylightSavingTime) * 60);
                        standardOffset = (((decimal)location.StandardTime) * 60);
                    }

                    foreach (var day in entity.Days)
                    {
                        if (day.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == day.Id)
                                {
                                    a.IsDeleted = day.IsDeleted;
                                    a.DayId = day.DayId;
                                    a.StartTime = (day.StartTime != null && a.StartTime != null && a.StartTime != day.StartTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)day.StartTime, daylightOffset, standardOffset) : a.StartTime);
                                    a.EndTime = (day.EndTime != null && a.EndTime != null && a.EndTime != day.EndTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)day.EndTime, daylightOffset, standardOffset) : a.EndTime);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = day.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.StartTime = (day.StartTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)day.StartTime, daylightOffset, standardOffset) : (DateTime?)null);
                            staffAvailability.EndTime = (day.EndTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)day.EndTime, daylightOffset, standardOffset) : (DateTime?)null);
                            staffAvailability.DayId = day.DayId;
                            staffAvailability.StaffAvailabilityTypeID = day.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = day.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    foreach (var avaliable in entity.Available)
                    {
                        if (avaliable.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == avaliable.Id)
                                {
                                    a.IsDeleted = avaliable.IsDeleted;
                                    a.Date = (avaliable.Date != null) ? avaliable.Date : (DateTime?)null;  // (CommonMethods.ConvertUtcTime((DateTime)avaliable.Date.Value.AddHours(avaliable.StartTime.Value.Hour).AddMinutes(avaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                                    a.StartTime = (avaliable.StartTime != null && a.StartTime != null && a.StartTime != avaliable.StartTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)avaliable.StartTime, daylightOffset, standardOffset) : a.StartTime);
                                    a.EndTime = (avaliable.EndTime != null && a.EndTime != null && a.EndTime != avaliable.EndTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)avaliable.EndTime, daylightOffset, standardOffset) : a.EndTime);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = avaliable.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.Date = (avaliable.Date != null) ? avaliable.Date : (DateTime?)null;  // (CommonMethods.ConvertUtcTime((DateTime)avaliable.Date.Value.AddHours(avaliable.StartTime.Value.Hour).AddMinutes(avaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                            staffAvailability.StartTime = (avaliable.StartTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)avaliable.StartTime, daylightOffset, standardOffset) : (DateTime?)null);
                            staffAvailability.EndTime = (avaliable.EndTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)avaliable.EndTime, daylightOffset, standardOffset) : (DateTime?)null);
                            staffAvailability.StaffAvailabilityTypeID = avaliable.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = avaliable.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    foreach (var unavaliable in entity.Unavailable)
                    {
                        if (unavaliable.Id > 0)
                        {
                            dbStaffAvailabilityUpdateList.ForEach(a =>
                            {
                                if (a.Id == unavaliable.Id)
                                {
                                    a.IsDeleted = unavaliable.IsDeleted;
                                    a.Date = (unavaliable.Date != null) ? unavaliable.Date : null; // (CommonMethods.ConvertUtcTime((DateTime)unavaliable.Date.Value.AddHours(unavaliable.StartTime.Value.Hour).AddMinutes(unavaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                                    a.StartTime = (unavaliable.StartTime != null && a.StartTime != null && a.StartTime != unavaliable.StartTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)unavaliable.StartTime, daylightOffset, standardOffset) : a.StartTime);
                                    a.EndTime = (unavaliable.EndTime != null && a.EndTime != null && a.EndTime != unavaliable.EndTime ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)unavaliable.EndTime, daylightOffset, standardOffset) : a.EndTime);
                                    a.UpdatedBy = token.UserID;
                                    a.UpdatedDate = DateTime.UtcNow;
                                    a.LocationId = unavaliable.LocationId;
                                }
                            });
                        }
                        else
                        {
                            Entity.StaffAvailability staffAvailability = new Entity.StaffAvailability();
                            staffAvailability.IsActive = true;
                            staffAvailability.IsDeleted = false;
                            staffAvailability.CreatedBy = token.UserID;
                            staffAvailability.CreatedDate = DateTime.UtcNow;
                            staffAvailability.Date = (unavaliable.Date != null) ? unavaliable.Date : null; // (CommonMethods.ConvertUtcTime((DateTime)unavaliable.Date.Value.AddHours(unavaliable.StartTime.Value.Hour).AddMinutes(unavaliable.StartTime.Value.Minute), token.Timezone)).Date : (DateTime?)null);
                            staffAvailability.StartTime = (unavaliable.StartTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)unavaliable.StartTime, daylightOffset, standardOffset) : (DateTime?)null);
                            staffAvailability.EndTime = (unavaliable.EndTime != null ? CommonMethods.ConvertToUtcTimeWithOffset((DateTime)unavaliable.EndTime, daylightOffset, standardOffset) : (DateTime?)null);
                            staffAvailability.StaffAvailabilityTypeID = unavaliable.StaffAvailabilityTypeID;
                            staffAvailability.StaffID = entity.StaffID;
                            staffAvailability.LocationId = unavaliable.LocationId;
                            dbStaffAvailabilityInsertList.Add(staffAvailability);
                        }
                    }

                    if (dbStaffAvailabilityInsertList.Count > 0)
                        _staffAvailabilityRepository.Create(dbStaffAvailabilityInsertList.ToArray());

                    if (dbStaffAvailabilityUpdateList.Count > 0)
                        _staffAvailabilityRepository.Update(dbStaffAvailabilityUpdateList.ToArray());

                    //save changes
                    _staffAvailabilityRepository.SaveChanges();

                    //transaction commit
                    transaction.Commit();

                    //Response.StatusCode = (int)HttpStatusCodes.OK;//(Status Ok)
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.SavedStaffAvailability,
                        StatusCode = (int)HttpStatusCodes.OK//(Status Ok)
                    };
                }
                catch (Exception ex)
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

        public JsonModel GetStaffAvailabilityWithLocation(string staffID, int locationId, bool isLeaveNeeded, TokenModel token)
        {
            try
            {
                AvailabilityModel list = new AvailabilityModel();
                Location location = new Location();
                decimal daylightOffset = 0;
                decimal standardOffset = 0;

                location = _locationRepository.GetByID(locationId);
                if (location != null)
                {
                    daylightOffset = (((decimal)location.DaylightSavingTime) * 60);
                    standardOffset = (((decimal)location.StandardTime) * 60);
                }

                List<int> staffIds = staffID.Split(',').Select(Int32.Parse).ToList();
                IQueryable<Entity.StaffAvailability> dbList = _staffAvailabilityRepository.GetAll(a => staffIds.Contains(a.StaffID) && a.LocationId == locationId && a.IsDeleted == false && a.IsActive == true).AsQueryable();
                IQueryable<MasterWeekDays> weekDays = _context.MasterWeekDays.Where(a => a.OrganizationID == token.OrganizationID).AsQueryable();
                //list.StaffID = staffID;
                list.LocationId = locationId;
               
                list.Days = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.WEEKDAY.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new DayModel()
                {
                    Id = y.Id,
                    DayId = y.DayId,
                    DayName = weekDays.Where(z => z.Id == y.DayId).FirstOrDefault().Day,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                list.Available = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.AVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                list.Unavailable = dbList.Where(a => a.StaffAvailabilityType.GlobalCodeName.ToUpper() == StaffAvailabilityEnum.UNAVAILABLE.ToString() && a.StaffAvailabilityType.OrganizationID == token.OrganizationID).OrderBy(a => a.StaffID).Select(y => new AvailabilityStatusModel()
                {
                    Id = y.Id,
                    Date = y.Date != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Date), daylightOffset, standardOffset, token) : (DateTime?)null,
                    StartTime = y.StartTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.StartTime), daylightOffset, standardOffset, token) : (DateTime?)null,
                    EndTime = y.EndTime != null ? CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.EndTime), daylightOffset, standardOffset, token) : (DateTime?)null,
                    StaffAvailabilityTypeID = y.StaffAvailabilityTypeID,
                    StaffID = y.StaffID,
                    IsDeleted = y.IsDeleted,
                    LocationId = y.LocationId
                }).ToList();
                if (isLeaveNeeded)
                    list.Unavailable.AddRange(_staffAvailabilityRepository.GetAllDatesForLeaveDateRange<AvailabilityStatusModel>(staffID, null, null).ToList());
                
                if (list.Days.Count() > 0 || list.Available.Count() > 0 || list.Unavailable.Count() > 0)
                {
                    response = new JsonModel(list, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);//(Status Ok)
                }
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
