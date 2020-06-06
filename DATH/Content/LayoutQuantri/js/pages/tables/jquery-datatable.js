//$(function () {
//    $('.js-basic-example').DataTable({
//        responsive: true
//    });

//    //Exportable table
//    $('.js-exportable').DataTable({
//        dom: 'Bfrtip',
//        responsive: true,
//        buttons: [
//            'copy', 'csv', 'excel', 'pdf', 'print'
//        ]
//    });
//});


$(document).ready(function () {

    var table = $('.js-exportable').DataTable({
        "language": { "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Vietnamese.json" },
        dom: 'Bfrtip',
        responsive: true,
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        columnDefs: [{
            sortable: false,
            "class": "index",
            targets: 0
        }],
        order: [[1, 'asc']],
        fixedColumns: true
    });
    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

});