$(document).ready(function () {
    moment.locale('id');
    $('#myTabel').DataTable({
        ajax: {
            url: "https://localhost:7025/api/employees/get-all-master",
            dataType: "JSON",
            dataSrc: "data" //data source -> butuh array of object
        },
        dom: 'Bfrtip',
        
        buttons: [
            {
                extend: 'colvis',
                collectionLayout: 'fixed three-column',
                postfixButtons: ['colvisRestore']

            },
            {
                extend: 'print',
                exportOptions: {
                    columns:':visible'
                }
            }, 'copy',{
                extend: 'excelHtml5',
                title: 'Excel',
                text: 'Export to excel',
                        /*Columns to export*/
                        exportOptions: {
                           columns: [0, 1, 2, 3,4,5,6,7]
                       }
            }, {
                extend: 'pdfHtml5',
                title: 'PDF',
                text: 'Export to PDF',
                //Columns to export
                exportOptions: {
                     columns: [0, 1, 2, 3, 4, 5, 6,7]
                  }
            }, 
        ], 
        fixedColumns: {
            left: 0,
        },
        autoWidth:false,
        columns: [
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: 'fullName'},
            {
                data: "birthDate",
                    render: function (data, type, row) {
                        return moment(data).format('DD MMMM YYYY');
                    }
                
            },
            {
                data: "gender",
                render: function (data, type, row) {
                    if (data == 0) {
                        return 'Female';
                    }
                    return 'Male';
                }
            },
            {
                data: "hiringDate",
                render: function (data, type, row) {
                    return moment(data).format('DD MMMM YYYY');
                }
            },
            { data: "email" },
            { data: "phoneNumber" },
            { data: "major" },
            { data: "degree" },
            { data: "universityName" },
            {
                data: null,
                render: function (data, type, row) {
                    return `<button onclick="detail('${data.url}')" data-bs-toggle="modal" data-bs-target="#modal" class="btn btn-primary">Detail</button>`;
                }
            }

        ],
        
    })
})