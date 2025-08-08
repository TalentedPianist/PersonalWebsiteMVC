

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
        }, 800, function () {
            window.location.hash = hash;
        });
    });

    let isMobile = window.matchMedia("only screen and (max-width: 768px)").matches;

    if (isMobile) {
        $(window).on('scroll', function () {
            if ($(window).scrollTop() >= 400) {
                $('header').addClass('sticky');
                $('header').prependTo('body');
            } else {
                $('header').removeClass('sticky');
                $('header').prependTo('#Home');
            }
        });
    }

});

var onloadCallback = function () {
    grecaptcha.render('g-recaptcha', {
        'site-key': '6LeCBlUrAAAAAGJFT1Rt-4hojR6NfEvqzsvZwnOz '

    });

};




// Load more stuff....
let skip = 1;
const pageSize = 1;
$('#loadMoreBtn').on('click', function (e) {
    e.preventDefault();

    $.get('/Blog/LoadMore', { skip: skip }, function (data) {
        const batch = $('<div class="postBatch"></div>').hide().append(data);
        $("#blogPosts").append(batch);
        batch.slideDown('fast');
        skip = skip + 1;
    });

});

// Show less button...
$('#showLessBtn').on('click', function (e) {
    e.preventDefault();
    const lastBatch = $(".postBatch").last();
    lastBatch.slideUp('fast', function () {
        lastBatch.remove();
        skip -= pageSize;
    });

});



