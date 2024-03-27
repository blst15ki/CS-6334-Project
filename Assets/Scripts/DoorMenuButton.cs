using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMenuButton : MonoBehaviour
{
    [SerializeField] DoorMenu doorMenu;
    bool pointer;
    string AInput;
    GameObject buttonObj;
    // Start is called before the first frame update
    void Start()
    {
        pointer = false;
        AInput = "js10";
        buttonObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(pointer && Input.GetButtonDown(AInput)) {
            doorMenu.ButtonPressed(buttonObj.name);
        }
    }

    public void PointerOn(GameObject obj) { pointer = true; buttonObj = obj; }
    public void PointerOff() { pointer = false; buttonObj = null; }
}
