using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Model.Staff;
using HC.Service;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IStaffService : IBaseService
    {
        JsonModel GetStaffByTags(ListingFiltterModel listingFiltterModel, TokenModel tokenModel);
        JsonModel GetStaffs(ListingFiltterModel listingFiltterModel, TokenModel token);
        JsonModel CreateUpdateStaff(Staffs staffs, TokenModel token);
        JsonModel GetStaffById(int id, TokenModel token);
        JsonModel DeleteStaff(int id, TokenModel token);
        JsonModel UpdateStaffActiveStatus(int staffId, bool isActive, TokenModel token);
        JsonModel GetDoctorDetailsFromNPI(string npiNumber,string enumerationType);
        JsonModel GetStaffProfileData(int staffId, TokenModel token);
        JsonModel GetAssignedLocationsById(int id, TokenModel token);
        JsonModel GetStaffHeaderData(int staffId, TokenModel tokenModel);
    }
}
