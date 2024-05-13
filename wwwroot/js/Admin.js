window.addEventListener("load", (e) => {
    ClassicEditor.create(document.querySelector("#editor"))
        .then(editor => {
            console.log(editor);
        })
        .catch(error => {
            console.error(error);
        });
});

$('#myModal').on('shown.bs.modal', function () {
    $('#').trigger('focus')
});