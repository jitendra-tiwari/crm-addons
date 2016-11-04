
$(document).ready(function () {
    // getValidUser();  
       
    CheckConfigurationDetails();

        $("#registration-form").submit(function (event) {
            event.preventDefault();           
            getValidUser();
        });
       
});


function getValidUser()
{
    
    //  var IsTrue = $("#configurationForm").validate();
    var trialId = "", subId = "", type = "";
    
    var serverUrl = parent.Xrm.Page.context.getClientUrl();
    var orgUniqueName = parent.Xrm.Page.context.getOrgUniqueName();

    trialId = $("input[id='optradio_try']:checked").attr("id");
    subId = $("input[id='optradio_subscribe']:checked").attr("id");
   
    if (typeof trialId != "undefined") {

        //type = $("label[for='" + trialId + "']").text();
        type = "Trial";
    }
    else if (typeof subId != "undefined")
    {
        //type = $("label[for='" + trialId + "']").text();
        type = "Subscribe";
    }
   
 

    var firstname = $("#trytxtfirstname").val();
    var lastname = $("#trytxtlastname").val();
    var email = $("#trytxtemail").val();

    var company = $("#trytxtcompany").val();
    var phonenumber = $("#trytxtphonenumber").val();
    var address = $("#trytxtaddress").val();
    var country = $("#trytxtcountry").val();
    var state = $("#trytxtstate").val();
    var city = $("#trytxtcity").val();

    var username = $("#trytxtusername").val();
    var password = $("#trytxtpassword").val();
    var postalcode="44";
    $(".loading").show();
   
    try
    {
   
        var oDataUri = serverUrl + "/XRMServices/2011/OrganizationData.svc/dots_configuration001Set?$filter=new_orguniquename eq '" + orgUniqueName + "' and new_serverurl eq '" + serverUrl + "' ";

        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            url: oDataUri,
            beforeSend: function (XMLHttpRequest) { XMLHttpRequest.setRequestHeader("Accept", "application/json"); },
            success: function (data, textStatus, XmlHttpRequest) {


                var output = data.d.results;
                console.log("------------check configuration-----------------");
                console.log(output);
                if (output.length > 0) {
                    //for update
                    console.log(output[0].new_registerid);
                    var tblconfigId = output[0].dots_configuration001Id;
                    CallConfigurationService(firstname, lastname, company, phonenumber, email, address, country, state, city, postalcode, type, orgUniqueName, serverUrl, username, password, output[0].new_registerid, tblconfigId);
                }
                else {
                    //for insert new 
                    CallConfigurationService(firstname, lastname, company, phonenumber, email, address, country, state, city, postalcode, type, orgUniqueName, serverUrl, username, password, null,null);
                }
            
            },
            error: function (XmlHttpRequest, textStatus, errorThrown) {           
                $(".loading").hide();
                $("#tryalertdanger").show();
                $("#tryalertdanger").text("Something going to wrong try again!");


            }
        });
    }
    catch(err) {       
        $(".loading").hide();
        $("#tryalertsuccess").hide();
        $("#tryalertdanger").show();
        $("#tryalertdanger").text(err.message);
    }
    
}


