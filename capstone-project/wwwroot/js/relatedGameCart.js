//relatedcart
$(document).ready(function () {
    // New Add to Cart functionality for related games
    $('.related-add-to-cart-btn').click(function (e) {
        e.preventDefault(); // Prevent the default form submission

        var button = $(this); // Store the clicked button
        var gameId = button.data('gameid'); // Get the Game ID
        var token = $('input[name="__RequestVerificationToken"]').val(); // Get the Anti-Forgery Token

        // Perform AJAX request to add the game to the cart
        $.ajax({
            url: '/Cart/AddRelatedGameToCart', // New Action for adding related games to the cart
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                gameId: gameId
            },
            success: function (response) {
                if (response.success) {
                    // Update cart item count dynamically
                    relupdateCartItemCount();

                    // Remove the added game from the related games list
                    $('#related-game-row-' + gameId).remove();

                    console.log("Related game added to the cart successfully!");
                } else {
                    console.log("Failed to add the game to the cart.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error occurred while adding the related game to the cart.");
            }
        });
    });

    // Function to update the cart item count badge
    function relupdateCartItemCount() {
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
});