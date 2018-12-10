import * as videoController from '../Controller/VideoController.js' // TODO - Pass in any state vars as parameters for functions
import * as videoBase from './base.js';

/**
 * Gets the video url
 * @returns {string} - The video url (identifier) used by the youtube API to get the video
 */
export function getVideoUrl() {
    return videoBase.elements.videoUrl.value;
}

/**
 * Gets the video id
 * @returns {string} - The id of the video used in the database
 */
export function getVideoId() {
    return videoBase.elements.videoId.value;
}

export function getCreatedAnnotationComment() {
    return videoBase.elements.createAnnotationTextarea.value;
}

export function getCurrentYoutubeTime() {
    return window.player.getCurrentTime();
}

/**
 * Hides the create annotation controls
 */
export function hideCreateAnnotationControls() {
    videoBase.elements.createAnnotation.classList.add('hidden');
    videoBase.elements.createAnnotationTextarea.value = '';
}

/**
 * Pauses the video
 */
export function pauseVideo() {
    window.player.pauseVideo();
}

/**
 * Plays the video
 */
export function playVideo() {
    window.player.playVideo();
}

/**
 * Sets up the annotation controls for the user so that they can write a new annotation. This should only be called
 * when the controls are already hidden, and they need to be displayed to the user.
 */
export function setupAnnotationControls() {
    // Set up controls
    videoBase.elements.newAnnotationTimestamp.innerText = videoController.getTimestampToDisplay(window.player.getCurrentTime());
    // Create annotation controls are hidden, so display them
    videoBase.elements.createAnnotation.classList.remove('hidden');
}

/**
 * Checks if the create annotation controls are hidden
 * @returns {boolean} - true if create annotation controls are hidden, false otherwise
 */
export function areCreateAnnotationControlsHidden() {
    return videoBase.elements.createAnnotation.classList.contains('hidden');
}

/**
 * 
 * @param target
 * @returns {boolean}
 */
export function isClickedButtonSubmitAnnotationButton(target) {
    return target.classList.contains(videoBase.classNames.submitAnnotation);
}

export function isClickedButtonShowRepliesButton(target) {
    return target.classList.contains(videoBase.classNames.toggleRepliesButton);
}

/**
 * Checks if the replies for the annotation (target) are currently hidden by looking for the hidden class
 * @param target - The annotation HTML element
 * @returns {boolean} - True if hidden, false otherwise
 */
export function areRepliesHidden(target) {
    return $(target.closest('.annotation-wrapper').lastElementChild).hasClass('hidden');
}

export function displayReplies(annotationElement) {
    $(annotationElement.lastElementChild).removeClass('hidden');
    changeToggleRepliesTextToHide(annotationElement);
}

export function hideReplies(annotationElement) {
    $(annotationElement.lastElementChild).addClass('hidden');
    changeToggleRepliesTextToShow(annotationElement);
}

export function changeToggleRepliesTextToShow(annotationElement) {
    annotationElement.querySelector('.' + videoBase.classNames.toggleRepliesButton).innerHTML = getToggleRepliesShowHTML();
}

export function getToggleRepliesShowHTML() {
    return 'Show Replies<span class="glyphicon glyphicon-menu-down"></span>';
}

export function changeToggleRepliesTextToHide(annotationElement) {
    annotationElement.querySelector('.' + videoBase.classNames.toggleRepliesButton).innerHTML = getToggleRepliesHideHTML();
}

export function getToggleRepliesHideHTML() {
    return 'Hide Replies<span class="glyphicon glyphicon-menu-up"></span>';
}

/**
 * Prepends the annotation HTML to the annotations body
 * @param annotationHTML - HTML created by the backend that represents a single annotation
 */
export function prependAnnotationToAnnotationsBody(annotationHTML){
    $(videoBase.elements.annotationsBody).prepend(annotationHTML);
}

export function renderReplyControls(annotationElement) {
    var html = getCreateReplyControlsHTML();
    
    annotationElement.querySelector('.panel').insertAdjacentHTML('afterend', html);
}

