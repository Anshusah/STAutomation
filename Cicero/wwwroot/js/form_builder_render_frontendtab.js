prevCount = 1;

$("input[data-types='currency']").on({
    keyup: function () {
        formatCurrency($(this));
    },
    blur: function () {
        formatCurrency($(this), "blur");
    }
});

function formatNumber(n) {
    // format number 1000000 to 1,234,567
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}
function formatCurrency(input, blur) {
    // appends $ to value, validates decimal side
    // and puts cursor back in right position.

    // get input value
    var input_val = input.val();

    // don't validate empty input
    if (input_val === "") { return; }

    // original length
    var original_len = input_val.length;

    // initial caret position 
    var caret_pos = input.prop("selectionStart");

    // check for decimal
    if (input_val.indexOf(".") >= 0) {

        // get position of first decimal
        // this prevents multiple decimals from
        // being entered
        var decimal_pos = input_val.indexOf(".");

        // split number by decimal point
        var left_side = input_val.substring(0, decimal_pos);
        var right_side = input_val.substring(decimal_pos);

        // add commas to left side of number
        left_side = formatNumber(left_side);

        // validate right side
        right_side = formatNumber(right_side);

        // On blur make sure 2 numbers after decimal
        if (blur === "blur") {
            right_side += "00";
        }

        // Limit decimal to only 2 digits
        right_side = right_side.substring(0, 2);

        // join number by .
        input_val = left_side + "." + right_side;

    } else {
        // no decimal entered
        // add commas to number
        // remove all non-digits
        input_val = formatNumber(input_val);

        input_val = input_val;

        // final formatting
        if (blur === "blur") {
            input_val += ".00";
        }
    }

    // send updated string to input

    input.val(input_val);

    var updated_len = input_val.length;

    caret_pos = updated_len - original_len + caret_pos;
    input[0].setSelectionRange(caret_pos, caret_pos);
}
$(":input").inputmask();


////showhide collection for selectbox
//let uu = $('[select-target]');
//$.each(uu, function (k, l) {
//    showHideBySelectBox(l);
//});

//let btnHideShow = $('[data-action-click=onClick]');
//$.each(btnHideShow, function (k, l) {
//    showHideByButton(l);
//});

//let checkboxHideShow = $('[data-checkbox=true]');
//$.each(checkboxHideShow, function (k, l) {
//    showHideByCheckbox(l);
//});

//function showHideByButton(e) {
//    var dataTargets = $(e).attr('data-target').split(',');
//    var dataActions = $(e).attr('data-target-action').split(',');
//    $.each(dataTargets, function (i, data) {
//        if (dataActions[i] === "True") {
//            $(data).hide();
//        }
//        else {
//            $(data).show();
//        }
//    });
//}

//function showHideByCheckbox(e) {
//    var dataTarget = $(e).attr('data-target');
//    var dataTargets = [];
//    if (typeof (dataTarget) !== "undefined") {
//        dataTargets = dataTarget.split(',');
//    }

//    var dataAction = $(e).attr('data-target-action');
//    var dataActions = [];
//    if (typeof (dataAction) !== "undefined") {
//        dataActions = dataAction.split(',');
//    }


//    $.each(dataTargets, function (i, data) {
//        if (dataActions[i] === "True") {
//            $(data).hide();
//        }
//        else {
//            $(data).show();
//        }
//    });
//}

//function showHideByCheckboxBySelection(e) {
//    var dataTarget = $(e).attr('data-target');
//    var dataTargets = [];
//    if (typeof (dataTarget) !== "undefined") {
//        dataTargets = dataTarget.split(',');
//    }

//    var dataAction = $(e).attr('data-target-action');
//    var dataActions = [];
//    if (typeof (dataAction) !== "undefined") {
//        dataActions = dataAction.split(',');
//    }

//    dataActions = FormBuilder.Element.Event().toggleElementForCheckbox(dataTargets, dataActions);
//    $(e).attr('data-target-action', dataActions);
//}

////showhide function for selectbox on change
//jQuery(document).on("change", "[select-target]", function () {
//    showHideBySelectBox(this);

//});

