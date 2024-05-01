using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GetPlayerId : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public int GetLocalPlayerID()
    {
        if (PhotonNetwork.LocalPlayer != null)
        {
            return PhotonNetwork.LocalPlayer.ActorNumber;
        }
        else
        {
            Debug.Log("Failed player ID");
            return -1;
        }
    }
}
