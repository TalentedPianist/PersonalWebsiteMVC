$('#Gallery a').on("click", function () {
    $('#Modal').modal('show');
});


$('#Gallery a').on('click', function () {
    var imageSource = $(this).attr('src');
    var fileName = $(this).data("name");
    var src = $(this).data("src");
    $("#modal_title").empty();
    $("#modal_title").append(fileName);
    $("#photo_modal").empty();
    $("#photo_modal").append("<img src='" + src + "'>");
});