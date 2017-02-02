using LinqToTwitter;
using Microsoft.Ajax.Utilities;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using WebApi.Database;
using WebApi.Models;

namespace WebApi.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : Controller
    {
        MicrosoftCRMEntities Obj = new MicrosoftCRMEntities();
        public object MessageBox { get; private set; }



        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<ActionResult> BeginAsync(string rowId, string url, string publisher, string media)
        {
            //var auth = new MvcSignInAuthorizer          

            string pathurl = "";


            if (!string.IsNullOrEmpty(rowId) && !string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(publisher) && !string.IsNullOrEmpty(media))
            {

                string[] CurrentUrl = Request.Url.ToString().Split('?');
                var auth = new MvcAuthorizer
                {
                    CredentialStore = new SessionStateCredentialStore
                    {
                        ConsumerKey = ConfigurationManager.AppSettings["consumerKey"],
                        ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"]

                    }
                };

                // string twitterCallbackUrl = Request.Url.ToString().Replace("Begin", "Complete");
                string twitterCallbackUrl = CurrentUrl[0].Replace("Begin", "Complete");
                // auth.Callback = new Uri(twitterCallbackUrl);
                await auth.BeginAuthorizationAsync(new Uri(twitterCallbackUrl + "?rowId=" + rowId + "&CRMUrl=" + url + "&PublisherName=" + publisher + "&SocialMediaType=" + media));

                pathurl = "https://api.twitter.com/oauth/authorize?oauth_token=" + auth.Parameters["oauth_token"] + "";
                return Json(pathurl, JsonRequestBehavior.AllowGet);
            }
            else
            {
                pathurl = "Error";
                return Json(pathurl, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> CompleteAsync()
        {
            try
            {

                string rowId = Request.QueryString["rowId"].ToString();
                string CRMUrl = Request.QueryString["CRMUrl"].ToString();
                string PublisherName = Request.QueryString["PublisherName"].ToString();
                string SocialMediaType = Request.QueryString["SocialMediaType"].ToString();
                var auth = new MvcAuthorizer
                {
                    CredentialStore = new SessionStateCredentialStore()
                };
                var oauth_consumer_key = Request.QueryString["oauth_consumer_key"];
                var oauth_token = Request.QueryString["oauth_token"];
                auth.Parameters["oauth_consumer_key"] = oauth_consumer_key;
                auth.Parameters["oauth_token"] = oauth_token;
                await auth.CompleteAuthorizeAsync(Request.Url);
                // await auth.CompleteAuthorizeAsync(new Uri( CurrentUrl[0]));

                //###########Start#############
                var credentials = auth.CredentialStore;
                credentials.ConsumerKey = ConfigurationManager.AppSettings["consumerKey"];
                credentials.ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
                credentials.OAuthToken = auth.CredentialStore.OAuthToken;
                credentials.OAuthTokenSecret = auth.CredentialStore.OAuthTokenSecret;
                credentials.ScreenName = auth.CredentialStore.ScreenName;
                credentials.UserID = auth.CredentialStore.UserID;
                auth.CredentialStore = credentials;
                var ctx = new TwitterContext(auth);

                User currentUser = (from user in ctx.User
                                    where user.Type == LinqToTwitter.UserType.Show && user.ScreenName == auth.CredentialStore.ScreenName
                                    select user).ToList<User>().SingleOrDefault();

                //############END###############

                tbl_Twitter tbltwr = new tbl_Twitter();
                Guid newId = Guid.NewGuid();
                tbltwr.Id = newId;
                tbltwr.Row_Id = rowId;
                tbltwr.UserID = auth.CredentialStore.UserID.ToString();
                tbltwr.OauthToken = auth.CredentialStore.OAuthToken;
                tbltwr.OauthTokenSecret = auth.CredentialStore.OAuthTokenSecret;
                tbltwr.ScreenName = auth.CredentialStore.ScreenName;
                tbltwr.Image_Url = currentUser.ProfileImageUrlHttps;
                tbltwr.CRMUrl = CRMUrl;
                tbltwr.PublisherName = PublisherName;
                tbltwr.SocialMediaType = SocialMediaType;

                tbltwr.AuthenticateDate = DateTime.Now;
                Obj.tbl_Twitter.Add(tbltwr);
                Obj.SaveChanges();

                return RedirectToAction("Success", "Home");
                //return RedirectToAction("BeginAsync", new { rowId = "Done" });
                //return Json(new { Id= newId.ToString(),IsSucces=true,Message="Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSucces = false, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
            //return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [ActionName("Tweet")]
        public async Task<ActionResult> TweetAsync(string text, string rowId)
        {

            try
            {
                if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(rowId))
                {

                    var auth = new MvcAuthorizer
                    {
                        CredentialStore = new SessionStateCredentialStore()
                    };


                    var credentials = auth.CredentialStore;
                    var twitterUser = Obj.tbl_Twitter.SingleOrDefault(o => o.Row_Id == rowId);
                    credentials.ConsumerKey = ConfigurationManager.AppSettings["consumerKey"];
                    credentials.ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
                    credentials.OAuthToken = twitterUser.OauthToken;
                    credentials.OAuthTokenSecret = twitterUser.OauthTokenSecret;
                    credentials.ScreenName = twitterUser.ScreenName;
                    credentials.UserID = Convert.ToUInt64(twitterUser.UserID);


                    auth.CredentialStore = credentials;
                    var ctx = new TwitterContext(auth);

                    Status responseTweet = await ctx.TweetAsync(text);

                    if (responseTweet != null)
                    {

                        return Json(new { IsSuccess = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        return Json(new { IsSuccess = false, Message = "Failed" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "Failed" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new { IsSuccess = false, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


        [ActionName("HomeTimeline")]
        public async Task<ActionResult> HomeTimelineAsync(string id, string mediaType)
        {

            try
            {


                if (!string.IsNullOrEmpty(id))
                {
                    var auth = new MvcAuthorizer
                    {
                        CredentialStore = new SessionStateCredentialStore()
                    };

                    Guid Id = Guid.Empty;
                    if (id != null)
                        Id = Guid.Parse(id);

                    var credentials = auth.CredentialStore;
                    var twitterUser = Obj.tbl_Twitter.SingleOrDefault(o => o.Id == Id && o.SocialMediaType == mediaType);
                    credentials.ConsumerKey = ConfigurationManager.AppSettings["consumerKey"];
                    credentials.ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
                    credentials.OAuthToken = twitterUser.OauthToken;
                    credentials.OAuthTokenSecret = twitterUser.OauthTokenSecret;
                    credentials.ScreenName = twitterUser.ScreenName;
                    credentials.UserID = Convert.ToUInt64(twitterUser.UserID);


                    auth.CredentialStore = credentials;

                    var ctx = new TwitterContext(auth);

                    var tweets =
                        await
                        (from tweet in ctx.Status
                         where tweet.Type == StatusType.Home && tweet.Count == 50
                         select new TweetViewModel
                         {
                             ImageUrl = tweet.User.ProfileImageUrl,
                             ScreenName = tweet.User.ScreenNameResponse,
                             Text = tweet.Text,

                         })
                        .ToListAsync();

                    return Json(new { IsSuccess = true, Message = "Success", UserTweet = tweets }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "Failed" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Index()
        {

            ViewBag.Title = "Home Page";

            //string password = "HomePage";
            //var en = EncryptString(password, "@123");
            //var de = DecryptString(en, "@123");

            string Id = "58EB3029-E870-4132-8AE7-2E9294276BD7";
            string entityid = "{5AC551A1-3FD3-E611-8101-C4346BAC0A3C}";
            Guid Ids = Guid.Empty;
            if (Ids != null)
                Ids = Guid.Parse(Id);

            Guid EIds = Guid.Empty;
            if (EIds != null)
                EIds = Guid.Parse(entityid);

            // GetEntityFields(Ids,EIds);
            return View();
        }


        private const string initVector = "akram@dots9uzpgzl88";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
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
        public async Task<string> GetResult()
        {
            using (var httpClient = new HttpClient())
            {
                var httpResponse = await httpClient.GetAsync("http://localhost:54126/api/values/GetForm");
                var rs = httpResponse.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<HtmlConversion>(rs.Result);
                return model.ToString();
                //return await httpResponse.Content.ReadAsStringAsync();
            }
        }
        public bool IsSolutionUnInstall(tbl_Configuration tbl)
        {
            OrganizationService _service;
            CrmConnection connection = CrmConnection.Parse("Url='" + tbl.ServerUrl + "'; Username='" + tbl.UserName + "'; Password='" + tbl.Password + "'");
            using (_service = new OrganizationService(connection))
            {
                QueryExpression query = new QueryExpression();
                query.EntityName = "dots_webformconfiguration";
                query.ColumnSet = new ColumnSet(true);

                EntityCollection col = _service.RetrieveMultiple(query);

                Entity SampleCofiguration = null;
                if (col != null && col.Entities.Count > 0)
                {
                    SampleCofiguration = col.Entities[0];
                    //for solution installed and also registerd
                    if (SampleCofiguration["dots_registerid"].ToString() != null)
                        return false;
                    else //for solution installed but not registerd
                        return true;
                }
                else
                {
                    return true;
                }
            }
        }


        public FormModel GetEntityFields(Guid Id, Guid entityId)
        {
            var FormModel = new FormModel();


            var tbl = Obj.tbl_Configuration.SingleOrDefault(o => o.Id == Id);
            OrganizationService _service;
            CrmConnection connection = CrmConnection.Parse("Url='" + tbl.ServerUrl + "'; Username='" + tbl.UserName + "'; Password='" + tbl.Password + "'");

            using (_service = new OrganizationService(connection))
            {
                Entity webform = new Entity("dots_webform");
                //ColumnSet attributes = new ColumnSet(new string[] { "name", "address1_postalcode", "lastusedincampaign", "versionnumber" });
                ColumnSet attributes = new ColumnSet("dots_name", "dots_email", "dots_relatedentity", "dots_labelposition", "dots_submitmessage", "dots_submitbutton", "dots_redirecturl", "new_redirectmode", "dots_linkbuttontext", "dots_css");
                // Retrieve the dots_webform and its name and attributes.
                var retrievedwebform = _service.Retrieve(webform.LogicalName, entityId, attributes);

                if (retrievedwebform.Attributes.Count > 0)
                {


                    string strText = "";

                    FormModel.FormName = retrievedwebform.Attributes["dots_name"].ToString();
                    FormModel.FormId = retrievedwebform.Attributes["dots_name"].ToString();
                    FormModel.configId = Id.ToString();


                    FormModel.Name = retrievedwebform.Attributes["dots_name"].ToString();
                    if (retrievedwebform.Attributes.Keys.Contains("dots_email") == true)
                        FormModel.Email = retrievedwebform.Attributes["dots_email"].ToString();

                    FormModel.RelatedEntity = retrievedwebform.Attributes["dots_relatedentity"].ToString();

                    if (retrievedwebform.Attributes.Keys.Contains("dots_labelposition") == true)
                    {

                        //int value = ((OptionSetValue)retrievedwebform.Attributes["dots_labelposition"]).Value;

                        //var dots_labelposition = retrievedwebform.Attributes["dots_labelposition"];
                        //string labelvalue = dots_labelposition.ToString();
                        ////int OptionsetValue =;
                        ////int OptionsetValued = 1;
                        //var OptionsetValue = CRMHelper.getOptionSetValue("dots_webform", "dots_labelposition", labelvalue, _service);
                        //strText = CRMHelper.getOptionSetText("dots_webform", "dots_labelposition", OptionsetValue, _service);

                        // strText = retrievedwebform.Attributes["dots_labelposition"].ToString();
                        strText = retrievedwebform.FormattedValues["dots_labelposition"];
                        FormModel.LabelPosition = strText;
                    }

                    if (retrievedwebform.Attributes.Keys.Contains("dots_submitmessage") == true)
                        FormModel.SubmitMessage = retrievedwebform.Attributes["dots_submitmessage"].ToString();
                    if (retrievedwebform.Attributes.Keys.Contains("dots_submitbutton") == true)
                        FormModel.SubmitButtonText = retrievedwebform.Attributes["dots_submitbutton"].ToString();
                    if (retrievedwebform.Attributes.Keys.Contains("dots_redirecturl") == true)
                        FormModel.RedirectUrl = retrievedwebform.Attributes["dots_redirecturl"].ToString();
                    if (retrievedwebform.Attributes.Keys.Contains("new_redirectmode") == true)
                    {
                        //int plainValue = (int)retrievedwebform.Attributes["new_redirectmode"];


                        int OptionsetValue = ((Microsoft.Xrm.Sdk.OptionSetValue)retrievedwebform.Attributes["new_redirectmode"]).Value;
                        var OptionsetText = retrievedwebform.FormattedValues["new_redirectmode"];
                        //string redirectMode = "";
                        //if (OptionsetValue == 1)
                        //    redirectMode = "Auto";
                        //else if (OptionsetValue == 2)
                        //    redirectMode = "Link";
                        //else if (OptionsetValue == 3)
                        //    redirectMode = "Button";

                        FormModel.RedirectMode = OptionsetText;
                    }
                    if (retrievedwebform.Attributes.Keys.Contains("dots_linkbuttontext") == true)
                        FormModel.LinkButtonText = retrievedwebform.Attributes["dots_linkbuttontext"].ToString();
                    if (retrievedwebform.Attributes.Keys.Contains("dots_css") == true)
                        FormModel.CSS = retrievedwebform.Attributes["dots_css"].ToString();


                    // Retrieve the related dots_field
                    QueryExpression query = new QueryExpression("dots_field");
                    query.ColumnSet = new ColumnSet("dots_displayorder", "dots_fieldlabel", "dots_fieldtooltip", "dots_fieldtype", "dots_fielddefaultvalue", "dots_fieldlength", "dots_fieldrequired", "dots_mapfield");
                    // query.ColumnSet = new ColumnSet("dots_fieldlabel", "dots_displayorder", "dots_fieldtype", "dots_mapfield");
                    query.Criteria.AddCondition("dots_parent_webformid", ConditionOperator.Equal, entityId);
                    EntityCollection results = _service.RetrieveMultiple(query);

                    //DataCollection<Entity> dotsFields = _service.RetrieveMultiple(
                    //    query).Entities;

                    var secoundModellst = new List<SecoundFormFieldsModel>();
                    // var secoundModel = new SecoundFormFieldsModel();

                    string strFieldType = null;

                    int displayNo = 1000;
                    foreach (Entity act in results.Entities)
                    {

                        //secoundModellst.Add(new SecoundFormFieldsModel { FieldLabel = act["dots_fieldlabel"].ToString(), DisplayOrder = act["dots_displayorder"].ToString(), FieldToolTip = act["dots_fieldtooltip"].ToString(), FieldType = act["dots_fieldtype"].ToString(), FieldDefaultValue = act["dots_fielddefaultvalue"].ToString(), FieldLength = act["dots_fieldlength"].ToString(), FieldRequired = act["dots_fieldrequired"].ToString(), MapField = act["dots_mapfield"].ToString() });

                        //if (act.Attributes.Keys.Contains("dots_fieldtype") == true)
                        //{
                        //   // int plainValue = (int)retrievedwebform["new_redirectmode"];

                        //    int OptionsetValue = ((Microsoft.Xrm.Sdk.OptionSetValue)act.Attributes["dots_fieldtype"]).Value;
                        //    var OptionsetText = act.FormattedValues["dots_fieldtype"];
                        //    //strFieldType = CRMHelper.getOptionSetText("dots_webform", "dots_fieldtype", OptionsetValue, _service);
                        //    strFieldType = OptionsetText;

                        //}

                        secoundModellst.Add(new SecoundFormFieldsModel
                        {
                            DisplayOrder = (act.Attributes.Keys.Contains("dots_displayorder") == false ? displayNo : Convert.ToInt32(act["dots_displayorder"].ToString())),
                            FieldLabel = act["dots_fieldlabel"].ToString(),
                            FieldToolTip = (act.Attributes.Keys.Contains("dots_fieldtooltip") == false ? null : act["dots_fieldtooltip"].ToString()),
                            FieldType = (act.Attributes.Keys.Contains("dots_fieldtype") == false ? "Text" : act.FormattedValues["dots_fieldtype"]),
                            FieldDefaultValue = (act.Attributes.Keys.Contains("dots_fielddefaultvalue") == false ? null : act["dots_fielddefaultvalue"].ToString()),
                            FieldLength = (act.Attributes.Keys.Contains("dots_fieldlength") == false ? null : act["dots_fieldlength"].ToString()),
                            //FieldRequired= ((act.Attributes.Keys.Contains("dots_fieldrequired") == true) ? "true" : "false"),
                            FieldRequired = act["dots_fieldrequired"].ToString(),
                            MapField = act["dots_mapfield"].ToString(),

                        });
                        displayNo += 1;
                    }


                    // model.FirstFormFields = firstModel;
                    FormModel.SecoundFormFields = secoundModellst;

                }



            }

            return FormModel;
        }
        [HttpGet]
        public  ActionResult CrmForm(string rId, string entityId,string Type)
        {          
                     
            try
            {
                string message = "";
                // if (rId != Guid.Empty)
                if (!String.IsNullOrEmpty(rId) && !String.IsNullOrEmpty(entityId) && !String.IsNullOrEmpty(Type))
                {
                    WebApi.Database.MicrosoftCRMEntities Obj = new Database.MicrosoftCRMEntities();
                    var registerId = Guid.Parse(rId);
                    var currentEntityId = Guid.Parse(entityId);

                    var objtbl_Configuration = Obj.tbl_Configuration.SingleOrDefault(o => o.Id == registerId);

                    if (objtbl_Configuration != null)
                    {
                        //check solution uninstalled or not
                        if (IsSolutionUnInstall(objtbl_Configuration) == false)
                        {
                            if (DateTime.Now.Date <= objtbl_Configuration.ExpireDate.Date)
                            {
                                StringBuilder sb = new StringBuilder();
                                FormModel formFieldsModel = new FormModel();

                                formFieldsModel = GetEntityFields(registerId, currentEntityId);


                                String strPathAndQuery = HttpContext.Request.Url.PathAndQuery;
                                var AbsoluteUri = HttpContext.Request.Url.AbsoluteUri;
                                formFieldsModel.FormAction = AbsoluteUri.Split('?')[0];

                                var sd = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);


                                sb.Append("<html lang='en'>");
                                // sb.Append("<head><meta charset='utf-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='viewport' content = 'width=device-width, initial-scale=1'><meta name='description' content=''><meta name = 'author' content=''><link rel='icon' href = '../../favicon.ico'><title>Dotsquares</title><link href='CSS/bootstrap.min.css' rel='stylesheet'><link href='CSS/style.css' rel='stylesheet'></head>");
                                sb.Append("<head>");
                                sb.Append("<meta charset='utf-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='viewport' content='width=device-width, initial-scale=1'><meta name='description' content='' ><meta name='author' content='' ><link rel='icon' href = '../../favicon.ico'>");
                                sb.Append("<title>Dotsquares</title>");
                                if (formFieldsModel.CSS != null)
                                    sb.Append("<style type='text/css'>"+ formFieldsModel.CSS + "</style>");

                                sb.Append("<link href='" + sd + "/CSS/bootstrap.min.css' rel='stylesheet'><link href='" + sd + "/CSS/style.css' rel='stylesheet'>");
                                sb.Append("</head>");
                                sb.Append("<body>");

                                sb.Append("<div class='container'><div id='loginbox' style='margin-top:50px;' class='mainbox col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2'><div class='panel panel-info pop_up_margin_top'><div style = 'padding-top:30px' class='panel-body'><div style = 'display:none' id='login-alert' class='alert alert-danger col-sm-12'>AlertMessage</div>");

                                sb.Append("<form action=" + formFieldsModel.FormAction + " id=" + formFieldsModel.FormId + " method='POST' class='form-horizontal'>");


                                //form setting
                                if (formFieldsModel.configId != null)
                                {
                                    sb.Append("<input id='configId' name='configId' type='Hidden' value = " + formFieldsModel.configId + " >");
                                    sb.AppendLine();
                                }
                                if (formFieldsModel.RelatedEntity != null)
                                {
                                    sb.Append("<input id='entity' name='entity' type='Hidden' value = " + formFieldsModel.RelatedEntity + " >");
                                    //sb.Append("</div>");
                                    sb.AppendLine();
                                }
                                //for submit mesage
                                if (formFieldsModel.SubmitMessage!=null)
                                {
                                    sb.Append("<input id='submitMessage' name='submitMessage' type='Hidden' value = '"+formFieldsModel.SubmitMessage+"' >");
                                   // sb.Append("<input id='submitMessage' name='submitMessage' type='Hidden' value = 'Record saved successfully!' >");
                                    //sb.Append("</div>");
                                    sb.AppendLine();
                                }
                                //for submit button text
                                if (formFieldsModel.RedirectUrl != null)
                                {
                                    sb.Append("<input id='redirectUrl' name='redirectUrl' type='Hidden' value = '" + formFieldsModel.RedirectUrl + "' >");
                                    //sb.Append("</div>");
                                    sb.AppendLine();
                                }

                                if (formFieldsModel.RedirectMode != null)
                                {
                                    sb.Append("<input id='redirectMode' name='redirectMode' type='Hidden' value = " + formFieldsModel.RedirectMode + " >");
                                    //sb.Append("</div>");
                                    sb.AppendLine();
                                }
                                if (formFieldsModel.LinkButtonText != null)
                                {
                                    sb.Append("<input id='linkButtonText' name='linkButtonText' type='Hidden' value = '" + formFieldsModel.LinkButtonText + "' >");
                                    //sb.Append("</div>");
                                    sb.AppendLine();
                                }
                                //form setting end


                                foreach (var fields in formFieldsModel.SecoundFormFields.ToList().OrderBy(o=>o.DisplayOrder))
                                {

                                    if (fields.FieldType == "Hidden")
                                    {
                                        //sb.Append("<div style='margin-bottom: 25px ; width:100%;' class='input-group'>");
                                        //sb.Append("<label for=" + fields.FieldName + ">" + fields.DisplayName + "<em class='red'>*</em></label>");

                                        sb.Append("<input id=" + fields.FieldLabel + " name=" + fields.MapField + " type=" + (fields.FieldType == null ? "Text" : fields.FieldType) + " value = " + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + " maxlength=" + (fields.FieldLength == null ? "100" : fields.FieldLength) + " >");
                                        //sb.Append("</div>");
                                        sb.AppendLine();
                                    }
                                    else if (fields.FieldType == "Text")
                                    {

                                        sb.Append("<div style='margin-bottom: 25px ; width:100%;' class='input-group'>");
                                        //sb.Append("<label style='float:'"+ formFieldsModel.LabelPosition+" for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");
                                        if (fields.FieldRequired == "True") {                                         
                                            sb.Append("<label style='float:"+formFieldsModel.LabelPosition+"' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " type=" + fields.FieldType + "   value='"+ (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue )+ "'   data-validation='required' data-validation-error-msg='This is a required field.' class='form-control'  maxlength='" + (fields.FieldLength == null ? "100" : fields.FieldLength) + "' placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'>");
                                        }
                                        else {                                         
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "</label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " type=" + fields.FieldType + "   value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue )+ "' class='form-control'   maxlength='" + (fields.FieldLength == null ? "" : fields.FieldLength )+ "' placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'>");
                                        }
                                        sb.Append("</div>");
                                        sb.AppendLine();
                                    }
                                    else if (fields.FieldType == "MultiLineText")
                                    {
                                        sb.Append("<div style='margin-bottom: 25px ; width:100%;' class='input-group'>");
                                        //sb.Append("<label style='float:'" + formFieldsModel.LabelPosition + " for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");

                                        if (fields.FieldRequired == "True")
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");
                                            sb.Append("<textarea id =" + fields.MapField + " name = " + fields.MapField + "  value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'  data-validation='required' data-validation-error-msg='This is a required field.' class='form-control'  maxlength='" + (fields.FieldLength == null ? "1000" : fields.FieldLength) + "' placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'></textarea>");
                                        }
                                        else
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "</label>");
                                            sb.Append("<textarea id =" + fields.MapField + " name = " + fields.MapField + " value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'  class='form-control'  maxlength='" + (fields.FieldLength == null ? "1000" : fields.FieldLength) + "' placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'></textarea>");
                                        }
                                        sb.Append("</div>");
                                        sb.AppendLine();
                                    }

                                    else if (fields.FieldType == "CheckBox")
                                    {
                                        sb.Append("<div style='margin-bottom: 25px ; width:100%;' class='input-group'>");
                                        //sb.Append("<label style='float:'" + formFieldsModel.LabelPosition + " for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");

                                        if (fields.FieldRequired == "True")
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " type=" + fields.FieldType.ToLower() + " value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'   data-validation='required' data-validation-error-msg='This is a required field.'   maxlength='" + (fields.FieldLength == null ? "1000" : fields.FieldLength) + "' placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'>");
                                        }
                                        else
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "</label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " type=" + fields.FieldType.ToLower() + " value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'    maxlength='" + (fields.FieldLength == null ? "" : fields.FieldLength) + "' placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'>");
                                        }
                                        sb.Append("</div>");
                                        sb.AppendLine();
                                    }

                                    else if (fields.FieldType == "Date")
                                    {
                                        sb.Append("<div style='margin-bottom: 25px ; width:100%;' class='input-group'>");
                                       // sb.Append("<label style='float:'" + formFieldsModel.LabelPosition + " for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");

                                        if (fields.FieldRequired == "True")
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'   type='text'  data-validation='date' data-validation-format='dd/mm/yyyy' data-validation-error-msg='This is a required field with(dd/mm/yyyy).' class='form-control' >");
                                        }
                                        else
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "</em></label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'  type='text' data-validation-format='dd/mm/yyyy' class='form-control' >");
                                        }

                                        sb.Append("</div>");
                                        sb.AppendLine();
                                    }
                                    else if (fields.FieldType == "EmailAddress")
                                    {
                                        sb.Append("<div style='margin-bottom: 25px ; width:100%;' class='input-group'>");
                                        // sb.Append("<label style='float:'" + formFieldsModel.LabelPosition + " for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");

                                        if (fields.FieldRequired == "True")
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " type=" + (fields.FieldType == "EmailAddress" ? "Text" : "Text") + " value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'   data-validation='email'  class='form-control' maxlength='" + (fields.FieldLength == null ? "200" : fields.FieldLength) + "'  placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'>");
                                        }
                                        else
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "</label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " type=" + (fields.FieldType == "EmailAddress" ? "Text" : "Text") + " value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'   class='form-control' maxlength='" + (fields.FieldLength == null ? "200" : fields.FieldLength) + "'  placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'>");
                                        }

                                        sb.Append("</div>");
                                        sb.AppendLine();
                                    }

                                    else if (fields.FieldType == "ZipCode")
                                    {
                                        sb.Append("<div style='margin-bottom: 25px ; width:100%;' class='input-group'>");
                                        //sb.Append("<label style='float:'" + formFieldsModel.LabelPosition + " for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");


                                        if (fields.FieldRequired == "True")
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " type='Text' value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'  data-validation='required' data-validation-error-msg='This is a required field.' class='form-control' maxlength='" + (fields.FieldLength == null ? "6" : fields.FieldLength) + "'  placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'>");
                                        }
                                        else
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " type='text' value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'  class='form-control' maxlength='" + (fields.FieldLength == null ? "6" : fields.FieldLength) + "'  placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'>");
                                        }
                                        sb.Append("</div>");
                                        sb.AppendLine();
                                    }

                                    else if (fields.FieldType == "PhoneNumber")
                                    {
                                        sb.Append("<div style='margin-bottom: 25px ; width:100%;' class='input-group'>");
                                        // sb.Append("<label style='float:'" + formFieldsModel.LabelPosition + " for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");

                                        if (fields.FieldRequired == "True")
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " type='text' value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'  data-validation='required' data-validation-error-msg='This is a required field.' class='form-control' maxlength='" + (fields.FieldLength == null ? "13" : fields.FieldLength) + "'  placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'>");
                                        }
                                        else
                                        {
                                            sb.Append("<label style='float:" + formFieldsModel.LabelPosition + "' for=" + fields.FieldLabel + ">" + fields.FieldLabel + "<em class='red'>*</em></label>");
                                            sb.Append("<input id=" + fields.MapField + " name=" + fields.MapField + " type='text' value='" + (fields.FieldDefaultValue == null ? "" : fields.FieldDefaultValue) + "'  class='form-control' maxlength='" + (fields.FieldLength == null ? "13" : fields.FieldLength) + "'  placeholder='" + (fields.FieldLabel == null ? "" : fields.FieldLabel) + "'>");
                                        }
                                        sb.Append("</div>");
                                        sb.AppendLine();
                                    }
                                }

                           
                                if ((Type == "IFRAME") || (Type == "URL"))
                                    sb.Append("<div style='margin-top:10px' class='form-group'><div class='col-sm-12 controls'><button type = 'submit' class='btn btn-success' id='btn-f-submit' >" + (formFieldsModel.SubmitButtonText == null ? "Submit" : formFieldsModel.SubmitButtonText) + "</button></div></div>");
                               else if (Type== "PREVIEW")
                                    //sb.Append("<div style='margin-top:10px' class='form-group'><div class='col-sm-12 controls'><button type = 'submit' class='btn btn-success' id='btn-f-submit' >" + (formFieldsModel.SubmitButtonText == null ? "Submit" : formFieldsModel.SubmitButtonText) + "</button></div></div>");

                                sb.Append("</form>");
                                sb.Append("</div></div></div></div>");
                                sb.Append("<script src='https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js'></script>");
                                sb.Append("<script src='https://cdnjs.cloudflare.com/ajax/libs/jquery-form-validator/2.3.26/jquery.form-validator.min.js'></script>");
                                sb.Append("<link href='https://cdnjs.cloudflare.com/ajax/libs/jquery-form-validator/2.3.26/theme-default.min.css' rel='stylesheet' type='text/css'/>");
                                sb.Append("<script src='" + sd + "/Scripts/bootstrap.min.js'></script>");
                                sb.Append("<script>$.validate({form: '" + '#' + formFieldsModel.FormId + "', });</script>");

                                sb.Append("</body>");
                               // return Content(sb.ToString(), "text/html");
                                sb.Append("</html>");
                                if (Type == "PREVIEW")                                   
                                 return Json(new { IsSuccess=true, successMessage="Success", htmlResult =sb.ToString() }, JsonRequestBehavior.AllowGet);
                                else if ((Type == "IFRAME") || (Type == "URL"))
                                    ViewBag.result = sb;
                                else
                                    ViewBag.message = "Something going wrong!!! ";

                            }
                            else
                            {
                                message = "Trial pack has expire!!!";
                                if (Type == "PREVIEW")
                                    return Json(new { IsSuccess = false, successMessage = message, }, JsonRequestBehavior.AllowGet);
                                else
                                {
                                  
                                    ViewBag.message = message;
                                }
                            }
                        }
                        else
                        {
                            message = "Please check solution registration and installation exist or not!!!";
                            if (Type == "PREVIEW")
                                return Json(new { IsSuccess = false, successMessage = message, }, JsonRequestBehavior.AllowGet);
                            else
                            ViewBag.message = message;
                        }
                    }
                    else
                    {
                        message = "You are not authorized user!!!";
                        if (Type == "PREVIEW")
                            return Json(new { IsSuccess = false, successMessage = message, }, JsonRequestBehavior.AllowGet);
                        else
                        ViewBag.message = message;
                    }
                }
                else
                {

                    message = "Something going wrong!!!";
                    if (Type == "PREVIEW")
                        return Json(new { IsSuccess = false, successMessage = message, }, JsonRequestBehavior.AllowGet);
                    else
                    ViewBag.message = message;
                }


            }
            catch (Exception ex)
            {
                if (Type == "PREVIEW")
                    return Json(new { IsSuccess = false, successMessage = ex.Message, }, JsonRequestBehavior.AllowGet);
                else
                ViewBag.message = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public ActionResult CrmForm(FormCollection formCollection)
        {
            try
            {
                if (formCollection.AllKeys.Length > 0)
                {
                    Dictionary<string, string> Dctn = new Dictionary<string, string>();

                    foreach (var key in formCollection.Keys)
                    {
                        var Recordkey = key.ToString();
                        var value = formCollection[Recordkey];
                        Dctn.Add(Recordkey, value);
                    }

                                     
                    var Id = Dctn["configId"];
                    Guid configId = Guid.Empty;
                    if (configId != null)
                        configId = Guid.Parse(Id);

                    var tblConfiData = Obj.tbl_Configuration.SingleOrDefault(o => o.Id == configId);
                    if (tblConfiData != null)
                    {
                       
                        //var result = DataInsertTimeUserAuthenticate(tblConfiData.OrgUniqueName, tblConfiData.ServerUrl, tblConfiData.UserName, tblConfiData.Password);

                        var result = new ValuesController().DataInsertTimeUserAuthenticate(tblConfiData.OrgUniqueName, tblConfiData.ServerUrl, tblConfiData.UserName, tblConfiData.Password);
                        if (result.IsSuccess)
                        {
                            OrganizationService _service;
                            CrmConnection connection = CrmConnection.Parse("Url='" + tblConfiData.ServerUrl + "'; Username='" + tblConfiData.UserName + "'; Password='" + tblConfiData.Password + "'");
                            using (_service = new OrganizationService(connection))
                            {
                                Entity entityObj = new Entity(Dctn["entity"]);
                                foreach (var item in Dctn)
                                {
                                    if ((item.Key.ToString() == "entity"))
                                        continue;
                                    else if ((item.Key.ToString() == "configId"))
                                        continue;
                                    else if ((item.Key.ToString() == "submitMessage"))
                                        continue;
                                    else if ((item.Key.ToString() == "redirectUrl"))
                                        continue;
                                    else if ((item.Key.ToString() == "redirectMode"))
                                        continue;
                                    else if ((item.Key.ToString() == "linkButtonText"))
                                        continue;
                                    else
                                        entityObj.Attributes[item.Key] = item.Value;

                                }
                                if (entityObj.Attributes.Count > 0)
                                {
                                    var _entityId = _service.Create(entityObj);
                                    return Json(new { IsSuccess = true, successMessage = (Dctn.ContainsKey("submitMessage") == false ? null : Dctn["submitMessage"].ToString()), redirectMode = (Dctn.ContainsKey("redirectMode") == false ? null : Dctn["redirectMode"].ToString()), linkButtonText = (Dctn.ContainsKey("linkButtonText") == false ? null : Dctn["linkButtonText"].ToString()), redirectUrl = (Dctn.ContainsKey("redirectUrl") == false ? null : Dctn["redirectUrl"].ToString()) });
                                }else                                                               
                                return Json(new { IsSuccess = false, errorMessage = "Entity attributes are not found." });
                                //ViewBag.successMessage = Dctn["submitMessage"];
                                //ViewBag.redirectMode= Dctn["redirectMode"];
                                //ViewBag.redirectUrl = Dctn["redirectUrl"];


                            }


                        }
                        else
                            return Json(new { IsSuccess = false, errorMessage = "User is not authorized please check." });
                    }
                }
                else
                    return Json(new { IsSuccess = false, errorMessage = "The input values are not found." });
            }
            catch (Exception ex)
            {
               
                return Json(new { IsSuccess = false,errorMessage=ex.Message.ToString() });
            }


            return View();
        }


    }





}




