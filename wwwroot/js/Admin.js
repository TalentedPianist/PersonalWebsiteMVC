
$('#Gallery img[data-toggle="lightbox"], img.thickbox').each(function () {
    if (!$(this).attr("data-remote")) {
        $(this).attr("data-remote", $(this).attr("src")).css("cursor", "pointer");
    }
});