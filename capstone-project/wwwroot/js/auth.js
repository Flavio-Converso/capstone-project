// Handle the first "Prosegui" button click (Step 1)
document.getElementById("continue-btn").addEventListener("click", function () {
    let continueBtn = document.getElementById("continue-btn");

    if (continueBtn.innerText === "Prosegui") {
        // Hide the first form fields (username, password, confirm password, email)
        document.querySelectorAll(".contHide").forEach(function (element) {
            element.classList.add("d-none");
            element.classList.remove("mb-3");
        });

        // Show the second set of form groups (name, surname, birthdate, etc.)
        document.querySelectorAll(".contShow").forEach(function (element) {
            element.classList.remove("d-none");
            element.classList.add("d-flex", "flex-column", "align-items-center");
        });

        // Change button text to "Indietro"
        continueBtn.innerText = "Indietro";
    } else {
        // Show the first form fields (username, password, confirm password, email)
        document.querySelectorAll(".contHide").forEach(function (element) {
            element.classList.remove("d-none");
            element.classList.add("mb-3");
        });

        // Hide the second form groups (name, surname, birthdate, etc.)
        document.querySelectorAll(".contShow").forEach(function (element) {
            element.classList.add("d-none");
            element.classList.remove("d-flex", "flex-column", "align-items-center");
        });

        // Change button text back to "Prosegui"
        continueBtn.innerText = "Prosegui";
    }
});

// Handle the second "Continua" button click (Step 2)
document.getElementById("continue-btn-step2").addEventListener("click", function () {
    let continueBtnStep2 = document.getElementById("continue-btn-step2");

    if (continueBtnStep2.innerText === "Continua") {
        // Hide the second form fields (name, surname, birthdate, etc.)
        document.querySelectorAll(".contShow").forEach(function (element) {
            element.classList.add("d-none");
            element.classList.remove("d-flex", "flex-column", "align-items-center");
        });

        // Show the third set of form fields (image upload, category selection)
        document.querySelectorAll(".contShow2").forEach(function (element) {
            element.classList.remove("d-none");
            element.classList.add("d-flex", "flex-column", "align-items-center");
        });
        document.getElementById("continue-btn").classList.add("d-none")
        document.getElementById("back-btn-step3").classList.remove("d-none")
    }
});

document.getElementById("back-btn-step3").addEventListener("click", function () {
    document.querySelectorAll(".contShow2").forEach(function (element) {
        element.classList.add("d-none");
        element.classList.remove("d-flex", "flex-column", "align-items-center");
    });

    document.querySelectorAll(".contShow").forEach(function (element) {
        element.classList.remove("d-none");
        element.classList.add("d-flex", "flex-column", "align-items-center");
    });

    document.getElementById("continue-btn").classList.remove("d-none")
    document.getElementById("back-btn-step3").classList.add("d-none")
})


$(document).ready(function () {
    // Toggle pre-defined images visibility
    $('#toggle-predefined-images').click(function () {
        $('#predefined-images-container').toggle();

        var isVisible = $('#predefined-images-container').is(':visible');
        $(this).text(isVisible ? 'Hide Predefined Images' : 'Select Predefined Image');
    });

    // Handle image click
    $('.selectable-image').click(function () {
        var selectedImage = $(this).parent().data('image');

        // If the image is already selected, unselect it
        if ($(this).hasClass('selected')) {
            // Unselect image
            $('#SelectedPredefinedImage').val(''); // Clear the hidden input
            $('.selectable-image').removeClass('selected'); // Remove the selected class

            // Show the file upload section again
            $('#upload-image-section').show();

            // Change button text to allow selection again
            $('#toggle-predefined-images').text('Select Predefined Image');
        } else {
            // Set the selected image value to the hidden input
            $('#SelectedPredefinedImage').val(selectedImage);

            // Hide the pre-defined images section
            $('#predefined-images-container').hide();

            // Hide the file upload section
            $('#upload-image-section').hide();

            // Optionally, highlight the selected image
            $('.selectable-image').removeClass('selected'); // Remove any previous selected class
            $(this).addClass('selected'); // Add selected class to the clicked image
        }
    });
});