using HC.Model;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.EDI
{
    public interface IEDI999ParserService : IBaseService
    {
        JsonModel ReadEDI999(TokenModel token);
    }
}
