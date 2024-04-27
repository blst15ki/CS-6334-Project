using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] clips; // currently has 3 audio clips
    private AudioSource audioSource;
    private RaycastHit hit;
    [SerializeField] Camera mainCamera;
    private string YInput, BInput;
    private int songNum;
    [SerializeField] GameObject radio;
    [SerializeField] Hotbar hotbar;
    void Start()
    {
        audioSource = radio.GetComponent<AudioSource>();
        BInput = "js5"; //select
        YInput = "js3"; //cycle through radio
        songNum=0;
    }

    void Update()
    {
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit)) {
            if(hit.collider.gameObject.tag == "Radio"){
                hotbar.DisableHotbar();
                if(Input.GetButtonDown(BInput)) {
                    audioSource.clip = clips[songNum%3];
                    audioSource.Play();
                }
                else if (Input.GetButtonDown(YInput)){
                    audioSource.Stop();
                    songNum++;
                    audioSource.clip = clips[songNum%3];
                    audioSource.Play();
                }
            }
            else{
                hotbar.EnableHotbar();
                audioSource.Stop();
            }
        }
    }
}
