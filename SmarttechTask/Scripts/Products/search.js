$(document).ready(function () {

    $("#search").click(function () {
        Search();
    });

});

function Search() {

    sendData = new FormData();
    sendData.append("searchString", JSON.stringify("o"));

    $.ajax({
        url: $("#Index").attr("action"),
        method: "get",
        data: sendData,
        success: function (result) {
            alert("Product added successfully!");
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(JSON.stringify(xhr) + " " + ajaxOptions + ": " + thrownError);
            alert("Product wasn't added");
        }
    });
}