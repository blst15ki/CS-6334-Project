using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LobbyInteractiveItem : MonoBehaviourPun, IPunObservable
{
    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    //     if (stream.IsWriting) {
    //         stream.SendNext(gameObject.activeSelf);
    //     }
    //     else {
    //         bool isActive = (bool)stream.ReceiveNext();
    //         gameObject.SetActive(isActive);
    //     }
    // }

    public void ChangeActiveState(bool isActive) {
        photonView.RPC("RPCSetActive", RpcTarget.All, isActive);
        if(isActive){
            photonView.RPC("RPCClearOwnership", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPCSetActive(bool isActive){
        gameObject.SetActive(isActive);
    }

    [PunRPC]
    void RPCClearOwnership() {
        if (photonView.Owner != null) {
            photonView.TransferOwnership(0);
        }
    }
}