export function getCreateReplyControlsHTML() {

    return `<div class="create-reply-container">
                <div class="panel panel-default">
                    <div class="panel panel-heading reply-header">
                        <span class="annotation-options glyphicon glyphicon-option-horizontal" aria-hidden="true"></span>
                        <!--<span class="annotation-author-time block">-->
                        <!--</span>-->
                    </div>
                    <div class="panel-body reply-body">
                        <div class="reply-text-wrapper row">
                            <textarea></textarea>
                            <button type="button" class="submit-reply btn btn-success btn-sm">Submit</button>
                            <button type="button" class="cancel-reply btn btn-secondary btn-sm">Cancel</button>
                        </div>
                    </div>
                </div>
                <hr>
            </div>`;
}

export function isClickedButtonCreateReplyButton(target) {
    return target.classList.contains('reply-button');
}

export function isClickedButtonCancelCreateReplyButton(target) {
    return target.classList.contains(videoBase.classNames.cancelCreateReplyButton);
}

export function areCreateReplyControlsDisplayed(target) {
    return target.closest('.' + videoBase.classNames.annotationWrapper).getElementsByClassName(videoBase.classNames.createReplyControls).length > 0;
}

export function doesAnnotationHaveReplies(annotationElement) {
    return annotationElement.querySelector(videoBase.selectors.annotationReplies).innerHTML.trim() !== '';
}

export function isClickedButtonSubmitReplyButton(target) {
    return target.classList.contains(videoBase.classNames.submitReply);
}

export function prependReplyToRepliesBody(annotationElement, replyHTML){
    var repliesBody = annotationElement.querySelector('.' + videoBase.classNames.annotationReplies);
    $(repliesBody).prepend(replyHTML);
}

export function removeCreateReplyControls(annotationElement) {
    var createReplyControls = annotationElement.querySelector('.' + videoBase.classNames.createReplyControls);
    annotationElement.removeChild(createReplyControls);
}

export function getCreatedReplyText(annotationElement) {
    return annotationElement.querySelector('textarea').value;
}

export function doesAnnotationElementHaveToggleRepliesButton(annotationElement) {
    return annotationElement.getElementsByClassName(videoBase.classNames.toggleRepliesButton).length > 0;
}

export function renderToggleRepliesButton(annotationElement) {
    annotationElement.querySelector('.panel-body').insertAdjacentHTML('beforeend', getToggleRepliesDefaultHTML());
}

export function getToggleRepliesDefaultHTML() {
    return `
        <div class="toggle-replies-wrapper">
            <a class="annotation-text toggle-replies" href="#">${getToggleRepliesShowHTML()}</a>
        </div>
    `
}

/**
 * Using all of the annotation elements on the page, get the user id's and display names of
 * the users who posted those annotations and return them
 * @param annotationElements - An array of the annotation elements on the page
 * @return {object} - An object of user id's mapped to user display names. Ex: {1: 'Kyle Jones'}
 */
export function getUserIdsAndNames(annotationElements) {
    var userIdsAndNames = {};
    
    for(var i = 0; i < annotationElements.length; i++) {
        var annotationElement = annotationElements.item(i);

        addUserIdAndNameFromElement(annotationElement, userIdsAndNames);
    }
    
    return userIdsAndNames;
}

/**
 * Add the user id and display name of an individual annotation element to the userIdsAndNames param
 * @param annotationElement - The annotation where the user id and display name will come from
 * @param userIdsAndNames - A reference to an object where user ids are mapped to display names
 */
export function addUserIdAndNameFromElement(annotationElement, userIdsAndNames) {
    var userId = annotationElement.dataset['authorId'];

    if(userIdsAndNames[userId] === undefined) {
        userIdsAndNames[userId] = annotationElement.querySelector(videoBase.selectors.displayName).innerText;
        insertIntoFilterByUserDropdown(userId, userIdsAndNames[userId]);
    }
}

/**
 * Gets the element where the annotations are stored
 * @returns {HTMLElement}
 */
export function getAnnotationElements() {
    return document.getElementById(videoBase.idNames.annotationsBody);
}

/**
 * Gets the elements where the replies are stored
 * @returns {HTMLCollectionOf<Element>}
 */
export function getReplyElements() {
    return document.getElementsByClassName('reply-container');
}

export function insertIntoFilterByUserDropdown(userId, displayName) {
    var dropdown = videoBase.elements.annotations.querySelector(videoBase.selectors.filterAnnotationsList);
    
    var listElement = document.createElement('li');
    var listButton = document.createElement('a');
    var text = document.createTextNode(displayName);
    listButton.appendChild(text);
    listElement.appendChild(listButton);
    
    listButton.dataset['authorId'] = userId;
    listButton.href = '#';
    
    dropdown.appendChild(listElement);
}

