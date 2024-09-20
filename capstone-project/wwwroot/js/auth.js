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
        var container = $('#predefined-images-container');

        if (container.hasClass('show')) {
            container.removeClass('show');
            setTimeout(function () {
                container.hide();  // Hide after the transition finishes
                // Change the button text after the container is hidden
                $('#toggle-predefined-images').text("Scegli un'immagine");
            }, 500);  // Match the CSS transition duration (0.5s)
        } else {
            container.show();  // Make it visible instantly
            setTimeout(function () {
                container.addClass('show');  // Apply the transition effect
                // Change the button text after the container is shown
                $('#toggle-predefined-images').text('Nascondi');
            }, 10);  // Small delay to ensure CSS transition is triggered
        }
    });

    // Handle image click
    $('.selectable-image').click(function () {
        var selectedImage = $(this).parent().data('image');

        // If the image is already selected, unselect it
        if ($(this).hasClass('selected')) {
            // Unselect image
            $('#SelectedPredefinedImage').val(''); // Clear the hidden input
            $('.selectable-image').removeClass('selected'); // Remove the selected class

            // Show the file upload section and oSpacer again
            $('#upload-image-section').removeClass('d-none');
            $('#oSpacer').removeClass('d-none');

            // Change button text to allow selection again
            $('#toggle-predefined-images').text("Scegli un'immagine");
        } else {
            // Set the selected image value to the hidden input
            $('#SelectedPredefinedImage').val(selectedImage);

            // Hide the pre-defined images section
            $('#predefined-images-container').removeClass('show');
            setTimeout(function () {
                $('#predefined-images-container').hide(); // Hide after transition
            }, 500);  // Match transition duration

            // Hide the file upload section and oSpacer
            $('#upload-image-section').addClass('d-none');
            $('#oSpacer').addClass('d-none');

            // Optionally, highlight the selected image
            $('.selectable-image').removeClass('selected'); // Remove any previous selected class
            $(this).addClass('selected'); // Add selected class to the clicked image

            // Change button text back to 'Scegli un\'immagine'
            $('#toggle-predefined-images').text('Scegli un\'immagine');
        }
    });
});