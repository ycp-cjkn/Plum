import * as streamModeController from './StreamModeController.js';
import * as videoBase from '../View/base.js';
import * as videoView from '../View/VideoView.js';
import * as videoModel from '../Model/VideoModel.js';

export var state = {};

$(document).ready(function(){
    state = {
        userIdsAndNames: {},
        annotationElements: {},
        hasAnnotations: null,
        filterUserId : new Set(),
        currentUserId: null,
        replyElements: {},
        userRole: null
    };
    
    initialize();
});


function initialize() {
    // Initialize state
    initializeAnnotationElements();
    initializeUserIdsAndNames();
    initializeHasAnnotations();
    initializeCurrentUserId();
    initializeReplyElements();
    initializeLibraryId();
    initializeUserRole(state.libraryId);

    // Initialize content
    initializeAnnotationOptionDropdowns();
    initializeNoAnnotationText();
    initializeReplyOptionDropdowns();
    
    // Initialize event listeners
    initializeTimestampClickEventListener();
    initializeCreateAnnotationControlDisplayEventListener();
    initializeSubmitAnnotationButtonEventListener();
    initializeShowRepliesButtonEventListener();
    initializeCreateReplyButtonEventListener();
    initializeSubmitReplyButtonEventListener();
    initializeFilterByUserDropdownEventListener();
    initalizeFilterByUserDropdownContentEventListener();
    initializeCancelEditAnnotationButtonEventListener();
    initializeSubmitEditAnnotationButtonEventListener();
    initializeDeleteAnnotationButtonEventListener();
    initializeReplyOptionsDropdownContentEventListener();
    initializeCancelReplyButtonEventListener();
    initializeSubmitEditReplyButtonEventListener();
    initializeDeleteReplyButtonEventListener();
    streamModeController.initializeChangeModeSelectEventListener();

    // Initialize mutation observers
    initializeAnnotationElementsMutationObserver();
    initializeReplyElementsMutationObserver();
}

/**
 * Display the no annotations message to the user if there are no annotations
 */
function initializeNoAnnotationText() {
    if(state.annotationElements.children.length === 0) {
        // unhide no annotation text message
        videoView.unhideNoAnnotationText();
    }
}

/**
 * Initializes the event listener that listens for any clicks to the annotation options dropdown 
 * entries
 */
function initalizeFilterByUserDropdownContentEventListener() {
    videoBase.elements.annotations.addEventListener('click', function(e){
        var target = e.target;
        
        if(target.classList.contains(videoBase.classNames.editAnnotation)) {
            // edit annotation clicked
            
            // Get annotation element
            var annotationElement = target.closest(videoBase.selectors.annotationWrapper);
            
            // Insert edit controls
            var annotationElementBody = annotationElement.querySelector(videoBase.selectors.annotationBody);
            addEditAnnotationControls(annotationElementBody);
            
        } else if(target.classList.contains(videoBase.classNames.deleteAnnotation)) {
            // delete annotation clicked
        }
    });
}

/**
 * Initializes the event listener that listens for any clicks to the annotation options dropdown
 * entries
 */
function initializeReplyOptionsDropdownContentEventListener() {
    videoBase.elements.annotations.addEventListener('click', function(e){
        var target = e.target;

        if(target.classList.contains(videoBase.classNames.editReply)) {
            // edit reply clicked

            // Get reply element
            var replyElement = target.closest(videoBase.selectors.replyContainer);

            // Insert edit controls
            var replyElementBody = replyElement.querySelector(videoBase.selectors.replyBody);
            addEditReplyControls(replyElementBody);

        } else if(target.classList.contains(videoBase.classNames.deleteAnnotation)) {
            // delete annotation clicked
        }
    });
}

/**
 * Initializes an event listener that listeners for any clicks to the cancel annotation edit button
 */
function initializeCancelEditAnnotationButtonEventListener() {
    videoBase.elements.annotations.addEventListener('click', function(e) {
        var target = e.target;
        
        if(target.classList.contains(videoBase.classNames.cancelEditAnnotation)) {
            var annotationElementBody = target.closest(videoBase.selectors.annotationWrapper).querySelector(videoBase.selectors.annotationBody);
            removeEditControls(annotationElementBody);
            videoView.unhideAnnotationText(annotationElementBody);
        }
    })
}

/**
 * Initialize the event listener for the submit edited annotation button
 */
