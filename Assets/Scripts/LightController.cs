using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] Raycast raycast;
    Outline outline;
    bool pointer, on;
    string XInput;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        // controls the main light
        GameObject.Find("Directional Light").GetComponent<Light>().enabled = false;

        pointer = false;
        on = false;
        XInput = "js2";  
    }

    // Update is called once per frame
    void Update()
    {
        if(pointer) {
            if(Input.GetButtonDown(XInput)) {
                Debug.Log(gameObject.name);
                if(on) {
                    TurnOff();
                } else {
                    TurnOn();
                }
            }
        }   
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }

    void TurnOn() { 
        outline.OutlineColor = Color.yellow; 
        on = true; 
        GameObject light = gameObject;
        light.GetComponent<Renderer>().material.color = Color.yellow;
        GameObject.Find("Directional Light").GetComponent<Light>().enabled = true;
    }
    void TurnOff() { 
        outline.OutlineColor = Color.white; 
        on = false;
        GameObject light = gameObject;
        GameObject.Find("Directional Light").GetComponent<Light>().enabled = false;
        light.GetComponent<Renderer>().material.color = Color.white;
    }
}

