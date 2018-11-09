function Annotation(videoId, comment, timestamp) {
    this.videoId = videoId;
    this.comment = comment;
    this.timestamp = timestamp;
}

Annotation.prototype.submit = function(player) {
    $.ajax({
        url: apiUrls.submitAnnotation,
        data: {
            comment: this.comment,
            timestamp: this.timestamp,
            videoId: this.videoId
        },
        method: 'POST',
        dataType: 'html',
        beforeSend: function(xhr) {
            // Set header for security
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function(annotationHTML) {
            prependAnnotationToAnnotationsBody(annotationHTML);
            hideCreateAnnotationControls();

            if (state.hasAnnotations === false) {
                // Remove element, since there is now annotations to show
                elements.noAnnotationsText.parentElement.removeChild(elements.noAnnotationsText);
                state.hasAnnotations = true;
            }

            // Continue playing video
            playVideo(state.player);
        }
    });
};

function ExistingAnnotation(videoId, comment, annotationId) {
    this.videoId = videoId;
    this.comment = comment;
    this.annotationId = annotationId;
} 

ExistingAnnotation.prototype.edit = function(annotationElementBody) {
    $.ajax({
        url: apiUrls.editAnnotation,
        data: {
            comment: this.comment,
            videoId: this.videoId,
            annotationId: this.annotationId
        },
        method: 'POST',
        beforeSend: function(xhr) {
            // Set header for security
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function() {
            unhideAnnotationText(annotationElementBody);
            updateAnnotationText(annotationElementBody);
            removeEditControls(annotationElementBody);
        }
    });
};

function Reply(annotationId, text) {
    this.annotationId = annotationId;
    this.text = text;
}

Reply.prototype.submit = function(annotationElement) {
    $.ajax({
        url: apiUrls.submitReply,
        data: {
            annotationId: this.annotationId,
            text: this.text
        },
        method: 'POST',
        dataType: 'html',
        beforeSend: function(xhr) {
            // Set header for security
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function(replyHTML) {
            prependReplyToRepliesBody(annotationElement, replyHTML);
            removeCreateReplyControls(annotationElement);
            
            if(!doesAnnotationElementHaveToggleRepliesButton(annotationElement)) {
                renderToggleRepliesButton(annotationElement);
            }
        }
    });
};