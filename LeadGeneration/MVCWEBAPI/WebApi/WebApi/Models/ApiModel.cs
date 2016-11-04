using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class CreateGetApiFieldsModel
    {
        public string FormName { get; set; }
        public string FormId { get; set; }
        public string FormAction { get; set; }
        public List<entityFields> entityFields { get; set; }
    }
    public class entityFields
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string DisplayName { get; set; }



    }

    public class GetFormData
    {
        public Guid Id { get; set; }
        public string new_firstname { get; set; }
        public string new_lastname { get; set; }
        public string new_email { get; set; }
        public string new_description { get; set; }

        //public string new_url { get; set; }
        //public string new_username { get; set; }
        //public string new_pass { get; set; }
    }


    public class LeadCreationModel
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
    }

    public class HtmlConversion
    {
        public string m_StringValue { get; set; }
       
    }

    public class UserAuthenticateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
    }

    public class CRMUserAuthenticateModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string UserName { get; set; }        
        public string Password { get; set; }
        public string EPassword { get; set; }
        public string SubscriptionType { get; set; }
        public string SolutionName { get; set; }
        public DateTime ExpireDate { get; set; }

        public bool IsSuccess { get; set; }
        public string Error { get; set; }

        public bool IsCreated { get; set; }
       
    }
    public class DotsWebFormModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

    }
    public class ParamsObject
    {
        public string orgName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}