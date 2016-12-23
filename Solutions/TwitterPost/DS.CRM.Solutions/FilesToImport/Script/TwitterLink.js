$(document).ready(function () {
    var entityId = parent.Xrm.Page.data.entity.getId();
    if (entityId) {
        //ShowAccountConnectButton();
        CheckConfigurationDetails(entityId);
    }
});




function ShowAccountConnectButton() {
      
    $("#twr_button").html('<div><img id="twitterImg" src="Images/Twitter.png"><br /><input type="button" id="btnAccount"  value="Connect To Account" onclick="checkPublisher();" /></div>');

    
}

function refreshWebresource() {
    var entityId = parent.Xrm.Page.data.entity.getId();
    if (entityId) {
        parent.Xrm.Page.getControl('WebResource_LinkTwitter').getObject().contentWindow.location.reload();
    }
}
function GetToken() {

    //var userUrl = "https://crmwebapi.24livehost.com/Home/BeginAsync";
    
    var entityId = parent.Xrm.Page.data.entity.getId();
    var serverUrl = parent.Xrm.Page.context.getClientUrl();
    //var publisher = parent.Xrm.Page.getControl('dots_alias').getValue();
    var publisher = parent.Xrm.Page.getAttribute('dots_alias').getValue();
    var mediaOptionsetValue = parent.Xrm.Page.getAttribute('dots_media').getValue();

    $.ajax({

        url: "https://crmwebapi.24livehost.com/Home/BeginAsync",
        type: "get", //send it through get method
        // data: { serverUrl: myserverUrl, orgName: myorgUniqueName },
        data: { rowId: entityId, url: serverUrl, publisher: publisher, media: mediaOptionsetValue },
        success: function (response) {
           
            if (response != "Error") {
                var myWindow = window.open(response, "MsgWindow", "width=600,height=600");
               
            }
            else {
                alert("Error, Please Try again letter!");
            }

        },
        error: function (xhr) {

        }
    });
}

function checkPublisher() {
    var entityId = parent.Xrm.Page.data.entity.getId();
    $.ajax({
        url: "https://crmwebapi.24livehost.com/api/values/TwitterUser",
        type: "get", //send it through get method
        // data: { serverUrl: myserverUrl, orgName: myorgUniqueName },
        data: { rowId: entityId },
        success: function (response) {
                    
            if (response.IsSuccess && response.Message == "Success") {
                alert("This Publisher is already Created!");
            } else if (response.IsSuccess==false && response.Message == "Failed") {
                GetToken();
            }
           

        },
        error: function (xhr) {
            alert("Error");
        }
    });
}

function CheckConfigurationDetails(entityId) {
     


        $.ajax({           
            url: "https://crmwebapi.24livehost.com/api/values/TwitterUser",
            type: "get", //send it through get method
            // data: { serverUrl: myserverUrl, orgName: myorgUniqueName },
            data: { rowId:entityId },
            success: function (response) {
               
                if (response.IsSuccess) {
                   
                    $("#twr_button").html("");
                    $("#twr_button").html('<div><img id="twitterImg" src="Images/Twitter.png"> <img id="twitterProfileImg" src=' + response.Image_Url + '></div>');
                    //refreshWebresource();
                }
                else if(response.IsSuccess==false && response.Message=="Failed") {
                    ShowAccountConnectButton();
                    
                }
                else if (response.IsSuccess == false && response.Message == "InValidRowId") {
                    
                }
                else
                {
                    alert(response.Message);
                }

            },
            error: function (xhr) {
              
            }
        });



  

}

function SelectTwitterPostConfigurationDetails() {

    parent.Xrm.Page.ui.tabs.get('{2a6cf850-7053-4e00-b7c7-5f5c285225aa}').setVisible(false);
    //var entityId = parent.Xrm.Page.data.entity.getId();    

    //parent.Xrm.Page.getControl("dots_alias").setVisible(false);
    //parent.Xrm.Page.getControl("dots_targetattributelogicalname").setVisible(false);
    //parent.Xrm.Page.getControl("dots_placeholder").setDisabled(true);
    //parent.Xrm.Page.getControl("dots_attributename").setDisabled(true);
    //parent.Xrm.Page.getControl("dots_maxlength").setDisabled(true);
    //parent.Xrm.Page.getControl("dots_targetentityname").setDisabled(true);
    //parent.Xrm.Page.getControl("dots_targetattributename").setDisabled(true);


    var serverUrl = parent.Xrm.Page.context.getClientUrl();
    try {

        var oDataUri = serverUrl + "/XRMServices/2011/OrganizationData.svc/dots_twitterconfigurationSet";

        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            url: oDataUri,
            beforeSend: function (XMLHttpRequest) { XMLHttpRequest.setRequestHeader("Accept", "application/json"); },
            success: function (data, textStatus, XmlHttpRequest) {
                var output = data.d.results;
                if (output.length == 0) {
                    parent.Xrm.Page.ui.tabs.get('{2a6cf850-7053-4e00-b7c7-5f5c285225aa}').setVisible(false);
                    alert("Please Register Goto-->Setting->Solutions-->select solution and register!");

                }
                else {
                    parent.Xrm.Page.ui.tabs.get('{2a6cf850-7053-4e00-b7c7-5f5c285225aa}').setVisible(true);
                }


            },
            error: function (XmlHttpRequest, textStatus, errorThrown) {

                alert("error has occured");


            }
        });
    }
    catch (err) {
        alert(err);

    }
}