/**
 * Renders the annotation options dropdown html for all annotations on the page
 */
export function renderAnnotationOptionsDropdowns() {
    for(var i = 0; i< videoController.state.annotationElements.children.length; i++) {
        var annotation = videoController.state.annotationElements.children.item(i);
        renderAnnotationOptionsDropdown(annotation);
    }
}

/**
 * Renders the reply options dropdown html for all annotations on the page
 */
export function renderReplyOptionsDropdowns() {
    for(var i = 0; i< videoController.state.replyElements.length; i++) {
        var reply = videoController.state.replyElements.item(i);
        
        renderReplyOptionsDropdown(reply);
    }
}

/**
 * Renders the reply options dropdown html for a single reply element
 * @param reply -  the reply element
 */
export function renderReplyOptionsDropdown(reply) {
    var replyOptionsUl = reply.querySelector('.reply-options-ul');

    if(reply.dataset['authorId'] === videoController.state.currentUserId) {
        var editListElement = document.createElement('li');
        var editListButton = document.createElement('a');
        var editText = document.createTextNode('Edit');
        editListButton.appendChild(editText);
        editListElement.appendChild(editListButton);
        editListButton.href = '#';
        editListButton.classList.add(videoBase.classNames.editReply);

        replyOptionsUl.appendChild(editListElement);

        var deleteListElement = document.createElement('li');
        var deleteListButton = document.createElement('a');
        var deleteText = document.createTextNode('Delete');
        deleteListButton.appendChild(deleteText);
        deleteListElement.appendChild(deleteListButton);
        deleteListButton.href = '#';
        deleteListButton.classList.add(videoBase.classNames.deleteReply);

        replyOptionsUl.appendChild(deleteListElement);
    } else {
        // reply is not owned by current user
        var listElement = document.createElement('li');
        var text = document.createTextNode('No Options');
        listElement.appendChild(text);

        replyOptionsUl.appendChild(listElement);
    }
}

/**
 * Unhides the message that tells the user that there are no annotations
 */
export function unhideNoAnnotationText() {
    videoBase.elements.annotations.querySelector(videoBase.selectors.noAnnotationsText).classList.remove('hidden');
}

/**
 * Hides the message that tells the user that there are no annotations
 */
export function hideNoAnnotationText() {
    videoBase.elements.annotations.querySelector(videoBase.selectors.noAnnotationsText).classList.add('hidden');
}

/**
 * Renders the annotation options dropdown html for a single annotation element
 * @param annotation -  the annotation element
 */
export function renderAnnotationOptionsDropdown(annotation) {
    var annotationOptionsUl = annotation.querySelector('.annotation-options-ul');

    if(annotation.dataset['authorId'] === videoController.state.currentUserId) {
        var editListElement = document.createElement('li');
        var editListButton = document.createElement('a');
        var editText = document.createTextNode('Edit');
        editListButton.appendChild(editText);
        editListElement.appendChild(editListButton);
        editListButton.href = '#';
        editListButton.classList.add(videoBase.classNames.editAnnotation);

        annotationOptionsUl.appendChild(editListElement);

        var deleteListElement = document.createElement('li');
        var deleteListButton = document.createElement('a');
        var deleteText = document.createTextNode('Delete');
        deleteListButton.appendChild(deleteText);
        deleteListElement.appendChild(deleteListButton);
        deleteListButton.href = '#';
        deleteListButton.classList.add(videoBase.classNames.deleteAnnotation);

        annotationOptionsUl.appendChild(deleteListElement);
    } else {
        // annotation is not owned by current user
        var listElement = document.createElement('li');
        var text = document.createTextNode('No Options');
        listElement.appendChild(text);

        annotationOptionsUl.appendChild(listElement);
    }
}

/**
 * Gets the HTML for the edit annotation controls
 * @returns {string}
 */
export function getEditAnnotationControlsHTML() {
    return `
        <div class="edit-annotation-text-wrapper row">
            <textarea></textarea>
            <button type="button" class="submit-edit-annotation btn btn-success btn-sm">Submit</button>
            <button type="button" class="cancel-edit-annotation btn btn-secondary btn-sm">Cancel</button>
        </div>
    `;
}

/**
 * Gets the HTML for the edit annotation controls
 * @returns {string}
 */
