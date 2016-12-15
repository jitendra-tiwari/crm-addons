using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    //public class TwitterAPIModel
    //{

    //    public Guid Id { get; set; }
    //    public string Row_Id { get; set; }
    //    public string UserID { get; set; }
    //    public string OauthToken { get; set; }
    //    public string OauthTokenSecret { get; set; }
    //    public string ScreenName { get; set; }
    //    public string Image_Url { get; set; }
    //    public DateTime AuthenticateDate { get; set; }
    //    public string IsSuccess { get; set; }


    //}
    public class TwitterAPISuccesModel
    {
        public Guid Id { get; set; }
        public string Row_Id { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        //public string Image_Url { get; set; }
    }
}