using LinqToTwitter;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
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
        public async Task<ActionResult> BeginAsync(string rowId)
        {
            //var auth = new MvcSignInAuthorizer          
           
            string pathurl = "";

           
                if (!string.IsNullOrEmpty(rowId))
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
                    await auth.BeginAuthorizationAsync(new Uri(twitterCallbackUrl + "?rowId=" + rowId));

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

               
                tbl_Twitter tbltwr = new tbl_Twitter();
                Guid newId = Guid.NewGuid();
                tbltwr.Id = newId;
                tbltwr.Row_Id = rowId;
                tbltwr.UserID = auth.CredentialStore.UserID.ToString();
                tbltwr.OauthToken = auth.CredentialStore.OAuthToken;
                tbltwr.OauthTokenSecret = auth.CredentialStore.OAuthTokenSecret;
                tbltwr.ScreenName = auth.CredentialStore.ScreenName;
                tbltwr.Image_Url = null;
                tbltwr.AuthenticateDate = DateTime.Now;
                Obj.tbl_Twitter.Add(tbltwr);
                Obj.SaveChanges();

               return RedirectToAction("Success", "Home");
                //return RedirectToAction("BeginAsync", new { rowId = "Done" });
                //return Json(new { Id= newId.ToString(),IsSucces=true,Message="Success" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { IsSucces=false , Message = ex.Message.ToString()}, JsonRequestBehavior.AllowGet);
            }
            //return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [ActionName("Tweet")]
        public async Task<ActionResult> TweetAsync(string text ,string rowId)
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
            catch(Exception ex)
            {
                
                return Json(new { IsSuccess = false, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
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
                var rs= httpResponse.Content.ReadAsStringAsync();
               var  model =  JsonConvert.DeserializeObject<HtmlConversion>(rs.Result);
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
                query.EntityName = "dots_configuration001";
                query.ColumnSet = new ColumnSet(true);

                EntityCollection col = _service.RetrieveMultiple(query);

                Entity SampleCofiguration = null;
                if (col != null && col.Entities.Count > 0)
                {
                    SampleCofiguration = col.Entities[0];
                    //for solution installed and also registerd
                    if (SampleCofiguration["new_registerid"].ToString() != null)
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
        public async Task<ActionResult> CrmForm(Guid rId)
        {
            
            WebApi.Database.MicrosoftCRMEntities Obj = new Database.MicrosoftCRMEntities();
            try
            {
                string message = "";
                if (rId != Guid.Empty)
                {
                    var objtbl_Configuration = Obj.tbl_Configuration.SingleOrDefault(o => o.Id == rId);
                    
                    if (objtbl_Configuration != null)
                    {
                        //check solution uninstalled or not
                        if (IsSolutionUnInstall(objtbl_Configuration) == false)
                        {
                            if (DateTime.Now.Date <= objtbl_Configuration.ExpireDate.Date)
                            {
                                string getUrlName = "GetForm";
                                var AbsoluteUri = Request.Url.AbsoluteUri;
                                var sd = Request.Url.GetLeftPart(UriPartial.Authority);
                                string ApiUrl = AbsoluteUri.Replace(AbsoluteUri, sd + "/api/values/" + getUrlName.ToLower());
                                HttpClient httpClient = new HttpClient();


                                string server = "http://wds1.projectstatus.co.uk/CRMWebAPI/api/values/GetForm";
                                //string local = "http://localhost:54126/api/values/GetForm";
                                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ApiUrl);
                                HttpResponseMessage response = await httpClient.SendAsync(request);
                                string result = await response.Content.ReadAsStringAsync();
                                var rs = JsonConvert.DeserializeObject<HtmlConversion>(result);
                                string body = rs.m_StringValue;
                                body = body.Replace("###5", rId.ToString());

                                ViewBag.result = body;

                                ViewBag.url = Request.Url.AbsoluteUri;
                            }
                            else
                            {
                                message = "Trial pack has expire!!!";
                                ViewBag.message = message;
                            }
                        }
                        else
                        {
                            message = "Please check solution registration and installation exist or not!!!";
                            ViewBag.message = message;
                        }
                    }
                    else
                    {
                        message = "You are not authorized user!!!";
                        ViewBag.message = message;
                    }
                }
               

            }
            catch(Exception ex)
            {
                ViewBag.message = ex.Message;
            }
            return View();
        }
    }


}
