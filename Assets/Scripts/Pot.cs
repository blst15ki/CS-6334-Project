using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pot : MonoBehaviour
{
    [SerializeField] PlantInterface plantInterface;
    [SerializeField] Hotbar hotbar;
    [SerializeField] GameObject plantObj;
    [SerializeField] Plant plant;
    Outline outline;
    bool pointer;
    string AInput;

    public string PlantID = null;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        pointer = false;
        AInput = "js10";

        //checking for child and getting the plant id
        // if (transform.childCount > 0) {
        //     var plant = transform.GetChild(0).GetComponent<Plant>();
        //     if (plant != null && plant.gameObject.tag == "Plant") {
        //         PlantID = plant.id;
        //     }
        // }
    }

    // Update is called once per frame
    void Update()
    {

        if (hotbar == null) {
            hotbar = FindObjectOfType<Hotbar>();
        }

        if (plantInterface == null) {
            plantInterface = FindObjectOfType<PlantInterface>();
        }
        
        if(pointer) {
            if(Input.GetButtonDown(AInput)) { // select object to place
                if(hotbar.SelectObject(gameObject)) {
                    pointer = false;
                    outline.enabled = false;

                    // disable plant interface
                    plantInterface.DisableInterface();
                }
            }
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }

    public bool AddFertilizer() {
        // check if plant exists
        if(transform.childCount > 0 && transform.GetChild(0).gameObject.tag == "Plant") {
            transform.GetChild(0).gameObject.GetComponent<Plant>().Fertilize();
            return true;
        }
        return false;
    }

    public bool HasPlant() { return plantObj != null; }
    public GameObject GetPlant() { return plantObj; }
    public void SetPlant(GameObject obj) {
        // link plant to pot
        plantObj = obj;
        plant = obj.GetComponent<Plant>();

        // set plantinterface link
        EventTrigger eventT = gameObject.AddComponent<EventTrigger>();

        // add OnPointerEnter
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        eventT.triggers.Add(entry);

        // add OnPointerExit
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        eventT.triggers.Add(entry);
    }
    public void WaterPlant() { plant.GiveWater(); }
    public void StopWaterPlant() { plant.StopWater(); }

    public void OnPointerEnterDelegate(PointerEventData data) {
        plantInterface.EnableInterface(plantObj);
    }
    public void OnPointerExitDelegate(PointerEventData data) {
        plantInterface.DisableInterface();
    }

    public void setPlantID(string id){
        PlantID = id;
    }
}
