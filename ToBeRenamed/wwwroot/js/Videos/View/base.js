var idNames = {
    videoUrl: 'video-url',
    createAnnotationButton: 'create-annotation-btn',
    annotationsBody: 'annotations-body',
    noAnnotationsText: 'no-annotation-text',
    annotations: 'annotations'
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
    cancelCreateReplyButton: 'cancel-reply'
};

var selectors = {
    createAnnotationTextarea: '.create-annotation-container textarea'
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
    submitAnnotation: '/Videos/' + elements.videoId.value + '?handler=CreateAnnotation'
};