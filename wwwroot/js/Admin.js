
var elementExists = document.getElementById("#ckeditor1");

if (elementExists) {
    ClassicEditor
        .create(document.querySelector('#ckeditor1'))
        .then(editor => {
            console.log(editor);
        })
        .catch(error => {
            console.error(error);
        });
}