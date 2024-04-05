using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour
{
    [SerializeField] Raycast raycast;
    Outline outline;
    bool pointer, on;
    string AInput, XInput;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        pointer = false;
        on = false;
        AInput = "js10";
        XInput = "js2";
    }

    // Update is called once per frame
    void Update()
    {
        if(pointer) {
<<<<<<< Updated upstream
            if(Input.GetButtonDown(AInput)) {
                raycast.SelectObject(gameObject);
                pointer = false;
                outline.enabled = false;
            } else if(Input.GetButtonDown(XInput)) {
                on = !on;
=======
            if(Input.GetButtonDown(AInput)) { // select object to place
            
                if(raycast.SelectObject(gameObject)) {
                    pointer = false;
                    TurnOff();
                }
            } else if(Input.GetButtonDown(XInput)) { // toggle sprinkler
>>>>>>> Stashed changes
                if(on) {
                    outline.OutlineColor = Color.cyan;
                } else {
                    outline.OutlineColor = Color.white;
                }
            }
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }
}
