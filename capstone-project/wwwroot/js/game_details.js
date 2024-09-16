$(document).on('click', '.like-button', function (e) {
    e.preventDefault();

    var button = $(this);
    var reviewId = button.data('review-id');
    var liked = button.data('liked') === 'true';

    $.ajax({
        url: '@Url.Action("LikeReview", "Review")',
        type: 'POST',
        data: {
            reviewId: reviewId,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            if (response.success) {
                // Update like count
                $('#like-count-' + reviewId).text(response.likeCount);

                // Hide the placeholder text for this specific review
                $('#plchld-' + reviewId).addClass('display-none');

                // Toggle button text and color
                if (response.liked) {
                    button.text('Unlike');
                    button.removeClass('btn-success').addClass('btn-danger');
                    button.data('liked', 'true');
                } else {
                    button.text('Like');
                    button.removeClass('btn-danger').addClass('btn-success');
                    button.data('liked', 'false');
                }
            }
        },
        error: function () {
            alert('An error occurred while processing your request. Please try again.');
        }
    });
});

