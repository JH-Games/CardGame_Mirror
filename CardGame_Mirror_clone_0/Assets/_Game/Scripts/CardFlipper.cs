using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts {
    public class CardFlipper : NetworkBehaviour {
        public Sprite CardFront;
        public Sprite CardBack;
        
        private Image _image;
        private bool _isHidden;
        private bool _isInit;


        private void Awake() {
            if (isLocalPlayer && !_isInit) Init();
        }

        private void Init() {
            _image = GetComponent<Image>();
            _image.sprite = CardBack;
            _isHidden = true;
            _isInit = true;
        }

        public void Show() {
            if (!_isInit) Init();
            _isHidden = false;
            _image.sprite = CardFront;
        }
        
        public void Hide() {
            if (!_isInit) Init();
            _image.sprite = CardBack;
            _isHidden = true;
        }

        public void SetFace() {
            if (!_isInit) Init();
            if (isOwned) RpcFlip();
        }

        private void RpcFlip() {
            _image.sprite = _isHidden ? CardFront : CardBack;
            _isHidden = !_isHidden;
        }
         
    }
}