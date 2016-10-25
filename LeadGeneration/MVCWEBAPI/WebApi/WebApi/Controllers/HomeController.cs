using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        public object MessageBox { get; private set; }

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
                        if (objtbl_Configuration.ExpireDate <= DateTime.Now)
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
