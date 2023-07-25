// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



var num = 200;

// Code to make header sticky on scroll
//https://stackoverflow.com/questions/22541364/sticky-navbar-onscroll
$(window).bind('scroll', function () {
    if ($(window).scrollTop() > num) {
        $('nav').addClass('fixed');
    } else {
        $('nav').removeClass('fixed');
    }
});

// jQuery smooth scroll code
// https://stackoverflow.com/questions/7717527/smooth-scrolling-when-clicking-an-anchor-link
$(document).on('click', 'a[href^="#"]', function (event) {
    event.preventDefault();

    $('html, body').animate({
        scrollTop: $($.attr(this, 'href')).offset().top
    }, 500);
});

// Scroll to top of page when home link is clicked
// https://stackoverflow.com/questions/1144805/scroll-to-the-top-of-the-page-using-javascript
$(".home").on("click", function () {
    $("html, body").animate({ scrollTop: 0 }, "slow");
    return false;
});

