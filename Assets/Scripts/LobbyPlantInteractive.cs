using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LobbyPlantInteractive : MonoBehaviourPun, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    }

    public void setPlantData(string plantType, Vector3 plantPosition, int water, string stage, string timeHalf, string timeMature, string timeLeave, bool delay, bool isHalf, bool isMature, Vector3 scale, string potID) {
        photonView.RPC("RPCSetPlant", RpcTarget.AllBuffered, plantType, plantPosition, water, stage, timeHalf, timeMature, timeLeave, delay, isHalf, isMature, scale, potID);
    }

    [PunRPC]
    void RPCSetActive(bool isActive){
        gameObject.SetActive(isActive);
    }

    [PunRPC]
    void RPCSetPlant(string plantType, Vector3 plantPosition, int water, string stage, string timeHalf, string timeMature, string timeLeave, bool delay, bool isHalf, bool isMature, Vector3 scale, string potID)
    {
        Plant networkPlant = gameObject.GetComponent<Plant>();
        networkPlant.SetPlantType(plantType);
        networkPlant.SetWater(water);
        networkPlant.SetStage(stage);
        networkPlant.SetTimeHalf(timeHalf);
        networkPlant.SetTimeMature(timeMature);
        networkPlant.SetTimeLeave(timeLeave);
        networkPlant.delay = delay;
        networkPlant.isHalf = isHalf;
        networkPlant.isMature = isMature;
        networkPlant.transform.localScale = scale;
		networkPlant.SetPotID(potID);

		GameObject pot = GameManager.Instance.FindPotByID(potID);
		if (pot != null) {
			networkPlant.SetPot(pot);
		}
    }
}