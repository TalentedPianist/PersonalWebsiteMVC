



function isDevice() {
    return /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}


function DetectScroll() {
    $(window).on("scroll", function () {
        if ($(window).scrollTop() >= 530) {
            $("header").addClass("sticky");
            $("body").prepend($("header"));
        } else {
            $("header").removeClass("sticky");
            $(".overlay").prepend($("header"));
        }
    });
}

function SmoothScroll(target) {
    // This needs the target parameter to work, otherwise we get an error with top, because it can't find it.
    if (target.length) {
    
        $('html, body').animate({
            scrollTop: $($.attr(this, target)).offset().top
        }, 500);
      
    }

}


function HideIcon() {
    $('.MenuIcon').css('display', 'none');
}

function ShowIcon() {
    $('.MenuIcon').css('display', 'flex');
}

function LoadMore() {
    $(function () {
        // class .mud-paper seems to work here
        $('.mud-paper').hide();
        $('.mud-paper').slice(0, 3).show();
        x = 3; 
        $('.loadMorePosts').on('click', function (e) {
            e.preventDefault();
            x = x + 1;
            $('.mud-paper').slice(0, x).show(); // show x amount of images
        });
    });
}

function HideContent() {
    $("#MainContent").hide();
}


function LoadCKEditor() {
    /*
    ClassicEditor
        .create(document.querySelector('#editor1'), {
            toolbar: ['undo', 'redo', 'bold', 'italic', 'numberedList', 'bulletedList']
        })
        .catch(error => {
            console.error(error);
        });
    */
   
}

function HideIcon() {
    $('.MenuIcon').css('display', 'none');
}

function ShowIcon() {
    $('.MenuIcon').css('display', 'none');
}



