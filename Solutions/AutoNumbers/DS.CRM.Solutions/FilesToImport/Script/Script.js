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




function disableIntialNumber() {

  
    var entityId = parent.Xrm.Page.data.entity.getId();
    if (entityId) {
        parent.Xrm.Page.getControl("new_initializenumber").setDisabled(true);
        parent.Xrm.Page.getControl("new_fieldformat").setDisabled(true);

    } else {
        parent.Xrm.Page.getControl("new_initializenumber").setDisabled(false);
        parent.Xrm.Page.getControl("new_fieldformat").setDisabled(false);
    }
   
}