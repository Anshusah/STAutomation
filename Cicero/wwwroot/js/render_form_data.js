var forItems;
var selectElements;
var toggleAgainVal;
function setValueOn(id, value) {
    $("#" + id).val(value);
    $("#" + id).trigger('change');
    //check for repeat items
    setTimeout(function () {
        if (forItems != undefined) {
            var parents = forItems.split(",");
            for (i = 0; i < parents.length; i++) {
                1
                if (parents[i] != "undefined") {
                    value = parseInt($("#" + parents[i]).val());
                    if ($("#" + parents[i]).parents("[style^='display']").length == 0) {
                        $("#" + parents[i]).val(value + 1);
                        $("#" + parents[i]).trigger('change');
                        $("#" + parents[i]).val(value);
                        $("#" + parents[i]).trigger('change');
                    }
                }

            }
        }
        setCurrencyValueEvent();
    }, 300);


}
//render form data
function renderData(allValues) {
    $.each(allValues, function (k, v) {
        var elmname = $('[name^="' + k + '"]').attr("name");
        var hasParentTarget = false;
        var elmKey = k;
        var isRepeatItem = false;
        var hasBrackets = false;
        var isTableItem = false;
        var isRepeatRow = false;
        var hasMultipleValue = false;
        var elmCount = 0;
        elmValueCount = 0;
        var number = 0;
        if (elmname != undefined && elmname != "" && v != "Null") {
            if (elmname.indexOf("[]") > -1 || elmname.indexOf("[") > -1) {
                hasBrackets = true;
            }
            if (hasBrackets) {
                if (v.length == undefined) {
                    $.each(v, function (k, l) {
                        // console.log(k, l);
                        elmValueCount = parseInt(k) + 1;
                    });
                    if (elmValueCount >= 1) {
                        hasMultipleValue = true;
                    }

                }
                else {
                    if (v.length != 0) {
                        elmValueCount = 1;
                        hasMultipleValue = true;
                    }

                }

            }
            if (hasMultipleValue) {
                if (!$('[id="' + k + "[]" + '"]').hasClass("media-list")) {

                    elmCount = $('[name="' + elmname + '"]').length;
                    var isTableElm = $('[name="' + elmname + '"]').attr("data-table-elm");
                    var isRepeatElm = $('[name="' + elmname + '"]').attr("data-repeat-elm");
                    if (isTableElm == "true") {//table element
                        elmCount = $('[name^="' + k + '"]').length;
                        isTableItem = true;
                        if (elmValueCount > elmCount) {
                            diff = elmValueCount - elmCount;
                            addTableRow(diff, $("[name='" + elmname + "']"));

                        }
                    }
                    else if (isRepeatElm == "true") // repeat row
                    {
                        elmCount = $('[name^="' + k + '"]').length;
                        isRepeatRow = true;
                        if (elmValueCount > elmCount) {
                            diff = elmValueCount - elmCount;
                            addRepeatRow(diff, $("[name='" + elmname + "']"));

                        }
                    }
                    else {
                        if (elmValueCount == 1)//only 1 repeat item
                        {
                            parentid = $('[name="' + elmname + '"]').parents(".repeatItem").last().attr("id");
                            if (parentid != undefined) {
                                var sd = parentid.replace(/[^0-9]/gi, ''); // Replace everything that is not a number with nothing
                                number = parseInt(sd, 10);
                                var parentName = $("[data-target^='#" + number + "']").attr("name");

                                if (parentName != undefined) {
                                    prevVal = $("#" + parentName).val();
                                    $("#" + parentName).val(elmValueCount);
                                    $("#" + parentName).attr("data-act", elmValueCount);
                                    forItems = forItems + "," + parentName;
                                    hasParentTarget = true;
                                    isRepeatItem = true;

                                }
                            }
                        }
                        //for more than one repeat item
                        else {
                            if (elmCount == 1) {
                                parentid = $('[name="' + elmname + '"]').parents("[style^='display']").attr("id");
                                if (parentid != undefined) {
                                    var sd = parentid.replace(/[^0-9]/gi, ''); // Replace everything that is not a number with nothing
                                    number = parseInt(sd, 10);
                                    var parentName = $("[data-target^='#" + number + "']").attr("name");
                                    if (parentName != undefined) {
                                        prevVal = $("#" + parentName).val();

                                        $("#" + parentName).val(elmValueCount);
                                        $("#" + parentName).attr("data-act", elmValueCount);
                                        forItems = forItems + "," + parentName;
                                        cloneByNumber($("#" + parentName, "true"));


                                        hasParentTarget = true;
                                        isRepeatItem = true;
                                    }

                                }
                            }
                            else {
                                parentid = $('[name="' + elmname + '"]').parents(".repeatItem").last().attr("id");
                                if (parentid != undefined) {
                                    var sd = parentid.replace(/[^0-9]/gi, ''); // Replace everything that is not a number with nothing
                                    number = parseInt(sd, 10);
                                    hasParentTarget = true;
                                    isRepeatItem = true;
                                }
                            }
                        }
                    }

                }
                else {

                    var items = Object.getOwnPropertyNames(v);
                    var itemsValues = [];
                    $.each(items, function (index, value) {
                        itemsValues.push(v[items[value]]);
                    });
                    var url = "/admin/getImageDetails.html?values=" + itemsValues;
                    $.get(url, function (mediaDatas) {
                        for (var i = 0; i < items.length; i++) {
                            var imgSrc = "";
                            var imgTitle = "";
                            var imgValue = v[items[i]];
                            if (mediaDatas !== null && mediaDatas != undefined && mediaDatas.length > 0) {
                                var mData = mediaDatas.filter(x => x.id === parseInt(imgValue));
                                var extension = mData[0].url.substr(mData[0].url.lastIndexOf('.') + 1).toLowerCase();
                                imgSrc = "/uploads/" + mData[0].url;
                                imgTitle = mData[0].title;

                                var li = '';

                                if (extension === "pdf") {
                                    li = '<li class="thumbs fileuploader__item file-type file-type--pdf"><a class="fileuploader-item-inner" target="_blank" href="' + imgSrc + '"><div class="thumbnail-holder"><div class="fileuploader__item-image"><img src="/images/pdf.png" data-imgtitle="' + imgTitle + '"></div></div><div class="actions-holder"><button class="btn fileuploader__action fileuploader__action-remove" type="button" title="Remove"><i class="fileuploader__icon-remove ri-close-circle-fill" aria-hidden="true"></i></button></div></a><input type="hidden" name="images[]" value="' + imgValue + '"></li>';
                                }
                                else if (extension === "doc" || extension === "docx" || extension === "txt") {
                                    li = '<li class="thumbs fileuploader__item file-type file-type--doc"><a class="fileuploader-item-inner" target="_blank" href="' + imgSrc + '"><div class="thumbnail-holder"><div class="fileuploader__item-image"><img src="/images/doc.png" data-imgtitle="' + imgTitle + '"></div></div><div class="actions-holder"><button class="btn fileuploader__action fileuploader__action-remove" type="button" title="Remove"><i class="fileuploader__icon-remove ri-close-circle-fill" aria-hidden="true"></i></button></div></a><input type="hidden" name="images[]" value="' + imgValue + '"></li>';

                                }
                                else {
                                    li = '<li class="thumbs fileuploader__item file-type file-type--image"><a class="pop fileuploader-item-inner" href="javascript:void(0)"><div class="thumbnail-holder"><div class="fileuploader__item-image"><img src="' + imgSrc + '" data-imgtitle="' + imgTitle + '"></div></div><div class="actions-holder"><button class="btn fileuploader__action fileuploader__action-remove" type="button" title="Remove"><i class="fileuploader__icon-remove ri-close-circle-fill" aria-hidden="true"></i></button></div></a><input type="hidden" name="images[]" value="' + imgValue + '"></li>';
                                }


                            }

                            $('[id="' + k + "[]" + '"]').find(".fileuploader__items").append(li);
                            //$('#' + k)


                        }

                        //$('#' + k).attr("id", k + "[]");

                    });
                }
            }



            //If repeatItem

            if (isTableItem) {
                $.each(v, function (k, l) {
                    index = parseInt(k);
                    var elmName = elmKey + "[" + index + "]";

                    var elm = "[name='" + elmName + "']";
                    if ($(elm).attr('multiple') == 'multiple') {
                        $.each(l.split(","), function (i, e) {
                            $(elm + " option[value='" + e + "']").prop("selected", true);
                        });
                        $(elm).attr("data-act", l);
                        $(elm).closest("td").find(".display-data").html(l);
                    }
                    else if ($(elm).attr('type') == 'radio') {
                        $(elm).removeAttr("checked");
                        $(elm + " [value='" + l + "']").prop("checked", true);
                        $(elm).attr("data-act", l);
                        $(elm).closest("td").find(".display-data").html(l);
                    }
                    else if ($(elm).attr('type') == 'checkbox') {
                        var values = l.split(",");
                        for (i = 0; i < values.length; i++) {
                            $(elm + "[value='" + values[i] + "']").prop("checked", true);
                        }
                        $(elm).attr("data-act", l);
                        $(elm).closest("td").find(".display-data").html(l);
                    }
                    else if ($(elm).attr("data-elm-type") != undefined && $(elm).attr("data-elm-type") === "label") {
                        $(elm).find(".label-text").html(l);
                        $(elm).attr("data-act", l);
                        $(elm).closest("td").find(".display-data").html(l);
                    }
                    else if ($(elm).attr("data-elm-type") != undefined && $(elm).attr("data-elm-type") === "heading") {
                        $(elm).html(l);
                        $(elm).attr("data-act", l);
                        $(elm).closest("td").find(".display-data").html(l);
                    }
                    else {
                        $(elm).val(l);
                        $(elm).attr("data-act", l);
                        if ($(elm).attr("data-tagsinput") != undefined) {
                            $(elm).tagsinput("add", v);
                        }
                        $(elm).closest("td").find(".display-data").html(l);
                        if ($(elm).attr('data-value-for') != undefined) {
                            setvalueTo = $(elm).attr('data-value-for');
                            $("[name^='" + setvalueTo + "']").val(l);
                            var t = $("[name^='" + setvalueTo + "']").val();
                            $(elm).closest("td").find(".display-data").html(t);
                        }
                        if ($(elm).attr('data-for-switch') != undefined) {
                            setvalueTo = $(elm).attr("data-for-switch");
                            if (l.toUpperCase() == "TRUE") {
                                $("[name='" + setvalueTo + "']").prop("checked", true);
                            }
                            else {
                                $("[name='" + setvalueTo + "']").prop("checked", false);
                            }
                            $("[name='" + setvalueTo + "']").trigger("change");
                        }
                    }

                });
            }
            else if (isRepeatRow) {
                $.each(v, function (k, l) {
                    index = parseInt(k);
                    var elmName = elmKey + "[" + index + "]";

                    var elm = "[name='" + elmName + "']";
                    if ($(elm).attr('multiple') == 'multiple') {
                        $.each(l.split(","), function (i, e) {
                            $(elm + " option[value='" + e + "']").prop("selected", true);
                        });
                        $(elm).attr("data-act", l);

                    }
                    else if ($(elm).attr('type') == 'radio') {
                        $(elm).removeAttr("checked");
                        $(elm + " [value='" + l + "']").prop("checked", true);
                        $(elm).attr("data-act", l);

                    }
                    else if ($(elm).attr('type') == 'checkbox') {
                        var values = l.split(",");
                        for (i = 0; i < values.length; i++) {
                            $(elm + "[value='" + values[i] + "']").prop("checked", true);
                        }
                        $(elm).attr("data-act", l);

                    }
                    else if ($(elm).attr("data-elm-type") != undefined && $(elm).attr("data-elm-type") === "label") {
                        $(elm).find(".label-text").html(l);
                        $(elm).attr("data-act", l);

                    }
                    else if ($(elm).attr("data-elm-type") != undefined && $(elm).attr("data-elm-type") === "heading") {
                        $(elm).html(l);
                        $(elm).attr("data-act", l);

                    }
                    else if ($(elm).hasClass("media-list") == true) {
                        var itemsValues = [];
                        if (l != "") {
                            itemsValues = l.split(",");
                            var url = "/admin/getImageDetails.html?values=" + itemsValues;
                            $.get(url, function (mediaDatas) {
                                console.log("values");
                                for (var i = 0; i < itemsValues.length; i++) {
                                    var imgSrc = "";
                                    var imgTitle = "";
                                    var imgValue = itemsValues[i];
                                    if (mediaDatas !== null && mediaDatas != undefined && mediaDatas.length > 0) {
                                        var mData = mediaDatas.filter(x => x.id === parseInt(imgValue));
                                        var extension = mData[0].url.substr(mData[0].url.lastIndexOf('.') + 1).toLowerCase();
                                        imgSrc = "/uploads/" + mData[0].url;
                                        imgTitle = mData[0].title;
                                        var li = '';
                                        if (extension === "pdf") {
                                            li = '<li class="thumbs fileuploader__item file-type file-type--pdf"><a class="fileuploader-item-inner" target="_blank" href="' + imgSrc + '"><div class="thumbnail-holder"><div class="fileuploader__item-image"><img src="/images/pdf.png" data-imgtitle="' + imgTitle + '"></div></div><div class="actions-holder"><button class="btn fileuploader__action fileuploader__action-remove" type="button" title="Remove"><i class="fileuploader__icon-remove ri-close-circle-fill" aria-hidden="true"></i></button></div></a><input type="hidden" name="images[]" value="' + imgValue + '"></li>';
                                        }
                                        else if (extension === "doc" || extension === "docx" || extension === "txt") {
                                            li = '<li class="thumbs fileuploader__item file-type file-type--doc"><a class="fileuploader-item-inner" target="_blank" href="' + imgSrc + '"><div class="thumbnail-holder"><div class="fileuploader__item-image"><img src="/images/doc.png" data-imgtitle="' + imgTitle + '"></div></div><div class="actions-holder"><button class="btn fileuploader__action fileuploader__action-remove" type="button" title="Remove"><i class="fileuploader__icon-remove ri-close-circle-fill" aria-hidden="true"></i></button></div></a><input type="hidden" name="images[]" value="' + imgValue + '"></li>';

                                        }
                                        else {
                                            li = '<li class="thumbs fileuploader__item file-type file-type--image"><a class="pop fileuploader-item-inner" href="javascript:void(0)"><div class="thumbnail-holder"><div class="fileuploader__item-image"><img src="' + imgSrc + '" data-imgtitle="' + imgTitle + '"></div></div><div class="actions-holder"><button class="btn fileuploader__action fileuploader__action-remove" type="button" title="Remove"><i class="fileuploader__icon-remove ri-close-circle-fill" aria-hidden="true"></i></button></div></a><input type="hidden" name="images[]" value="' + imgValue + '"></li>';
                                        }
                                    }
                                    $(elm).find(".fileuploader__items").append(li);
                                }
                            })
                        }
                    }
                    else {
                        $(elm).val(l);
                        $(elm).attr("data-act", l);
                        if ($(elm).attr("data-tagsinput") != undefined) {
                            $(elm).tagsinput("add", v);
                        }
                        if ($(elm).attr('data-value-for') != undefined) {
                            setvalueTo = $(elm).attr('data-value-for');
                            $("[name^='" + setvalueTo + "']").val(l);
                            var t = $("[name^='" + setvalueTo + "']").val();

                        }
                        if ($(elm).attr('data-for-switch') != undefined) {
                            setvalueTo = $(elm).attr("data-for-switch");
                            if (l.toUpperCase() == "TRUE") {
                                $("[name='" + setvalueTo + "']").prop("checked", true);
                            }
                            else {
                                $("[name='" + setvalueTo + "']").prop("checked", false);
                            }
                            $("[name='" + setvalueTo + "']").trigger("change");
                        }
                    }

                });
            }
            else if (hasParentTarget && isRepeatItem) {
                var i = 0;
                if (v.length == undefined) {
                    $.each(v, function (k, l) {
                        i = parseInt(k);
                        var elm = "[name='" + number + "_" + (i + 1) + "'] [name='" + elmname + "']";
                        if ($(elm).attr('multiple') == 'multiple') {
                            $.each(l.split(","), function (i, e) {
                                $(elm + " option[value='" + e + "']").prop("selected", true);
                            });
                            $(elm).attr("data-act", l);
                        }
                        else if ($(elm).attr('type') == 'radio') {
                            $(elm).removeAttr("checked");
                            $(elm + " [value='" + l + "']").prop("checked", true);
                            $(elm).attr("data-act", l);
                        }
                        else if ($(elm).attr('type') == 'checkbox') {
                            var values = l.split(",");
                            for (i = 0; i < values.length; i++) {
                                $(elm + ' [value="' + values[i] + '"]').prop("checked", true);
                            }
                            $(elm).attr("data-act", l);
                        }
                        else if ($(elm).attr("data-elm-type") != undefined && $(elm).attr("data-elm-type") === "label") {
                            $(elm).find(".label-text").html(l);
                            $(elm).attr("data-act", l);
                        }
                        else if ($(elm).attr("data-elm-type") != undefined && $(elm).attr("data-elm-type") === "heading") {
                            $(elm).html(l);
                            $(elm).attr("data-act", l);
                        }
                        else {
                            $(elm).val(l);
                            $(elm).attr("data-act", l);
                            if ($(elm).attr("data-tagsinput") != undefined) {
                                $(elm).tagsinput("add", v);
                            }
                            if ($(elm).attr('data-value-for') != undefined) {
                                setvalueTo = $(elm).attr('data-value-for');
                                $("[name = '" + number + "_" + (i + 1) + "'] [name^='" + setvalueTo + "']").val(l);
                            }
                            if ($(elm).attr('data-for-switch') != undefined) {
                                setvalueTo = $(elm).attr("data-for-switch");
                                if (l.toUpperCase() == "TRUE") {
                                    $("[name='" + setvalueTo + "']").prop("checked", true);
                                }
                                else {
                                    $("[name='" + setvalueTo + "']").prop("checked", false);
                                }
                                $("[name='" + setvalueTo + "']").trigger("change");
                            }
                        }

                    });
                }
                else {
                    l = v;
                    var elm = "[name='" + number + "_" + (i + 1) + "'] [name='" + elmname + "']";
                    if ($(elm).attr('multiple') == 'multiple') {
                        $.each(l.split(","), function (i, e) {
                            $(elm + " option[value='" + e + "']").prop("selected", true);
                        });
                    }
                    else if ($(elm).attr('type') == 'radio') {
                        $(elm).removeAttr("checked");
                        $(elm + " [value='" + l + "']").prop("checked", true);
                    }
                    else if ($(elm).attr('type') == 'checkbox') {
                        var values = l.split(",");
                        for (i = 0; i < values.length; i++) {
                            $(elm + ' [value="' + values[i] + '"]').prop("checked", true);
                        }
                    }
                    else if ($(elm).attr("data-elm-type") != undefined && $(elm).attr("data-elm-type") === "label") {
                        $(elm).find(".label-text").html(l);
                        $(elm).attr("data-act", l);
                    }
                    else if ($(elm).attr("data-elm-type") != undefined && $(elm).attr("data-elm-type") === "heading") {
                        $(elm).html(l);
                        $(elm).attr("data-act", l);
                    }
                    else {
                        $(elm).val(l);
                        $(elm).attr("data-act", l);
                        if ($(elm).attr("data-tagsinput") != undefined) {
                            $(elm).tagsinput("add", v);
                        }
                        if ($(elm).attr('data-value-for') != undefined) {
                            setvalueTo = $(elm).attr('data-value-for');
                            $("[name = '" + number + "_" + (i + 1) + "'] [name^='" + setvalueTo + "']").val(l);
                        }
                        if ($(elm).attr('data-for-switch') != undefined) {
                            setvalueTo = $(elm).attr("data-for-switch");
                            if (l.toUpperCase() == "TRUE") {
                                $("[name='" + setvalueTo + "']").prop("checked", true);
                            }
                            else {
                                $("[name='" + setvalueTo + "']").prop("checked", false);
                            }
                            $("[name='" + setvalueTo + "']").trigger("change");
                        }
                    }

                }

                //check for repeat items
                setTimeout(function () {
                    if (forItems != undefined) {
                        var parents = forItems.split(",");
                        for (i = 0; i < parents.length; i++) {
                            if (parents[i] != "undefined") {

                                value = parseInt($("#" + parents[i]).val());
                                if ($("#" + parents[i]).parents("[style^='display']").length == 0) {
                                    $("#" + parents[i]).val(value + 1);
                                    $("#" + parents[i]).trigger('change');
                                    $("#" + parents[i]).val(value);
                                    $("#" + parents[i]).trigger('change');
                                }
                            }

                        }
                    }
                }, 300);
            }
            else {
                if ($('[name="' + k + '"]').attr('multiple') == 'multiple') {
                    $.each(v.split(","), function (i, e) {
                        $("[name='" + k + "'] option[value='" + e + "']").prop("selected", true);
                    });
                }
                else if ($('[name="' + k + '"]').attr('type') == 'radio') {
                    $("[name='" + k + "']").removeAttr("checked");
                    $("[name='" + k + "'][value='" + v + "']").prop("checked", true);
                }
                else if ($('[name="' + k + '"]').attr('type') == 'checkbox') {
                    var values = v.split(",");
                    for (i = 0; i < values.length; i++) {
                        $('[name="' + k + '"][value="' + values[i] + '"]').prop("checked", true);
                    }
                }
                else if ($('[name="' + k + '"]').attr("data-elm-type") != undefined && $('[name="' + k + '"]').attr("data-elm-type") === "label") {
                    $('[name="' + k + '"]').find(".label-text").html(v);
                    $('[name="' + k + '"]').attr("data-act", v);
                }
                else if ($('[name="' + k + '"]').attr("data-elm-type") != undefined && $('[name="' + k + '"]').attr("data-elm-type") === "heading") {
                    $('[name="' + k + '"]').html(v);
                    $('[name="' + k + '"]').attr("data-act", v);
                }
                else {
                    $('[name="' + k + '"]').val(v);
                    $('[name="' + k + '"]').attr("data-act", v);
                    if ($('[name="' + k + '"]').attr("data-tagsinput") != undefined) {

                        $('[name="' + k + '"]').tagsinput("add", v);
                    }
                    if ($('[name="' + k + '"]').attr('data-for-switch') != undefined) {
                        setvalueTo = $('[name="' + k + '"]').attr("data-for-switch");
                        if (v.toUpperCase() == "TRUE") {
                            $("[name='" + setvalueTo + "']").prop("checked", true);
                        }
                        else {
                            $("[name='" + setvalueTo + "']").prop("checked", false);
                        }
                        $("[name='" + setvalueTo + "']").trigger("change");
                    }

                    if ($('[name="' + k + '"]').attr('data-value-for') != undefined) {
                        setvalueTo = $('[name="' + k + '"]').attr('data-value-for');
                        $("[name='" + setvalueTo + "']").val(v);
                    }
                    if ($('[name="' + k + '"]').attr('data-elm-select') == 'elmSelect') {
                        v = v.trim();

                        if (v != "") {
                            $('[name="' + k + '"]').parents(".form-row").find('a[value="' + v + '"]').trigger('click');
                            $('[name="' + k + '"]').parents(".form-row").find('a').removeClass("show Image-target").addClass("disabled");
                            $('[name="' + k + '"]').parents(".form-row").find('a[value="' + v + '"]').addClass("show Image-target").removeClass("disabled");
                            var toggle = $('[name="' + k + '"]').parents(".form-row").find('a[value="' + v + '"]').attr("toggle-options");
                            if (toggle == "true") {
                                selectElements = selectElements + "," + k;
                            }
                            else {
                                toggleAgainVal = toggleAgainVal + "," + k + "-" + v;
                                // $('[name="' + k + '"]').parents(".form-row").find('a').removeAttr("onclick");
                            }
                            var ids = $('[name="' + k + '"]').parents(".form-row").find('a[value="' + v + '"]').attr("data-target").split(",");
                            var bools = $('[name="' + k + '"]').parents(".form-row").find('a[value="' + v + '"]').attr("data-target-action").split(",");
                            if (ids.length > 0 && bools.length > 0) {
                                for (i = 0; i < ids.length; i++) {
                                    if (bools[i] == "False") {
                                        $(ids[i]).attr("style", "display:none");
                                    } else {
                                        $(ids[i]).removeAttr("style");
                                    }
                                    //if (bools[i] == "False") {
                                    //    $("#" + ids[i]).attr("style", "display:none");
                                    //} else {
                                    //    $("#" + ids[i]).removeAttr("style");
                                    //}

                                }
                            }
                        }

                    }
                }
            }

        }
    });
    setCurrencyValueEvent();
    toggleSelectOptions();
    setTimeout(function () { toggleAgain(); }, 1000);
    if ($('select.selectCountry').val() !== "") {
        $(".btn-getstarted").click();
    }
}

