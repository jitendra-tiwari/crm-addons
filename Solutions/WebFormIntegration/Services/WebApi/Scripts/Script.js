$(document).ready(function () {
    $("#optradio_try").prop("checked", true);
    $("#registration-form_subscription").hide();
    $("#optradio_try").click(function () {
        if ($("#optradio_try").prop("checked", true)) {
           // alert("hii")
            $("#registration-form").show();
            $("#registration-form_subscription").hide();
        }
        else
        {
            //$("#optradio_try").prop("checked", true)
            $("#registration-form").hide();
            $("#registration-form_subscription").show();
        }
       
    });
    $("#optradio_subscribe").click(function () {
        if ($("#optradio_subscribe").prop("checked", true)) {
            // alert("hii")
            $("#registration-form").hide();
            $("#registration-form_subscription").show();
            
        }
        else
        {
            $("#registration-form").show();
            $("#registration-form_subscription").hide();
            
        }
   
    });

});