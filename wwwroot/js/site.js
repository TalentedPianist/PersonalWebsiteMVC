
// Sticky header
$(window).scroll(function () {

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

window.scrollIntoView = (elementId) => {
    var elem = document.getElementById(elementId);
    if (elem) { 
        window.location.reload();
        elem.scrollIntoView();
        window.location.hash = elementId;
    } 
}

