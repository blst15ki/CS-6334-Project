using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Photon.Pun;

public class Pot : MonoBehaviour
{
    [SerializeField] PlantInterface plantInterface;
    [SerializeField] Hotbar hotbar;
    [SerializeField] GameObject plantObj;
    [SerializeField] Plant plant;
    Outline outline;
    bool pointer;
    string AInput;
    public Guid uuid = Guid.NewGuid();
    public string id;
    public string plantID = null;
    public bool isDataLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        pointer = false;
        AInput = "js10";
    }

    void Awake() {
        if(!isDataLoaded) {
            id = uuid.ToString();
        }
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

    [PunRPC]
    void SetActiveState(bool active) {
        gameObject.SetActive(active);
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }

    public bool AddFertilizer() {
        // check if plant exists
        if(HasPlant()) {
            plant.Fertilize();
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
    }
    public void WaterPlant() { plant.GiveWater(); }
    public void StopWaterPlant() { plant.StopWater(); }
    public string GetPlantID() { return plantID; }
    public void SetPlantID(string id){ plantID = id; }
    public void ClearPlant() {
        plantObj = null;
        plant = null;
        plantID = null;
    }
}
