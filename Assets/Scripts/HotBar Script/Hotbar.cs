using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public abstract class Hotbar : MonoBehaviourPunCallbacks
{
    [SerializeField] protected GameObject[] slots = new GameObject[9]; // references outer image of slots (for slot outlines)
    [SerializeField] protected GameObject[] itemSlots = new GameObject[9]; // references inner image of slots
    [SerializeField] protected Camera mainCamera;
    protected GameObject[] items = new GameObject[9]; // contains item references per slot
    protected UnityEngine.UI.Outline[] slotOutlines = new UnityEngine.UI.Outline[9];  // handles outer slot outline
    protected Image[] images = new Image[9]; // contains sprites for slots
    protected int slot, floorLayer;
    protected string XInput, YInput, AInput, BInput;
    public bool enable = true;
    protected bool wait, inUse;
    protected RaycastHit hit;
    
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

    void Update()
    {
        if(enable) {
            // Prevent using other buttons if item is being used
            if(!inUse) {
                if(Input.GetButtonDown(XInput)) {
                    MoveSlot("left");
                } else if(Input.GetButtonDown(YInput)) {
                    MoveSlot("right");
                } else if(Input.GetButtonDown(AInput) && !wait) {
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
                    if(hit.collider != null) { // ensure plant is not dead before acting on plant
                        if(hit.collider.gameObject.tag == "Pot" && hit.collider.gameObject.GetComponent<Pot>().HasPlant()) {
                            hit.collider.gameObject.GetComponent<Pot>().StopWaterPlant();
                        } else if(hit.collider.gameObject.tag == "Plant") {
                            hit.collider.gameObject.GetComponent<Plant>().StopWater();
                        }
                        hit.collider.gameObject.GetComponent<Outline>().OutlineColor = Color.white;
                    }
                    inUse = false;
                }
            }
        }
    }

    public abstract void EnableHotbar();
    public abstract void DisableHotbar();

    void MoveSlot(string direction) {
        slotOutlines[slot].enabled = false;

        if(direction == "left") {
            slot--;
        } else if(direction == "right") {
            slot++;
        }

        if(slot < 0) {
            slot += 9;
        } else if(slot >= 9) {
            slot -= 9;
        }

        slotOutlines[slot].enabled = true;
    }

    public abstract void PlaceObject();

    public bool HasObject() { return items[slot] != null; }

    public bool SelectObject(GameObject obj) {
        return SelectObject(obj, this.slot);
    }

    public abstract bool SelectObject(GameObject obj, int slotIndex);

    public abstract void UseItem();

    public void SetIcon(string tag, int index, GameObject obj) {
        if(tag == "Seed") {
            images[index].sprite = GameManager.Instance.GetSprite(obj.GetComponent<Seed>().GetPlantType());
        } else {
            images[index].sprite = GameManager.Instance.GetSprite(tag);
        }
        images[index].color = Color.white;
    }

    public List<GameObject> GetItems() {
        List<GameObject> objectList = new List<GameObject>();

        foreach (GameObject item in items) {
            objectList.Add(item);
        }
        return objectList;
    }

    public void ClearSlot() {
        Destroy(items[slot]);
        items[slot] = null;
        images[slot].sprite = null;
        images[slot].color = Color.grey;
    }
}
