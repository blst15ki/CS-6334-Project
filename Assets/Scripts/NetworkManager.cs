using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Transform spawnPoint;
    private string roomName = "Lobby";

    void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        CreateOrJoinRoom();
    }

    void CreateOrJoinRoom() {
        PhotonNetwork.JoinOrCreateRoom(roomName, null, null);
    }

    public override void OnJoinedRoom() {
        Debug.Log("Joined a room: " + PhotonNetwork.CurrentRoom.Name);
        GameObject player = PhotonNetwork.Instantiate("Character", spawnPoint.position, Quaternion.identity);
    }

}