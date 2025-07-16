


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
    $("a.gallery").featherlightGallery({
        afterContent: function (e) {
            $(document).on('submit', function (e) {
                e.preventDefault();
                let captchaSecret = "6LeCBlUrAAAAACVipFQ2hXQkaRn1i_pFJEZIegge";
                console.log("Form was submitted!");
                console.log(grecaptcha.getResponse());
                $.ajax({
                    type: "POST",
                    url: "/ValidateRecaptcha",
                    data: { secret: captchaSecret, response: grecaptcha.getResponse() },
                    dataType: "text",
                    async: false,
                    cache: false,
                    success: function (message) {
                       let result = $.parseJSON(message);
                       if (result.success === true) 
                       {
                            doStuff();
                       }
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
                function doStuff()
                {
                    console.log("Doing stuff...");
                    let name = $("input[name=txtName]").val();
                    let email = $("input[name=txtEmail]").val();
                    let website = $("input[name=txtWebsite]").val();
                    let comment = $("textarea[name=txtComment]").val();
                    
                    $.ajax({
                        method: "POST",
                        url: "/photos/AddComment", 
                        headers: {
                            "Content-type": "application/data", 
                        },
                        data: JSON.stringify({ CommentAuthor: name, CommentAuthorEmail: email, CommentAuthorUrl: website, CommentContent: comment }),
                        async: false,
                        cache: false,
                        success: function(message) { 
                            console.log(message);
                        },
                        error: function(error) { 
                            console.log(error);
                        }
                    });
                }
            });

            let response = undefined;

            console.log(this.$content);

            $(this.$content).after(`
                <div class="commentForm">
                    <form method="post" id="commentForm" name="CommentForm">
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
                            <label for="CommentContent">Comment:</label>
                            <textarea rows="10" cols="30" name="txtComment"></textarea>
                        </div>
                        
                        <div id="g-recaptcha" data-sitekey="6LeCBlUrAAAAAGJFT1Rt-4hojR6NfEvqzsvZwnOz" data-callback="onHuman"></div>
                        <button type="submit">Submit</button>
                    </form>
                </div>
                <script src="https://www.google.com/recaptcha/api.js?onload=onloadCallback"></script>
	<script>
    function onHuman(response) {
                document.getElementById('g-recaptcha').value = response;
                doOtherStuff(response);
            }

		var onloadCallback = function() {
			grecaptcha.render('g-recaptcha', { 
                'sitekey': '6LeCBlUrAAAAAGJFT1Rt-4hojR6NfEvqzsvZwnOz',
			});
            
		};


        
	</script>
               
            `);
       
        }
    });

    
}

window.openFeatherlight = () => {

}


