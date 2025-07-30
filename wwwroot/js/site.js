

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


// Load more stuff....
let skip = 1; 
const pageSize = 1; 
$('#loadMoreBtn').on('click', function(e) { 
    e.preventDefault();

    $.get('/Blog/LoadMore', { skip: skip }, function(data) { 
        console.log(data);
        $('#Blog section:nth-of-type(1)').append(data);
        skip = skip + 1; 

    });
});

// Show less button...
$('#showLessBtn').on('click', function(e) { 
    e.preventDefault();

    const lastBatch = $('#blogPosts').last();
    lastBatch.slideUp('fast', function() { 
        lastBatch.remove();
        skip -= pageSize;
    });
});