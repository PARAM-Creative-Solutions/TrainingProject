
//$(document).ready(function () {
$('.SalesOrderDatatable').DataTable({
    dom: 'Bfrtip',
    //buttons: [
    //        {
    //            extend: 'copy',
    //            text:      '<i class="fa fa-files-o"></i>',
    //            titleAttr: 'Copy'
    //        },
    //        {
    //            extend: 'csv',
    //            text:      '<i class="fa fa-file-excel-o"></i>',
    //            titleAttr: 'Excel'
    //        },
    //        {
    //            extend: 'print',
    //            text:      '<i class="fa fa-file-text-o"></i>',
    //            titleAttr: 'CSV'
    //        },
    //        {
    //            extend: 'colvis',
    //            text:      '<i class="fa fa-file-pdf-o"></i>',
    //            titleAttr: 'PDF'
    //        }
    //],
    buttons: ['copy', 'csv', 'print', {
        extend: 'colvis',
        postfixButtons: ['colvisRestore'],
        columnText: function (dt, idx, title) {
            return (idx + 1) + ': ' + title;
        }
    }],
    stateSave: true,
    rowReorder: {
        selector: 'td:nth-child(2)'
    },
    responsive: true,
    "order": [[0, "desc"]]
});
//});