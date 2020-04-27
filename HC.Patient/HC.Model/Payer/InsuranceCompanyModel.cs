using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Payer
{
    public class InsuranceCompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InsuranceTypeId { get; set; }
        public string InsType { get; set; } //Insurance type
        public string Address { get; set; }
        public string City { get; set; }
        public int StateID { get; set; }
        public int? CountryID { get; set; }
        public string Fax { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CarrierPayerID { get; set; }
        public bool DayClubByProvider { get; set; }
        public string TPLCode { get; set; }
        public string InsOthers { get; set; }
        public bool IsEDIPayer { get; set; }
        public bool IsPractitionerIsRenderingProvider { get; set; }
        public int Form1500PrintFormat { get; set; }
        public string AdditionalClaimInfo { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ApartmentNumber { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalRecords { get; set; }
    }
}
