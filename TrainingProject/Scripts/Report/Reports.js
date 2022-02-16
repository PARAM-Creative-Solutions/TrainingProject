

$(document).ready(function () {
    //  var dt=  $("#documenttable").val();
    $('#SelectedUniqueSONumber').editableSelect();
    $('#SelectedProjectNumber').editableSelect();

    $("#SelectedUniqueSONumber").autoComplete(
    {
      
        //minChars: 1,
        source: function (request, response) {
            //gets the value of the textbox of SO Number
            $.ajax(
            {
                url: '/Base/GetSONumbers',
                type: "GET",
                contentType: "application/json",
                data: { Uniquesono: request } ,
                datatype: "Json",
                global: true,
                success: function (data) {
                    response(data);
                }
            });
        }
    });
    //****gets called when top engg is changed or selected
    $("#SelectedEngg").on('change', function () {
        var TOpengg = $('#SelectedEngg').val();
        if (TOpengg != "") {
            $.ajax(
            {
                type: 'GET',
                url: '/Dashboard/GetSalesOrders',
                data: { engg: TOpengg },
                datatype: 'Json',
                global: true,//not to call the global handler function
                success: (function (res) {
                    $('#SelectedProjectNumber').val('');
                    var parent = $('#SelectedProjectNumber').parent('.col-md-4');
                    var child = parent.children();
                    parent.find("ul.es-list").empty();
                    $(res).each(function (index, item) {
                        parent.find("ul.es-list").append('<li class="es-visible">' + item + '</li>')

                    })
                    $.ajax(
          {
              type: 'GET',
              url: '/Dashboard/getso',
              data: { engg: TOpengg },
              datatype: 'Json',
              global: true,//not to call the global handler function
              success: (function (res) {
                  $('#SelectedUniqueSONumber').val('');
                  var parent = $('#SelectedUniqueSONumber').parent('.col-md-4');
                  var child = parent.children();
                  parent.find("ul.es-list").empty();
                  $(res).each(function (index, item) {
                      parent.find("ul.es-list").append('<li class="es-visible">' + item + '</li>')
                  })
              })
          });
                })
            });
        }
    });

    //****gets called when Editable ProjectName is changed or selected
    $("#SelectedProjectNumber").on('blur', function () {
        var projectid = $('#SelectedProjectNumber').val();
        if (projectid != "") {
            $.ajax(
            {
                type: 'GET',
                url: '/Dashboard/getso_project',
                data: { projectid: projectid },
                datatype: 'Json',
                global: true,//not to call the global handler function
                success: (function (res) {
                    $('#SelectedUniqueSONumber').val('');
                    var parent = $('#SelectedUniqueSONumber').parent('.col-md-4');
                    var child = parent.children();
                    parent.find("ul.es-list").empty();
                    $(res).each(function (index, item) {
                        parent.find("ul.es-list").append('<li class="es-visible">' + item + '</li>')

                    })

                })
            });
        }
    });

    $("#CheckAllEngg").on('ifChecked', function ()
    {
        var ch = $('#CheckAllEngg').is(":checked");
        if (ch = 'true')
        {
            //var abc = $("#CheckAllProject").val();
            var abc = $("#CheckAllSalesOrder").val();
            //$('#CheckAllProject').iCheck('uncheck');
            $('#CheckAllSalesOrder').iCheck('uncheck');
            $('#CheckAllUniqueSONumber').iCheck('uncheck');
            $("#SelectedEngg").attr("disabled", true)
            //$("#SelectedProjectNumber").attr("disabled", true)
            //$("#SelectedSalesOrderNumber").attr("disabled", true)
            $("#SelectedUniqueSONumber").attr("disabled", true)
        }
        else {
            $("#SelectedEngg").attr("disabled", false)
        }
    })
    $("#CheckAllEngg").on('ifUnchecked', function ()
    {
        var ch = $('#CheckAllEngg').is(":unchecked");
        if (ch = 'true') {
            $("#SelectedEngg").attr("disabled", false)
            //$("#SelectedProjectNumber").attr("disabled", false)
            //$("#SelectedSalesOrderNumber").attr("disabled", false)
            $("#SelectedUniqueSONumber").attr("disabled", false)
        }
    })

    //$("#CheckAllProject").on('ifChecked', function () {
    $("#CheckAllSalesOrder").on('ifChecked', function ()
    {
        //var ch = $('#CheckAllProject').is(":checked");
        var ch = $('#CheckAllSalesOrder').is(":checked");
        if (ch = 'true') {
            //$('#CheckAllEngg').iCheck('uncheck');
            $('#CheckAllUniqueSONumber').iCheck('uncheck');
            //$("#SelectedProjectNumber").attr("disabled", true)
            $("#SelectedSalesOrderNumber").attr("disabled", true)
            $("#SelectedUniqueSONumber").attr("disabled", true)

        }
        else {
            //$("#SelectedProjectNumber").attr("disabled", false)
            $("#SelectedSalesOrderNumber").attr("disabled", false)
            $("#SelectedUniqueSONumber").attr("disabled", false)
        }
    })

    //$("#CheckAllProject").on('ifUnchecked', function () {
    $("#CheckAllSalesOrder").on('ifUnchecked', function ()
    {
         //var ch = $('#CheckAllProject').is(":unchecked");
        var ch = $('#CheckAllSalesOrder').is(":unchecked");
        if (ch = 'true') {
            //$("#SelectedProjectNumber").attr("disabled", false)
            $("#SelectedSalesOrderNumber").attr("disabled", false)
            $("#SelectedUniqueSONumber").attr("disabled", false)
        }
    })
    $("#CheckAllUniqueSONumber").on('ifChecked', function () {
        var ch = $('#CheckAllUniqueSONumber').is(":checked");
        if (ch = 'true') {
            $('#CheckAllEngg').iCheck('uncheck');
            //$('#CheckAllProject').iCheck('uncheck');
            $('#CheckAllSalesOrder').iCheck('uncheck');
            $("#SelectedUniqueSONumber").attr("disabled", true)
        }
        else {
            $("#SelectedUniqueSONumber").attr("disabled", false)

        }
    })
    $("#CheckAllUniqueSONumber").on('ifUnchecked', function () {
        var ch = $('#CheckAllUniqueSONumber').is(":unchecked");
        if (ch = 'true') {
            $("#SelectedUniqueSONumber").attr("disabled", false)
        }
    })
    $("#CheckAllVendor").on('ifChecked', function () {
        var ch = $('#CheckAllVendor').is(":checked");
        if (ch = 'true') {
            $("#SelectedVendor").attr("disabled", true)
        }
        else {
            $("#SelectedVendor").attr("disabled", false)
        }
    })
    $("#CheckAllVendor").on('ifUnchecked', function () {
        var ch = $('#CheckAllVendor').is(":unchecked");
        if (ch = 'true') {
            $("#SelectedVendor").attr("disabled", false)
        }
    })
    //#####Used in WorkStatus View by priyanka
    $("#chkAllpendSub").on('ifChecked', function () {
        var ch = $('#chkAllpendSub').is(":checked");
        if (ch = 'true') {
            $("#pendSubDashType").attr("disabled", true)
        }
        else {
            $("#pendSubDashType").attr("disabled", false)
        }
    })
    $("#chkAllpendSub").on('ifUnchecked', function () {
        var ch = $('#pendSubDashType').is(":unchecked");
        if (ch = 'true') {
            $("#pendSubDashType").attr("disabled", false)
        }
    })

    $("#chkAllMC").on('ifChecked', function () {
        var ch = $('#chkAllMC').is(":checked");
        if (ch = 'true') {
            $("#ManufacturingCtype").attr("disabled", true)
        }
        else {
            $("#ManufacturingCtype").attr("disabled", false)
        }
    })
    $("#chkAllMC").on('ifUnchecked', function () {
        var ch = $('#ManufacturingCtype').is(":unchecked");
        if (ch = 'true') {
            $("#ManufacturingCtype").attr("disabled", false)
        }
    })
    //###############################################
});
