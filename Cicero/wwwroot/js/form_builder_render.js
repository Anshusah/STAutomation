function initRepeatRowClone() {
    var all = $("[data-setAsRepeatItem='true']");
    var showInAccordain = $("[data-showInAccordion='true']").length;

    for (i = 0; i < $(all).length; i++) {
        $.each($(all[i]).find("input,select,textarea,label,i,a,div,span,ul"), function () {
            var itmElm = $(this);
            if (itmElm.attr("name") !== "elmloggedinuser") {
                if (itmElm.attr("name") !== undefined) {
                    var name = itmElm.attr("name");
                    if (name.indexOf("[]") != -1) {
                        name = name.split("[")[0];
                    }
                    var repeatName = name + "[{0}]";
                    itmElm.attr("data-repeat-name", repeatName);
                    itmElm.attr("name", name + "[0]");
                }
                if (itmElm.attr("id") !== undefined) {
                    var id = itmElm.attr("id");
                    if (id.indexOf("[]") != -1) {
                        id = id.split("[")[0];
                    }
                    var repeatId = id + "[{0}]";
                    itmElm.attr("data-repeat-id", repeatId);
                    itmElm.attr("id", id + "[0]");

                }
                if (itmElm.attr("for") !== undefined) {
                    var forItem = itmElm.attr("for");
                    var repeatFor = forItem + "[{0}]";
                    itmElm.attr("data-repeat-for", repeatFor);
                    itmElm.attr("for", forItem + "[0]");
                }
                if (itmElm.attr("data-for-check") !== undefined) {
                    var checkFor = itmElm.attr("data-for-check");
                    var checkRepeatFor = checkFor + "[{0}]";
                    itmElm.attr("data-repeat-for-check", checkRepeatFor);
                    itmElm.attr("data-for-check", checkFor + "[0]");
                }
                if (itmElm.attr("data-value-for") !== undefined) {
                    var valueFor = itmElm.attr("data-value-for");
                    var valueForRepeat = valueFor + "[{0}]";
                    itmElm.attr("data-repeat-value-for", valueForRepeat);
                    itmElm.attr("data-value-for", valueFor + "[0]");
                }
                if (itmElm.attr("data-display-for") !== undefined) {
                    var displayFor = itmElm.attr("data-display-for");
                    var displayForRepeat = displayFor + "[{0}]";
                    itmElm.attr("data-repeat-display-for", displayForRepeat);
                    itmElm.attr("data-display-for", displayFor + "[0]");
                }
                if (itmElm.attr("data-valmsg-for") != undefined) {
                    var valMsgFor = itmElm.attr("data-valmsg-for");
                    var valMsgFoRep = valMsgFor + "[{0}]";
                    itmElm.attr("data-repeat-valmsg-for", valMsgFoRep);
                    itmElm.attr("data-valmsg-for", valMsgFor + "[0]");
                }
                itmElm.attr("data-repeat-elm", "true");
            }
        });
        var rowId = $(all[i]).attr("id");
        $(all[i]).attr("id", rowId + "[0]");
        var repeatRowId = rowId + "[{0}]";
        $(all[i]).attr("data-repeat-id", repeatRowId);
        var title = $(all[i]).attr("data-repeatItemTitle");
        if (showInAccordain === 0) {
            var wrapper = "<div class=\"accordion card border mb-2\" id=\"accordionExample" + rowId + "-1\">"
                + "<a class=\"btn\" style=\"position: absolute; top: 0; right: 0; padding:4px; margin:4px;\" data-action=\"delete-row\" title=\"Delete\"><i class=\"ri-delete-bin-line\"></i><span class=\"sr-only\">Delete</span></a>"
                +"<div class=\"repeat-item-body\">"
                + "<div class=\"form-row \" id=\"" + rowId + "-1\" name=\"" + rowId + "-1\">"
                + all[i].innerHTML
                + "</div></div></div>";
        } else {
            var wrapper = "<div class=\"accordion card border mb-2\" id=\"accordionExample" + rowId + "-1\">"
                + "<div class=\"repeat-item\">"
                + "<div class=\"repeat-item-header\" id=\"heading" + rowId + "-1\">"
                + "<h6 class=\"repeat-item-title\"><span data-toggle=\"collapse\" data-target=\"#collapse" + rowId + "-1\" aria-expanded=\"false\" aria-controls=\"collapse\" class=\"p-4 collapsed\" style=\"display: block;\" data-title=\"" + title + "\"> 1: " + title + "</span ></h6 >"
                + "<a class=\"btn\" style=\"position: absolute; top: 0; right: 0; padding:4px; margin:4px;\" data-action=\"delete-row\" title=\"Delete\"><i class=\"ri-delete-bin-line\"></i><span class=\"sr-only\">Delete</span></a>"
                // +"<span style=\"position: absolute; top: 0; right: 0;\" class=\"p-3\">Delete</span>"
                + "</div>"
                + "<div id=\"collapse" + rowId + "-1\" class=\"p-4 collapse\" aria-labelledby=\"headingOne\" data-parent=\"#accordionExample" + rowId + "-1\">"
                + "<div class=\"repeat-item-body\">"
                + "<div class=\"form-row \" id=\"" + rowId + "-1\" name=\"" + rowId + "-1\">"
                + all[i].innerHTML
                + "</div></div></div></div></div>";
        }
      
        $(all[i]).parent("#repeatDiv_" + rowId).append("<div class=\"form-row\"><div class=\"offset-10 col-lg-2 p-2\"><button class=\"btn btn-outline-primary form-control\" data-action=\"clone-row\" type='button'>Add New</button></div>");
        $(all[i]).parent("#repeatDiv_" + rowId).append(wrapper);
        $(all[i]).remove();
        validationInit();
        reintDatetime();
        setCurrencyValueEvent();
        initBrowseImage();
    }

}
function initTagClickAdd() {
    $("[data-taglist]").unbind("click");
    $("[data-taglist]").on("click", function () {
        debugger;
        var value = $(this).text();
        var elm = $(this).closest(".form-group").find("[data-tagsinput]").first();
        $(elm).tagsinput("add", value);
    })
}
$(function () {
    initRepeatRowClone();

    //$("[data-tagsinput]").tagsinput(); // init tagsinput
    //initTagClickAdd();
    var popupElements = $("[data-popup-enable='true']");
    for (i = 0; i < popupElements.length; i++) {
        var e = $(popupElements[i]);
        var disabledPopupClose = e.attr('data-popup-close-disable');
        var allTargets = "";
        var modalTitle = "modal-";
        var targets = e.attr("data-popup-target");
        var title = e.attr("data-popup-title");
        var modalClass = e.attr("data-popup-class");
        if (targets != "" && targets != undefined) {
            targets = targets.split(",");
            $.each(targets, function (k, v) {
                allTargets = allTargets + ", #" + v;
            });
            $.each(targets, function (k, v) {
                modalTitle = modalTitle + v;
            })
            allTargets = allTargets.substr(2);
            if ($(allTargets).parent(".modal-body").length == 0) {
                $(allTargets).wrapAll("<div class=\"modal-body\" data-modal-bodyfor='" + modalTitle + "'></div>");
                $(allTargets).parent(".modal-body").wrapAll("<div class='modal fade popup' id='" + modalTitle + "' tabindex='-1' role='dialog'>"
                    + "<div class='modal-dialog modal-dialog-scrollable modal-dialog-centered " + modalClass + "'>"
                    + "<div class='modal-content'></div></div>");
                $(allTargets).closest("#" + modalTitle).wrapAll("<div id='popupInitial_" + modalTitle + "'></div>");
                if (title !== undefined && title != "") {
                    $(allTargets).parents(".modal-content").prepend("<div class='modal-header'>"
                        + "<h5 class='modal-title'>" + title + "</h5>"
                        + "<button type='button' class='close' data-dismiss='modal' aria-label='Close'>"
                        + "<span aria-hidden='true'>×</span>"
                        + "</button>"
                        + "</div>");
                }

            }

            if (e.prop("tagName") == "OPTION") { // check if option element
                var p = e.parent("select");
                p.on("change", function () {
                    var value = $(this).val();
                    var option = $(this).find("option[value='" + value + "']");
                    if (option.attr("data-popup-enable") != undefined && option.attr("data-popup-enable") == 'true') {
                        var t = option.attr("data-popup-target");
                        if (t != "") {
                            t = t.split(",");
                            var mTitle = "#modal-";
                            $.each(t, function (k, v) {
                                mTitle = mTitle + v;
                            })
                            $(mTitle).detach().appendTo("body");
                            if (disabledPopupClose === 'true') {
                                $(mTitle).find('.close').remove();
                                $(mTitle).modal({
                                    backdrop: 'static',
                                    keyboard: false
                                });
                            }
                            else {
                                $(mTitle).modal('show');
                            }
                        }

                    }
                });
            }
            else {
                e.on("click", function () {
                    if ($(this).attr("type") == "checkbox") {//check if checkbox
                        if ($(this).prop("checked") == true) {// show on checked only
                            var t = $(this).attr("data-popup-target");
                            if (t != "") {
                                t = t.split(",");
                                var mTitle = "#modal-";
                                $.each(t, function (k, v) {
                                    mTitle = mTitle + v;
                                })
                                $(mTitle).detach().appendTo("body");
                                if (disabledPopupClose === 'true') {
                                    $(mTitle).find('.close').remove();
                                    $(mTitle).modal({
                                        backdrop: 'static',
                                        keyboard: false
                                    });
                                }
                                else {
                                    $(mTitle).modal('show');
                                }
                            }
                        }
                    }
                    else {
                        var t = $(this).attr("data-popup-target");
                        if (t != "") {
                            t = t.split(",");
                            var mTitle = "#modal-";
                            $.each(t, function (k, v) {
                                mTitle = mTitle + v;
                            })
                            $(mTitle).detach().appendTo("body");
                            if (disabledPopupClose === 'true') {
                                $(mTitle).find('.close').remove();
                                $(mTitle).modal({
                                    backdrop: 'static',
                                    keyboard: false
                                });
                            }
                            else {
                                $(mTitle).modal('show');
                            }
                        }
                    }


                });

            }
        }

    }

    $(".popup").on("hide.bs.modal", function () {
        var id = $(this).attr("id");
        setTimeout(function () {
            $("#" + id).detach().appendTo("#popupInitial_" + id);
        }, 1000, id);
    });

    //showhide collection for selectbox
    let uu = $('[select-target]');
    $.each(uu, function (k, l) {
        showHideBySelectBox(l);
    });

    let btnHideShow = $('[data-action-click=onClick]');
    $.each(btnHideShow, function (k, l) {
        showHideByButtonInit(l);
    });



    jQuery(document).on("click", "[data-action-click=onClick]", function () {
        showHideByButton(this);
    });


    let checkboxHideShow = $('[data-checkbox=true]');
    $.each(checkboxHideShow, function (k, l) {
        if ($(l).prop("checked") == true) {
            showHideByCheckbox(l);
        }

    });
    //appendElements
    var appendElements = $("[data-appendElm]");
    for (i = 0; i < appendElements.length; i++) {
        var e = $(appendElements[i]);
        var elements = e.attr("data-appendElm").split(",");
        for (j = 0; j < elements.length; j++) {
            var append = $("[name='elm" + elements[j] + "']");
            if (append.length === 0) {
                append = $("#" + elements[j]);
            }
            append.detach().appendTo(e);
        }
    }


  

    //showhide function for selectbox on change
    jQuery(document).on("change", "[select-target]", function () {
        showHideBySelectBox(this);

    });


    jQuery(document).on("change", "[data-checkbox=true]", function () {
        showHideByCheckboxBySelection(this);

    });

    
    //showhide collection for radio
    let rd = $('.radio-target');
    $.each(rd, function (k, l) {
        showHideByRadio(l);
    });

    //showhide function for selectbox on change
    jQuery(document).on("change", ".radio-target", function () {
        showHideByRadio(this);

    });

    
    let ii = $('.Image-target');
    $.each(ii, function (k, l) {

        showHideByImage(l);
    });

    //showhide function for selectbox on change
    jQuery(document).on("click", ".Image-target", function () {
        $(this).parent().parent().find('.Image-target').removeClass('show');
        $(this).addClass('show');
        showHideByImage(this);

    });

   
    //Radio Slider
    //showhide collection for radio
    let rds = $('.radio-slider-target');
    $.each(rds, function (k, l) {
        showHideByRadioSlider(l);
    });

    //showhide function for selectbox on change
    jQuery(document).on("change", ".radio-slider-target", function () {
        showHideByRadioSlider(this);

    });

    

});


