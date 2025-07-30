import { ClassicEditor }  from '@ckeditor/ckeditor5-editor-classic';
import { Essentials } from '@ckeditor/ckeditor5-essentials';

ClassicEditor
    .create(document.querySelector('#ckeditor1'), { 
        plugins: [ Essentials ],
        toolbar: []
    })
    .then (editor => { 
        console.log(editor);
    })
    .catch(error => { 
        console.error(error.stack);
    });