// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

(function () {
    let menu = document.getElementById("menuLink");
    let nav = document.querySelector("nav");
    let header = document.querySelector("header");
    let home = document.querySelector(".homeIcon");
    let menuIcon = document.querySelector(".menuIcon");

    menu.addEventListener("click", function (event) {
        event.preventDefault();
        home.style.display = "none";
        menuIcon.style.display = "none";
        nav.classList.add("mobileNav");
        header.classList.add("Modal");
        let ul = document.querySelector("nav ul");
        let li = document.createElement("li");
        li.innerHTML = "<a href='' id='closeLink'><img src='../icons/close.svg' style='position: relative; z-index: 2;'></a>";
        ul.appendChild(li);
        ul.prepend(li);
    });

   
}());


