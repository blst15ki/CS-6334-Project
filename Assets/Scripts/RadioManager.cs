using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] clips; // currently has 3 audio clips
    private AudioSource audioSource;
    private RaycastHit hit;
    [SerializeField] Camera mainCamera;
    private string YInput, BInput, AInput;
    private int songNum;
    [SerializeField] GameObject radio;
    [SerializeField] Hotbar hotbar;
    private bool musicEnable;
    void Start()
    {
        audioSource = radio.GetComponent<AudioSource>();
        BInput = "js5"; //select
        YInput = "js3"; //cycle through radio
        AInput = "js10"; // stop the song
        songNum=0;
        musicEnable =false;
    }

    void Update()
    {
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit)) {
            if(hit.collider.gameObject.tag == "Radio"){
                if(Input.GetButtonDown(BInput)) {
                    musicEnable=true;
                    playMusic();
                }
                else if (Input.GetButtonDown(YInput) && musicEnable){
                    hotbar.DisableHotbar();
                    audioSource.Stop();
                    songNum++;
                    audioSource.clip = clips[songNum%3];
                    audioSource.Play();
                    hotbar.EnableHotbar();
                }
                else if (Input.GetButtonDown(AInput) && musicEnable){
                    musicEnable = false;
                    audioSource.Stop();
                    radio.transform.Find("MusicParticleSystem").GetComponent<ParticleSystem>().Stop();
                }
            }
        }
    }
    void playMusic(){
        if(musicEnable){
            audioSource.clip = clips[songNum%3];
            audioSource.Play();
            radio.transform.Find("MusicParticleSystem").GetComponent<ParticleSystem>().Play();
        }
    }
}
