


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


//Sticky header
$(window).on('scroll', function () {
    if ($(this).scrollTop() > 100) { 
        $('header').addClass('sticky');
        $('header').appendTo('body');
    } else {
        
    }    
});

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


