using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Image;
using HC.Patient.Model.PatientEncounters;
using HC.Patient.Service.IServices.Images;
using HC.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace HC.Patient.Service.Services.Images
{
    public class ImageService : BaseService, IImageService
    {
        /// <summary>
        /// it will save organization's logo and favicon images
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public string SaveImages(string base64String,string directory, string folderName)
        {
            //get current directory root
            string webRootPath = Directory.GetCurrentDirectory();

            //add your custom path
            webRootPath = webRootPath + directory + folderName;

            //check 
            if (!Directory.Exists(webRootPath))
            {
                Directory.CreateDirectory(webRootPath);
            }

            if (!string.IsNullOrEmpty(base64String))
            {
                //getting data from base64 url
                var base64Data = Regex.Match(base64String, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                //getting extension of the image
                string extension = CommonMethods.GetExtenstion(Regex.Match(base64String, @"data:(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0]);

                //file name
                string fileName = folderName + "_" + DateTime.UtcNow.TimeOfDay.ToString();

                //update file name remove unsupported attr.
                fileName = fileName.Replace(" ", "_").Replace(":", "_");

                //add extension
                if (!extension.StartsWith("."))
                {
                    extension = "." + extension;
                }
                fileName = fileName + extension;

                //create path for save location
                string path = webRootPath + "/" + fileName;

                //convert files into base
                byte[] bytes = Convert.FromBase64String(base64Data);

                TypeImageModel typeImageModel = new TypeImageModel();
                typeImageModel.Type = folderName;
                typeImageModel.Url = path;
                typeImageModel.Bytes = bytes;

                ////save into the directory
                CommonMethods.SaveImages(typeImageModel);
                //File.WriteAllBytes(path, bytes);

                return fileName;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Save patient images
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commonMethods"></param>
        /// <returns></returns>
        public Patients ConvertBase64ToImage(Patients entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(entity.PhotoBase64))
                {
                    string webRootPath = "";

                    //get root path
                    webRootPath = Directory.GetCurrentDirectory();

                    //getting data from base64 url
                    var base64Data = Regex.Match(entity.PhotoBase64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                    //getting extension of the image
                    string extension = Regex.Match(entity.PhotoBase64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];

                    extension = "." + extension;

                    if (!Directory.Exists(webRootPath + ImagesPath.PatientPhotos))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientPhotos);
                    }
                    if (!Directory.Exists(webRootPath + ImagesPath.PatientThumbPhotos))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientThumbPhotos);
                    }

                    string picName = Guid.NewGuid().ToString();

                    List<ImageModel> obj = new List<ImageModel>();

                    ImageModel img = new ImageModel();

                    img.Base64 = base64Data;
                    img.ImageUrl = webRootPath + ImagesPath.PatientPhotos + picName + extension;
                    img.ThumbImageUrl = webRootPath + ImagesPath.PatientThumbPhotos + picName + extension;
                    obj.Add(img);

                    CommonMethods.SaveImageAndThumb(obj);

                    entity.PhotoPath = picName + extension;
                    entity.PhotoThumbnailPath = picName + extension;

                }
                else if (string.IsNullOrEmpty(entity.PhotoPath) && string.IsNullOrEmpty(entity.PhotoThumbnailPath))
                {
                    entity.PhotoPath = string.Empty;
                    entity.PhotoThumbnailPath = string.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }

        /// <summary>
        /// Save user(staff) images
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commonMethods"></param>
        /// <returns></returns>
        public Staffs ConvertBase64ToImageForUser(Staffs entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(entity.PhotoBase64))
                {
                    string webRootPath = "";

                    //get root path
                    webRootPath = Directory.GetCurrentDirectory();

                    //getting data from base64 url
                    var base64Data = Regex.Match(entity.PhotoBase64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                    //getting extension of the image
                    string extension = Regex.Match(entity.PhotoBase64, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];

                    extension = "." + extension;

                    if (!Directory.Exists(webRootPath + ImagesPath.StaffPhotos))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.StaffPhotos);
                    }
                    if (!Directory.Exists(webRootPath + ImagesPath.StaffThumbPhotos))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.StaffThumbPhotos);
                    }

                    string picName = Guid.NewGuid().ToString();

                    List<ImageModel> obj = new List<ImageModel>();

                    ImageModel img = new ImageModel();

                    img.Base64 = base64Data;
                    img.ImageUrl = webRootPath + ImagesPath.StaffPhotos + picName + extension;
                    img.ThumbImageUrl = webRootPath + ImagesPath.StaffThumbPhotos + picName + extension;
                    obj.Add(img);

                    CommonMethods.SaveImageAndThumb(obj);

                    entity.PhotoPath = picName + extension;
                    entity.PhotoThumbnailPath = picName + extension;

                }
                else if (string.IsNullOrEmpty(entity.PhotoPath) && string.IsNullOrEmpty(entity.PhotoThumbnailPath))
                {
                    entity.PhotoPath = string.Empty;
                    entity.PhotoThumbnailPath = string.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }

        /// <summary>
        /// Save patient insurance detail images
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="commonMethods"></param>
        public PatientInsuranceDetails ConvertBase64ToImageForInsurance(PatientInsuranceDetails entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(entity.Base64Front))
                {
                    List<ImageModel> obj = new List<ImageModel>();

                    string webRootPath = "";

                    //get root path
                    webRootPath = Directory.GetCurrentDirectory();

                    //getting data from base64 url
                    var base64Data = Regex.Match(entity.Base64Front, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                    //getting extension of the image
                    string extension = Regex.Match(entity.Base64Front, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];

                    extension = "." + extension;

                    if (!Directory.Exists(webRootPath + ImagesPath.PatientInsuranceFront))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientInsuranceFront);
                    }
                    if (!Directory.Exists(webRootPath + ImagesPath.PatientInsuranceFrontThumb))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientInsuranceFrontThumb);
                    }

                    string picName = Guid.NewGuid().ToString();

                    

                    ImageModel img = new ImageModel();

                    img.Base64 = base64Data;
                    img.ImageUrl = webRootPath + ImagesPath.PatientInsuranceFront + picName + extension;                    
                    img.ThumbImageUrl = webRootPath + ImagesPath.PatientInsuranceFrontThumb + "thumb_"+picName + extension;
                    obj.Add(img);

                    CommonMethods.SaveImageAndThumb(obj);

                    entity.InsurancePhotoPathFront = picName + extension;
                    entity.InsurancePhotoPathThumbFront = "thumb_" + picName + extension;

                }
                else if (string.IsNullOrEmpty(entity.InsurancePhotoPathFront) && string.IsNullOrEmpty(entity.InsurancePhotoPathThumbFront))
                {
                    entity.InsurancePhotoPathFront = string.Empty;
                    entity.InsurancePhotoPathThumbFront = string.Empty;
                }

                if (!string.IsNullOrEmpty(entity.Base64Back))
                {
                    List<ImageModel> obj = new List<ImageModel>();

                    string webRootPath = "";

                    //get root path
                    webRootPath = Directory.GetCurrentDirectory();

                    //getting data from base64 url
                    var base64Data = Regex.Match(entity.Base64Back, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                    //getting extension of the image
                    string extension = Regex.Match(entity.Base64Back, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Split(';')[0];

                    extension = "." + extension;

                    if (!Directory.Exists(webRootPath + ImagesPath.PatientInsuranceBack))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientInsuranceBack);
                    }
                    if (!Directory.Exists(webRootPath + ImagesPath.PatientInsuranceBackThumb))
                    {
                        Directory.CreateDirectory(webRootPath + ImagesPath.PatientInsuranceBackThumb);
                    }

                    string picName = Guid.NewGuid().ToString();

                    ImageModel img = new ImageModel();

                    img.Base64 = base64Data;
                    img.ImageUrl = webRootPath + ImagesPath.PatientInsuranceBack + picName + extension;
                    img.ThumbImageUrl = webRootPath + ImagesPath.PatientInsuranceBackThumb + "thumb_" + picName + extension;
                    obj.Add(img);

                    CommonMethods.SaveImageAndThumb(obj);

                    entity.InsurancePhotoPathBack = picName + extension;
                    entity.InsurancePhotoPathThumbBack = "thumb_" + picName + extension;

                }
                else if (string.IsNullOrEmpty(entity.InsurancePhotoPathBack) && string.IsNullOrEmpty(entity.InsurancePhotoPathThumbBack))
                {
                    entity.InsurancePhotoPathBack     = string.Empty;
                    entity.InsurancePhotoPathThumbBack = string.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }
    }
}
