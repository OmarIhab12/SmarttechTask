
$(document).ready(function () {

    $("#productImageSelector").change(function () {
        ChangeDisplayedImage(this);
    });

    $("#createProduct").click(function () {
        CreateProduct();
    });

});

function ChangeDisplayedImage(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#productImage').attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function CreateProduct() {

    if ($.trim($("#Name").val()) == "") {
        $("#Name").siblings(".field-validation-valid").html("The Name field is required.");
    }
    else if (parseFloat($("#Price").val()) != null){
        var product =
        {
            "Name": $("#Name").val(),
            "Price": parseFloat($("#Price").val())
        };

        var photo = document.getElementById('productImageSelector').files[0];

        sendData = new FormData();
        sendData.append("productString", JSON.stringify(product));
        sendData.append("photo", photo);

        $.ajax({
            url: $("#Create").attr("action"),
            method: "POST",
            processData: false,
            contentType: false,
            data: sendData,
            success: function (result) {
                alert("Product added successfully!");
                window.location.href = "/Products/Index";
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(JSON.stringify(xhr) + " " + ajaxOptions + ": " + thrownError);
                alert("Product wasn't added");
            }
        });
    }
}