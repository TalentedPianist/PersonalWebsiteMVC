

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
                //$('header').prependTo('body');
            } else {
                $('header').removeClass('sticky');
                //$('header').prependTo('#Home');
            }
        });
    }

});

var onloadCallback = function () {
    if (document.querySelector('#g-recaptcha')) {
        grecaptcha.render('g-recaptcha', {
            'site-key': '6LeCBlUrAAAAAGJFT1Rt-4hojR6NfEvqzsvZwnOz'

        });
    }
};




// Load more stuff....
let skip = 3;
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



// Specifically for search box stuff
$("#searchLink").featherlight({
    targetAttr: 'href',
    beforeContent: function (e) {
        let instance = this.$instance[0];
        $(".featherlight-content").addClass("searchPopup");
        let searchPopup = $(".featherlight-content").find('.searchPopup');
        $(searchPopup).css('display', 'flex');
    },
    afterContent: function (e) {
        let instance = this.$instance[0];
        let search = instance.querySelector("#searchInput");
        let results = instance.querySelector("#results");

        search.addEventListener("keyup", delay(function (f) {
            console.log(f.target.value);
            // Use Ajax to send search term to server and process result
            $.ajax({
                method: "POST",
                url: "/Search",
                data: { Term: f.target.value },
                dataType: 'text',
                async: false,
                cache: false,
                success: function (message) {
                    $(results).html(message);
                },
                error: function (error) {
                    $(results).html(error);
                }
            });
        }, 500));
    }
});

// Function to delay processing until the user stops typing
function delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}