using HC.Model;
using HC.Patient.Model.SecurityQuestion;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface IMasterSecurityQuestionService :IBaseService
    {
        JsonModel GetSecurityQuestions(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SaveSecurityQuestions(MasterSecurityQuestionModel masterSecurityQuestionModel, TokenModel tokenModel);
        JsonModel GetSecurityQuestionsById(int id, TokenModel tokenModel);
        JsonModel DeleteSecurityQuestions(int id, TokenModel tokenModel);
    }
}