export function getEditReplyControlsHTML() {
    return `
        <div class="edit-reply-text-wrapper row">
            <textarea></textarea>
            <button type="button" class="submit-edit-reply btn btn-success btn-sm">Submit</button>
            <button type="button" class="cancel-edit-reply btn btn-secondary btn-sm">Cancel</button>
        </div>
    `;
}

/**
 * Hides the annotation text.
 * This is useful when you need to edit the text, and hide it before displaying the edit controls
 * @param annotationElementBody - The annotation element's body element
 */
export function hideAnnotationText(annotationElementBody) {
    annotationElementBody.querySelector(videoBase.selectors.annotationText).classList.add('hidden');
}

/**
 * Hides the reply text.
 * This is useful when you need to edit the text, and hide it before displaying the edit controls
 * @param replyElementBody - The reply element's body element
 */
export function hideReplyText(replyElementBody) {
    replyElementBody.querySelector(videoBase.selectors.replyText).classList.add('hidden');
}

/**
 * Removes the annotation element from the view.
 * Useful when an annotation gets deleted.
 * @param annotationElement - The annotation to be deleted
 */
export function removeAnnotation(annotationElement) {
    annotationElement.parentElement.removeChild(annotationElement);
}

export function removeReply(replyElement) {
    replyElement.parentElement.removeChild(replyElement);
}

/**
 * Unhides the annotation text.
 * Useful when closing the edit annotation controls and showing the existing annotation text again
 * @param annotationElementBody
 */
export function unhideAnnotationText(annotationElementBody) {
    annotationElementBody.querySelector(videoBase.selectors.annotationText).classList.remove('hidden');
}

/**
 * Updates the annotation text.
 * Useful after the annotation gets updated.
 * @param annotationElementBody - The annotation element's body element
 */
export function updateAnnotationText(annotationElementBody) {
    annotationElementBody.querySelector(videoBase.selectors.annotationText).innerText = document.querySelector(videoBase.selectors.editAnnotationText).value;
}

export function updateReplyText(replyElementBody) {
    replyElementBody.querySelector(videoBase.selectors.replyText).innerText = document.querySelector(videoBase.selectors.editReplyText).value;
}

/**
 * Renders the edit annotation controls
 * @param annotationElementBody - The annotation element's body element, where the controls will be displayed
 */
export function renderEditAnnotationControls(annotationElementBody) {
    var html = getEditAnnotationControlsHTML();
    
    $(annotationElementBody).prepend(html);
    
    // Add existing annotation text to textarea
    var annotationText = annotationElementBody.querySelector(videoBase.selectors.annotationText).innerText.trim();
    annotationElementBody.querySelector('textarea').value = annotationText;
}

/**
 * Renders the edit reply controls
 * @param replyElementBody - The reply element's body element, where the controls will be displayed
 */
export function renderEditReplyControls(replyElementBody) {
    var html = getEditReplyControlsHTML();

    $(replyElementBody).prepend(html);

    // Add existing annotation text to textarea
    var replyText = replyElementBody.querySelector(videoBase.selectors.replyText).innerText.trim();
    replyElementBody.querySelector('textarea').value = replyText;
}

/**
 * Removes the edit annotation controls from the view
 * @param annotationElementBody - The annotation element's body element
 */
export function removeEditAnnotationControls(annotationElementBody) {
    var editAnnotationTextWrapper = annotationElementBody.querySelector(videoBase.selectors.editAnnotationTextWrapper);
    annotationElementBody.removeChild(editAnnotationTextWrapper);
}

export function setCurrentUserId() {
    videoController.state.currentUserId = document.querySelector('#user-id').value;
}

export function doesVideoHaveAnnotations() {
    return videoBase.elements.annotations.querySelector(videoBase.selectors.noAnnotationsText) === null;
}

/**
 * Highlight or unhighlight (if already highlighted) the clickedEntryElement
 * @param clickedEntryElement - the filter dropdown entry that was clicked
 */
export function updateHighlightedUser(clickedEntryElement) {
    // add 'active' class to classlist if it already isn't in it
    if(clickedEntryElement.classList.contains('active')) {
        clickedEntryElement.classList.remove('active');
    } else {
        clickedEntryElement.classList.add('active');
    }
}

/**
 * Update the state object that holds the data about which user id's are currently
 * being filtered
 * @param clickedEntryElement - The entry in the filter dropdown that was clicked
 */