function CallConfigurationService(firstname, lastname, company, phonenumber, email, address, country, state, city, postalcode, type, orgUniqueName, serverUrl, username, password, registrationId,tblconfigId)
{
  
    $.ajax({
        url: "https://crmwebapi.24livehost.com/api/values/UserAuthenticate",
        //url: "http://localhost:54126/api/values/UserAuthenticate",
        type: "get", //send it through get method
        data: { FirstName: firstname, LastName: lastname, Company: company, ContactNo: phonenumber, Email: email, Address: address, Country: country, State: state, City: city, PostalCode: postalcode, SubscriptionType: type, orgName: orgUniqueName, ServerUrl: serverUrl, UserName: username, Password: password, RegisterId: registrationId },
        success: function (response) {
            console.log(response);
            if (response.IsSuccess) {
                if (response.IsCreated) {

                    SaveConfigurationDetails(response.Id, serverUrl, orgUniqueName, response.UserName, response.EPassword);
                }
                else {
                    UpdateConfigurationDetails(response.Id, serverUrl, orgUniqueName, response.UserName, response.EPassword, tblconfigId);
                }


            }
            else {
                $(".loading").hide();
                $("#tryalertdanger").show();
                $("#tryalertdanger").text("Please check your credential otherwise contact to admin.!");
            }
        },
        error: function (xhr) {
            $(".loading").hide();
            //Do Something to handle error
            $("#tryalertdanger").show();
            $("#tryalertdanger").text("Some error occur.!");
        }
    });
}
function SaveConfigurationDetails(RegisterId,serverUrl, orgUniqueName, username, password) {

    if (serverUrl != "") {
        var context = parent.Xrm.Page.context;
        var serverUrl = context.getClientUrl();
        var ODATA_ENDPOINT = "/XRMServices/2011/OrganizationData.svc";
        var CRMObject = new Object();
        ///////////////////////////////////////////////////////////// 
        // Specify the ODATA entity collection 
        var ODATA_EntityCollection = "/dots_configuration001Set";
        ///////////////////////////////////////////////////////////// 
        // Define attribute values for the CRM object you want created 

       
        CRMObject.new_serverurl = serverUrl;
        CRMObject.new_registerid = RegisterId;
        CRMObject.new_orguniquename = orgUniqueName;
        CRMObject.new_username = username;
        CRMObject.new_password = password;

        //Parse the entity object into JSON 
        var jsonEntity = window.JSON.stringify(CRMObject);
        //Asynchronous AJAX function to Create a CRM record using OData 
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            url: serverUrl + ODATA_ENDPOINT + ODATA_EntityCollection,
            data: jsonEntity,
            beforeSend: function (XMLHttpRequest) {
                //Specifying this header ensures that the results will be returned as JSON. 
                XMLHttpRequest.setRequestHeader("Accept", "application/json");
            },
            success: function (data, textStatus, XmlHttpRequest) {

                var NewCRMRecordCreated = data["d"];
                $(".loading").hide();
                console.log(data["d"]);
                $("#tryalertsuccess").show();
                $("#tryalertdanger").hide();
                $("#tryalertsuccess").text("User Authenticate Successfully!");
                //appned button to div for open popup
                AppendButton();
                //create url and set value to popup
                CreateUrl(RegisterId);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $(".loading").hide();
                $("#tryalertdanger").show();
                $("#tryalertdanger").text("Please check your credential otherwise contact to admin.!");
                
            }
        });
    }

}

function UpdateConfigurationDetails(RegisterId, serverUrl, orgUniqueName, username, password, tblconfigId) {

  
    var CRMObject = new Object();
    ///////////////////////////////////////////////////////////// 
    // Specify the ODATA entity collection 
    var ODATA_EntityCollection = "dots_configuration001Set";
    ///////////////////////////////////////////////////////////// 
    // Define attribute values for the CRM object you want created 


    CRMObject.new_serverurl = serverUrl;
    //CRMObject.new_registerid = RegisterId;
    CRMObject.new_orguniquename = orgUniqueName;
    CRMObject.new_username = username;
    CRMObject.new_password = password;

    //var crmUrl = parent.Xrm.Page.context.getClientUrl() + "/XRMServices/2011/OrganizationData.svc/dots_configuration001Set?$filter=new_registerid eq '" + RegisterId + "'";
    var crmUrl = parent.Xrm.Page.context.getClientUrl() + "/XRMServices/2011/OrganizationData.svc/dots_configuration001Set(guid'"+ tblconfigId + "')";
    var jsonEntity = window.JSON.stringify(CRMObject);
    //The OData end-point
    var ODATA_ENDPOINT = "/XRMServices/2011/OrganizationData.svc";
    //Asynchronous AJAX function to Update a CRM record using OData
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        data: jsonEntity,
        url: crmUrl,
        beforeSend: function (XMLHttpRequest) {
            //Specifying this header ensures that the results will be returned as JSON.
            XMLHttpRequest.setRequestHeader("Accept", "application/json");
            //Specify the HTTP method MERGE to update just the changes you are submitting.
            XMLHttpRequest.setRequestHeader("X-HTTP-Method", "MERGE");
        },
        success: function (data, textStatus, XmlHttpRequest) {
                     
            console.log(data);                    
            $(".loading").hide();
            $("#tryalertdanger").hide();
            $("#tryalertsuccess").show();
            $("#tryalertsuccess").text("Record updated Successfully!");
            //  alert("order updated successfully !");


        },
        error: function (XmlHttpRequest, textStatus, errorThrown) {
                           
                $(".loading").hide();
                $("#tryalertdanger").show();
                $("#tryalertdanger").text("Error while updating");
           
        }
    });



}

