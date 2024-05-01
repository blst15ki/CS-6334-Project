using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class PlayerSetup : MonoBehaviourPun
{
    public CharacterMovement movement;
    public GameObject camera;
    private int playerNum;
    [SerializeField] TMP_Text playerText;
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
        System.Random rnd = new System.Random();
        playerNum  = rnd.Next(1, 10);
        playerText.text = "Player " + playerNum.ToString();
    }

    void DisablePlayer() {
        movement.enabled = false;
        camera.SetActive(false);
    }
}