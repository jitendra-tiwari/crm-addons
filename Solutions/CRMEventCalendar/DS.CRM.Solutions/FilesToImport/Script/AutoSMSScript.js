$(document).ready(function () {
    $("#optradio_try").prop("checked", true);
    $("#registration-form_subscription").hide();
    $("#optradio_try").click(function () {
        if ($("#optradio_try").prop("checked")) {
            // alert("hii")
            $("#registration-form").show();
            $("#registration-form_subscription").hide();
        }
        else {
            //$("#optradio_try").prop("checked", true)
            $("#registration-form").hide();
            $("#registration-form_subscription").show();
        }

    });
    $("#optradio_subscribe").click(function () {
        if ($("#optradio_subscribe").prop("checked")) {
            // alert("hii")
            $("#registration-form").hide();
            $("#registration-form_subscription").show();

        }
        else {
            $("#registration-form").show();
            $("#registration-form_subscription").hide();

        }

    });

});




function SelectConfigurationDetails(serverUrl) {   
    try {

        var oDataUri = serverUrl + "/XRMServices/2011/OrganizationData.svc/AppointmentSet";

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
                if (output.length == 0) {
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