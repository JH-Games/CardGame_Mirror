using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts {
    public class DragAndDrop : NetworkBehaviour {
        private EventTrigger eventTrigger;
        
        private PlayerManager PlayerManager;
        private bool _isDraggable;
        
        private bool _isDragging;
        private Vector2 _startPosition;
        private Transform _startParent;
        private Vector2 _offset;
        private Camera _camera;
        

        private void Start() {
            _isDraggable = isOwned;
            
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
            if (!_isDragging) return;
            
            var newPos = _camera.ScreenToWorldPoint((Vector2)Input.mousePosition + _offset);
            newPos.z = 1;
            transform.position = newPos;
        }

        private void StartDrag(BaseEventData arg0) {
            if (!_isDraggable) return;
            
            _isDragging = true;
            _startParent = transform.parent;
            _startPosition = transform.position;
            var mousePosition = _camera.ScreenToWorldPoint((Vector2)Input.mousePosition);
            _offset = _startPosition - new Vector2(mousePosition.x, mousePosition.y);
        }
        
        private void EndDrag(BaseEventData arg0) {
            if (!_isDraggable) return;

            _isDragging = false;
            if (CardsManager.Instance.IsOverDropZone(Input.mousePosition)) {
                transform.SetParent(CardsManager.Instance._dropZone, false);
                _isDraggable = false;
                PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
                PlayerManager.PlayCard(gameObject);
            }
            else {
                transform.position = _startPosition;
                transform.SetParent(_startParent, false);
            }
                
            
            
        }

    }
}