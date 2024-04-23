using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField] GameObject[] slots = new GameObject[9]; // references outer image of slots (for slot outlines)
    [SerializeField] GameObject[] itemSlots = new GameObject[9]; // references inner image of slots
    [SerializeField] Camera mainCamera;
    [SerializeField] Sprite wateringCanIcon;
    [SerializeField] Sprite potIcon;
    [SerializeField] Sprite fertilizerIcon;
    [SerializeField] Sprite sprinklerIcon;
    [SerializeField] GameObject plantPrefab;
    GameObject[] items = new GameObject[9]; // contains item references per slot
    UnityEngine.UI.Outline[] slotOutlines = new UnityEngine.UI.Outline[9]; // handles outer slot outline
    Image[] images = new Image[9]; // contains sprites for slots
    int slot, floorLayer;
    string XInput, YInput, AInput, BInput;
    bool enable = true;
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
        if(items[slot] == null) {
            SetIcon(obj.tag);
            items[slot] = obj;
            obj.SetActive(false);
            wait = true;

            // if pot with plant, disable plant too
            if(obj.tag == "Pot" && obj.GetComponent<Pot>().HasPlant()) {
                obj.GetComponent<Pot>().GetPlant().SetActive(false);
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
                    GameObject plant = Instantiate(plantPrefab, obj.transform.position + new Vector3(0f, 0.75f, -0.15f), Quaternion.identity);
                    
                    // link pot and plant
                    obj.GetComponent<Pot>().SetPlant(plant);
                    plant.GetComponent<Plant>().SetPot(obj);

                    // delete seed from hotbar
                    ClearSlot();
                }
            }
        }
    }

    void SetIcon(string tag) {
        if(tag == "Watering Can") {
            images[slot].sprite = wateringCanIcon;
        } else if(tag == "Pot") {
            images[slot].sprite = potIcon;
        } else if(tag == "Fertilizer") {
            images[slot].sprite = fertilizerIcon;
        } else if(tag == "Sprinkler") {
            images[slot].sprite = sprinklerIcon;
        }
        images[slot].color = Color.white;
    }

    void ClearSlot() {
        Destroy(items[slot]);
        items[slot] = null;
        images[slot].sprite = null;
        images[slot].color = Color.grey;
    }
}
