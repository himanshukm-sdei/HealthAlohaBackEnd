using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.MasterQuestionnaire;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.MasterQuestionnaire
{
    public class MasterQuestionnaireBenchmarkRangeRepository : MasterRepositoryBase<MasterQuestionnaireBenchmarkRange>, IMasterQuestionnaireBenchmarkRangeRepository
    {
        private HCMasterContext _masterContext;
        public MasterQuestionnaireBenchmarkRangeRepository(HCMasterContext masterContext) : base(masterContext)
        {
            this._masterContext = masterContext;
        }
    }
}
