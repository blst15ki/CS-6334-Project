using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [SerializeField] Hotbar hotbar;
    Outline outline;
    bool pointer;
    string AInput;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        pointer = false;
        AInput = "js10";
    }
   
    // Update is called once per frame
    void Update()
    {
        if(pointer) {
            if(Input.GetButtonDown(AInput)) { // select object to place
                if(hotbar.SelectObject(gameObject) == true) {
                    pointer = false;
                    outline.enabled = false;
                }
            }
        }
    }

    public void WaterOn(GameObject pot){
        GetComponent<AudioSource>().enabled = true;

        var canOutline = GetComponent<Outline>();
        canOutline.enabled = true;
        canOutline.OutlineColor = Color.yellow;
        canOutline.OutlineWidth = 5f;
                   
        var potOutline = pot.GetComponent<Outline>();
        potOutline.enabled = true;
        potOutline.OutlineColor = Color.blue;
        potOutline.OutlineWidth = 5f;
    }
    

    public void WaterOff(GameObject pot){
        GetComponent<Outline>().enabled = false;
        pot.GetComponent<Outline>().enabled = false;
        GetComponent<AudioSource>().enabled = false;
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }
}

