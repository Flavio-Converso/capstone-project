var token = $('input[name="__RequestVerificationToken"]').val();

// Like button handler
$(document).on('click', '.like-btn', function () {
    var reviewId = $(this).data('review-id');
    $.ajax({
        url: '/Review/LikeReview',
        type: 'POST',
        data: {
            reviewId: reviewId,
            __RequestVerificationToken: token
        },
        success: function (result) {
            if (result.success) {
                // Update the like count
                $('#like-count-' + reviewId).html('<span class="text-orange">' + result.likeCount + '</span> ');

                // Change the button class and icon
                var button = $('[data-review-id="' + reviewId + '"]');
                button.removeClass('like-btn').addClass('unlike-btn');
                button.find('i').removeClass('bi-hand-thumbs-up').addClass('bi-hand-thumbs-up-fill fs-4');
            }
        }
    });
});

// Unlike button handler
$(document).on('click', '.unlike-btn', function () {
    var reviewId = $(this).data('review-id');
    $.ajax({
        url: '/Review/UnlikeReview',
        type: 'POST',
        data: {
            reviewId: reviewId,
            __RequestVerificationToken: token
        },
        success: function (result) {
            if (result.success) {
                // Update the like count
                $('#like-count-' + reviewId).html('<span class="text-orange">' + result.likeCount + '</span> ');

                // Change the button class and icon
                var button = $('[data-review-id="' + reviewId + '"]');
                button.removeClass('unlike-btn').addClass('like-btn');
                button.find('i').removeClass('bi-hand-thumbs-up-fill').addClass('bi-hand-thumbs-up fs-4');
            }
        }
    });
});
