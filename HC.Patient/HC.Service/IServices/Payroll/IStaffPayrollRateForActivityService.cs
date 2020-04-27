using HC.Model;
using HC.Patient.Model.Payroll;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.Payroll
{
    public interface IStaffPayrollRateForActivityService : IBaseService
    {
        JsonModel SaveUpdateStaffPayrollRateForActivity(List<StaffPayrollRateForActivityModel> staffPayrollRateForActivityModels, TokenModel tokenModel);
        JsonModel GetStaffPayRateOfActivity(SearchFilterModel searchFilterModel, TokenModel tokenModel);
    }
}
