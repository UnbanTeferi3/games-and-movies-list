var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({

        

        "ajax": {
            "url": "/videoGames/getall/",
            "type": "GET",
            "datatype": "json"
        },

        //"order": [[3, "desc"]],
        

        "columns": [
            { "data": "name"},
            { "data": "company" },
            { "data": "director" },
            { "data": "system"},
            { "data": "yearReleased"},
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/VideoGames/Upsert?id=${data}" class='btn btn-info text-white text-center' style='margin: 10px; cursor:pointer; width:70px;'>
                            Edit
                        </a>                       
                        <a class='btn btn-danger text-white text-center' style='margin: 10px; cursor:pointer; width:70px;'
                            onclick=Delete('/videoGames/Delete?id='+${data})>
                            Delete
                        </a>
                        </div>`;
                }//, "width": "32%"
            }
        ],

        "responsive": true,
        
        "columnDefs": [
            { "orderable": false, "targets": 5 },
            { "responsivePriority": 1, "targets": 0 },
            { "responsivePriority": 2, "targets": 3 }
        ],

        "language": {
            "emptyTable": "no data found"
        },
        //"width": "100%"
    });
}

function CheckLimit(url) {

    //console.log("CheckLimit is executing");
    
    //console.log(videoGameId);


    //let limitReached = false;
    let count = 0;

    if (videoGameId == 0) {

        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                count = data.count;
                //console.log("Total count is " + count);

                if (count > 19) {

                    swal({
                        title: "You have reached the maximum list amount!",
                        text: "Please delete a row and try again.",
                        icon: "warning",
                        button: "OK",
                        dangerMode: false
                    })

                }
                else if (count <= 19) {

                    document.getElementById("form_id").submit();

                }
            }
        });

    }
    else if (videoGameId != 0) {

        document.getElementById("form_id").submit();

    }

    

}

function Delete(url) {

    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover the data.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
   
}