var idNames = {
    videoUrl: 'video-url',
    createAnnotationButton: 'create-annotation-btn',
    annotationsBody: 'annotations-body',
    noAnnotationsText: 'no-annotation-text'
};

var classNames = {
    createAnnotation: 'create-annotation-container',
    cancelAnnotation: 'cancel-annotation',
    newAnnotationTimestamp: 'timestamp',
    submitAnnotation: 'submit-annotation',
    videoId: 'video-id'
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
    noAnnotationsText: document.getElementById(idNames.noAnnotationsText)
};

var apiUrls = {
    submitAnnotation: '/Videos/' + elements.videoId.value + '?handler=CreateAnnotation'
};