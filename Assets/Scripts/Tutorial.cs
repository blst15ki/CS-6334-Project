using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject TutorialObj;
    [SerializeField] Hotbar hotbar;
    [SerializeField] SettingsMenu settingsMenu;
    [SerializeField] GameObject player;
    [SerializeField] Camera mainCamera;
    CharacterMovement charMove;
    TrackedPoseDriver tpd;
    PhysicsRaycaster pRaycast;
    string AInput;


    // Start is called before the first frame update
    void Start()
    {
        charMove = player.GetComponent<CharacterMovement>();
        tpd = mainCamera.GetComponent<TrackedPoseDriver>();
        pRaycast = mainCamera.GetComponent<PhysicsRaycaster>();
        AInput = "js10";

        // disable character/camera movement
        charMove.enabled = false;
        tpd.trackingType = TrackedPoseDriver.TrackingType.PositionOnly;
        pRaycast.enabled = false;

        // disable hotbar and settings
        hotbar.DisableHotbar();
        settingsMenu.DisableSettings();

        TutorialObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf && Input.GetButtonDown(AInput)) {
            // enable character/camera movement
            charMove.enabled = true;
            tpd.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
            pRaycast.enabled = true;

            // enable hotbar and settings
            hotbar.EnableHotbar();
            settingsMenu.EnableSettings();

            TutorialObj.SetActive(false);
        }
    }
}
