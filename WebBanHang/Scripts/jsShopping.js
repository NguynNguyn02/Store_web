$(document).ready(function () {
    ShowCount()
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var Quantity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            Quantity = parseInt(tQuantity);
        }

        $.ajax({
            url: '/shoppingcart/addtocart',
            type: 'POST',
            data: { id: id, quantity: Quantity },
            success: function (rs) {
                if (rs.success) {
                    $('#checkout_items').html(rs.Count);
                    alert(rs.msg);
                }
            }

        })


    })
    $('body').on('click', '.btnDelete', function () {
        var id = $(this).data('id');
        
        $.ajax({
            url: '/shoppingcart/delete',
            type: 'POST',
            data: { id: id },
            success: function (rs) {
                if (rs.success) { 
                    $('#trow_' + id).remove();
                    $('#checkout_items').html(rs.Count);
                    LoadCart();
                }
            }
        })

    })
    $('body').on('click', '.btnUpdate', function () {
        var id = $(this).data('id');
        var quantity = $('#Quantity_' + id).val();
        Update(id, quantity);
    })
    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault;
        var conf = confirm('Bạn có chắc muốn xóa hết giỏ hàng?');
        if (conf == true) {
            DeleteAll();
        }
        
    })
});
function ShowCount() {
    $.ajax({
        url: '/shoppingcart/ShowCount',
        type: 'GET',
        success: function (rs) {
            $('#checkout_items').html(rs.Count);
        }
    })
}
function DeleteAll() {
    $.ajax({
        url: '/shoppingcart/DeleteAll',
        type: 'POST',
        success: function (rs) {
            if (rs.success) {
                LoadCart();
            }
        }
    })
}
function LoadCart() {
    $.ajax({
        url: '/shoppingcart/Partial_Item_Cart',
        type: 'GET',
        success: function (rs) {
            $('#load_data').html(rs);
        }
    })
}
function Update(id,quantity) {
    $.ajax({
        url: '/shoppingcart/Update',
        type: 'POST',
        data: { id: id, quantity: quantity },
        success: function (rs) {
            if (rs.success) {
                LoadCart();
            }
        }
    })
}




