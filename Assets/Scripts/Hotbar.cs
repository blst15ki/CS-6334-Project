using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hotbar : MonoBehaviour
{
    [SerializeField] GameObject[] slots = new GameObject[9]; // references outer image of slots (for slot outlines)
    [SerializeField] GameObject[] itemSlots = new GameObject[9]; // references inner image of slots
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject basicPlantPrefab;
    GameObject[] items = new GameObject[9]; // contains item references per slot
    UnityEngine.UI.Outline[] slotOutlines = new UnityEngine.UI.Outline[9]; // handles outer slot outline
    Image[] images = new Image[9]; // contains sprites for slots
    int slot, floorLayer;
    string XInput, YInput, AInput, BInput;
    public bool enable = true;
    bool wait, inUse;
    RaycastHit hit;
    
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
        if(items[slot] == null) {
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
            wait = true;

            // if pot with plant, disable plant too
            if(obj.tag == "Pot" && obj.GetComponent<Pot>().HasPlant()) {
                GameObject plantObj = obj.GetComponent<Pot>().GetPlant();
                if(plantObj.GetComponent<DontDestroy>() == null) {
                    plantObj.AddComponent<DontDestroy>();
                }
                plantObj.SetActive(false);
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
                        items[slot].GetComponentInChildren<Renderer>().enabled = false;
                        items[slot].GetComponentInChildren<Collider>().enabled = false;
                        items[slot].GetComponent<AudioSource>().Play();

                        // water plant
                        hit.collider.gameObject.GetComponent<Outline>().OutlineColor = Color.blue;
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
                    hit.collider.gameObject.GetComponent<Outline>().OutlineColor = Color.blue;
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
                    // instantiate plant prefab on top of pot +(0, 0.75, -0.15)
                    GameObject plant = Instantiate(basicPlantPrefab, obj.transform.position + new Vector3(0f, 0.75f, -0.15f), Quaternion.identity);
                    
                    // link pot and plant
                    Pot potScript = obj.GetComponent<Pot>();
                    Plant plantScript = plant.GetComponent<Plant>();
                    potScript.SetPlant(plant);
                    potScript.SetPlantID(plantScript.id);
                    plantScript.SetPot(obj);
                    plantScript.SetPotID(potScript.id);

                    // delete seed from hotbar
                    ClearSlot();
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

    void ClearSlot() {
        Destroy(items[slot]);
        items[slot] = null;
        images[slot].sprite = null;
        images[slot].color = Color.grey;
    }
}
