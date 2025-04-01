function loadDataTable() {
    $("#Product-table").DataTable({
        ajax: { url: '/Products/get' },
        columns: [
            { data: 'serialNumber', width: '45%' },
            { data: 'brand', width: '45%' },
            { data: 'model', width: '45%' },
            {
                data: 'id',
                render: function (data) {
                    return `<div class="w-100 btn-group" role="group">
                        <a href="/Product/update?id=${data}" class="btn btn-primary mx-2">
                            <i class="bi bi-pencil-square"></i> Edit
                        </a>
                        <form method="post" action="/Product/delete" onsubmit="return confirm('Do you really want to delete Product @obj.FirstName');">
                            <input type="hidden" name="Id" value="${data}" />
                            <button type="submit" class="btn btn-danger">
                                <i class="bi-trash"></i>Delete
                            </button>
                        </form>
                    </div>`;
                },
                width: '10%'
            }
        ]
       }
    );
}

$(document).ready(function () {
    loadDataTable();
});