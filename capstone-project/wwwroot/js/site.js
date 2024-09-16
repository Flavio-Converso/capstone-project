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
$(document).ready(function () {
    $.ajax({
        url: '/Cart/GetCartItemCount', // Adjust the URL as needed based on your controller
        method: 'GET',
        success: function (response) {
            if (response.success && response.count > 0) {
                // Display the cart item count in the badge
                $('#cart-item-count').text(response.count);
                $('#cart-item-count').show(); // Show the badge only if there are items
            } else {
                $('#cart-item-count').hide(); // Hide the badge if no items
            }
        },
        error: function () {
            console.log('Error fetching cart item count');
        }
    });
});
$(document).ready(function () {
    $.ajax({
        url: '/Wishlist/GetWishlistItemCount', // Adjust the URL if necessary
        method: 'GET',
        success: function (response) {
            if (response.success && response.count > 0) {
                // Update the wishlist item count badge
                $('#wishlist-item-count').text(response.count);
                $('#wishlist-item-count').show(); // Show the badge if there are items
            } else {
                $('#wishlist-item-count').hide(); // Hide the badge if no items
            }
        },
        error: function () {
            console.log('Error fetching wishlist item count');
        }
    });
});
