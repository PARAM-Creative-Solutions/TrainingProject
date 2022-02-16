/// <reference path="../AppScripts/PNotifyMessages.js" />
$(document).ready(function () {

    //******converts textbox to datepicker
    $('.datepic').datepicker();

    //*converts dropdown to editable deropdown
    $('.EditableSO').editableSelect();
    $('.EditableLineItem').editableSelect();
    $('.EditablePump').editableSelect();

    //****gets called when so number is changed or selected
    $('.EditableSO').on('blur', function () {
        if ($(this).val() != "") {
            $.ajax(
            {
                type: 'GET',
                url: '/LineItems/LoadLineItems',
                data: { SoNo: $(this).val() },
                datatype: 'html',
                global: false,//not to call the global handler function
                beforeSend: function () {
                    $('#SoSpinner').addClass("fa fa-spinner fa-spin");
                },
                error: function (x, status, error) {
                    $('#SoSpinner').removeClass("fa fa-spinner fa-spin");
                    $('#SoSpinner').html('Fail To Load..Try Again').delay(2000).fadeOut(100);;
                },
                complete: function () {
                    $('#SoSpinner').removeClass("fa fa-spinner fa-spin");
                },
                success: (function (res) {
                    $('#LineItem').val('');
                    var parent = $('#LineItem').parent('.col-md-6');
                    var child = parent.children();
                    parent.find("ul.es-list").empty();
                    $(res).each(function (index, item) {
                        parent.find("ul.es-list").append('<li class="es-visible">' + item + '</li>')
                    })
                })
            });
        }
    });
    //****gets called when line item is changed or selected
    $('.EditableLineItem').on('blur', function () {
        var SO_Number = $('#SalesOrderNumber').val();
        var SO_lineItem = $(this).val();
        var projectId = Number($('#SalesOrderId').val());
        if (SO_Number != "") {
            $.ajax(
            {
                type: 'GET',
                url: '/LineItems/CheckSOByLineItemAndSO',
                data: { so: SO_Number, lineitem: SO_lineItem },
                datatype: 'Json',
                global: false,//not to call the global handler function
                beforeSend: function () {
                    $('#SoLineItemSpinner').addClass("fa fa-spinner fa-spin");
                },
                error: function () {
                    $('#SoLineItemSpinner').removeClass("fa fa-spinner fa-spin");
                    $('#SoLineItemSpinner').html('Fail To Load..Try Again').delay(2000).fadeOut(100);;
                },
                complete: function () {
                    $('#SoLineItemSpinner').removeClass("fa fa-spinner fa-spin");
                },
                success: function (res) {
                    if (res.IsNewSo == false) {
                        $.ajax(
                            {
                                type: 'POST',
                                url: '/LineItems/GetSOByLineItemAndSO',
                                data: { so: SO_Number, lineitem: SO_lineItem },
                                datatype: 'Json',
                                global: false,//not to call the global handler function
                                beforeSend: function () {
                                    $('#SoLineItemSpinner').addClass("fa fa-spinner fa-spin");
                                },
                                error: function () {
                                    $('#SoLineItemSpinner').removeClass("fa fa-spinner fa-spin");
                                    $('#SoLineItemSpinner').html('Fail To Load..Try Again').delay(2000).fadeOut(100);;
                                },
                                complete: function () {
                                    $('#SoLineItemSpinner').removeClass("fa fa-spinner fa-spin");
                                },
                                success: function (res) {
                                    $('#divSo_Create').html(res);
                                }
                            });
                    }
                }
            });
        }
    });

    //****8gets called when pump type one dropdown is changed/selected
    $('#ValveModel').on('blur', function () {
        $.ajax(
        {
            type: 'GET',
            url: '/LineItems/LoadValveDetails',
            data: { valvemodel: $('#ValveModel').val() },
            datatype: 'Json',
            global: false,//not to call the global handler function
            beforeSend: function () {
                $('#SoPumpTypeSpinner').addClass("fa fa-spinner fa-spin");
            },
            error: function () {
                $('#SoPumpTypeSpinner').removeClass("fa fa-spinner fa-spin");
                $('#SoPumpTypeSpinner').html('Fail To Load..Try Again').delay(2000).fadeOut(100);;
            },
            complete: function () {
                $('#SoPumpTypeSpinner').removeClass("fa fa-spinner fa-spin");
            },
            success: function (res) {
                $('.valve').val("");
                $('#ValveSize').val(res.valvesize);
                $('#BodyRating').val(res.BodyRatingAndEndConnection);
                $('#BodyMaterial').val(res.Material);
                $('#ActuatorSize').val(res.ActuatorSize);
                $('#ActuatorType').val(res.ActuatorType);
                $('#ValveRating').val(res.ValveRating);
            }
        });
    });

    //**********gets called when Admin is creating the new LineItem
    $('#btnsubmit').on('click', function () {
        //debugger;
        var soNumber = $('#SalesOrderNumber').val();
        var lineItem = $('#LineItemNo').val();
        var isSoPresent = false;
        var Isvalid = $('#LineItem').valid();
        var salesorderId = Number($('#SalesOrderId').val());
        if (Isvalid) {
            var modeldata = $('#LineItem').serialize();
            $.ajax(
            {
                type: 'POST',
                url: '/LineItems/CheckSOByLineItemAndSO',
                data: { so: soNumber, lineitem: lineItem },
                global: false,
                datatype: 'JSON',
                success: function (res) {
                    if (res.IsNewSo == false) {
                        MessageBoxOk("So With This Line Item Is Already Present !");
                        isSoPresent = true;
                    }
                    else {
                        if (Isvalid && !isSoPresent) {
                            var Notify = MessageBoxOK_Cancel("Do You Want To Create New LineItem With SO Number : " + soNumber);
                            //$('#LineItem').val() + '-' + $('#LineItem').val()
                            Notify.get().on('pnotify.confirm', function () {
                                $.ajax(
                                {
                                    url: '/LineItems/Create',
                                    type: 'POST',
                                    data: modeldata,
                                    datatype: 'Json',
                                    success: function (res) {
                                        if (res.Result) {
                                            var notify = MessageBoxOk(res.Message);
                                            notify.get().on('pnotify.confirm', function () {
                                                //window.history.back();
                                                //window.location.href = "/Projects/Details?Id=" + projectId;
                                                window.location.href = "/SalesOrders/Details?Id=" + $("#SalesOrderId").val();
                                            });
                                        }
                                        else {
                                            res.Result = MessageBoxError(res.Message);
                                        }
                                    }
                                });
                            });
                        }
                    }
                }
            });
        }
    });

    //***********gets called when so details are edited
    $('#btnEditSOsubmit').on('click', function () {
        
        var Isvalid = $('#frmEditSO').valid();
        if (Isvalid) {
            
        var modeldata = $('#frmEditSO').serialize();
            var Notify = MessageBoxOK_Cancel("Do You Want To Save Changes ?");
            Notify.get().on('pnotify.confirm', function () {
                
                
                $.ajax(
                {
                    url: '/LineItems/Edit',
                    type: 'POST',
                    data: modeldata,
                    datatype: 'Json',
                    success: function (res) {
                        
                        if (res.Result) {
                            var notifySuccess = MessageBoxOk(res.Message)
                            notifySuccess.get().on('click', function () {
                                window.location.href = '/SalesOrders/Details?Id=' + $("#SalesOrderId").val();
                            });
                        }
                    }
                });
            });
        }
    });

  
});