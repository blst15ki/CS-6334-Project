using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class Hotbar : MonoBehaviour
{
    [SerializeField] public GameObject[] slots = new GameObject[9]; // References to the outer images of slots (for slot outlines)
    [SerializeField] public GameObject[] itemSlots = new GameObject[9]; // References to the inner images of slots
    [SerializeField] public Camera mainCamera;
    public GameObject[] items = new GameObject[9]; // Contains item references per slot
    public Outline[] slotOutlines = new Outline[9]; // Handles outer slot outline
    public Image[] images = new Image[9]; // Contains sprites for slots
    public int slot, floorLayer;
    public string XInput, YInput, AInput, BInput;
    public bool enable = true;
    public bool wait, inUse;
    public RaycastHit hit;
    
    void Start()
    {
        for(int i = 0; i < 9; i++) {
            slotOutlines[i] = slots[i].GetComponent<Outline>();
            images[i] = itemSlots[i].GetComponent<Image>();
        }

        slot = 0;
        floorLayer = 7;
        XInput = "js2";
        YInput = "js3";
        AInput = "js10";
        BInput = "js5";
        wait = false; // Prevent selecting and placing an object in the same frame
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

    public virtual bool SelectObject(GameObject obj) {
        return SelectObject(obj, this.slot);
    }

    public abstract bool SelectObject(GameObject obj, int slotIndex);

    public abstract void UseItem();

    public void SetIcon(string tag, int index) {
        images[index].sprite = GameManager.Instance.GetSprite(tag);
        images[index].color = Color.white;
    }

    public List<GameObject> GetItems() {
        List<GameObject> objectList = new List<GameObject>();

        foreach (GameObject item in items) {
            objectList.Add(item);
        }
        return objectList;
    }

    public abstract void LoadItems(List<GameObject> listOfItems);

    public void ClearSlot() {
        Destroy(items[slot]);
        items[slot] = null;
        images[slot].sprite = null;
        images[slot].color = Color.grey;
    }
}
