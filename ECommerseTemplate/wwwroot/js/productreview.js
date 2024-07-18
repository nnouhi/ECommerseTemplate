$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblData').DataTable({
        ajax: {
            url: "/admin/productreview/getall",
            type: "GET",
            datatype: "json",
            succuss: function (data) { console.log(data) }
        },
        columns: [
            { data: 'name', width: '20%' },
            { data: 'rating', width: '20%' },
            { data: 'email', width: '20%' },
            { data: 'title', width: '20%' },
            {
                data: 'review',
                width: '20%',
                render: function (data) {
                    // Truncate the review text and use ellipsis if it's too long
                    let truncatedReview = data.length > 50 ? data.substring(0, 50) + '...' : data;
                    return `<span title="${data}">${truncatedReview}</span>`;
                }
            },
            { data: 'product.title', width: '20%' },
            {
                data: 'images',
                width: '20%',
                render: function (images) {
                    const baseUrl = "https://localhost:7080/"; // Will need to be adjusted

                    // Create clickable links for images, adjusting URLs to remove "/Admin"
                    let imgLinks = images.map(imgPath => {
                        // Remove "/Admin" from the path
                        let adjustedPath = imgPath.replace('/Admin', '');

                        return `<a href="${baseUrl}${adjustedPath}" target="_blank">Image
                                </a>`;
                    }).join('');

                    return imgLinks;
                }
            },
            {
                data: null,
                render: function (data) {
                    let replyBtnText = data.reply ? "Edit Reply" : "Reply";
                    if (data.isAdminApproved) {
                        return `
                            <div class="w-75 btn-group" role="group">
                                <a href="/admin/productreview/verifyreview?id=${data.id}&verify=false" class="btn btn-danger mx-2"><i class="bi bi-eye-slash-fill"></i> Hide</a>
                                <a href="/admin/productreview/reply?id=${data.id}" class="btn btn-info mx-2"><i class="bi bi-reply-fill"></i> ${replyBtnText}</a>
                            </div>
                        `;
                    } else {
                        return `
                            <div class="w-75 btn-group" role="group">
                                <a href="/admin/productreview/verifyreview?id=${data.id}&verify=true" class="btn btn-primary mx-2"><i class="bi bi-eye-fill"></i> Verify</a>
                                <a href="/admin/productreview/reply?id=${data.id}" class="btn btn-info mx-2"><i class="bi bi-reply-fill"></i> ${replyBtnText} </a>
                            </div>
                        `;
                    }
                },
                width: "15%"
            }
        ],
        destroy: true // Allows re-initialization of the DataTable
    });
}
