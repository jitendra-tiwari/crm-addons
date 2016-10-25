
$(document).ready(function () {
   // getValidUser();  

        $("#registration-form").submit(function (event) {
            event.preventDefault();
            alert('call function here');
            getValidUser();
        });
       
});
function getValidUser()
{
    
    //  var IsTrue = $("#configurationForm").validate();
    var trialId = "",subId="",type="";
    var serverUrl = parent.Xrm.Page.context.getClientUrl();
    var orgUniqueName = parent.Xrm.Page.context.getOrgUniqueName();

    trialId = $("input[id='optradio_try']:checked").attr("id");
    subId = $("input[id='optradio_subscribe']:checked").attr("id");
    if (typeof trialId === "undefined") {

        type = $("label[for='" + trialId + "']").text();
    }
    else if (typeof subId === "undefined")
    {
        type = $("label[for='" + trialId + "']").text();
    }

    var firstname = $("#trytxtfirstname").val();
    var lastname = $("#trytxtlastname").val();
    var email = $("#trytxtemail").val();

    var company = $("#trytxtcompany").val();
    var phonenumbar = $("#trytxtphonenumbar").val();
    var address = $("#trytxtemail").val();
    var country = $("#trytxtcountry").val();
    var state = $("#trytxtstate").val();
    var city = $("#trytxtcity").val();

    var username = $("#trytxtusername").val();
    var password = $("#trytxtpassword").val();

    $(".loading").show();
    //if(IsTrue)
    //{
    
        $.ajax({
            url: "https://crmwebapi.24livehost.com/api/values/UserAuthenticate",
            type: "get", //send it through get method
            data: {FirstName:firstname,LastName:lastname, Company:company,ContactNo:phonenumbar,Email:email,Address:address,Country:country,State:state,City:city,PostalCode:"",SubscriptionType:type,orgName: orgUniqueName,ServerUrl:serverUrl, UserName: username, Password: password },
            success: function (response) {
                console.log(response);
                if (response.IsSuccess) {
                    SaveConfigurationDetails(response.Id, serverUrl, orgUniqueName, username, password);
                }
                else {
                    $(".loading").hide();
                    $("#tryalertdanger").text("Please check your credential otherwise contact to admin.!");
                }
            },
            error: function (xhr) {
                $(".loading").hide();
                //Do Something to handle error
                alert("error occurs");
            }
        });
           
        
    //}
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
                $("#tryalertsuccess").text("User Authenticate Successfully!");
               

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $(".loading").hide();
                $("#tryalertdanger").text("Please check your credential otherwise contact to admin.!");
                alert("failure");
            }
        });
    }

}