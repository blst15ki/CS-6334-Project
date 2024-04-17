using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField] GameObject[] slots = new GameObject[9];
    [SerializeField] GameObject[] itemSlots = new GameObject[9];
    GameObject[] items = new GameObject[9];
    [SerializeField] Camera mainCamera;
    UnityEngine.UI.Outline[] slotOutlines = new UnityEngine.UI.Outline[9];
    Image[] images = new Image[9];
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
                if (items[slot].GetComponent<DontDestroy>() != null) {
                    DontDestroy dontDestroyComponent = items[slot].GetComponent<DontDestroy>();
                    Destroy(dontDestroyComponent);
                    GameObject duplicateObject = Instantiate(items[slot]);
                    Destroy(items[slot]);
                    items[slot] = duplicateObject;
                }
                images[slot].sprite = null;
                images[slot].color = Color.grey;
                items[slot].transform.position = new Vector3(hit.point.x, items[slot].transform.position.y, hit.point.z);
                items[slot].SetActive(true);
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

    public void SetIcon(string tag, int i) {
        Debug.Log(tag);
        images[i].sprite = GameManager.Instance.GetSprite(tag);
        images[i].color = Color.white;
    }

    public List<GameObject> GetItems()
    {
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

    public void LoadItems(List<GameObject> listOfItems)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (listOfItems[i] != null)
            {
                SelectObject(listOfItems[i], i);
            }
        }
    }
}
