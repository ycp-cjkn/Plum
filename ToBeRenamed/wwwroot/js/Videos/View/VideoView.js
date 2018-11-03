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

function displayReplies(target) {
    $(target.closest('.annotation-wrapper').lastElementChild).removeClass('hidden');
}

function hideReplies(target) {
    $(target.closest('.annotation-wrapper').lastElementChild).addClass('hidden');
}

function changeToggleRepliesTextToShow(element) {
    element.innerHTML = 'Show Replies<span class="glyphicon glyphicon-menu-down"></span>'
}

function changeToggleRepliesTextToHide(element) {
    element.innerHTML = 'Hide Replies<span class="glyphicon glyphicon-menu-up"></span>'
}
/**
 * Prepends the annotation HTML to the annotations body
 * @param annotationHTML - HTML created by the backend that represents a single annotation
 */
function prependAnnotationToAnnotationsBody(annotationHTML){
    $(elements.annotationsBody).prepend(annotationHTML);
}