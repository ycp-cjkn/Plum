var idNames = {
    videoUrl: 'video-url',
    createAnnotationButton: 'create-annotation-btn',
    annotationsBody: 'annotations-body',
    noAnnotationsText: 'no-annotation-text',
    annotations: 'annotations',
    filterAnnotationsList: 'filter-annotations-list',
    libraryId: 'library-id'
};

var classNames = {
    createAnnotation: 'create-annotation-container',
    cancelAnnotation: 'cancel-annotation',
    newAnnotationTimestamp: 'timestamp',
    submitAnnotation: 'submit-annotation',
    videoId: 'video-id',
    toggleRepliesButton: 'toggle-replies',
    replyButton: 'reply-button',
    createReplyControls: 'create-reply-container',
    annotationWrapper: 'annotation-wrapper',
    cancelCreateReplyButton: 'cancel-reply',
    submitReply: 'submit-reply',
    annotationReplies: 'annotation-replies',
    displayName: 'display-name',
    editAnnotation: 'edit-annotation',
    annotationBody: 'annotation-body',
    deleteAnnotation: 'delete-annotation',
    cancelEditAnnotation: 'cancel-edit-annotation',
    annotationText: 'annotation-text',
    editAnnotationTextWrapper: 'edit-annotation-text-wrapper'
};

var selectors = {
    createAnnotationTextarea: '.create-annotation-container textarea',
    displayName: `.${classNames.displayName}`,
    filterAnnotationsList: `#${idNames.filterAnnotationsList}`,
    noAnnotationsText: `#${idNames.noAnnotationsText}`,
    filterAnnotationsListEntry: `#${idNames.filterAnnotationsList} li`,
    editAnnotation: `#${idNames.annotations} a.${classNames.editAnnotation}`,
    annotationWrapper: `.${classNames.annotationWrapper}`,
    annotationBody: `.${classNames.annotationBody}`,
    annotationText: `.${classNames.annotationText}`,
    editAnnotationTextWrapper: `.${classNames.editAnnotationTextWrapper}`,
    editAnnotationText: `.${classNames.editAnnotationTextWrapper} textarea`,
    libraryId: `#${idNames.libraryId}`
};

var elements = {
    videoUrl: document.getElementById(idNames.videoUrl),
    createAnnotation: document.querySelector('.' + classNames.createAnnotation),
    createAnnotationButton: document.getElementById(idNames.createAnnotationButton),
    cancelAnnotation: document.querySelector('.' + classNames.cancelAnnotation),
    newAnnotationTimestamp: document.querySelector('.' + classNames.newAnnotationTimestamp),
    createAnnotationTextarea: document.querySelector(selectors.createAnnotationTextarea),
    videoId: document.querySelector('.' + classNames.videoId),
    annotationsBody: document.getElementById(idNames.annotationsBody),
    noAnnotationsText: document.getElementById(idNames.noAnnotationsText),
    annotations: document.getElementById(idNames.annotations),
};

var apiUrls = {
    submitAnnotation: '/Videos/' + elements.videoId.value + '?handler=CreateAnnotation',
    submitReply: '/Videos/' + elements.videoId.value + '?handler=CreateReply',
    editAnnotation: '/Videos/' + elements.videoId.value + '?handler=EditAnnotation',
    deleteAnnotation: '/Videos/' + elements.videoId.value + '?handler=DeleteAnnotation',
    fetchRole: '/Videos/' + elements.videoId.value + '?handler=FetchRole'
};