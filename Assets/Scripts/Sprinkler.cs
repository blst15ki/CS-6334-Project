using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour
{
    [SerializeField] Hotbar hotbar;
    Outline outline;
    bool pointer, on;
    string AInput, XInput;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        pointer = false;
        on = false;
        AInput = "js10";
        XInput = "js2";
    }

    // Update is called once per frame
    void Update()
    {
        if(pointer) {
            if(Input.GetButtonDown(AInput)) { // select object to place
                if(hotbar.SelectObject(gameObject)) {
                    pointer = false;
                    TurnOff();
                }
            } else if(Input.GetButtonDown(XInput)) { // toggle sprinkler
                if(on) {
                    TurnOff();
                } else {
                    TurnOn();
                }
            }
        }
    }

    public void PointerOn() { pointer = true; hotbar.DisableHotbar(); }
    public void PointerOff() { pointer = false; hotbar.EnableHotbar(); }

    void TurnOn() { outline.OutlineColor = Color.cyan; on = true; }
    void TurnOff() { outline.OutlineColor = Color.white; on = false; }
}
