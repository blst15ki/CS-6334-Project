using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SpatialTracking;
using UnityEngine.EventSystems;
using System.Linq;
public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenuObj;
    [SerializeField] GameObject button0, button1;
    [SerializeField] GameObject player;
    [SerializeField] Camera mainCamera;
    [SerializeField] Hotbar hotbar;
    CharacterMovement charMove;
    TrackedPoseDriver tpd;
    PhysicsRaycaster pRaycast;
    Image image0, image1;
    string XInput, AInput, OKInput;
    int button;
    bool enable = true;
    [SerializeField] GameObject pot;
    // Start is called before the first frame update
    void Start()
    {
        charMove = player.GetComponent<CharacterMovement>();
        tpd = mainCamera.GetComponent<TrackedPoseDriver>();
        pRaycast = mainCamera.GetComponent<PhysicsRaycaster>();
        image0 = button0.GetComponent<Image>();
        image1 = button1.GetComponent<Image>();
        XInput = "js2";
        AInput = "js10";
        OKInput = "js0";
        button = 0;

        settingsMenuObj.SetActive(false);
       
        PotData data = Save.LoadPot();
        var posPot = pot.transform.position;
        // Vector3 posPot;
        posPot.x = data.position[0];
        posPot.y = data.position[1];
        posPot.z = data.position[2];
        pot.transform.position = posPot;
    }

    // Update is called once per frame
    void Update()
    {
        if(enable) {
            if(Input.GetButtonDown(OKInput) && settingsMenuObj.activeSelf == false) {
                // initialize settings menu
                settingsMenuObj.SetActive(true);
                image0.color = Color.yellow;
                image1.color = Color.white;

                // disable character/camera movement
                charMove.enabled = false;
                tpd.trackingType = TrackedPoseDriver.TrackingType.PositionOnly;
                pRaycast.enabled = false;

                // disable hotbar
                hotbar.DisableHotbar();
            }

            // settings menu is active
            if(settingsMenuObj.activeSelf) {
                if(Input.GetButtonDown(XInput)) {
                    CycleButton();
                } else if(Input.GetButtonDown(AInput)) {
                    SelectButton();
                }
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
            // close settings menu
            settingsMenuObj.SetActive(false);

            // enable character/camera movement
            charMove.enabled = true;
            tpd.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
            pRaycast.enabled = true;

            // enable hotbar
            hotbar.EnableHotbar();
        } else if(button == 1) { // quit
            // Save.SavePot(pot);
            Application.Quit();
        }
    }

    public void EnableSettings() { enable = true; }
    public void DisableSettings() { enable = false; }
}
