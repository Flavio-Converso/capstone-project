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

document.addEventListener("DOMContentLoaded", function () {
    // Check if scrollToReviewForm or scrollToReviewDeleted exists in the URL query string
    const urlParams = new URLSearchParams(window.location.search);
    const scrollToReviewForm = urlParams.get('scrollToReviewForm');
    const scrollToReviewDeleted = urlParams.get('scrollToReviewDeleted');
    const reviewId = urlParams.get('reviewId'); // From previous logic for review scroll

    // Scroll to the review form if the scrollToReviewForm parameter is true
    if (scrollToReviewForm) {
        const reviewFormElement = document.getElementById('reviewForm'); // Assuming this is the ID of your review form element
        if (reviewFormElement) {
            const elementPosition = reviewFormElement.getBoundingClientRect().top + window.scrollY;
            window.scrollTo({
                top: elementPosition - 240, // Adjust for offset (-240px)
                behavior: 'smooth'
            });
        }
    }

    // Scroll to the "reviewDeleted" section if the scrollToReviewDeleted parameter is true
    if (scrollToReviewDeleted) {
        const reviewDeletedElement = document.getElementById('reviewDeleted'); // Assuming this is the ID of your h2 element
        if (reviewDeletedElement) {
            const elementPosition = reviewDeletedElement.getBoundingClientRect().top + window.scrollY;
            window.scrollTo({
                top: elementPosition - 240, // Adjust for offset (-240px)
                behavior: 'smooth'
            });
        }
    }

    // Existing logic for scrolling to a specific review
    if (reviewId) {
        const reviewElement = document.getElementById('review-' + reviewId);
        if (reviewElement) {
            const elementPosition = reviewElement.getBoundingClientRect().top + window.scrollY;
            window.scrollTo({
                top: elementPosition - 240, // Adjust the offset here (-240px)
                behavior: 'smooth'
            });
        }
    }
});