export function updateFilterUserIdState(clickedEntryElement) {
    var userId = clickedEntryElement.dataset['authorId'];
    
    if(videoController.state.filterUserId.has(userId)) {
        // user is already being filtered, turn filtering off for the user
        videoController.state.filterUserId.delete(userId);
    } else {
        // user is not being filtered, so turn it on for the user
        videoController.state.filterUserId.add(userId);
    }
}

/**
 * Filter the annotations so that the annotations that belong to any user
 * id's in the state object that holds the filter data are displayed, and any
 * user id's that aren't in it are hidden.
 */
export function filterAnnotationsByUserId() {
    var annotationElements = videoController.state.annotationElements.children;
    
    for(var i = 0; i< annotationElements.length; i++) {
        var annotation = annotationElements.item(i);
        filterAnnotationByUserId(annotation);
    }
}

/**
 * Hides or displays an annotation according to state's filter user data
 * @param annotation - The annotation element that will be displayed or hidden
 */
export function filterAnnotationByUserId(annotation) {
    var annotationUserId = annotation.dataset['authorId'];

    if(videoController.state.filterUserId.size === 0) {
        // No annotations are being filtered, display all
        annotation.classList.remove('hidden');
    } else if(videoController.state.filterUserId.has(annotationUserId)) {
        // Filter by user id, so make sure it's being displayed
        annotation.classList.remove('hidden');
    } else {
        // User id is not in filter, so hide it
        annotation.classList.add('hidden');
    }
}

export function isClickedButtonCancelEditReplyButton(clickedElement) {
    return clickedElement.classList.contains(videoBase.classNames.cancelEditReply);
}

export function removeEditReplyControls(replyElementBody) {
    var editReplyTextWrapper = replyElementBody.querySelector(videoBase.selectors.editReplyTextWrapper);
    replyElementBody.removeChild(editReplyTextWrapper);
}

export function unhideReplyText(replyElementBody) {
    replyElementBody.querySelector(videoBase.selectors.replyText).classList.remove('hidden');
}

export function getLibraryId() {
    return document.querySelector(videoBase.selectors.libraryId).value;
}

export function removeToggleRepliesWrapper(annotationElement) {
    var toggleRepliesWrapper = annotationElement.querySelector(videoBase.selectors.toggleRepliesWrapper);
    toggleRepliesWrapper.parentElement.removeChild(toggleRepliesWrapper);
}

export function getAnnotationTimestamp(annotationElement) {
    return annotationElement.dataset['timestamp'];
}

/**
 * Insert the new annotation after it's created
 * @param thisAnnotation - The 'this' keyword for the Annotation object
 * @param annotationHTML - The new annotation HTML
 */
export function insertNewAnnotation(thisAnnotation, annotationHTML) {
    let annotationElements = videoController.state.annotationElements.children;
    let priorAnnotationElement;
    let lastAnnotationTimestamp;
    if(annotationElements[annotationElements.length - 1].dataset['timestamp']) {
        lastAnnotationTimestamp = parseInt(annotationElements[annotationElements.length - 1].dataset['timestamp']);
    }
    
    if(thisAnnotation.timestamp > lastAnnotationTimestamp) {
        // New annotation has largest timestamp, must be added to end
        priorAnnotationElement = annotationElements[annotationElements.length - 1];
    } else {
        // Find annotation where new annotation will be added in front of it
        for(let i = 0; i < annotationElements.length; i++) {
            let timestamp = annotationElements[i].dataset['timestamp'];
            if(thisAnnotation.timestamp > parseInt(timestamp) && i !== 0) {
                // Found annotation with larger timestamp, will need to insert new annotation right before this one
                priorAnnotationElement = annotationElements[i];
            }
        }
    }
    
    // If priorAnnotationElement is undefined, then there are no annotations except the new one
    if(priorAnnotationElement !== undefined) {
        priorAnnotationElement.insertAdjacentHTML('afterend', annotationHTML);

        // Scroll to new annotation
        let annotationsElement = document.getElementById(videoBase.idNames.annotations);
        let newAnnotationElement = priorAnnotationElement.nextSibling;
        $(annotationsElement).animate({ scrollTop: newAnnotationElement.offsetTop}, 1000);

    } else if (priorAnnotationElement === undefined) {
        // Add new annotation, which will be only annotation
        prependAnnotationToAnnotationsBody(annotationHTML);
    }
}