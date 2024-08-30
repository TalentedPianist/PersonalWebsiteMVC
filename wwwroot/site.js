$(function () {

   

    $("#menu a").on('click', function (e) {
        $("header").show();
    
        // User has clicked navbar link, perform smooth scrolling

        if (this.hash !== "") {
         
        
            

            // Store hash
            var hash = this.hash;

            // Using jQuery's animate() method to add smooth page scroll
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 800, function () {
                // Add hash (#) to the URL when done scrolling (default click behaviour) 
                window.location.hash = hash;
                $("header").removeClass("sticky");
                $(".modal").modal('hide');
            });
        }// End if

    });


    window.addEventListener("scroll", () => {
        // https://webdesign.tutsplus.com/create-an-animated-sticky-header-on-scroll-with-a-bit-of-javascript--cms-93428t
        const currentScroll = window.pageYOffset;
        var header = document.querySelector("header");
        if (header) {
            if (currentScroll > 150) {
                $("header").prependTo("body");
                document.querySelector("header").classList.add("sticky");
            } else {
                document.querySelector("header").classList.remove("sticky");
                $("header").prependTo("#Home section");
            }
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

    // Begin comments slice for mobile view
    $("#Comments section div").not(':eq(0)').hide();
    $("#Comments section div").slice(0, 1);
    let y = 2; 
    $(".loadMoreComments").on('click', function (e) {
        e.preventDefault();
        console.log("Button was clicked");
        y = y + 1; 
        $("#Comments section div").slice(0, y).show();
    });
    // End comments slice for mobile view

    $("#singlePost").on('click', function (e) {
        //e.preventDefault();
        console.log("Link was clicked");
    });


    ClassicEditor
        .create(document.querySelector("#ckeditor1"), {
            // To make editor typable, the plugins[] bit needs to be taken out in spie of documentation.
            toolbar: [],
        })
        .then(editor => {
            editor = editor;
            editor.setData('<p>Blah blah blah</p>');
        })
        .catch(error => {
            console.error(error);
        });


    $("header a").on('click', function (e) {
        e.preventDefault();
        $("header").hide();
        $("#menu").modal('show');
    });
});

