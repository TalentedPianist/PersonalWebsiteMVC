
$(function() {
    $(".mobileNav a").on('click', function(event) {
    
        const modal = document.querySelector('#my_nav');
        modal.close();

        $("html, body").animate({ 
            scrollTop: $(hash).offset().top
        }, 200, function() { 
        
        });
    });
     

    
});