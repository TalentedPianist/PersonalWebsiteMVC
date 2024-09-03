
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



