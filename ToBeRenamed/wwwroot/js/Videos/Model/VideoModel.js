import * as videoController from '../Controller/VideoController.js';
import * as videoView from '../View/VideoView.js';
import * as videoBase from '../View/base.js';

export function Annotation(videoId, comment, timestamp) {
    this.videoId = videoId;
    this.comment = comment;
    this.timestamp = timestamp;
}

Annotation.prototype.submit = function() {
    let that = this;
    $.ajax({
        url: videoBase.apiUrls.submitAnnotation,
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
            videoView.hideCreateAnnotationControls();
            videoView.insertNewAnnotation(that, annotationHTML);

            if (videoController.state.hasAnnotations === false) {
                // Remove element, since there is now annotations to show
                videoBase.elements.annotations.querySelector(videoBase.selectors.noAnnotationsText).classList.add('hidden');
                videoController.state.hasAnnotations = true;
            }

            // Continue playing video
            videoView.playVideo(window.player);
        }
    });
};

export function ExistingAnnotation(userId, comment, annotationId) {
    this.userId = userId;
    this.comment = comment;
    this.annotationId = annotationId;
} 

ExistingAnnotation.prototype.edit = function(annotationElementBody) {
    $.ajax({
        url: videoBase.apiUrls.editAnnotation,
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
            videoView.unhideAnnotationText(annotationElementBody);
            videoView.updateAnnotationText(annotationElementBody);
            videoController.removeEditControls(annotationElementBody);
        }
    });
};

ExistingAnnotation.prototype.delete = function(annotationElement) {
    $.ajax({
        url: videoBase.apiUrls.deleteAnnotation,
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
            videoView.removeAnnotation(annotationElement);
        }
    });
};

export function Reply(annotationId, text) {
    this.annotationId = annotationId;
    this.text = text;
}

Reply.prototype.submit = function(annotationElement) {
    $.ajax({
        url: videoBase.apiUrls.submitReply,
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
            videoView.prependReplyToRepliesBody(annotationElement, replyHTML);
            videoView.removeCreateReplyControls(annotationElement);
            
            if(!videoView.doesAnnotationElementHaveToggleRepliesButton(annotationElement)) {
                videoView.renderToggleRepliesButton(annotationElement);
            }
        }
    });
};

export function ExistingReply(text, replyId, userId) {
    this.text = text;
    this.replyId = replyId;
    this.userId = userId;
}

ExistingReply.prototype.edit = function(replyElementBody) {
    $.ajax({
        url: videoBase.apiUrls.editReply,
        data: {
            replyId: this.replyId,
            text: this.text,
            userId: this.userId
        },
        method: 'POST',
        dataType: 'json',
        beforeSend: function(xhr) {
            // Set header for security
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function() {
            videoView.unhideReplyText(replyElementBody);
            videoView.updateReplyText(replyElementBody);
            videoView.removeEditReplyControls(replyElementBody);
        }
    });
};

ExistingReply.prototype.delete = function(replyElement) {
    $.ajax({
        url: videoBase.apiUrls.deleteReply,
        data: {
            userId: this.userId,
            replyId: this.replyId
        },
        method: 'POST',
        dataType: 'json',
        beforeSend: function(xhr) {
            // Set header for security
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function() {
            var annotationElement = replyElement.closest(videoBase.selectors.annotationWrapper);
            
            videoView.removeReply(replyElement);

            if(!videoView.doesAnnotationHaveReplies(annotationElement)) {
                videoView.removeToggleRepliesWrapper(annotationElement);
            }
        }
    });
};

export function Role(libraryId) {
    this.libraryId = libraryId;
    this.id = null;
    this.privileges = null;
    this.title = null;
}

Role.prototype.fetchAndSet = function () {
    $.ajax({
        url: videoBase.apiUrls.fetchRole,
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
            videoController.state.userRole.id = roleData[1].id;
            videoController.state.userRole.privileges = roleData[1].privileges;
            videoController.state.userRole.title = roleData[1].title;
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

