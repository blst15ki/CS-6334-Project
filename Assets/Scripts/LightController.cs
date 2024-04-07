using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] Hotbar hotbar;
    [SerializeField] GameObject dirLight;
    Outline outline;
    Light l;
    bool pointer;
    string XInput;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        l = dirLight.GetComponent<Light>();
        l.enabled = true;
        pointer = false;
        XInput = "js2";
    }

    // Update is called once per frame
    void Update()
    {
        if(pointer) {
            if(Input.GetButtonDown(XInput)) {
                l.enabled = !l.enabled;
            }
        }   
    }

    public void PointerOn() { pointer = true; hotbar.DisableHotbar(); }
    public void PointerOff() { pointer = false; hotbar.EnableHotbar(); }
}