function showHideByButtonInit(e) {
    var dataTargets = $(e).attr('data-target').split(',');
    var dataActions = $(e).attr('data-target-action').split(',');
    $.each(dataTargets, function (i, data) {
        if (dataActions[i].toLowerCase() === "true") {
            $(data).hide();
        }
        else if (dataActions[i].toLowerCase() === "false") {
            $(data).show();
        }
        else if (dataActions[i].toLowerCase() === "enable") {
            $(data).attr('disabled', 'disabled');
        }
        else {
            $(data).removeAttr('disabled');
        }
    });
}


function showHideByButton(e) {
    var dataTargets = $(e).attr('data-target').split(',');
    var dataActions = $(e).attr('data-target-action').split(',');

    $.each(dataTargets, function (i, data) {
        var elm;
        if ($(data).length > 0) {
            elm = data;
        }
        else {
            elm = "#elm" + data.split("#")[1];
        }
        if (dataActions[i].toLowerCase() === "false") {
            $(elm).hide()
        }
        else if (dataActions[i].toLowerCase() === "enable") {
            $(elm).removeAttr('disabled');
        }
        else if (dataActions[i].toLowerCase() === "disable") {
            $(elm).attr('disabled', 'disabled');
        }
        else {
            $(elm).show();
        }
    });
};

