using HC.Model;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.GlobalCodes
{
    public interface IGlobalCodeService: IBaseService
    {
        int GetGlobalCodeValueId(string globalCodeCategoryName, string globalCodeValue,TokenModel token);
    }
}
