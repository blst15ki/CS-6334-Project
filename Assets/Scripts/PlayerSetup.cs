using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPun
{
    public CharacterMovement movement;
    public GameObject camera;

    void Start() {
        if (photonView.IsMine) {
            EnablePlayer();
        }
        else {
            DisablePlayer();
        }
    }

    void EnablePlayer() {
        movement.enabled = true;
        camera.SetActive(true);
    }

    void DisablePlayer() {
        movement.enabled = false;
        camera.SetActive(false);
    }
}