
var element = document.getElementById('#ckeditor1');
if (typeof (element) != 'undefined' && element != null) {
    ClassicEditor
        .create(document.querySelector('#ckeditor1'))
        .then(editor => {
            console.log(editor);
        })
        .catch(error => {
            console.error(error);
        });

}