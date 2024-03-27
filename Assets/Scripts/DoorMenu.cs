using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorMenu : MonoBehaviour
{
    [SerializeField] GameObject doorMenuObj;
    [SerializeField] GameObject door;
    bool pointer;
    string XInput;

    // Start is called before the first frame update
    void Start()
    {
        pointer = false;
        XInput = "js2";
        doorMenuObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(pointer && Input.GetButtonDown(XInput)) {
            doorMenuObj.SetActive(true);
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }
    
    public void ButtonPressed(string button) {
        if(button.Equals("InsideButton")) {
            SceneManager.LoadScene("Inside");
        } else if(button.Equals("OutsideButton")) {
            SceneManager.LoadScene("Outdoor");
        } else if(button.Equals("CloseButton")) {
            doorMenuObj.SetActive(false);
        }
    }
    
}
