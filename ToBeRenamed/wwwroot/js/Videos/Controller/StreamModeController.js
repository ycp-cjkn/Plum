import { elements as changeModeElements } from '../View/StreamModeBase.js';
import * as videoView from '../View/VideoView.js';
import * as videoController from './VideoController.js';

var streamModeState = {
    isStreamModeEnabled: false
};

$(document).ready(function(){
    initializeChangeModeSelectEventListener();
});

export function initializeChangeModeSelectEventListener() {
    changeModeElements.changeModeSelect.addEventListener('change', function(e) {
        var annotationElements = videoController.state.annotationElements.children;
        var selectionValue = e.target.value;
        let currentTimestamp = videoController.getVideoTimestamp();
        
        if(selectionValue === 'default') {
            // unhide all annotations
            for(let i = 0; i < annotationElements.length; i++) {
                annotationElements[i].classList.remove('hidden');
            }
        } else if (selectionValue === 'stream') {
            // hide all annotations that have timestamp greater than current timestamp
            for(let i = 0; i < annotationElements.length; i++) {
                let annotationElement = annotationElements[i];
                let annotationTimestamp = videoController.getAnnotationTimestamp(annotationElement);
                
                if(annotationTimestamp > currentTimestamp){
                    annotationElement.classList.add('hidden');
                }
            }
        }
    });
}