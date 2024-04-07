using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] PlantInterface plantInterface;
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
                if(hotbar.SelectObject(gameObject)) {
                    pointer = false;
                    outline.enabled = false;

                    // disable plant interface if open
                    if(plantInterface.activeSelf) {
                        plantInterface.DisableInterface();
                    }
                }
            }
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }

    public bool AddFertilizer() {
        // check if plant exists
        if(transform.childCount > 0 && transform.GetChild(0).gameObject.tag == "Plant") {
            transform.GetChild(0).gameObject.GetComponent<Plant>().Fertilize();
            return true;
        }
        return false;
    }
}
