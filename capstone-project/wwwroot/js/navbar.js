window.addEventListener("scroll", function () {
    // Check if the window width is large or greater (lg+ = 992px and above)
    if (window.innerWidth >= 992) {
        var navbar = document.querySelector(".navbar-gradient");

        if (window.scrollY > 50) {
            navbar.classList.add("navbar-scrolled");
        } else {
            navbar.classList.remove("navbar-scrolled");
        }
    } else {
        // If the screen is smaller than lg, ensure the 'navbar-scrolled' class is removed
        var navbar = document.querySelector(".navbar-gradient");
        navbar.classList.remove("navbar-scrolled");
    }
});



//
document.querySelector('.search-icon').addEventListener('click', function (e) {
    e.preventDefault();  // Prevent default link behavior
    const searchPill = document.querySelector('.search-pill');
    const categoryDiv = document.querySelector('#category');
    const searchInput = document.querySelector('.search-input');

    // Check if the search input is already expanded
    if (!searchPill.classList.contains('expanded')) {
        // First click: expand input and hide category
        categoryDiv.classList.add('hidden'); // Hide category
        searchPill.classList.add('expanded');

        // Show the input and trigger transition
        searchInput.style.display = 'block';
        setTimeout(() => {
            searchInput.style.opacity = '1'; // Make input visible with smooth transition
        }, 10); // Short delay to trigger opacity transition

        searchInput.focus();  // Focus on the input field
    } else {
        // Second click: revert everything back to the initial state
        searchPill.classList.remove('expanded');

        // Hide input smoothly
        searchInput.style.opacity = '0';
        setTimeout(() => {
            searchInput.style.display = 'none'; // Hide the input after transition ends
            categoryDiv.classList.remove('hidden'); // Show category again
        }, 300);  // Match the opacity transition time
    }
});

// Add focusout event to reset the state when the search input loses focus
document.querySelector('.search-input').addEventListener('focusout', function () {
    const searchPill = document.querySelector('.search-pill');
    const categoryDiv = document.querySelector('#category');
    const searchInput = document.querySelector('.search-input');

    // Reset the category visibility and search pill state with transition
    searchPill.classList.remove('expanded');

    // Hide input smoothly
    searchInput.style.opacity = '0';
    setTimeout(() => {
        searchInput.style.display = 'none'; // Hide the input after transition
        categoryDiv.classList.remove('hidden'); // Show category again
    }, 300);  // Match the opacity transition time
});