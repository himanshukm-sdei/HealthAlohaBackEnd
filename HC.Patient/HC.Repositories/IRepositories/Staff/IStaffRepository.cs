using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Staff;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Staff
{
    public interface IStaffRepository : IRepositoryBase<Staffs>
    {
        IQueryable<T> GetStaffByTags<T>(ListingFiltterModel listingFiltterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetStaff<T>(ListingFiltterModel listingFiltterModel, TokenModel token) where T : class, new();
        StaffProfileModel GetStaffProfileData(int staffId, TokenModel token);
        IQueryable<T> GetAssignedLocationsById<T>(int staffId, TokenModel token) where T : class, new();
        IQueryable<T> GetStaffHeaderData<T>(int staffId, TokenModel tokenModel) where T : class, new();
    }
}
