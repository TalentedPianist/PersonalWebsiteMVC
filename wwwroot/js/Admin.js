
if (document.querySelector("#ckeditor1")) {
    ClassicEditor
        .create(document.querySelector('#ckeditor1'))
        .then(editor => {
            console.log(editor);
        })
        .catch(error => {
            console.error(error);
        });
}

$("#photoModal .close").on('click', function (e) {
    e.preventDefault();
    console.log('Button was clicked');
});



 



