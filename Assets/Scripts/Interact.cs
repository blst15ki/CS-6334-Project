using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Interact : MonoBehaviour
{
    public Hotbar hotbar;
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

    [PunRPC]
    void SetActiveState(string name, bool active) {
        GameObject obj = GameObject.Find(name);
        if (obj != null) {
            obj.SetActive(active);
            Debug.Log($"RPC SetActiveState: Setting {name} to {active}");
        } else {
            Debug.LogError("Object not found: " + name);
        }
    }
    // Update is called once per frame
    void Update()
    {
        // if (hotbar == null) {
        //     hotbar = FindObjectOfType<Hotbar>();
        // }
        
        if (pointer) {
            if (Input.GetButtonDown(AInput)) { // select object to place
                if (hotbar.SelectObject(gameObject) == true) {
                    pointer = false;
                    outline.enabled = false;
                }
            }
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }
}
