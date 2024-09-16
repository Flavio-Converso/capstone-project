$(document).ready(function () {
    // Bind the remove from wishlist functionality
    $('.remove-from-wishlist-btn').click(function (e) {
        e.preventDefault();

        var gameId = $(this).data('gameid'); // Get the GameId from the data attribute
        var form = $('#remove-from-wishlist-form-' + gameId); // Get the specific form
        var token = $('input[name="__RequestVerificationToken"]', form).val(); // Get the anti-forgery token

        $.ajax({
            url: '/Wishlist/RemoveFromWishlist',
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                gameId: gameId,
                source: 'Wishlist'
            },
            success: function (response) {
                if (response.success) {
                    // Remove the item row from the wishlist UI
                    $('#wishlist-item-row-' + gameId).remove();

                    console.log("Game removed from the wishlist successfully!");

                    // Check if the wishlist is now empty and update the UI accordingly
                    if ($('tbody').children().length === 0) {
                        $('table').hide();
                        $('h2').hide();
                        $('.btn-secondary').hide();
                        $('p').show().text("Your wishlist is empty.");
                    }

                    // Optionally update the wishlist item count
                    updateWishlistItemCountWishListView();
                } else {
                    console.log("Failed to remove the game from the wishlist.");
                }
            },
            error: function () {
                console.log("Error occurred while removing the game from the wishlist.");
            }
        });
    });

    // Function to update the wishlist item count badge (if applicable)
    function updateWishlistItemCountWishListView() {
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