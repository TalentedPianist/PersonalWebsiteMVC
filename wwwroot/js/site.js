

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

// var onloadCallback = function () {
//     grecaptcha.render('recaptcha', {
//         'site-key': '6LeCBlUrAAAAAGJFT1Rt-4hojR6NfEvqzsvZwnOz '

//     });

// };

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


// Photo gallery comments
// Initialize Featherlight instance
$("a.gallery").featherlightGallery({ 
    galleryFadeIn: 100, 
    galleryFadeOut: 300,
    afterContent: function() { 
        let image = $(this.$currentTarget);
        let name = $(image).data('name');
        let comments = $(this.$content);
        console.log(this);
        console.log(comments);
        // Get comments for photo using Ajax
        $.ajax({
            method: "GET",
            url: "/Album/GetComments",
            data: { name: name },
            async: false,
            cache: false,
            success: function(message) { 
                $(comments).after(`
                    <form method="post">
                        <div>
                            <label>Name:</label>
                            <input type="text" placeholder="John Smith" name="txtName">
                        </div>
                        <div>
                            <label>Email:</label>
                            <input type="text" placeholder="john@johnsmith.com">
                        </div>
                        <div>
                            <label>Website:</label>
                            <input type="text" placeholder="https://www.johnsmith.com">
                        </div>
                        <div>
                            <label>Comment:</label>
                            <textarea rows="10" cols="30" placeholder="This is a great photo...."></textarea>
                        </div>
                        <button type="submit">Comment</button>
                    </form>

                `);
            },
            error: function(error) { 
                console.log(error);
            }
        });
       
    },
});