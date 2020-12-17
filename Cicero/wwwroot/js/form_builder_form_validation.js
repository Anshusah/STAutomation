
//remote validation scripts


function validate(type, typeVal, name) {
    // debugger;
    var name = name;
    if (name !== undefined && name !== "") {
        if (name.indexOf('elm') > -1) {
            var elmType = type;
            var value = typeVal;
            var op = $("[name='" + name + "']").attr("data-op");
            var opValue = $("[name='" + name + "']").attr("data-op-val");
            var opMsgShow = $("[name='" + name + "']").attr("data-op-msg");
            $.ajax({
                async: false,
                type: "POST",
                url: '/admin/validate.html',
                data: { type: elmType, val: value, checkOp: op, opVal: opValue, opMesg: opMsgShow },
                dataType: "json",
                success: function (response) {
                    var valid = response.split(",")[0];
                    var msgSelect = response.split(",")[1];
                    if ($("[name='" + name + "']").attr("data-table-elm") != "true" && $("[name='" + name + "']").attr("data-repeat-elm") != "true") {
                        if (name.indexOf("[") != -1) {
                            name = name.substring(0, name.indexOf("["));
                        }
                    }

                    if (valid == "false") {
                        showValidtionErrorMessage(name, msgSelect);
                    }
                    else {
                        removeValidationErrorMessage(name);
                    }
                },
                error: function (response) {
                }
            });
        }
    }


}


function validationInit() {
    //checkbox
    $("#" + formTagName).removeData("unobtrusiveValidation"); //remove unobstructive validation for dynamic form
    var validator = $("#" + formTagName).validate();
    validator.destroy();

    $("input[type=checkbox]").on('click', function () {
        var name = $(this).attr("name");
        value = $("input[name='" + name + "']:checked").length;
        //debugger;
        validate("checkboxgroup", value, name);
    });

    $("input[type=radio]").on("change", function () {
        var name = $(this).attr("name");
        value = $("[name='" + name + "']").val();
        validate("selectbox", value, name);
    });
    $("select").focusout(function () {
        //debugger;
        var name = $(this).attr("name");

        if ($(this).attr("multiple") == "multiple") {
            value = $("[name='" + name + "']").val().length;
            validate("multiselectbox", value, name);
        }
        else {
            value = $("[name='" + name + "']").val();
            validate("selectbox", value, name);
        }
    });

    $('input[type=text]').focusout(function () {
        var name = $(this).attr("name");
        var value;
        var type = $("[name='" + name + "']").attr("data-elm-val");
        if (type == undefined) {
            value = $("input[name='" + name + "']").val();
        }
        else {
            name = name.split("_")[1];
            name = "elm" + name;
            value = $("input[name='" + name + "']").val();
        }

        // debugger;
        validate("textbox", value, name);
    });

    $('input[data-elm-type=selectbox]').on('change', function () {
        var name = $(this).attr("name");
        var value;
        var type = $("[name='" + name + "']").attr("data-elm-val");
        if (type == undefined) {
            value = $("input[name='" + name + "']").val();
        }
        else {
            name = name.split("_")[1];
            name = "elm" + name;
            value = $("input[name='" + name + "']").val();
        }

        // debugger;
        validate("textbox", value, name);
    });

    $('input[type=number]').focusout(function () {
        var name = $(this).attr("name");
        value = $("input[name='" + name + "']").val();
        //debugger;
        validate("number", value, name);
    });
    $('input[type=range]').focusout(function () {
        var name = $(this).attr("name");
        value = $("input[name='" + name + "']").val();
        //debugger;
        validate("number", value, name);
    });

    $('textarea').focusout(function () {
        var name = $(this).attr("name");
        value = $("textarea[name='" + name + "']").val();
        //debugger;
        validate("textarea", value, name);
    });
}


$(function () {

    validationInit();
    currencyValidateInit();
});


