var state = {
    player: null,
    userIdsAndNames: {},
    annotationElements: {}
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
    initalizeAnnotationElements();
    initializeUserIdsAndNames();
    
    // Initialize event listeners
    initializeTimestampClickEventListener();
    initializeCreateAnnotationControlDisplayEventListener();
    initializeSubmitAnnotationButtonEventListener();
    initializeShowRepliesButtonEventListener();
    initializeCreateReplyButtonEventListener();
    initializeSubmitReplyButtonEventListener();

    // Initialize mutation observers
    initializeAnnotationElementsMutationObserver();
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
                    // A new annotation was added, make sure name exists in names to filter by
                    addUserIdAndNameFromElement(mutation.addedNodes[0], state.userIdsAndNames);
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
    state.userIdsAndNames = getUserIdsAndNames(state.annotationElements.children);
}

/**
 * Initializes the state variable that contains the annotation elements.
 * This is helpful since the variable gets automatically updated as the DOM changes.
 */
function initalizeAnnotationElements() {
    state.annotationElements = getAnnotationElements();
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
        var annotationElement = target.closest('.' + classNames.annotationWrapper);
        var annotationId = annotationElement.dataset.id;

        if (isClickedButtonSubmitReplyButton(target)) {
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