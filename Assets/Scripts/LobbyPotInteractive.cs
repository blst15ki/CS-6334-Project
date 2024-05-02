using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LobbyPotInteractive : MonoBehaviourPun, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    }

    public void setPotData(string plantID) {
        // Debug.Log("Here is the plant id from lobby pot " + plantID);
        photonView.RPC("RPCSetPot", RpcTarget.AllBuffered, plantID);
    }

    [PunRPC]
    void RPCSetActive(bool isActive){
        gameObject.SetActive(isActive);
    }

    [PunRPC]
    void RPCSetPot(string plantID)
    {
        Pot networkPot = gameObject.GetComponent<Pot>();

        networkPot.SetPlantID(plantID);

        GameObject plant = GameManager.Instance.FindPlantByID(plantID);
        if (plant != null) {
          networkPot.SetPlant(plant);
        }
    }
}