function currencyValidateInit() {
    $('input[data-elm-type=currency]').focusout(function () {
        var name = $(this).attr("name");
        var value;
        var type = $("[name='" + name + "']").attr("data-elm-type");
        if (type == undefined) {
            value = $("input[name='" + name + "']").val();
        }
        else {
            focusName = name;
            name = name.split("_")[1];
            name = "elm" + name;
            value = $("input[name='" + name + "']").val();
        }

        // debugger;
        validate("number", value, name);
    });
}
function showValidtionErrorMessage(name, msg) {
    var b = '<span id="' + name + '-error" class="">' + msg + '</span>'
    //debugger;
    $("span[data-valmsg-for^='" + name + "']").html(b);
    $("span[data-valmsg-for^='" + name + "']").removeClass("field-validation-valid");
    $("span[data-valmsg-for^='" + name + "']").addClass("field-validation-error");
    $("span[data-valmsg-for^='" + name + "']").removeAttr("data-valmsg-replace");
    $("[name^='" + name + "']").attr("aria-invalid", "true");
    $("[name^='" + name + "']").addClass("input-validation-error");
    $("span[data-valmsg-for^='" + name + "']").addClass("field-validation-error");
}
function removeValidationErrorMessage(name) {
    $("span[data-valmsg-for^='" + name + "']").html("");
    $("span[data-valmsg-for^='" + name + "']").removeClass("field-validation-error");
    $("span[data-valmsg-for^='" + name + "']").addClass("field-validation-valid");
    $("span[data-valmsg-for^='" + name + "']").attr("data-valmsg-replace", true);
    $("[name^='" + name + "']").attr("aria-invalid", "false");
    $("[name^='" + name + "']").removeClass("input-validation-error");
}
function removePreviousValidations() {
    $(".input-validation-error").attr("aria-invalid", "false");
    $(".input-validation-error").removeClass("input-validation-error");
    $(".field-validation-error").addClass("field-validation-valid");
    $(".field-validation-error").html("");
    $(".field-validation-error").removeClass("field-validation-error");

}
//function checkValidation(form, formstr) {
//    $("#send-btn").attr("disabled", "disabled");
//    $("#save-btn").attr("disabled", "disabled");
//    $("#loading").attr("style","display:block;")
//    var medias = $("#" + form).find("ul[id^='Media']");
//    for (i = 0; i < medias.length; i++) {
//        var images = $(medias[i]).find("input[name^='images']");
//        for (j = 0; j < images.length; j++) {
//            $(images[j]).attr("name", $(medias[i]).attr("id"));
//        }
//    }
//    var fd = $("#" + form).serializeArray(); //formdata
//    var d = {}; //data
//    var hd = $("#" + form + " div[style*='display']"); //hidden form data
//    var d1 = [];//data1
//    $(fd).each(function () {
//        if (d[this.name] !== undefined) {
//            if (!Array.isArray(d[this.name])) {
//                d[this.name] = [d[this.name]];
//            }
//            d[this.name].push(this.value);
//        } else {
//            d[this.name] = this.value;
//        }
//    });

//    hd.find("[name^='elm']").each(function () {
//        d1.push($(this).attr("name"));
//    });

//    var formstring = formstr;
//    //console.log(d);
//    $.ajax({
//        async: false,
//        type: "POST",
//        url: '/admin/validateAll.html',
//        data: { formData: d, form: formstring, hiddenFormData: d1, isFrontEndValidation: false },
//        //contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (response) {
//            if (response.isFormValid) {
//                $.each(d1, function (k, v) {
//                    $("[name^='" + v + "']").attr("disabled", "disabled");  //set hidden fields to disable for not submitting
//                });

//                $("#" + form).submit();
//            }
//            else {
//                removePreviousValidations();
//                var elements = response.elementValidations;
//                for (i = 0; i < elements.length; i++) {
//                    if (!elements[i].isValid) {
//                        showValidtionErrorMessage(elements[i].elementId, elements[i].validationMessage);
//                    }
//                }
//                setFocusOnInvalidElement();
//                $("#send-btn").removeAttr("disabled");
//                $("#save-btn").removeAttr("disabled");
//                $("#loading").attr("style", "display:none;")
//            }
//        },
//        error: function (response) {
//            console.log(response);
//        }
//    });
//    setFocusOnInvalidElement();
//}

