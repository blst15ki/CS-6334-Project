using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] Hotbar hotbar;
    Outline outline;
    bool pointer;
    string AInput, XInput;

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
                if(hotbar.SelectObject(gameObject)) {
                    pointer = false;
                    outline.enabled = false;
                }
            }
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }

    // detects fertilizer cube in the pot
    // void OnTriggerEnter(Collider collider) {
    //     if(collider.gameObject.tag == "Fertilizer") {
    //         Destroy(collider.gameObject);
    //         gameObject.GetComponent<Outline>().OutlineColor = Color.green;
    //     }
    // }
}
