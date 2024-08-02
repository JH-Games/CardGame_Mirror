using System.Collections.Generic;
using Mirror;
using OknaaEXTENSIONS;
using OknaaEXTENSIONS.CustomWrappers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts {
    public class CardsManager : Singleton<CardsManager> {
        private const string DropZoneTag = "DropZone";
        
        private PlayerManager PlayerManager;

        public Button _drawButton;
        public Transform _dropZone;
        
        

        private void Start() {
            _drawButton.onClick.AddListener(DrawCard);
        }

        private void DrawCard() {
            PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
            PlayerManager.CmdDealCard();
        }
        
        
        public bool IsOverDropZone(Vector2 position) {
            var pointer = new PointerEventData(EventSystem.current) {
                position = position
            };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);
            foreach (var result in raycastResults) {
                if (result.gameObject.CompareTag(DropZoneTag)) {
                    return true;
                }
            }

            return false;
        }
    }
}