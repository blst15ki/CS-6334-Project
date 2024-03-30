using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SpatialTracking;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenuObj;
    [SerializeField] GameObject button0, button1;
    [SerializeField] GameObject player;
    [SerializeField] Camera mainCamera;
    CharacterMovement charMove;
    TrackedPoseDriver tpd;
    Image image0, image1;
    string XInput, AInput, OKInput;
    int button;

    // Start is called before the first frame update
    void Start()
    {
        charMove = player.GetComponent<CharacterMovement>();
        tpd = mainCamera.GetComponent<TrackedPoseDriver>();
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
        if(Input.GetButtonDown(OKInput) && settingsMenuObj.activeSelf == false) {
            // initialize settings menu
            settingsMenuObj.SetActive(true);
            image0.color = Color.yellow;
            image1.color = Color.white;

            // disable character/camera movement
            charMove.enabled = false;
            tpd.trackingType = TrackedPoseDriver.TrackingType.PositionOnly;
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

        if(button == 0) { // resume
            image0.color = Color.yellow;
            image1.color = Color.white;
        } else if(button == 1) { // quit
            image0.color = Color.white;
            image1.color = Color.yellow;
        }
    }
    
    public void SelectButton() {
        if(button == 0) { // resume
            // close settings menu and enable character/camera movement
            settingsMenuObj.SetActive(false);
            charMove.enabled = true;
            tpd.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
        } else if(button == 1) { // quit
            Application.Quit();
        }
    }
}
