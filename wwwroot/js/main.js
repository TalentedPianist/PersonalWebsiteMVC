import { ClassicEditor, Essentials, Bold, Italic, Font, Paragraph } from 'ckeditor5';

ClassicEditor
    .create(document.querySelector("#ckeditor1"),  {
        plugins: [ Essentials ],
        toolbar: [],
    })
    .then()
    .catch();

