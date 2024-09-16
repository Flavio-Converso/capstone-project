
//profile image of logged in user (navbar)
$(document).ready(function () {
    $.ajax({
        url: '/User/GetProfileImage',
        method: 'GET',
        success: function (response) {
            if (response.success && response.image) {
                // Correctly set the Base64 image in the img tag
                $('#profile-image-nav').attr('src', 'data:image/jpeg;base64,' + response.image);
            } else {
                console.log(response.message); // Log if there's no image
            }
        },
        error: function () {
            console.log('Error fetching profile image');
        }
    });
});


//wishlist
$(document).ready(function () {
    // Update wishlist item count when the page loads
    updateWishlistItemCount();

    // Add to wishlist functionality
    $('#add-to-wishlist-btn').click(function (e) {
        e.preventDefault();

        var form = $('#add-to-wishlist-form');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        var gameId = $('input[name="gameId"]', form).val();
        var source = $('input[name="source"]', form).val();

        $.ajax({
            url: '/Wishlist/AddToWishlist',
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                gameId: gameId,
                source: source
            },
            success: function (response) {
                if (response.success) {
                    // Replace with remove from wishlist form and include Anti-Forgery token
                    var tokenField = `<input type="hidden" name="__RequestVerificationToken" value="${token}" />`;
                    $('#add-to-wishlist-form').replaceWith(`
                        <form id="remove-from-wishlist-form" class="me-3">
                            ${tokenField}
                            <input type="hidden" name="gameId" value="${gameId}" />
                            <input type="hidden" name="source" value="${source}" />
                            <button type="button" id="remove-from-wishlist-btn" class="auth-btn fs-4">
                                <i class="bi bi-suit-heart-fill"></i>
                            </button>
                        </form>
                    `);

                    // Bind the remove functionality to the new button
                    bindRemoveFromWishlist();

                    // Update the wishlist item count badge
                    updateWishlistItemCount();
                } else {
                    console.log("Failed to add the game to the wishlist.");
                }
            },
            error: function () {
                console.log("Error occurred while adding the game to the wishlist.");
            }
        });
    });

    // Remove from wishlist functionality
    function bindRemoveFromWishlist() {
        $('#remove-from-wishlist-btn').click(function (e) {
            e.preventDefault();

            var form = $('#remove-from-wishlist-form');
            var token = $('input[name="__RequestVerificationToken"]', form).val();
            var gameId = $('input[name="gameId"]', form).val();
            var source = $('input[name="source"]', form).val();

            $.ajax({
                url: '/Wishlist/RemoveFromWishlist',
                type: 'POST',
                data: {
                    __RequestVerificationToken: token,
                    gameId: gameId,
                    source: source
                },
                success: function (response) {
                    if (response.success) {
                        // Replace with add to wishlist form and include Anti-Forgery token
                        var tokenField = `<input type="hidden" name="__RequestVerificationToken" value="${token}" />`;
                        $('#remove-from-wishlist-form').replaceWith(`
                            <form id="add-to-wishlist-form" class="me-3">
                                ${tokenField}
                                <input type="hidden" name="gameId" value="${gameId}" />
                                <input type="hidden" name="source" value="${source}" />
                                <button type="button" id="add-to-wishlist-btn" class="auth-btn fs-4">
                                    <i class="bi bi-suit-heart"></i>
                                </button>
                            </form>
                        `);

                        // Bind the add functionality to the new button
                        bindAddToWishlist();

                        // Update the wishlist item count badge
                        updateWishlistItemCount();
                    } else {
                        console.log("Failed to remove the game from the wishlist.");
                    }
                },
                error: function () {
                    console.log("Error occurred while removing the game from the wishlist.");
                }
            });
        });
    }

    // Function to bind the add to wishlist functionality
    function bindAddToWishlist() {
        $('#add-to-wishlist-btn').click(function (e) {
            e.preventDefault();

            var form = $('#add-to-wishlist-form');
            var token = $('input[name="__RequestVerificationToken"]', form).val();
            var gameId = $('input[name="gameId"]', form).val();
            var source = $('input[name="source"]', form).val();

            $.ajax({
                url: '/Wishlist/AddToWishlist',
                type: 'POST',
                data: {
                    __RequestVerificationToken: token,
                    gameId: gameId,
                    source: source
                },
                success: function (response) {
                    if (response.success) {
                        var tokenField = `<input type="hidden" name="__RequestVerificationToken" value="${token}" />`;
                        $('#add-to-wishlist-form').replaceWith(`
                            <form id="remove-from-wishlist-form" class="me-3">
                                ${tokenField}
                                <input type="hidden" name="gameId" value="${gameId}" />
                                <input type="hidden" name="source" value="${source}" />
                                <button type="button" id="remove-from-wishlist-btn" class="auth-btn fs-4">
                                    <i class="bi bi-suit-heart-fill"></i>
                                </button>
                            </form>
                        `);

                        bindRemoveFromWishlist();
                        updateWishlistItemCount();
                    } else {
                        console.log("Failed to add the game to the wishlist.");
                    }
                },
                error: function () {
                    console.log("Error occurred while adding the game to the wishlist.");
                }
            });
        });
    }

    // Initially bind the remove from wishlist functionality if the button exists
    if ($('#remove-from-wishlist-btn').length) {
        bindRemoveFromWishlist();
    }

    // Function to update wishlist item count badge
    function updateWishlistItemCount() {
        $.ajax({
            url: '/Wishlist/GetWishlistItemCount',
            method: 'GET',
            success: function (response) {
                if (response.success && response.count > 0) {
                    $('#wishlist-item-count').text(response.count);
                    $('#wishlist-item-count').show();
                } else {
                    $('#wishlist-item-count').hide();
                }
            },
            error: function () {
                console.log('Error fetching wishlist item count');
            }
        });
    }
});
//cart
$(document).ready(function () {
    updateCartItemCount();

    // Add to cart functionality
    $('#add-to-cart-btn').click(function (e) {
        e.preventDefault(); // Prevent the form from submitting traditionally

        var form = $('#add-to-cart-form');
        var token = $('input[name="__RequestVerificationToken"]', form).val(); // Get the anti-forgery token
        var gameId = $('input[name="gameId"]', form).val(); // Get the game ID
        var source = $('input[name="source"]', form).val(); // Get the source value

        // Perform AJAX request to add the game to the cart
        $.ajax({
            url: '/Cart/AddToCart',
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                gameId: gameId,
                source: source
            },
            success: function (response) {
                if (response.success) {
                    // Update the button to "Nel carrello" and disable it
                    $('#add-to-cart-btn').replaceWith(`
                        <button class="cart-btn mx-2 fs-4 ms-2" disabled>
                            <i class="bi bi-cart-check-fill pe-2"></i> Nel carrello
                        </button>
                    `);

                    // Update the cart item count badge
                    updateCartItemCount();

                    console.log("Game added to the cart successfully!");
                } else {
                    console.log("Failed to add the game to the cart.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error occurred while adding the game to the cart.");
                console.log(xhr.responseText);
            }
        });
    });

    // Function to handle removing items from the cart
    $('.remove-from-cart-btn').click(function (e) {
        e.preventDefault(); // Prevent the default form submission

        var gameId = $(this).data('gameid'); // Get the GameId from the data attribute
        var form = $('#remove-from-cart-form-' + gameId); // Get the specific form
        var token = $('input[name="__RequestVerificationToken"]', form).val(); // Get the anti-forgery token

        $.ajax({
            url: '/Cart/RemoveFromCart', // URL to handle the remove functionality
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                gameId: gameId,
                source: 'Cart'
            },
            success: function (response) {
                if (response.success) {
                    // Remove the item row from the cart UI
                    $('#cart-item-row-' + gameId).remove();

                    // Update the cart item count badge
                    updateCartItemCount();

                    // Update the total price in the UI
                    $('#total-price').text(response.cartTotal);

                    console.log("Game removed from the cart successfully!");

                    // Check if the cart is now empty and update the UI accordingly
                    if ($('tbody').children().length === 0) {
                        $('table').hide();
                        $('h3').hide();
                        $('.btn-success').hide();
                        $('p').show().text("Your cart is empty.");
                    }
                } else {
                    console.log("Failed to remove the game from the cart.");
                }
            },
            error: function () {
                console.log("Error occurred while removing the game from the cart.");
            }
        });
    });

    // Function to handle quantity updates
    function updateQuantity(button, increment, gameId) {
        const input = $('#quantity-' + gameId);
        const currentQuantity = parseInt(input.val(), 10);
        const maxQuantity = parseInt(input.attr('data-max-quantity'), 10);
        const newQuantity = currentQuantity + increment;

        if (newQuantity >= 1 && newQuantity <= maxQuantity) {
            input.val(newQuantity); // Update the quantity input value

            // Send an AJAX request to update the quantity in the backend
            $.ajax({
                url: '/Cart/UpdateCartItemQuantity', // Adjust the URL as per your controller action
                type: 'POST',
                data: {
                    gameId: gameId,
                    quantity: newQuantity,
                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() // Anti-forgery token
                },
                success: function (response) {
                    if (response.success) {
                        // Update the total price for this item
                        $('#total-' + gameId).text(response.itemTotal);

                        // Update the total price of the cart
                        $('#total-price').text(response.cartTotal);

                        // Update the cart item count badge
                        updateCartItemCount();
                    } else {
                        alert("Failed to update the cart.");
                    }
                },
                error: function () {
                    alert("Error occurred while updating the cart item.");
                }
            });
        }
    }

    // Attach click event handlers for quantity buttons
    $('.btn-increment').click(function () {
        var gameId = $(this).data('gameid');
        updateQuantity(this, 1, gameId);
    });

    $('.btn-decrement').click(function () {
        var gameId = $(this).data('gameid');
        updateQuantity(this, -1, gameId);
    });

    // Function to update the cart item count badge
    function updateCartItemCount() {
        $.ajax({
            url: '/Cart/GetCartItemCount',
            method: 'GET',
            success: function (response) {
                if (response.success && response.count > 0) {
                    $('#cart-item-count').text(response.count);
                    $('#cart-item-count').show();
                } else {
                    $('#cart-item-count').hide();
                }
            },
            error: function () {
                console.log('Error fetching cart item count');
            }
        });
    }

    // Function to update total price (optional)
    function updateTotalPrice() {
        // Example implementation: retrieve total price via AJAX or calculate from the DOM
        $.ajax({
            url: '/Cart/GetTotalPrice',
            method: 'GET',
            success: function (response) {
                if (response.success) {
                    $('h3').text("Total: " + response.totalPrice);
                    console.log("Total price updated"); // Debugging
                }
            },
            error: function () {
                console.log('Error fetching total price');
            }
        });
    }
});
