using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Model
{
    public class FilterModel
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = string.Empty;
        public string sortOrder { get; set; } = string.Empty;
    }

    public class ListingFiltterModel : FilterModel
    {
        public string SearchKey { get; set; } = string.Empty;
        public string StartWith { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string LocationIDs { get; set; } = string.Empty;
        public string IsActive { get; set; }
        public string RoleIds { get; set; } = string.Empty;
    }
    public class PatientPayerServiceCodeFilterModel
    {
        public int PatientId { get; set; }
        public string PayerPreference { get; set; }
        public DateTime? Date { get; set; }
        public int PayerId { get; set; }
        public int PatientInsuranceId { get; set; }
    }

    public class SearchFilterModel : FilterModel
    {
        public int StaffId { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public int PayerId { get; set; } = 0;
        public int ActivityId { get; set; } = 0;
        public int AuthorizationId { get; set; } = 0;

    }
    public class PatientGuartdianFilterModel : FilterModel
    {
        public int PatientId { get; set; } = 0;
        public string SearchKey { get; set; } = string.Empty;
    }
    public class PatientFilterModel: SearchFilterModel
    {
        public int PatientId { get; set; } = 0;
    }
    public class CategoryCodesFilterModel : FilterModel
    {
        public int CategoryId { get; set; } = 0;
        public string SearchText { get; set; } = string.Empty;
    }
    public class CommonFilterModel : FilterModel
    {   
        public string SearchText { get; set; } = string.Empty;
    }
    public class SectionFilterModel : FilterModel
    {
        public int DocumentId { get; set; } = 0;
    }
    public class PatientDocumentAnswerFilterModel
    {
        public int DocumentId { get; set; } = 0;
        public int PatientId { get; set; } = 0;
    }
    public class PatientDocumentFilterModel: FilterModel
    {
        public int PatientId { get; set; } = 0;
        public int Status { get; set; } = 0;
        public int DocumentId { get; set; } = 0;
    }
    public class CheckListFilterModel : FilterModel
    {
        public string Type { get; set; }
        public string SearchText { get; set; } = string.Empty;
      public string CategoryName { get; set; }
    }

    public class VideoAndArticlesCategoriesFilterModel : FilterModel
    {
        public string SearchText { get; set; } = string.Empty;
        public string Description { get; set; }
        public int ID { get; set; } = 0;
    }
  public class MasterSectionFilterModel : FilterModel
    {
        public int DocumentId { get; set; } = 0;
    }
}

