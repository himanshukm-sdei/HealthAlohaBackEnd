using HC.Model;
using HC.Patient.Repositories.IRepositories.GlobalCodes;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.Services.GlobalCodes
{
    public class GlobalCodeService :BaseService, IGlobalCodeService
    {
        private readonly IGlobalCodeRepository _globalCodeRepository;
        public GlobalCodeService(IGlobalCodeRepository globalCodeRepository)
        {
            _globalCodeRepository = globalCodeRepository;
        }

        public int GetGlobalCodeValueId(string globalCodeCategoryName, string globalCodeValue,TokenModel token)
        {   
            return _globalCodeRepository.Get(a => a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == globalCodeCategoryName.ToLower() && a.GlobalCodeValue.ToLower() == globalCodeValue.ToLower() && a.OrganizationID==token.OrganizationID && a.IsActive==true && a.IsDeleted==false).Id;
        }
    }
}
