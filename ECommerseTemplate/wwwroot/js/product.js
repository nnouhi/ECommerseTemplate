$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblData').DataTable({
        ajax: "/admin/product/getall",
        columns: [
            { data: 'title', width: '25%' },
            { data: 'isbn', width: '15%' },
            { data: 'listPrice', width: '10%' }, 
            { data: 'author', width: '20%' }, 
            { data: 'category.name', width: '15%' },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil"></i> Edit</a>
                            <a href="/Admin/Product/Delete?id=${data}" class="btn btn-danger mx-2"><i class="bi bi-trash"></i> Delete</a>
                        </div>
                    `;
                },
                "width": "15%"
            }
        ]
    });
}