function initializeSubmitEditAnnotationButtonEventListener() {
    videoBase.elements.annotations.addEventListener('click', function(e) {
        var target = e.target;
        
        if(target.classList.contains('submit-edit-annotation')) {
            // submit edited annotation
            var annotationElement = target.closest(videoBase.selectors.annotationWrapper);
            var annotationElementBody = annotationElement.querySelector(videoBase.selectors.annotationBody);
            var annotationUserId = annotationElement.dataset['authorId'];
            var annotationId = annotationElement.dataset['id'];
            var newAnnotationComment = annotationElementBody.querySelector(videoBase.selectors.editAnnotationText).value;
            
            var existingAnnotation = new ExistingAnnotation(annotationUserId, newAnnotationComment, annotationId);
            existingAnnotation.edit(annotationElementBody);
        }
    })
}

/**
 * Initializes the delete annotation button event listener
 */
function initializeDeleteAnnotationButtonEventListener() {
    videoBase.elements.annotations.addEventListener('click', function(e) {
        var target = e.target;

        if(target.classList.contains('delete-annotation')) {
            // delete annotation
            var annotationElement = target.closest(videoBase.selectors.annotationWrapper);
            var annotationUserId = annotationElement.dataset['authorId'];
            var annotationId = annotationElement.dataset['id'];

            var existingAnnotation = new videoModel.ExistingAnnotation(annotationUserId, null, annotationId);
            existingAnnotation.delete(annotationElement);
        }
    })
}

/**
 * Initializes the delete reply button event listener
 */
function initializeDeleteReplyButtonEventListener() {
    videoBase.elements.annotations.addEventListener('click', function(e) {
        var target = e.target;

        if(target.classList.contains(videoBase.classNames.deleteReply)) {
            // delete reply
            var replyElement = target.closest(videoBase.selectors.replyContainer);
            var replyUserId = replyElement.dataset['authorId'];
            var replyId = replyElement.dataset['id'];

            var existingReply = new videoModel.ExistingReply(null, replyId, replyUserId);
            existingReply.delete(replyElement);
        }
    })
}

/**
 * Removes the edit annotation controls from the view
 * @param annotationElementBody - the body of the annotation element
 */
export function removeEditControls(annotationElementBody) {
    videoView.removeEditAnnotationControls(annotationElementBody);
}

/**
 * Add the edit annotation controls to the view
 * @param annotationElementBody - the body of the annotation element
 */
function addEditAnnotationControls(annotationElementBody) {
    videoView.hideAnnotationText(annotationElementBody);
    videoView.renderEditAnnotationControls(annotationElementBody);
}

/**
 * Add the edit reply controls to the view
 * @param replyElementBody - the body of the reply element
 */
function addEditReplyControls(replyElementBody) {
    videoView.hideReplyText(replyElementBody);
    videoView.renderEditReplyControls(replyElementBody);
}

function initializeAnnotationOptionDropdowns() {
    videoView.renderAnnotationOptionsDropdowns();
}

function initializeReplyOptionDropdowns() {
    videoView.renderReplyOptionsDropdowns();
}

function initializeCurrentUserId() {
    videoView.setCurrentUserId();
}

function initializeLibraryId() {
    state.libraryId = videoView.getLibraryId();
}

function initializeUserRole(libraryId) {
    state.userRole = new videoModel.Role(libraryId);
    state.userRole.fetchAndSet();
}

function initializeHasAnnotations() {
    state.hasAnnotations = videoView.doesVideoHaveAnnotations();
}

/**
 * Initializes the mutation observer for the annotation elements.
 * Helpful to catch any changes to the list of annotations
 */
function initializeAnnotationElementsMutationObserver() {
    var config = { childList: true };
    var callback = function(mutationsList, observer) {
        for(var mutation of mutationsList) {
            if(mutation.type === 'childList') {
                if(mutation.addedNodes.length !== 0) {
                    var annotationElement = mutation.addedNodes[0];
                    // If there were previously no annotations, hide the no annotations text
                    if(mutation.target.children.length === 1) {
                        // Annotation count was previously 0 before this created one
                        videoView.hideNoAnnotationText();
                    }
                    // A new annotation was added, make sure name exists in names to filter by
                    videoView.addUserIdAndNameFromElement(annotationElement, state.userIdsAndNames);
                    
                    // Hide/display the annotation according to the current user filter
                    videoView.filterAnnotationByUserId(annotationElement);
                    
                    // Add the HTML for the annotation options dropdown
                    // TODO - Check permissions before adding this
                    videoView.renderAnnotationOptionsDropdown(annotationElement);
                    
                    // Add the mutation observer for the new replies container
                    createReplyElementsMutationObserver(annotationElement);
                } else if (mutation.removedNodes.length === 1) {
                    // Put up "No annotations" text if no more annotations
                    if(mutation.target.children.length === 0) {
                        videoView.unhideNoAnnotationText();
                    }
                }
            }
        }
    };
    
    var observer = new MutationObserver(callback);
    observer.observe(state.annotationElements, config);
}

/**
 * Initializes the mutation observer for the reply elements.
 * Helpful to catch any changes to the list of replies
 */
