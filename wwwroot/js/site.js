

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

    $(form).on('submit', function (e) {
        e.preventDefault();
        button.disabled = true; // Prevent the form from being submitted multiple times with empty data

       var x = $("form").serializeArray();
       $.each(x, function(i, field) { 
        console.log(field.name + " " + field.value);
       });

        let oldSummary = document.getElementById("errorsSummary");
        if (oldSummary) {
            oldSummary.remove();
        }

        let errors = [];


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

        if (window.editor.getData() === '') {
            $(".ck").parent().find('.error').text('Please enter a message.');
            $(".ck").addClass('inputError');
            errors.push("Please enter a message.");
        } else {
            $(".ck").parent().find('.error').text('');
            $(".ck").removeClass('inputError');
            button.disabled = false;

        }



        if (errors.length === 0) {

            // Post data to the server
            $.ajax({
                method: "POST",
                url: "/Mobile/SubmitContactForm",
                data: { name: name.value, email: email.value, website: website.value, message: window.editor.getData(), captchaResponse: document.querySelector('.g-recaptcha-response').value },
                cache: false,
                success: function (message) {
                    // .g-captcha-response needs to be set on submit.  It won't return any value if it's set outside submit.  
                    console.log(message);
                    // if (message.error) {
                    //     $(form).before('<p>Please take a second to do the captcha to prove that you are not a robot.  This helps me fight spam.  Thank you.</p>');
                    // } else {
                    //     $(form).before(`<p>Hi ${name.value}, thanks for your email.  A reply will be sent if necessary.</p>`);
                    // }
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
        let postID = document.querySelector("#postID");
        let captcha = document.querySelector(".g-recaptcha-response");

        // Remove old summary of errors so as not to display them every time submit is clicked!
        let oldSummary = document.getElementById("errorsSummary");
        if (oldSummary) { 
            oldSummary.remove();
        }

        let errors = [];

        if (!name.validity.valid) { 
            $("#name").parent().find('.error').text('Please enter your name.');
            $("#name").addClass('inputError');
            errors.push("Please enter your name.");
        } else { 
            $("#name").parent().find('.error').text('');
            $("#name").removeClass('inputError');
        }

        if (!email.validity.valid) { 
            $(email).parent().find('.error').text('Please enter your email address.');
            $(email).addClass('inputError');
            errors.push("Please enter your email address.");
        } else { 
            $("#email").parent().find('.error').text('');
            $("#email").removeClass('inputError');
        }

        if (window.editor.getData() === '') { 
            $(".ck").parent().find('.error').text('Please enter a message.');
            $(".ck").addClass("inputError");
            errors.push("Please enter a message.");
        } else { 
            $(".ck").parent().find('.error').text('');
            $(".ck").removeClass('inputError');
       
        }


        if (errors.length === 0) {
            $.ajax({
                method: "POST", 
                url: "/Blog/AddComment",
                data: { name: name.value, email: email.value, website: website.value, message: window.editor.getData(), captchaResponse: captcha.value, postID: postID.value },
                async: false,
                cache: false,
                success: function(message) { 
                    if (message.error) { 
                        $("form").before(`<hr><p>${message.error}</p>`);
                    } else { 
                    $("form").before(`
                        <hr>
                        <div class="flex flex-row flex-1">
                            <img src="https://gravatar.com/avatar/27205e5c51cb03f862138b22bcb5dc20f94a342e744ff6df1b8dc8af3c865109&d=mp" />
                            <p>Posted by ${message.CommentAuthor} on ${dayjs(message.CommentDate).format('dddd MMMM D')} at ${ dayjs(message.CommentDate).format('hh:ss A')}.</p>
                        </div>
                        ${message.CommentContent}
                        `);
                    }
                }, 
                error: function(error) { 
                    console.log(error);
                }  
            });
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
            $(form).before(errorsDiv);

        }

    });
}


