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
                elements.annotations.querySelector(selectors.noAnnotationsText).classList.add('hidden');
                state.hasAnnotations = true;
            }

            // Continue playing video
            playVideo(state.player);
        }
    });
};

function ExistingAnnotation(userId, comment, annotationId) {
    this.userId = userId;
    this.comment = comment;
    this.annotationId = annotationId;
} 

ExistingAnnotation.prototype.edit = function(annotationElementBody) {
    $.ajax({
        url: apiUrls.editAnnotation,
        data: {
            comment: this.comment,
            userId: this.userId,
            annotationId: this.annotationId
        },
        method: 'POST',
        dataType: 'json',
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

ExistingAnnotation.prototype.delete = function(annotationElement) {
    $.ajax({
        url: apiUrls.deleteAnnotation,
        data: {
            userId: this.userId,
            annotationId: this.annotationId
        },
        method: 'POST',
        dataType: 'json',
        beforeSend: function(xhr) {
            // Set header for security
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function() {
            removeAnnotation(annotationElement);
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

function Role(libraryId) {
    this.libraryId = libraryId;
    this.id = null;
    this.privileges = null;
    this.title = null;
}

Role.prototype.fetchAndSet = function () {
    $.ajax({
        url: apiUrls.fetchRole,
        data: {
            libraryId: this.libraryId
        },
        method: 'POST',
        dataType: 'json',
        beforeSend: function(xhr) {
            // Set header for security
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function(roleData) {
            state.userRole.id = roleData[1].id;
            state.userRole.privileges = roleData[1].privileges;
            state.userRole.title = roleData[1].title;
        }
    });
};

Role.prototype.hasPrivilege = function(possiblePrivilege) {
    if(this.privileges === null) {
        alert('Error: User does not have any privileges set');
        return;
    }
    
    for (let privilege of this.privileges) {
        if(possiblePrivilege === privilege.alias) {
            return true;
        }
    }
    
    return false;
};