using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Organizations
{
    public class OrganizationModel
    {
        public int Id { get; set; }        
        public string OrganizationName { get; set; }
        public string BusinessName { get; set; }
        public string Description { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int? StateID { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public int? CountryID { get; set; }
        public string CountryName { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string LogoBase64 { get; set; }
        public string Logo { get; set; }
        public string Favicon { get; set; }
        public string FaviconBase64 { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonMiddleName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string ContactPersonPhoneNumber { get; set; }        
        public int? ContactPersonMaritalStatus { get; set; }
        public int? ContactPersonGender { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DatabaseDetailId { get; set; }
        public string DatabaseName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ApartmentNumber { get; set; }
        public string VendorIdDirect { get; set; }        
        public string VendorIdIndirect { get; set; }        
        public string VendorNameDirect { get; set; }        
        public string VendorNameIndirect { get; set; }
        public string PayrollStartWeekDay { get; set; }
        public string PayrollEndWeekDay { get; set; }
        public List<OrganizationSubscriptionPlanModel> OrganizationSubscriptionPlans { get; set; }
        public List<OrganizationSMTPDetailsModel> OrganizationSMTPDetail { get; set; }
    }
    public class OrganizationSubscriptionPlanModel
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public DateTime StartDate { get; set; }
        public int PlanType { get; set; }
        public decimal AmountPerClient { get; set; }
        public int TotalNumberOfClients { get; set; }
        public int OrganizationID { get; set; }
        public bool IsDeleted { get; set; }

    }
    public class OrganizationSMTPDetailsModel
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string Port { get; set; }
        public string ConnectionSecurity { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public int OrganizationID { get; set; }
        public bool IsDeleted { get; set; }
    }
    // ViewModel to get all the details of any particular Agency/Organization
    public class OrganizationDetailModel
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; }
        public string BusinessName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int? CountryID { get; set; }
        public string CountryName { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string LogoBase64 { get; set; }
        public string Logo { get; set; }
        public string Favicon { get; set; }
        public string FaviconBase64 { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhoneNumber { get; set; }
        public string UserName { get; set; }
        public int DatabaseDetailId { get; set; }
        public string DatabaseName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string ApartmentNumber { get; set; }
        // public string VendorIdDirect { get; set; }
        // public string VendorIdIndirect { get; set; }
        // public string VendorNameDirect { get; set; }
        // public string VendorNameIndirect { get; set; }
        public string PayrollStartWeekDay { get; set; }
        public string PayrollEndWeekDay { get; set; }
        public string PlanName { get; set; }
        public DateTime StartDate { get; set; }
        public int PlanType { get; set; }
        public decimal AmountPerClient { get; set; }
        public int TotalNumberOfClients { get; set; }
        public string ServerName { get; set; }
        public string Port { get; set; }
        public string SMTPUserName { get; set; }
    }
    public class OrganizationDatabaseDetailModel
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class MasterOrganizationModel
    {
        public int OrganizationID { get; set; }
        public string BusinessName { get; set; }
        public string OrganizationName { get; set; }
        public string Address { get; set; }
        public decimal TotalRecords { get; set; }
    }

    #region Admin Dashboard model
    public class OrganizationDashboardModel
    {
        public int OrganizationTotalClients { get; set; }
        public int OnlineUser { get; set; }
        public OrganizationTotalRevenuModel OrganizationTotalRevenu { get; set; }        
        //public List<OrganizationAuthorizationModel> OrganizationAuthorization { get; set; }
        //public List<OrganizationEncounterModel> OrganizationEncounter { get; set; }
        public List<ClientStatusChartModel> ClientStatusChart { get; set; }
    }
    public class OrganizationTotalRevenuModel
    {
        public decimal TotalRevenue { get; set; }
    }
    public class OrganizationTotalClientsModel
    {
        public int TotalClientCount { get; set; }
    }
    public class OrganizationAuthorizationModel
    {
        public int AuthorizationId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime EndDate { get; set; }
        public int RemainedUnits { get; set; }
        public decimal TotalRecords { get; set; }
        public string WarningColorCode { get; set; }        
    }
    public class OrganizationEncounterModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string StaffName { get; set; }
        public string ClientName { get; set; }
        public int PatientAppointmentId { get; set; }        
        public string Day { get; set; }
        public DateTime DateOfService { get; set; }
        public string Status { get; set; }
        public decimal TotalRecords { get; set; }
        public bool IsBillableEncounter { get; set; }
        public string LocationName { get; set; }
        public string ServiceType { get; set; }
        public string BillableEncounterColorCode { get; set; }
    }
    public class OrganizationRegiesteredClientCountModel
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }

    public class ClientStatusChartModel
    {
        public DateTime RegisteredDate { get; set; }
        public int Active { get; set; }
        public int Inactive { get; set; }
        public int ActiveClients { get; set; }

    }
    #endregion
}
