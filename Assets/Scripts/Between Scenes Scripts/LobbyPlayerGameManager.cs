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
        List<GameObject> objectsToDestroy = new List<GameObject>();

        foreach (GameObject obj in listOfHotBar) {
            if (obj != null) {
                HotBarItem item = new HotBarItem(obj);
                hotBarItems.Add(item);
                

                if (obj.tag == "Pot") {
                    Pot pot = obj.GetComponent<Pot>();
                    GameObject plant = pot.GetPlant();
                    if (plant != null) {
                        objectsToDestroy.Add(plant);
                    }
                }
                objectsToDestroy.Add(obj);
            }
        }

        foreach (GameObject obj in objectsToDestroy) {
            Destroy(obj);
        }

        return new LobbyHotBarData(hotBarItems);
    }
}