using HC.Patient.Model.Common;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HC.Patient.Model.MasterQuestionnaire
{
    public class MasterCategoryModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public decimal? PerfectScore { get; set; }
        public int[] HRACategoryRiskIds { get; set; }
        public int? DisplayOrder { get; set; }
        public decimal TotalRecords { get; set; }
    }

    public class MasterCategoryCodeModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public decimal Score { get; set; }
        public string PhotoBase64 { get; set; }
        public string OptionLogo { get; set; }
        public decimal TotalRecords { get; set; }
    }

    //public class MasterQuestionnaireDocumentModel
    //{
    //    public int Id { get; set; }
    //    public string DocumentName { get; set; }
    //    public string Description { get; set; }
    //    public int? DisplayOrder { get; set; }
    //    public decimal TotalRecords { get; set; }
    //}
    public class MasterQuestionnaireDocumentModel
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public int? DocumenttypeId { get; set; }
        public int MasterAssessmentTypeId { get; set; }
        public decimal TotalRecords { get; set; }
        public List<BenchmarkRangeModel> BenchmarkRangeModel { get; set; }
    }
    public class BenchmarkRangeModel
    {
        public int Id { get; set; }
        public int BenchmarkId { get; set; }
        public decimal MinRange { get; set; }
        public decimal MaxRange { get; set; }
        public int QuestionnaireId { get; set; }

    }
    public class MasterQuestionnaireSectionModel
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string SectionName { get; set; }
        public int? DisplayOrder { get; set; }
        public int? HRAGenderCriteria { get; set; }
        public decimal TotalRecords { get; set; }
    }

    public class MasterQuestionnaireSectionItemModel
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int SectionId { get; set; }
        public int Itemtype { get; set; }//type of the question like textbox or radiobutton etc.
        public string ItemLabel { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsMandatory { get; set; }
        public bool? IsNumber { get; set; }
        public string Placeholder { get; set; }
        public string PhotoBase64 { get; set; }
        public string QuestionLogo { get; set; }        
    }



    //public class QuestionnaireSectionModel
    //{
    //    public int Id { get; set; }
    //    public int DocumentId { get; set; }
    //    public string SectionName { get; set; }
    //    public int? DisplayOrder { get; set; }
    //    public decimal TotalRecords { get; set; }
    //}

    //public class MasterQuestionnaireSectionItemModel
    //{
    //    public int Id { get; set; }
    //    public int DocumentId { get; set; }
    //    public int SectionId { get; set; }
    //    public int Itemtype { get; set; }//type of the question like textbox or radiobutton etc.
    //    public string ItemLabel { get; set; }
    //    public Nullable<int> CategoryId { get; set; }
    //    public int? DisplayOrder { get; set; }
    //}


    //#region Section Item model
    //public class SectionItemlistingModel
    //{
    //    public List<SectionItemModel> SectionItems { get; set; }
    //    public List<CodeModel> Codes { get; set; }
    //}

    //public class SectionItemModel
    //{
    //    public int Id { get; set; }
    //    public int CategoryId { get; set; }
    //    public string SectionName { get; set; }
    //    public string InputType { get; set; }
    //    public string Question { get; set; }
    //    public int? DisplayOrder { get; set; }
    //    public decimal TotalRecords { get; set; }
    //}

    //public class CodeModel
    //{
    //    public int Id { get; set; }
    //    public int CategoryId { get; set; }
    //    public string Option { get; set; }        
    //    public int? DisplayOrder { get; set; }
    //}

    //public class SectionItemDDValueModel
    //{
    //    public List<MasterDropDown> SectionItems { get; set; }
    //    public List<MasterDropDown> ControlTypes { get; set; }
    //}
    //#endregion

    //#region Answer
    //public class AnswersModel
    //{
    //    public int PatientID { get; set; } //Document related to patient
    //    public int DocumentId { get; set; } //Answer reted to document
    //    public List<SectionItemModel> SectionItems { get; set; }
    //    public List<CodeModel> Codes { get; set; }
    //    public List<AnswerModel> Answer { get; set; }
    //    public List<DocumentSignatureModel> DocumentSignature { get; set; }
    //    //for parse Answer model to xml
    //    public XElement questionAnswerXML { get; set; }
    //}
    //public class AnswerModel
    //{   
    //    public int Id { get; set; }       
    //    public int SectionItemId { get; set; } //Question Id from section item table
    //    public Nullable<int> AnswerId { get; set; } = 0; //foreign key from DFA_CategoryCode
    //    public string TextAnswer { get; set; }
    //}

    //public class DocumentSignatureModel
    //{
    //    public int PatientDocumentId { get; set; }
    //    public byte[] PatientSign { get; set; }
    //    public byte[] ClinicianSign { get; set; }
    //    public string Status { get; set; }
    //}
    //#endregion

    //#region Patient Documents

    //public class PatientDocumentModel
    //{
    //    public int Id { get; set; }
    //    public int DocumentId { get; set; }
    //    public string DocumentName { get; set; }
    //    public string PatientName { get; set; }
    //    public int PatientId { get; set; }
    //    public string AssignedBy { get; set; }
    //    public string Status { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public decimal TotalRecords { get; set; }
    //}
    //public class AssignDocumentToPatientModel
    //{   
    //    public int Id { get; set; }
    //    public int PatientId { get; set; }
    //    public int DocumentId { get; set; }        
    //    public int AssignedBy { get; set; }
    //}

    //public class SaveSignatureModel
    //{
    //    public int PatientDocumentId { get; set; }
    //    public byte[] PatientSign { get; set; } = null;
    //    public byte[] ClinicianSign { get; set; } = null;
    //}
    //#endregion

}
