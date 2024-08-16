$(function () {

    $('#menu').on('shown.bs.modal', function () {
        $("header svg").css("display", "none");
    });

    $('#menu').on('hide.bs.modal', function () {
        $("header svg").css("display", "flex");
    });

    $("#menu a").on('click', function (e) {
        e.preventDefault();
        $(".modal").modal('hide');
        // User has clicked navbar link, perform smooth scrolling
        if (this.hash !== "") {
            e.preventDefault();
            // Store hash
            var hash = this.hash;

            // Using jQuery's animate() method to add smooth page scroll
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 800, function () {
                // Add hash (#) to the URL when done scrolling (default click behaviour) 
                window.location.hash = hash;
            });
        }// End if

    });

   
    window.addEventListener("scroll", () => {
        // https://webdesign.tutsplus.com/create-an-animated-sticky-header-on-scroll-with-a-bit-of-javascript--cms-93428t
        const currentScroll = window.pageYOffset;
        if (currentScroll > 150) {
            document.querySelector("header").classList.add("sticky");
        } else {
            document.querySelector("header").classList.remove("sticky");
        }
    });

    // Begin blog card slice
    $(".card").not(':eq(0)').hide();
    $(".card").slice(0, 1);
    $("#Blog").css("height", "100vh");
    let x = 1;
    $(".loadMorePosts").on('click', function (e) {
        e.preventDefault();
        $("#Blog").css("height", "100%");
        x = x + 1;
        $(".card").css('flex-direction', 'column');
        $(".card").slice(0, x).show(); // Show x amount of images
    });
    // End blog card slice

    $("#singlePost").on('click', function (e) {
        //e.preventDefault();
        console.log("Link was clicked");
    });
});