function showHideByCheckbox(e) {
    var dataTarget = $(e).attr('data-target');
    var dataTargets = [];
    if (typeof (dataTarget) !== "undefined") {
        dataTargets = dataTarget.split(',');
    }

    var dataAction = $(e).attr('data-target-action');
    var dataActions = [];
    if (typeof (dataAction) !== "undefined") {
        dataActions = dataAction.split(',');
    }


    $.each(dataTargets, function (i, data) {
        var elm;
        if ($(data).length > 0) {
            elm = data;
        }
        else {
            elm = "#elm" + data.split("#")[1];
        }
        if (dataActions[i].toLowerCase() === "false") {

            $(elm).hide();
            dataActions[i] = "true";
        }
        else if (dataActions[i].toLowerCase() === "enable") {
            $(elm).removeAttr('disabled');
            dataActions[i] = "disable";

        }
        else if (dataActions[i].toLowerCase() === "disable") {
            $(elm).attr('disabled', 'disabled');
            dataActions[i] = "enable";
        }
        else {
            $(elm).show();
            dataActions[i] = "false";

        }
        $(e).attr('data-target-action', dataActions[i]);
    });
}

function showHideByCheckboxBySelection(e) {
    var dataTarget = $(e).attr('data-target');
    var dataTargets = [];
    if (typeof (dataTarget) !== "undefined") {
        dataTargets = dataTarget.split(',');
    }

    var dataAction = $(e).attr('data-target-action');
    var dataActions = [];
    if (typeof (dataAction) !== "undefined") {
        dataActions = dataAction.split(',');
    }

    dataActions = FormBuilder.Element.Event().toggleElementForCheckbox(dataTargets, dataActions);
    $(e).attr('data-target-action', dataActions);
};

//showhide function for selectbox
function showHideBySelectBox(e) {

    if ($(e).children("option:selected").attr("data-target") != null && $(e).children("option:selected").attr("data-target-action") != "") {
        let dataTargets = $(e).children("option:selected").attr("data-target").split(",");
        let dataActions = $(e).children("option:selected").attr("data-target-action").split(",");

        $.each(dataTargets, function (i, data) {
            var elm;
            if ($(data).length > 0) {
                elm = data;
            }
            else {
                elm = "#elm" + data.split("#")[1];
            }
            if (dataActions[i].toLowerCase() === "false") {
                $(elm).hide()
            }
            else if (dataActions[i].toLowerCase() === "enable") {
                $(elm).removeAttr('disabled');
            }
            else if (dataActions[i].toLowerCase() === "disable") {
                $(elm).attr('disabled', 'disabled');
            }
            else {
                $(elm).show();
            }
        });
    }

}

//showhide function for selectbox
function showHideByRadio(e) {
    if ($(e).data("target") != null && $(e).data("target") != "" && $(e)[0].checked == true) {
        let dataTargets = $(e).attr("data-target").split(",");
        let dataActions = $(e).attr("data-target-action").split(",");

        $.each(dataTargets, function (i, data) {
            var elm;
            if ($(data).length > 0) {
                elm = data;
            }
            else {
                elm = "#elm" + data.split("#")[1];
            }
            if (dataActions[i].toLowerCase() === "false") {
                $(elm).closest('.form-group').hide();
            }
            else if (dataActions[i].toLowerCase() === "enable") {
                $(elm).removeAttr('disabled');
            }
            else if (dataActions[i].toLowerCase() === "disable") {
                $(elm).attr('disabled', 'disabled');
            }
            else {
                $(elm).closest('.form-group').show();
            }
        });
    }

}
    //show hide for Image

