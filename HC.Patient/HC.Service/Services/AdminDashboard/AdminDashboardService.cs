using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Model.Organizations;
using HC.Patient.Repositories.IRepositories.AdminDashboard;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.AdminDashboard;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.AdminDashboard
{
    public class AdminDashboardService : BaseService, IAdminDashboardService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly HCOrganizationContext _context;
        public AdminDashboardService(IPatientRepository patientRepository, IAdminDashboardRepository adminDashboardRepository, HCOrganizationContext context)
        {
            _patientRepository = patientRepository;
            _adminDashboardRepository = adminDashboardRepository;
            _context = context;
        }
        public JsonModel GetTotalClientCount(TokenModel token)
        {
            try
            {
                IQueryable<int> TotalCount = _patientRepository.GetAll(a => a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).Select(a => a.Id);
                OrganizationTotalClientsModel orgClientCount = new OrganizationTotalClientsModel();
                orgClientCount.TotalClientCount = TotalCount.Count();
                return new JsonModel()
                {
                    data = orgClientCount,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
        public JsonModel GetTotalRevenue(TokenModel token)
        {
            try
            {
                OrganizationTotalRevenuModel TotalCount = _adminDashboardRepository.GetTotalRevenue<OrganizationTotalRevenuModel>(token).FirstOrDefault();
                return new JsonModel()
                {
                    data = TotalCount,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
        public JsonModel GetOrganizationAuthorization(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token)
        {
            try
            {
                List<OrganizationAuthorizationModel> organizationAuthorizationModels = _adminDashboardRepository.GetOrganizationAuthorization<OrganizationAuthorizationModel>(pageNumber, pageSize, sortColumn, sortOrder, token).ToList();
                return new JsonModel()
                {
                    data = organizationAuthorizationModels,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = organizationAuthorizationModels != null && organizationAuthorizationModels.Count > 0 ? organizationAuthorizationModels[0].TotalRecords : 0,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal((organizationAuthorizationModels != null && organizationAuthorizationModels.Count > 0 ? organizationAuthorizationModels[0].TotalRecords : 0) / pageSize))
                    },
                    StatusCode = (int)HttpStatusCodes.OK
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
        public JsonModel GetOrganizationEncounter(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token)
        {
            try
            {
                List<OrganizationEncounterModel> organizationEncounterModels = _adminDashboardRepository.GetOrganizationEncounter<OrganizationEncounterModel>(pageNumber, pageSize, sortColumn, sortOrder, token).ToList();
                if (organizationEncounterModels.Count > 0)
                    return new JsonModel()
                    {
                        data = organizationEncounterModels,
                        Message = StatusMessage.FetchMessage,
                        meta = new Meta()
                        {
                            TotalRecords = organizationEncounterModels[0].TotalRecords,
                            CurrentPage = pageNumber,
                            PageSize = pageSize,
                            DefaultPageSize = pageSize,
                            TotalPages = Math.Ceiling(Convert.ToDecimal(organizationEncounterModels[0].TotalRecords / pageSize))
                        },
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                else
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

        public JsonModel GetStaffEncounter(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token)
        {
            try
            {
                List<OrganizationEncounterModel> organizationEncounterModels = _adminDashboardRepository.GetStaffEncounter<OrganizationEncounterModel>(pageNumber, pageSize, sortColumn, sortOrder, token).ToList();
                if (organizationEncounterModels != null && organizationEncounterModels.Count > 0)
                    return new JsonModel()
                    {
                        data = organizationEncounterModels,
                        Message = StatusMessage.FetchMessage,
                        meta = new Meta()
                        {
                            TotalRecords = organizationEncounterModels[0].TotalRecords,
                            CurrentPage = pageNumber,
                            PageSize = pageSize,
                            DefaultPageSize = pageSize,
                            TotalPages = Math.Ceiling(Convert.ToDecimal(organizationEncounterModels[0].TotalRecords / pageSize))
                        },
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                else
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


        public JsonModel GetRegiesteredClientCount(TokenModel token)
        {
            try
            {
                List<OrganizationRegiesteredClientCountModel> orgRegClientCount = _adminDashboardRepository.GetRegiesteredClientCount<OrganizationRegiesteredClientCountModel>(token).ToList();
                if (orgRegClientCount.Count > 0)
                    return new JsonModel()
                    {
                        data = orgRegClientCount,
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                else
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound
                    };
            }
            catch (Exception)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
        public JsonModel GetAdminDashboardData(TokenModel token)
        {
            try
            {
                OrganizationDashboardModel organizationDashoabrd = new OrganizationDashboardModel();

                #region  Total Client count
                IQueryable<int> TotalCount = _patientRepository.GetAll(a => a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).Select(a => a.Id);
                organizationDashoabrd.OrganizationTotalClients = TotalCount.Count();
                #endregion region

                #region Organization Total Revenu
                organizationDashoabrd.OrganizationTotalRevenu = _adminDashboardRepository.GetTotalRevenue<OrganizationTotalRevenuModel>(token).FirstOrDefault();
                #endregion

                //#region Organization Authorization
                //organizationDashoabrd.OrganizationAuthorization = _adminDashboardRepository.GetOrganizationAuthorization<OrganizationAuthorizationModel>(1, 10, "", "", token).ToList();
                //#endregion

                //#region Organization Encounters
                //organizationDashoabrd.OrganizationEncounter = _adminDashboardRepository.GetOrganizationEncounter<OrganizationEncounterModel>(1, 10, "", "", token).ToList();
                //#endregion

                #region Online Users
                //To-Do static values                 
                //organizationDashoabrd.OnlineUser = _context.User.Join(_context.Patients, U => U.Id, P => P.UserID, (U, P) => new { U, P }).Where(z => z.P.IsDeleted == false && z.P.IsActive == true && z.U.IsOnline == true).Select(x => x.U.Id).Count();
                organizationDashoabrd.OnlineUser = _context.User.Where(z => z.IsDeleted == false && z.IsActive == true && z.IsOnline == true && z.UserRoles.UserType.ToLower() != UserTypeEnum.ADMIN.ToString().ToLower()).Select(x => x.Id).Count();
                #endregion

                #region Get Client Status Chart
                organizationDashoabrd.ClientStatusChart = _adminDashboardRepository.GetClientStatusChart<ClientStatusChartModel>(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow, token).ToList();
                #endregion

                return new JsonModel()
                {
                    data = organizationDashoabrd,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
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



        #region Angular Code APIs

        public JsonModel GetClientStatusChart(TokenModel token)
        {
            try
            {
                List<ClientStatusChartModel> clientStatusChart = _adminDashboardRepository.GetClientStatusChart<ClientStatusChartModel>(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow, token).ToList();
                return new JsonModel()
                {
                    data = clientStatusChart,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }

        public JsonModel GetDashboardBasicInfo(TokenModel token)
        {
            try
            {
                OrganizationDashboardModel organizationDashoabrd = new OrganizationDashboardModel();

                #region  Total Client count
                IQueryable<int> TotalCount = _patientRepository.GetAll(a => a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).Select(a => a.Id);
                organizationDashoabrd.OrganizationTotalClients = TotalCount.Count();
                #endregion region

                #region Organization Total Revenu
                organizationDashoabrd.OrganizationTotalRevenu = _adminDashboardRepository.GetTotalRevenue<OrganizationTotalRevenuModel>(token).FirstOrDefault();
                #endregion

                //#region Organization Authorization
                //organizationDashoabrd.OrganizationAuthorization = _adminDashboardRepository.GetOrganizationAuthorization<OrganizationAuthorizationModel>(1, 10, "", "", token).ToList();
                //#endregion

                //#region Organization Encounters
                //organizationDashoabrd.OrganizationEncounter = _adminDashboardRepository.GetOrganizationEncounter<OrganizationEncounterModel>(1, 10, "", "", token).ToList();
                //#endregion

                #region Online Users
                //To-Do static values                 
                //organizationDashoabrd.OnlineUser = _context.User.Join(_context.Patients, U => U.Id, P => P.UserID, (U, P) => new { U, P }).Where(z => z.P.IsDeleted == false && z.P.IsActive == true && z.U.IsOnline == true).Select(x => x.U.Id).Count();
                organizationDashoabrd.OnlineUser = _context.User.Where(z => z.IsDeleted == false && z.OrganizationID==token.OrganizationID && z.IsActive == true && z.IsOnline == true && z.UserRoles.UserType.ToLower() != UserTypeEnum.ADMIN.ToString().ToLower()).Select(x => x.Id).Count();
                #endregion

                //#region Get Client Status Chart
                //organizationDashoabrd.ClientStatusChart = _adminDashboardRepository.GetClientStatusChart<ClientStatusChartModel>(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow, token).ToList();
                //#endregion

                return new JsonModel()
                {
                    data = organizationDashoabrd,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
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
        #endregion
    }
}
