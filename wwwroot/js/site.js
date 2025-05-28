document.querySelector("#About").on('click', function (event) {
    // Prevent default anchor click behavior
    event.preventDefault();
    // Store hash
    var hash = this.hash;

    // Using jQuery's animate() method to add smooth page scroll
    // The optional number (800) specifies the number of milliseconds it takes to scroll to the specified area
    $('html, body').animate({
        scrollTop: $(hash).offset().top
    }, 800, function () {
        // Add hash (#) to URL when done scrolling (default click behaviour) 
        window.location.hash = hash;
    });
});

// Sticky header
$(window).scroll(function () {
    if ($(this).scrollTop() > 1) {
        $('header').addClass('sticky');
    } else {
        $('header').removeClass('sticky');
    }
});