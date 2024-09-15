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
