using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] Raycast raycast;
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
            if(Input.GetButtonDown(AInput)) {
                if(raycast.SelectObject(gameObject)) {
                    pointer = false;
                    outline.enabled = false;
                }
            }
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }
}