//jQuery(document).on("change", "[data-checkbox=true]", function () {
//    showHideByCheckboxBySelection(this);

//});

////showhide function for selectbox
//function showHideBySelectBox(e) {

//    if ($(e).children("option:selected").data("target") != null && $(e).children("option:selected").data("action") != "") {
//        let targets = $(e).children("option:selected").attr("data-target").split(",");
//        let actions = $(e).children("option:selected").attr("data-target-action").split(",");

//        for (var i = 0; i < targets.length; i++) {

//            if (actions[i].toLowerCase().trim() == "true") {
//                $(targets[i]).show();
//            }
//            else {
//                $(targets[i]).hide();
//            }
//        }
//    }

//}

////showhide collection for radio
//let rd = $('.radio-target');
//$.each(rd, function (k, l) {
//    showHideByRadio(l);
//});

////showhide function for selectbox on change
//jQuery(document).on("change", ".radio-target", function () {
//    showHideByRadio(this);

//});

////showhide function for radio
//function showHideByRadio(e) {
//    if ($(e).data("target") != null && $(e).data("target") != "" && $(e)[0].checked == true) {
//        let targets = $(e).attr("data-target").split(",");
//        let actions = $(e).attr("data-target-action").split(",");

//        for (var i = 0; i < targets.length; i++) {
//            if (actions[i].toLowerCase().trim() == "true") {
//                $(targets[i]).show();
//            }
//            else {
//                $(targets[i]).hide();
//            }
//        }
//    }

//}


////Radio Slider
////showhide collection for radio
//let rds = $('.radio-slider-target');
//$.each(rds, function (k, l) {
//    showHideByRadioSlider(l);
//});

////showhide function for selectbox on change
//jQuery(document).on("change", ".radio-slider-target", function () {
//    showHideByRadioSlider(this);

//});

////showhide function for radio
//function showHideByRadioSlider(e) {
//    if ($(e).data("targety") != null && $(e).data("targety") != "" && $(e)[0].checked == true) {
//        let targets = $(e).attr("data-targety").split(",");
//        let actions = $(e).attr("data-actiony").split(",");

//        for (var i = 0; i < targets.length; i++) {
//            if (actions[i].toLowerCase().trim() == "true") {
//                $(targets[i]).show();
//            }
//            else {
//                $(targets[i]).hide();
//            }
//        }
//    }
//    if ($(e).data("targetn") != null && $(e).data("targetn") != "" && $(e)[0].checked == false) {
//        let targets = $(e).attr("data-targetn").split(",");
//        let actions = $(e).attr("data-actionn").split(",");

//        for (var i = 0; i < targets.length; i++) {
//            if (actions[i].toLowerCase().trim() == "true") {
//                $(targets[i]).show();
//            }
//            else {
//                $(targets[i]).hide();
//            }
//        }
//    }
//}

////end Radio Slider

////show hide for Image
//let ii = $('.Image-target');
//$.each(ii, function (k, l) {

//    showHideByImage(l);
//});

////showhide function for selectbox on change
//jQuery(document).on("click", ".Image-target", function () {
//    $(this).parent().parent().find('.Image-target').removeClass('show');
//    $(this).addClass('show');
//    showHideByImage(this);

//});

////showhide function for selectbox
//function showHideByImage(e) {

//    if (e.classList.contains('show') && $(e).data("target") != null && $(e).data("target") != "") {
//        let targets = $(e).attr("data-target").split(",");
//        let actions = $(e).attr("data-target-action").split(",");
//        var arrayHide = [];
//        for (var i = 0; i < targets.length; i++) {
//            if (actions[i].toLowerCase().trim() == "true") {
//                $(targets[i]).show();
//                $('#accordionExample' + targets[i].replace('#', '')).show();
//                var oo = $(targets[i]).find('[number-target]');

//                $.each(oo, function (k, l) {

//                    let count = parseInt($(e).val());
//                    let jsonData1 = $(l).attr("data-target").split(",");
//                    if (isNaN(count)) {

//                        $.each(jsonData1, function (x, y) {
//                            arrayHide.push(y);


//                        });
//                    };

