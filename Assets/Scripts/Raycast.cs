using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    GameObject saveObj;
    int floorLayerMask;
    string AInput;
    bool wait; // to prevent SelectObject and PlaceObject from being called in the same frame

    // Start is called before the first frame update
    void Start()
    {
        saveObj = null;
        floorLayerMask = 1 << 7;
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
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity, floorLayerMask)) {
            saveObj.transform.position = new Vector3(hit.point.x, saveObj.transform.position.y, hit.point.z);
            saveObj.SetActive(true);
            saveObj = null;
        }
    }

    public void SelectObject(GameObject obj) {
        saveObj = obj;
        obj.SetActive(false);
        wait = true;
    }
}
