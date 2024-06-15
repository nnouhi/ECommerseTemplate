var dataTable;

$(document).ready(function () {
    // Get the order status filter from the url
    var urlParams = new URLSearchParams(window.location.search);
    var status = urlParams.get('status');

    loadDataTable(status);

});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: `/admin/order/getall?status=${status}` },
        "columns": [
            { "data": "id", "width": "15%" },
            { "data": "name", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "applicationUser.email", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/admin/order/details?orderHeaderId=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> More</a>
                            </div>`
                },
                "width": "25%"
            }
        ]
    });
}

