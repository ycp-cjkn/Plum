import { elements as changeModeElements } from '../View/StreamModeBase.js';
import * as videoView from '../View/VideoView.js';
import * as videoController from './VideoController.js';

// Must be global so that it can be used in Youtube API event listener
var streamModeState = {
    isStreamModeEnabled: false,
    streamModeTimer: null
};

export function initializeChangeModeSelectEventListener() {
    changeModeElements.changeModeSelect.addEventListener('change', function(e) {
        var annotationElements = videoController.state.annotationElements.children;
        var selectionValue = e.target.value;
        let currentTimestamp = videoController.getVideoTimestamp();
        
        if(selectionValue === 'default') {
            // unhide all annotations
            for(let i = 0; i < annotationElements.length; i++) {
                let annotationElement = annotationElements[i];
                $(annotationElement).fadeIn();
            }
            
            // Clear streamModeTimer
            clearInterval(streamModeState.streamModeTimer);
        } else if (selectionValue === 'stream') {
            // hide all annotations that have timestamp greater than current timestamp
            for(let i = 0; i < annotationElements.length; i++) {
                let annotationElement = annotationElements[i];
                let annotationTimestamp = videoController.getAnnotationTimestamp(annotationElement);

                if(annotationTimestamp > currentTimestamp){
                    $(annotationElement).fadeOut();
                }
            }
            
            // start interval that will unhide annotations at their timestamps
            streamModeState.streamModeTimer = setInterval(function() { 
                handleAnnotationsDisplay(annotationElements, videoController.getVideoTimestamp())
            }, 500);
        }
    });
}

function handleAnnotationsDisplay(annotationElements, currentVideoTimestamp) {
    for(let i = 0; i < annotationElements.length; i++) {
        let annotationElement = annotationElements[i];
        let annotationTimestamp = videoController.getAnnotationTimestamp(annotationElement);

        if(annotationTimestamp <= currentVideoTimestamp){
            $(annotationElement).fadeIn();
        } else if (annotationTimestamp > currentVideoTimestamp) {
            $(annotationElement).fadeOut();
        }
    }
    console.log('running');
}