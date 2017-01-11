using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class FormModel
    {
        // public List<FirstFormFieldsModel> FirstFormFields { get; set; }

        public string FormName { get; set; }
        public string FormId { get; set; }
        public string FormAction { get; set; }
        public string configId { get; set; }
       

        public string RelatedEntity { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string LabelPosition { get; set; }
        public string SubmitMessage { get; set; }
        public string SubmitButtonText { get; set; }
        public bool Captcha { get; set; }
        public string RedirectUrl { get; set; }
        public string RedirectMode { get; set; }
        public string CSS { get; set; }

        public List<SecoundFormFieldsModel> SecoundFormFields { get; set; }

       

    }

    
        
    public class FirstFormFieldsModel
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
    public class SecoundFormFieldsModel
    {
        //public Dictionary<string, string> MapField { get; set; }
        //public Dictionary<string, string> DisplayOrder { get; set; }
        //public Dictionary<string, string> FieldLabel { get; set; }
        //public Dictionary<string, string> FieldToolTip { get; set; }
        //public Dictionary<string, string> FieldType { get; set; }
        //public Dictionary<string, string> FieldDefaultValue { get; set; }
        //public Dictionary<string, string> FieldLength { get; set; }
        //public Dictionary<string, string> FieldRequired { get; set; }

        public string MapField { get; set; }
        public string DisplayOrder { get; set; }
        public string FieldLabel { get; set; }
        public string FieldToolTip { get; set; }
        public string FieldType { get; set; }
        public string FieldDefaultValue { get; set; }
        public string FieldLength { get; set; }
        public string FieldRequired { get; set; }
    }
}