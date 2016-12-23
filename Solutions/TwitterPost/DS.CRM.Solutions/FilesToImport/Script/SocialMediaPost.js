var selectbox;
var result;
$(document).ready(function () {
   
    getPublisher();
});

function getPublisher() {
  var serverUrl = parent.Xrm.Page.context.getClientUrl();
  // var serverUrl = "https://dotsquares5.crm.dynamics.com";
   
    $.ajax({
        url: "https://crmwebapi.24livehost.com/api/values/GetPublisher",
       // url: "http://localhost:54126/api/values/GetPublisher",
        type: "get", //send it through get method      
        data: { CrmUrl: serverUrl },
        success: function (response) {           
            //console.log(response);
           
            selectbox = document.getElementById("selectbox_Publisher");
            if (response.IsSuccess) {
                for (var i = 0; i < response.PublishersModel.length; i++) {
                    var attribute = response.PublishersModel[i];

                    var optn = document.createElement("option");
               
                    optn.text = attribute.PublisherName;

                    optn.value = attribute.Id;

                    selectbox.options.add(optn);
                
                }
            }
            else {
                alert(response.Message);
            }
        },
        error: function (xhr) {

            alert("Some error has occured!");
        }
    });

}

function getMediaResponse()
{
    var publisher = $("#selectbox_Publisher").val();
    var media = $("#selectbox_Media").val();
  
    if (publisher != "" && media != "") {
        $.ajax({
            url: "https://crmwebapi.24livehost.com/Home/HomeTimeline",
            type: "get", //send it through get method      
            data: { id: publisher, mediaType:media },
            success: function (response) {               
                if (response.IsSuccess) {                 
                   
                    for (var i = 0; i < response.UserTweet.length; i++) {
                        var attribute = response.UserTweet[i];   
                        var html = "<tr><td><img src=" + attribute.ImageUrl + " alt=" + attribute.ScreenName + " /></td><td>" + attribute.ScreenName + "</td><td>" + attribute.Text + "</td></tr>";
                       result += html;
                    }
                    var tbl = "<table class='table'><tr><th>ImageUrl</th><th> ScreenName</th><th>Text</th><th></th></tr>"+result+"</table>";                   

                    $("#media_rows").html("");
                    $("#media_rows").html(tbl);
                }
                else {
                   
                    parent.Xrm.Utility.alertDialog(response.Message);
                }
            },
            error: function (xhr) {

                alert("Some error has occured!");
            }
        });
    }
    else if (publisher == "" && media == "") {
        parent.Xrm.Utility.alertDialog("Please select both publisher and mediaType.");
    }
    else if (publisher == "") {
        parent.Xrm.Utility.alertDialog("Please select  publisher.");
    }
    else if (media == "") {
        parent.Xrm.Utility.alertDialog("Please select mediaType.");
    }
}

