using HC.Model;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.EDI
{
    public interface IEDI271ParserService:IBaseService
    {
        JsonModel ReadEDI271(TokenModel token);
    }
}
