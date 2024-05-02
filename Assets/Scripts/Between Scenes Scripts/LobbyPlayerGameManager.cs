using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class LobbyPlayerGameManager : MonoBehaviourPunCallbacks
{
    public LobbyHotbar hotbar;

    void Start() {
        if (GameManager.Instance == null) {
            return;
        }

        LoadItemsIntoHotbar();
    }

    public override void OnJoinedRoom() {
        if (!photonView.IsMine)
            return;

        LoadItemsIntoHotbar();
    }

    public void LoadItemsIntoHotbar() {
        LobbyHotBarData lobbyHotBarData = GameManager.Instance.GetLobbyHotBarData();
        hotbar.LoadItems(lobbyHotBarData.listOfHotBarItem);
    }

    public LobbyHotBarData ConvertHotBarToLobbyHotBarData() {
        List<GameObject> listOfHotBar = hotbar.GetItems();
        List<HotBarItem> hotBarItems = new List<HotBarItem>();

        foreach (GameObject obj in listOfHotBar){
            HotBarItem item = new HotBarItem(obj);
            hotBarItems.Add(item);
            Destroy(obj);
        }

        return new LobbyHotBarData(hotBarItems);
    }
}