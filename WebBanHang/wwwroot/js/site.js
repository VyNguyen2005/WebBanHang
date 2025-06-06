$(document).ready(function () {
    showQuantityCart();
});
let showQuantityCart = () => {
    $.ajax({
        url: "/Customer/Cart/GetQuantityOfCart",
        success: function (data) {
            $(".showcart").text(data.quantity);
        }
    });
}

$(document).on("click", ".addtocart", function (evt) {
    evt.preventDefault();
    let id = $(this).attr("data-productId");
    $.ajax({
        url: "/Customer/Cart/AddToCartApi",
        data: { "productId": id },
        success: function (data) {
            Swal.fire({
                title: "Product added to cart",
                icon: "success"
            });
            showQuantityCart();
        }
    });
})

$(document).on("click", ".btn-increase", function () {
    let container = $(this).closest(".cart-item");
    let quantitySpan = container.find(".quantity-display");

    let currentQuantity = parseInt(quantitySpan.attr("data-quantity"));
    let newQuantity = currentQuantity + 1;

    quantitySpan.attr("data-quantity", newQuantity).text(newQuantity);
});

$(document).on("click", ".btn-decrease", function () {
    let container = $(this).closest(".cart-item");
    let quantitySpan = container.find(".quantity-display");

    let currentQuantity = parseInt(quantitySpan.attr("data-quantity"));

    if (currentQuantity > 1) {
        let newQuantity = currentQuantity - 1;
        quantitySpan.attr("data-quantity", newQuantity).text(newQuantity); 
    }
});

$(document).on("click", ".update-cart", function () {
    const container = $(this).closest(".cart-item");

    const productId = $(this).attr("data-productid");

    const quantity = container.find(".quantity-display").attr("data-quantity");

    $.ajax({
        type: "POST",
        url: "/Customer/Cart/UpdateCartItemAPI",
        data: {
            productId: productId,
            quantity: quantity
        },
        success: function (res) {
            Swal.fire({
                title: "The cart has been updated!",
                icon: "success"
            });

            $(".showcart").text(res.quantity);

            $(".total-price-text").text(`$${res.total}`);
        }
    });
});

