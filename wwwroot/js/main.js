import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';

ClassicEditor
    .create(document.querySelector('#ckeditor1'), { 
        toolbar: []
    }
})
.