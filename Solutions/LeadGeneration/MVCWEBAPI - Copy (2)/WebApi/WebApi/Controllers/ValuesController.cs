using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using WebApi.Models;
using System.Web.Http.Cors;
using System.ServiceModel.Description;

// These namespaces are found in the Microsoft.Xrm.Sdk.dll assembly
// located in the SDK\bin folder of the SDK download.
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Xrm;
using Microsoft.Xrm.Sdk.Query;
using System.Security;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Client;
using WebApi.Database;
using System.Security.Cryptography;
using System.IO;


using System.Configuration;
using LinqToTwitter;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    //[Authorize]
    //[EnableCors(origins: "*", headers: "*", methods: "*")] 
    public class ValuesController : ApiController
    {
        int trialTimePeriodInDays = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["trialTimePeriodInDays"]);
        WebApi.Database.MicrosoftCRMEntities Obj = new Database.MicrosoftCRMEntities();

        private const string initVector = "akram@dots9uzpgzl88";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;


        // To get discovery service address and organization unique name, 
        // Sign in to your CRM org and click Settings, Customization, Developer Resources.
        // On Developer Resource page, find the discovery service address under Service Endpoints and organization unique name under Your Organization Information.


        //private OrganizationService _service;



        [HttpGet]
        public TwitterAPISuccesModel TwitterUser(string rowId)
        {
            var model = new TwitterAPISuccesModel();
            try
            {
                if (rowId != null)
                {
                    var objtbl_Twitter = Obj.tbl_Twitter.SingleOrDefault(o => o.Row_Id == rowId);
                    if (objtbl_Twitter != null)
                    {
                        model.Id = objtbl_Twitter.Id;
                        model.Row_Id = objtbl_Twitter.Row_Id;
                        model.Image_Url = objtbl_Twitter.Image_Url;
                        model.IsSuccess = true;
                        model.Message = "Success";
                        return model;
                    }
                    else
                    {
                        model.Message = "Failed";
                        model.IsSuccess = false;
                        return model;
                    }
                }
                else
                {
                    model.Message = "InValidRowId";
                    model.IsSuccess = false;
                    return model;
                }
            }
            catch (Exception ex)
            {
                model.Message = ex.Message;
                model.IsSuccess = false;
                return model;
            }


        }


        [HttpGet]
        public TwitterPublisherModel GetPublisher(string CrmUrl)
        {
            var model = new TwitterPublisherModel();

            try
            {
                var result = Obj.tbl_Twitter.Where(o => o.CRMUrl == CrmUrl).Select(o => new PublisherModel { Id = o.Id, PublisherName = o.PublisherName }).ToList();
                           
                if (result.Count > 0)
                {
                    model.IsSuccess = true;
                    model.Message = "Success";
                    model.PublishersModel = result;
                    return model;
                }

                else
                {
                    model.IsSuccess = false;
                    model.Message = "No record found!";
                    return model;
                }
            }
            catch(Exception ex)
            {
                model.IsSuccess = false;
                model.Message = ex.Message;
                return model;
            }

        }


        //for firsttime solution delete all social media
        [HttpGet]
        public TwitterPublisherModel DeleteSocialMediaPublisher(string CrmUrl)
        {
            var model = new TwitterPublisherModel();

            try
            {
                var result = Obj.tbl_Twitter.Where(o => o.CRMUrl == CrmUrl).ToList();             

                if (result.Count > 0)
                {
                    Obj.tbl_Twitter.RemoveRange(result);
                    Obj.SaveChanges();
                    model.IsSuccess = true;
                    model.Message = "Success";                    
                    return model;
                }

                else
                {
                    model.IsSuccess = false;
                    model.Message = "No record found!";
                    return model;
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = ex.Message;
                return model;
            }
        }

        [HttpGet]
        public StringBuilder GetForm()
        {

            StringBuilder sb = new StringBuilder();
            CreateGetApiFieldsModel form = new CreateGetApiFieldsModel();
            List<entityFields> formFields = new List<entityFields>();

            var s = HttpContext.Current.Request.Url.Authority;
            String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
            var AbsoluteUri = HttpContext.Current.Request.Url.AbsoluteUri;
            //var k = ss.Replace("GetForm", "");
            // String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
            // String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/api/values/PostForm");
            string getUrlName = "GetForm";
            string postUrlName = "PostForm";
            var sd = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            //string strUrl = AbsoluteUri.Replace(getUrlName.ToLower(), postUrlName.ToLower());
            string strUrl = AbsoluteUri.Replace(AbsoluteUri, sd + "/api/values/" + postUrlName.ToLower());

            formFields.Add(new entityFields { FieldName = "new_firstname", FieldType = "text", DisplayName = "FirstName" });
            formFields.Add(new entityFields { FieldName = "new_lastname", FieldType = "text", DisplayName = "LastName" });
            formFields.Add(new entityFields { FieldName = "new_email", FieldType = "text", DisplayName = "Email" });
            formFields.Add(new entityFields { FieldName = "new_description", FieldType = "textarea", DisplayName = "Message" });
            formFields.Add(new entityFields { FieldName = "Id", FieldType = "hidden", DisplayName = "" });
            //formFields.Add(new entityFields { FieldName = "new_username", FieldType = "hidden", DisplayName = "" });
            //formFields.Add(new entityFields { FieldName = "new_pass", FieldType = "hidden", DisplayName = "" });
            form.FormName = "leadform";
            form.FormId = "leadform_Id";
            form.FormAction = strUrl;
            form.entityFields = formFields;
            // form.Add(new CreateGetApiFieldsModel { FormName = "leadform", FormId = "leadform_Id", FormAction = "", entityFields = formFields });
     

            sb.Append("<html lang='en'>");
           // sb.Append("<head><meta charset='utf-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='viewport' content = 'width=device-width, initial-scale=1'><meta name='description' content=''><meta name = 'author' content=''><link rel='icon' href = '../../favicon.ico'><title>Dotsquares</title><link href='CSS/bootstrap.min.css' rel='stylesheet'><link href='CSS/style.css' rel='stylesheet'></head>");
            sb.Append("<head>");
            sb.Append("<meta charset='utf-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='viewport' content='width=device-width, initial-scale=1'><meta name='description' content='' ><meta name='author' content='' ><link rel='icon' href = '../../favicon.ico'>");
            sb.Append("<title>Dotsquares</title>");
            sb.Append("<link href='"+sd+"/CSS/bootstrap.min.css' rel='stylesheet'><link href='"+sd+"/CSS/style.css' rel='stylesheet'>");
            sb.Append("</head>");
            sb.Append("<body>");

            sb.Append("<div class='container'><div id='loginbox' style='margin-top:50px;' class='mainbox col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2'><div class='panel panel-info'><div style = 'padding-top:30px' class='panel-body'><div style = 'display:none' id='login-alert' class='alert alert-danger col-sm-12'>AlertMessage</div>");

            sb.Append("<form action="+form.FormAction+" id="+form.FormId+" method='POST' class='form-horizontal'>");
            int i = 0;
            foreach (var fields in form.entityFields)
            {
                i += 1;
                if (fields.FieldType.ToLower()== "hidden")
                {
                    //sb.Append("<div style='margin-bottom: 25px ; width:100%;' class='input-group'>");
                    //sb.Append("<label for=" + fields.FieldName + ">" + fields.DisplayName + "<em class='red'>*</em></label>");
                    sb.Append("<input id=" + fields.FieldName + " name=" + fields.FieldName + " type=" + fields.FieldType + " value = ###" + i + " >");
                    //sb.Append("</div>");
                    sb.AppendLine();
                }
                else
                {
                    sb.Append("<div style='margin-bottom: 25px ; width:100%;' class='input-group'>");
                    sb.Append("<label for=" + fields.FieldName + ">" + fields.DisplayName + "<em class='red'>*</em></label>");
                    sb.Append("<input id=" + fields.FieldName + " name=" + fields.FieldName + " type=" + fields.FieldType + " data-validation='required' data-validation-error-msg='This is a required field.' class='form-control'  placeholder="+fields.DisplayName + ">");
                    sb.Append("</div>");
                    sb.AppendLine();
                }
            }

            sb.Append("<div style='margin-top:10px' class='form-group'><div class='col-sm-12 controls'><button type = 'submit' class='btn btn-success' id='btn-f-submit' >Submit</button></div></div>");


            sb.Append("</form>");
            sb.Append("</div></div></div></div>");
            sb.Append("<script src='https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js'></script>");
            sb.Append("<script src='https://cdnjs.cloudflare.com/ajax/libs/jquery-form-validator/2.3.26/jquery.form-validator.min.js'></script>");
            sb.Append("<link href='https://cdnjs.cloudflare.com/ajax/libs/jquery-form-validator/2.3.26/theme-default.min.css' rel='stylesheet' type='text/css'/>");
            sb.Append("<script src='"+sd+"/Script/bootstrap.min.js'></script>");
            sb.Append("<script>$.validate({form: '"+'#'+form.FormId+"', });</script>");

            sb.Append("</body>");

            sb.Append("</html>");



            return sb;

        }

        [HttpPost]        
        public LeadCreationModel PostForm([FromBody]  GetFormData model)
        {
            var Leadmodel = new LeadCreationModel();
            try
            {
                               
                var tblConfiData = Obj.tbl_Configuration.SingleOrDefault(o => o.Id == model.Id);
                if (tblConfiData != null)
                {

                    var result = DataInsertTimeUserAuthenticate(tblConfiData.OrgUniqueName, tblConfiData.ServerUrl, tblConfiData.UserName, tblConfiData.Password);

                    if (result.IsSuccess)
                    {
                        OrganizationService _service;
                        //EntityMap lead;
                        //Entity contactRetrive;
                        //var rs = fm["new_firstname"];                                        

                        CrmConnection connection = CrmConnection.Parse("Url='" + tblConfiData.ServerUrl + "'; Username='" + tblConfiData.UserName + "'; Password='" + tblConfiData.Password + "'");
                        using (_service = new OrganizationService(connection))
                        {

                            //ColumnSet columns = new ColumnSet(new string[] { "firstname", "lastname", });
                            //lead = _service.Retrieve("lead", new Guid(Request["LeadID"].ToString()), columns);

                            Xrm.Lead contactLead = new Xrm.Lead
                            {
                                FirstName = model.new_firstname,
                                LastName = model.new_lastname,

                                //Address1_City = "Sammamish",
                                //Address1_StateOrProvince = "MT",
                                //Address1_PostalCode = "99999",
                                //Telephone1 = "12345678",
                                EMailAddress1 = model.new_email,
                                //Subject = model.Topic,
                                Description = model.new_description
                                

                            };

                            var id = _service.Create(contactLead);
                            Leadmodel.IsSuccess = true;
                            return Leadmodel;

                        }


                    }
                    else
                    {
                        Leadmodel.IsSuccess = false;
                        Leadmodel.Error = "User is not authorized please check!!!";
                        return Leadmodel;
                    }

                }
                else
                {
                    Leadmodel.IsSuccess = false;
                    Leadmodel.Error = "Something going wrong!!!";
                    return Leadmodel;
                }
            }
            catch (Exception ex)
            {
                Leadmodel.IsSuccess = false;
                Leadmodel.Error = ex.ToString();
                return Leadmodel;
            }
            
        }

       
        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
        public CRMUserAuthenticateModel DataInsertTimeUserAuthenticate(string orgName, string ServerUrl, string UserName, string Password)
        {



            //    String _discoveryServiceAddress = "https://disco.crm8.dynamics.com/XRMServices/2011/Discovery.svc";
            // String _organizationUniqueName = "orgb51959c4";
            //// Provide your user name and password.
            // String _userName = "akram@dotsjpr.onmicrosoft.com";
            // String _password = "dots@123";

            //// Provide domain name for the On-Premises org.
            // String _domain = "mydomain";

            string finalUrl = "";
            var strorgName = ServerUrl.Split('.');
            if (strorgName.Contains("api"))
                finalUrl = strorgName[2] + "." + strorgName[3] + "." + strorgName[4].Replace("\"", "");
            else
                finalUrl = strorgName[1] + "." + strorgName[2] + "." + strorgName[3].Replace("\"", "");



            // for uk
            String _discoveryServiceAddress = "https://disco." + finalUrl + "/XRMServices/2011/Discovery.svc";
            String _organizationUniqueName = orgName;
            // Provide your user name and password.
            String _userName = UserName;
            String _password = Password;

            // Provide domain name for the On-Premises org.
            String _domain = "mydomain";
            //var model = new UserAuthenticateModel();
            var model = new CRMUserAuthenticateModel();
            try
            {
                //CrmConnection connection = CrmConnection.Parse(PortalPage.GetServiceConfiguration());

                IServiceManagement<IDiscoveryService> serviceManagement =
                                ServiceConfigurationFactory.CreateManagement<IDiscoveryService>(
                                new Uri(_discoveryServiceAddress));
                AuthenticationProviderType endpointType = serviceManagement.AuthenticationType;

                // Set the credentials.
                AuthenticationCredentials authCredentials = GetCredentials(serviceManagement, endpointType, _userName, _password, _domain);


                String organizationUri = String.Empty;
                // Get the discovery service proxy.
                using (DiscoveryServiceProxy discoveryProxy =
                    GetProxy<IDiscoveryService, DiscoveryServiceProxy>(serviceManagement, authCredentials))
                {
                    // Obtain organization information from the Discovery service. 
                    if (discoveryProxy != null)
                    {
                        // Obtain information about the organizations that the system user belongs to.
                        OrganizationDetailCollection orgs = DiscoverOrganizations(discoveryProxy);
                        // Obtains the Web address (Uri) of the target organization.
                        organizationUri = FindOrganization(_organizationUniqueName,
                            orgs.ToArray()).Endpoints[EndpointType.OrganizationService];

                    }
                }


                if (!String.IsNullOrWhiteSpace(organizationUri))
                {
                    IServiceManagement<IOrganizationService> orgServiceManagement =
                        ServiceConfigurationFactory.CreateManagement<IOrganizationService>(
                        new Uri(organizationUri));

                    // Set the credentials.
                    AuthenticationCredentials credentials = GetCredentials(orgServiceManagement, endpointType, _userName, _password, _domain);

                    // Get the organization service proxy.
                    using (OrganizationServiceProxy organizationProxy =
                        GetProxy<IOrganizationService, OrganizationServiceProxy>(orgServiceManagement, credentials))
                    {
                        // This statement is required to enable early-bound type support.
                        organizationProxy.EnableProxyTypes();

                        // Now make an SDK call with the organization service proxy.
                        // Display information about the logged on user.
                        Guid userid = ((WhoAmIResponse)organizationProxy.Execute(
                            new WhoAmIRequest())).UserId;
                        SystemUser systemUser = organizationProxy.Retrieve("systemuser", userid,
                            new ColumnSet(new string[] { "firstname", "lastname" })).ToEntity<SystemUser>();
                        //Console.WriteLine("Logged on user is {0} {1}.",
                        //    systemUser.FirstName, systemUser.LastName);
                        model.FirstName = systemUser.FirstName;
                        model.LastName = systemUser.LastName;
                        model.IsSuccess = true;
                    }
                }


                return model;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Error = ex.Message;
                return model;
            }
        }

        [HttpGet]
        public CRMUserAuthenticateModel LoadDetails(string registerId)
        {
            var model = new CRMUserAuthenticateModel();
            Guid Id = Guid.Empty;
            if (registerId != null)
                Id = Guid.Parse(registerId);

            var result = Obj.tbl_Configuration.SingleOrDefault(o => o.Id == Id);
            if (result != null)
            {
                model.FirstName = result.FirstName;
                model.LastName = result.LastName;
                model.Company = result.Company;
                model.ContactNo = result.ContactNo;
                model.Email = result.Email;
                model.Address = result.Address;
                model.Country = result.Country;
                model.State = result.State;
                model.City = result.City;
                model.PostalCode = result.PostalCode;
                model.UserName = result.UserName;
                model.Password = "*****";
                model.SubscriptionType = result.SubscriptionType;
                model.ExpireDate = result.ExpireDate.Date;
                model.IsSuccess = true;
                return model;
            }
            else
            {
                model.IsSuccess = false;
                return model;
            }
        }


        [HttpGet]
        public CRMUserAuthenticateModel LoadDetailsDotsCommon(string serverUrl, string orgName)
        {
            var model = new CRMUserAuthenticateModel();
           

            var result = Obj.tbl_Configuration.SingleOrDefault(o => o.ServerUrl.ToLower() == serverUrl.ToLower() && o.OrgUniqueName.ToLower()==orgName);
            if (result != null)
            {
                model.Id = result.Id;
                model.FirstName = result.FirstName;
                model.LastName = result.LastName;
                model.Company = result.Company;
                model.ContactNo = result.ContactNo;
                model.Email = result.Email;
                model.Address = result.Address;
                model.Country = result.Country;
                model.State = result.State;
                model.City = result.City;
                model.PostalCode = result.PostalCode;
                model.UserName = result.UserName;
                model.Password = "*****";
                model.SubscriptionType = result.SubscriptionType;
                model.ExpireDate = result.ExpireDate.Date;
                model.IsSuccess = true;
                return model;
            }
            else
            {
                model.IsSuccess = false;
                return model;
            }
        }
        [HttpGet]
        public CRMUserAuthenticateModel DotsCommon(string FirstName, string LastName, string Company, string ContactNo, string Email, string Address, string Country, string State, string City, string PostalCode, string SubscriptionType, string orgName, string ServerUrl, string UserName, string Password, string SName)
        {
            var model = new CRMUserAuthenticateModel();
            try
            {
               
                tbl_Configuration tblConfig = new tbl_Configuration();
                //always return null
                var update = Obj.tbl_Configuration.SingleOrDefault(o => o.ServerUrl.ToLower() == ServerUrl.ToLower() && o.OrgUniqueName.ToLower() == orgName.ToLower() && o.SolutionName=="####");
                if (update != null)
                {

                    update.FirstName = FirstName;
                    update.LastName = LastName;
                    update.Company = Company;
                    update.ContactNo = ContactNo;
                    update.Email = Email;
                    update.Address = Address;
                    update.Country = Country;
                    update.State = State;
                    update.City = City;
                    update.PostalCode = PostalCode;
                    update.UserName = UserName;
                    update.Password = Password;
                    update.OrgUniqueName = orgName;
                    update.ServerUrl = ServerUrl;
                    update.SubscriptionType = SubscriptionType;
                    update.ModifyDate = DateTime.Now;
                    update.IsCreated = false;
                    // tblConfig.ExpireDate = DateTime.Now.AddDays(trialTimePeriodInDays);
                    Obj.SaveChanges();
                }

                else
                {
                    tblConfig.Id = Guid.NewGuid();
                    tblConfig.FirstName = FirstName;
                    tblConfig.LastName = LastName;
                    tblConfig.Company = Company;
                    tblConfig.ContactNo = ContactNo;
                    tblConfig.Email = Email;
                    tblConfig.Address = Address;
                    tblConfig.Country = Country;
                    tblConfig.State = State;
                    tblConfig.City = City;
                    tblConfig.PostalCode = PostalCode;
                     tblConfig.UserName = UserName;
                     tblConfig.Password = Password;
                    tblConfig.OrgUniqueName = orgName;
                    tblConfig.ServerUrl = ServerUrl;
                    tblConfig.SubscriptionType = SubscriptionType;
                    tblConfig.SolutionName = SName;
                    tblConfig.CreateDate = DateTime.Now;
                    tblConfig.ModifyDate = null;
                    tblConfig.ExpireDate = DateTime.Now.AddDays(trialTimePeriodInDays);
                    tblConfig.IsCreated = true;
                    Obj.tbl_Configuration.Add(tblConfig);
                    Obj.SaveChanges();
                }

                //return model
                
                if (update != null)
                {
                    model.Id = update.Id;
                    model.IsCreated = update.IsCreated;
                }
                else
                {
                    model.Id = tblConfig.Id;
                    model.IsCreated = tblConfig.IsCreated;
                }
                model.FirstName = FirstName;
                model.LastName = LastName;
                model.Company = Company;
                model.ContactNo = ContactNo;
                model.Email = Email;
                model.Address = Address;
                model.Country = Country;
                model.State = State;
                model.City = City;
                model.PostalCode = PostalCode;
                model.UserName = UserName;
                model.Password = Password;
                model.SubscriptionType = SubscriptionType;
                model.SolutionName = SName;

                model.IsSuccess = true;

                return model;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Error = ex.Message;
                return model;
            }
        }
        [HttpGet]       
        public CRMUserAuthenticateModel UserAuthenticate(string FirstName, string LastName, string Company, string ContactNo, string Email, string Address, string Country, string State, string City, string PostalCode, string SubscriptionType, string orgName, string ServerUrl, string UserName, string Password,string RegisterId)
        {

           
          
          
            //string orgName = paramsObject.orgName;
            //string UserName = paramsObject.UserName;
            //string Password = paramsObject.Password;

            //    String _discoveryServiceAddress = "https://disco.crm8.dynamics.com/XRMServices/2011/Discovery.svc";
            // String _organizationUniqueName = "orgb51959c4";
            //// Provide your user name and password.
            // String _userName = "akram@dotsjpr.onmicrosoft.com";
            // String _password = "dots@123";

            //// Provide domain name for the On-Premises org.
            // String _domain = "mydomain";

            string finalUrl = "";
            var strorgName = ServerUrl.Split('.');
            if (strorgName.Contains("api"))
                finalUrl = strorgName[2] + "." + strorgName[3] + "." + strorgName[4].Replace("\"", "");
            else
                finalUrl = strorgName[1] + "." + strorgName[2] + "." + strorgName[3].Replace("\"", "");

            

            // for uk
             String _discoveryServiceAddress = "https://disco." + finalUrl + "/XRMServices/2011/Discovery.svc";
            String _organizationUniqueName = orgName;
            // Provide your user name and password.
            String _userName = UserName;
            String _password = Password;

            // Provide domain name for the On-Premises org.
            String _domain = "mydomain";
            //var model = new UserAuthenticateModel();
            var model = new CRMUserAuthenticateModel();
            try
            {
                //CrmConnection connection = CrmConnection.Parse(PortalPage.GetServiceConfiguration());
               
                IServiceManagement<IDiscoveryService> serviceManagement =
                                ServiceConfigurationFactory.CreateManagement<IDiscoveryService>(
                                new Uri(_discoveryServiceAddress));
                AuthenticationProviderType endpointType = serviceManagement.AuthenticationType;

                // Set the credentials.
                AuthenticationCredentials authCredentials = GetCredentials(serviceManagement, endpointType ,_userName,_password,_domain);

                //for stop 5 second to response some server 
                System.Threading.Thread.Sleep(5000);

                String organizationUri = String.Empty;
                // Get the discovery service proxy.
                using (DiscoveryServiceProxy discoveryProxy =
                    GetProxy<IDiscoveryService, DiscoveryServiceProxy>(serviceManagement, authCredentials))
                {
                    // Obtain organization information from the Discovery service. 
                    if (discoveryProxy != null)
                    {
                        // Obtain information about the organizations that the system user belongs to.
                        OrganizationDetailCollection orgs = DiscoverOrganizations(discoveryProxy);
                        // Obtains the Web address (Uri) of the target organization.
                        organizationUri = FindOrganization(_organizationUniqueName,
                            orgs.ToArray()).Endpoints[EndpointType.OrganizationService];

                    }
                }


                if (!String.IsNullOrWhiteSpace(organizationUri))
                {
                    IServiceManagement<IOrganizationService> orgServiceManagement =
                        ServiceConfigurationFactory.CreateManagement<IOrganizationService>(
                        new Uri(organizationUri));

                    // Set the credentials.
                    AuthenticationCredentials credentials = GetCredentials(orgServiceManagement, endpointType,_userName,_password,_domain);

                    // Get the organization service proxy.
                    using (OrganizationServiceProxy organizationProxy =
                        GetProxy<IOrganizationService, OrganizationServiceProxy>(orgServiceManagement, credentials))
                    {
                        // This statement is required to enable early-bound type support.
                        organizationProxy.EnableProxyTypes();

                        // Now make an SDK call with the organization service proxy.
                        // Display information about the logged on user.
                        Guid userid = ((WhoAmIResponse)organizationProxy.Execute(
                            new WhoAmIRequest())).UserId;
                        SystemUser systemUser = organizationProxy.Retrieve("systemuser", userid,
                            new ColumnSet(new string[] { "firstname", "lastname" })).ToEntity<SystemUser>();
                        //Console.WriteLine("Logged on user is {0} {1}.",
                        //    systemUser.FirstName, systemUser.LastName);
                        Guid registerId = Guid.Empty;
                        if (RegisterId != null)
                            registerId = Guid.Parse(RegisterId);
                        
                                                    tbl_Configuration tblConfig = new tbl_Configuration();
                        var update = Obj.tbl_Configuration.SingleOrDefault(o => o.Id == registerId);
                        if (update != null)
                        {

                            update.FirstName = FirstName;
                            update.LastName = LastName;
                            update.Company = Company;
                            update.ContactNo = ContactNo;
                            update.Email = Email;
                            update.Address = Address;
                            update.Country = Country;
                            update.State = State;
                            update.City = City;
                            update.PostalCode = PostalCode;
                            update.UserName = UserName;
                            update.Password = Password;
                            update.OrgUniqueName = orgName;
                            update.ServerUrl = ServerUrl;
                            update.SubscriptionType = SubscriptionType;
                            update.SolutionName = "LeadCreation";
                            update.ModifyDate = DateTime.Now;                           
                            update.IsCreated = false;
                            // tblConfig.ExpireDate = DateTime.Now.AddDays(trialTimePeriodInDays);
                            Obj.SaveChanges();
                        }

                        else
                        {
                            tblConfig.Id = Guid.NewGuid();
                            tblConfig.FirstName = FirstName;
                            tblConfig.LastName = LastName;
                            tblConfig.Company = Company;
                            tblConfig.ContactNo = ContactNo;
                            tblConfig.Email = Email;
                            tblConfig.Address = Address;
                            tblConfig.Country = Country;
                            tblConfig.State = State;
                            tblConfig.City = City;
                            tblConfig.PostalCode = PostalCode;
                            tblConfig.UserName = UserName;
                            tblConfig.Password = Password;
                            tblConfig.OrgUniqueName = orgName;
                            tblConfig.ServerUrl = ServerUrl;
                            tblConfig.SubscriptionType = SubscriptionType;
                            tblConfig.SolutionName = "LeadCreation";
                            tblConfig.CreateDate = DateTime.Now;
                            tblConfig.ModifyDate = null;
                            tblConfig.ExpireDate = DateTime.Now.AddDays(trialTimePeriodInDays);
                            tblConfig.IsCreated = true;                           
                            Obj.tbl_Configuration.Add(tblConfig);
                            Obj.SaveChanges();
                        }

                        //return model
                        string  EncrypyUserName="",EncryptPassword="";
                        Guid Id = Guid.Empty;
                        if (update != null)
                        {
                            model.Id = update.Id;
                            model.IsCreated = update.IsCreated;
                        }
                        else
                        {
                            model.Id = tblConfig.Id;
                            model.IsCreated = tblConfig.IsCreated;
                        }
                        model.FirstName = FirstName;
                        model.LastName = LastName;
                        model.Company = Company;
                        model.ContactNo = ContactNo;
                        model.Email = Email;
                        model.Address = Address;
                        model.Country = Country;
                        model.State = State;
                        model.City = City;
                        model.PostalCode = PostalCode;
                        
                        if(UserName!=null)
                            EncrypyUserName= EncryptString(UserName, "@123");
                        if (Password != null)
                                EncryptPassword =  EncryptString(Password, "@123");
                        
                        
                        model.UserName = EncrypyUserName;
                        model.EPassword = EncryptPassword;                                        
                        model.SubscriptionType = SubscriptionType;
                                                                  
                        model.IsSuccess = true;
                        
                    }
                }


                return model;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Error = ex.Message;
                return model;
            }

        }
        private AuthenticationCredentials GetCredentials<TService>(IServiceManagement<TService> service, AuthenticationProviderType endpointType, string _userName, string _password, string _domain)
        {
            AuthenticationCredentials authCredentials = new AuthenticationCredentials();

            switch (endpointType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    authCredentials.ClientCredentials.Windows.ClientCredential =
                        new System.Net.NetworkCredential(_userName,
                            _password,
                            _domain);
                    break;
                case AuthenticationProviderType.LiveId:
                    authCredentials.ClientCredentials.UserName.UserName = _userName;
                    authCredentials.ClientCredentials.UserName.Password = _password;
                    authCredentials.SupportingCredentials = new AuthenticationCredentials();
                    authCredentials.SupportingCredentials.ClientCredentials =
                        Microsoft.Crm.Services.Utility.DeviceIdManager.LoadOrRegisterDevice();
                    break;
                default: // For Federated and OnlineFederated environments.                    
                    authCredentials.ClientCredentials.UserName.UserName = _userName;
                    authCredentials.ClientCredentials.UserName.Password = _password;
                    // For OnlineFederated single-sign on, you could just use current UserPrincipalName instead of passing user name and password.
                    // authCredentials.UserPrincipalName = UserPrincipal.Current.UserPrincipalName;  // Windows Kerberos

                    // The service is configured for User Id authentication, but the user might provide Microsoft
                    // account credentials. If so, the supporting credentials must contain the device credentials.
                    if (endpointType == AuthenticationProviderType.OnlineFederation)
                    {
                        IdentityProvider provider = service.GetIdentityProvider(authCredentials.ClientCredentials.UserName.UserName);
                        if (provider != null && provider.IdentityProviderType == IdentityProviderType.LiveId)
                        {
                            authCredentials.SupportingCredentials = new AuthenticationCredentials();
                            authCredentials.SupportingCredentials.ClientCredentials =
                                Microsoft.Crm.Services.Utility.DeviceIdManager.LoadOrRegisterDevice();
                        }
                    }

                    break;
            }

            return authCredentials;
        }
        public OrganizationDetailCollection DiscoverOrganizations(
            IDiscoveryService service)
        {
            if (service == null) throw new ArgumentNullException("service");
            RetrieveOrganizationsRequest orgRequest = new RetrieveOrganizationsRequest();
            RetrieveOrganizationsResponse orgResponse =
                (RetrieveOrganizationsResponse)service.Execute(orgRequest);

            return orgResponse.Details;
        }
        public OrganizationDetail FindOrganization(string orgUniqueName,
                    OrganizationDetail[] orgDetails)
        {
            if (String.IsNullOrWhiteSpace(orgUniqueName))
                throw new ArgumentNullException("orgUniqueName");
            if (orgDetails == null)
                throw new ArgumentNullException("orgDetails");
            OrganizationDetail orgDetail = null;

            foreach (OrganizationDetail detail in orgDetails)
            {
                if (String.Compare(detail.UniqueName, orgUniqueName,
                    StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    orgDetail = detail;
                    break;
                }
            }
            return orgDetail;
        }
        private TProxy GetProxy<TService, TProxy>(
               IServiceManagement<TService> serviceManagement,
               AuthenticationCredentials authCredentials)
               where TService : class
               where TProxy : ServiceProxy<TService>
        {
            Type classType = typeof(TProxy);

            if (serviceManagement.AuthenticationType !=
                AuthenticationProviderType.ActiveDirectory)
            {
                AuthenticationCredentials tokenCredentials =
                    serviceManagement.Authenticate(authCredentials);
                // Obtain discovery/organization service proxy for Federated, LiveId and OnlineFederated environments. 
                // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and SecurityTokenResponse.
                return (TProxy)classType
                    .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(SecurityTokenResponse) })
                    .Invoke(new object[] { serviceManagement, tokenCredentials.SecurityTokenResponse });
            }

            // Obtain discovery/organization service proxy for ActiveDirectory environment.
            // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and ClientCredentials.
            return (TProxy)classType
                .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(ClientCredentials) })
                .Invoke(new object[] { serviceManagement, authCredentials.ClientCredentials });
        }
    }
}
