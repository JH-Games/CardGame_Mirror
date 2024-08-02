using System.Collections.Generic;
using Mirror;
using OknaaEXTENSIONS;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour {
    public Transform PlayerArea;
    public Transform EnemyArea;
    public Transform DropZone;

    public Image Card;
    public List<Sprite> cardImages;
    public int maxCards = 5;

    private List<Image> _cards = new List<Image>();
    private List<Image> _drawnCards = new List<Image>();


    public override void OnStartClient() {
        base.OnStartClient();
        PlayerArea = GameObject.Find("PlayerArea").transform;
        EnemyArea = GameObject.Find("EnemyArea").transform;
        DropZone = GameObject.Find("DropZone").transform;
    }


    [Server]
    public override void OnStartServer() {
        base.OnStartServer();
        _cards.Add(Card);
        Debug.Log(_cards);
    }


    [Command]
    public void CmdDealCard() {
        for (int i = 0; i < maxCards; i++) {
            var card = Instantiate(Card, PlayerArea);
            card.sprite = cardImages.Random();
            NetworkServer.Spawn(card.gameObject, connectionToClient);
            RpcShowCard(card.gameObject, "Dealt");
            _drawnCards.Add(card);
        }
    }

    [ClientRpc]
    private void RpcShowCard(GameObject card, string type) {
        switch (type) {
            case "Dealt":
                card.transform.SetParent(authority ? PlayerArea : EnemyArea, false);
                break;
            case "Played":
                card.transform.SetParent(DropZone, false);
                break;
        }
    }


    public void PlayCard(GameObject card) {
        CmdPlayCard(card);
    }

    [Command]
    private void CmdPlayCard(GameObject card) {
        RpcShowCard(card, "Played");
    }
}