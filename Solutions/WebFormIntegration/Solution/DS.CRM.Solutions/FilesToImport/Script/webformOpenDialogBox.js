function getDialogBox() {


    // Passing parameters to webresource
    var entityId = parent.Xrm.Page.data.entity.getId();    

        var addParams = "eId=" + entityId + "";
        var webresourceurl = "/webresources/wf_/ImportWebResources/dots_webform_DialogBox.html?Data=" + encodeURIComponent(addParams);

        //Passing parameters to webresource
        //var addParams = "Param1=" + param1 + "&Param2=" + param2;
        //var webresourceurl = "/webresources/new_/webresource.htm?Data=" + encodeURIComponent(addParams);

        //If you don't need to pass any parameters use following code instead:
        // var webresourceurl = "/webresources/wf_/ImportWebResources/dots_webform_DialogBox.html";

        var DialogOptions = new parent.Xrm.DialogOptions();
        DialogOptions.width = 620;
        DialogOptions.height = 500;

        //parent.Xrm.Internal.openDialog(Mscrm.CrmUri.create(webresourceurl), DialogOptions, null, null, callBackFunction);
        parent.Xrm.Internal.openDialog(webresourceurl, DialogOptions, null, null, callBackFunction);

       

    }
   


function showHideDisplayButton()
{
    var entityId = parent.Xrm.Page.data.entity.getId();
    if (entityId) {
        return true;
    }
    else {
        return false;
    }
}

function callBackFunction(result) {
    alert(result);
}