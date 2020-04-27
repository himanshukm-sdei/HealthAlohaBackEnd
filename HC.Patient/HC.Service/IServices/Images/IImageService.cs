using HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.Images
{
    public interface IImageService :IBaseService
    {
        string SaveImages(string base64String, string directory, string folderName);
        Patients ConvertBase64ToImage(Patients entity);
        Staffs ConvertBase64ToImageForUser(Staffs entity);
        PatientInsuranceDetails ConvertBase64ToImageForInsurance(PatientInsuranceDetails entity);
    }
}
