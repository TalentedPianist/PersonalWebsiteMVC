
CKEditorInterop = (() => {
    var editors = {};

    return {
        init(id, dotNetReference) {
            window.ClassicEditor
                .create(document.getElementById(id), {
                    toolbar: ['undo', 'redo', 'bold', 'italic', 'link', 'numberedList', 'bulletedList']
                })
                .then(editor => {
                    editors[id] = editor;
                    editor.model.document.on('change', () => {
                        var data = editor.getData();

                        var el = document.createElement('div');
                        el.innerHTML = data;
                        if (el.innerText.trim() === '')
                            data = null; // Weirdness with this line.  The first time I couldn't type in the text box because of it but without it data is null on submit.

                        dotNetReference.invokeMethodAsync('EditorDataChanged', data);
                    });
                })
                .catch(error => console.error(error));
        },
        destroy(id) {
            
            editors[id].destroy()
                .then(() => delete editors[id])
                .catch(error => console.log(error));
                
        }
    }
})();