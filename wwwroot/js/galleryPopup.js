$('[data-featherlight-gallery').featherlightGallery({
    afterContent: function () {
        let current = $.featherlight.current().$content;
        let ckElement = current.find('#ckeditor1')[0];
        let modalEditor;



        window.ClassicEditor.create(ckElement, {
            licenseKey: 'GPL',
            plugins: [
                window.CKEditorEssentials,
                window.CKEditorParagraph,
                window.CKEditorBold,
                window.CKEditorItalic,
                window.CKEditorFont
            ],
            toolbar: []
        }).then(editorInstance => {
            currentEditor = editorInstance;


            $(document).on('submit', function (e) {
                e.preventDefault();

                let name = current.find("#name").val();
                let email = current.find("#email").val();
                let website = current.find("#website").val();
                let comment = currentEditor.getData();

                let $inst = $.featherlight.current().$currentTarget;
                let photoId = $inst.data('photo-id');

                $.ajax({
                    method: "POST",
                    url: "/Photo/AddComment",
                    data: JSON.stringify({
                        'CommentAuthor': name,
                        'CommentAuthorEmail': email,
                        'CommentAuthorUrl': website,
                        'CommentContent': comment,
                        'PhotoID': photoId,
                    }),
                    headers: {
                        "Content-type": "application/json",
                    },
                    async: false,
                    cache: false,
                    success: function (message) {
                        $(".comments-container").append(`
                                <div class="comment-item">
                                                                                <div class="flex flex-row flex-1">
                                                                                        <img src="https://gravatar.com/avatar/27205e5c51cb03f862138b22bcb5dc20f94a342e744ff6df1b8dc8af3c865109?f=y" class="mr-5" alt="">
                                                                                        <p class="ml-5">
                                                                                            Posted by ${message.CommentAuthor} on
                                                                                                ${dayjs(message.CommentDate).format('dddd YY MMMM YYYY')} at
                                                                                                ${dayjs(message.CommentDate).format('hh:mm A')}.
                                                                                        </p>
                                                                                </div>
                                                                                ${message.CommentContent}
                                                                       </div>      
                            `);
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });


            });


        }).catch(console.error);

        let $trigger = this.$currentTarget;
        let $inst = this.$instance;

        $('.lightbox-image').attr('src', $trigger.find('img').attr('src'));
        let name = $trigger.data('name');
        let photoId = $trigger.data('photo-id');
        $(".photo-id").val(photoId);


        $.ajax({
            method: "POST",
            url: "/Album/GetPhoto",
            data: { name: name },
            async: false,
            cache: false,
            success: function (message) {
                // jQuery seems to prefer using classes for manipulating the DOM rather than ids.  
                $('.photoId').text(message.PhotoID);
                GetComments(photoId);
            },
            error: function (error) {
                console.log(error);
            }
        });


        function GetComments(id, page = 1) {


            $.ajax({
                method: "POST",
                url: "/Album/GetComments",
                data: { id: id, page: page },
                async: false,
                cache: false,
                success: function (message) {
                    if (message.length) {
                        $(".comment-status").text(`${message.length} comments have been found for this photo.`);
                    } else if (message.length === 1) {
                        $(".comment-status").text('One comment has been found for this photo.');
                    } else if (message.length === 0) {
                        $(".comment-status").text('No comments have been found for this photo.  Be the first to add a comment using the form below.')
                    }

                    var current = $.featherlight.current().$content;
                    let commentsContainer = $(current).find('.comments-container');
                    console.log(commentsContainer);

                    $.each(message, function (index) {
                        commentsContainer.append(`
                                                                        <div class="comment-item">
                                                                                <div class="flex flex-row flex-1">
                                                                                        <img src="https://gravatar.com/avatar/27205e5c51cb03f862138b22bcb5dc20f94a342e744ff6df1b8dc8af3c865109?f=y" class="mr-5" alt="">
                                                                                        <p class="ml-5">
                                                                                            Posted by ${message[index].CommentAuthor} on
                                                                                                ${dayjs(message[index].CommentDate).format('dddd YY MMMM YYYY')} at
                                                                                                ${dayjs(message[index].CommentDate).format('hh:mm A')}.
                                                                                        </p>
                                                                                </div>
                                                                                ${message[index].CommentContent}
                                                                       </div>        
                                                                 `)
                    })

                    var commentItems = commentsContainer.find('.comment-item');
                    var perPage = 6;

                    commentItems.hide().slice(0, perPage).show();

                    var paginationContainer = $(current).find('.pagination-container');
                    paginationContainer.pagination({
                        items: commentItems.length,
                        itemsOnPage: perPage,
                        onPageClick: function (pageNumber, e) {
                            e.preventDefault(); // Stop featherlight from closing
                            var showFrom = perPage * (pageNumber - 1);
                            var showTo = showFrom + perPage;
                            commentItems.hide().slice(showFrom, showTo).show();
                        }
                    });




                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    } // End AfterContent function
});
