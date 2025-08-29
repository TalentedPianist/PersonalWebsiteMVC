

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


if (document.getElementById('contactForm')) {
    const form = document.querySelector('form');
    const button = document.querySelector('#contactButton');
    const name = document.querySelector('#name');
    const email = document.querySelector('#email');
    const website = document.querySelector('#website');
    const captcha = document.querySelector('.g-recaptcha-response');

    [name, email].forEach(field => {
        field.addEventListener('input', () => button.disabled = false);
    });
    $(".ck").on('input', () => button.disabled = false);

    $(form).on('submit', function(e) {
        e.preventDefault();
        button.disabled = true; // Prevent the form from being submitted multiple times with empty data

        const rawData = window.editor.getData();
        const plainText = rawData.replace(/<[^>]*>/g, '').trim();

        let oldSummary = document.getElementById("errorsSummary");
        if (oldSummary) {
            oldSummary.remove();
        }

        let errors = [];


        console.log('Submit triggered');
        if (!name.validity.valid) {
            $("#name").parent().find('.error').text('Please enter your name.');
            $("#name").addClass('inputError');
            errors.push("Please enter your name.");

        } else {
            $("#name").parent().find('.error').text('');
            $("#name").removeClass('inputError');
         
        }

        if (!email.validity.valid) {
            $("#email").parent().find('.error').text('Please enter your email address.');
            $("#email").addClass('inputError');
            errors.push("Please enter your email address.");

        } else {
            $("#email").parent().find('.error').text('');
            $("#email").removeClass('inputError');
        
        }

        if (plainText === '') {
            $(".ck").parent().find('.error').text('Please enter a message.');
            $(".ck").addClass('inputError');
            errors.push("Please enter a message.");
        } else {
            $(".ck").parent().find('.error').text('');
            $(".ck").removeClass('inputError');
            button.disabled = false;

        }



        if (errors.length === 0) {
            console.log('Form is valid!');
            // Post data to the server
            $.ajax({
                method: "POST",
                url: "/Mobile/SubmitContactForm",
                data: { name: name.value, email: email.value, website: website.value, message: window.editor.getData() },
                cache: false,
                success: function (message) {
                    console.log(message);
                },
                error: function (error) {
                    console.log(error);
                }
            });
            
        } else {
            // let errorsDiv = document.createElement('div');
            // errorsDiv.setAttribute("id", "errorsSummary");
            // errorsDiv.innerHTML = "<p>Please correct the following errors:</p>";
            // errorsDiv.innerHTML += "<ul>";
            // errors.forEach(function (value, index) {
            //     errorsDiv.innerHTML += `<li>${value}</li>`;
            // });
            // errorsDiv.innerHTML += "</ul>";
            // $(form).before(errorsDiv);
            
        }
    });
}


if (document.getElementById('commentsForm')) {
    const form = document.querySelector("form");
    const button = document.querySelector("button");
    const name = document.querySelector("#name");
    const email = document.querySelector("#email");
    const website = document.querySelector("#website");
    const message = document.querySelector("#ckeditor1");



    form.addEventListener('submit', function (e) {
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

        if (form.valid) {
            console.log('form is valid');
        } else {
            let errorsDiv = document.createElement('div');
            errorsDiv.setAttribute("id", "errorsSummary");
            errorsDiv.innerHTML = "<hr>";
            errorsDiv.innerHTML += "<p>Please correct the following errors:</p>";
            errorsDiv.innerHTML += "<ul>";
            errors.forEach(function (value, index) {
                errorsDiv.innerHTML += `<li>${value}</li>`;
            });
            errorsDiv.innerHTML += "</ul>";
            $(form).before('<p>Hello World!</p>');

        }

    });
}


