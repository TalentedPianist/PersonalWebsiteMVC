
// Sticky header
$(window).scroll(function () {
    console.log($(this).scrollTop());
    if ($(this).scrollTop() > 100) {
        $('header').addClass('sticky');
        $("header").prependTo("body");
    } else {
        $('header').removeClass('sticky');
        $("header").prependTo(".overlay");
    }
});

window.ScrollToView = (name) => { 
    $('html, body').animate({ 
        scrollTop: $("#" + name).offset().top
    }, 200);
}

window.scrollToHash = () => { 
    var hash = window.location.hash;
    if (hash && $(hash).length)  {
        $('html,body').animate({ 
            scrollTop: $(hash).offset().top
        }, 900, 'swing');
    }
}
