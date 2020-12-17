var Cicero = {
    SelectedImages: [],
    DataTableSetting: function () {
        return {
            "processing": true,
            "serverSide": true,
            "dom": 'r<"clear">t<"row"<"col-sm-3"l><"col-sm-3"i><"col-sm-6"p>><"clear">',
            'searching': true,
            "ajax": { "url": null, "type": "POST" },
            'stateSave': false,
            'select': {
                info: false
            },
            'columnDefs': [
                {
                    'targets': 0,
                    'checkboxes': {
                        'selectRow': true,
                        'selectAll': true
                    }
                }
            ],
            'select': {
                'style': 'multi'
            },
            "rowCallback": function (row, data, index) {
                jQuery('td:first-child', row).find("input").attr({ "id": "Ids", "name": "Ids", "value": data.id });
            },
            "columns": [],
            order: [
                [1, "asc"]
            ],
            "paging": true,
            "pageLength": 10,
            "lengthMenu": [[5, 10, 20, 50, 100, 500], [5, 10, 20, 50, 100, 500]]
        }
    },
    DataTableLength: [[5, 10, 20, 50, 100, 500], [5, 10, 20, 50, 100, 500]],
    DataTablePageLength: 10,
    ConfirmString: function (e) {
        return '({button:{cancel:"Close",confirm:"Confirm"},size:"", title:"Confirm",content:"Are you sure?",open:function(){$("#confirm").modal("show");},close:function(){$("#confirm").modal("hide");},' + e + '})';
    },
    MediaString: function (e) {
        return '({button:{cancel:"Close",insert:"Insert"},size:"modal-lg", title:"Media Manager",open:function(){$("#media").modal("show");},close:function(){$("#media").modal("hide");},' + e + '})';
    },
    OnInit: function () {
        var self = this;
        jQuery(document).on("click", "[data-confirm]", function () {
            var str = jQuery(this).data("confirm");

            str = str.replace(/^\{|\}$/g, '');

            var objs = eval(self.ConfirmString(str));
            objs.OnInit();
            jQuery("#confirm").find(".modal-title").html(objs.title);
            jQuery("#confirm").find(".modal-body").html(objs.content);
            jQuery("#confirm").find(".btn-secondary").html(objs.button.cancel);
            jQuery("#confirm").find(".btn-primary").html(objs.button.confirm);
            jQuery("#confirm").find(".modal-dialog").removeClass("modal-lg modal-md modal-sm").addClass(objs.size);

            jQuery("#confirm .btn-secondary").on("click", function () {
                objs.close();
                objs.OnCancelled();
                jQuery(this).off("click")
            });
            jQuery("#confirm .btn-primary").on("click", function () {
                objs.close();
                objs.OnConfirm();
                jQuery(this).off("click")
            });

        });
        jQuery(document).on("click", "[data-media]", function () {
            var str = jQuery(this).data("media");

            str = str.replace(/^\{|\}$/g, '');

            var objs = eval(self.MediaString(str));
            objs.OnInit();
            jQuery("#media").find(".modal-title").html(objs.title);
            jQuery("#media").find(".modal-body").html("<div class='row'><iframe frameborder='0' src='/admin/media/pick.html?time=" + new Date().getTime() + "' frameborder='0'></iframe></div>");
            jQuery("#media").find(".btn-secondary").html(objs.button.cancel);
            jQuery("#media").find(".btn-primary").html(objs.button.insert);
            jQuery("#media").find(".modal-dialog").removeClass("modal-lg modal-md modal-sm").addClass(objs.size);

            jQuery("#media .btn-secondary").on("click", function () {
                objs.close();
                objs.OnCancelled();
                jQuery(this).off("click")
            });
            jQuery("#media .btn-primary").on("click", function () {
                objs.close();
                objs.OnInsert(Cicero.SelectedImages);
                jQuery(this).off("click")
            });

        });
    },
    Case: {
        Setting: { FilterUrl: null },
        OnInit: function (e) {

        },
        DeleteById: function (e) {
            $.getJSON("/user/case/delete-by-id.html", { id: e }, function (types) {
                if (types) {
                    jQuery(".filter-" + e).remove();
                }
                else {
                    toastr.warning("Something went wrong! Please try again later.");
                };
            });
        },
        InsertImages: function (e) {
            $.each(e, function (i, v) {
                var extension = v.url.substr((v.url.lastIndexOf('.') + 1));
                if (extension == "pdf") {
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/images/pdf-icon.png'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
                else if (extension == "doc" || extension == "docx" || extension == "txt") {
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/images/doc-icon.png'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
                else {
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/uploads/" + v.url + "'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
            });
        }
    },
    Article: {
        Setting: { FilterUrl: null },
        OnInit: function (e) {
            jQuery(document).on("click", ".thumbs a i", function () {
                jQuery(this).closest('li').remove();
            })
        },
        InitDataTable: function (e) {
            Cicero.Article.Setting.FilterUrl = e.FilterUrl;
            var TableSetting = Cicero.DataTableSetting();
            TableSetting.ajax.url = Cicero.Article.Setting.FilterUrl;
            TableSetting.columns = [
                { "title": "", "data": "id", "orderable": false },
                { "title": "Title", "data": "title", "orderable": true },
                { "title": "Status", "data": "status", "orderable": true },
                { "title": "Created At", "data": "created_at", "orderable": false },
                { "title": "Updated At", "data": "updated_at", "orderable": false },
                { "title": "Version", "data": "version", "orderable": false },
                { "title": "", "data": "action", "orderable": false, "class": "action", }
            ];
            var table = jQuery("#articles");
            table.dataTable(TableSetting);
            jQuery(document).on("keyup", "#gridsearch", function () {
                table.fnFilter(this.value);
            });

        }
    },
    Media: {
        Delete: function (e) {
            var obj = jQuery(e).closest('a').data('json');
            $.getJSON(CiceroVars.base_url + "admin/media/delete-by-id.html", { selected_image_id: obj.id }, function (types) {
                if (types.status == "true") {
                    jQuery(e).closest('li').remove();
                }
            });
        },
        InsertImages: function (e) {
            $.each(e, function (i, v) {
                var extension = v.url.substr((v.url.lastIndexOf('.') + 1));
                if (extension == "pdf") {
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/images/pdf-icon.png'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
                else if (extension == "doc" || extension == "docx" || extension == "txt") {
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/images/doc-icon.png'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
                else {
                    console.log(extension);
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/uploads/" + v.url + "'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
            });
        }
    },
    User: {
        InsertImages: function (e) {
            jQuery(".thumbs").not(".add").remove();
            $.each(e, function (i, v) {
                var extension = v.url.substr((v.url.lastIndexOf('.') + 1));
                if (extension == "pdf") {
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/images/pdf-icon.png'><i class='fas' aria-hidden='true'><img src='/frontend/img/delete.png' /></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
                else if (extension == "doc" || extension == "docx" || extension == "txt") {
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/images/doc-icon.png'><i class='fas' aria-hidden='true'><img src='/frontend/img/delete.png' /></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
                else {
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/uploads/" + v.url + "'><i class='fas' aria-hidden='true'><img src='/frontend/img/delete.png' /></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
                return false;
            });

        },
        Contact: {
            OnInit: function (e) {
                jQuery("#contact-form").on("submit", function (e) {
                    e.preventDefault();
                    $.ajax({
                        type: "POST",
                        url: 'home/send-email.html',
                        async: true,
                        data: {
                            'name': $('#name').val(),
                            'email': $('#email').val(),
                            'phone': $('#phone').val(),
                            'message': $('#message').val()
                        },
                        success: function (succ) {
                            if (succ.status === "success") {
                                toastr.success(html.message);
                                //$('.status').text(succ.message);
                                //$('.status').removeClass('alert alert-danger').addClass('alert alert-success');
                            }

                            else {
                                toastr.error(html.message);
                                //$('.status').text(succ.message);
                                //$('.status').removeClass('alert alert-success').addClass('alert alert-danger');
                            }
                        }
                    });
                });
            }
        }
    },
    Message: {
        InsertImages: function (e) {
            //jQuery(".thumbs").not(".add").remove();
            $.each(e, function (i, v) {
                var extension = v.url.substr((v.url.lastIndexOf('.') + 1));
                jQuery(".uploaded-files").append("<span class='msg-attachment-file px-2 py-1'><i class='ri-file-pdf-line'></i> <input type='hidden' name='images[]' value='" + v.id + "' />" + v.title + "<button type='button' class='close ml-1' data-dismiss='msg-attachment-file' aria-label='Close' title='Remove attachment'><span aria-hidden='true'>&times;</span></button></span>");
            });
        }
    },
    FB: {
        fields: [],
        fields_collection: "",
        database_values: [],
        filters: [],
        selector: "",
        render: function (e, selector) {
            Cicero.FB.selector = selector;
            Cicero.FB.fields_collection = "";
            var str = "";
            jQuery.each(e.element, function (i, v) {

                str = Cicero.FB.applyFilter(v.type, v);
                //console.log(v.type, v);
                Cicero.FB.fields_collection = Cicero.FB.fields_collection + str;
            });
            jQuery(Cicero.FB.selector).append("<div class='tab-pane fade dynamic' id='" + e.tabName.replace(/[^A-Z0-9]/ig, "-") + "' role='tabpanel' aria-labelledby='" + e.tabName.replace(/[^A-Z0-9]/ig, "-") + "-tab'><h5 class='mb-4 font-weight-bold'>" + e.tabName.replace("_", " ") + "</h5>" + Cicero.FB.fields_collection + "</div>");
            //jQuery(Cicero.FB.selector).append("<div class='tab-pane fade p-3 py-5 dynamic' id='" + e.tabName.replace(/[^A-Z0-9]/ig, "-") + "' role='tabpanel' aria-labelledby='profile-tab'><div class='form-row'>" + Cicero.FB.fields_collection + "</div></div>");
        },
        addFilter: function (type, method) {
            Cicero.FB.filters.push({ type: type, method });
        },
        applyFilter: function (type, arg) {
            var strlen = "";

            jQuery.each(Cicero.FB.filters, function (i, v) {
                if (v.type == type) {
                    var str_data = v.method.call(this, arg);
                    if (typeof (str_data) == 'undefined') {
                        console.log("This method is returning empty : \n" + v.method);
                        str_data = "";
                    }
                    strlen = strlen + str_data;
                }
            });
            return strlen;
        }
    }
}

jQuery(document).ready(function () {
    Cicero.OnInit();
    Cicero.Form.OnInit();
    Cicero.Article.OnInit();
});
function selected(e) {
    Cicero.SelectedImages = e;
}

Cicero.FB.addFilter('text', function (e) {
    if (e.subtype == "email") {
        if (e.value == null) {
            if (e.required == true) {
                return "<div class ='form-group'><label>" + e.label + "</label><input type='email' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' placeolder='" + e.placeholder + "' required/><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
            }
            else {
                return "<div class ='form-group'><label>" + e.label + "</label><input type='email' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' placeolder='" + e.placeholder + "' /></div>";

            }
        }
        else {
            if (e.required == true) {
                return "<div class ='form-group'><label>" + e.label + "</label><input type='email' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' placeolder='" + e.placeholder + "' required/><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
            }
            else {
                return "<div class ='form-group'><label>" + e.label + "</label><input type='email' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' placeolder='" + e.placeholder + "' /></div>";
            }
        }
    }
    else if (e.subtype == "tel") {
                if (e.value == null) {
                    if (e.required == true) {
                        return "<div class ='form-group'><label>" + e.label + "</label><input type='tel' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' placeolder='" + e.placeholder + "' required/><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
                    }
                    else {
                        return "<div class ='form-group'><label>" + e.label + "</label><input type='tel' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' placeolder='" + e.placeholder + "' /></div>";
                    }
                }
        else {
                if (e.required == true) {
                    return "<div class ='form-group'><label>" + e.label + "</label><input type='tel' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' placeolder='" + e.placeholder + "' required/><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
                }
                else {
                    return "<div class ='form-group'><label>" + e.label + "</label><input type='tel' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' placeolder='" + e.placeholder + "' /></div>";
                }
        }
    }
    else {
            if (e.value == null) {
                if (e.required == true) {
                    return "<div class ='form-group'><label>" + e.label + "</label><input type='text' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' placeolder='" + e.placeholder + "'required/><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
                }
                else {
                    return "<div class ='form-group'><label>" + e.label + "</label><input type='text' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' placeolder='" + e.placeholder + "' /></div>";
                }
            }
            else {
                if (e.required == true) {
                    return "<div class ='form-group'><label>" + e.label + "</label><input type='text' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' placeolder='" + e.placeholder + "' required/><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
                }
                else {
                    return "<div class ='form-group'><label>" + e.label + "</label><input type='text' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' placeolder='" + e.placeholder + "' /></div>";
                }
            }
    }
});
Cicero.FB.addFilter('date', function (e) {
    if (e.value == null) {
        if (e.required == true) {
            return "<div class ='form-group'><label>" + e.label + "</label><input type='text' class='form-control length-lg datepicker' name='" + e.name + "'  id='" + e.name + "-preview' placeolder='" + e.placeholder + "' required/><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
        }
        else {
            return "<div class ='form-group'><label>" + e.label + "</label><input type='text' class='form-control length-lg datepicker' name='" + e.name + "'  id='" + e.name + "-preview' placeolder='" + e.placeholder + "' /></div>";
        }
    }
    else {
        if (e.required == true) {
            return "<div class ='form-group'><label>" + e.label + "</label><input type='text' class='form-control length-lg datepicker' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' placeolder='" + e.placeholder + "' required/><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
        }
        else {
            return "<div class ='form-group'><label>" + e.label + "</label><input type='text' class='form-control length-lg datepicker' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' placeolder='" + e.placeholder + "' /></div>";
        }
    }
});
Cicero.FB.addFilter('number', function (e) {
    if (e.value == null) {
        if (e.required == true) {
            return "<div class ='form-group'><label>" + e.label + "</label><input type='number' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' placeolder='" + e.placeholder + "' required/><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
        }
        else {
            return "<div class ='form-group'><label>" + e.label + "</label><input type='number' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' placeolder='" + e.placeholder + "' /></div>";
        }
    }
    else {
        if (e.required == true) {
            return "<div class ='form-group'><label>" + e.label + "</label><input type='number' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' placeolder='" + e.placeholder + "' required /><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
        }
        else {
            return "<div class ='form-group'><label>" + e.label + "</label><input type='number' class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' placeolder='" + e.placeholder + "' /></div>";
        }
    }
});
Cicero.FB.addFilter('radio-group', function (e) {
    var radioText = "<div class ='form-group'><label>" + e.label + "</label><div>";
    $.each(e.values, function (y, x) {
        if (x.selected == true) {
            if (e.values[0].value == x.value && e.required == true) {
                radioText = radioText + "<div class='form-check form-check-inline'><input type='radio' class='form-check-input' name='" + e.name + "' checked  id='" + e.name + "-preview' value='" + x.value + "' placeolder='" + e.placeholder + "' data-json='" + JSON.stringify(e.values) + "' data-select required/><label class='form-check-label'>" + x.label + "</label></div>";
            }
            else {
                radioText = radioText + "<div class='form-check form-check-inline'><input type='radio' class='form-check-input' name='" + e.name + "' checked  id='" + e.name + "-preview' value='" + x.value + "' placeolder='" + e.placeholder + "' data-json='" + JSON.stringify(e.values) + "' data-select/><label class='form-check-label'>" + x.label + "</label></div>";
            }
        }
        else {
            if (e.values[0].value == x.value && e.required == true) {
                radioText = radioText + "<div class='form-check form-check-inline'><input type='radio' class='form-check-input' name='" + e.name + "'  id='" + e.name + "-preview' value='" + x.value + "' placeolder='" + e.placeholder + "' data-json='" + JSON.stringify(e.values) + "' data-select required/><label class='form-check-label'>" + x.label + "</label></div>";
            }
            else {
                radioText = radioText + "<div class='form-check form-check-inline'><input type='radio' class='form-check-input' name='" + e.name + "'  id='" + e.name + "-preview' value='" + x.value + "' placeolder='" + e.placeholder + "' data-json='" + JSON.stringify(e.values) + "' data-select/><label class='form-check-label'>" + x.label + "</label></div>";
            }
        }
    });
    radioText = radioText + "<span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div></div>";
    return radioText;
});
Cicero.FB.addFilter('checkbox-group', function (e) {
    var checkboxText = "<div class ='form-group'><label>" + e.label + "</label><div>";
    $.each(e.values, function (y, x) {
        if (x.selected == true) {
            if (e.required == true) {
                checkboxText = checkboxText + "<div class='form-check form-check-inline'><input type='checkbox' class='form-check-input' name='" + e.name + "' checked  id='" + e.name + "-preview' value='" + x.value + "' placeolder='" + e.placeholder + "' data-json='" + JSON.stringify(e.values) + "' data-select required/><label class='form-check-label'>" + x.label + "</label></div>";
            }
            else {
                checkboxText = checkboxText + "<div class='form-check form-check-inline'><input type='checkbox' class='form-check-input' name='" + e.name + "' checked  id='" + e.name + "-preview' value='" + x.value + "' placeolder='" + e.placeholder + "' data-json='" + JSON.stringify(e.values) + "' data-select /><label class='form-check-label'>" + x.label + "</label></div>";
            }
        }
        else {
            if (e.required == true) {
                checkboxText = checkboxText + "<div class='form-check form-check-inline'><input type='checkbox' class='form-check-input' name='" + e.name + "'  id='" + e.name + "-preview' value='" + x.value + "' placeolder='" + e.placeholder + "' data-json='" + JSON.stringify(e.values) + "' data-select required/><label class='form-check-label'>" + x.label + "</label></div>";
            }
            else {
                checkboxText = checkboxText + "<div class='form-check form-check-inline'><input type='checkbox' class='form-check-input' name='" + e.name + "'  id='" + e.name + "-preview' value='" + x.value + "' placeolder='" + e.placeholder + "' data-json='" + JSON.stringify(e.values) + "' data-select /><label class='form-check-label'>" + x.label + "</label></div>";
            }
        }
    });
    checkboxText = checkboxText + "<span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div></div>";
    return checkboxText;
});
Cicero.FB.addFilter('select', function (e) {
    let selectText = "";
    if (e.multiple == true) {
        if (e.required == true) {
            selectText = "<div class ='form-group'><label>" + e.label + "</label><select multiple class='form-control' name='" + e.name + "'  id='" + e.name + "-preview' required>";
        }
        else {
            selectText = "<div class ='form-group'><label>" + e.label + "</label><select multiple class='form-control' name='" + e.name + "'  id='" + e.name + "-preview'>";
        }
    }
    else {
        if (e.required == true) {
            selectText = "<div class ='form-group'><label>" + e.label + "</label><select class='form-control' data-json='" + JSON.stringify(e.values) + "' name='" + e.name + "'  id='" + e.name + "-preview' data-select required>";
        }
        else {
            selectText = "<div class ='form-group'><label>" + e.label + "</label><select class='form-control' data-json='" + JSON.stringify(e.values) + "' name='" + e.name + "'  id='" + e.name + "-preview' data-select>";
        }
    }

    $.each(e.values, function (y, x) {
        if (x.selected == true) {
            selectText = selectText + "<option name='" + e.name + "' selected value='" + x.value + "' placeolder='" + e.placeholder + "' data-json='" + JSON.stringify(e.values) + "' data-selects >" + x.label + "</option>";
        }
        else {
            selectText = selectText + "<option name='" + e.name + "' value='" + x.value + "' placeolder='" + e.placeholder + "' data-json='" + JSON.stringify(e.values) + "' data-selects >" + x.label + "</option>";
        }
    });
    selectText = selectText + "</select><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";

    return selectText;
});
Cicero.FB.addFilter('targetForm', function (e) {
    let selectText = "";
    let finalText = "";

    if (e.required == true) {
        selectText = "<div class = 'form-group mapping-field'><label>" + e.label + "</label><div class ='col-lg-4'><select class='form-control ' data-json='" + JSON.stringify(e.targetformdata) + "' name='" + e.name + "'  id='" + e.name + "-preview' data-targetform='" + e.name + "-child' required><option value='>Select an option</option>";
    }
    else {
        selectText = "<div class = 'form-group mapping-field'><label>" + e.label + "</label><div class ='col-lg-4'><select class='form-control ' data-json='" + JSON.stringify(e.targetformdata) + "' name='" + e.name + "'  id='" + e.name + "-preview' data-targetform='" + e.name + "-child'><option value='>Select an option</option>";
    }

    $.each(e.targetformdata, function (a, b) {

        if (b.selected == true) {
            selectText = selectText + "<option name='" + b.name + "' selected value='" + b.name + "' placeolder='" + e.placeholder + "'>" + b.name + "</option>";
        }
        else {
            selectText = selectText + "<option name='" + b.name + "' value='" + b.name + "' placeolder='" + e.placeholder + "'>" + b.name + "</option>";
        }

    });
    finalText = finalText + selectText + "</select><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";

    if (e.required == true) {
        selectText = "<div class ='col-lg-4'><select class='form-control  target-form' name='" + e.name + "-child' id='" + e.name + "-child' required><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span><option value='>Select an option</option>";
    }
    else {
        selectText = "<div class ='col-lg-4'><select class='form-control  target-form' name='" + e.name + "-child' id='" + e.name + "-child'><option value='>Select an option</option>";
    }

    $.each(e.targetformdata, function (a, b) {
        if (b.selected == true) {
            $.each(b.childrens, function (y, x) {
                if (x.selected == true) {
                    selectText = selectText + "<option name='" + b.name + "' selected value='" + x.list + "' placeolder='" + e.placeholder + "'>" + x.list + "</option>";
                }
                else {
                    selectText = selectText + "<option name='" + b.name + "' value='" + x.list + "' placeolder='" + e.placeholder + "'>" + x.list + "</option>";
                }
            });
        }

    });
    finalText = finalText + selectText + "</select><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "-child' data-valmsg-replace='true'></span></div></div>";

    return finalText;
});
Cicero.FB.addFilter('textarea', function (e) {
    if (e.value == null) {
        if (e.required == true) {
            return "<div class ='form-group'><label>" + e.label + "</label><textarea class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' required></textarea><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
        }
        else {
            return "<div class ='form-group'><label>" + e.label + "</label><textarea class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' ></textarea></div>";
        }
    }
    else {
        if (e.required == true) {
            return "<div class ='form-group'><label>" + e.label + "</label><textarea class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' required> " + e.value + "</textarea><span class='text-danger field-validation-valid' data-valmsg-for='" + e.name + "' data-valmsg-replace='true'></span></div>";
        }
        else {
            return "<div class ='form-group'><label>" + e.label + "</label><textarea class='form-control length-lg' name='" + e.name + "'  id='" + e.name + "-preview' value='" + e.value + "' >" + e.value + " </textarea></div>";
        }
    }
});
Cicero.FB.addFilter('header', function (e) {
    if (e.wrapper_class != null) {
        return "<div class ='form-group col-lg-12" + " " + e.wrapper_class + "'><" + e.subtype + ">" + e.label + "</" + e.subtype + "></div>";
    }
    else {
        return "<div class ='form-group col-lg-12'><" + e.subtype + ">" + e.label + "</" + e.subtype + "></div>";
    }
});
Cicero.FB.addFilter('paragraph', function (e) {

    if (e.wrapper_class != null) {
        return "<div class ='form-group col-lg-12" + " " + e.wrapper_class + "'><p>" + e.label + "</p></div>";
    }
    else {
        return "<div class ='form-group col-lg-12'><p>" + e.label + "</p></div>";
    }
});