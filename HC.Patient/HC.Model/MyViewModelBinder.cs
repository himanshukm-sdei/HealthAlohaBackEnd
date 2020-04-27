using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.Patient.Model
{
    public class MyViewModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //var day = 0;
            //var month = 0;
            //var year = 0;

            //if (!int.TryParse(bindingContext.ActionContext.HttpContext.Request.Query["day"], out day) ||
            //!int.TryParse(bindingContext.ActionContext.HttpContext.Request.Query["month"], out month) ||
            //!int.TryParse(bindingContext.ActionContext.HttpContext.Request.Query["year"], out year))
            //{
            //    return Task.CompletedTask;
            //}

            var result = new MasterDataModel
            {
                MasterCountry = new List<MasterCountry>(),
                MasterDocumentType= new List<MasterDocumentType>(),
                MasterEthnicity = new List<MasterEthnicity>(),
                MasterFundingSource = new List<MasterFundingSource>(),
                MasterInsurancePCP = new List<MasterInsurancePCP>(),
                MasterInsuranceType = new List<MasterInsuranceType>(),
                MasterOccupation = new List<MasterOccupation>(),
                MasterPreferredLanguage = new List<MasterPreferredLanguage>(),
                MasterRace = new List<MasterRace>(),
                MasterState = new List<MasterState>(),
                MasterStatus = new List<MasterStatus>()
            };

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
