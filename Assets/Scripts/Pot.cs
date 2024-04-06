using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] Raycast raycast;
    [SerializeField] WateringCan wateringCan;
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
        XInput = "js2";
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
                wateringCan.WaterOn(gameObject);
                
            }

            else if(Input.GetButtonUp(XInput)){
                wateringCan.WaterOff(gameObject);
            }
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }

    // detects fertilizer cube in the pot
    void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.tag == "Fertilizer") {
            Destroy(collider.gameObject);
            gameObject.GetComponent<Outline>().OutlineColor = Color.green;
        }
    }
}