//showhide function for selectbox
function showHideByImage(e) {

    if (e.classList.contains('show') && $(e).data("target") != null && $(e).data("target") != "") {
        let dataTargets = $(e).attr("data-target").split(",");
        let dataActions = $(e).attr("data-target-action").split(",");
        var arrayHide = [];
        $.each(dataTargets, function (i, data) {
            var elm;
            if ($(data).length > 0) {
                elm = data;
            }
            else {
                elm = "#elm" + data.split("#")[1];
            }
            if (dataActions[i].toLowerCase() === "false") {
                $(elm).hide();
                $('#accordionExample' + dataTargets[i].replace('#', '')).hide();
            }
            else if (dataActions[i].toLowerCase() === "enable") {
                $(elm).removeAttr('disabled');
                $('#accordionExample' + dataTargets[i].replace('#', '')).removeAttr("disabled");
            }
            else if (dataActions[i].toLowerCase() === "disable") {
                $(elm).attr('disabled', 'disabled');
                $('#accordionExample' + dataTargets[i].replace('#', '')).attr("disabled", "disabled");
            }
            else {
                $(elm).show();
                $('#accordionExample' + dataTargets[i].replace('#', '')).show();
                var oo = $(dataTargets[i]).find('[number-target]');

                $.each(oo, function (k, l) {

                    let count = parseInt($(e).val());
                    let jsonData1 = $(l).attr("data-target").split(",");
                    if (isNaN(count)) {

                        $.each(jsonData1, function (x, y) {
                            arrayHide.push(y);
                        });
                    };

                });

            }
        });
        $.each(arrayHide, function (x, y) {
            $(y).hide()
        });

    }

}
    //end of Show hide for Image

//showhide function for radio
function showHideByRadioSlider(e) {
    if ($(e).data("targety") != null && $(e).data("targety") != "" && $(e)[0].checked == true) {
        let dataTargets = $(e).attr("data-targety").split(",");
        let dataActions = $(e).attr("data-actiony").split(",");

        $.each(dataTargets, function (i, data) {
            var elm;
            if ($(data).length > 0) {
                elm = data;
            }
            else {
                elm = "#elm" + data.split("#")[1];
            }
            if (dataActions[i].toLowerCase() === "false") {
                $(elm).hide()
            }
            else if (dataActions[i].toLowerCase() === "enable") {
                $(elm).removeAttr('disabled');
            }
            else if (dataActions[i].toLowerCase() === "disable") {
                $(elm).attr('disabled', 'disabled');
            }
            else {
                $(elm).show();
            }
        });
    }
    if ($(e).data("targetn") != null && $(e).data("targetn") != "" && $(e)[0].checked == false) {
        let dataTargets = $(e).attr("data-targetn").split(",");
        let dataActions = $(e).attr("data-actionn").split(",");

        $.each(dataTargets, function (i, data) {
            var elm;
            if ($(data).length > 0) {
                elm = data;
            }
            else {
                elm = "#elm" + data.split("#")[1];
            }
            if (dataActions[i].toLowerCase() === "false") {
                $(elm).hide()
            }
            else if (dataActions[i].toLowerCase() === "enable") {
                $(elm).removeAttr('disabled');
            }
            else if (dataActions[i].toLowerCase() === "disable") {
                $(elm).attr('disabled', 'disabled');
            }
            else {
                $(elm).show();
            }
        });
    }
}

function addTableRow(count, element) {
    for (i = 0; i < count; i++) {
        var itm = element.closest("tr").clone();
        $.each(itm.find("input,select,textarea"), function () {
            var itmElm = $(this);
            if (itmElm.is("input")) {
                if (itmElm.is("input:checkbox")) {
                    itmElm.prop("checked", false);
                }
                else if (itmElm.is("input:radio")) {
                    itmElm.prop("checked", false);
                }
                else {
                    itmElm.val("");
                }
            }
            else if (itmElm.is("select")) {
                itmElm.find("option:selected").prop("selected", false);
                itmElm.find("option:first").prop("selected", "selected");
            }
            else {
                itmElm.val("");
            }

        });
        element.closest("tbody").append(itm);
        var totalrows = element.closest("tbody").find("tr");
        $.each(totalrows, function () {
            var index = $(this).index();
            $.each($(this).find("input,select,textarea,label,i,a,div,span"), function () {
                var namevar = $(this).data("repeat-name");
                var idvar = $(this).data("repeat-id");
                var forvar = $(this).data("repeat-for");
                var forCheckvar = $(this).data("repeat-for-check");
                var valueFor = $(this).data("repeat-value-for");
                var displayFor = $(this).data("repeat-display-for");
                var valMsgFor = $(this).data("valmsg-for");
                if (namevar != undefined) {
                    try {
                        $(this).attr("name", namevar.format(index));
                    }
                    catch (e) {
                        $(this).attr("name", namevar.replace("{0}", index));
                    }
                }
                if (idvar != undefined) {
                    try {
                        $(this).attr("id", idvar.format(index));
                    }
                    catch (e) {
                        $(this).attr("id", idvar.replace("{0}", index));
                    }

                }
                if (forvar != undefined) {
                    try {
                        $(this).attr("for", forvar.format(index));
                    }
                    catch (e) {
                        $(this).attr("for", forvar.replace("{0}", index));
                    }

                }
                if (forCheckvar != undefined) {
                    try {
                        $(this).attr("data-for-check", forCheckvar.format(index));
                    }
                    catch (e) {
                        $(this).attr("data-for-check", forCheckvar.replace("{0}", index));
                    }
                }
                if (valueFor != undefined) {
                    try {
                        $(this).attr("data-value-for", valueFor.format(index));
                    }
                    catch (e) {
                        $(this).attr("data-value-for", valueFor.replace("{0}", index));
                    }
                }
                if (displayFor != undefined) {
                    try {
                        $(this).attr("data-display-for", displayFor.format(index));
                    }
                    catch (e) {
                        $(this).attr("data-display-for", displayFor.replace("{0}", index));
                    }
                }
                if (valMsgFor != undefined) {
                    try {
                        var currIndex = valMsgFor.split("[")[1].split("]")[0];
                        $(this).attr("data-valmsg-for", valMsgFor.replace("[" + currIndex + "]", "[" + index + "]"));
                    }
                    catch (e) {

                    }
                }
            })
        });
    }
    validationInit();
    reintDatetime();
    setCurrencyValueEvent();
}

