import { elements as changeModeElements } from '../View/StreamModeBase.js';
import * as videoView from '../View/VideoView.js';
import {state as videoState}  from './VideoController.js';

var streamModeState = {
    annotationsData: {}
};

$(document).ready(function(){
    initializeChangeModeSelectEventListener();
});

export function initializeChangeModeSelectEventListener() {
    changeModeElements.changeModeSelect.addEventListener('change', function(e) {
        var annotationElements = videoState.annotationElements.children;
        var selectionValue = e.target.value;
        
        if(selectionValue === 'default') {
            // unhide all annotations
            for(let i = 0; i < annotationElements.length; i++) {
                annotationElements[i].classList.remove('hidden');
            }
        } else if (selectionValue === 'stream') {
            // hide all annotations
            for(let i = 0; i < annotationElements.length; i++) {
                annotationElements[i].classList.add('hidden');
            }
        }
    });
}