/**
 * 
 */
function loadIFramePlayerAPI() {
    
}

/**
 * Gets the video url
 * @returns {string} - The video url (identifier) used by the youtube API to get the video
 */
function getVideoUrl() {
    return elements.videoUrl.value;
}

/**
 * Gets the video id
 * @returns {string} - The id of the video used in the database
 */
function getVideoId() {
    return elements.videoId.value;
}

function getCreatedAnnotationComment() {
    return elements.createAnnotationTextarea.value;
}

function getCurrentYoutubeTime(player) {
    return player.getCurrentTime();
}

/**
 * Hides the create annotation controls
 */
function hideCreateAnnotationControls() {
    elements.createAnnotation.classList.add('hidden');
    elements.createAnnotationTextarea.value = '';
}

/**
 * Pauses the video
 * @param player - The youtube player
 */
function pauseVideo(player) {
    player.pauseVideo();
}

/**
 * Plays the video
 * @param player - the youtube player
 */
function playVideo(player) {
    player.playVideo();
}

/**
 * Sets up the annotation controls for the user so that they can write a new annotation. This should only be called
 * when the controls are already hidden, and they need to be displayed to the user.
 */
function setupAnnotationControls() {
    // Set up controls
    elements.newAnnotationTimestamp.innerText = getTimestampToDisplay(state.player.getCurrentTime());
    // Create annotation controls are hidden, so display them
    elements.createAnnotation.classList.remove('hidden');
}

/**
 * Checks if the create annotation controls are hidden
 * @returns {boolean} - true if create annotation controls are hidden, false otherwise
 */
function areCreateAnnotationControlsHidden() {
    return elements.createAnnotation.classList.contains('hidden');
}

/**
 * 
 * @param target
 * @returns {boolean}
 */
function isClickedButtonSubmitAnnotationButton(target) {
    return target.classList.contains(classNames.submitAnnotation);
}

function isClickedButtonShowRepliesButton(target) {
    return target.classList.contains(classNames.toggleRepliesButton);
}

/**
 * Checks if the replies for the annotation (target) are currently hidden by looking for the hidden class
 * @param target - The annotation HTML element
 * @returns {boolean} - True if hidden, false otherwise
 */
function areRepliesHidden(target) {
    return $(target.closest('.annotation-wrapper').lastElementChild).hasClass('hidden');;
}

function displayReplies(annotationElement) {
    $(annotationElement.lastElementChild).removeClass('hidden');
    changeToggleRepliesTextToHide(annotationElement);
}

function hideReplies(annotationElement) {
    $(annotationElement.lastElementChild).addClass('hidden');
    changeToggleRepliesTextToShow(annotationElement);
}

function changeToggleRepliesTextToShow(annotationElement) {
    annotationElement.querySelector('.' + classNames.toggleRepliesButton).innerHTML = getToggleRepliesShowHTML();
}

function getToggleRepliesShowHTML() {
    return 'Show Replies<span class="glyphicon glyphicon-menu-down"></span>';
}

function changeToggleRepliesTextToHide(annotationElement) {
    annotationElement.querySelector('.' + classNames.toggleRepliesButton).innerHTML = getToggleRepliesHideHTML();
}

function getToggleRepliesHideHTML() {
    return 'Hide Replies<span class="glyphicon glyphicon-menu-up"></span>';
}

/**
 * Prepends the annotation HTML to the annotations body
 * @param annotationHTML - HTML created by the backend that represents a single annotation
 */
function prependAnnotationToAnnotationsBody(annotationHTML){
    $(elements.annotationsBody).prepend(annotationHTML);
}

function renderReplyControls(annotationElement) {
    var html = getCreateReplyControlsHTML();
    
    annotationElement.querySelector('.panel').insertAdjacentHTML('afterend', html);
}

function getCreateReplyControlsHTML() {

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

function isClickedButtonCreateReplyButton(target) {
    return target.classList.contains('reply-button');
}

function isClickedButtonCancelCreateReplyButton(target) {
    return target.classList.contains(classNames.cancelCreateReplyButton);
}

function areCreateReplyControlsDisplayed(target) {
    return target.closest('.' + classNames.annotationWrapper).getElementsByClassName(classNames.createReplyControls).length > 0;
}

function doesAnnotationHaveReplies(annotationElement) {
    return annotationElement.getElementsByClassName(classNames.toggleRepliesButton).length > 0;
}

function isClickedButtonSubmitReplyButton(target) {
    return target.classList.contains(classNames.submitReply);
}

function prependReplyToRepliesBody(annotationElement, replyHTML){
    var repliesBody = annotationElement.querySelector('.' + classNames.annotationReplies);
    $(repliesBody).prepend(replyHTML);
}

function removeCreateReplyControls(annotationElement) {
    var createReplyControls = annotationElement.querySelector('.' + classNames.createReplyControls);
    annotationElement.removeChild(createReplyControls);
}

function getCreatedReplyText(annotationElement) {
    return annotationElement.querySelector('textarea').value;
}

function doesAnnotationElementHaveToggleRepliesButton(annotationElement) {
    return annotationElement.getElementsByClassName(classNames.toggleRepliesButton).length > 0;
}

function renderToggleRepliesButton(annotationElement) {
    annotationElement.querySelector('.panel-body').insertAdjacentHTML('beforeend', getToggleRepliesDefaultHTML());
}

function getToggleRepliesDefaultHTML() {
    return `
        <div class="toggle-replies-wrapper">
            <a class="annotation-text toggle-replies" href="#">${getToggleRepliesShowHTML()}</a>
        </div>
    `
}