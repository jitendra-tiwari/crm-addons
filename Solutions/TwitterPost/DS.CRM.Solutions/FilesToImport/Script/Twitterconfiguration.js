
$(document).ready(function () {
    // getValidUser();  
    var srUrl = parent.Xrm.Page.context.getClientUrl();
    SelectConfigurationDetails(srUrl);

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
    var userName = parent.Xrm.Page.context.getUserName();
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

   // var username = $("#trytxtusername").val();
   // var password = $("#trytxtpassword").val();
    var postalcode="44";
    $(".loading").show();
   
    try
    {
        $.ajax({
            url: "https://crmwebapi.24livehost.com/api/values/DotsCommon",
            //url: "http://localhost:54126/api/values/UserAuthenticate",
            type: "get", //send it through get method
            data: { FirstName: firstname, LastName: lastname, Company: company, ContactNo: phonenumber, Email: email, Address: address, Country: country, State: state, City: city, PostalCode: postalcode, SubscriptionType: type, orgName: orgUniqueName, ServerUrl: serverUrl, UserName: userName, Password: null, SName: "TwitterAccount" },
            success: function (response) {
                console.log(response);
                if (response.IsSuccess) {
                    if (response.IsCreated) {
                        SaveConfigurationDetails(serverUrl, response.Id, "test");
                        $(".loading").hide();
                        $("#tryalertsuccess").show();
                        $("#tryalertsuccess").text("User registration is successfull. !");
                    }
                    else {
                        $(".loading").hide();
                        $("#tryalertsuccess").show();
                        $("#tryalertsuccess").text("Records are updated successfully. !");
                    }
                }
                else {
                    $(".loading").hide();
                    $("#tryalertdanger").show();
                    $("#tryalertdanger").text(response.Error);
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
    catch(err) {       
        $(".loading").hide();
        $("#tryalertsuccess").hide();
        $("#tryalertdanger").show();
        $("#tryalertdanger").text(err.message);
    }
    
}



function CheckConfigurationDetails(registerid) {
    
    if (registerid != null) {
        $(".loading").show();
        $("#btn-try").hide();
       
        var myserverUrl = parent.Xrm.Page.context.getClientUrl();
        var myorgUniqueName = parent.Xrm.Page.context.getOrgUniqueName();


        $.ajax({
            // url: "https://crmwebapi.24livehost.com/api/values/LoadDetailsDotsCommon",
            url: "https://crmwebapi.24livehost.com/api/values/LoadDetails",
            type: "get", //send it through get method
            // data: { serverUrl: myserverUrl, orgName: myorgUniqueName },
            data: { registerId: registerid },
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

                    // $("#trytxtusername").val(response.UserName);
                    $("#solutionStatus").text(response.SubscriptionType);
                    var date = new Date(response.ExpireDate);
                    var fulldate = ((date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
                    $("#solutionExpires").text(fulldate);
                    // $("#trytxtpassword").val(response.Password);

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

}


function SaveConfigurationDetails(serverUrl, RegisterId, encryptedValue) {

    if (serverUrl != "") {
        var context = parent.Xrm.Page.context;
        var serverUrl = context.getClientUrl();
        var ODATA_ENDPOINT = "/XRMServices/2011/OrganizationData.svc";
        var CRMObject = new Object();
        ///////////////////////////////////////////////////////////// 
        // Specify the ODATA entity collection 
        var ODATA_EntityCollection = "/dots_twitterconfigurationSet";
        ///////////////////////////////////////////////////////////// 
        // Define attribute values for the CRM object you want created 


        CRMObject.dots_type = RegisterId;
        CRMObject.dots_value = encryptedValue;
    

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
                $("#btn-try").hide();
                $(".loading").hide();
                console.log(data["d"]);

                //$("#tryalertsuccess").show();
                //$("#tryalertdanger").hide();
                //$("#tryalertsuccess").text("User Authenticate Successfully!");


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $(".loading").hide();
                //$("#tryalertdanger").show();
                //$("#tryalertdanger").text("Please check your credential otherwise contact to admin.!");

            }
        });
    }

}

function SelectConfigurationDetails(serverUrl)
{
    $(".loading").show();
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
                console.log("====Now Test====");
                console.log(output);
                if (output.length==0) {
                     //SaveConfigurationDetails(serverUrl, RegisterId, encryptedValue);
                   // CheckConfigurationDetails(null);
                    //for update
                    // console.log(output[0].new_type);
                    // var tblconfigId = output[0].dots_configuration001Id;
                   
                    $(".loading").hide();
                }
                else {
                    CheckConfigurationDetails(output[0].dots_type);
                }
               

            },
            error: function (XmlHttpRequest, textStatus, errorThrown) {
                $(".loading").hide();
                //$("#tryalertdanger").show();
                //$("#tryalertdanger").text("Something going to wrong try again!");


            }
        });
    }
    catch (err) {
        $(".loading").hide();
        //$("#tryalertsuccess").hide();
        //$("#tryalertdanger").show();
        //$("#tryalertdanger").text(err.message);
    }
}



