using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LobbyDoorTeleport : MonoBehaviourPunCallbacks
{
    [SerializeField] string scene;
    private string BInput;
    public GameObject player;
    bool pointer;
	[SerializeField] GameObject sign;

    void Start()
    {
		sign.SetActive(false); 
        BInput = "js5";
    }

    void Update()
    {
        if(pointer && Input.GetButtonDown(BInput)) {
            TeleportPlayer();
        }
    }

    public void PointerOn() { 
		sign.SetActive(true); 
        pointer = true;
    }

    public void PointerOff() {
		sign.SetActive(false); 
        pointer = false;
    }

    private void TeleportPlayer() {
        LobbyHotbar hotbar = player.GetComponentInChildren<LobbyHotbar>();
		GameManager.Instance.SaveInsideHotBarData(hotbar.ConvertHotBarToInsideHotBarData());
        

        GameManager.Instance.indoorSpawnPoint = "front";
		GameManager.Instance.ResetPots();
		GameManager.Instance.ResetPlants();

        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.LeaveRoom();
        } else {
            LoadNewScene();
        }
    }

	public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log("Disconnected from the Server");
        LoadNewScene();
    }

    public override void OnLeftRoom() {
        Debug.Log("Left the room");
        PhotonNetwork.Disconnect();
    }

    private void LoadNewScene(){
        SceneManager.LoadScene(scene);
    }
}