function createReplyElementsMutationObserver(annotationElement) {
    var config = { childList: true };
    var callback = function(mutationsList, observer) {
        for(var mutation of mutationsList) {
            if(mutation.type === 'childList') {
                if(mutation.addedNodes.length !== 0) {
                    var replyElement = mutation.addedNodes[0];
                    
                    // Add the HTML for the reply options dropdown
                    // TODO - Check permissions before adding this
                    videoView.renderReplyOptionsDropdown(replyElement);
                }
            }
        }
    };
    
    var observer = new MutationObserver(callback);
    var repliesContainer = annotationElement.querySelector('.annotation-replies');
    observer.observe(repliesContainer, config);
}

function initializeReplyElementsMutationObserver() {
    for(var i = 0; i < state.annotationElements.children.length; i++){
        var annotationElement = state.annotationElements.children.item(i);
        createReplyElementsMutationObserver(annotationElement);
    }
}

/**
 * Initializes the state variable that contains the user's id mapped to the user's display name
 */
function initializeUserIdsAndNames() {
    if(state.annotationElements.children.length !== 0) {
        // Only need to initialize if there are annotations
        state.userIdsAndNames = videoView.getUserIdsAndNames(state.annotationElements.children);
    }
}

/**
 * Initializes the state variable that contains the annotation elements.
 * This is helpful since the variable gets automatically updated as the DOM changes.
 */
function initializeAnnotationElements() {
    state.annotationElements = videoView.getAnnotationElements();
}

/**
 * Initializes the state variable that contains the reply elements.
 * This is helpful since the variable gets automatically updated as the DOM changes.
 */
function initializeReplyElements() {
    state.replyElements = videoView.getReplyElements();
}

/**
 * Initializes the event listener for the filter dropdown
 */
function initializeFilterByUserDropdownEventListener() {
    var filterByUserListElement = videoBase.elements.annotations.querySelector(videoBase.selectors.filterAnnotationsList);
    filterByUserListElement.addEventListener('click', function(e) {
        // Stop dropdown from closing
        e.stopPropagation();
        
        var clickedEntryElement = e.target;
        
        // User clicked in filter dropdown
        updateUserFilter(clickedEntryElement);
    });
}

/**
 * Executes after a user clicks on an entry in the filter dropdown
 * @param clickedEntryElement - The entry in the filter dropdown that got clicked
 */
function updateUserFilter(clickedEntryElement) {
    videoView.updateHighlightedUser(clickedEntryElement);
    videoView.updateFilterUserIdState(clickedEntryElement);
    videoView.filterAnnotationsByUserId(clickedEntryElement);
}

/**
 * Handles clicking timestamp to go to a time in video
 */
function initializeTimestampClickEventListener() {
    var annotationsElement = document.querySelector("#annotations");
    annotationsElement.addEventListener('click', function(e) {
        var targetElement = e.target;

        if (targetElement.classList.contains('timestamp')) {
            var time = parseFloat(targetElement.dataset.timestamp);
            window.player.seekTo(time);
        }
    });
}

/**
 * Handles displaying and hiding the create annotation controls
 */
function initializeCreateAnnotationControlDisplayEventListener() {
    var createAnnotationElement = videoBase.elements.createAnnotation;
    var createAnnotationButtonElement = videoBase.elements.createAnnotationButton;
    // var createAnnotationTextarea = document.querySelector('.create-annotation-container textarea');
    var createAnnotationCancelButton = videoBase.elements.cancelAnnotation;
    var newAnnotationTimestampElement = videoBase.elements.newAnnotationTimestamp;
    // var noAnnotationTextElement = document.getElementById('no-annotation-text');

    createAnnotationButtonElement.addEventListener('click', function(e) {
        if (videoView.areCreateAnnotationControlsHidden()) {
            videoView.pauseVideo(window.player);
            videoView.setupAnnotationControls();
        } else {
            // Create annotation controls are displayed, so hide them
            videoView.hideCreateAnnotationControls();
        }
    });

    createAnnotationCancelButton.addEventListener('click', function(e) {
        videoView.hideCreateAnnotationControls();
    });
}


function initializeSubmitAnnotationButtonEventListener() {
    var createAnnotationElement = document.querySelector('.create-annotation-container');
    createAnnotationElement.addEventListener('click', function(e) {
        var target = e.target;

        if (videoView.isClickedButtonSubmitAnnotationButton(target)) {
            var annotation = new videoModel.Annotation(
                videoView.getVideoId(),
                videoView.getCreatedAnnotationComment(),
                videoView.getCurrentYoutubeTime(window.player)
            );
            
            annotation.submit();
        }
    });
}

