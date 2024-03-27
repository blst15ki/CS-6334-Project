using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenuObj;
    [SerializeField] GameObject button0, button1;
    Image image0, image1;
    string XInput, AInput, OKInput;
    int button;

    // Start is called before the first frame update
    void Start()
    {
        image0 = button0.GetComponent<Image>();
        image1 = button1.GetComponent<Image>();
        XInput = "js2";
        AInput = "js10";
        OKInput = "js0";
        button = 0;

        settingsMenuObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(OKInput)) {
            settingsMenuObj.SetActive(true);
            image0.color = Color.yellow;
            image1.color = Color.white;
        }

        if(settingsMenuObj.activeSelf) {
            if(Input.GetButtonDown(XInput)) {
                CycleButton();
            } else if(Input.GetButtonDown(AInput)) {
                SelectButton();
            }
        }
    }

    public void CycleButton() {
        button = (button + 1) % 2;

        if(button == 0) {
            image0.color = Color.yellow;
            image1.color = Color.white;
        } else if(button == 1) {
            image0.color = Color.white;
            image1.color = Color.yellow;
        }
    }
    
    public void SelectButton() {
        if(button == 0) {
            settingsMenuObj.SetActive(false);
        } else if(button == 1) {
            Application.Quit();
        }
    }
}
