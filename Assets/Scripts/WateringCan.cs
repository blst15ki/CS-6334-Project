using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [SerializeField] Raycast raycast;
    Outline outline;
    bool pointer;
    string AInput;
    string XInput;
    GameObject potObj; 

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        pointer = false;
        AInput = "js10";
        XInput = "js2";

        potObj = GameObject.Find("Pot");
    }
   
    // Update is called once per frame
    void Update()
    {
        if(pointer) {
            if(Input.GetButtonDown(AInput)) { // select object to place
                if(raycast.SelectObject(gameObject)) {
                    pointer = false;
                    outline.enabled = false;
                }
            }

            else if(Input.GetButtonDown(XInput)){
                GameObject waterCan = gameObject;
                waterCan.GetComponent<AudioSource>().enabled = true;
                waterCan.GetComponent<Outline>().enabled = true;

                var canOutline = waterCan.GetComponent<Outline>();
                canOutline.OutlineColor = Color.yellow;
                canOutline.OutlineWidth = 5f;

                potObj.GetComponent<Outline>().enabled = true;
                   
                var potOutline = potObj.GetComponent<Outline>();
                potOutline.OutlineColor = Color.blue;
                potOutline.OutlineWidth = 5f;
                
            }

            else if(Input.GetButtonUp(XInput)){
                GameObject waterCan = gameObject;
                waterCan.GetComponent<Outline>().enabled = false;

                potObj.GetComponent<Outline>().enabled = false;
                waterCan.GetComponent<AudioSource>().enabled = false;
            }
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }
}
