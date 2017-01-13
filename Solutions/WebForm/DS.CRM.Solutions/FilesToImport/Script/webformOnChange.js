

function redirectModeOnChange() {
    var redirectModeValue = parent.Xrm.Page.getAttribute("new_redirectmode").getValue();
    if (redirectModeValue == null) {
        parent.Xrm.Page.getControl("dots_linkbuttontext").setVisible(false);
        parent.Xrm.Page.getAttribute("dots_linkbuttontext").setRequiredLevel("none");

        parent.Xrm.Page.getControl("dots_redirecturl").setVisible(false);
        parent.Xrm.Page.getAttribute("dots_redirecturl").setRequiredLevel("none");
        
    } else if (redirectModeValue=="1") {
        parent.Xrm.Page.getControl("dots_linkbuttontext").setVisible(false);
        parent.Xrm.Page.getAttribute("dots_linkbuttontext").setRequiredLevel("none");

        parent.Xrm.Page.getControl("dots_redirecturl").setVisible(true);
        parent.Xrm.Page.getAttribute("dots_redirecturl").setRequiredLevel("required");
    }

    else if (redirectModeValue == "2" || redirectModeValue == "3") {
        parent.Xrm.Page.getControl("dots_linkbuttontext").setVisible(true);
        parent.Xrm.Page.getAttribute("dots_linkbuttontext").setRequiredLevel("required");

        parent.Xrm.Page.getControl("dots_redirecturl").setVisible(true);
        parent.Xrm.Page.getAttribute("dots_redirecturl").setRequiredLevel("required");
    }
    parent.Xrm.Page.getAttribute("new_redirectmode").addOnChange(applyValidation);
}

function applyValidation() {
    var redirectModeValue = parent.Xrm.Page.getAttribute("new_redirectmode").getValue();
    if (redirectModeValue == null) {
        parent.Xrm.Page.getControl("dots_linkbuttontext").setVisible(false);
        parent.Xrm.Page.getAttribute("dots_linkbuttontext").setRequiredLevel("none");

        parent.Xrm.Page.getControl("dots_redirecturl").setVisible(false);
        parent.Xrm.Page.getAttribute("dots_redirecturl").setRequiredLevel("none");

    } else if (redirectModeValue == "1") {
        parent.Xrm.Page.getControl("dots_linkbuttontext").setVisible(false);
        parent.Xrm.Page.getAttribute("dots_linkbuttontext").setRequiredLevel("none");

        parent.Xrm.Page.getControl("dots_redirecturl").setVisible(true);
        parent.Xrm.Page.getAttribute("dots_redirecturl").setRequiredLevel("required");
    }

    else if (redirectModeValue == "2" || redirectModeValue == "3") {
        parent.Xrm.Page.getControl("dots_linkbuttontext").setVisible(true);
        parent.Xrm.Page.getAttribute("dots_linkbuttontext").setRequiredLevel("required");

        parent.Xrm.Page.getControl("dots_redirecturl").setVisible(true);
        parent.Xrm.Page.getAttribute("dots_redirecturl").setRequiredLevel("required");
    }

}