using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer : MonoBehaviour
{
    [SerializeField] Raycast raycast;

    Outline outline;
    bool pointer, on;
    string AInput;
    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        pointer = false;
        on = false;
        AInput = "js10";
    }

    // Update is called once per frame
    void Update()
    {
        if(pointer) {
            if(Input.GetButtonDown(AInput)) { // select object to place
                if(raycast.SelectObject(gameObject)) {
                    Debug.Log(gameObject.name);
                    pointer = false;
                    TurnOff();
                }
            } 
        }   
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }

    void TurnOn() { outline.OutlineColor = Color.cyan; on = true; }
    void TurnOff() { outline.OutlineColor = Color.white; on = false; }
    
}

