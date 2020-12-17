var FormBuilder = FormBuilder || {};
var timer;
FormBuilder.Element = FormBuilder.Element || {};
FormBuilder.Element.Event = FormBuilder.Element.Event || {};
FormBuilder.Element.Event = function () {
    function Event() {
        this.init = function () {
            $('body').on('click', '[data-action-click]', function () {
                var eleName = $(this).attr('name');
                eval("FormBuilder.Element.Event()." + $(this).data('action-click') + "('" + eleName + "')");

            });
            $('body').on('change', '[data-action-onchange]', function () {
                var name1 = $(this).attr('name');
                eval("FormBuilder.Element.Event()." + $(this).data('action-onchange') + "('" + name1 + "')");
            });

            $('body').on('keyup', '[data-action-onkeyup]', function () {
                clearTimeout(timer);
                var str = $(this).val();
                var name1 = $(this).attr('name');
                var element = $(this);
                timer = setTimeout(function () {
                    value = str;
                    eval("FormBuilder.Element.Event()." + element.data('action-onkeyup') + "('" + name1 + "')");
                }, 800);
            });

        };
        this.getEventType = function (type) {
            switch (type) {
                case "onload":
                    return 0;
                case "onclick":
                    return 1;
                case "onchange":
                    return 2;
                case "onkeyup":
                    return 3;
                case "onfocus":
                    return 4;
            }
        };

        this.onLoad = function (name) {
            FormBuilder.Element.Event().setWorkData(name, "onload");
        };
        this.onClick = function (name) {
            var isDataWorkEvent = $('[name=' + name + ']').attr('data-work-onclick');
            var isSaveFormEvent = $('[name=' + name + ']').attr('data-saveform');
            var switchTab = $('[name=' + name + ']').attr('data-switchTab');
            var isValidateOnSwitchTab = $('[name=' + name + ']').attr('data-switchTab-validate');
            var clearTab = $('[name=' + name + ']').attr('data-cleartabfields');

            if (clearTab !== undefined) {
                var itm = $('[name=' + name + ']').closest(".tab-pane");
                FormBuilder.Element.Event().clearTabField(itm);
            }
            if (isDataWorkEvent) {
                FormBuilder.Element.Event().setWorkData(name, "onclick", function (d) {
                    if (d) {
                        if (isSaveFormEvent === "true") {
                            FormBuilder.Element.Event().saveForm(function () {
                                if (switchTab !== "0" && switchTab !== undefined) {
                                    FormBuilder.Element.Event().switchTab(switchTab);
                                }
                            });
                        }
                        else {
                            if (switchTab !== "0" && switchTab !== undefined) {
                                if (isValidateOnSwitchTab === "true") {
                                    var checkValid = checkValidation(formTagName, form, true).then(function (check) {
                                        $("#loading").attr("style", "display:none;");
                                        if (!check) {
                                            //fail
                                        }
                                        else {
                                            FormBuilder.Element.Event().switchTab(switchTab);
                                        }
                                    });
                                }
                                else {
                                    FormBuilder.Element.Event().switchTab(switchTab);
                                }

                            }
                        }
                    }

                });

            }
            else {
                var e = $("[name='" + name + "']");
                if (typeof e.data('target') !== 'undefined') {
                    if (typeof e.data('action') !== 'undefined') {
                        var dataTargets = e.attr('data-target').split(',');
                        var dataActions = e.attr('data-action').split(',');
                        dataActions = FormBuilder.Element.Event().toggleElement(dataTargets, dataActions);
                        e.attr('data-action', dataActions);
                    }
                }

                if (isSaveFormEvent === "true") {
                    FormBuilder.Element.Event().saveForm(function () {
                        if (switchTab !== "0" && switchTab !== undefined) {
                            FormBuilder.Element.Event().switchTab(switchTab);
                        }
                    });
                }
                else {
                    if (switchTab !== "0" && switchTab !== undefined) {
                        if (isValidateOnSwitchTab === "true") {
                            var checkValid = checkValidation(formTagName, form, true).then(function (check) {
                                $("#loading").attr("style", "display:none;");
                                if (!check) {
                                    //fail
                                }
                                else {
                                    FormBuilder.Element.Event().switchTab(switchTab);
                                }
                            });
                        }
                        else {
                            FormBuilder.Element.Event().switchTab(switchTab);
                        }

                    }
                }
            }

        };
        this.onChange = function (name) {
            FormBuilder.Element.Event().setWorkData(name, "onchange");
        };
        this.onKeyUp = function (name) {
            FormBuilder.Element.Event().setWorkData(name, "onkeyup");
        };
        this.saveForm = function (callback) {
            var isAjax = true;
            var checkValid = checkValidation(formTagName, form, isAjax).then(function (check) {

                if (!check) {
                    $("#loading").attr("style", "display:none;");
                    return false;
                }

                var fd = $("#caseform").serializeArray(); //formdata
                var d = {}; //data
                var hd = $("#caseform" + " div[style*='display']"); //hidden form data
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
                d["cvm"] = cvm;
                d["tenant_identifier"] = tenant_identifier;
                d["form"] = form;

                $.ajax({
                    async: false,
                    dataType: 'json',
                    url: "/admin/form/ajax/edit.html",
                    type: "post",
                    data: d,
                    success: function (response) {
                        $('#elmcaseid').val(response.encryptedcaseid);
                        $('#encryptedcaseid').val(response.encryptedcaseid);
                        $('#case_id').val(response.encryptedcaseid);
                        cvm.id = response.cvm.id;
                        //                        if (form.toLowerCase() !== "jazzcash") {
                        //                          $('#elmformid').val(response.encryptedcaseid);
                        //                    }
                        var elems = $('[data-setvaluefrom]');
                        $.each(elems, function (i, v) {
                            var elemId = "elm" + $(v).attr("data-setvaluefrom");
                            if ($(v).attr("data-elm-type") === "label") {
                                $(v).find('.label-text').text(response.formDatas[elemId.toString()]);
                            }
                            else if ($('#' + elemId).attr('data-elm-type') === "label") {
                                response.formDatas[v.id.toString()] = $('#' + elemId).find('.label-text').text();
                            }
                            else {
                                response.formDatas[v.id.toString()] = response.formDatas[elemId.toString()];
                            }
                        });

                        renderData(response.formDatas);
                        if ($(".beneCity").length > 0) {
                            if ($(".beneCity").val().length === 1) {
                                $(".beneCountry").trigger('change');
                            }
                        }

                        //$('.nav-link.active').parent().next().find('a').click();
                        removePreviousValidations();
                        $("#loading").attr("style", "display:none;");

                        if (typeof (callback) !== "undefined") {
                            return callback();
                        }

                        $("#loading").attr("style", "display:none;");
                    },
                    error: function (error) {
                        $("#loading").attr("style", "display:none;");
                        if (error.status === 500) {
                            location.href = "/st/user/login.html";
                        }

                        //if (error.status === 400) {
                        //    toastr.warning("The Identity Verification Process is in progress. Please try again after verification completed.");
                        //}
                        var message = JSON.parse(error.responseText).message;
                        if (message) {
                            if (error.status === 404 && message.toString().includes("Onfido")) {
                                toastr.info("You have not verified your identity yet. Do you want to verify now?<br /><br /><button type='button' onClick='triggerOnfido()' class='btn btn-success clear'>Yes</button><button type='button' class='btn btn-danger clear'>No</button>", { timeOut: 50000 });
                            }
                            else {
                                toastr.info("You are listed in Sanction Pep.");
                            }
                        }
                       
                    }
                });
            });
        };

        this.switchTab = function (tabId) {
            $('#tab' + tabId).click();
            $(window).scrollTop(0);
        };
        this.clearTabField = function (itm) {
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
        };

        this.setWorkData = function (name, type, callback) {
            var e = $("[name='" + name + "']");

            if (e.length > 0) {
                var check = e.attr("data-work-" + type);
                var parnames = e.attr("data-val-" + type + "-parname").split(",");
                var parvalidate = e.attr("data-val-" + type + "-parname-validation").split(",");
                var parelements = e.attr("data-val-" + type + "-parelm").split(",");
                var resnames = e.attr("data-val-" + type + "-resname").split(",");
                var reselements = e.attr("data-val-" + type + "-reselm").split(",");
                var start = 0;
                var end = 0;
                var caseFormId = e.attr("data-work-" + type + "-formid");
                var targets = "";
                var triggerEvent = [];
                if (e.attr("data-val-" + type + "-restrigger") !== undefined) {
                    triggerEvent = e.attr("data-val-" + type + "-restrigger").split(",");
                }
                var elementId = name.split("m")[1];

                if ($("[data-target-setting-for='" + e.attr('name') + "']").length !== 0) {
                    targets = $("[data-target-setting-for='" + e.attr('name') + "']").val();
                    try {
                        targets = JSON.parse(targets);
                    }
                    catch (ex) {
                        console.log();
                    }
                }
                datav1 = {};
                validateData = {};
                valData = [];
                var datapar = [];
                var datares = [];
                var elm;
                var elmType;
                var value;
                var t;
                var a;

                if (parnames.length === parelements.length) { //get elements to validate before call
                    for (var i = 0; i < parnames.length; i++) {
                        elm = $("[name='elm" + parelements[i] + "']");
                        elmType = elm.attr("data-elm-type");
                        if (elmType === 'multiselectbox') { //for multi select box
                            t = $("[name='elm" + parelements[i] + "']").val();
                            $.each(t, function (k, v) {
                                value = value + v;
                                if (k !== t.length - 1) {
                                    value = value + ",";
                                }
                            });
                        }
                        else if (elmType === 'radiogroup' || elmType === 'checkboxgroup') { //for radio button and checkbox
                            value = $("[name='elm" + parelements[i] + "']:checked").val();

                        }
                        else if (elmType === "selectbox") { //for select box
                            if (elm.hasClass("selectpicker")) {
                                if (elm.attr("data-act")) {
                                    value = elm.attr("data-act");
                                }
                                else {
                                    value = elm.val();
                                }
                            }
                            else {
                                value = elm.val();
                            }
                        }
                        else {
                            value = $("[name='elm" + parelements[i] + "']").val();
                        }

                        if (parvalidate[i] === "True") {
                            validateData["elm" + parelements[i]] = value;
                            valData.push("elm" + parelements[i]);
                        }
                    }
                }
                if (parnames.length === parelements.length) {
                    for (i = 0; i < parnames.length; i++) {
                        elm = $("[name='elm" + parelements[i] + "']");
                        elmType = elm.attr("data-elm-type");
                        if (elmType === 'multiselectbox') { //for multi select box
                            t = $("[name='elm" + parelements[i] + "']").val();
                            $.each(t, function (k, v) {
                                value = value + v;
                                if (k !== t.length - 1) {
                                    value = value + ",";
                                }
                            });
                        }
                        else if (elmType === 'radiogroup' || elmType === 'checkboxgroup') { //for radio button and checkbox
                            value = $("[name='elm" + parelements[i] + "']:checked").val();

                        }
                        else if (elmType === "selectbox") { //for select box

                            if (elm.hasClass("selectpicker")) {
                                if (elm.attr("data-act")) {
                                    value = elm.attr("data-act");
                                }
                                else {
                                    value = elm.val();
                                }
                            }
                            else {
                                value = elm.val();
                            }
                        }
                        else {
                            value = $("[name='elm" + parelements[i] + "']").val();
                        }

                        temp = { "Name": parnames[i], "ElementId": parelements[i], "Value": value };
                        datapar.push(temp);
                    }
                }
                if (reselements.length === resnames.length) {
                    for (i = 0; i < resnames.length; i++) {
                        value = $("[name='elm" + reselements[i] + "']").val();
                        temp = { "Name": resnames[i], "ElementId": reselements[i], "Value": value, "IsTriggerEvent": triggerEvent[i] };
                        datares.push(temp);
                    }
                }
                var work = { "Start": start, "End": end, "CaseFormId": caseFormId, "EventType": FormBuilder.Element.Event().getEventType(type), "ElementId": elementId };
                datav1 = { "Parameter": datapar, "Response": datares, "Work": work, "Targets": targets };

                if (check === "true") {
                    if (jQuery.isEmptyObject(validateData) === false) {

                        var allElements = $("[name^='elm']");
                        var hd = [];
                        for (i = 0; i < allElements.length; i++) {
                            if (jQuery.inArray($(allElements[i]).attr("name"), valData) === -1) {
                                hd.push($(allElements[i]).attr("name")); //get all elments except parameter elem
                            }
                        }
                        FormBuilder.Element.Event().validateElement(caseFormId, validateData, hd, function () {

                            // console.log(caseFormId);

                            if (work.CaseFormId !== "" && work.ElementId !== "" && work.EventType >= 0) {
                                FormBuilder.Element.Event().callWorkFlow(datav1, e, function (d) {
                                    if (typeof (callback) !== "undefined") {
                                        return callback(d);
                                    }
                                });
                            }
                            else {
                                if (typeof e.data('target') !== 'undefined') {
                                    if (typeof e.data('action') !== 'undefined') {
                                        var dataTargets = e.attr('data-target').split(',');
                                        var dataActions = e.attr('data-target-action').split(',');
                                        dataActions = FormBuilder.Element.Event().toggleElement(dataTargets, dataActions);
                                        e.attr('data-action', dataActions);
                                    }
                                }
                            }
                        });
                    }
                    else {

                        // console.log(caseFormId);
                        if (work.CaseFormId !== "" && work.ElementId !== "" && work.EventType >= 0) {
                            FormBuilder.Element.Event().callWorkFlow(datav1, e, function (d) {
                                if (typeof (callback) !== "undefined") {
                                    return callback(d);
                                }
                            });
                        }
                        else {
                            if (typeof e.data('target') !== 'undefined') {
                                if (typeof e.data('action') !== 'undefined') {
                                    var dataTargets = e.attr('data-target').split(',');
                                    var dataActions = e.attr('data-target-action').split(',');
                                    dataActions = FormBuilder.Element.Event().toggleElement(dataTargets, dataActions);
                                    e.attr('data-action', dataActions);
                                }
                            }
                        }
                    }


                }
            }         

        };
        this.callWorkFlow = function (datav1, e, callback) {
            $.ajax({
                type: "POST",
                dataType: "json",
                data: JSON.stringify(datav1),
                url: "/admin/workflow/run",
                contentType: "application/json",
                success: function (response) {
                    var res = response.data;
                    if (!response.success) {
                        toastr.error(response.message);
                    }
                    if (typeof e.data('target') !== 'undefined') {
                        if (typeof e.data('action') !== 'undefined') {
                            var dataTargets = e.attr('data-target').split(',');
                            var dataActions = e.attr('data-action').split(',');
                            if (res !== "" && response.success) {
                                dataActions = FormBuilder.Element.Event().toggleElementForValidation(dataTargets, dataActions, true);
                            }
                            else {
                                dataActions = FormBuilder.Element.Event().toggleElementForValidation(dataTargets, dataActions, false);
                                toastr.error(response.message);
                            }
                            e.attr('data-action', dataActions);
                        }
                    }
                    // else {

                    //   }

                    if (response.target !== "") {
                        FormBuilder.Element.Event().setTarget(response.target, function () {
                            FormBuilder.Element.Event().setWorkFlowValues(res, e, function () {
                                if (typeof (callback) !== "undefined") {
                                    return callback(response.success);
                                }
                            });
                        });
                    }
                    else {
                        FormBuilder.Element.Event().setWorkFlowValues(res, e, function () {
                            if (typeof (callback) !== "undefined") {
                                return callback(response.success);
                            }
                        });
                    }
                    // console.log(JSON.stringify(response));

                }
            });
        };

        this.setWorkFlowValues = function (datav1, e, callback) {
            var isParTableElm = false;
            var parTableElmName = e.attr("name");

            var dataAct;
            if (e.attr("data-table-elm") !== undefined) {
                isParTableElm = true;
            }
            for (i = 0; i < datav1.length; i++) {
                var isResTableElm = false;
                var elmName = "elm" + datav1[i].elementId;
                if ($("[name='" + elmName + "']").length === 0) { //check for tableElement
                    isResTableElm = true;
                    if (isParTableElm === true) {
                        var start = parTableElmName.indexOf("[") + 1;
                        var end = parTableElmName.indexOf("]");
                        var index = parseInt(parTableElmName.substring(start, end));
                        elmName = elmName + "[" + index + "]";
                    }

                }
                var elm = $("[name='" + elmName + "']");
                if (elm.length > 1) {
                    elm = $(elm[0]);
                }
                var values;
                var elmType = elm.attr("data-elm-type");
                try {
                    values = JSON.parse(datav1[i].value);
                }
                catch (e) {
                    values = datav1[i].value;
                }
                if (elmType === 'multiselectbox') { //for multi select box
                    if (Array.isArray(values)) {
                        elm.html("");
                        for (j = 0; j < values.length; j++) {
                            if (values[j].selected === true) {
                                elm.append("<option value='" + values[j].value + "' selected>" + values[j].text + "</option>");
                            }
                            else {
                                elm.append("<option value='" + values[j].value + "'>" + values[j].text + "</option>");
                            }

                        }
                    } else {
                        elm.html("<option value='' selected>Select Option</option>");
                        elm.append("<option value='" + values.value + "'>" + values.text + "</option>");

                    }
                    if (elm.attr("data-act") !== undefined && isResTableElm) { //set value
                        if (elm.attr("data-act-set") === undefined) {
                            dataAct = elm.attr("data-act");
                            elm.val(dataAct);
                            elm.attr("data-act-set", "true");
                            elm.trigger("change");
                        }
                    }
                }

                else if (elmType === 'radiogroup' || elmType === 'checkboxgroup') { //for radio button and checkbox
                    temp = elm.parent();
                    appendTo = temp.parent();
                    appendLast = appendTo.find(".d-flex");

                    if (Array.isArray(values)) {
                        appendTo.html("");
                        for (j = 0; j < values.length; j++) {
                            cloned = temp.clone();
                            cloned.find("label").attr("for", "elm" + values[j].text).html(values[j].text);
                            cloned.find("input").attr("value", values[j].value);
                            cloned.find("input").attr("id", "elm" + values[j].text);
                            cloned.find("i").attr("for", "elm" + values[j].text);
                            if (values[j].selected === true) {
                                cloned.find("input").prop("checked", true);
                            }
                            appendTo.append(cloned);
                        }
                    }
                    else {
                        if (typeof (values) === "object") {
                            appendTo.html("");
                            cloned = temp.clone();
                            cloned.find("label").attr("for", "elm" + values.text).html(values.text);
                            cloned.find("input").attr("value", values.value);
                            cloned.find("input").attr("id", "elm" + values.text);
                            if (values.selected === true) {
                                cloned.find("input").prop("checked", true);
                            }
                            cloned.find("i").attr("for", "elm" + values.text);
                            appendTo.append(cloned);
                        }
                        else {
                            var checkedVal = values.toString().split(",");
                            $("[name='" + elmName + "']").prop("checked", false);
                            $.each(checkedVal, function (i, v) {
                                $("[name='" + elmName + "'][value='" + v + "']").prop("checked", true);
                            });
                        }

                    }
                    if (elm.attr("data-act") !== undefined && isResTableElm) {//set value
                        if (elm.attr("data-act-set") === undefined) {
                            dataAct = elm.attr("data-act").split(",");
                            $.each(values, function () {
                                $("[name='" + elmName + "'] [value=" + this + "]").prop("checked", "checked");
                            });
                            //elm.val(dataAct);
                            elm.attr("data-act-set", "true");
                            elm.trigger("change");
                        }
                    }



                }

                else if (elmType === "selectbox") { //for select box
                    if (elm.attr("type") === "text-box") {
                        elm.val(datav1[i].value);
                        if (elm.attr("data-elm-select") !== undefined) {
                            if (elm.attr('data-elm-select') === "elmSelect") {
                                var a = $("a[value='" + datav1[i].value + "']");

                                a.parent().parent().find('.Image-target').removeClass('show');
                                a.addClass('show');
                                showHideByImage(a[0]);
                                a.trigger('click');

                            }

                        }
                    }
                    else {
                        if (Array.isArray(values)) {
                            elm.html("");
                            if (elm.hasClass('selectpicker')) { // for selectpicker
                                for (j = 0; j < values.length; j++) {
                                    if (values[j].selected === true) {
                                        if (values[j].icon !== undefined && values[j].icon !== "") {
                                            elm.append("<option value='" + values[j].value + "' selected data-icon='" + values[j].icon + "'>" + values[j].text + "</option>");
                                        }
                                        else {
                                            elm.append("<option value='" + values[j].value + "' selected>" + values[j].text + "</option>");
                                        }
                                    }
                                    else {
                                        if (values[j].icon !== undefined && values[j].icon !== "") {
                                            elm.append("<option value='" + values[j].value + "' data-icon='" + values[j].icon + "'>" + values[j].text + "</option>");
                                        }
                                        else {
                                            elm.append("<option value='" + values[j].value + "' selected>" + values[j].text + "</option>");
                                        }
                                    }
                                }


                                if (elm.attr("data-act") !== undefined && isResTableElm) { //set value
                                    if (elm.attr("data-act-set") === undefined) {
                                        dataAct = elm.attr("data-act");
                                        elm.val(dataAct);
                                        elm.attr("data-act-set", "true");
                                        elm.trigger("change");
                                    }
                                }
                                elm.selectpicker("refresh");
                                elm.parent(".dropdown").addClass("form-control");
                            } else {
                                for (j = 0; j < values.length; j++) {
                                    if (values[j].selected === true) {
                                        elm.append("<option value='" + values[j].value + "' selected>" + values[j].text + "</option>");
                                    }
                                    else {
                                        elm.append("<option value='" + values[j].value + "'>" + values[j].text + "</option>");
                                    }

                                }
                                if (elm.attr("data-act") !== undefined && isResTableElm) {//set value
                                    if (elm.attr("data-act-set") === undefined) {
                                        dataAct = elm.attr("data-act");
                                        elm.val(dataAct);
                                        elm.attr("data-act-set", "true");
                                        elm.trigger("change");
                                    }
                                }
                            }

                        } else {
                            if (elm.hasClass('selectpicker')) { //for selectpicker

                                elm.html("<option value='' selected>Select Option</option>");
                                elm.append("<option value='" + values.value + "'>" + values.text + "</option>");

                                if (elm.attr("data-act") !== undefined && isResTableElm) { //set value
                                    if (elm.attr("data-act-set") === undefined) {
                                        dataAct = elm.attr("data-act");
                                        elm.val(dataAct);
                                        elm.attr("data-act-set", "true");
                                        elm.trigger("change");
                                    }
                                }
                                elm.selectpicker("refresh");
                                elm.parent(".dropdown").addClass("form-control");
                            }
                            else {
                                elm.html("<option value='' selected>Select Option</option>");
                                elm.append("<option value='" + values.value + "'>" + values.text + "</option>");

                                if (elm.attr("data-act") !== undefined && isResTableElm) {//set value
                                    if (elm.attr("data-act-set") === undefined) {
                                        dataAct = elm.attr("data-act");
                                        elm.val(dataAct);
                                        elm.attr("data-act-set", "true");
                                        elm.trigger("change");
                                    }
                                }
                            }


                        }
                    }
                }

                else if (elmType === "label")//for label
                {
                    var stop = false;
                    var selected = elm.find(".label-text");
                    if (selected.length > 0) {
                        do {
                            if (selected.children().length > 0) {
                                selected = selected.children();
                            }
                            else {
                                stop = true;
                            }

                        } while (stop === false);
                        selected.html(values);
                    }
                    else {
                        selected = elm.find('.label-icon');
                        do {
                            if (selected.children().length > 0) {
                                selected = selected.children();
                            }
                            else {
                                stop = true;
                            }
                        } while (stop === false);

                        selected.removeClass();
                        selected.addClass(values);
                    }

                }

                else if (elmType === "paragraph" || elmType === "heading") //for paragraph and heading
                {
                    elm.html(values);
                }

                else {//for text box, number and textarea
                    elm.val(datav1[i].value);

                    if (elm.attr("data-act") !== undefined && isResTableElm) {//set value
                        if (elm.attr("data-act-set") === undefined) {
                            dataAct = elm.attr("data-act");
                            elm.val(dataAct);
                            elm.attr("data-act-set", "true");
                            elm.trigger("keyup");
                        }
                    }
                    if (elm.attr("data-elm-select") !== undefined) {
                        if (elm.attr('data-elm-select') === "elmSelect") {
                            a = $("a[value='" + datav1[i].value + "']");

                            a.parent().parent().find('.Image-target').removeClass('show');
                            a.addClass('show');
                            showHideByImage(a[0]);
                            a.trigger('click');

                        }

                    }
                }

                if (datav1[i].isTriggerEvent === "True" && elm.attr("data-action-onchange") !== undefined) {
                    elm.trigger('change');
                }
            }
            if (typeof (callback) !== "undefined") {
                removePreviousValidations();
                return callback();
            }
        };

        this.toggleElement = function (dataTargets, dataActions) {
            $.each(dataTargets, function (i, data) {
                if (dataActions[i] === "True") {
                    $(data).show();
                    dataActions[i] = "True";
                }
                else {
                    $(data).hide();
                    dataActions[i] = "False";
                }
            });
            return dataActions;
        };

        this.toggleElementForCheckbox = function (dataTargets, dataActions) {
            $.each(dataTargets, function (i, data) {
                if (dataActions[i] === "True") {
                    $(data).show();
                    dataActions[i] = "False";
                }
                else {
                    $(data).hide();
                    dataActions[i] = "True";
                }
            });
            return dataActions;
        };

        this.toggleElementForValidation = function (dataTargets, dataActions, value) {
            $.each(dataTargets, function (i, data) {
                if (value) {
                    $(data).show();
                    dataActions[i] = "True";
                }
                else {
                    $(data).hide();
                    dataActions[i] = "True";
                }
            });
            return dataActions;
        };

        this.validateElement = function (formId, d, hd, callback) {
            var isFront = false;
            if ($("#isFrontEnd").val() === "frontend") {
                isFront = true;
            }
            $.ajax({
                async: false,
                type: "POST",
                url: '/admin/workflow/elementvalidation.html',
                data: { formData: d, formId: formId, exclude: hd, isFrontValidation: isFront },
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.isFormValid) {
                        if (typeof (callback) !== "undefined") {
                            return callback();
                        }
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
                        $("#send-btn").removeAttr("disabled");
                        $("#save-btn").removeAttr("disabled");
                        $("#loading").attr("style", "display:none;");

                    }
                },
                error: function (response) {
                    //  console.log(response);
                }
            });
            setFocusOnInvalidElement();
        };

        this.setTarget = function (target, callback) {
            try {
                target = JSON.parse(target);
            }
            catch (ex) {
                target = target;
            }
            for (i = 0; i < target.length; i++) {

                elm = $("[name='elm" + target[i].elmName + "']");
                if (elm.length === 0) {
                    elm = $("#" + target[i].elmName);
                }
                otherElm = "";
                if (elm.attr('hidden') === "hidden") {
                    otherElm = $("[name*='" + target[i].elmName + "']");
                }
                switch (target[i].type) {
                    case "0":

                        switch (target[i].setAs) {
                            case "disable":
                                elm.attr("disabled", "disabled");
                                if (otherElm !== "") {
                                    otherElm.attr("disabled", "disabled");
                                }
                                break;
                            case "enable":
                                elm.removeAttr("style");
                                elm.removeAttr("disabled");
                                if (otherElm !== "") {
                                    otherElm.removeAttr("disabled");
                                }
                                break;
                            case "hide":
                                elm.closest(".form-group").attr("style", "display:none");
                                if (otherElm !== "") {
                                    otherElm.attr("style", "display:none");
                                }
                                elm.attr("style", "display:none");
                                break;
                            case "show":
                                elm.closest(".form-group").removeAttr("style");
                                if (otherElm !== "") {
                                    otherElm.removeAttr("style");
                                }
                                elm.removeAttr("style");
                                break;
                        };
                        break;

                    case "1":
                        for (j = 0; j < target[i].subTarget.length; j++) {
                            var elmType = elm.attr("data-elm-type");
                            if (elmType !== undefined && elmType !== "") {
                                switch (elmType) {
                                    case "multiselectbox":
                                        switch (target[i].subTarget[j].setAs) {
                                            case "disabled":
                                                elm.find("option[value='" + target[i].subTarget[j].value + "']").attr("disabled", "disabled");
                                                break;
                                            case "enabled":
                                                elm.find("option[value='" + target[i].subTarget[j].value + "']").removeAttr("disabled");
                                                break;
                                            case "hide":
                                                elm.find("option[value='" + target[i].subTarget[j].value + "']").attr("style", "display:none");
                                                break;
                                            case "show":
                                                elm.find("option[value='" + target[i].subTarget[j].value + "']").removeAttr("style", "display:none");
                                                break;
                                        }

                                        break;
                                    case "radiogroup":
                                        subElm = $("[data-name='" + target[i].elmName + "'][value='" + target[i].subTarget[j].value + "']");
                                        switch (target[i].subTarget[j].setAs) {
                                            case "disabled":
                                                subElm.attr("disabled", "disabled");
                                                break;
                                            case "enabled":
                                                subElm.removeAttr("disabled");
                                                break;
                                            case "hide":
                                                subElm.parent("div").attr("style", "display:none;");
                                                break;
                                            case "show":
                                                subElm.parent("div").removeAttr("style");
                                                break;
                                        }
                                        break;
                                    case "checkboxgroup":
                                        subElm = $("[data-name='" + target[i].elmName + "'][value='" + target[i].subTarget[j].value + "']");
                                        switch (target[i].subTarget[j].setAs) {
                                            case "disabled":
                                                subElm.attr("disabled", "disabled");
                                                break;
                                            case "enabled":
                                                subElm.removeAttr("disabled");
                                                break;
                                            case "hide":
                                                subElm.parent("div").attr("style", "display:none;");
                                                break;
                                            case "show":
                                                subElm.parent("div").removeAttr("style");
                                                break;
                                        }
                                        break;
                                    case "selectbox":
                                        if (elm.attr("type") === "text-box") { // with icon
                                            subElm = elm.parent(".form-row").find("a[value='" + target[i].subTarget[j].value + "']");
                                            switch (target[i].subTarget[j].setAs) {
                                                case "disable":
                                                    subElm.addClass("disabled");
                                                    break;
                                                case "enable":
                                                    subElm.removeClass("disabled");
                                                    break;
                                                case "hide":
                                                    subElm.parent("div").attr("style", "display:none");
                                                    break;
                                                case "show":
                                                    subElm.parent("div").removeAttr("style", "display:none");
                                                    break;
                                            }
                                        }
                                        else {
                                            switch (target[i].subTarget[j].setAs) {
                                                case "disable":
                                                    elm.find("option[value='" + target[i].subTarget[j].value + "']").attr("disabled", "disabled");
                                                    break;
                                                case "enable":
                                                    elm.find("option[value='" + target[i].subTarget[j].value + "']").removeAttr("disabled");
                                                    break;
                                                case "hide":
                                                    elm.find("option[value='" + target[i].subTarget[j].value + "']").attr("style", "display:none");
                                                    break;
                                                case "show":
                                                    elm.find("option[value='" + target[i].subTarget[j].value + "']").removeAttr("style", "display:none");
                                                    break;
                                            }
                                        }

                                        break;
                                }
                            }
                            else {//with icon
                                subElm = elm.parent(".form-row").find("a[value='" + target[i].subTarget[j].value + "']");
                                switch (target[i].subTarget[j].setAs) {
                                    case "disable":
                                        subElm.addClass("disabled");
                                        break;
                                    case "enable":
                                        subElm.removeClass("disabled");
                                        break;
                                    case "hide":
                                        subElm.parent("div").attr("style", "display:none");
                                        break;
                                    case "show":
                                        subElm.parent("div").removeAttr("style", "display:none");
                                        break;
                                }
                            }

                        }
                        break;
                }
            }

            if (typeof (callback) !== "undefined") {
                return callback();
            }
        };

    }
    return new Event();
};

jQuery(document).ready(function () {

    FormBuilder.Element.Event().init();
    $(function () {
        var onLoadItems = $("[data-action-onload ='onLoad']");
        if (onLoadItems.length > 0) {
            for (i = 0; i < onLoadItems.length; i++) {
                FormBuilder.Element.Event().onLoad($(onLoadItems[i]).attr('name'));
            }
        }
    });
});