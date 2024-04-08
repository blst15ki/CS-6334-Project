using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField] GameObject[] slots = new GameObject[9];
    [SerializeField] GameObject[] itemSlots = new GameObject[9];
    [SerializeField] Camera mainCamera;
    [SerializeField] Sprite wateringCan;
    [SerializeField] Sprite pot;
    [SerializeField] Sprite fertilizer;
    [SerializeField] Sprite sprinkler;
    GameObject[] items = new GameObject[9];
    UnityEngine.UI.Outline[] slotOutlines = new UnityEngine.UI.Outline[9];
    Image[] images = new Image[9];
    int slot, floorLayer;
    string XInput, YInput, AInput, BInput;
    bool enable, wait, inUse;
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
        enable = true;
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
                    hit.collider.gameObject.GetComponent<Plant>().StopWater();
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

    // TODO: change to use image icon instead of white/grey for select/place
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
                // check if raycasthit is plant
                if(hit.collider.gameObject.tag == "Plant") {
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
                        Destroy(items[slot]);
                        items[slot] = null;
                        images[slot].sprite = null;
                        images[slot].color = Color.grey;
                    }
                }
            }
        }
    }

    void SetIcon(string tag) {
        if(tag == "Watering Can") {
            images[slot].sprite = wateringCan;
        } else if(tag == "Pot") {
            images[slot].sprite = pot;
        } else if(tag == "Fertilizer") {
            images[slot].sprite = fertilizer;
        } else if(tag == "Sprinkler") {
            images[slot].sprite = sprinkler;
        }
        images[slot].color = Color.white;
    }
}
