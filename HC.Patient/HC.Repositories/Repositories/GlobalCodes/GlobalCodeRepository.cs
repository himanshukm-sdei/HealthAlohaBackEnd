using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.GlobalCodes;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.GlobalCodes
{
    public class GlobalCodeRepository : RepositoryBase<GlobalCode>, IGlobalCodeRepository
    {
        private HCOrganizationContext _context;
        public GlobalCodeRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
