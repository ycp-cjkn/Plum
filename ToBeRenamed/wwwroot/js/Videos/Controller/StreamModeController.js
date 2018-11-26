import { elements as changeModeElements } from '../View/StreamModeBase.js';

var streamModeState = {
    annotationsData: {}
};

$(document).ready(function(){
    initializeChangeModeSelectEventListener();
});

export function initializeChangeModeSelectEventListener(annotationElements) {
    changeModeElements.changeModeSelect.addEventListener('change', function(e) {
        var selectionValue = e.target.value;
        
        if(selectionValue === 'default') {
            // unhide all annotations
        } else if (selectionValue === 'stream') {
            // hide all annotations
            for(var i = 0; i < annotationElements.length; i++) {
                annotationElements[i].classList.add('hidden');
            }
        }
    });
}