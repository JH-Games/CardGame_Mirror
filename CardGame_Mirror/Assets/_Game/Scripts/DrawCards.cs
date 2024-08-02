using System.Collections.Generic;
using OknaaEXTENSIONS;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts {
    public class DrawCards : MonoBehaviour {
        public Transform cardHolder;
        public Image cardPrefab;
        public List<Sprite> cardImages;
        
        public int maxCards = 5;
        
        private List<Image> _drawnCards = new List<Image>();
        private Button drawButton;

        private void Start() {
            drawButton = GetComponent<Button>();
            drawButton.onClick.AddListener(DrawCard);
        }

        private void DrawCard() {
            if (_drawnCards.Count >= maxCards) return;
            
            var card = Instantiate(cardPrefab, cardHolder);
            card.sprite = cardImages.Random();
            card.transform.position = new Vector2(_drawnCards.Count * 2, 0);
            _drawnCards.Add(card);
        }
    }
}