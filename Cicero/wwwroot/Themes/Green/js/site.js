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
         
            var hide = objs.hide;
            jQuery("#media").find(".modal-title").html(objs.title);
            jQuery("#media").find(".modal-body").html("<div class='row'>"
                + "<iframe frameborder='0' src='/admin/media/pick.html?hide=" + hide + "&time=" + new Date().getTime() + "' frameborder='0'></iframe></div>"
                + "<div class='alert alert-danger' role='alert' id='no-media' style='display:none;'>"
                + "Please select atleast one file.</div>");
            jQuery("#media").find(".btn-secondary").html(objs.button.cancel);
            jQuery("#media").find(".btn-primary").html(objs.button.insert);
            jQuery("#media").find(".modal-dialog").removeClass("modal-lg modal-md modal-sm").addClass(objs.size);

            jQuery("#media .btn-secondary").on("click", function () {
                objs.close();
                objs.OnCancelled();
                jQuery(this).off("click")
            });
            jQuery("#media .btn-primary").on("click", function () {
                if (Cicero.SelectedImages.length > 0) {
                    objs.close();
                    objs.OnInsert(Cicero.SelectedImages);
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

        });
    },
    Form: {
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
                    jQuery("<li class='thumbs'><a target='_blank' href='/uploads/" + v.url + "'><img src='/images/pdf-icon.png' data-imgtitle='" + v.title + "'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
                else if (extension == "doc" || extension == "docx" || extension == "txt") {
                    jQuery("<li class='thumbs'><a target='_blank' href='/uploads/" + v.url + "'><img src='/images/doc-icon.png' data-imgtitle='" + v.title + "'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
                }
                else {
                    jQuery("<li class='thumbs'><a class='pop' href='javascript:void(0)'><img src='/uploads/" + v.url + "' data-imgtitle='" + v.title + "'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
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
                    //console.log(extension);
                    jQuery("<li class='thumbs'><a href='javascript:void(0)'><img src='/uploads/" + v.url + "' data-imgTtile='" + v.Title + "'><i class='fas fa-trash-alt' aria-hidden='true'></i></a><input type='hidden' name='images[]' value='" + v.id + "' /></li>").insertBefore(".thumbs.add");
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
                                toastr.success(succ.message);
                            }
                            else {
                                toastr.error(succ.message);
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
                jQuery(".uploaded-files").append("<span class='msg-attachment-file px-2 py-1'><i class='far fa-file-pdf'></i> <input type='hidden' name='images[]' value='" + v.id + "' />" + v.title + "<button type='button' class='close ml-1' data-dismiss='msg-attachment-file' aria-label='Close'> <span aria-hidden='true'>&times;</span></button></span>");
            });
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
