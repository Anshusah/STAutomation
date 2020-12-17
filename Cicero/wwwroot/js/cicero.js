
String.prototype.format = String.prototype.format || function () {
    "use strict";
    var str = this.toString();
    if (arguments.length) {
        var t = typeof arguments[0];
        var key;
        var args = ("string" === t || "number" === t) ?
            Array.prototype.slice.call(arguments)
            : arguments[0];

        for (key in args) {
            str = str.replace(new RegExp("\\{" + key + "\\}", "gi"), args[key]);
        }
    }

    return str;
};
                //"Hello, {name}, are you feeling {adjective}?".format({name:"Kishan", adjective: "oK"});

                // Or

//"a{0}bcd{1}ef".format("foo", "bar");

var Cicero = {
    SelectedImages: [],
    DataTableSetting: function () {
        var customCheckbox = '<div class="custom-control custom-checkbox"><input type="checkbox" class="custom-control-input dt-checkboxes" id=""><label class="custom-control-label"></label></div>';
        return {
            "processing": true,
            "serverSide": true,
            //"dom": 'r<"clear">t<"row"<"col-sm-3"l><"col-sm-3"i><"col-sm-6"p>><"clear">',
            "dom": 'r<"clear">t<"datatable__controls"<"datatable__length"l><"datatable__info"i><"datatable__numbers"p>>',
            'searching': true,
            "ajax": { "url": null, "type": "POST" },
            "oLanguage": {sProcessing: "<div class='spinner'><div class='spinner-border text-primary' role='status'><span class='sr-only'>loading...</span></div></div>"},
            'stateSave': false,
            'select': {
                info: false,
                'style': 'multi'
            },
            'columnDefs': [
                {
                    'targets': 0,
                    'render': function (data, type, row, meta) {
                        if (type === 'display') {
                            data = customCheckbox;
                        }
                        return data;
                    },
                    'checkboxes': {
                        'selectRow': true,
                        'selectAll': true,
                        'selectAllRender': customCheckbox
                    }
                }
            ],
            "rowCallback": function (row, data, index) {
                jQuery('td:first-child', row).find("input").attr({ "id": "Ids", "name": "Ids", "value": data.id, "data-state-id": data.state_id, "data-switch": data.states });
            },
            "columns": [],
            order: [
                [1, "desc"]
            ],
            "paging": true,
            "pageLength": 10,
            "lengthMenu": [[5, 10, 20, 50, 100, 500], [5, 10, 20, 50, 100, 500]]
        }
    },
    DataTableLength: [[5, 10, 20, 50, 100, 500], [5, 10, 20, 50, 100, 500]],
    DataTablePageLength: 10,
    ConfirmString: function (e) {
        return '({button:{cancel:"Close",confirm:"Confirm"},size:"", title:"Confirm",content:"Are you sure?",open:function(){$("#confirm").modal({backdrop: "static",keyboard: false});},close:function(){$("#confirm").modal("hide");},' + e + '})';
    },
    MediaString: function (e) {
        return '({button:{cancel:"Close",insert:"Insert"},size:"modal-lg", title:"Media Manager",open:function(){$("#media").modal({backdrop: "static",keyboard: false});},close:function(){$("#media").modal("hide");},' + e + '})';
    },
    OnInit: function () {
        var self = this;
        jQuery(document).on("click", "[data-confirm]", function () {
            var __this=jQuery(this);
            var str = jQuery(this).data("confirm");

            str = str.replace(/^\{|\}$/g, '');
            console.log(str);
            console.log(self.ConfirmString(str));
            var objs = eval(self.ConfirmString(str)); 
            objs.OnInit();
            jQuery("#confirm").find(".modal-title").html(objs.title);
            jQuery("#confirm").find(".modal-body").html(objs.content);
            jQuery("#confirm").find(".btn-secondary").html(objs.button.cancel);
            jQuery("#confirm").find(".btn-primary").html(objs.button.confirm);
            jQuery("#confirm").find(".modal-dialog").removeClass("modal-lg modal-md modal-sm").addClass(objs.size);

            jQuery("#confirm .btn-secondary").on("click", function () { 
                objs.close();
                objs.OnCancelled(__this);
                jQuery(this).off("click")
            });
            jQuery("#confirm .btn-primary").on("click", function () {
                
                objs.close();
                objs.OnConfirm(__this);
                jQuery(this).off("click")
            });
            //close

        });
        jQuery(document).on("click", "[data-media]", function () {
            var _this = jQuery(this);
            var str = jQuery(this).data("media");

            str = str.replace(/^\{|\}$/g, '');
         
            var objs = eval(self.MediaString(str));
           
            objs.OnInit();
            var hide = objs.hide;
            jQuery("#media").find(".modal-title").html(objs.title);
            jQuery("#media").find(".modal-body").html("<div class='row'>"
                + "<iframe frameborder='0' src='/admin/media/pick.html?hide=" + hide + "&time=" + new Date().getTime() + "' frameborder='0'></iframe></div>"
                + "<div style='height:25px'><div class='alert alert-danger' role='alert' id='no-media' style='display:none;'>"
                + "Please select atleast one file.</div></div>" );
                jQuery("#media").find(".btn-secondary").html(objs.button.cancel);
                jQuery("#media").find(".btn-primary").html(objs.button.insert);
                jQuery("#media").find(".modal-dialog").removeClass("modal-lg modal-md modal-sm").addClass(objs.size);
    
            jQuery("#media .btn-secondary").on("click", function () {
                objs.close();
                objs.OnCancelled(_this);
                jQuery(this).off("click")
            });
            jQuery("#media .btn-primary").on("click", function () {

                if (Cicero.SelectedImages.length > 0) {
                    objs.close();
                    objs.OnInsert(Cicero.SelectedImages, _this);
                    Cicero.SelectedImages = [];
                    jQuery(this).off("click")
                }
                else {
                    
                    $("#no-media").fadeIn("500");
                    setTimeout(function () {
                        $("#no-media").fadeOut("500");
                    }, 3000);
                  
                }
            });
            jQuery("#media .close").on("click", function () {
                Cicero.SelectedImages = [];
                objs.OnInsert(Cicero.SelectedImages,_this);
                jQuery(this).off("click")
            });

        });
    },
    Article: {
        Setting: { FilterUrl: null },
        OnInit: function (e) {
            // jQuery(document).on("click", ".thumbs a i", function () {
            //     jQuery(this).closest('li').remove();
            // })
            jQuery(document).on("click", ".fileuploader__action-remove", function () {
                //jQuery(this).parent().parent().parent().parent().parent().css('display', 'none');
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
                { "title": "Parent", "data": "parent", "orderable": false },
                {
                    "title": "Status", "data": null, "orderable": true,
                    'render': function (data) {
                        if (data.status == null) {
                            data.status = "";
                        }
                        var content = data.status;
                        if (content == "Active") {
                            return "<span class='badge badge-success badge-pill'>Active</span>";
                        } else {
                            return "<span class='badge badge-secondary badge-pill'>Inactive</span>"
                        }
                    }
                },
                { "title": "Created At", "data": "created_at", "orderable": false },
                { "title": "Updated At", "data": "updated_at", "orderable": false },
                { "title": "Version", "data": "version", "orderable": false },
                { "title": "", "data": "action", "orderable": false, "class": "datatable__actions", }
            ];
            var table = jQuery("#articles");
            table.dataTable(TableSetting);
            jQuery(document).on("keyup", "#gridsearch", function () {
                table.fnFilter(this.value);
            });

        },
        InsertImages: function (e) {
            $.each(e, function (i, v) {
                var extension = v.url.substr((v.url.lastIndexOf('.') + 1));
                if (extension == "pdf") {
                    jQuery("<li class='thumbs fileuploader__item file-type file-type--pdf'><div class='fileuploader-item-inner'><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src='/images/pdf.png'></div></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' title='Remove'><i class='fileuploader__icon-remove ri-close-circle-fill' aria-hidden='true'></i></button></div></div><input type='hidden' name='images[]' value='" + v.id + "' /></li>").appendTo(".fileuploader__items");
                }
                else if (extension == "doc" || extension == "docx" || extension == "txt") {
                    jQuery("<li class='thumbs fileuploader__item file-type file-type--doc'><div class='fileuploader-item-inner'><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src='/images/doc.png'></div></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' title='Remove'><i class='fileuploader__icon-remove ri-close-circle-fill' aria-hidden='true'></i></button></div></div><input type='hidden' name='images[]' value='" + v.id + "' /></li>").appendTo(".fileuploader__items");
                }
                else {
                    //console.log(extension);
                    jQuery("<li class='thumbs fileuploader__item file-type file-type--image'><div class='fileuploader-item-inner'><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src='/uploads/" + v.url + "'></div></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' title='Remove'><i class='fileuploader__icon-remove ri-close-circle-fill' aria-hidden='true'></i></button></div></div><input type='hidden' name='images[]' value='" + v.id + "' /></li>").appendTo(".fileuploader__items");
                }
            });
        }
    },
    User: {
        InsertImages: function (e) {
            jQuery(".thumbs").not(".add").remove();
            jQuery(".fileuploader__wrapper").css('display', 'flex');
            $.each(e, function (i, v) {
                var extension = v.url.substr((v.url.lastIndexOf('.') + 1));
                if (extension == "pdf") {
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/images/pdf-icon.png'><i class='fas' aria-hidden='true'><img src='/frontend/img/delete.png' /></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
                else if (extension == "doc" || extension == "docx" || extension == "txt") {
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/images/doc-icon.png'><i class='fas' aria-hidden='true'><img src='/frontend/img/delete.png' /></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
                else {
                    jQuery("<li class='thumbs fileuploader__item'><div class='fileuploader__item-image'><img src='/uploads/" + v.url + "'></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' title='Remove'><i class='fileuploader__icon-remove ri-close-circle-fill' aria-hidden='true'></i></button></div><input type='hidden' name='images[]' value='" + v.id + "' /></li>").appendTo(".fileuploader__items");
                }
                return false;
            });

        }
    },
    Media: {
        Delete: function (e) {
            alert(1);
            var obj = jQuery(e).closest('a').data('json');
            $.getJSON(CiceroVars.base_url + "/admin/media/delete-by-id.html", { selected_image_id: obj.id }, function (types) {
                if (types.status == "true") {
                    jQuery(e).closest('li').remove();
                }
            });
        },
        Remove: function (e) {
            $.getJSON("/admin/media/delete-by-id.html", { selected_image_id: e }, function (types) {
                if (types.status === "true") {
                    $('a[data-json=' + e + ']').closest('li').remove();
                }
            });
        }
    },
    ActivityLog: {
        NotificationRead: function (e) {

            $.getJSON(CiceroVars.base_url + "admin/activity-notification-read.html", function (types) {
                if (types.status == "success") {
                    $('#notification-log-count').remove();
                    $('#notification-activity-log').html('<div class="notification-item"><div class= "media" ><div class="media-img circle"></div><div class="media-body"><p>No items to display</p></div></div ></div >');
                }
            });

        }
    },
    Tenant: {
        Setting: { FilterUrl: null },
        OnInit: function (e) {
            jQuery(document).on("click", ".thumbs a i", function () {
                jQuery(this).closest('li').remove();
            })
        },
        InitDataTable: function (e) {
            Cicero.Tenant.Setting.FilterUrl = e.FilterUrl;
            var TableSetting = Cicero.DataTableSetting();
            TableSetting.ajax.url = Cicero.Tenant.Setting.FilterUrl;
            TableSetting.columns = [
                { "title": "", "data": "id", "orderable": false },
                { "title": "Name", "data": "name", "orderable": true },
                { "title": "Email", "data": "email", "orderable": false },
                {
                    "title": "Status", "data": null, "orderable": true,
                    'render': function (data) {
                        if (data.status == null) {
                            data.status = "";
                        }
                        var content = data.status;
                        if (content == "Active") {
                            return "<span class='badge badge-success badge-pill'>Active</span>";
                        } else {
                            return "<span class='badge badge-secondary badge-pill'>Inactive</span>"
                        }
                    }
                },
                { "title": "Created At", "data": "created_at", "orderable": false },
                { "title": "Updated At", "data": "updated_at", "orderable": false },
                { "title": "", "data": "action", "orderable": false, "class": "datatable__actions", }
            ];
            var table = jQuery("#tenants");
            table.dataTable(TableSetting);
            jQuery(document).on("keyup", "#gridsearch", function () {
                table.fnFilter(this.value);
            });

        }
    },
    BuilderForm: {
        Setting: { FilterUrl: null },
        OnInit: function (e) {
            jQuery(document).on("click", ".thumbs a i", function () {
                jQuery(this).closest('li').remove();
            })
        },
        InitDataTable: function (e) {
            Cicero.BuilderForm.Setting.FilterUrl = e.FilterUrl;
            var TableSetting = Cicero.DataTableSetting();
            TableSetting.ajax.url = Cicero.BuilderForm.Setting.FilterUrl;
            TableSetting.columns = [
                { "title": "", "data": "id", "orderable": false },
                { "title": "Name", "data": "name", "orderable": true },
                { "title": "Tenant", "data": "tenant", "orderable": true },
                { "title": "Created At", "data": "created_at", "orderable": true },
                { "title": "Updated At", "data": "updated_at", "orderable": true},
                {
                    "title": "Status", "data": null, "orderable": false,
                    'render': function (data) {
                        if (data.status == null) {
                            data.status = "";
                        }
                        var content = data.status;
                        if (content == "Active") {
                            return "<span class='badge badge-success badge-pill'>Active</span>";
                        } else {
                            return "<span class='badge badge-secondary badge-pill'>Inactive</span>"
                        }
                    }
                },
                { "title": "", "data": "action", "orderable": false, "class": "datatable__actions" }
            ];
            var table = jQuery("#builder-forms");
            table.dataTable(TableSetting);
            jQuery(document).on("keyup", "#gridsearch", function () {
                table.fnFilter(this.value);
            });

        }
    },
    Queue: {
        Setting: { FilterUrl: null },
        OnInit: function (e) {
            jQuery(document).on("click", ".thumbs a i", function () {
                jQuery(this).closest('li').remove();
            })
        },
        InitDataTable: function (e) {
            var editor;

            Cicero.Queue.Setting.FilterUrl = e.FilterUrl;
            var TableSetting = Cicero.DataTableSetting();
            TableSetting.ajax.url = Cicero.Queue.Setting.FilterUrl;
            TableSetting.columns = [
                { "title": "", "data": "id", "orderable": false },
                { "title": "Name", "data": "name", "orderable": true },
                { "title": "Created By", "data": "created_by", "orderable": false },
                { "title": "Updated By", "data": "updated_by", "orderable": true },
                //{ "title": "Role", "data": "role", "orderable": true },
                { "title": "Updated At", "data": "updated_at", "orderable": false },
                //{ "title": "Order By", "data": "order", "orderable": true, "className": "editable", "render": editIcon },
                { "title": "", "data": "action", "orderable": false, "class": "datatable__actions" }
            ];
            var table = jQuery("#queues");
            table.dataTable(TableSetting);
            jQuery(document).on("keyup", "#gridsearch", function () {
                table.fnFilter(this.value);
            });

            //editor = new $.fn.dataTable.Editor({
            //    //ajax: Cicero.Queue.Setting.FilterUrl,
            //    ajax: CiceroVars.base_url + "queue/queue-edit-order",
            //    data: { "id": id, "order": order },
            //    table: "#queues",
            //    idSrc: 'id',
            //    fields: [{
            //        name: "id"
            //    }, {
            //        name: "name"
            //    }, {
            //        name: "created_by"
            //    }, {
            //        name: "updated_by"
            //    }, {
            //        name: "role"
            //    }, {
            //        name: "updated_at"
            //    },{
            //        name: "order"
            //    }, {
            //        name: "action"
            //    }]
            //});

            //// Activate an inline edit on click of a table cell
            //$('#queues').on('click', 'tbody td.editable', function (e) {
            //    editor.inline(this, {
            //        buttons: { label: '&gt;', fn: function () { this.submit(); } }
            //    });
            //});

            //var editIcon = function (data, type, row) {
            //    if (type === 'display') {
            //        return data + ' <i class="fa fa-pencil"/>';
            //    }
            //    return data;
            //};

        }
    },
    State: {
        Setting: { FilterUrl: null },
        OnInit: function (e) {
            jQuery(document).on("click", ".thumbs a i", function () {
                jQuery(this).closest('li').remove();
            })
        },
        InitDataTable: function (e) {
            Cicero.State.Setting.FilterUrl = e.FilterUrl;
            var TableSetting = Cicero.DataTableSetting();
            TableSetting.ajax.url = Cicero.State.Setting.FilterUrl;
            TableSetting.columns = [
                { "title": "", "data": "id", "orderable": false },
                { "title": "Name", "data": "name", "orderable": true },
                { "title": "Created By", "data": "created_by", "orderable": false },
                { "title": "Updated By", "data": "updated_by", "orderable": true },
                { "title": "Created At", "data": "created_at", "orderable": false },
                //{ "title": "State Role For", "data": "role", "orderable": true },
                { "title": "Updated At", "data": "updated_at", "orderable": false },
                { "title": "", "data": "action", "orderable": false, "class": "datatable__actions" }
            ];
            var table = jQuery("#states");
            table.dataTable(TableSetting);
            jQuery(document).on("keyup", "#gridsearch", function () {
                table.fnFilter(this.value);
            });

        }
    },
    Message: {
        InsertImages: function (e, r) {
            $.each(e, function (i, v) {
                var extension = v.url.substr(v.url.lastIndexOf('.') + 1).toLowerCase();
                if (extension === "pdf") {
                    jQuery("<span class='msg-attachment-file px-2 py-1'><i class='ri-file-pdf-line'></i> <input type='hidden' name='images[]' value='" + v.id + "' />" + v.title + "<button type='button' class='close ml-1 attachment__btn' data-dismiss='msg-attachment-file' aria-label='Close' title='Remove attachment'><span aria-hidden='true'>&times;</span></button></span>").appendTo(".uploaded-files");
                }
                else if (extension === "doc" || extension === "docx" || extension === "txt") {
                    jQuery("<span class='msg-attachment-file px-2 py-1'><i class='ri-file-word-2-line'></i> <input type='hidden' name='images[]' value='" + v.id + "' />" + v.title + "<button type='button' class='close ml-1 attachment__btn' data-dismiss='msg-attachment-file' aria-label='Close' title='Remove attachment'><span aria-hidden='true'>&times;</span></button></span>").appendTo(".uploaded-files");
                }
                else {
                    jQuery("<span class='msg-attachment-file px-2 py-1'><i class='ri-image-line'></i> <input type='hidden' name='images[]' value='" + v.id + "' />" + v.title + "<button type='button' class='close ml-1 attachment__btn' data-dismiss='msg-attachment-file' aria-label='Close' title='Remove attachment'><span aria-hidden='true'>&times;</span></button></span>").appendTo(".uploaded-files");
                }
            });
            //$.each(e, function (i, v) {
            //    var extension = v.url.substr((v.url.lastIndexOf('.') + 1));
            //    jQuery(".uploaded-files").append("<span class='msg-attachment-file px-2 py-1'><i class='far fa-file-pdf'></i> <input type='hidden' name='images[]' value='" + v.id + "' />" + v.title + "<button type='button' class='close ml-1' data-dismiss='msg-attachment-file' aria-label='Close'> <span aria-hidden='true'>&times;</span></button></span>");
            //});
        }
    },
    Template: {
        Setting: { FilterUrl: null },
        OnInit: function (e) {
            jQuery(document).on("click", ".thumbs a i", function () {
                jQuery(this).closest('li').remove();
            })
        },
        InitDataTable: function (e) {
            Cicero.Template.Setting.FilterUrl = e.FilterUrl;
            var TableSetting = Cicero.DataTableSetting();
            TableSetting.ajax.url = Cicero.Template.Setting.FilterUrl;
            TableSetting.columns = [
                { "title": "", "data": "id", "orderable": false },
                { "title": "Title", "data": "title", "orderable": true },
                { "title": "Tenant", "data": "tenant", "orderable": false },
                { "title": "Created At", "data": "created_at", "orderable": false },
                { "title": "Updated At", "data": "updated_at", "orderable": false },
                { "title": "Version", "data": "version", "orderable": false },
                { "title": "", "data": "action", "orderable": false, "class": "datatable__actions", }
            ];
            var table = jQuery("#templates");
            table.dataTable(TableSetting);
            jQuery(document).on("keyup", "#gridsearch", function () {
                table.fnFilter(this.value);
            });

        }
    },
    Utils: {
        OnInit: function () {
            jQuery(document).on("click", "[data-tenant-value]", function () {
                var str = $(this).data("tenant-value");
                //console.log(str);
                jQuery(".goto-selected").html($(this).data("tenant-text"));
                var _obj = Cicero.Utils.ReplaceTenantId(str);
                //console.log(_obj);
                if (_obj.status == 'success') {
                    window.location = _obj.url;
                } else {
                    window.location = "/admin/" + str + ".html";
                }

            })
            var isActive = jQuery("[data-tenant-value].active");
            if (isActive.length) {
                jQuery(".goto-selected").html(isActive.data("tenant-text"));
            }
        },
        ReplaceTenantId: function (url) {
            var str = window.location.href;

            var url_str = str.match(/admin\/(.*)\/(?:users|user|widget|builderform|widgets|article|articles|setting|settings|tenant|tenants|roles|role|menus|menu|medias|media|manage|messages|case|cases|templates|template|caseforms|caseform)/g);
            if (Array.isArray(url_str)) {
                //console.log(url_str[0]);

                var splitted_url = url_str[0].split("/");
                if (Array.isArray(splitted_url)) {
                    splitted_url[1] = url;
                }
                var joined = splitted_url.join("/");
                var toReturn = str.replace(url_str[0], joined)
                return { "status": "success", "url": toReturn };
            } else {
                var url_str = str.match(/admin\/(.*)/g);

                if (Array.isArray(url_str)) {
                    //console.log(url_str);
                    var splitted_url = url_str[0].split("/");

                    //console.log(splitted_url[2]);
                    if (Array.isArray(splitted_url)) {
                        if (splitted_url.length <= 2) {
                            return { "status": "failed" };
                        }
                        //console.log(splitted_url);
                        splitted_url.splice(1, 0, url);
                        //console.log("<");
                        //console.log(splitted_url);
                        //console.log(">");
                        var joined = splitted_url.join("/");

                        var toReturn = str.replace(url_str[0], joined)
                        return { "status": "success", "url": toReturn };
                    }
                } else {
                    //console.log(url_str);
                    return { "status": "falied" };
                }
            }

        }
    },
    Form: {
        Setting: { FilterUrl: null },
        OnInit: function (e) {
            $(document).on("click", "#reject-btn", function () {
                var reasonfield = $("#reason").val();
                if (reasonfield.trim() == "") {
                    $("#reason-validation").css('color', 'red');
                }
                else {
                    $("#reason-validation").css('color', 'black');
                    $("#claims-form").submit();
                }
            });

        },
        InitDataTable: function (e) {
            /*
             * 1= Created
             * 2= Receive
             * 3= In Review
             * 4= With Shipping Company
             * 5= In Court
             * 6= Settle
             * 7= Reject 
             */
            Cicero.Form.Setting.FilterUrl = e.FilterUrl;
            var TableSetting = e.TableSetting;
            //var TableSetting = Cicero.DataTableSetting();

         

            var table = jQuery("#case-forms");
            table.dataTable(TableSetting);
            jQuery(document).on("keyup", "#gridsearch", function () {
                table.fnFilter(this.value);
            });
            jQuery("#case-forms tbody, #case-forms thead").on('click', 'input[type="checkbox"]', function (e) {
                setTimeout(function (e) {
                    jQuery("select[name='action'] option").removeAttr("disabled");
                    var moving_states = [];
                    jQuery.each(jQuery('input[name = "Ids"]:checked'), function () {
                        var isJson = "[]";
                        try {
                            isJson = JSON.parse(jQuery(this).attr("data-switch"));
                        } catch (e) {
                            isJson = "[]";
                        }
                        moving_states.push(isJson);
                        console.log(isJson);
                    });

                    //console.log(moving_states)
                    var counts = {};
                    for (var i = 0; i < moving_states.length; i++) {
                        for (var j = 0; j < moving_states[i].length; j++) {
                            counts[moving_states[i][j].id] = 1 + (counts[moving_states[i][j].id] || 0);
                        }
                    }
                    //console.log(counts);
                    //console.log("Moving State Length=" + moving_states.length);
                    var state_ids = new Array();
                    for (var key in counts) {
                        //console.log(counts);
                        //console.log(moving_states.length);
                        if (counts.hasOwnProperty(key) && (moving_states.length == counts[key])) {
                            state_ids.push(key);
                            //console.log(key + " -> " + counts[key]);
                        }
                    }
                    //console.log(state_ids);
                    var state_to_show = new Array();
                    for (var _i = 0; _i < moving_states.length; _i++) {
                        for (var _j = 0; _j < moving_states[_i].length; _j++) {
                            for (var x = 0; x < state_ids.length; x++) {
                                if (state_ids[x] == moving_states[_i][_j].id) {
                                    //console.log(state_ids[x]);
                                    state_to_show[state_ids[x]] = moving_states[_i][_j];
                                    //console.log(state_to_show[state_ids[x]]);
                                }
                            }
                        }
                    }
                    state_to_show = state_to_show.filter(Boolean)
                    //console.log(state_to_show);
                    //$('#theOptions2 option[value='+ SC +']')
                    jQuery("select[name='action'] option[data-orginal='false']").remove()
                    //jQuery("select[name='action']").append("<option value='' data-delete='' data-reason=''>Select Action</option>");
                    for (var i = 0; i < state_to_show.length; i++) {
                        var option = $('<option>', this).attr({ "value": state_to_show[i].id, "text": state_to_show[i].name });

                        jQuery("select[name='action']").append("<option value='" + state_to_show[i].id + "' data-delete='" + state_to_show[i].can_delete + "' data-orginal='false'  data-reason='" + state_to_show[i].need_reason + "'>" + state_to_show[i].name + " </option>");
                    }                   
                    
                }, 100);

            });



        },
        InsertImages: function (e, f) {
            if (f != null && f != "") {
                $.each(e, function (i, v) {
                    var extension = v.url.substr((v.url.lastIndexOf('.') + 1));
                    if (extension == "pdf") {
                        jQuery("<li class='thumbs fileuploader__item file-type file-type--pdf'><a target='_blank' href='/uploads/" + v.url + "'><img src='/images/pdf-icon.png' data-imgtitle='" + v.title + "'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='" + f + "' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                    }
                    else if (extension == "doc" || extension == "docx" || extension == "txt") {
                        jQuery("<li class='thumbs fileuploader__item file-type file-type--doc'><a target='_blank' href='/uploads/" + v.url + "'><img src='/images/doc-icon.png' data-imgtitle='" + v.title + "'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='" + f + "' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                    }
                    else {
                        jQuery("<li class='thumbs fileuploader__item file-type file-type--image'><a class='pop' href='javascript:void(0)'><img src='/uploads/" + v.url + "' data-imgtitle='" + v.title + "'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='" + f + "' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                    }

                });
            }

        },
        ApplyAction: function (e) {
            $("#editAction").submit();
            //$.ajax({
            //    type: "POST",
            //    url: $("#action-page").val(),
            //    //contentType: "application/json; charset=utf-8",
            //    data: { Ids: $("#case_id").val(), action: $("#action").val(), page: $("#page").val() },
            //    dataType: "json",
            //    success: function (response) {
            //        console.log($("#page").val());
            //        console.log(response);
            //        if (response == "deleted") {
            //            window.location.href = $("#page").val();
            //        }
            //        else if (response == "state") {
            //            window.location.href = $("#page").val();
            //        }
            //        else {
            //            location.reload();
            //        }

            //    },
            //    error: function (response) {

            //    }
            //});
        },
        SendNotice: function (e) {
            $.ajax({
                type: "POST",
                url: CiceroVars.base_url + 'message/send-notice.html',
                //contentType: "application/json; charset=utf-8",
                data: { id: $("#case_id").val(), subject: $("#notice_subject").val(), message: $("#notice_message").val(), isNotify: $("#notice_checkbox").is(':checked') },
                dataType: "json",
                success: function (response) {
                    $("#exampleModalCenter").modal('hide');
                    if (response != null) {
                        const monthNames = ["January", "February", "March", "April", "May", "June",
                            "July", "August", "September", "October", "November", "December"];
                        $("#messages-list").children().removeClass("latest");
                        let responsedate = new Date(response.createdAt.replace(/\/Date\((-?\d+)\)\//, '$1'));
                        //var newDate = responsedate.getMonth();
                        let newDate = responsedate.getDate() + " " + monthNames[responsedate.getMonth()] + " " + responsedate.getFullYear();
                        $("#messages-list").prepend("<div class='message-box latest p-4 mb-4 position-relative'><span class='message-clientstat'><i class='fas fa-check-circle'></i>Client Notified</span><p class='kicker mb-1'><span class='sender mr-3'>From: " + response.nameSender + " </span><span class='date'>|&nbsp;" + newDate + "</span></p><h4 class='font-weight-bold mb-3'>" + response.subject + "</h4><p class='mb-0'>" + response.content + "</p></div>");
                        //window.location.href = CiceroVars.base_url + 'cases.html';
                    }
                    //else {
                    //    //console.log(response);
                    //    location.reload();
                    //}

                },
                error: function (response) {

                }
            });
        }
    }
}

jQuery(document).ready(function () {
    Cicero.OnInit();
    Cicero.Article.OnInit();
    Cicero.Utils.OnInit();
});

function selected(e) {
    Cicero.SelectedImages = e;
}
