$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblData').DataTable({
        ajax: {
            url: "/admin/productproducttag/getall",
            type: "GET",
            datatype: "json",
            //success: function (data) { console.log(data)},
        },
        columns: [
            { data: 'product.title', width: '25%' },
            { data: 'productTag.name', width: '25%' },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/admin/productproducttag/upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil"></i> Edit</a>
                            <a onClick=Delete('/admin/productproducttag/delete/${data}') class="btn btn-danger mx-2"><i class="bi bi-trash"></i> Delete</a>
                        </div>
                    `;
                },
                width: "15%"
            }
        ],
        destroy: true // Allows re-initialization of the DataTable
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            // Make api request
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        $('#tblData').DataTable().ajax.reload(null, false); // Reload the table data without resetting the paging
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