function addRepeatRow(count, element) {
    for (i = 0; i < count; i++) {
        var parent = element.closest("[id^='repeatDiv']");
        var newRow = element.closest("[id^='repeatDiv']").find(".accordion").first().clone();
        $.each(newRow.find("input,select,textarea,ul"), function () {
            var itmElm = $(this);
            if (itmElm.is("input")) {
                if (itmElm.is("input:checkbox")) {
                    itmElm.prop("checked", false);
                }
                else if (itmElm.is("input:radio")) {
                    itmElm.prop("checked", false);
                }
                else {
                    itmElm.val("");
                }
            }
            else if (itmElm.is("ul") && itmElm.hasClass("media-list")) {
                itmElm.find("ul").first().html("");
            }
            else if (itmElm.is("select")) {
                itmElm.find("option:selected").prop("selected", false);
                itmElm.find("option:first").prop("selected", "selected");
            }
            else {
                itmElm.val("");
            }

        });


        newRow.appendTo(parent);

    }
    allRow = parent.find(".accordion");
    for (i = 0; i < allRow.length; i++) {
        var s = $(allRow[i]).find("span[data-target]");
        var sval = s.attr("data-target").split("-")[0];
        s.attr("data-target", sval + "-" + i);
        var title = s.attr("data-title");
        s.html((i + 1) + ": " + title);
        var t = $(allRow[i]).find("div[id^='" + sval.split("#")[1] + "']");
        t.attr("id", sval.split("#")[1] + "-" + i);
        $.each($(allRow[i]).find("input,select,textarea,label,i,a,div,span,ul"), function () {
            var namevar = $(this).data("repeat-name");
            var idvar = $(this).data("repeat-id");
            var forvar = $(this).data("repeat-for");
            var forCheckvar = $(this).data("repeat-for-check");
            var valueFor = $(this).data("repeat-value-for");
            var displayFor = $(this).data("repeat-display-for");
            var valMsgFor = $(this).data("repeat-valmsg-for");
            if (namevar != undefined) {
                try {
                    $(this).attr("name", namevar.format(i));
                }
                catch (e) {
                    $(this).attr("name", namevar.replace("{0}", i));
                }
            }
            if (idvar != undefined) {
                try {
                    $(this).attr("id", idvar.format(i));
                }
                catch (e) {
                    $(this).attr("id", idvar.replace("{0}", i));
                }

            }
            if (forvar != undefined) {
                try {
                    $(this).attr("for", forvar.format(i));
                }
                catch (e) {
                    $(this).attr("for", forvar.replace("{0}", i));
                }

            }
            if (forCheckvar != undefined) {
                try {
                    $(this).attr("data-for-check", forCheckvar.format(i));
                }
                catch (e) {
                    $(this).attr("data-for-check", forCheckvar.replace("{0}", i));
                }
            }
            if (valueFor != undefined) {
                try {
                    $(this).attr("data-value-for", valueFor.format(i));
                }
                catch (e) {
                    $(this).attr("data-value-for", valueFor.replace("{0}", i));
                }
            }
            if (displayFor != undefined) {
                try {
                    $(this).attr("data-display-for", displayFor.format(i));
                }
                catch (e) {
                    $(this).attr("data-display-for", displayFor.replace("{0}", i));
                }
            }
            if (valMsgFor != undefined) {
                try {
                    $(this).attr("data-valmsg-for", valMsgFor.format(i));
                }
                catch (e) {
                    $(this).attr("data-valmsg-for", valMsgFor.replace("{0}", i));
                }
            }

        });
    }
    validationInit();
    reintDatetime();
    setCurrencyValueEvent();
    initBrowseImage();
}

$(document).on("click", ".row-option .field-controller [data-action='clone-rule']", function () {

    var itm = $(this).closest('tr').clone();
    //clear element value;
    $.each(itm.find("input,select,textarea"), function () {
        var itmElm = $(this);
        if (itmElm.is("input")) {
            if (itmElm.is("input:checkbox")) {
                itmElm.prop("checked", false);
            }
            else if (itmElm.is("input:radio")) {
                itmElm.prop("checked", false);
            }
            else {
                itmElm.val("");
            }
        }
        else if (itmElm.is("select")) {
            itmElm.find("option:selected").prop("selected", false);
            itmElm.find("option:first").prop("selected", "selected");
        }
        else {
            itmElm.val("");
        }

    });
    $(this).closest("tbody").append(itm);
    var totalrows = $(this).closest("tbody").find("tr");
    $.each(totalrows, function () {
        var index = $(this).index();
        $.each($(this).find("input,select,textarea,label,i,a,div,span"), function () {
            var namevar = $(this).data("repeat-name");
            var idvar = $(this).data("repeat-id");
            var forvar = $(this).data("repeat-for");
            var forCheckvar = $(this).data("repeat-for-check");
            var valueFor = $(this).data("repeat-value-for");
            var displayFor = $(this).data("repeat-display-for");
            var valMsgFor = $(this).data("valmsg-for");
            if (namevar != undefined) {
                try {
                    $(this).attr("name", namevar.format(index));
                }
                catch (e) {
                    $(this).attr("name", namevar.replace("{0}", index));
                }
            }
            if (idvar != undefined) {
                try {
                    $(this).attr("id", idvar.format(index));
                }
                catch (e) {
                    $(this).attr("id", idvar.replace("{0}", index));
                }

            }
            if (forvar != undefined) {
                try {
                    $(this).attr("for", forvar.format(index));
                }
                catch (e) {
                    $(this).attr("for", forvar.replace("{0}", index));
                }

            }
            if (forCheckvar != undefined) {
                try {
                    $(this).attr("data-for-check", forCheckvar.format(index));
                }
                catch (e) {
                    $(this).attr("data-for-check", forCheckvar.replace("{0}", index));
                }
            }
            if (valueFor != undefined) {
                try {
                    $(this).attr("data-value-for", valueFor.format(index));
                }
                catch (e) {
                    $(this).attr("data-value-for", valueFor.replace("{0}", index));
                }
            }
            if (displayFor != undefined) {
                try {
                    $(this).attr("data-display-for", displayFor.format(index));
                }
                catch (e) {
                    $(this).attr("data-display-for", displayFor.replace("{0}", index));
                }
            }
            if (valMsgFor != undefined) {
                try {
                    var currIndex = valMsgFor.split("[")[1].split("]")[0];
                    $(this).attr("data-valmsg-for", valMsgFor.replace("[" + currIndex + "]", "[" + index + "]"));
                }
                catch (e) {

                }
            }
        })
    });
    validationInit();
    reintDatetime();
    setCurrencyValueEvent();
});

