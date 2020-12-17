$(function () {
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
        singleDatePicker: true,
        opens: 'right',
        locale: {
            format: 'HH:mm A'
        }
    }).on('show.daterangepicker', function (ev, picker) {
        picker.container.find(".calendar-table").hide();
    });
    $(".beneCountry").change(function () {
        $.ajax({
            url: '/api/country/phonecode',
            data: {
                countryCode: $(this).val(),
            },
            error: function () {
                toastr.error('An error has occurred');
            },
            dataType: 'json',
            success: function (data) {
                $(".beneMobileNumber").closest(".form-group").find("label").html("Mobile Number (" + data + ")*");
            },
            type: 'GET'
        });
    });
    $(".selectCountry").change(function () {
        $.ajax({
            url: '/api/country/phonecode',
            data: {
                countryCode: $(this).val(),
            },
            error: function () {
                toastr.error('An error has occurred');
            },
            dataType: 'json',
            success: function (data) {
                $(".beneMobileNumber").closest(".form-group").find("label").html("Mobile Number (" + data + ")*");
                $(".beneCountry").addClass('disable');
            },
            type: 'GET'
        });
    });

    $(".cleartabfields").click(function () {
        var name = $(this).attr('name');
        var itm = $('[name=' + name + ']').closest(".tab-pane");
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
        removePreviousValidations();
        var country = $('select.selectCountry').val();
        $('select.beneCountry').val(country).trigger('change');

    });
    $(".redirectToPaymentPage").click(function () {
        window.location.href = "/admin/form/simpletransfer/transfer/" + $("#encryptedcaseid").val()+"/payment.html";
    });
    $(".redirectToMainPage").click(function () {
        window.location.href = "/st/user/dashboard.html";
    });
    $(".redirectDashboard").click(function () {
        //window.location.href = "/jazzcash/payer/index.html";
        window.location.href = "/st/user/dashboard.html";
    });

    $(".modalDismiss").click(function () {
       $(this).parents('.modal').modal('toggle');
    });

    $(".removeValidations").click(function () {
        removePreviousValidations();
    });
    if ($('select.selectCountry').val() !== "") {
        $(".btn-getstarted").click();
    }
})