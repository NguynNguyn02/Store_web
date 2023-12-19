$(document).ready(function () {
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var Quantity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            Quantity = parseInt(tQuantity);
        }
        alert(id + " " + Quantity);
    })
})