using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NormalHotbar : Hotbar
{

    public override void EnableHotbar() { enable = true; }
    public override void DisableHotbar() { enable = false; }
    public override void PlaceObject() {
        // cannot place empty slot
        if(items[slot] == null) {
            return;
        }

        // cannot place sprinkler in inside scene
        if(items[slot].tag == "Sprinkler" && SceneManager.GetActiveScene().name == "Inside") {
            return;
        }

        // cannot place seeds
        if(items[slot].tag == "Seed") {
            return;
        }

        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit)) {
            // ensure raycast is hitting floor
            if(hit.collider.gameObject.layer == floorLayer) {
                images[slot].sprite = null;
                images[slot].color = Color.grey;
                items[slot].transform.position = new Vector3(hit.point.x, items[slot].transform.position.y, hit.point.z);
                items[slot].SetActive(true);

                // if placing pot and pot has plant, move plant
                if(items[slot].tag == "Pot" && items[slot].GetComponent<Pot>().HasPlant()) {
                    GameObject plantObj = items[slot].GetComponent<Pot>().GetPlant();
                    Vector3 potPos = items[slot].transform.position;
                    plantObj.transform.position = new Vector3(potPos.x, plantObj.transform.position.y, potPos.z - 0.15f);
                    plantObj.SetActive(true);
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

    public override bool SelectObject(GameObject obj) {
        return SelectObject(obj, slot);
    }

    public override bool SelectObject(GameObject obj, int i) {
        if (items[i] == null) {

            if (obj.GetComponent<DontDestroy>() == null) {
                obj.AddComponent<DontDestroy>();
            }
            
            SetIcon(obj.tag, i);
            items[i] = obj;
            obj.SetActive(false);
            wait = true;

            // if pot with plant, disable plant too
            if(obj.tag == "Pot" && obj.GetComponent<Pot>().HasPlant()) {
                GameObject plantObj = obj.GetComponent<Pot>().GetPlant();
                if(plantObj.GetComponent<DontDestroy>() == null) {
                    plantObj.AddComponent<DontDestroy>();
                }
                plantObj.SetActive(false);
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
        if(items[slot] == null) {
            return;
        }

        if(items[slot].tag == "Watering Can") {
            if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit)) {
                if(hit.collider.gameObject.tag == "Pot") { // check if raycasthit is pot
                    // check if pot contains plant
                    if(hit.collider.gameObject.GetComponent<Pot>().GetPlant() != null) {
                        // enable watering can but hide mesh/collider to play audio
                        items[slot].SetActive(true);
                        items[slot].GetComponentInChildren<Renderer>().enabled = false;
                        items[slot].GetComponentInChildren<Collider>().enabled = false;
                        items[slot].GetComponent<AudioSource>().Play();
                        mainCamera.GetComponent<ParticleSystem>().Play();

                        // water plant
                        hit.collider.gameObject.GetComponent<Outline>().OutlineColor = Color.cyan;
                        hit.collider.gameObject.GetComponent<Pot>().WaterPlant();
                        inUse = true;
                    }
                } else if(hit.collider.gameObject.tag == "Plant") { // check if raycasthit is plant
                    // enable watering can but hide mesh/collider to play audio
                    items[slot].SetActive(true);
                    items[slot].GetComponentInChildren<Renderer>().enabled = false;
                    items[slot].GetComponentInChildren<Collider>().enabled = false;
                    items[slot].GetComponent<AudioSource>().Play();
                    mainCamera.GetComponent<ParticleSystem>().Play();

                    // water plant
                    hit.collider.gameObject.GetComponent<Outline>().OutlineColor = Color.cyan;
                    hit.collider.gameObject.GetComponent<Plant>().GiveWater();
                    inUse = true;
                }
            }
        } else if(items[slot].tag == "Fertilizer") {
            if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit)) {
                // check if raycasthit is pot
                if(hit.collider.gameObject.tag == "Pot") {
                    // remove fertilizer from scene if successful
                    if(hit.collider.gameObject.GetComponent<Pot>().AddFertilizer()) {
                        ClearSlot();
                    }
                }
            }
        } else if(items[slot].tag == "Seed") {
            if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit)) {
                // check if raycasthit is pot (plant seed in pot)
                GameObject obj = hit.collider.gameObject;
                if(obj.tag == "Pot" && !obj.GetComponent<Pot>().HasPlant()) {
                    string plantType = items[slot].GetComponent<Seed>().GetPlantType();
                    Vector3 adjustPlantPos;
                    if(plantType == "Basic Plant") {
                        adjustPlantPos = new Vector3(0f, 0.75f, -0.15f);
                    } else if(plantType == "Fern") {
                        adjustPlantPos = new Vector3(0f, 0.6f, -0.15f);
                    } else { // default
                        Debug.Log("Unexpected type: " + plantType);
                        plantType = "Basic Plant";
                        adjustPlantPos = new Vector3(0f, 0.75f, -0.15f);
                    }

                    GameObject plant = Instantiate(GameManager.Instance.GetPrefab(plantType), obj.transform.position + adjustPlantPos, Quaternion.identity);
                    
                    // link pot and plant
                    Pot potScript = obj.GetComponent<Pot>();
                    Plant plantScript = plant.GetComponent<Plant>();
                    potScript.SetPlant(plant);
                    potScript.SetPlantID(plantScript.id);
                    plantScript.SetPot(obj);
                    plantScript.SetPotID(potScript.id);

                    // delete seed from hotbar (passed asset so cannot destroy)
                    items[slot] = null;
                    images[slot].sprite = null;
                    images[slot].color = Color.grey;
                }
            }
        }
    }


    public override void LoadItems(List<GameObject> listOfItems) {
        if (listOfItems == null) { 
            return; 
        };

        for (int i = 0; i < items.Length; i++) {
            if (listOfItems[i] != null) {
                SelectObject(listOfItems[i], i);
            }
        }
    }
}