function initializeShowRepliesButtonEventListener() {
    videoBase.elements.annotations.addEventListener('click', function(e) {
        var target = e.target;
        var annotationElement = target.closest('.' + videoBase.classNames.annotationWrapper);
        
        if(videoView.isClickedButtonShowRepliesButton(target) && videoView.areRepliesHidden(target)) {
            // show replies
            videoView.displayReplies(annotationElement);
        } else if (videoView.isClickedButtonShowRepliesButton(target) && !videoView.areRepliesHidden(target)) {
            // hide replies
            videoView.hideReplies(annotationElement);
        }
    })
}

function initializeCreateReplyButtonEventListener() {
    videoBase.elements.annotations.addEventListener('click', function(e) {
        var target = e.target;
        var annotationElement = target.closest('.' + videoBase.classNames.annotationWrapper);
        
        if(videoView.isClickedButtonCreateReplyButton(target) && !videoView.areCreateReplyControlsDisplayed(target)) {
            // Display create reply controls
            videoView.renderReplyControls(annotationElement);
            if(videoView.doesAnnotationHaveReplies(annotationElement)) {
                videoView.displayReplies(annotationElement);
            }
        } else if ((videoView.isClickedButtonCreateReplyButton(target) && videoView.areCreateReplyControlsDisplayed(target))
                    || videoView.isClickedButtonCancelCreateReplyButton(target)) {
            // Remove create reply controls
            videoView.removeCreateReplyControls(annotationElement);
        }
    })
}

function initializeSubmitReplyButtonEventListener() {
    videoBase.elements.annotations.addEventListener('click', function(e) {
        var target = e.target;

        if (videoView.isClickedButtonSubmitReplyButton(target)) {
            var annotationElement = target.closest('.' + videoBase.classNames.annotationWrapper);
            var annotationId = annotationElement.dataset.id;
            
            var reply = new videoModel.Reply(
                annotationId,
                videoView.getCreatedReplyText(annotationElement)
            );

            reply.submit(annotationElement);
        }
    })
}

function initializeCancelReplyButtonEventListener() {
    videoBase.elements.annotations.addEventListener('click', function(e) {
        var target = e.target;
        
        if(videoView.isClickedButtonCancelEditReplyButton(target)) {
            var replyElementBody = target.closest(videoBase.selectors.replyContainer).querySelector(videoBase.selectors.replyBody);
            videoView.removeEditReplyControls(replyElementBody);
            videoView.unhideReplyText(replyElementBody);
        }
    })
}

/**
 * Initialize the event listener for the submit edited reply button
 */
function initializeSubmitEditReplyButtonEventListener() {
    videoBase.elements.annotations.addEventListener('click', function(e) {
        var target = e.target;

        if(target.classList.contains(videoBase.classNames.submitEditReply)) {
            // submit edited reply
            var replyElement = target.closest(videoBase.selectors.replyContainer);
            var replyElementBody = replyElement.querySelector(videoBase.selectors.replyBody);
            var replyUserId = replyElement.dataset['authorId'];
            var replyId = replyElement.dataset['id'];
            var newReplyText = replyElementBody.querySelector(videoBase.selectors.editReplyText).value;

            var existingReply = new videoModel.ExistingReply(newReplyText, replyId, replyUserId);
            existingReply.edit(replyElementBody);
        }
    })
}

export function getTimestampToDisplay(timestampNumber) {
    var totalSeconds = Math.floor(timestampNumber);
    var minutes = (totalSeconds / 60 < 10) ? "0" + Math.floor(totalSeconds / 60).toString() : Math.floor(totalSeconds / 60).toString();
    var seconds = (totalSeconds % 60 < 10) ? "0" + (totalSeconds % 60).toString() : (totalSeconds % 60).toString();

    return minutes+ ":" + seconds;
}
// YOUTUBE API

/**
 * Sets up youtube player
 * Loads IFrame player API. Used by Youtube API.
 * Creates an <iframe> (and Youtube player) after the API code downloads
 */
// function createYoutubePlayer() {
//     var tag = document.createElement('script');
//
//     tag.src = "https://www.youtube.com/iframe_api";
//     var firstScriptTag = document.getElementsByTagName('script')[0];
//     firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
//     function onYouTubeIframeAPIReady() {
//         player = new YT.Player('player', {
//             height: '390',
//             width: '640',
//             videoId: getVideoUrl(),
//             events: {
//                 'onReady': onPlayerReady
//             }
//         });
//     }
// }

// var tag = document.createElement('script');
//
// tag.src = "https://www.youtube.com/iframe_api";
// var firstScriptTag = document.getElementsByTagName('script')[0];
// firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
//
// state.player =  new YT.Player('player', {
//     height: '390',
//     width: '640',
//     videoId: getVideoUrl(),
//     events: {
//         'onReady': onPlayerReady
//     }
// });