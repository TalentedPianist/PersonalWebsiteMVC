


window.blazorHelpers = {
    scrollToFragment: (elementId) => {
        var element = document.getElementById(elementId);

        if (element) {
            element.scrollIntoView({
                behavior: 'smooth'
            });
        }
    }
};

window.goBack = () => {
    return history.back();
}


//Sticky header - needs a media query in JS else it will mess up the desktop view!!
if (window.matchMedia('(max-width: 600px)').matches) {
    $(window).on('scroll', function () {

        if ($(this).scrollTop() > 100) {
            $('header').addClass('sticky');
            $('header').appendTo('body');
        } else {

        }
    });
}

window.ScrollToView = (name) => {

    $('html, body').animate({
        scrollTop: $("#" + name).offset().top
    }, 200);
}

window.scrollToHash = () => {
    var hash = window.location.hash;
    if (hash && $(hash).length) {
        $('html,body').animate({
            scrollTop: $(hash).offset().top
        }, 900, 'swing');
    }
}

window.scrollIntoView = (elementId) => {
    var elem = document.getElementById(elementId);
    if (elem) {
        window.location.reload();
        elem.scrollIntoView();
        window.location.hash = elementId;
    }
}

// jQuery functionality
window.initializeJQuery = () => {
    $("form").on("submit", function (e) {
        e.preventDefault();
        console.log("Form was submitted!");
    });

    $(".gallery").featherlightGallery({
        afterOpen: async function (event) {
            var commentForm = "";
            $.ajax({
                method: "GET",
                url: "/PhotoCommentForm",
                async: false,
                cache: false,
                success: function (message) {
                    console.log(message);
                    
                },
                error: function (error) {
                    console.log(error);
                }
            });
            
            $(this.$content).after(`
           
            `);

        }
    });
}

window.openFeatherlight = () => {
};