//                });

//            }
//            else {

//                $(targets[i]).hide();
//                $('#accordionExample' + targets[i].replace('#', '')).hide();

//            }
//        }
//        $.each(arrayHide, function (x, y) {
//            $(y).hide()
//        });

//    }

//}
////end of Show hide for Image




//clone on change number field
jQuery(document).on("change", "[number-target]", function () {
    cloneByNumber(this, "false");

});
//clone function using number field
//clone function using number field
function cloneByNumber(e, isRenderValue) {
    let value = $(e).val();
    if (isRenderValue == "true") {
        value = "";
    }
    let count = parseInt(value);
    let jsonData = $(e).attr("data-target").split(",");
    var all = $(e).attr("data-target");
    if (count >= prevCount || isNaN(count)) {
        var i = prevCount - 1;
        do {
            var all;
            $.each(jsonData, function (x, y) {
                if ($(y).attr("id") !== undefined) {
                    $.each($(y).find("input,select,textarea"), function (a, b) {
                        let name = $(b).attr("name");
                        if (name.indexOf("[") < 0) {
                            $(b).attr("repert-propertie", name);
                            $(b).attr("name", name + "[" + i + "]");
                            $(b).attr("id", "" + name + "[" + i + "]");
                        }
                    });
                }
            });
            if ($(all).attr("id") != undefined) {
                //let colname = $(y).attr("id").replace('#', '');
                let colname = $(all).attr("id");

                if (!$(all).parent().hasClass("repeat-item-body") && i == 0) {
                    $(all).addClass("repeatItem");
                    $(all).attr("name", colname + "_" + (i + 1));
                    $(all).removeAttr("style");
                    $(all).wrapAll("<div class='accordion card border p-4' id='accordionExample" + colname + "'><div class='repeat-item'><div id='collapse-" + (i + 1) + "' class='collapse show' aria-labelledby='headingOne' data-parent='#accordionExample" + colname + "'><div class='repeat-item-body'></div></div></div></div>");
                    $(all).parents(".repeat-item").prepend("<div class='repeat-item-header' id='heading-" + (i + 1) + "'><h6 class='repeat-item-title'><span data-toggle='collapse' data-target='#collapse-" + (i + 1) + "' aria-expanded='true' aria-controls='collapse' data-target='#collapse-" + (i + 1) + "'>" + $(e).siblings("label").html() + " Item " + (i + 1) + "</span></h6></div>");
                }

                if ($("#" + e.id).val() == "" || count == 0) {
                    $("[name='" + colname + "']:not([id])").parents(".card").remove();
                    $(all).parents(".accordion").hide();
                }
                else if (count == 1) {
                    $("[name='" + colname + "']:not([id])").remove();
                    $(all).parents(".accordion").show();
                }
                else {
                    if (i == 0) {
                        $("[name='" + colname + "']:not([id])").parents(".card").remove();
                    }

                    $(all).parents(".accordion").show();
                    $(all).removeAttr("style");
                    let temp = $(all).clone().removeAttr("id").attr("name", colname + "_" + (i + 2)).addClass("repeatItem");


                    $.each(temp.find("input,textarea,select"), function (c, d) {
                        $(d).attr("name", $(this).attr("repert-propertie") + "[" + (i + 1) + "]");
                        $(d).attr("id", "" + $(this).attr("repert-propertie") + "[" + (i + 1) + "]");
                    });

                    $.each(temp.find("select"), function (c, d) {
                        $(d).find("option:first").attr("selected", "selected");
                    });

                    $(temp).appendTo($("#accordionExample" + colname)).wrapAll("<div class='repeat-item'><div id='collapse-" + (i + 2) + "' class='collapse' aria-labelledby='headingOne' data-parent='#accordionExample" + colname + "'><div class='repeat-item-body'></div></div></div>");

                    $(temp).parents(".repeat-item").prepend("<div class='repeat-item-header' id='heading-" + (i + 2) + "'><h6 class='repeat-item-title'><span data-toggle='collapse' data-target='#collapse-" + (i + 2) + "' aria-expanded='false' aria-controls='collapse' data-target='#collapse-" + (i + 2) + "'>" + $(e).siblings("label").html() + " Item " + (i + 2) + "</span></h6></div>");

                }
                $(all).removeAttr("style");
            }

            i++;
        } while (i < count - 1);
        setCurrencyValueEvent();
    }
    else if (count < prevCount) {
        if (count < 1) {
            $(".repeat-item").slice(1).remove();
            $(all).parents(".accordion").hide();
        }
        else {
            $(".repeat-item").slice(count).remove();
        }
    }
    if (isNaN(count)) {
        prevCount = 1;
    }
    else {
        if (count == 0) {
            prevCount = 1;
        }
        else {
            prevCount = count;
        }


    }
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

//function cloneByNumber(e) {

//    let count = parseInt($(e).val());
//    let jsonData = $(e).data("target").split(",");
//    if (isNaN(count)) {

//        $.each(jsonData, function (x, y) {

//            $(y).hide()

//        });
//    }
//    else {

//        let datatemp = $(jsonData[0]);
//        var colids = $(jsonData[0]).attr("id");
//        datatemp.hide();
//        $('#accordionExample' + colids).html("");
//        $('#accordionExample' + colids).wrap(datatemp);

//        for (var i = 0; i < count; i++) {
//            $.each(jsonData, function (x, y) {

//                if ($(y).attr("id") !== undefined) {

//                    $.each($(y).find("input,select,textarea"), function (a, b) {
//                        let name = $(b).attr("name");
//                        if (name.indexOf("[]") < 0) {
//                            $(b).attr("name", name + "[]");
//                        }

//                    });


//                    if ($(y).attr("id") != undefined) {
//                        $(y).show();
//                        //let colname = $(y).attr("id").replace('#', '');
//                        let colname = $(y).attr("id");

//                        if (!$(y).parent().hasClass("repeat-item-body") && i == 0) {
//                            $(y).attr("class", "");

//                            $(y).wrap("<div class='accordion repeat-items' id='accordionExample" + colname + "'></div>").wrap("<div class='repeat-item'></div>").wrap("<div id='collapse-" + (i + 1) + "' class='collapse show' aria-labelledby='headingOne' data-parent='#accordionExample" + colname + "'></div>").wrap("<div class='repeat-item-body'></div>");

//                            $(y).parents(".repeat-item").prepend("<div class='repeat-item-header' id='heading-" + (i + 1) + "'><h5 class='repeat-item-header-title mb-0' data-toggle='collapse' data-target='#collapse-" + (i + 1) + "' aria-expanded='true' aria-controls='collapse' data-target='#collapse-" + (i + 1) + "'>Item " + (i + 1) + "</h5></div>");
//                        }

//                        if (count == 1) {
//                            //$("[name='" + colname + "']:not([id])").remove();
//                            $(y).parents(".accordion").show();
//                        }
//                        else {
//                            if (i != 0) {
//                                $(y).parents(".accordion").show();
//                                let temp = $(y).clone().removeAttr("id").attr("name", colname);

//                                $.each(temp.find("input,select,textarea"), function (c, d) {
//                                    $(d).removeAttr("id").val("");
//                                });

//                                $(temp).appendTo($("#accordionExample" + colname)).wrap("<div class='repeat-item'></div>").wrap("<div id='collapse-" + (i + 1) + "' class='collapse' aria-labelledby='headingOne' data-parent='#accordionExample" + colname + "'></div>").wrap("<div class='repeat-item-body'></div>");

//                                $(temp).parents(".repeat-item").prepend("<div class='repeat-item-header' id='heading-" + (i + 1) + "'><h5 class='repeat-item-header-title mb-0' data-toggle='collapse' data-target='#collapse-" + (i + 1) + "' aria-expanded='false' aria-controls='collapse' data-target='#collapse-" + (i + 1) + "'>Item " + (i + 1) + "</h5></div>");

//                            }


//                        }
//                    }

//                }

//            });

//        }


//    }




//    $(document).on("keypress", ".datetime", function (e) {
//        e.preventDefault();
//    });

//    $(document).on("keypress", ".date", function (e) {
//        e.preventDefault();
//    });

//    $(document).on("keypress", ".time", function (e) {
//        e.preventDefault();
//    });

//    $('.datetime').daterangepicker({
//        "singleDatePicker": true,
//        "timePicker": true,
//        locale: {
//            format: 'DD/MM/YYYY hh:mm A'
//        }
//    });

//    $('.date').daterangepicker({
//        "singleDatePicker": true,
//        locale: {
//            format: 'DD MMMM, YYYY'
//        }
//    });

//    $('.time').daterangepicker({
//        timePicker: true,
//        timePicker24Hour: true,
//        timePickerIncrement: 1,
//        //timePickerSeconds: true,
//        singleDatePicker: true,
//        opens: 'right',
//        locale: {
//            format: 'HH:mm A'
//        }
//    }).on('show.daterangepicker', function (ev, picker) {
//        picker.container.find(".calendar-table").hide();
//    });
//}

//datetime load ui
jQuery(document).ready(function () {
    $(document).on("keypress", ".datetime", function (e) {
        e.preventDefault();
    });

    $(document).on("keypress", ".date", function (e) {
        e.preventDefault();
    });

    $(document).on("keypress", ".time", function (e) {
        e.preventDefault();
    });

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

    $('.time').daterangepicker({
        timePicker: true,
        timePicker24Hour: true,
        timePickerIncrement: 1,
        //timePickerSeconds: true,
        singleDatePicker: true,
        opens: 'right',
        locale: {
            format: 'HH:mm A'
        }
    }).on('show.daterangepicker', function (ev, picker) {
        picker.container.find(".calendar-table").hide();
    });
});

//validation section
function validatefield(invalidsection) {

    $.each(invalidsection, function (i, v) {
        let divName = $("#" + this).attr('href');
        let checker = true;
        $(divName + ' :input').each(function () {

            let validatingform = $('#caseform').validate().element(this);
            if (validatingform == false) {
                checker = false;
            }

        });
        if (checker == true) {
            $("#" + v).removeClass('false');
            $("#" + v).addClass('true');
        }
        else {
            $("#" + v).removeClass('true');
            $("#" + v).addClass('false');
        }
    });
};
//tab count change
function changeCount() {
    let tabcount = $("#myTabContent").find(".tab-pane").length;
    let span = $("#additional-information-tab>span.nav-count>span");
    let span_payment = $("#payment-details-tab>span.nav-count>span");
    $("#payment-details-tab>span.nav-count").html(tabcount - 1);
    $("#payment-details-tab>span.nav-count").append(span_payment);
    $("#additional-information-tab>span.nav-count").html(tabcount);
    $("#additional-information-tab>span.nav-count").append(span);
}
//change continue button to send
function changebutton() {
    let invalidsection = [];
    let i = 0;
    $.each($("ul.nav.nav-tabs>li>a.nav-link"), function () {
        i++;
        var isvalid = $(this).hasClass("true");
        if (!isvalid) {

            invalidsection.push($(this).attr('id'));
        }

        //back btn
        if ($(this).hasClass("active")) {
            if (i != 1) {
                $("#back-btn").show();
            }
            else {
                $("#back-btn").hide();
            }
        }


        //continue btn
        if ($(this).hasClass("active") && $("ul.nav.nav-tabs>li>a.nav-link").length == i) {
            $("#next-btn").hide();
        }
        else {
            $("#next-btn").show();
        }

    });
    //console.log(invalidsection.length);
    if (invalidsection.length == 0) {

        $("#send-btn").show();
        $("#next-btn").hide();
        $("#back-btn").show();
    }
};



//tab click event
$(document).on("click", "ul.nav.nav-tabs>li>a.nav-link", function () {

    changebutton();
});

$(document).on("click", "#back-btn", function () {

    if (!$(this).hasClass('true')) {
        let invalidsection = [];
        invalidsection.push($(".nav.nav-tabs").find('a.active').attr('id'));
        //  validatefield(invalidsection);
    }
    $(".nav").find(".nav-link.active").parent().prev(".nav-item").children(".nav-link").trigger("click");

    changebutton();
});