using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts {
    public class DragAndDrop : MonoBehaviour {
        private EventTrigger eventTrigger;
        
        private bool _isDragging;
        private Vector2 _startPosition;
        private Vector2 _offset;
        private Camera _camera;

        private void Start() {
            _camera = Camera.main;
            
            eventTrigger = GetComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();
            
            var beginDrag = new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag };
            beginDrag.callback.AddListener(StartDrag);
            
            var endDrag = new EventTrigger.Entry {eventID = EventTriggerType.EndDrag};
            endDrag.callback.AddListener(EndDrag);
            
            eventTrigger.triggers.Add(beginDrag);
            eventTrigger.triggers.Add(endDrag);
        }

     
        private void Update() {
            if (_isDragging) {
                var newPos = _camera.ScreenToWorldPoint((Vector2)Input.mousePosition + _offset);
                newPos.z = 0;
                transform.position = newPos;
                
                // _transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }

        private void StartDrag(BaseEventData arg0) {
            _isDragging = true;
            _startPosition = transform.position;
            var mousePosition = _camera.ScreenToWorldPoint((Vector2)Input.mousePosition);
            _offset = _startPosition - new Vector2(mousePosition.x, mousePosition.y);
        }
        
        private void EndDrag(BaseEventData arg0) {
            _isDragging = false;
        }

    }
}