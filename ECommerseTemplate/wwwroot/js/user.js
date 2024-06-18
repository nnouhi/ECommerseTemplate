﻿var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            url: "/admin/user/getall",
            type: "GET",
            datatype: "json"
        },
        columns: [
            { data: 'name', width: '15%' },
            { data: 'email', width: '15%' },
            { data: 'phoneNumber', width: '15%' },
            { data: 'company.name', width: '15%' },
            { data: 'role', width: '15%' },
            {
                data: { id: 'id', lockoutEnd: 'lockoutEnd' },
                render: function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    var isLocked = lockout > today;
                    
                    if (isLocked) {
                        return `
                          <div class="text-center">
                                  <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                        <i class="bi bi-unlock-fill"></i>  Unlock
                                    </a>
                                    <a href="/admin/user/RoleManagement?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
                                         <i class="bi bi-pencil-square"></i> Permission
                                    </a>
                            </div> 
                        `;
                    } else {
                        return `
                            <div class="text-center">
                                 <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                        <i class="bi bi-lock-fill"></i>  Lock
                                    </a> 
                                    <a href="/admin/user/RoleManagement?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
                                         <i class="bi bi-pencil-square"></i> Permission
                                    </a>
                            </div>
                        `;
                    }
                },
                width: "25%"
            }
        ],
        destroy: true // Allows re-initialization of the DataTable
    });
}

function LockUnlock(id) {
    console.log(id)
    $.ajax({
        type: "POST",
        url: '/admin/user/lockunlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();

            }
        }
    });
}
