//create and save new form
var saveForm = function (tenant_identifier, urlEdit) {
    var name = $("#form-title").val();
    var tenantId = tenant_identifier;
    var icon = $("#form-icon").val();
     $.ajax({
        type: "POST",
        url: '/admin/' + tenant_identifier + '/builderform/createAction.html',
        data: { formName: name, tenant_identifier: tenantId, formIcon: icon },
        dataType: "json",
        success: function (response) {
            if (response != 0) {
                selectedForm = response;
                formKey = "form_" + selectedForm;
                createNewStorage();
                // setUrl(currStep + stepCount);
                localStorageSetFormId(response);
                localStorageSetIsNew(true);
                localStorageSetConfirmation(false);
                localStorageSetFormName(name);
                localStorageSetCurrStep(stepCount);
                // checkSteps(data, true);
                getSystemDateTime();
                checkCancelbtn();//check for cancelbtn
                window.location.replace(urlEdit);
            }
        },

        error: function (response) {

        }
    });
}
//gets system's datetime
var getSystemDateTime = function () {
    $.ajax({
        type: "POST",
        url: '/admin/' + tenant_identifier + '/getsystemdatetime.html',
        dataType: "json",
        success: function (response) {
            dashboardCurrData.startDateTime = response;
            updateLocalStorage();
        },
        error: function (e) { }
    });
}


//build url for navigation
var buildUrl = function (url) {
    url = url + dashboardCurrData.isNew;
    stepCount++;
    localStorageSetCurrStep(stepCount);
    window.open(url, '_self');

}


function createNewForm() {
    $("#createNewForm").modal('show');
}
var getTenantForms = function () {
    $.ajax({
        type: "POST",
        url: '/admin/' + tenant_identifier + '/gettenantforms.html',
        data: { isActiveOnly: true },
        dataType: "json",
        success: function (response) {
            JSON.stringify(response);
            $(".form-row").html("");
            if (response.length >= 1) {
               
                selectChange();
                for (i = 0; i < response.length; i++) {

                  //  var c = (response[i].name).split(',');
                  //  fromname = c[1];
                  //  console.log(response[i].url)

                    $(".form-row").append(" <div class='col-md-4 mb-3'> <a href='" + response[i].url + "' class='module-tab'>"
                        + "<div class='module-type module-type--icon d-flex justify-content-center align-items-center'> <i class='" + response[i].icon + "'></i>" + " </div><p class='m-0'>" + response[i].name + "</p></div></a>");
                } 
               
            } else {
               
                $(".form-row").append("<div class='col-md-12 text-center'>No Forms available. Please try again later.</div>");
            }
             
        },
        error: function (response) {
        }

    });
}

var showLoading = function () {
    $("#loading").fadeIn("10");
}
var hideLoading = function () {
    $("#loading").fadeOut("400");
}
//initialize list of forms
$(function () {
    showLoading();
    getTenantForms();
    hideLoading();
})

//get currently added data
var getAddedData = function () {
    var rolesAdded, usersAdded, workflowAdded;
    $.ajax({
        type: "POST",
        url: '/admin/' + tenant_identifier + '/getaddeddata.html',
        data: { startDateTime: dashboardCurrData.startDateTime },
        dataType: "json",
        success: function (response) {
            if (response.roleData != null) {
                rolesAdded = response.roleData.split(",");
                localStorageSetCurrRoles(rolesAdded);
            }
            if (response.userData != null) {
                usersAdded = response.userData.split(",");
                localStorageSetCurrUsers(usersAdded);
            }
            if (response.workflowData != null) {
                workflowAdded = response.workflowData.split(",");
                localStorageSetCurrWorkflow(workflowAdded);
            }
        },
        error: function () { }
    });
}
var selectChange = function () {

    $("#selectForm").on("change", function () {
        if ($(this).val() != "") {

            $("#selected-form-text").html($('#selectForm :selected').text());
            var data = getCounts();
            //checkSteps(data,false);
            if (dashboardCurrData.isNew == false) {
                localStorageSetConfirmation(true);
                if (stepCount == "" || stepCount == 1) {
                    checkSteps(data, false);
                }
            }
            else {
                checkSteps(data, true);
            }
            if (hasAllCount == true) {
                complete();
            }
            else {
                updateSteps();
            }

            //stepChange(currStep);
            selectedForm = $(this).val();
            localStorageSetFormId($(this).val());
            checkCancelbtn();//check for cancelbtn
            //setUrl(currStep + stepCount);
            //  $("#" + currStep + stepCount).on("click", buildForm);
        }
        else {
            startUp();
            stepRevert(currStep);
            removeUrl(currStep + stepCount);
        }
    });
}