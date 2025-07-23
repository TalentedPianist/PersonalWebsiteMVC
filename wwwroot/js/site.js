
const dialog = document.getElementById("myNav");

function showDialog() {
    dialog.showModal();
}

function closeDialog() {
    dialog.close();
}


$(function () {
    $(".mobileNav a").on('click', function (event) {
        event.preventDefault();
        closeDialog();
        
        // Store hash
        var hash = this.hash;
        // Use jQuery's animate() method to add smooth scroll
        $('html, body').animate({ 
            scrollTop: $(hash).offset().top
        }, 800, function() { 
            window.location.hash = hash;
        });
    });


    $(window).on('scroll', function() { 
        if ($(window).scrollTop() >= 400) { 
            $('header').addClass('sticky');
            $('header').prependTo('body');
        } else { 
            $('header').removeClass('sticky');
            $('header').prependTo('#Home');
        }
    });

});