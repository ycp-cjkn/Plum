var state = {
    player: null,
    userIdsAndNames: {},
    annotationElements: {},
    hasAnnotations: null,
    filterUserId : new Set(),
    currentUserId: null,
    userRole: null
};

// Initialize Youtube API
var tag = document.createElement('script');
tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
function onYouTubeIframeAPIReady() {
    state.player = new YT.Player('player', {
        height: '390',
        width: '640',
        videoId: getVideoUrl(),
        events: {
            'onReady': onPlayerReady
        }
    });
}
$(document).ready(function(){
    initialize();
});


function initialize() {
    // Initialize state
    initializeAnnotationElements();
    initializeUserIdsAndNames();
    initializeHasAnnotations();
    initializeCurrentUserId();
    initializeLibraryId();
    initializeUserRole(state.libraryId);

    // Intialize content
    initializeAnnotationOptionDropdowns();
    initializeNoAnnotationText();
    
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
    initalizeSubmitEditAnnotationButtonEventListener();
    initializeDeleteAnnotationButtonEventListener();

    // Initialize mutation observers
    initializeAnnotationElementsMutationObserver();
}

/**
 * Display the no annotations message to the user if there are no annotations
 */
function initializeNoAnnotationText() {
    if(state.annotationElements.children.length === 0) {
        // unhide no annotation text message
        unhideNoAnnotationText();
    }
}

/**
 * Initializes the event listener that listens for any clicks to the annotation options dropdown 
 * entries
 */
function initalizeFilterByUserDropdownContentEventListener() {
    elements.annotations.addEventListener('click', function(e){
        var target = e.target;
        
        if(target.classList.contains(classNames.editAnnotation)) {
            // edit annotation clicked
            
            // Get annotation element
            var annotationElement = target.closest(selectors.annotationWrapper);
            
            // Insert edit controls
            var annotationElementBody = annotationElement.querySelector(selectors.annotationBody);
            addEditAnnotationControls(annotationElementBody);
            
        } else if(target.classList.contains(classNames.deleteAnnotation)) {
            // delete annotation clicked
        }
    });
}

/**
 * Initializes an event listener that listeners for any clicks to the cancel annotation edit button
 */
function initializeCancelEditAnnotationButtonEventListener() {
    elements.annotations.addEventListener('click', function(e) {
        var target = e.target;
        
        if(target.classList.contains(classNames.cancelEditAnnotation)) {
            var annotationElementBody = target.closest(selectors.annotationWrapper).querySelector(selectors.annotationBody);
            removeEditControls(annotationElementBody);
            unhideAnnotationText(annotationElementBody);
        }
    })
}

/**
 * Initialize the event listener for the submit edited annotation button
 */
function initalizeSubmitEditAnnotationButtonEventListener() {
    elements.annotations.addEventListener('click', function(e) {
        var target = e.target;
        
        if(target.classList.contains('submit-edit-annotation')) {
            // submit edited annotation
            var annotationElement = target.closest(selectors.annotationWrapper);
            var annotationElementBody = annotationElement.querySelector(selectors.annotationBody);
            var annotationUserId = annotationElement.dataset['authorId'];
            var annotationId = annotationElement.dataset['id'];
            var newAnnotationComment = annotationElementBody.querySelector(selectors.editAnnotationText).value;
            
            var existingAnnotation = new ExistingAnnotation(annotationUserId, newAnnotationComment, annotationId);
            existingAnnotation.edit(annotationElementBody);
        }
    })
}

/**
 * Initializes the delete annotation button event listener
 */
function initializeDeleteAnnotationButtonEventListener() {
    elements.annotations.addEventListener('click', function(e) {
        var target = e.target;

        if(target.classList.contains('delete-annotation')) {
            // delete annotation
            var annotationElement = target.closest(selectors.annotationWrapper);
            var annotationUserId = annotationElement.dataset['authorId'];
            var annotationId = annotationElement.dataset['id'];

            var existingAnnotation = new ExistingAnnotation(annotationUserId, null, annotationId);
            existingAnnotation.delete(annotationElement);
        }
    })
}

/**
 * Removes the edit annotation controls from the view
 * @param annotationElementBody - the body of the annotation element
 */
function removeEditControls(annotationElementBody) {
    removeEditAnnotationControls(annotationElementBody);
}

/**
 * Add the edit annotation controls to the view
 * @param annotationElementBody - the body of the annotation element
 */
function addEditAnnotationControls(annotationElementBody) {
    hideAnnotationText(annotationElementBody);
    renderEditAnnotationControls(annotationElementBody);
}

function initializeAnnotationOptionDropdowns() {
    renderAnnotationOptionsDropdowns();
}

function initializeCurrentUserId() {
    setCurrentUserId();
}

function initializeLibraryId() {
    state.libraryId = getLibraryId();
}

function initializeUserRole(libraryId) {
    state.userRole = new Role(libraryId);
    state.userRole.fetchAndSet();
}

function initializeHasAnnotations() {
    state.hasAnnotations = doesVideoHaveAnnotations();
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
                        hideNoAnnotationText();
                    }
                    // A new annotation was added, make sure name exists in names to filter by
                    addUserIdAndNameFromElement(annotationElement, state.userIdsAndNames);
                    
                    // Hide/display the annotation according to the current user filter
                    filterAnnotationByUserId(annotationElement);
                    
                    // Add the HTML for the annotation options dropdown
                    // TODO - Check permissions before adding this
                    renderAnnotationOptionsDropdown(annotationElement);
                } else if (mutation.removedNodes.length === 1) {
                    // Put up "No annotations" text if no more annotations
                    if(mutation.target.children.length === 0) {
                        unhideNoAnnotationText();
                    }
                }
            }
        }
    };
    
    var observer = new MutationObserver(callback);
    observer.observe(state.annotationElements, config);
}

