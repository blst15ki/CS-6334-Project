using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRotation : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject[] avatar;
    Transform avatarTransform;

    void Start() {
        avatarTransform = avatar[GameManager.Instance.avatarValue].transform;
        avatar[GameManager.Instance.avatarValue].SetActive(true);
    }
    
    void Update() {
        avatarTransform.localEulerAngles = new Vector3(avatarTransform.localEulerAngles.x, mainCamera.transform.localEulerAngles.y, avatarTransform.localEulerAngles.z);
    }
}
