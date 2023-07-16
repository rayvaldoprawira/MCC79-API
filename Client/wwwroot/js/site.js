$(document).ready(function () {
    moment.locale('id');
    $('#myTabel').DataTable({
        ajax: {
            url: "https://localhost:7025/api/employees",
            dataType: "JSON",
            dataSrc: "data" //data source -> butuh array of object
        },
        dom: /*"<'ui grid'" +
            "<'row'" + "<'col-3'l>" + "<'col-6 mt--2'B>" + "<'col-3'f>" + ">" +
            "<'row dt-table'" + "<'col'tr>" + ">" +
            "<'row'" + "<'col-4'i>" + "<'col-8'p>" + ">" +
            ">",*/ 'lBfrtip',    

        buttons: [
            {
                extend: 'colvis',
                collectionLayout: 'fixed three-column',
                postfixButtons: ['colvisRestore']

            },
            {
                extend: 'print',
                className: 'btn-success',
                exportOptions: {
                    columns: ':visible'
                }
               
            }, 'copy', {
                extend: 'excelHtml5',
                title: 'Excel',
                text: 'Export to excel',
                /*Columns to export*/
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            }, {
                extend: 'pdfHtml5',
                title: 'PDF',
                text: 'Export to PDF',
                //Columns to export
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            },
        ],
        fixedColumns: {
            left: 0,
        },
        autoWidth: false,
        columns: [
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {data: 'nik'},
            {
                data: 'firstName',
                render: function (data, type, row) {
                    return row.firstName + ' ' + row.lastName;
                }
            },
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
            {
                data: null,
                render: function (data, type, row) {
                    return `<div><button onclick="Update('${row.guid}')" data-bs-toggle="modal" data-bs-target="#modalUpdate" class="btn btn-warning">Update</button> <button onclick="Delete('${row.guid}')" class="btn btn-danger">Delete</button></div>`;
                }
            }

        ],

    })
});
function Insert() {
    // Mendapatkan nilai-nilai input
    let firstName = document.querySelector("#firstName").value;
    let lastName = document.querySelector("#lastName").value;
    let birthDate = document.querySelector("#birthDate").value;
    let gender = document.querySelector("#gender").value;
    let genderEnum;
    if (gender === "Female") {
        genderEnum = 0;
    } else if (gender === "Male") {
        genderEnum = 1;
    }
    let hiringDate = document.querySelector("#hiringDate").value;
    let email = document.querySelector("#email").value;
    let phoneNumber = document.querySelector("#phoneNumber").value;
    // Data ke api
    let data = {
        firstName: firstName,
        lastName: lastName,
        birthDate: birthDate,
        gender: genderEnum,
        hiringDate: hiringDate,
        email: email,
        phoneNumber: phoneNumber
    };
    $.ajax({
        url: "https://localhost:7025/api/employees", // Sesuaikan URL sesuai dengan endpoint API Anda
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json"
    }).done(result => {
        Swal.fire(
            'Good job!',
            'Create Success',
            'success'
        )  // Tampilkan alert pemberitahuan jika berhasil
        $("#myTabel").DataTable().ajax.reload();
    }).fail(error => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: '<a href="">Maybe Wrong Input ?</a>'
        }) // Tampilkan alert pemberitahuan jika gagal
    });// Tampilkan alert pemberitahuan jika gagal

}

function Update(data) {
    console.log(data);
    $.ajax({
        url: `https://localhost:7025/api/employees/` + data,
        type: "GET",
        success: (res) => {
            let nik = parseInt(res.data.nik);
            let birthDate = moment(res.data.birthDate).format('YYYY-MM-DD');
            let hiringDate = moment(res.data.hiringDate).format('YYYY-MM-DD');
            let genderEnum = res.data.gender;
            if (genderEnum === 0) {
                gender = "F";
            } else if (genderEnum === 1) {
                gender = "M";
            };
            $("#guidU").val(res.data.guid);
            $('#nikU').val(nik);
            $('#firstNameU').val(res.data.firstName);
            $('#lastNameU').val(res.data.lastName);
            $('#birthDateU').val(birthDate);
            $("#genderU").val(gender);
            $('#hiringDateU').val(hiringDate);
            $('#emailU').val(res.data.email);
            $('#phoneNumberU').val(res.data.phoneNumber);
            $('#guid').val(res.data.guid);
            console.log(res);

        },
        error: (data) => {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!',
                footer: '<a href="">Why do I have this issue?</a>'
            })
        }
    });
}

function Update1() {
    let firstName = document.querySelector("#firstNameU").value;
    let lastName = document.querySelector("#lastNameU").value;
    let guid = document.querySelector("#guidU").value;
    let nik = document.querySelector("#nikU").value;
    let birthDate = document.querySelector("#birthDateU").value;
    let gender = document.querySelector("#genderU").value;
    let genderEnum;
    if (gender === "F") {
        genderEnum = 0;
    } else if (gender === "M") {
        genderEnum = 1;
    }
    let hiringDate = document.querySelector("#hiringDateU").value;
    let email = document.querySelector("#emailU").value;
    let phoneNumber = document.querySelector("#phoneNumberU").value;

    let data = {
        firstName: firstName,
        lastName: lastName,
        birthDate: birthDate,
        gender: genderEnum,
        hiringDate: hiringDate,
        email: email,
        phoneNumber: phoneNumber,
        nik: nik,
        guid: guid
            
    };
    console.log(data);
    $.ajax({
        url: "https://localhost:7025/api/employees",
        type: "PUT",
        data: JSON.stringify(data),
        contentType: "application/json"
    }).done(res => {
        Swal.fire({
            position: 'center',
            icon: 'success',
            title: 'Your work has been saved',
            showConfirmButton: true,
            confirmButtonText: 'OK',
        }).then((result) => {
            if (result.isConfirmed) {
                location.reload();
            }
        });
    }).fail(error => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: '<a href="">Why do I have this issue?</a>'
        }) // Tampilkan alert pemberitahuan jika gagal
        $("#myTabel").DataTable().ajax.reload(); // Reload Browser
    });
}
function Delete(deleteId) {
    console.log(deleteId);
    $.ajax({
        url: "https://localhost:7025/api/employees?guid=" + deleteId, // Sesuaikan URL sesuai dengan endpoint API Anda
        type: "DELETE",
    }).done(result => {
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })// Tampilkan alert pemberitahuan jika berhasil
        $("#myTabel").DataTable().ajax.reload();
    }).fail(error => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: '<a href="">Why do I have this issue?</a>'
        }) // Tampilkan alert pemberitahuan jika gagal
        $("#myTabel").DataTable().ajax.reload();
    });
}