/**
 * Initializes the state variable that contains the user's id mapped to the user's display name
 */
function initializeUserIdsAndNames() {
    if(state.annotationElements.children.length !== 0) {
        // Only need to initialize if there are annotations
        state.userIdsAndNames = getUserIdsAndNames(state.annotationElements.children);
    }
}

/**
 * Initializes the state variable that contains the annotation elements.
 * This is helpful since the variable gets automatically updated as the DOM changes.
 */
function initializeAnnotationElements() {
    state.annotationElements = getAnnotationElements();
}

/**
 * Initializes the event listener for the filter dropdown
 */
function initializeFilterByUserDropdownEventListener() {
    var filterByUserListElement = elements.annotations.querySelector(selectors.filterAnnotationsList);
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
    updateHighlightedUser(clickedEntryElement);
    updateFilterUserIdState(clickedEntryElement);
    filterAnnotationsByUserId(clickedEntryElement);
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
            state.player.seekTo(time);
        }
    });
}

/**
 * Handles displaying and hiding the create annotation controls
 */
function initializeCreateAnnotationControlDisplayEventListener() {
    var createAnnotationElement = elements.createAnnotation;
    var createAnnotationButtonElement = elements.createAnnotationButton;
    // var createAnnotationTextarea = document.querySelector('.create-annotation-container textarea');
    var createAnnotationCancelButton = elements.cancelAnnotation;
    var newAnnotationTimestampElement = elements.newAnnotationTimestamp;
    // var noAnnotationTextElement = document.getElementById('no-annotation-text');

    createAnnotationButtonElement.addEventListener('click', function(e) {
        if (areCreateAnnotationControlsHidden()) {
            pauseVideo(state.player);
            setupAnnotationControls();
        } else {
            // Create annotation controls are displayed, so hide them
            hideCreateAnnotationControls();
        }
    });

    createAnnotationCancelButton.addEventListener('click', function(e) {
            hideCreateAnnotationControls();
        }
    );
}


function initializeSubmitAnnotationButtonEventListener() {
    var createAnnotationElement = document.querySelector('.create-annotation-container');
    createAnnotationElement.addEventListener('click', function(e) {
        var target = e.target;

        if (isClickedButtonSubmitAnnotationButton(target)) {
            var annotation = new Annotation(
                getVideoId(),
                getCreatedAnnotationComment(),
                getCurrentYoutubeTime(state.player)
            );
            
            annotation.submit();
        }
    });
}

function initializeShowRepliesButtonEventListener() {
    elements.annotations.addEventListener('click', function(e) {
        var target = e.target;
        var annotationElement = target.closest('.' + classNames.annotationWrapper);
        
        if(isClickedButtonShowRepliesButton(target) && areRepliesHidden(target)) {
            // show replies
            displayReplies(annotationElement);
        } else if (isClickedButtonShowRepliesButton(target) && !areRepliesHidden(target)) {
            // hide replies
            hideReplies(annotationElement);
        }
    })
}

function initializeCreateReplyButtonEventListener() {
    elements.annotations.addEventListener('click', function(e) {
        var target = e.target;
        var annotationElement = target.closest('.' + classNames.annotationWrapper);
        
        if(isClickedButtonCreateReplyButton(target) && !areCreateReplyControlsDisplayed(target)) {
            // Display create reply controls
            renderReplyControls(annotationElement);
            if(doesAnnotationHaveReplies(annotationElement)) {
                displayReplies(annotationElement);
            }
        } else if ((isClickedButtonCreateReplyButton(target) && areCreateReplyControlsDisplayed(target))
                    || isClickedButtonCancelCreateReplyButton(target)) {
            // Remove create reply controls
            removeCreateReplyControls(annotationElement);
        }
    })
}

function initializeSubmitReplyButtonEventListener() {
    elements.annotations.addEventListener('click', function(e) {
        var target = e.target;

        if (isClickedButtonSubmitReplyButton(target)) {
            var annotationElement = target.closest('.' + classNames.annotationWrapper);
            var annotationId = annotationElement.dataset.id;
            
            var reply = new Reply(
                annotationId,
                getCreatedReplyText(annotationElement)
            );

            reply.submit(annotationElement);
        }
    })
}




function getTimestampToDisplay(timestampNumber) {
    var totalSeconds = Math.floor(timestampNumber);
    var minutes = (totalSeconds / 60 < 10) ? "0" + Math.floor(totalSeconds / 60).toString() : Math.floor(totalSeconds / 60).toString()
    var seconds = (totalSeconds % 60 < 10) ? "0" + (totalSeconds % 60).toString() : (totalSeconds % 60).toString()

    return minutes+ ":" + seconds;
}
// YOUTUBE API

/**
 * Sets up youtube player
 * Loads IFrame player API. Used by Youtube API.
 * Creates an <iframe> (and Youtube player) after the API code downloads
 */
function createYoutubePlayer() {
    var tag = document.createElement('script');

    tag.src = "https://www.youtube.com/iframe_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
    function onYouTubeIframeAPIReady() {
        player = new YT.Player('player', {
            height: '390',
            width: '640',
            videoId: getVideoUrl(),
            events: {
                'onReady': onPlayerReady
            }
        });
    }
}

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

/**
 * Gets called when the video is ready
 * @param {object} event - This function should only ever have the onReady event as a parameter
 * @see {@link https://developers.google.com/youtube/iframe_api_reference#Events}
 */
function onPlayerReady(event) {
    // This function currently does nothing
    // event.target.playVideo();
}