$(document).on("click", ".row-option .field-controller [data-action='remove-rule']", function () {

    if ($(this).closest("tbody").find("tr").length > 1) {
        $(this).closest('tr').remove();
        var totalrows = $(this).closest("tbody").find("tr");
        $.each(totalrows, function () {
            var index = $(this).index();
            $.each($(this).find("input,select,textarea,label,i,a"), function () {
                var namevar = $(this).data("repeat-name");
                var idvar = $(this).data("repeat-id");
                var forvar = $(this).data("repeat-for");
                var forCheckvar = $(this).data("repeat-for-check");
                var valueFor = $(this).data("repeat-value-for");
                var displayFor = $(this).data("repeat-display-for");
                var valMsgFor = $(this).data("valmsg-for");
                if (namevar != undefined) {
                    try {
                        $(this).attr("name", namevar.format(index));
                    }
                    catch (e) {
                        $(this).attr("name", namevar.replace("{0}", index));
                    }
                }
                if (idvar != undefined) {
                    try {
                        $(this).attr("id", idvar.format(index));
                    }
                    catch (e) {
                        $(this).attr("id", idvar.replace("{0}", index));
                    }

                }
                if (forvar != undefined) {
                    try {
                        $(this).attr("for", forvar.format(index));
                    }
                    catch (e) {
                        $(this).attr("for", forvar.replace("{0}", index));
                    }

                }
                if (forCheckvar != undefined) {
                    try {
                        $(this).attr("data-for-check", forCheckvar.format(index));
                    }
                    catch (e) {
                        $(this).attr("data-for-check", forCheckvar.replace("{0}", index));
                    }
                }
                if (valueFor != undefined) {
                    try {
                        $(this).attr("data-value-for", valueFor.format(index));
                    }
                    catch (e) {
                        $(this).attr("data-value-for", valueFor.replace("{0}", index));
                    }
                }
                if (displayFor != undefined) {
                    try {
                        $(this).attr("data-display-for", displayFor.format(index));
                    }
                    catch (e) {
                        $(this).attr("data-display-for", displayFor.replace("{0}", index));
                    }
                }
                if (valMsgFor != undefined) {
                    try {
                        var currIndex = valMsgFor.split("[")[1].split("]")[0];
                        $(this).attr("data-valmsg-for", valMsgFor.replace("[" + currIndex + "]", "[" + index + "]"));
                    }
                    catch (e) {

                    }
                }
            })
        });
    }
    else {
        toastr.warning('You can\'t delete last option');
    }
});

$(document).on("click", ".row-option .field-controller [data-action='edit-rule']", function () {
    var tr = $(this).closest("tr");
    tr.find(".hide-icon").removeClass("hide-icon");
    $(this).addClass("hide-icon");

    tr.find(".display-data").attr("data-label-hide", "true");
    tr.find("[data-elm-hide='true']").attr("data-elm-hide", "false");
});
$(document).on("click", ".row-option .field-controller [data-action='ok-rule']", function () {
    var tr = $(this).closest("tr");
    tr.find(".fc-icon").addClass("hide-icon");
    tr.find("[data-action='edit-rule']").removeClass("hide-icon");
    var fd = tr.find("input,select,textarea").serializeArray(); //tr data
    var d = {}; //data
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
    setNewValueOnTableRow(d);
    tr.find(".display-data").removeAttr("data-label-hide");
    tr.find("[data-elm-hide='false']").attr("data-elm-hide", "true");
});

$(document).on("click", ".row-option .field-controller [data-action='cancel-rule']", function () {
    var tr = $(this).closest("tr");
    tr.find(".fc-icon").addClass("hide-icon");
    tr.find("[data-action='edit-rule']").removeClass("hide-icon");
    setPrevValueOnTableRow(tr);
    tr.find(".display-data").removeAttr("data-label-hide");
    tr.find("[data-elm-hide='false']").attr("data-elm-hide", "true");

});
$(document).on("change", ".custom-range", function () {
    var value = $(this).val();
    if ($(this).attr("data-value-for") !== undefined) {
        var setValueToId = $(this).attr("data-value-for");
        $("#" + setValueToId).val(value);
        validate("number", value, $(this).attr("name"));
    }
});
$(document).on("input", ".custom-range", function () {
    var value = $(this).val();
    if ($(this).attr("data-value-for") !== undefined) {
        var setValueToId = $(this).attr("data-value-for");
        $("#" + setValueToId).val(value);
        validate("number", value, $(this).attr("name"));
    }
});

