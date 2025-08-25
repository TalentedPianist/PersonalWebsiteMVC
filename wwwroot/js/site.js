

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


$("#Contact form").on('submit', function(e) { 
    e.preventDefault();
    let name = $("#name").val();
    let email = $("#email").val();
    let website = $("#website").val();
    let message = $("#ckeditor1").val();
    let captcha = $(".g-recaptcha-response").val();
    

    $.ajax({ 
        method: "POST", 
        url: "/Mobile/SubmitContactForm", 
        data: { name: name, email: email, website: website, message: message, captchaResponse: captcha },
        async: false,
        cache: false,
        success: function(message) { 
            console.log(message);
        }, 
        error: function(error) { 
            console.log(error);
        }
    });
});

$("#Comments form").on('submit', function(e) { 
    e.preventDefault();
    let name = $("#name").val();
    let email = $("#email").val();
    let website = $("#website").val();
    let message = window.editor.getData();
    let captcha = $("#g-recaptcha-response").val();
    let postId = $("#postId").val();
    
    $.ajax({ 
        method: "POST", 
        url: "/Blog/AddComment", 
        data: { name: name, email: email, website: website, message: message, captchaResponse: captcha, postId: postId },
        async: false,
        cache: false,
        success: function(message) { 
            console.log(message);
            $(".comments-container").after(
                `
                    <div class="flex flex-row flex-1">
                         <img src="https://gravatar.com/avatar/27205e5c51cb03f862138b22bcb5dc20f94a342e744ff6df1b8dc8af3c865109?f=y" class="mr-5" alt="">
                         <p class="ml-5">Posted by ${message.CommentAuthor} on ${dayjs(message.CommentDate).format('dddd YY MMMM YYYY')} at ${dayjs(message.CommentDate).format('hh:mm A')}.
                    </div>
                    ${message.CommentContent}
                `);
        }, 
        error: function(error) { 
            console.log(error);
        }
    });
    
});