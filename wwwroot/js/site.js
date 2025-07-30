

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


    $(window).on('scroll', function () {
        if ($(window).scrollTop() >= 400) {
            $('header').addClass('sticky');
            $('header').prependTo('body');
        } else {
            $('header').removeClass('sticky');
            $('header').prependTo('#Home');
        }
    });

});

var onloadCallback = function () {
    grecaptcha.render('recaptcha', {
        'site-key': '6LeCBlUrAAAAAGJFT1Rt-4hojR6NfEvqzsvZwnOz '

    });

};

// const { ClassicEditor, Essentials, Bold, Italic, Font, Paragraph } = CKEDITOR;

// ClassicEditor
//     .create(document.querySelector('#ckeditor1'), {
//         licenseKey: 'GPL',
//         plugins: [Essentials, Bold, Italic, Font, Paragraph],
//         toolbar: []
//     })
//     .then(editor => {
//         console.log(editor);
//     })
//     .catch(error => {
//         console.log(error);
//     });


if (/Mobi|Android|iPhone/i.test(navigator.userAgent)) {

    // Infinate scroll stuff
    let elem = document.querySelector('#PostContent');
    $(elem).not(':eq(0)').hide();
}