$(document).on("click", "[data-action='clone-row']", function () {
    var parent = $(this).closest("[id^='repeatDiv']");
    var newRow = $(this).closest("[id^='repeatDiv']").find(".accordion").first().clone();
    newRow.appendTo(parent);
    $.each(newRow.find("input,select,textarea,ul"), function () {
        var itmElm = $(this);
        if (itmElm.is("input")) {
            if (itmElm.is("input:checkbox")) {
                itmElm.prop("checked", false);
            }
            else if (itmElm.is("input:radio")) {
                itmElm.prop("checked", false);
            }
            else {
                itmElm.val("");
            }
        }
        else if (itmElm.is("ul") && itmElm.hasClass("media-list")) {
            itmElm.find("ul").first().html("");
        }
        else if (itmElm.is("select")) {
            itmElm.find("option:selected").prop("selected", false);
            itmElm.find("option:first").prop("selected", "selected");
        }
        else {
            itmElm.val("");
        }

    });
    allRow = parent.find(".accordion");
    for (i = 0; i < allRow.length; i++) {
        var s = $(allRow[i]).find("span[data-target]");
        var sval = s.attr("data-target").split("-")[0];
        s.attr("data-target", sval + "-" + i);
        var title = s.attr("data-title");
        s.html((i + 1) + ": " + title);
        var t = $(allRow[i]).find("div[id^='" + sval.split("#")[1] + "']");
        t.attr("id", sval.split("#")[1] + "-" + i);
        $.each($(allRow[i]).find("input,select,textarea,label,i,a,div,span,ul"), function () {
            var namevar = $(this).data("repeat-name");
            var idvar = $(this).data("repeat-id");
            var forvar = $(this).data("repeat-for");
            var forCheckvar = $(this).data("repeat-for-check");
            var valueFor = $(this).data("repeat-value-for");
            var displayFor = $(this).data("repeat-display-for");
            var valMsgFor = $(this).data("repeat-valmsg-for");
            if (namevar != undefined) {
                try {
                    $(this).attr("name", namevar.format(i));
                }
                catch (e) {
                    $(this).attr("name", namevar.replace("{0}", i));
                }
            }
            if (idvar != undefined) {
                try {
                    $(this).attr("id", idvar.format(i));
                }
                catch (e) {
                    $(this).attr("id", idvar.replace("{0}", i));
                }

            }
            if (forvar != undefined) {
                try {
                    $(this).attr("for", forvar.format(i));
                }
                catch (e) {
                    $(this).attr("for", forvar.replace("{0}", i));
                }

            }
            if (forCheckvar != undefined) {
                try {
                    $(this).attr("data-for-check", forCheckvar.format(i));
                }
                catch (e) {
                    $(this).attr("data-for-check", forCheckvar.replace("{0}", i));
                }
            }
            if (valueFor != undefined) {
                try {
                    $(this).attr("data-value-for", valueFor.format(i));
                }
                catch (e) {
                    $(this).attr("data-value-for", valueFor.replace("{0}", i));
                }
            }
            if (displayFor != undefined) {
                try {
                    $(this).attr("data-display-for", displayFor.format(i));
                }
                catch (e) {
                    $(this).attr("data-display-for", displayFor.replace("{0}", i));
                }
            }
            if (valMsgFor != undefined) {
                try {
                    $(this).attr("data-valmsg-for", valMsgFor.format(i));
                }
                catch (e) {
                    $(this).attr("data-valmsg-for", valMsgFor.replace("{0}", i));
                }
            }

        });
    }
    validationInit();
    reintDatetime();
    setCurrencyValueEvent();
    initBrowseImage();
});
$(document).on("click", "[data-action='delete-row']", function () {
    var parent = $(this).closest("[id^='repeatDiv']");
    allRow = parent.find(".accordion");
    if (allRow.length > 1) {
        $(this).closest(".accordion").remove();
        allRow = parent.find(".accordion");
        for (i = 0; i < allRow.length; i++) {
            var s = $(allRow[i]).find("span[data-target]");
            var sval = s.attr("data-target").split("-")[0];
            s.attr("data-target", sval + "-" + i);
            var title = s.attr("data-title");
            s.html((i + 1) + ": " + title);
            var t = $(allRow[i]).find("div[id^='" + sval.split("#")[1] + "']");
            t.attr("id", sval.split("#")[1] + "-" + i);
            $.each($(allRow[i]).find("input,select,textarea,label,i,a,div,span"), function () {
                var namevar = $(this).data("repeat-name");
                var idvar = $(this).data("repeat-id");
                var forvar = $(this).data("repeat-for");
                var forCheckvar = $(this).data("repeat-for-check");
                var valueFor = $(this).data("repeat-value-for");
                var displayFor = $(this).data("repeat-display-for");
                var valMsgFor = $(this).data("repeat-valmsg-for");
                if (namevar != undefined) {
                    try {
                        $(this).attr("name", namevar.format(i));
                    }
                    catch (e) {
                        $(this).attr("name", namevar.replace("{0}", i));
                    }
                }
                if (idvar != undefined) {
                    try {
                        $(this).attr("id", idvar.format(i));
                    }
                    catch (e) {
                        $(this).attr("id", idvar.replace("{0}", i));
                    }

                }
                if (forvar != undefined) {
                    try {
                        $(this).attr("for", forvar.format(i));
                    }
                    catch (e) {
                        $(this).attr("for", forvar.replace("{0}", i));
                    }

                }
                if (forCheckvar != undefined) {
                    try {
                        $(this).attr("data-for-check", forCheckvar.format(i));
                    }
                    catch (e) {
                        $(this).attr("data-for-check", forCheckvar.replace("{0}", i));
                    }
                }
                if (valueFor != undefined) {
                    try {
                        $(this).attr("data-value-for", valueFor.format(i));
                    }
                    catch (e) {
                        $(this).attr("data-value-for", valueFor.replace("{0}", i));
                    }
                }
                if (displayFor != undefined) {
                    try {
                        $(this).attr("data-display-for", displayFor.format(i));
                    }
                    catch (e) {
                        $(this).attr("data-display-for", displayFor.replace("{0}", i));
                    }
                }
                if (valMsgFor != undefined) {
                    try {
                        $(this).attr("data-valmsg-for", valMsgFor.format(i));
                    }
                    catch (e) {
                        $(this).attr("data-valmsg-for", valMsgFor.replace("{0}", i));
                    }
                }

            });
        }
        validationInit();
        reintDatetime();
        setCurrencyValueEvent();
        initBrowseImage();
    } else {
        toastr.warning('You can\'t delete last option');
    }
});
function setPrevValueOnTableRow(row) {
    $.each(row.find("input,select,textarea"), function () {

        var value = $(this).attr("data-act");
        var elmName = $(this).attr("name");
        var elm = "[name='" + elmName + "']";
        if (value != undefined) {
            if (value.indexOf(",") > -1) {

                if ($(elm).attr('multiple') == 'multiple') {
                    $.each(value.split(","), function (i, e) {
                        $(elm + " option[value='" + e + "']").prop("selected", true);
                    });
                }
                else if ($(elm).attr('type') == 'radio') {
                    $(elm).removeAttr("checked");
                    $(elm + " [value='" + value + "']").prop("checked", true);
                }
                else if ($(elm).attr('type') == 'checkbox') {
                    var values = value.split(",");
                    $(elm).prop("checked", false);
                    for (i = 0; i < values.length; i++) {
                        $(elm + "[value='" + values[i] + "']").prop("checked", true);
                    }
                }

                else {
                    $(elm).val(value);
                    if ($(elm).attr('data-value-for') != undefined) {
                        setvalueTo = $(elm).attr('data-value-for');
                        $("[name^='" + setvalueTo + "']").val(value);

                    }
                    if ($(elm).attr('data-for-switch') != undefined) {
                        setvalueTo = $(elm).attr("data-for-switch");
                        if (value.toUpperCase() == "TRUE") {
                            $("[name='" + setvalueTo + "']").prop("checked", true);
                        }
                        else {
                            $("[name='" + setvalueTo + "']").prop("checked", false);
                        }

                    }
                }
            }
            else {
                if ($(elm).attr('multiple') == 'multiple') {
                    $(elm).prop("checked", false);
                    $(elm + " option[value='" + value + "']").prop("selected", true);
                }
                else if ($(elm).attr('type') == 'radio') {
                    $(elm).removeAttr("checked");
                    $(elm + " [value='" + value + "']").prop("checked", true);
                }
                else if ($(elm).attr('type') == 'checkbox') {
                    $(elm).prop("checked", false);
                    $(elm + "[value='" + value + "']").prop("checked", true);
                }
                else {
                    $(elm).val(value);
                    if ($(elm).attr('data-value-for') != undefined) {
                        setvalueTo = $(elm).attr('data-value-for');
                        $("[name^='" + setvalueTo + "']").val(value);

                    }
                    if ($(elm).attr('data-for-switch') != undefined) {
                        setvalueTo = $(elm).attr("data-for-switch");
                        if (value.toUpperCase() == "TRUE") {
                            $("[name='" + setvalueTo + "']").prop("checked", true);
                        }
                        else {
                            $("[name='" + setvalueTo + "']").prop("checked", false);
                        }

                    }
                }
            }

        }

        else {
            if ($(elm).attr('multiple') == 'multiple') {

                $(elm + " option").prop("selected", false);
            }
            else if ($(elm).attr('type') == 'radio') {

                $(elm).prop("checked", false);
            }
            else if ($(elm).attr('type') == 'checkbox') {

                $(elm).prop("checked", false);
            }
            else {
                $(elm).val("");
                if ($(elm).attr('data-value-for') != undefined) {
                    setvalueTo = $(elm).attr('data-value-for');
                    $("[name^='" + setvalueTo + "']").val("");

                }
                if ($(elm).attr('data-for-switch') != undefined) {
                    setvalueTo = $(elm).attr("data-for-switch");
                    $("[name='" + setvalueTo + "']").prop("checked", false);
                }
            }

        }


    });

}
function setNewValueOnTableRow(values) {
    $.each(values, function (k, l) {
        var elmName = k;
        var elm = "[name='" + elmName + "']";
        var v = "";
        if (Array.isArray(l)) {
            $.each(l, function (i, e) {
                v = v + "," + e;
            });
            v = v.substr(1, v.length);
        }
        else {
            v = l;
        }
        if ($(elm).attr('multiple') == 'multiple') {
            $(elm).attr("data-act", v);
            $(elm).closest("td").find(".display-data").html(v);
        }
        else if ($(elm).attr('type') == 'radio') {

            $(elm).attr("data-act", v);
            $(elm).closest("td").find(".display-data").html(l);
        }
        else if ($(elm).attr('type') == 'checkbox') {
            $(elm).attr("data-act", v);
            $(elm).closest("td").find(".display-data").html(v);
        }
        else if ($(elm).attr("data-elm-type") != undefined && $(elm).attr("data-elm-type") === "label") {
            $(elm).attr("data-act", v);
            $(elm).closest("td").find(".display-data").html(v);
        }
        else if ($(elm).attr("data-elm-type") != undefined && $(elm).attr("data-elm-type") === "heading") {
            $(elm).attr("data-act", v);
            $(elm).closest("td").find(".display-data").html(v);
        }
        else {
            $(elm).attr("data-act", v);
            $(elm).closest("td").find(".display-data").html(v);
            if ($(elm).attr('data-value-for') != undefined) {
                setvalueTo = $(elm).attr('data-value-for');
                $("[name^='" + setvalueTo + "']").val(v);
                var t = $("[name^='" + setvalueTo + "']").val();
                $(elm).closest("td").find(".display-data").html(t);
            }
            if ($(elm).attr('data-for-switch') != undefined) {
                setvalueTo = $(elm).attr("data-for-switch");
                if (v.toUpperCase() == "TRUE") {
                    $("[name='" + setvalueTo + "']").prop("checked", true);
                }
                else {
                    $("[name='" + setvalueTo + "']").prop("checked", false);
                }

            }
        }

    })
}
function reintDatetime() {
    $('.datetime').daterangepicker({
        "singleDatePicker": true,
        "timePicker": true,
        locale: {
            format: 'DD/MM/YYYY hh:mm A'
        }
    });
    $('.date').daterangepicker({
        "singleDatePicker": true,
        locale: {
            format: 'DD MMMM, YYYY'
        }
    });
}



