


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


//Sticky header - needs a media query in JS else it will mess up the desktop view!!
if (window.matchMedia('(max-width: 600px)').matches) {
    $(window).on('scroll', function () {

        if ($(this).scrollTop() > 100) {
            $('header').addClass('sticky');
            $('header').appendTo('body');
        } else {

        }
    });
}

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

// jQuery functionality
window.initializeJQuery = () => {

    $(".gallery").featherlightGallery({
        persist: true,
        afterOpen: async function (event) {
            var token = "";
            var formFieldName = "";
            var headerName = "";
            $.ajax({
                method: "GET",
                url: "/PhotoCommentForm",
                async: false,
                cache: false,
                success: function (message) {
                    token = message.RequestToken;
                    formFieldName = message.FormFieldName;
                    headerName = message.headerName;
                },
                error: function (error) {
                    console.log(error);
                }
            });

            $(document).on("submit", "form[formname='CommentForm']", function (e) {
                e.preventDefault();
                console.log("Form was submitted!"); 
            });

            var onloadCallback = function() {
                console.log("grecaptcha is ready!");
            };

            $(this.$content).after(`
                <div id="Comments">
                <form method="post" formname="CommentForm">
                    <div>
                        <label for="CommentAuthor">Name:</label>
                        <input type="text" name="txtName">
                    </div>
                    <div>
                        <label for="CommentAuthorEmail">Email:</label>
                        <input type="email" name="txtEmail">
                    </div>
                    <div>
                        <label for="CommentAuthorUrl">Website:</label>
                        <input type="text" name="txtWebsite">
                    </div>
                    <div>
                        <label for="CommentContent>Comment:</label>
                        <textarea name="txtComment" rows="10" cols="30"></textarea>
                    </div>
                    <input type="hidden" value="${token}" name="${formFieldName}">
                    <button type="submit">Comment</button>
                </div>
            `);

        }
    });
}

window.openFeatherlight = () => {
};