async function checkValidation(form, formstr, isAjax) {
    $("#send-btn").attr("disabled", "disabled");
    $("#save-btn").attr("disabled", "disabled");
    $("#loading").attr("style", "display:block;")
    var medias = $("#" + form).find("ul[id^='Media']");
    for (i = 0; i < medias.length; i++) {
        var images = $(medias[i]).find("input[name^='images']");
        for (j = 0; j < images.length; j++) {
            $(images[j]).attr("name", $(medias[i]).attr("id"));
        }
    }
    var fd = $("#" + form).serializeArray(); //formdata
    var d = {}; //data
    var hd = $("#" + form + " div[style*='display']"); //hidden form data
    var tabs = $('.tab-pane.active').nextAll();
    var d1 = [];//data1
    $(fd).each(function () {
        if (d[this.name] !== undefined) {
            if (!Array.isArray(d[this.name])) {
                d[this.name] = [d[this.name]];
            }
            d[this.name].push(this.value);
        } else {
            d[this.name] = this.value;
        }
    });

    hd.find("[name^='elm']").each(function () {
        d1.push($(this).attr("name"));
    });
    if (isAjax) {
        tabs.find("[name^='elm']").each(function () {
            d1.push($(this).attr("name"));
        });
    }
    var formstring = formstr;
    //console.log(d);
    var isFront = false;
    if ($("#isFrontEnd").val() === "frontend") {
        isFront = true;
    }
    return new Promise((resolve, reject) => {
        $.ajax({
            async: false,
            type: "POST",
            url: '/admin/validateAll.html',
            data: { formData: d, form: formstring, hiddenFormData: d1, isFrontValidation: isFront },
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.isFormValid) {
                    if (isAjax) {
                        resolve(true);
                        return;
                    }
                    $.each(d1, function (k, v) {
                        $("[name^='" + v + "']").attr("disabled", "disabled");  //set hidden fields to disable for not submitting
                    });


                    $("#" + form).submit();
                }
                else {
                    removePreviousValidations();
                    var elements = response.elementValidations;
                    for (i = 0; i < elements.length; i++) {
                        if (!elements[i].isValid) {
                            showValidtionErrorMessage(elements[i].elementId, elements[i].validationMessage);
                        }
                    }
                    setFocusOnInvalidElement();
                    if (isAjax) {
                        resolve(false);
                        return;
                    }

                    $("#send-btn").removeAttr("disabled");
                    $("#save-btn").removeAttr("disabled");
                    $("#loading").attr("style", "display:none;")
                }
            },
            error: function (response) {
                console.log(response);
                if (isAjax) {
                    resolve(false);
                    return;
                }
            }
        });
        setFocusOnInvalidElement();
    });
}

function onSendButtonClick(form) {
    $("#loading").attr("style", "display:block;")
    $("#save-btn").attr("disabled", "disabled");
    $("#send-btn").attr("disabled", "disabled");
    var hd = $("#" + form + " div[style*='display']"); //hidden form data
    var d1 = [];//data1

    var medias = $("#" + form).find("ul[id^='Media']");
    for (i = 0; i < medias.length; i++) {
        var images = $(medias[i]).find("input[name^='images']");
        for (j = 0; j < images.length; j++) {
            $(images[j]).attr("name", $(medias[i]).attr("id"));
        }
    }

    hd.find("[name^='elm']").each(function () {
        d1.push($(this).attr("name"));
    });

    $.each(d1, function (k, v) {
        $("[name^='" + v + "']").attr("disabled", "disabled");  //set hidden fields to disable for not submitting
    });

    $("#" + form).submit();
}

function setFocusOnInvalidElement() {
    var element = $('.input-validation-error').filter(":first");
    if (element.length == 0) {
        element = $('.field-validation-error').filter(":first");
    }
    var tabId = element.closest(".tab-pane").attr("id");
    $('a[href$="#' + tabId + '"]').trigger('click');
    setTimeout(function () {

        value1 = element.attr("data-value-for");
        if (value1 === undefined) {
            element.focus();
            if (element.hasClass('disable')) {
                element.blur();
            }
        }
        else {
            element.focus();
            $("[name='" + value1 + "']").focus();
            if (element.hasClass('disable')) {
                element.blur();
            }
        }

    }, 1000);
}



async function checkValidationTab(form, formstr, isAjax) {
    

    var fd = $("#" + form).serializeArray(); //formdata
    var d = {}; //data
    var hd = $("#" + form + " div[style*='display']"); //hidden form data
    var tabs = $('.tab-pane.active').nextAll();
    var d1 = [];//data1
    $(fd).each(function () {
        if (d[this.name] !== undefined) {
            if (!Array.isArray(d[this.name])) {
                d[this.name] = [d[this.name]];
            }
            d[this.name].push(this.value);
        } else {
            d[this.name] = this.value;
        }
    });

    hd.find("[name^='elm']").each(function () {
        d1.push($(this).attr("name"));
    });
    if (isAjax) {
        tabs.find("[name^='elm']").each(function () {
            d1.push($(this).attr("name"));
        });
    }
    var formstring = formstr;
    //console.log(d);
    var isFront = false;
    if ($("#isFrontEnd").val() === "frontend") {
        isFront = true;
    }
    return new Promise((resolve, reject) => {
        $.ajax({
            async: false,
            type: "POST",
            url: '/admin/validateAll.html',
            data: { formData: d, form: formstring, hiddenFormData: d1, isFrontValidation: isFront },
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.isFormValid) {
                   //do nothing 
                }
                else {
                    removePreviousValidations();
                    var elements = response.elementValidations;
                    for (i = 0; i < elements.length; i++) {
                        if (!elements[i].isValid) {
                            showValidtionErrorMessage(elements[i].elementId, elements[i].validationMessage);
                        }
                    }
                  //  setFocusOnInvalidElement();
                    if (isAjax) {
                        resolve(false);
                        return;
                    }
                }
            },
            error: function (response) {
                if (isAjax) {
                    resolve(false);
                    return;
                }
            }
        });
       // setFocusOnInvalidElement();
    });
}
