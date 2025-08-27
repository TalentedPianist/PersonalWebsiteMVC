

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


$("#Contact form").on('submit', function (e) {
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
        success: function (message, status, xhr) {
            console.log(message);
        },
        error: function (error) {
            console.log(error.responseText);
        }
    });
});


if (document.getElementById('commentsForm')) {
    const form = document.querySelector("form");
    const button = document.querySelector("button");
    const name = document.querySelector("#name");
    const email = document.querySelector("#email");
    const website = document.querySelector("#website");
    const message = document.querySelector("#ckeditor1");



    form.addEventListener('submit', function(e) {
        e.preventDefault();

        let errors = [];
        if (name.value === "" && email.value === "" && message.value === "") {
            $("button").attr("disabled", false);
          
        }

        if (name.value === "") {
            errors.push("You must enter your name.");
            $(name).after('<p class="error">You must enter your email address.');
            $(name).addClass('inputError');
        }
        if (email.value === "") {
            errors.push("You must enter your email address.");
            $(email).after('<p class="error">You must enter your email address.</p>');
            $(email).addClass('inputError');
        }

        if (message.value === "") { 
            errors.push("You must enter a comment.");
            $(".ck .ck-editor__main").after('<p class="error">You must enter a comment.</p>');
            $(".ck .ck-editor__main").addClass('inputError');
        }
    
        if (errors.length === 0) { 
            // Form validation is successful, post data to server
        } else { 
            // Errors occurred, display summary
           let errorsDiv = document.createElement('div');
           errorsDiv.setAttribute("id", "errorsSummary");
            
           errorsDiv.innerHTML = "<hr>";
           errorsDiv.innerHTML += "<p>Please correct the following errors:</p>";
           errorsDiv.innerHTML += "<ul>";
            errors.forEach(function(value, index) {
                 errorsDiv.innerHTML += `<li>${value}</li>`;
            });
            errorsDiv.innerHTML += "</ul>";
            $(form).before(errorsDiv);
            $("button").attr("disabled", true);
            return false;
        } 
        
    });
}


