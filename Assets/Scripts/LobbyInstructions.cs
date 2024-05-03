using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyInstructions : MonoBehaviour
{
    [SerializeField] GameObject instructionsMenuObj;
    string BInput = "js5";

    void Update() {
        if(instructionsMenuObj.activeSelf && Input.GetButtonDown(BInput)) {
            instructionsMenuObj.SetActive(false);
        }
    }
}
