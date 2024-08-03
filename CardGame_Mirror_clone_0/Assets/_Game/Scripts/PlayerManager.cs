using System.Collections.Generic;
using _Game.Scripts;
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

    private int _clientsCount;

    public override void OnStartClient() {
        base.OnStartClient();
        PlayerArea = GameObject.Find("PlayerArea").transform;
        EnemyArea = GameObject.Find("EnemyArea").transform;
        DropZone = GameObject.Find("DropZone").transform;
        
        // // get a list of the clients 
        // var clients = NetworkServer.connections;
        // _clientsCount++;
        //
        // if (_clientsCount == 1) {
        //     PlayerArea = GameObject.Find("PlayerArea").transform;
        //     EnemyArea00 = GameObject.Find("EnemyArea_00").transform;
        //     EnemyArea01 = GameObject.Find("EnemyArea_01").transform;
        // } else if (_clientsCount == 2) {
        //     PlayerArea = GameObject.Find("EnemyArea_00").transform;
        //     EnemyArea00 = GameObject.Find("PlayerArea").transform;
        //     EnemyArea01 = GameObject.Find("EnemyArea_01").transform;
        // } else if (_clientsCount == 3) {
        //     PlayerArea = GameObject.Find("EnemyArea_01").transform;
        //     EnemyArea00 = GameObject.Find("EnemyArea_00").transform;
        //     EnemyArea01 = GameObject.Find("PlayerArea").transform;
        // }
        // DropZone = GameObject.Find("DropZone").transform;

    }


    [Server]
    public override void OnStartServer() {
        base.OnStartServer();
        _cards.Add(Card);
    }


    [Command]
    public void CmdDealCardInServer() {
        for (int i = 0; i < maxCards; i++) {
            var card = Instantiate(Card);
            card.sprite = cardImages.Random();
            NetworkServer.Spawn(card.gameObject, connectionToClient);
            RpcShowCardInClient(card.gameObject, "Dealt");
            _drawnCards.Add(card);
        }
    }

    [ClientRpc]
    private void RpcShowCardInClient(GameObject card, string type) {
        switch (type) {
            case "Dealt":
                card.transform.SetParent(isOwned ? PlayerArea : EnemyArea, false);
                card.GetComponent<CardFlipper>().SetFace();
                break;
            case "Played":
                card.transform.SetParent(DropZone, false);
                card.GetComponent<CardFlipper>().Show();
                break;
        }
    }


    public void PlayCard(GameObject card) => CmdPlayCardInServer(card);

    [Command]
    private void CmdPlayCardInServer(GameObject card) {
        RpcShowCardInClient(card, "Played");
    }
}