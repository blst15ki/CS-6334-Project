using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    GameObject saveObj;
    int floorLayer;
    string AInput;
    bool wait; // to prevent SelectObject and PlaceObject from being called in the same frame

    // Start is called before the first frame update
    void Start()
    {
        saveObj = null;
        floorLayer = 7;
        AInput = "js10";
        wait = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(wait) {
            wait = false;
        } else {
            if(saveObj != null && Input.GetButtonDown(AInput)) {
                PlaceObject();
            }
        }
    }

    public void PlaceObject() {
        RaycastHit hit;
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit)) {
            
            // Done to place the fertilizer anywhere in the scene
            if(saveObj.name != "Fertilizer Cube"){
                // ensure raycast is hitting floor
                if(hit.collider.gameObject.layer == floorLayer) {
                saveObj.transform.position = new Vector3(hit.point.x, saveObj.transform.position.y, hit.point.z);
                saveObj.SetActive(true);
                saveObj = null;
                }
            }
            else{
                saveObj.transform.position = new Vector3(hit.point.x, saveObj.transform.position.y, hit.point.z);
                saveObj.SetActive(true);
                saveObj = null;
            }
        }
    }

    public bool SelectObject(GameObject obj) {
        if(saveObj == null) {
            saveObj = obj;
            obj.SetActive(false);
            wait = true;
            return true;
        }
        return false;
    }
}
