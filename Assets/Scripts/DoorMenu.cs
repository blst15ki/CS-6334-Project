using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMenu : MonoBehaviour
{
    [SerializeField] GameObject doorMenuObj;
    [SerializeField] GameObject door;
    [SerializeField] Camera mainCamera;
    bool pointer;
    string XInput;

    // Start is called before the first frame update
    void Start()
    {
        pointer = false;
        XInput = "js2";
        doorMenuObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(pointer && Input.GetButtonDown(XInput)) {
            Debug.Log("False");
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }
}
