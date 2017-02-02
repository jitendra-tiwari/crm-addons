using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        public string Image_Url { get; set; }
    }

    public class TweetViewModel
    {
        [DisplayName("Image")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [DisplayName("Screen Name")]
        public string ScreenName { get; set; }

        [DisplayName("Tweet")]
        public string Text { get; set; }

       
    }

    public class TwitterPublisherModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<PublisherModel> PublishersModel { get; set; }
        //public string CRMUrl { get; set; }       
        //public string SocialMediaType { get; set; }
        //public string Row_Id { get; set; }
        //public string UserID { get; set; }
        //public string OauthToken { get; set; }
        //public string OauthTokenSecret { get; set; }
        //public string ScreenName { get; set; }
        //public string Image_Url { get; set; }
        //public DateTime AuthenticateDate { get; set; }
        //public bool IsSuccess { get; set; }
        //public string Message { get; set; }
        //public string Image_Url { get; set; }


    }

    public class PublisherModel
    {
        public Guid Id { get; set; }
        public string PublisherName { get; set; }
    }

}