function setSwitchValue() {
    $("[name^='check']").unbind();
    $("[name^='check']").on("change", function () {
        id = $(this).attr("data-for-check");
        if ($(this).prop("checked") == true) {
            $("[name='" + id + "']").val("True");
        }
        else {
            $("[name='" + id + "']").val("False");
        }
    });
}



function toggleSelectOptions() {
    if (selectElements != undefined) {
        toggleSelect = selectElements.split(",");
        $.each(toggleSelect, function (k, v) {
            if (k != "") {
                setValue = "";
                var temp = $('[name="' + v + '"]').parents(".form-row").find('a');
                var optCount = [];
                $.each(temp, function (k1, v1) {
                    fv = $(v1).attr("data-target-action").split(",");
                    c = 0;
                    $.each(fv, function (k2, v2) {
                        if (v2 == "True") {
                            c = c + 1;
                        }
                    });
                    value = $(v1).attr("value");
                    optCount.push(value + "," + c); // get all selectoptions
                });
                optCount.sort(function (a, b) { //sort each options true count descending
                    if (a.split(",")[1] > b.split(",")[1]) {
                        return -1;
                    }
                    if (b.split(",")[1] > a.split(",")[1]) {
                        return 1;
                    }
                    return 0;
                });
                // console.log(optCount);
                $.each(optCount, function (k3, v3) {
                    opt = v3.split(",")[0];
                    mc = 0;
                    if (opt != "") {

                        var ids = $('[name="' + v + '"]').parents(".form-row").find('a[value="' + opt + '"]').attr("data-target").split(",");
                        var bools = $('[name="' + v + '"]').parents(".form-row").find('a[value="' + opt + '"]').attr("data-target-action").split(",");
                        if (ids.length > 0 && bools.length > 0) {
                            for (i = 0; i < ids.length; i++) {
                                ic = 0;
                                if (bools[i] == "True") {
                                    fe = $(ids[i]).find("input, select, textarea, checkbox");
                                    $.each(fe, function (k4, v4) {
                                        if ($(v4).attr("data-act") != "" && $(v4).attr("data-act") != undefined) {
                                            ic = ic + 1;
                                        }

                                        //donot remove yet
                                        //type = $(v4).prop('tagName').toLowerCase();
                                        //if (type == 'input') {
                                        //    type = $(v4).attr("type");
                                        //}
                                        //switch (type) {
                                        //    case "radio":
                                        //        if ($(v4).attr("data-act") != "" && $(v4).attr("data-act") != undefined) {
                                        //            ic = ic + 1;
                                        //        }
                                        //        break;
                                        //    case "checkbox":
                                        //        if ($(v4).attr("data-act") != "" && $(v4).attr("data-act") != undefined) {
                                        //            ic = ic + 1;
                                        //        }
                                        //        break;
                                        //    default:
                                        //        if ($(v4).attr("data-act") != "" && $(v4).attr("data-act") != undefined) {
                                        //            ic = ic + 1;
                                        //        }
                                        //        break;
                                        //}
                                    });
                                    if (ic > 0) {
                                        mc = mc + 1;
                                    }
                                }


                            }
                        }
                    }
                    if (mc == parseInt(v3.split(",")[1])) {
                        setValue = opt;
                        return false;
                    }

                });
                //set selected target
                if (setValue != "") {
                    $('[name="' + v + '"]').parents(".form-row").find('a[value="' + setValue + '"]').trigger('click');
                    $('[name="' + v + '"]').parents(".form-row").find('a').removeClass("show Image-target").addClass("disabled");
                    $('[name="' + v + '"]').parents(".form-row").find('a[value="' + setValue + '"]').addClass("show Image-target").removeClass("disabled");
                    $('[name="' + v + '"]').parents(".form-row").find('a').removeAttr("onclick");
                    var id = $('[name="' + v + '"]').parents(".form-row").find('a[value="' + setValue + '"]').attr("data-target").split(",");
                    var bool = $('[name="' + v + '"]').parents(".form-row").find('a[value="' + setValue + '"]').attr("data-target-action").split(",");
                    if (id.length > 0 && bool.length > 0) {
                        for (i = 0; i < id.length; i++) {
                            if (bool[i] == "False") {
                                $(id[i]).attr("style", "display:none");
                            } else {
                                $(id[i]).removeAttr("style");
                            }
                            //if (bools[i] == "False") {
                            //    $("#" + ids[i]).attr("style", "display:none");
                            //} else {
                            //    $("#" + ids[i]).removeAttr("style");
                            //}

                        }
                    }
                }
            }
        });
    }

}
function toggleAgain() {
    //debugger;
    if (toggleAgainVal != undefined) {
        toggleA = toggleAgainVal.split(",");
        $.each(toggleA, function (k, v) {
            name = v.split("-")[0];
            value = v.split("-")[1];
            $('[name="' + name + '"]').parents(".form-row").find('a[value="' + value + '"]').trigger('click');
            $('[name="' + name + '"]').parents(".form-row").find('a').removeAttr("onclick");
        })
    }
}

function setCurrencyValueEvent() {
    $("[data-elm-type='currency']").unbind();
    $("[data-elm-type='currency']").inputmask();
    $("[data-elm-type='currency']").on("keyup", delay(function (e) {
        name = $(this).attr("name").split("_")[1];
        value = parseFloat($(this).val().replace(/[^-0-9\.]+/g, ""));
        $(this).closest(".currencyType").find("[name^='elm" + name + "']").val(value);
        $(this).closest(".form-group").find("[name^='elm" + name + "']").val(value);
        if ($(this).closest(".form-group").find("[name^='elm" + name + "']").attr("type") === "range") {
            rangeVar = $(this).closest(".form-group").find("[name^='elm" + name + "']");
            if (value > parseFloat(rangeVar.attr("max"))) {  //if value greater than range max set range value
                $(this).val(parseFloat(rangeVar.attr("max")));
            }
        }
    }, 100));
    currencyValidateInit();
}

function delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}


$(function () {
    setCurrencyValueEvent();
    setSwitchValue();

})
// Example usage:

//$('#input').keyup(delay(function (e) {
//    console.log('Time elapsed!', this.value);
//}, 500));