function CheckConfigurationDetails() {
    $(".loading").show();
    var myserverUrl = parent.Xrm.Page.context.getClientUrl();
    var myorgUniqueName = parent.Xrm.Page.context.getOrgUniqueName();

    var oDataUri = myserverUrl + "/XRMServices/2011/OrganizationData.svc/dots_configuration001Set?$filter=new_orguniquename eq '" + myorgUniqueName + "' and new_serverurl eq '" + myserverUrl + "' ";
   
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        url: oDataUri,
        beforeSend: function (XMLHttpRequest) { XMLHttpRequest.setRequestHeader("Accept", "application/json"); },
        success: function (data, textStatus, XmlHttpRequest) {
                      
            var output = data.d.results;            
            console.log(output);
            if (output.length > 0) {
                console.log(output[0].new_registerid);
                LoadConfigurationService(output[0].new_registerid);
            }
            else
            {
                $(".loading").hide();

            }
           
               
            

        },
        error: function (XmlHttpRequest, textStatus, errorThrown) {
            //alert('OData Select Failed: ' + oDataUri);
            $(".loading").hide();
            $("#tryalertdanger").show();
            $("#tryalertdanger").text("Some error occur.!");

        }
    });

   
   
   
}
function LoadConfigurationService(registrationId) {

    $.ajax({
        url: "https://crmwebapi.24livehost.com/api/values/LoadDetails",
        type: "get", //send it through get method
        data: { RegisterId: registrationId },
        success: function (response) {
            console.log(response);
            if (response.IsSuccess) {
                $("#trytxtfirstname").val(response.FirstName);
                $("#trytxtlastname").val(response.LastName);
                $("#trytxtemail").val(response.Email);

                $("#trytxtcompany").val(response.Company);
                $("#trytxtphonenumber").val(response.ContactNo);
                $("#trytxtaddress").val(response.Address);
                $("#trytxtcountry").val(response.Country);
                $("#trytxtstate").val(response.State);
                $("#trytxtcity").val(response.City);

                $("#trytxtusername").val(response.UserName);
                $("#solutionStatus").text(response.SubscriptionType);
                var date = new Date(response.ExpireDate);
                var fulldate = ((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
                $("#solutionExpires").text(fulldate);
                // $("#trytxtpassword").val(response.Password); 

                //appned button to div for open popup
                AppendButton();
                //create url and set value to popup
                CreateUrl(registrationId);

                $(".loading").hide();

            }
            else {

                $(".loading").hide();
            }

        },
        error: function (xhr) {
            $(".loading").hide();
            //Do Something to handle error
            $("#tryalertdanger").show();
            $("#tryalertdanger").text(xhr.responseText);
        }
    });
}
function AppendButton() {
    $('#mydivbutton').append('<button id="btnurlpopup" data-target="#myModalUrl" data-toggle="modal">Get Url</button>');
}

function CreateUrl(RegisterId) {
    var link = "https://crmwebapi.24livehost.com/Home/CrmForm?rId=" + RegisterId + "";
    $("#txtUrl-Link").val(link);
    $("#txtIframe-Src").val("<iframe style='width:500px;height:300px' src='" + link + "'></iframe>");
}
