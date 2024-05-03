using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class LobbyHotbar : Hotbar
{
	public GameObject rpcPot = null;
	public GameObject rpcPlant = null;

    public override void EnableHotbar() {
        enable = true;
    }

    public override void DisableHotbar() {
        enable = false;
    }

    public override void PlaceObject() {
		if(items[slot] == null) {
            return;
        }

        if(items[slot].tag == "Seed") {
            return;
        }

        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit)) {
            // ensure raycast is hitting floor
            if(hit.collider.gameObject.layer == floorLayer) {
				// PhotonTransformView transformView = items[slot].GetComponent<PhotonTransformView>();
				PhotonView photonView = items[slot].GetComponent<PhotonView>();
				// photonView.ObservedComponents.Add(transformView);
				// transformView.m_SynchronizePosition = true;
				// transformView.m_SynchronizeRotation = true;
				// transformView.m_SynchronizeScale = false;
                images[slot].sprite = null;
                images[slot].color = Color.grey;
                items[slot].transform.position = new Vector3(hit.point.x, items[slot].transform.position.y, hit.point.z);
                // items[slot].SetActive(true);
				LobbyInteractiveItem interactiveItem = items[slot].GetComponent<LobbyInteractiveItem>();
				if (interactiveItem != null) {
					interactiveItem.ChangeActiveState(true);
				}

                // if placing pot and pot has plant, move plant
                if(items[slot].tag == "Pot" && items[slot].GetComponent<Pot>().HasPlant()) {
                    GameObject plantObj = items[slot].GetComponent<Pot>().GetPlant();
                    Vector3 potPos = items[slot].transform.position;
                    plantObj.transform.position = new Vector3(potPos.x, plantObj.transform.position.y, potPos.z - 0.15f);
                    // plantObj.SetActive(true);
					LobbyInteractiveItem interactiveItemPlant = plantObj.GetComponent<LobbyInteractiveItem>();
					if (interactiveItemPlant != null) {
						interactiveItemPlant.ChangeActiveState(true);
					}
                }
                
                // if sprinkler, move sprinkler particle system with sprinkler
                if(items[slot].tag == "Sprinkler") {
                    GameObject spsObj = items[slot].GetComponent<Sprinkler>().GetSprinklerParticleSystem();
                    spsObj.transform.position = items[slot].transform.position + new Vector3(0f, 0.5f, 0f);
                    spsObj.SetActive(true);
                }

                items[slot] = null;
            }
        }
    }

	public override bool SelectObject(GameObject obj, int i) {

		if(obj.GetComponent<PhotonView>().OwnerActorNr != PhotonNetwork.LocalPlayer.ActorNumber) {
			return;
		}

        if (items[i] == null) {

            if (obj.GetComponent<DontDestroy>() == null) {
                obj.AddComponent<DontDestroy>();
            }

			PhotonView itemView = obj.GetComponent<PhotonView>();
			Debug.Log("Local Player ID: " + PhotonNetwork.LocalPlayer.ActorNumber);
			Debug.Log("Before " + itemView.Owner);
			if (itemView != null) {
                itemView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
			Debug.Log("After " + itemView.Owner);
            
            SetIcon(obj.tag, i);
            items[i] = obj;
            // obj.SetActive(false);
			LobbyInteractiveItem interactiveItem = obj.GetComponent<LobbyInteractiveItem>();
			if (interactiveItem != null) {
				interactiveItem.ChangeActiveState(false);
			}
            wait = true;

            // if pot with plant, disable plant too
            if(obj.tag == "Pot" && obj.GetComponent<Pot>().HasPlant()) {
                GameObject plantObj = obj.GetComponent<Pot>().GetPlant();
                if(plantObj.GetComponent<DontDestroy>() == null) {
                    plantObj.AddComponent<DontDestroy>();
                }
                // plantObj.SetActive(false);
				LobbyInteractiveItem interactiveItemPlant = plantObj.GetComponent<LobbyInteractiveItem>();
				if (interactiveItemPlant != null) {
					interactiveItemPlant.ChangeActiveState(false);
				}
            }

            // if sprinkler, disable sprinkler particle system too if it exists
            if(obj.tag == "Sprinkler") {
                GameObject spsObj = obj.GetComponent<Sprinkler>().GetSprinklerParticleSystem();
                if(spsObj != null) {
                    if(spsObj.GetComponent<DontDestroy>() == null) {
                        spsObj.AddComponent<DontDestroy>();
                    }
                    spsObj.SetActive(false);
                }
            }

            return true;
        }
        return false;
    }

    public override void UseItem() {
		return;
    }

    public void LoadItems(List<HotBarItem> listOfHotBarItem) {

		if (!photonView.IsMine)
            return;

		Debug.Log("Local Player ID From Load Items: " + PhotonNetwork.LocalPlayer.ActorNumber);
		Debug.Log("Item is Loading");

		if (listOfHotBarItem == null) { 
            return; 
        };
		GameObject obj;

        for (int i = 0; i < listOfHotBarItem.Count && i < items.Length; i++) {
			switch (listOfHotBarItem[i].type) {
				case "None":
					break;
				case "Watering Can":
					obj = PhotonNetwork.Instantiate("WateringCanPrefab", listOfHotBarItem[i].position, Quaternion.identity);
					SelectObject(obj, i);
					break;
				case "Sprinkler":
					obj = PhotonNetwork.Instantiate("Sprinkler", listOfHotBarItem[i].position, Quaternion.identity);
					SelectObject(obj, i);
					break;
				case "Pot":
					obj = PhotonNetwork.Instantiate("flowerpot", listOfHotBarItem[i].position, Quaternion.identity);
					Pot pot = obj.GetComponent<Pot>();
					GameManager.Instance.RegisterPot(pot.id, obj);

					if(listOfHotBarItem[i].hasPlant) {
						GameObject objPlant = PhotonNetwork.Instantiate(listOfHotBarItem[i].plantType, listOfHotBarItem[i].plantPosition, Quaternion.identity);
						GameManager.Instance.RegisterPlant(objPlant.GetComponent<Plant>().id, objPlant);
						LobbyPlantInteractive lobbyPlantInteractive = objPlant.GetComponent<LobbyPlantInteractive>();
						if (lobbyPlantInteractive != null) {
							Debug.Log("Inside Plant");
							lobbyPlantInteractive.setPlantData(listOfHotBarItem[i].plantType, listOfHotBarItem[i].plantPosition, listOfHotBarItem[i].water, listOfHotBarItem[i].stage, listOfHotBarItem[i].timeHalf, listOfHotBarItem[i].timeMature, listOfHotBarItem[i].timeLeave, listOfHotBarItem[i].delay, listOfHotBarItem[i].isHalf, listOfHotBarItem[i].isMature, listOfHotBarItem[i].scale, pot.id);
						}
						LobbyPotInteractive lobbyPotInteractive = obj.GetComponent<LobbyPotInteractive>();
						if (lobbyPotInteractive != null) {
							// Debug.Log("Inside Pot");
							lobbyPotInteractive.setPotData(objPlant.GetComponent<Plant>().id);
						}
						// Plant networkPlant = objPlant.GetComponent<Plant>();
						// networkPlant.SetPlantType(listOfHotBarItem[i].plantType);
						// networkPlant.SetWater(listOfHotBarItem[i].water);
						// networkPlant.SetStage(listOfHotBarItem[i].stage);
						// networkPlant.SetTimeHalf(listOfHotBarItem[i].timeHalf);
						// networkPlant.SetTimeMature(listOfHotBarItem[i].timeMature);
						// networkPlant.SetTimeLeave(listOfHotBarItem[i].timeLeave);
						// networkPlant.delay = listOfHotBarItem[i].delay;
						// networkPlant.isHalf = listOfHotBarItem[i].isHalf;
						// networkPlant.isMature = listOfHotBarItem[i].isMature;
						// networkPlant.transform.localScale = listOfHotBarItem[i].scale;
						// networkPlant.SetPot(obj);
						// networkPlant.SetPotID(pot.id);
						// pot.SetPlant(objPlant);
						// pot.SetPlantID(networkPlant.id);
					}
					SelectObject(obj, i);

					break;
				case "Seed":
					string seedString = listOfHotBarItem[i].seedType + " Seed";
					obj = PhotonNetwork.Instantiate(seedString, listOfHotBarItem[i].position, Quaternion.identity);
					SelectObject(obj, i);
					break;
				case "Fertilizer":
					obj = PhotonNetwork.Instantiate("Fertilizer Cube", listOfHotBarItem[i].position, Quaternion.identity);
					SelectObject(obj, i);
					break;
				default:
					break;
			}
        }
    }

	public InsideHotBarData ConvertHotBarToInsideHotBarData() {
        List<HotBarItem> hotBarItems = new List<HotBarItem>();

        foreach (GameObject obj in items){
            HotBarItem item = new HotBarItem(obj);
            hotBarItems.Add(item);
            Destroy(obj);
        }

        return new InsideHotBarData(hotBarItems);
    }

	// [PunRPC]
    // void RPCCreatePlant(string plantType, Vector3 plantPosition, int water, string stage, string timeHalf, string timeMature, string timeLeave, bool delay, bool isHalf, bool isMature, Vector3 scale)
    // {
    //     GameObject objPlant = PhotonNetwork.Instantiate(plantType, plantPosition, Quaternion.identity);
    //     Plant networkPlant = objPlant.GetComponent<Plant>();
    //     networkPlant.SetPlantType(plantType);
    //     networkPlant.SetWater(water);
    //     networkPlant.SetStage(stage);
    //     networkPlant.SetTimeHalf(timeHalf);
    //     networkPlant.SetTimeMature(timeMature);
    //     networkPlant.SetTimeLeave(timeLeave);
    //     networkPlant.delay = delay;
    //     networkPlant.isHalf = isHalf;
    //     networkPlant.isMature = isMature;
    //     networkPlant.transform.localScale = scale;
	// 	rpcPlant = objPlant;
    // }

    // public void CreatePlant(string plantType, Vector3 plantPosition, int water, string stage, string timeHalf, string timeMature, string timeLeave, bool delay, bool isHalf, bool isMature, Vector3 scale)
    // {
		
    //     photonView.RPC("RPCCreatePlant", RpcTarget.AllBuffered, plantType, plantPosition, water, stage, timeHalf, timeMature, timeLeave, delay, isHalf, isMature, scale);
	// 	Plant plant = rpcPlant.GetComponent<Plant>();
	// 	Pot pot = rpcPot.GetComponent<Pot>();
	// 	plant.SetPot(rpcPot);
    //     plant.SetPotID(pot.id);
    //     pot.SetPlant(rpcPlant);
    //     pot.SetPlantID(plant.id);
    // }
}
