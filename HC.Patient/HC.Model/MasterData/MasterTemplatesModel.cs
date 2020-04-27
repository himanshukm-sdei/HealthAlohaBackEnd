using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.MasterData
{
    public class MasterTemplatesModel
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateJson { get; set; }
        public decimal TotalRecords { get; set; }
    }
}
