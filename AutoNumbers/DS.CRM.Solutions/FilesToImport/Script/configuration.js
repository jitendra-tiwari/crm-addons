
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
            data: { FirstName: firstname, LastName: lastname, Company: company, ContactNo: phonenumber, Email: email, Address: address, Country: country, State: state, City: city, PostalCode: postalcode, SubscriptionType: type, orgName: orgUniqueName, ServerUrl: serverUrl, UserName: userName, Password: null, SName: "AutoNumber" },
            success: function (response) {
                console.log(response);
                if (response.IsSuccess) {
                    if (response.IsCreated) {
                        $(".loading").hide();
                        $("#tryalertdanger").show();
                        $("#tryalertdanger").text("User registration is successfull. !");
                    }
                    else {
                        $(".loading").hide();
                        $("#tryalertdanger").show();
                        $("#tryalertdanger").text("Records are updated successfully.. !");
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



function CheckConfigurationDetails() {
    $(".loading").show();
    var myserverUrl = parent.Xrm.Page.context.getClientUrl();
    var myorgUniqueName = parent.Xrm.Page.context.getOrgUniqueName();
   

    $.ajax({
        url: "https://crmwebapi.24livehost.com/api/values/LoadDetailsDotsCommon",
        type: "get", //send it through get method
        data: { serverUrl: myserverUrl, orgName: myorgUniqueName },
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

