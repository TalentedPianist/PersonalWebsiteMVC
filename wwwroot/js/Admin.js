$('#Gallery a').on("click", function () {
    $('#Modal').modal('show');
});

$(function () {
    
});

$('#Gallery a').on('click', function () {
    var imageSource = $(this).attr('src');
    var fileName = $(this).data("name");
    var src = $(this).data("src");
    $("#modal_title").empty();
    $("#modal_title").append(fileName);
    $("#photo_modal").empty();
    $("#photo_modal").append("<img id='photo1' src='" + src + "'>");
});