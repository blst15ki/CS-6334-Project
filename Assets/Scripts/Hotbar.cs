using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;



public class Hotbar : MonoBehaviour
{
    [SerializeField] GameObject[] slots = new GameObject[9]; // references outer image of slots (for slot outlines)
    [SerializeField] GameObject[] itemSlots = new GameObject[9]; // references inner image of slots
    [SerializeField] Camera mainCamera;
    GameObject[] items = new GameObject[9]; // contains item references per slot
    UnityEngine.UI.Outline[] slotOutlines = new UnityEngine.UI.Outline[9]; // handles outer slot outline
    Image[] images = new Image[9]; // contains sprites for slots
    int slot, floorLayer;
    string XInput, YInput, AInput, BInput;
    public bool enable = true;
    bool wait, inUse;
    RaycastHit hit;
    PhotonView photonView = null;
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 9; i++) {
            slotOutlines[i] = slots[i].GetComponent<UnityEngine.UI.Outline>();
            images[i] = itemSlots[i].GetComponent<Image>();
        }

        slot = 0;
        floorLayer = 7;
        XInput = "js2";
        YInput = "js3";
        AInput = "js10";
        BInput = "js5";
        wait = false; // prevent selecting and placing an object in the same frame
        inUse = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView == null) {
            photonView = GetComponent<PhotonView>();
        }

        if(enable) {
            // prevent using other buttons if item is being used
            if(inUse == false) {
                if(Input.GetButtonDown(XInput)) {
                    MoveSlot("left");
                } else if(Input.GetButtonDown(YInput)) {
                    MoveSlot("right");
                } else if(Input.GetButtonDown(AInput) && wait == false) {
                    PlaceObject();
                } else if(Input.GetButtonDown(BInput)) {
                    UseItem();
                }
            }

            if(wait) {
                wait = false;
            }

            if(Input.GetButtonUp(BInput) && inUse) {
                if(items[slot].tag == "Watering Can") {
                    // stop audio and disable watering can
                    items[slot].GetComponent<AudioSource>().Stop();
                    items[slot].GetComponentInChildren<Renderer>().enabled = true;
                    items[slot].GetComponentInChildren<Collider>().enabled = true;
                    items[slot].SetActive(false);

                    if(PhotonNetwork.IsConnected != null && PhotonNetwork.IsConnected) {
                        photonView.RPC("SetActiveState", RpcTarget.Others, items[slot].name, false);
                    }
                    mainCamera.GetComponent<ParticleSystem>().Stop();
                    // stop watering
                    if(hit.collider.gameObject.tag == "Pot") {
                        hit.collider.gameObject.GetComponent<Pot>().StopWaterPlant();
                    } else if(hit.collider.gameObject.tag == "Plant") {
                        hit.collider.gameObject.GetComponent<Plant>().StopWater();
                    }

                    hit.collider.gameObject.GetComponent<Outline>().OutlineColor = Color.white;
                    inUse = false;
                }
            }
        }
    }

    public void EnableHotbar() { enable = true; }
    public void DisableHotbar() { enable = false; }

    void MoveSlot(string str) {
        slotOutlines[slot].enabled = false;

        if(str == "left") {
            slot--;
        } else if(str == "right") {
            slot++;
        }

        if(slot == -1) {
            slot += 9;
        } else if(slot == 9) {
            slot -= 9;
        }

        slotOutlines[slot].enabled = true;
    }

    public void PlaceObject() {
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
                PhotonTransformView transformView = items[slot].GetComponent<PhotonTransformView>();
                photonView = GetComponent<PhotonView>();
                photonView.ObservedComponents.Add(transformView);
                transformView.m_SynchronizePosition = true;
                transformView.m_SynchronizeRotation = true;
                transformView.m_SynchronizeScale = false;
                images[slot].sprite = null;
                images[slot].color = Color.grey;
                items[slot].transform.position = new Vector3(hit.point.x, items[slot].transform.position.y, hit.point.z);
                items[slot].SetActive(true);

                if(PhotonNetwork.IsConnected != null && PhotonNetwork.IsConnected) {
                    photonView.RPC("SetActiveState", RpcTarget.Others, items[slot].name, true);
                }

                // if placing pot and pot has plant, move plant
                if(items[slot].tag == "Pot" && items[slot].GetComponent<Pot>().HasPlant()) {
                    GameObject plantObj = items[slot].GetComponent<Pot>().GetPlant();
                    Vector3 potPos = items[slot].transform.position;
                    plantObj.transform.position = new Vector3(potPos.x, plantObj.transform.position.y, potPos.z - 0.15f);
                    plantObj.SetActive(true);
                    if(PhotonNetwork.IsConnected != null && PhotonNetwork.IsConnected) {
                        photonView.RPC("SetActiveState", RpcTarget.Others, plantObj.name, true);
                    }
                }
                
                // if sprinkler, move sprinkler particle system with sprinkler
                if(items[slot].tag == "Sprinkler") {
                    GameObject spsObj = items[slot].GetComponent<Sprinkler>().GetSprinklerParticleSystem();
                    spsObj.transform.position = items[slot].transform.position + new Vector3(0f, 0.5f, 0f);
                    spsObj.SetActive(true);
                    if(PhotonNetwork.IsConnected != null && PhotonNetwork.IsConnected) {
                        photonView.RPC("SetActiveState", RpcTarget.Others, spsObj.name, true);
                    }
                }

                items[slot] = null;
            }
        }
    }

    public bool SelectObject(GameObject obj) {
        return SelectObject(obj, slot);
    }

    public bool SelectObject(GameObject obj, int i) {
        if (items[i] == null) {

            if (obj.GetComponent<DontDestroy>() == null) {
                obj.AddComponent<DontDestroy>();
            }
            
            SetIcon(obj.tag, i);
            items[i] = obj;
            obj.SetActive(false);
            if(PhotonNetwork.IsConnected != null && PhotonNetwork.IsConnected) {
                photonView.RPC("SetActiveState", RpcTarget.Others, obj.name, false);
            }
            wait = true;

            // if pot with plant, disable plant too
            if(obj.tag == "Pot" && obj.GetComponent<Pot>().HasPlant()) {
                GameObject plantObj = obj.GetComponent<Pot>().GetPlant();
                if(plantObj.GetComponent<DontDestroy>() == null) {
                    plantObj.AddComponent<DontDestroy>();
                }
                plantObj.SetActive(false);
                if(PhotonNetwork.IsConnected != null && PhotonNetwork.IsConnected) {
                    photonView.RPC("SetActiveState", RpcTarget.Others, plantObj.name, false);
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
                    if(PhotonNetwork.IsConnected != null && PhotonNetwork.IsConnected) {
                        photonView.RPC("SetActiveState", RpcTarget.Others, spsObj.name, false);
                    }
                }
            }

            return true;
        }
        return false;
    }

    void UseItem() {
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
                        if(PhotonNetwork.IsConnected != null && PhotonNetwork.IsConnected) {
                            photonView.RPC("SetActiveState", RpcTarget.Others, items[slot].name, true);
                        }
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
                    if(PhotonNetwork.IsConnected != null && PhotonNetwork.IsConnected) {
                        photonView.RPC("SetActiveState", RpcTarget.Others, items[slot].name, true);
                    }
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

    public void SetIcon(string tag, int i) {
        images[i].sprite = GameManager.Instance.GetSprite(tag);
        images[i].color = Color.white;
    }

    public List<GameObject> GetItems() {
        List<GameObject> objectList = new List<GameObject>();

        foreach (GameObject item in items)
        {
            if (item != null) {
                objectList.Add(item);
            }
            else {
                objectList.Add(null);
            }
        }
        return objectList;
    }

    public void LoadItems(List<GameObject> listOfItems) {
        if (listOfItems == null) { 
            return; 
        };

        for (int i = 0; i < items.Length; i++) {
            if (listOfItems[i] != null) {
                SelectObject(listOfItems[i], i);
            }
        }
    }

    public List<GameObject> getNonPhotonVeiwVersion() {
        List<GameObject> objectList = new List<GameObject>();

        foreach (GameObject item in items)
        {
            if (item != null) {
                GameObject prefab = GameManager.Instance.GetPrefab(item.tag);
                if (prefab != null) {
                    GameObject newItem = GameObject.Instantiate(prefab, item.transform.position, item.transform.rotation);
                    if (newItem.GetComponent<DontDestroy>() == null) {
                        newItem.AddComponent<DontDestroy>();
                    }
                    objectList.Add(newItem);
                } else {
                    Debug.Log("Prefab not found: " + item.tag);
                    objectList.Add(null);
                }
                Destroy(item);
            }
            else {
                objectList.Add(null);
            }
        }
        return objectList;
    }

    [PunRPC]
    void SetActiveState(string name, bool active) {
        GameObject obj = GameObject.Find(name);
        if (obj != null) {
            obj.SetActive(active);
            Debug.Log($"RPC SetActiveState: Setting {name} to {active}");
        } else {
            Debug.LogError("Object not found: " + name);
        }
    }

    
    public void LobbyLoadItems(List<GameObject> listOfItems) {
        if (listOfItems == null) {
            return;
        }

        for (int i = 0; i < items.Length; i++) {
            if (listOfItems[i] != null) {
                GameObject item = listOfItems[i];
                string itemObject = getPrefabName(item);

                // if (PhotonNetwork.IsMasterClient) {
                
                    GameObject networkedItem = PhotonNetwork.Instantiate(itemObject, item.transform.position, item.transform.rotation);
                    networkedItem.SetActive(false);                  
                    photonView = GetComponent<PhotonView>();
                    photonView.RPC("SetActiveState", RpcTarget.Others, networkedItem.name, false);
                    

                    Debug.Log(itemObject);

                    if(itemObject == "flowerpot") {
                        Pot potScript = item.GetComponent<Pot>();
                        
                        Pot networkPotScript = networkedItem.GetComponent<Pot>();
                        networkPotScript.SetPlantID(potScript.GetPlantID());
                        networkPotScript.id = potScript.id;
                        networkPotScript.isDataLoaded = potScript.isDataLoaded;
                        GameObject plantObjectNetwork = null;
                        if (potScript.HasPlant()) {
                            GameObject plant = potScript.GetPlant();
                            if (plant != null) {
                                Plant plantScript = plant.GetComponent<Plant>();
                                string plantPrefabName = getPrefabName(plant);
                                plantObjectNetwork = PhotonNetwork.Instantiate(plantPrefabName, plant.transform.position, plant.transform.rotation);
                                Debug.Log(plantObjectNetwork.tag);
                                // networkPlant.transform.SetParent(networkedItem.transform);
                                
                                plantObjectNetwork.SetActive(false);
                                photonView = GetComponent<PhotonView>();
                                photonView.RPC("SetActiveState", RpcTarget.Others, plantObjectNetwork.name, false);
                                
                                Plant networkPlantScript = plantObjectNetwork.GetComponent<Plant>();
                                if (networkPlantScript != null) {
                                    networkPotScript.SetPlant(plantObjectNetwork);
                                    Debug.Log(plantObjectNetwork.name);
                                    Debug.Log(networkPotScript.GetPlant().name);
                                    networkPlantScript.SetPot(networkedItem);
                                    networkPlantScript.water = plantScript.water;
                                    networkPlantScript.maxWater = plantScript.maxWater;
                                    networkPlantScript.time = plantScript.time;
                                    networkPlantScript.type = plantScript.type;
                                    networkPlantScript.id = plantScript.id;
                                    networkPlantScript.potID = plantScript.potID;
                                    networkPlantScript.stage = plantScript.stage;
                                    networkPlantScript.cur = plantScript.cur;
                                    networkPlantScript.timeHalf = plantScript.timeHalf;
                                    networkPlantScript.timeMature = plantScript.timeMature;
                                    networkPlantScript.timeLeave = plantScript.timeLeave;
                                    networkPlantScript.isHalf = plantScript.isHalf;
                                    networkPlantScript.isMature = plantScript.isMature;
                                    networkPlantScript.hasLight = plantScript.hasLight;
                                    networkPlantScript.uuid = plantScript.uuid;
                                    networkPlantScript.isDataLoaded = plantScript.isDataLoaded;
                                    networkPlantScript.delay = plantScript.delay;
                                    networkPlantScript.lightSource = plantScript.lightSource;
                                }
                                Destroy(plant);
                            }
                        }
                    }
                    Destroy(item);
                    SelectObject(networkedItem, i);
                // }
            }
        }
    }

    public string getPrefabName(GameObject item) {
        string itemObject = null;
        switch (item.tag){
            case "Fertilizer":
                itemObject = "Fertilizer Cube";
                break;
            case "Pot":
                itemObject = "flowerpot";
                break;
            case "Watering Can":
                itemObject = "WateringCanPrefab";
                break;
            case "Sprinkler":
                itemObject = "Sprinkler";
                break;
            case "Plant":
                if (item.name.Contains("Fern")) {
                    itemObject = "Fern";
                } else {
                    itemObject = "Basic Plant";
                }
                break;
            case "Seed":
                //TODO: replace with seed prefab
                itemObject = "WateringCanPrefab";
                break;
            case "Chest":
                //TODO: replace with chest prefab
                itemObject = "WateringCanPrefab";
                break;
            default:
                itemObject = "WateringCanPrefab";
                break;
        }

        return itemObject;
    }

    void ClearSlot() {
        Destroy(items[slot]);
        items[slot] = null;
        images[slot].sprite = null;
        images[slot].color = Color.grey;
    }
}
